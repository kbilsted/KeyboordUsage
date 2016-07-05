using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using KeyboordUsage.Configuration;
using KeyboordUsage.Configuration.Keyboard;
using KeyboordUsage.Configuration.UserStates;
using Newtonsoft.Json;

namespace KeyboordUsage
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		KeyboardListener listener;
		readonly JsonKeyboard[] keyboards;
		readonly KeysCounter counter;
		UserState state;
		readonly FileHandler fileHandler = new FileHandler(); 
		KeyPressController keyPressController;

		public MainWindow()
		{
			InitializeComponent();

			var style = (Style)FindResource("InformButton");
			keyboards = fileHandler.GetKeyboards(style);

			state = fileHandler.LoadUserState();
			counter = new KeysCounter(state);

			KeyboardChooser.ItemsSource = keyboards.Select(x => x.Name);
			KeyboardChooser.SelectedIndex = 0;
		}

		private void Closeing(object sender, CancelEventArgs e)
		{
			try
			{
				fileHandler.StoreUserState(state);
				listener.Closing();
			}
			catch (Exception ex)
			{
				ExceptionShower.Do(ex);
			}
		}


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

			var currentSelectedHeatmap = result.Item2;
			Keyboard.Children.Clear();
			Keyboard.Children.Add(result.Item1);

			if (listener == null)
			{
				keyPressController = new KeyPressController(
					() => WindowState == WindowState.Minimized, 
					x => CurrentKey.Content = x,
					x => KeyHistory.Text = x, 
					counter, 
					currentSelectedHeatmap);

				keyPressController.ForceRepaint();

				listener = new KeyboardListener(counter, keyPressController);
				listener.Subscribe();
			}
			else
			{
				keyPressController.ChangeKeyboard(currentSelectedHeatmap);
				keyPressController.ForceRepaint();
			}
		}

		private void MenuItem_ClearStatistics(object sender, RoutedEventArgs e)
		{
			var answer = MessageBox.Show(this, "Are you sure you want to loose all changes?", "Clear recorded statistics",
				MessageBoxButton.YesNo);

			if (answer == MessageBoxResult.Yes)
			{
				listener.Counter.Clear();
				ChangeKeyboard(KeyboardChooser.SelectedIndex);
			}
		}

		private void MenuItem_About(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(this, @"Listen to your keyboooord!
A simple keyboard usage monitor that respects your privacy! 

You can minimize CPU usage by minimizing the window when you are not looking at it.

You can easily define your own keyboard layouts and share them on GitHub. Just take outset in the *.json files accompanying the .exe file

Backups of the keypresses are made on every program exit to your temp folder, often 'C:\Users\XXX\AppData\Local\Temp'
Made by Kasper B. Graversen 2016- ", "About...", MessageBoxButton.OK);
		}

		private void MenuItem_Exit(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void MainWindow_OnWindowStateChanged(object sender, EventArgs e)
		{
			if (keyPressController != null)
			{
				keyPressController.ForceRepaint();
			}
		}
	}
}
