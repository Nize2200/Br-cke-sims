using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SharpDX.XInput;
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace XInputRemapper
{
    public partial class MainWindow : Window
    {
        private Controller[] controllers;
        private Task[] readingTasks;
        private DatabaseHandler databaseHandler;
        private ButtonMapper buttonMapper;
        private GamepadButtonFlags buttonToRemapFrom;
        private GamepadButtonFlags buttonToRemapTo;
        private bool isRemapFromButtonPressed = false;
        private bool isRemapToButtonPressed = false;
        private DispatcherTimer connectionCheckTimer;
        private int selectedControllerIndex = -1; // Track the currently selected controller

        public ICommand ShowWindowCommand { get; }
        public ICommand ExitApplicationCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            // Create an array of controllers
            controllers = new[]
            {
                new Controller(UserIndex.One),
                new Controller(UserIndex.Two),
                new Controller(UserIndex.Three),
                new Controller(UserIndex.Four)
            };
            readingTasks = new Task[controllers.Length];
            databaseHandler = new DatabaseHandler();
            buttonMapper = new ButtonMapper();
            ShowWindowCommand = new RelayCommand(ShowWindow);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
            DataContext = this;
            StartReadingInput();

            // Initialize and start the connection check timer
            connectionCheckTimer = new DispatcherTimer();
            connectionCheckTimer.Interval = TimeSpan.FromSeconds(1);
            connectionCheckTimer.Tick += CheckControllerConnections;
            connectionCheckTimer.Start();
        }

        private void ShowWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate(); // Brings the window to the foreground
        }

        private void ExitApplication()
        {
            MyNotifyIcon.Dispose();
            Application.Current.Shutdown();
        }

        private void SaveProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                Title = "Save Remapping Profile"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                buttonMapper.SaveProfile(saveFileDialog.FileName);
                MessageBox.Show("Profile saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                Title = "Load Remapping Profile"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    buttonMapper.LoadProfile(openFileDialog.FileName);
                    MessageBox.Show("Profile loaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ApplyProfileToController(selectedControllerIndex); // Apply the loaded profile to the selected controller
                    DisplayBindingsTable(); // Update the display after loading a profile
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load profile: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DisplayBindingsTable()
        {

            BindingsTextBox.Text = buttonMapper.GenerateBindingsTable();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            MyNotifyIcon.Dispose();
            base.OnClosed(e);
        }

        private async void StartReadingInput()
        {
            var statusText = "Controller Status:\n";
            var tasks = new List<Task>();
            for (int i = 0; i < controllers.Length; i++)
            {
                var controller = controllers[i];
                if (controller.IsConnected)
                {
                    statusText += $"Controller {i}: Connected\n";
                    tasks.Add(StartReadingInput(controller, i));
                }
                else
                {
                    statusText += $"Controller {i}: Not Connected\n";
                }
            }
            ControllerStatusTextBlock.Text = statusText;
            // Start all tasks concurrently
            await Task.WhenAll(tasks);
        }

        private async Task StartReadingInput(Controller controller, int index)
        {
            var previousState = controller.GetState().Gamepad;
            while (controller.IsConnected)
            {
                var state = controller.GetState().Gamepad;
                if (!state.Equals(previousState))
                {
                    OnControllerStateChanged(state, index);
                    previousState = state;
                }
                await Task.Delay(100); // Adding a delay to avoid excessive database writes
            }
        }

        private void CheckControllerConnections(object sender, EventArgs e)
        {
            var statusText = "Controller Status:\n";
            for (int i = 0; i < controllers.Length; i++)
            {
                var controller = controllers[i];
                if (controller.IsConnected)
                {
                    statusText += $"Controller {i}: Connected\n";
                    if (readingTasks[i] == null || readingTasks[i].IsCompleted)
                    {
                        readingTasks[i] = StartReadingInput(controller, i);
                    }
                }
                else
                {
                    statusText += $"Controller {i}: Not Connected\n";
                    readingTasks[i] = null;
                }
            }
            ControllerStatusTextBlock.Text = statusText;
        }

        private void OnControllerStateChanged(Gamepad state, int controllerIndex)
        {
            if (isRemapFromButtonPressed || isRemapToButtonPressed || controllerIndex != selectedControllerIndex)
            {
                return;
            }
            try
            {
                var remappedState = buttonMapper.MapButtons(state);
                string stateJson = Newtonsoft.Json.JsonConvert.SerializeObject(remappedState);
                databaseHandler.AddToDatabase(controllerIndex, stateJson); // Only save for selected controller
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating database: {ex.Message}");
            }
        }

        private void RemapFromComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RemapFromComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var buttonName = selectedItem.Content.ToString();
                buttonToRemapFrom = ButtonMapper.GetButtonFlag(buttonName);
                RemapFromTextBlock.Text = $"Remap from: {buttonName}";
            }
        }

        private void RemapToComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RemapToComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var buttonName = selectedItem.Content.ToString();
                buttonToRemapTo = ButtonMapper.GetButtonFlag(buttonName);
                RemapToTextBlock.Text = $"Remap to: {buttonName}";
            }
        }

        private void ApplyRemapButton_Click(object sender, RoutedEventArgs e)
        {
            buttonMapper.AddRemap(buttonToRemapFrom, buttonToRemapTo);
            MessageBox.Show($"Remapped {buttonToRemapFrom} to {buttonToRemapTo}");
            // Update the remapping information display
            DisplayBindingsTable();
            var remaps = buttonMapper.GetRemaps();
        }

        private void ControllerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ControllerComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string controllerName = selectedItem.Content.ToString();
                string imagePath = string.Empty;
                switch (controllerName)
                {
                    case "Xbox Controller":
                        imagePath = "pack://application:,,,/test.jpg";
                        break;
                    case "MD4":
                        imagePath = "pack://application:,,,/megadragon_1.png"; 
                        break;
                    case "MD10":
                        imagePath = "pack://application:,,,/simpleton.jpg";
                        break;
                    default:
                        MessageBox.Show("Unknown controller selected.");
                        return;
                }
                ControllerImage.Source = new BitmapImage(new Uri(imagePath));
            }
        }

        private void ControllerIndexComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ControllerIndexComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                selectedControllerIndex = ControllerIndexComboBox.SelectedIndex; // Track selected controller index
                DisplayBindingsTable(); // Update bindings display for the selected controller
            }
        }

        private void ApplyProfileToController(int controllerIndex)
        {
            // Apply the loaded profile to the selected controller
            var controller = controllers[controllerIndex];
            var state = controller.GetState().Gamepad;
            var remappedState = buttonMapper.MapButtons(state);
            // Update the controller state with the remapped state
            // This part depends on how you want to apply the remapped state to the controller
        }
    }
}
