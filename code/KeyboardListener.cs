using System;
using Gma.System.MouseKeyHook;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace KeyboordUsage
{
	public class KeyboardListener
	{
		private IKeyboardMouseEvents globalHook;
		public readonly KeysCounter Counter;
		readonly KeyPressController keyPressController;

		public KeyboardListener(KeysCounter counter, KeyPressController keyPressController)
		{
			this.Counter = counter;
			this.keyPressController = keyPressController;
		}

		public void Subscribe()
		{
			globalHook = Hook.GlobalEvents();

			//globalHook.MouseDownExt += GlobalHookMouseDownExt;
			//globalHook.KeyDown += RecordDown;
			globalHook.KeyUp += RecordKeyUp;
		}

		//private Keys previousKeyData = Keys.None;
		private void RecordKeyUp(object sender, KeyEventArgs e)
		{
			//if(previousKeyData == e.KeyData)
			//	return;
			//previousKeyData = e.KeyData;
			// det skal kun være ctrl og shift vi skal frasortere...

			Counter.Add(e.KeyData);

			keyPressController.Paint(e.KeyData);
		}

		private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
		{
			//Console.WriteLine("MouseDown: \t{0}; \t System Timestamp: \t{1}", e.Button, e.Timestamp);

			// uncommenting the following line will suppress the middle mouse button click
			// if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
		}

		public void Closing()
		{
			//globalHook.MouseDownExt -= GlobalHookMouseDownExt;
			globalHook.KeyUp -= RecordKeyUp;
			//globalHook.KeyDown -= RecordDown;

			globalHook.Dispose();
		}
	}
}
