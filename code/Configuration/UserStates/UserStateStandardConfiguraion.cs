using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KeyboordUsage.Configuration.Keyboard;

namespace KeyboordUsage.Configuration.UserStates
{
	class UserStateStandardConfiguraion
	{
		private readonly CommandLineArgs commandLineArgs;

		public UserStateStandardConfiguraion(CommandLineArgs commandLineArgs)
		{
			this.commandLineArgs = commandLineArgs;
		}

		public UserState CreateDefaultState()
		{
			var stdKeyClassConfiguration = CreateStdKeyClassConfiguration();
			var recodingSession = new RecodingSession(DateTime.Now, new Dictionary<Keys, int>(), new RatioCalculator());

			return new UserState(AppConstants.CurrentVersion, recodingSession, new List<RecodingSession>(), CreateGuiConfiguration(), stdKeyClassConfiguration);
		}

		public GuiConfiguration CreateGuiConfiguration()
		{
			return new GuiConfiguration(10, 10, 1125, 550, 1);
		}

		public KeyClassConfiguration CreateStdKeyClassConfiguration()
		{
			return new KeyClassConfiguration()
			{
				DestructiveKeyData = DestructionKeys(),
				MetaKeyData = MetaKeys(),
				NaviationKeyData = NavKeys(),
			};
		}

		private List<string> MetaKeys()
		{
			var metaKeys = new List<string>()
			{
				"Escape",
				"F1",
				"F2",
				"F4",
				"F5",
				"F9",
				"F10",
				"F11",
				"LControlKey",
				"RLControlKey",
				"LWin",
				"LMenu",
				"RMenu",
				"Fn",
				"Apps"
			};

			if (!commandLineArgs.UseVisualStudioNavigation)
			{
				metaKeys.AddRange(new[] {"F6", "F7", "F8", "F12"});
			}
			var meta = KeyboardConstants.CombineKeysWithStandardModifiers(metaKeys.ToArray());

			return meta.OrderBy(x => x).ToList();
		}

		private List<string> NavKeys()
		{
			var navs = new[] {"Home", "PageUp", "End", "Next", "Up", "Left", "Down", "Right"};
			var navKeys = KeyboardConstants.CombineKeysWithStandardModifiers(navs);
			navKeys.Add("G, Control");
			navKeys.Add("Tab, Alt");
			navKeys.Add("Tab, Control");
			navKeys.Add("Tab, Shift, Control");
			navKeys.AddRange(KeyboardConstants.CombineKeysWithStandardModifiers(new[] { "F3" }));

			if (commandLineArgs.UseVisualStudioNavigation)
			{
				navKeys.AddRange(KeyboardConstants.CombineKeysWithStandardModifiers(new[] {"F12"}));
				navKeys.AddRange(KeyboardConstants.KeysCombinedWithCodeModifiers(new[] {"T", "F6", "F7", "F8"}));

				navKeys.AddRange(KeyboardConstants.KeysCombinedWithControlAndShiftControl(new[] {"OemMinus", "Tab", "I"}));

				navKeys.Add("A, Shift, Control, Alt");

				navKeys.Add("L, Shift, Alt");
			}

			return navKeys.OrderBy(x => x).ToList();
		}

		private static List<string> DestructionKeys()
		{
			var destructionKeys = KeyboardConstants.CombineKeysWithStandardModifiers(new[] {"Back", "Delete"});
			destructionKeys.Add("L, Control");
			destructionKeys.Add("Z, Control");
			destructionKeys.Add("X, Control");
			destructionKeys.Add("Tab, Shift");

			return destructionKeys.OrderBy(x => x).ToList();
		}
	}
}
