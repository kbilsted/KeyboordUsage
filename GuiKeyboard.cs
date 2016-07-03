using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace KeyboordUsage
{
	public class GuiKeyboard
	{
		public readonly Dictionary<string, Button> KeyData2Button = new Dictionary<string, Button>();
		public readonly Dictionary<Button, List<string>> Button2Keydata= new Dictionary<Button, List<string>>();

		public void Add(string keyCode, Button button)
		{
			if (string.IsNullOrWhiteSpace(keyCode))
				return;

			if (KeyData2Button.ContainsKey(keyCode))
				throw new ArgumentException($"Keycode '{keyCode}' is already in use.");

			KeyData2Button.Add(keyCode, button);

			List<string> keycodes;
			if (!Button2Keydata.TryGetValue(button, out keycodes))
			{
				keycodes = new List<string>();
				Button2Keydata.Add(button, keycodes);
			}
			keycodes.Add(keyCode);
		}
	}
}
