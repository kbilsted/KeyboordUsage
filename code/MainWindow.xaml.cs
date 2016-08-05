using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KeyboordUsage.Configuration;
using KeyboordUsage.Configuration.Keyboard;
using KeyboordUsage.Configuration.UserStates;
using KeyboordUsage.Gui;

namespace KeyboordUsage
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		readonly KeyboardListener listener;
		readonly JsonKeyboard[] keyboards;
		readonly UserState state;
		readonly ConfigurationRepository configurationRepository; 
		readonly KeyPressController keyPressController;

		public MainWindow()
		{
			InitializeComponent();

			Title = $"Keyboord usage v{AppConstants.CurrentVersion} by Kasper B. Graversen";

			var args = new CommandLineParser().ParseCommandLine();

			var style = (Style)FindResource("InformButton");

			configurationRepository = new ConfigurationRepository(new UserStateStandardConfiguraion(args));

			keyboards = configurationRepository.GetKeyboards(style);

			state = configurationRepository.LoadUserState();
			var userState = new DailySessionDecorator(state);
			var counter = new KeysCounter(userState);

			keyPressController = new KeyPressController(
				() => WindowState == WindowState.Minimized,
				x => CurrentKey.Content = x,
				x => KeyHistory.Text = x,
				x => ProductiveRatio.Content = x.ToString() + "%",
				x => DestructiveRatio.Content = x.ToString() + "%",
				x => NavigationRatio.Content = x.ToString() + "%",
				x => MetaRatio.Content = x.ToString() + "%",
				counter,
				null);

			listener = new KeyboardListener(counter, keyPressController);
			listener.Subscribe();

			ResizeWindow(state);

			KeyboardChooser.ItemsSource = keyboards.Select(x => x.Name);
			KeyboardChooser.SelectedIndex = state.GetGuiConfiguration().SelectedKeyboardIndex;

			keyPressController.ForceRepaint();
		}

		private void ResizeWindow(IUserState state)
		{
			var guiConfiguration = state.GetGuiConfiguration();

			this.Width = guiConfiguration.Width;
			this.Height = guiConfiguration.Height;
			this.Top = guiConfiguration.Y;
			this.Left = guiConfiguration.X;
		}

		private void Closeing(object sender, CancelEventArgs e)
		{
			try
			{
				configurationRepository.BackupOldAndStoreNewUserState(state);
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
			// can happen eg when using the app, closing, deleting keyboard.json files and restarting
			if (selectedIndex < 0 || selectedIndex > keyboards.Length - 1)
			{
				MessageBox.Show(this, "Invalid keyboard selection. Using the first in the list", "error", MessageBoxButton.OK);
				selectedIndex = 0;
			}

			var result = keyboards[selectedIndex].CreateWpfKeys();

			Keyboard.Children.Clear();
			Keyboard.Children.Add(result.Item1);

			keyPressController.ChangeKeyboard(result.Item2);
			keyPressController.ForceRepaint();

			state.GuiConfiguration.SelectedKeyboardIndex = selectedIndex;
		}

		private void MenuItem_ClearStatistics(object sender, RoutedEventArgs e)
		{
			var answer = MessageBox.Show(this, "Are you sure you want to clear typing statistics? A backup is made in your temp folder. Keyclass settings are untouched.", "Clear recorded statistics", MessageBoxButton.YesNo);

			if (answer == MessageBoxResult.Yes)
			{
				configurationRepository.StoreUserState(state);
				configurationRepository.BackupOldState();

				listener.Counter.Clear();
				keyPressController.ForceRepaint();
			}
		}

		private void MenuItem_About(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(this, @"Listen to your keyboooord!
A simple keyboard usage monitor that respects your privacy! 

You can minimize CPU usage by minimizing the window when you are not looking at it.

You can easily define your own keyboard layouts and share them on GitHub. Just take outset in the *.json files accompanying the .exe file (the configuration folder),

Keypresses are stored in 'C:\Users\XXX\KeyboordUsage.json'. Backups are made on every program exit to the temp folder, typically 'C:\Users\XXX\AppData\Local\Temp'

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

		private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			state.GuiConfiguration.Height = e.NewSize.Height;
			state.GuiConfiguration.Width = e.NewSize.Width;
		}

		private void MainWindow_OnLocationChanged(object sender, EventArgs e)
		{
			state.GuiConfiguration.X = this.Left;
			state.GuiConfiguration.Y = this.Top;
		}
	}
}
