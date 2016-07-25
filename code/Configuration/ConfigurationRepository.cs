using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using KeyboordUsage.Configuration.Keyboard;
using KeyboordUsage.Configuration.UserStates;
using KeyboordUsage.Gui;
using Newtonsoft.Json;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace KeyboordUsage.Configuration
{
	class ConfigurationRepository
	{
		private UserStateStandardConfiguraion standardConfiguraion;

		public ConfigurationRepository(UserStateStandardConfiguraion standardConfiguraion)
		{
			this.standardConfiguraion = standardConfiguraion;
		}

		public UserState LoadUserState()
		{
			var path = GetStatePath();

			if (!File.Exists(path))
				return standardConfiguraion.CreateDefaultState();

			var json = File.ReadAllText(path);
			var state = JsonConvert.DeserializeObject<UserState>(json);

			var invalidJson = state == null;
			if (invalidJson)
			{
				MessageBox.Show(null, "Configuraiton file is invalid. Resetting the settings. A backup of the old configuration file is made in your temp-folder.", "Load problems", MessageBoxButtons.OK);
				BackupOldState();
				return standardConfiguraion.CreateDefaultState();
			}

			if (!AppConstants.CompatibleFileFormatVersions.Contains(state.ConfigurationVersion))
			{
				var answer = MessageBox.Show(
					null,
					"The existing configuration file is incompatible with the new version. Reset the settings? A backup of the old configuration file is made in your temp-folder", 
					"Upgrade problem",
					MessageBoxButtons.OKCancel);

				if (answer == DialogResult.Cancel)
				{
					Application.Exit();
					throw new Exception("User aborted.");
				}

				BackupOldState();
				return standardConfiguraion.CreateDefaultState();
			}

			return state;
		}

		public void BackupOldAndStoreNewUserState(UserState state)
		{
			BackupOldState();
			StoreUserState(state);
		}

		public void StoreUserState(UserState state)
		{
			var stateJson = JsonConvert.SerializeObject(state, Formatting.Indented);
			File.WriteAllText(GetStatePath(), stateJson, Encoding.UTF8);
		}

		public void BackupOldState()
		{
			if (File.Exists(GetStatePath()))
			{
				try
				{
					var backupPath = Path.Combine(Path.GetTempPath(), "KeyboordUsage." + Guid.NewGuid() + ".bak");
					File.Move(GetStatePath(), backupPath);
				}
				catch (Exception e)
				{
					ExceptionShower.Do(e, null);
				}
			}
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
