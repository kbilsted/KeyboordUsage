using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace KeyboordUsage.Configuration.Keyboard
{
	public class JsonKeyboard : AbsKeyboardUICreator
	{
		readonly KeyboardConfiguration config;
		public readonly string Name;

		public JsonKeyboard(Style style, string json, string jsonFilename) : base(style)
		{
			config = JsonConvert.DeserializeObject<KeyboardConfiguration>(json);
			config.Filename = jsonFilename;
			config.Validate();
			Name = config.KeyboardName;
		}

		public override Tuple<StackPanel, GuiKeyboard> CreateWpfKeys()
		{
			var heatmap = new GuiKeyboard();

			var keyboard = new StackPanel()
			{
				Orientation = Orientation.Vertical
			};

			foreach (var configRow in config.Rows)
			{
				var uiRow = CreateRow(keyboard);
				if (configRow.IsVertialSpacer)
				{
					AddSpacer(uiRow, 0, configRow.Height*NormalHeight);
					continue;
				}

				foreach (var key in configRow.Keys)
				{
					if (key.HorizontalSpace)
					{
						AddSpacer(uiRow, key.Width*NormalWidth, 0);
					}
					else
					{
						var button = CreateButton(key.Width*NormalWidth, NormalHeight, key.Label1, key.Label2, key.Label3, key.Label4);

						var keyCodes = stdcodes.Select(x => key.KeyCode + ", " + x).Concat(new[] { key.KeyCode }).ToArray();
						AddKey(heatmap, uiRow, button, keyCodes );
					}
				}
			}

			return Tuple.Create(keyboard, heatmap);
		}

		string[] stdcodes =
		{
			"Shift",
			"Control",
			"Alt",
			"Control, Alt",
			"Shift, Control",
			"Shift, Control, Alt",
			"Shift, Alt"
		};
	}
}