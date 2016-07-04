using System;
using System.Linq;
using System.Windows.Forms;

namespace KeyboordUsage
{
	public class KeyPressPainter
	{
		private string cachedKeyPopularity = "";
		private int nextRepaint = 10;
		private const int repaintDela = 1024;
		private GuiKeyboard keyboard;

		Func<bool> isWindowMinimized;
		private readonly Action<string> updateCurrentKey;
		private readonly Action<string> updateKeyPopularity;
		private readonly KeysCounter counter;
		private int keypresses = 0;

		public KeyPressPainter(Func<bool> isWindowMinimized, Action<string> updateCurrentKey, Action<string> updateKeyPopularity, KeysCounter counter, GuiKeyboard keyboard)
		{
			this.isWindowMinimized = isWindowMinimized;
			this.updateCurrentKey = updateCurrentKey;
			this.updateKeyPopularity = updateKeyPopularity;
			this.counter = counter;
			this.keyboard = keyboard;
		}

		public void ForceRepaint()
		{
			updateKeyPopularity(GetKeyPopularity());

			new HeatmapPainter(keyboard, counter).Do();
		}

		public void ChangeKeyboard(GuiKeyboard newKeyboard)
		{
			keyboard = newKeyboard;
			updateKeyPopularity(GetKeyPopularity());
		}

		private string GetKeyPopularity()
		{
			int no = 0;
			return string.Join("\n", counter.GetAccumulatedKeyPopularity().Select(x => (++no) + ". " + x.ToString()));
		}

		public void Paint(Keys keyData)
		{
			if (RepaintAlmostOnlyWhenVisible())
			{
				updateCurrentKey(keyData.ToString()); //string current = "code " + e.KeyCode + " value: " + e.KeyValue + " data: "+e.KeyData ;

				if (keypresses % 2 == 0)
					cachedKeyPopularity = GetKeyPopularity();

				updateKeyPopularity(cachedKeyPopularity);
				new HeatmapPainter(keyboard, counter).Do();
			}
			
			keypresses++;
		}

		private bool RepaintAlmostOnlyWhenVisible()
		{
			if (isWindowMinimized())
				return true;

			if (keypresses < nextRepaint)
				return false;

			nextRepaint = keypresses + repaintDela;
			return true;
		}
	}
}
