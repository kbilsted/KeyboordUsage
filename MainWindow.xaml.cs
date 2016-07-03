using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using KeyboordUsage.Configuration.Keyboard;

namespace KeyboordUsage
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private KeyboardKListener listener;
		readonly JsonKeyboard[] keyboards;

		public MainWindow()
		{
			InitializeComponent();

			keyboards = GetKeyboards();
			KeyboardChooser.ItemsSource = keyboards.Select(x => x.Name);
			KeyboardChooser.SelectedIndex = 0;
		}

		private void Closeing(object sender, CancelEventArgs e)
		{
			listener.Closing();
		}

		JsonKeyboard[] GetKeyboards()
		{
			var style = (Style)FindResource("InformButton");

			return 
			new DirectoryInfo(GetConfigPath()).EnumerateFiles("*.json")
				.Select(x => new { FileName=x, Content= File.ReadAllText(x.FullName)})
				.Select(x => new JsonKeyboard(style, x.Content, x.FileName.FullName))
				.ToArray();
		}

		private static string GetConfigPath()
		{
			var pathOfExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var userStatePath = Path.Combine(pathOfExe, "Configuration", "Keyboard");
			return userStatePath;
		}

		private GuiKeyboard currentSelectedHeatmap;

		private void KeyboardChooser_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				var selectedIndex = (sender as ComboBox).SelectedIndex;
				ChangeKeyboard(selectedIndex);
			}
			catch (Exception ex)
			{
				ExceptionShower.Do(ex, this);
			}
		}

		private void ChangeKeyboard(int selectedIndex)
		{
			var newKeyboard = keyboards[selectedIndex];

			var result = newKeyboard.Do();
			currentSelectedHeatmap = result.Item2;
			Keyboard.Children.Clear();
			Keyboard.Children.Add(result.Item1);

			if (listener == null)
			{
				var pp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "KeyboordUsage.json");
				listener = new KeyboardKListener(currentSelectedHeatmap, pp, x => CurrentKey.Content = x, x => KeyHistory.Text = x);
				listener.Subscribe();
			}
			else
			{
				listener.ChangeKeyboard(currentSelectedHeatmap);
			}
		}

		private void MenuItem_ClearStatistics(object sender, RoutedEventArgs e)
		{
			listener.Counter.Clear();
			ChangeKeyboard(KeyboardChooser.SelectedIndex);
		}

		private void MenuItem_About(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(this, @"Listen to your keyboooord!
A simple keyboard usage monitor that respects your privacy!

You can easily define your own keyboard layouts and share them on GitHub. Just take outset in the *.json files accompanying the .exe file
 
Made by Kasper B. Graversen 2016- ", "About...", MessageBoxButton.OK);
		}

		private void MenuItem_Exit(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
