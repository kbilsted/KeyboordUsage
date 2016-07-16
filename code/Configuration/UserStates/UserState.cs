using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KeyboordUsage.Configuration.Keyboard;

namespace KeyboordUsage.Configuration.UserStates
{
	public class UserState
	{
		/// <summary>must be public for json serialization</summary>
		public RecodingSession accumulated;
		/// <summary>must be public for json serialization</summary>
		public readonly List<RecodingSession> sessions; // tODO rename to PastSessions

		public readonly GuiConfiguration GuiConfiguration;

		public readonly KeyClassConfiguration KeyClasses;

		RecodingSession currentSession;

		/// <summary>
		/// called from json
		/// </summary>
		public UserState(RecodingSession accumulated, List<RecodingSession> sessions, GuiConfiguration guiConfiguration, KeyClassConfiguration keyclasses)
		{
			if (keyclasses == null)
				keyclasses = CreateStdKeyClassConfiguration();
			KeyClasses = keyclasses;

			if (accumulated == null)
				accumulated = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>(), new RatioCalculator());
			this.accumulated = accumulated;

			if (sessions == null)
				sessions = new List<RecodingSession>();
			this.sessions = sessions;

			if (guiConfiguration == null)
				guiConfiguration = CreateGuiConfiguration();
			GuiConfiguration = guiConfiguration;

			NewInstance();
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
			sessions.Clear();
			NewInstance();
		}

		private void NewInstance()
		{
			currentSession = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>(), new RatioCalculator());
			sessions.Add(currentSession);
		}
	}
}
