using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KeyboordUsage
{
	class HeatmapPainter
	{
		private readonly GuiKeyboard keyboard;
		private readonly KeysCounter counter;

		public HeatmapPainter(GuiKeyboard keyboard, KeysCounter counter)
		{
			this.keyboard = keyboard;
			this.counter = counter;
		}

		public void Do()
		{
			var sumButtons = new Dictionary<Button, int>();
			foreach (var kv in keyboard.Button2Keydata)
			{
				var sumButton = counter.GetRecords()
					.Where(x => kv.Value.Contains(x.Key.ToString()))
					.Sum(x => x.Value);
				sumButtons.Add(kv.Key, sumButton);
			}

			var max = sumButtons.Select(x => x.Value).Max();

			foreach (Button button in keyboard.Button2Keydata.Keys)
			{
				int value;

				if (!sumButtons.TryGetValue(button, out value))
					value = 0;

				var heatMapColor = CreateHeatColor(value, max);

				var shadow = Color.FromArgb(heatMapColor.A, 
					(byte) Math.Max(0, heatMapColor.R - 40),
					(byte) Math.Max(0, heatMapColor.G - 40), 
					(byte) Math.Max(0, heatMapColor.B - 40));

				button.Background = new LinearGradientBrush()
				{
					StartPoint = new Point(0, 0),
					EndPoint = new Point(0, 1),
					GradientStops = new GradientStopCollection()
					{
						new GradientStop(heatMapColor, 0.2),
						new GradientStop(shadow,  0.85),
						new GradientStop(heatMapColor, 1),
					}
				};
			}
		}

		public Color CreateHeatColor(decimal pct)
		{
			var color = new Color();
			color.A = 255;

			if (pct < 0.05M)
				return Color.FromArgb(0xFF, 0xFD, 0xF3, 0xE5);

			if (pct < 0.34M)
				return Color.FromArgb(0xFF, (byte)(128 + (127 * Math.Min(3 * pct, 1M))), 0, 0);

			if (pct < 0.67M)
				return Color.FromArgb(0xFF, 0xFF, (byte)(255 * Math.Min(3 * (pct - 0.333333M), 1M)), 0);

			return Color.FromArgb(0xFF, (byte)(255 * Math.Min(3 * (1M - pct), 1M)), 0xFF, 0);
		}

		private Color CreateHeatColor(int value, decimal max)
		{
			if (max == 0)
				max = 1M;
			decimal pct = value/max;

			return CreateHeatColor(pct);
		}
	}
}