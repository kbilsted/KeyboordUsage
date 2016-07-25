using System;
using System.Windows;

namespace KeyboordUsage.Gui
{
	static class ExceptionShower
	{
		public static void Do(Exception ex, Window window = null)
		{
			var text = $"{ex.GetType()}\n{ex.Message}";

			if (window == null)
				MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			else
				MessageBox.Show(window, text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}