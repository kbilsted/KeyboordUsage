using System;
using System.Linq;
using System.Windows.Forms;

namespace KeyboordUsage
{
	public class KeyPressController
	{
		private string cachedKeyPopularity = "";
		private GuiKeyboard keyboard;

		private readonly Func<bool> isWindowMinimized;
		private readonly Action<string> updateCurrentKey;
		private readonly Action<string> updateKeyPopularity;
		private readonly Action<double> updateProductiveRatio;
		private readonly KeysCounter counter;

		public KeyPressController(
			Func<bool> isWindowMinimized, 
			Action<string> updateCurrentKey, 
			Action<string> updateKeyPopularity, 
			Action<double> updateProductiveRatio,
			KeysCounter counter, 
			GuiKeyboard keyboard)
		{
			this.isWindowMinimized = isWindowMinimized;
			this.updateCurrentKey = updateCurrentKey;
			this.updateKeyPopularity = updateKeyPopularity;
			this.updateProductiveRatio = updateProductiveRatio;
			this.counter = counter;
			this.keyboard = keyboard;
		}

		public void ForceRepaint()
		{
			cachedKeyPopularity = GetKeyPopularity();
			updateKeyPopularity(cachedKeyPopularity);
			updateProductiveRatio(counter.GetProductiveRatio());

			new HeatmapPainter(keyboard, counter).Do();
		}

		public void ChangeKeyboard(GuiKeyboard newKeyboard)
		{
			keyboard = newKeyboard;
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

				ForceRepaint();
			}
		}

		private bool RepaintAlmostOnlyWhenVisible()
		{
			if (!isWindowMinimized())
				return true;

			return false;
		}
	}
}
