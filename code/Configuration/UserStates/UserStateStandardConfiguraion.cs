using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KeyboordUsage.Configuration.Keyboard;

namespace KeyboordUsage.Configuration.UserStates
{
	static class UserStateStandardConfiguraion
	{
		public static UserState CreateDefaultState()
		{
			var stdKeyClassConfiguration = CreateStdKeyClassConfiguration();
			var recodingSession = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>(), new RatioCalculator());

			return new UserState(AppConstants.CurrentVersion, recodingSession, new List<RecodingSession>(), UserStateStandardConfiguraion.CreateGuiConfiguration(), stdKeyClassConfiguration);
		}

		public static GuiConfiguration CreateGuiConfiguration()
		{
			return new GuiConfiguration(10, 10, 1125, 550, 1);
		}

		public static KeyClassConfiguration CreateStdKeyClassConfiguration()
		{
			var destructionKeys = KeyboardConstants.CombineKeysWithStandardModifiers(new[] { "Back", "Delete" });
			var navKeys = KeyboardConstants.CombineKeysWithStandardModifiers(new[] { "Home", "PageUp", "End", "Next", "Up", "Left", "Down", "Right" });

			var metaKeys = new[]
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
	}
}
