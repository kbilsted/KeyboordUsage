using System;
using System.Linq;
using System.Windows.Forms;

namespace KeyboordUsage
{
	public class KeyPressPainter
	{
		private string cachedKeyPopularity = "";
		private int nextRepaint = 1;
		private int repaintDela = 1;
		private GuiKeyboard keyboard;

		Func<bool> isWindowVisible;
		private readonly Action<string> updateCurrentKey;
		private readonly Action<string> updateKeyHistory;
		private readonly KeysCounter counter;
		private int keypresses = 0;

		public KeyPressPainter(Func<bool> isWindowVisible, Action<string> updateCurrentKey, Action<string> updateKeyHistory, KeysCounter counter, GuiKeyboard keyboard)
		{
			this.isWindowVisible = isWindowVisible;
			this.updateCurrentKey = updateCurrentKey;
			this.updateKeyHistory = updateKeyHistory;
			this.counter = counter;
			this.keyboard = keyboard;
		}

		public void OnStartup()
		{
			updateKeyHistory(GetKeyPopularity());

			new HeatmapPainter(keyboard, counter).Do();
		}

		public void ChangeKeyboard(GuiKeyboard newKeyboard)
		{
			keyboard = newKeyboard;
			updateKeyHistory(GetKeyPopularity());
		}

		private string GetKeyPopularity()
		{
			int no = 0;
			return string.Join("\n", counter.GetAccumulatedKeyPopularity().Select(x => (++no) + ". " + x.ToString()));
		}

		public void Paint(Keys keyData)
		{
			updateCurrentKey(keyData.ToString()); //string current = "code " + e.KeyCode + " value: " + e.KeyValue + " data: "+e.KeyData ;

			if (keypresses % 10 == 0)
			{
				cachedKeyPopularity = GetKeyPopularity();
			}

			updateKeyHistory(cachedKeyPopularity);

			if (keypresses == nextRepaint)
			{
				nextRepaint += ++repaintDela;

				new HeatmapPainter(keyboard, counter).Do();
			}

			keypresses++;
		}
	}
}
