using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using Button = System.Windows.Controls.Button;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace KeyboordUsage
{
	public class KeyboardKListener
	{
		private GuiKeyboard keyboard;
		private readonly Action<string> updateCurrentKey;
		private readonly Action<string> updateKeyHistory;
		private IKeyboardMouseEvents globalHook;
		public readonly KeysCounter Counter;

		public KeyboardKListener(GuiKeyboard keyboard, Action<string> updateCurrentKey, Action<string> updateKeyHistory, KeysCounter counter)
		{
			this.keyboard = keyboard;
			this.updateCurrentKey = updateCurrentKey;
			this.updateKeyHistory = updateKeyHistory;
			this.Counter = counter;
			OnStartup();

		}

		public void Subscribe()
		{
			// Note: for the application hook, use the Hook.AppEvents() instead
			globalHook = Hook.GlobalEvents();

			globalHook.MouseDownExt += GlobalHookMouseDownExt;
			//globalHook.KeyDown += RecordDown;
			globalHook.KeyUp += RecordKeyUp;
		}

		public void ChangeKeyboard(GuiKeyboard newKeyboard)
		{
			this.keyboard = newKeyboard;
			updateKeyHistory(GetKeyPopularity());
		}

		private string cachedKeyPopularity="";

		private int keypresses = 0;

		//private Keys previousKeyData = Keys.None;

		private void RecordKeyUp(object sender, KeyEventArgs e)
		{
			//if(previousKeyData == e.KeyData)
			//	return;
			//previousKeyData = e.KeyData;
			// det skal kun være ctrl og shift vi skal frasortere...

			Counter.Add(e.KeyData);

			updateCurrentKey(e.KeyData.ToString()); //string current = "code " + e.KeyCode + " value: " + e.KeyValue + " data: "+e.KeyData ;

			if (keypresses % 10 == 0)
			{
				cachedKeyPopularity = GetKeyPopularity();
			}

			updateKeyHistory(cachedKeyPopularity);

			//if (keypresses%5 == 0)
			{
				new HeatmapPainter(keyboard, Counter).Do();
			}

			keypresses++;
		}

		private string GetKeyPopularity()
		{
			int no = 0;
			return string.Join("\n", Counter.GetAccumulatedKeyPopularity().Select(x => (++no) +". "+ x.ToString()));
		}

		private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
		{
			Console.WriteLine("MouseDown: \t{0}; \t System Timestamp: \t{1}", e.Button, e.Timestamp);

			// uncommenting the following line will suppress the middle mouse button click
			// if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
		}

		public void Unsubscribe()
		{
			globalHook.MouseDownExt -= GlobalHookMouseDownExt;
			globalHook.KeyUp -= RecordKeyUp;
			//globalHook.KeyDown -= RecordDown;

			//It is recommened to dispose it
			globalHook.Dispose();
		}

		public void OnStartup()
		{
			updateKeyHistory(GetKeyPopularity());

			new HeatmapPainter(keyboard, Counter).Do();
		}

		public void Closing()
		{
			Unsubscribe();
		}
	}
}
