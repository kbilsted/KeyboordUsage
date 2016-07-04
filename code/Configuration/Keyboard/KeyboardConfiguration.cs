using System;
using System.Collections.Generic;
using System.Linq;

namespace KeyboordUsage.Configuration.Keyboard
{
	public class KeyboardConfiguration
	{
		public string KeyboardName;
		public	List<Row> Rows = new List<Row>();
		public string Filename;

		public void Validate()
		{
			if(string.IsNullOrEmpty(KeyboardName))
				throw new ArgumentException("'Keyboardname' is missing from configuration");

			try
			{
				foreach (var row in Rows)
				{
					row.Validate();
				}
			}
			catch (Exception e)
			{
				throw new ArgumentException("Filename: " + Filename + " KeyboardName:"+ KeyboardName + e.Message, e);
			}
		}
	}

	public class Row
	{
		public List<Key> Keys = new List<Key>();
		public bool IsVertialSpacer;
		public double Height;

		public void Validate()
		{
			if (IsVertialSpacer)
			{
				if(Height == 0)
					throw new ArgumentException("Spacer must specify a 'height'");

				if(Keys.Any())
					throw new ArgumentException("Spacer rows may not contain elements");

				return;
			}

			foreach (var key in Keys)
				key.Validate();
		}
	}

	public class Key
	{
		public string Label1 = "", Label2="", Label3="", Label4="";
		public string KeyCode;
		public double Width = 1;
		public bool HorizontalSpace = false;

		public void Validate()
		{
			if (HorizontalSpace)
			{
				if (KeyCode != null )
					throw new ArgumentException("KeyCode cannot have a value when you define a spacer");

				return;
			}

			if (string.IsNullOrWhiteSpace(Label1)
				&& string.IsNullOrWhiteSpace(Label2)
				&& string.IsNullOrWhiteSpace(Label3)
				&& string.IsNullOrWhiteSpace(Label4))
				throw new ArgumentException("Label1-4 must have a value when you define a key");

			if (string.IsNullOrWhiteSpace(KeyCode) )
		 		throw new ArgumentException("KeyCode must have a value when you define a key");
		}
	}
}