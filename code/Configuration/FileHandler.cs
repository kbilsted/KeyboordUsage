﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using KeyboordUsage.Configuration.Keyboard;
using KeyboordUsage.Configuration.UserStates;
using Newtonsoft.Json;

namespace KeyboordUsage.Configuration
{
	class FileHandler
	{
		public UserState LoadUserState()
		{
			var path = GetStatePath();
			if (File.Exists(path))
			{
				var json = File.ReadAllText(path);
				var state = JsonConvert.DeserializeObject<UserState>(json);

				var invalidJson = state == null;
				if (invalidJson)
				{
					state = CreateDefaultState();
				}

				return state;
			}

			return CreateDefaultState();
		}

		private static UserState CreateDefaultState()
		{
			return new UserState(new RecodingSession(DateTime.Now, new Dictionary<Keys, int>()), new List<RecodingSession>(), UserState.CreateGuiConfiguration());
		}

		public void StoreUserState(UserState state)
		{
			if (File.Exists(GetStatePath()))
			{
				var tempFileName = Path.GetTempFileName();
				File.Delete(tempFileName);
				File.Move(GetStatePath(), tempFileName+".KeyboordUsage.bak");
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