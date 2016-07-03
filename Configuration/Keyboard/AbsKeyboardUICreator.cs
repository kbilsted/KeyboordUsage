using System;
using System.Windows;
using System.Windows.Controls;

namespace KeyboordUsage.Configuration.Keyboard
{
	/// <summary>
	/// Helpers for different kinds of configuration formats
	/// </summary>
	public abstract class AbsKeyboardUICreator
	{
		private Style style;
		public const int NormalWidth = 24;
		public const int NormalHeight = 24;

		public AbsKeyboardUICreator(Style style)
		{
			this.style = style;
		}

		public abstract Tuple<StackPanel, GuiKeyboard> Do();

		protected void AddSpacer(StackPanel row, double width, double height)
		{
			if (width == 0)
				width = 0.1;
			if (height == 0)
				height = 0.1;

			var space = CreateButton(width, height, "", "");
			space.Visibility=Visibility.Hidden;
			row.Children.Add(space);
		}

		protected StackPanel CreateRow(StackPanel keyboard)
		{
			var row = new StackPanel()
			{
				Orientation = Orientation.Horizontal,
			};

			keyboard.Children.Add(row);

			return row;
		}

		protected void AddKey(GuiKeyboard heatmapKeys, StackPanel row, Button button, params string[] keyCodes)
		{
			foreach (var keyCode in keyCodes)
				heatmapKeys.Add(keyCode, button);

			row.Children.Add(button);
		}

		protected Button CreateButton(double width, double height, string topText, string bottomText)
		{
			var button = new Button()
			{
				Style = style,
				Content = new StackPanel()
				{
					Width = width,
					Height = height,
					Orientation = Orientation.Vertical,
					HorizontalAlignment = HorizontalAlignment.Left,
					VerticalAlignment = VerticalAlignment.Top,
					Children =
					{
						new TextBlock()
						{
							HorizontalAlignment = HorizontalAlignment.Left,
							VerticalAlignment = VerticalAlignment.Top,
							Text = topText,
						},
						new TextBlock()
						{
							HorizontalAlignment = HorizontalAlignment.Left,
							VerticalAlignment = VerticalAlignment.Bottom,
							Text = bottomText,
						}
					}
				}
			};

			return button;
		}
	}
}