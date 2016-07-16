using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using KeyboordUsage.Configuration.Keyboard;
using KeyboordUsage.Configuration.UserStates;
using Newtonsoft.Json;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace KeyboordUsage.Configuration
{
	class FileHandler
	{
		public UserState LoadUserState()
		{
			var path = GetStatePath();

			if (!File.Exists(path))
				return CreateDefaultState();

			var json = File.ReadAllText(path);
			var state = JsonConvert.DeserializeObject<UserState>(json);

			var invalidJson = state == null;
			if (invalidJson)
			{
				MessageBox.Show(null, "Configuraiton file is invalid. Resetting the settings.", "Load problems", MessageBoxButtons.OK);
				return CreateDefaultState();
			}

			if (state.ConfigurationVersion != AppConstants.CurrentVersion)
			{
				var answer = MessageBox.Show(
					null,
					"Old configuration format incompatible with the new format. Resetting the settings.", 
					"Upgrade problem",
					MessageBoxButtons.OKCancel);

				if (answer == DialogResult.Cancel)
				{
					Application.Exit();
					throw new Exception("User aborted.");
				}

				return CreateDefaultState();
			}

			return state;
		}

		private static UserState CreateDefaultState()
		{
			var stdKeyClassConfiguration = UserState.CreateStdKeyClassConfiguration();
			var recodingSession = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>(), new RatioCalculator());

			return new UserState(AppConstants.CurrentVersion, recodingSession, new List<RecodingSession>(), UserState.CreateGuiConfiguration(), stdKeyClassConfiguration);
		}

		public void StoreUserState(UserState state)
		{
			if (File.Exists(GetStatePath()))
			{
				try
				{
					var backupPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".KeyboordUsage.bak");
					File.Move(GetStatePath(), backupPath);
				}
				catch (Exception)
				{
				}
			}

			var stateJson = JsonConvert.SerializeObject(state, Formatting.Indented);
			File.WriteAllText(GetStatePath(), stateJson, Encoding.UTF8);
		}

		public JsonKeyboard[] GetKeyboards(Style style)
		{
			return
			new DirectoryInfo(GetConfigPath()).EnumerateFiles("*.json")
				.Select(x => new { FileName = x, Content = File.ReadAllText(x.FullName) })
				.Select(x =>
				{
					try
					{
						return new JsonKeyboard(style, x.Content, x.FileName.FullName);
					}
					catch (Exception e)
					{
						throw new Exception("File: "+x.FileName+ " " +e.Message, e);
					}
				})
				.ToArray();
		}

		private string GetConfigPath()
		{
			var pathOfExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var userStatePath = Path.Combine(pathOfExe, "Configuration", "Keyboard");
			return userStatePath;
		}

		private string GetStatePath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "KeyboordUsage.json");
		}
	}
}
