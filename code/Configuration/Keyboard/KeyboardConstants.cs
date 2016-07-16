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

		public static List<string> CombineKeysWithStandardModifiers(string[] keys)
		{
			var result = keys
				.SelectMany(key => StandardModifiers.Select(mod => key + ", " + mod).Concat(new[] { key }))
				.ToList();

			return result;
		}
	}
}