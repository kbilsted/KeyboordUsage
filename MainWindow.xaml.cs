using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using KeyboordUsage.Configuration.Keyboard;
using KeyboordUsage.Configuration.UserState;
using Newtonsoft.Json;

namespace KeyboordUsage
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private KeyboardKListener listener;
		readonly JsonKeyboard[] keyboards;
		private readonly KeysCounter counter;
		UserState state;

		public MainWindow()
		{
			InitializeComponent();

			keyboards = GetKeyboards();

			state = UserState.LoadFromJson(GetStatePath());
			counter = new KeysCounter(state);

			KeyboardChooser.ItemsSource = keyboards.Select(x => x.Name);
			KeyboardChooser.SelectedIndex = 0;
		}

		private void Closeing(object sender, CancelEventArgs e)
		{
			try
			{
				if (File.Exists(GetStatePath()))
				{
					var tempFileName = Path.GetTempFileName();
					File.Delete(tempFileName);
					File.Move(GetStatePath(), tempFileName);
				}

				var stateJson = JsonConvert.SerializeObject(state, Formatting.Indented);
				File.WriteAllText(GetStatePath(), stateJson, Encoding.UTF8);

				listener.Closing();
			}
			catch (Exception ex)
			{
				ExceptionShower.Do(ex);
			}
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

		private static string GetStatePath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "KeyboordUsage.json");
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
				listener = new KeyboardKListener(currentSelectedHeatmap, x => CurrentKey.Content = x, x => KeyHistory.Text = x, counter);
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
