using System.Collections.Generic;
using System.Linq;

namespace KeyboordUsage.Configuration.Keyboard
{
	public static class KeyboardConstants
	{
		public static string[] StandardModifiers =
		{
			"Shift",
			"Control",
			"Alt",
			"Control, Alt",
			"Shift, Control",
			"Shift, Control, Alt",
			"Shift, Alt"
		};

		public static string[] CodeModifiers =
		{
			"Control",
			"Alt",
			"Control, Alt",
			"Shift, Control",
			"Shift, Control, Alt",
			"Shift, Alt"
		};

		public static string[] ControlAndShiftControlModifiers =
		{
			"Control",
			"Shift, Control",
			"Shift, Control, Alt",
		};

		public static List<string> CombineKeysWithStandardModifiers(string[] keys)
		{
			var result = keys
				.SelectMany(key => StandardModifiers.Select(mod => key + ", " + mod).Concat(new[] { key }))
				.ToList();

			return result;
		}

		public static List<string> KeysCombinedWithCodeModifiers(string[] keys)
		{
			var result = keys
				.SelectMany(key => CodeModifiers.Select(mod => key + ", " + mod))
				.ToList();

			return result;
		}

		public static List<string> KeysCombinedWithControlAndShiftControl(string[] keys)
		{
			var result = keys
				.SelectMany(key => ControlAndShiftControlModifiers.Select(mod => key + ", " + mod))
				.ToList();

			return result;
		}
	}
}