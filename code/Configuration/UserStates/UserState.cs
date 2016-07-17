using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KeyboordUsage.Configuration.Keyboard;

namespace KeyboordUsage.Configuration.UserStates
{
	public interface IUserState
	{
		RecodingSession GetAccumulated();
		RecodingSession GetCurrentSession();
		void Clear();
		void NewSession();
		KeyClassConfiguration GetKeyClasses();
		GuiConfiguration GetGuiConfiguration();
	}


	public class UserState : IUserState
	{
		/// <summary>must be public for json serialization</summary>
		public string ConfigurationVersion;

		/// <summary>must be public for json serialization</summary>
		public RecodingSession accumulated;
		/// <summary>must be public for json serialization</summary>
		public readonly List<RecodingSession> pastSessions;

		public readonly GuiConfiguration GuiConfiguration;

		public readonly KeyClassConfiguration KeyClasses;

		RecodingSession currentSession;

		/// <summary>
		/// called from json
		/// </summary>
		public UserState(string configurationVersion, RecodingSession accumulated, List<RecodingSession> pastSessions, GuiConfiguration guiConfiguration, KeyClassConfiguration keyclasses)
		{
			ConfigurationVersion = configurationVersion;

			if (keyclasses == null)
				keyclasses = CreateStdKeyClassConfiguration();
			KeyClasses = keyclasses;

			if (accumulated == null)
				accumulated = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>(), new RatioCalculator());
			this.accumulated = accumulated;

			if (pastSessions == null)
				pastSessions = new List<RecodingSession>();
			this.pastSessions = pastSessions;

			if (guiConfiguration == null)
				guiConfiguration = CreateGuiConfiguration();
			GuiConfiguration = guiConfiguration;

			NewSession();
		}

		public static GuiConfiguration CreateGuiConfiguration()
		{
			return new GuiConfiguration(10, 10, 1125, 550, 1);
		}

		public static KeyClassConfiguration CreateStdKeyClassConfiguration()
		{
			var destructionKeys = KeyboardConstants.CombineKeysWithStandardModifiers(new [] { "Back", "Delete" });
			var navKeys = KeyboardConstants.CombineKeysWithStandardModifiers(new[] { "Home", "PageUp", "End", "Next", "Up", "Left", "Down", "Right" });

			var metaKeys = new [] 
			{
				"Escape",
				"F1",
				"F2",
				"F3",
				"F4",
				"F5",
				"F6",
				"F7",
				"F8",
				"F9",
				"F10",
				"F11",
				"F12",
				"LControlKey",
				"RLControlKey",
				"LWin",
				"LMenu",
				"RMenu",
				"Fn",
				"Apps"
			};
			var meta = KeyboardConstants.CombineKeysWithStandardModifiers(metaKeys);

			return new KeyClassConfiguration()
			{
				DestructiveKeyData = destructionKeys,
				MetaKeyData = meta,
				NaviationKeyData = navKeys,
			};
		}

		public RecodingSession GetAccumulated()
		{
			return accumulated;
		}

		public RecodingSession GetCurrentSession()
		{
			return currentSession;
		}

		public void Clear()
		{
			accumulated = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>(), new RatioCalculator());

			pastSessions.Clear();

			NewSession();
		}

		public void NewSession()
		{
			currentSession = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>(), new RatioCalculator());

			pastSessions.Add(currentSession);
		}

		public KeyClassConfiguration GetKeyClasses()
		{
			return KeyClasses;
		}

		public GuiConfiguration GetGuiConfiguration()
		{
			return GuiConfiguration;
		}
	}
}
