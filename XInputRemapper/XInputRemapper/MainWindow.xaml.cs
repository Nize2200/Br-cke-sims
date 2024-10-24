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

namespace XInputRemapper
{
    public partial class MainWindow : Window
    {
        private List<ControllerHandler> controllerHandlers;
        private DatabaseHandler databaseHandler;
        private ButtonMapper buttonMapper;
        private ButtonPositionHandler buttonPositionHandler;
        private GamepadButtonFlags buttonToRemapFrom;
        private GamepadButtonFlags buttonToRemapTo;
        private bool isRemapFromButtonPressed = false;
        private bool isRemapToButtonPressed = false;
        private DispatcherTimer connectionCheckTimer;
        public ICommand ShowWindowCommand { get; }
        public ICommand ExitApplicationCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            controllerHandlers = new List<ControllerHandler>();
            for (int i = 0; i < 4; i++)
            {
                int controllerIndex = i; // Ensure the correct index is captured
                var handler = new ControllerHandler((state) => OnControllerStateChanged(state, controllerIndex), (UserIndex)i);
                controllerHandlers.Add(handler);
            }
            databaseHandler = new DatabaseHandler();
            buttonMapper = new ButtonMapper();
            buttonPositionHandler = new ButtonPositionHandler();

            ShowWindowCommand = new RelayCommand(ShowWindow);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
            DataContext = this;
            DisplayBindingsTable();

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

            for (int i = 0; i < controllerHandlers.Count; i++)
            {
                var handler = controllerHandlers[i];

                if (handler.IsConnected())
                {
                    statusText += $"Controller {i}: Connected\n";
                    tasks.Add(handler.StartReadingInput());
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

        private void CheckControllerConnections(object sender, EventArgs e)
        {
            var statusText = "Controller Status:\n";

            for (int i = 0; i < controllerHandlers.Count; i++)
            {
                var handler = controllerHandlers[i];

                if (handler.IsConnected())
                {
                    statusText += $"Controller {i}: Connected\n";
                    if (!handler.IsReadingInput)
                    {
                        handler.StartReadingInput();
                    }
                }
                else
                {
                    statusText += $"Controller {i}: Not Connected\n";
                    if (handler.IsReadingInput)
                    {
                        handler.StopReadingInput();
                    }
                }
            }

            ControllerStatusTextBlock.Text = statusText;
        }

        private void UpdateUI(Gamepad gamepad)
        {
            // Update UI based on gamepad state
        }

        private void OnControllerStateChanged(Gamepad state, int controllerIndex)
        {
            if (isRemapFromButtonPressed || isRemapToButtonPressed)
            {
                return;
            }

            try
            {
                var remappedState = buttonMapper.MapButtons(state);
                string stateJson = Newtonsoft.Json.JsonConvert.SerializeObject(remappedState);

                databaseHandler.AddToDatabase(controllerIndex, stateJson);
                UpdateUI(remappedState);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating database: {ex.Message}");
            }
        }

        private void RemapFromButton_Click(object sender, RoutedEventArgs e)
        {
            if (RemapFromComboBox.SelectedItem != null)
            {
                var buttonName = (RemapFromComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                buttonToRemapFrom = ButtonMapper.GetButtonFlag(buttonName);
                RemapFromTextBlock.Text = buttonToRemapFrom.ToString();
            }
            else
            {
                MessageBox.Show("Please select a button to remap from.");
            }
        }

        private void RemapToButton_Click(object sender, RoutedEventArgs e)
        {
            if (RemapToComboBox.SelectedItem != null)
            {
                var buttonName = (RemapToComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                buttonToRemapTo = ButtonMapper.GetButtonFlag(buttonName);
                RemapToTextBlock.Text = buttonToRemapTo.ToString();
            }
            else
            {
                MessageBox.Show("Please select a button to remap to.");
            }
        }

        private void ApplyRemapButton_Click(object sender, RoutedEventArgs e)
        {
            buttonMapper.AddRemap(buttonToRemapFrom, buttonToRemapTo);
            MessageBox.Show($"Remapped {buttonToRemapFrom} to {buttonToRemapTo}");

            // Update the remapping information display
            DisplayBindingsTable();
            var remaps = buttonMapper.GetRemaps();
            RemapInfoTextBlock.Text = string.Join("\n", remaps.Select(remap => $"{ButtonMapper.GetButtonName(remap.from)} -> {ButtonMapper.GetButtonName(remap.to)}"));
        }

        private void ControllerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedController = (ControllerComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Hide all button labels initially
            ButtonA.Visibility = Visibility.Collapsed;
            ButtonB.Visibility = Visibility.Collapsed;
            ButtonX.Visibility = Visibility.Collapsed;
            ButtonY.Visibility = Visibility.Collapsed;
            DpadUp.Visibility = Visibility.Collapsed;
            DpadDown.Visibility = Visibility.Collapsed;
            DpadLeft.Visibility = Visibility.Collapsed;
            DpadRight.Visibility = Visibility.Collapsed;

            switch (selectedController)
            {
                case "Xbox Controller":
                    ControllerImage.Source = new BitmapImage(new Uri("pack://application:,,,/chat_bubble_message_contact_icon_264232.ico"));
                    buttonPositionHandler.UpdateButtonPositions("Xbox Controller", ButtonA, ButtonB, ButtonX, ButtonY, DpadUp, DpadDown, DpadLeft, DpadRight);
                    ButtonA.Visibility = Visibility.Visible;
                    ButtonB.Visibility = Visibility.Visible;
                    ButtonX.Visibility = Visibility.Visible;
                    ButtonY.Visibility = Visibility.Visible;
                    DpadUp.Visibility = Visibility.Visible;
                    DpadDown.Visibility = Visibility.Visible;
                    DpadLeft.Visibility = Visibility.Visible;
                    DpadRight.Visibility = Visibility.Visible;
                    break;
                case "Handgrepp med finger tryckplatta":
                    ControllerImage.Source = new BitmapImage(new Uri("pack://application:,,,/XInputRemapper;component/blade.jpg"));
                    buttonPositionHandler.UpdateButtonPositions("Handgrepp med finger tryckplatta", ButtonA, ButtonB, ButtonX, ButtonY, DpadUp, DpadDown, DpadLeft, DpadRight);
                    ButtonA.Visibility = Visibility.Visible;
                    ButtonB.Visibility = Visibility.Visible;
                    ButtonX.Visibility = Visibility.Visible;
                    ButtonY.Visibility = Visibility.Visible;
                    DpadUp.Visibility = Visibility.Visible;
                    DpadDown.Visibility = Visibility.Visible;
                    DpadLeft.Visibility = Visibility.Visible;
                    DpadRight.Visibility = Visibility.Visible;
                    break;
                case "Simpleton":
                    ControllerImage.Source = new BitmapImage(new Uri("pack://application:,,,/simpleton.jpg"));
                    buttonPositionHandler.UpdateButtonPositions("Simpleton", ButtonA, ButtonB, ButtonX, ButtonY, DpadUp, DpadDown, DpadLeft, DpadRight);
                    ButtonA.Visibility = Visibility.Visible;
                    ButtonB.Visibility = Visibility.Visible;
                    ButtonX.Visibility = Visibility.Visible;
                    ButtonY.Visibility = Visibility.Visible;
                    DpadUp.Visibility = Visibility.Visible;
                    DpadDown.Visibility = Visibility.Visible;
                    DpadLeft.Visibility = Visibility.Visible;
                    DpadRight.Visibility = Visibility.Visible;
                    break;
                default:
                    ControllerImage.Source = null; // Clear the image if no controller is selected
                    break;
            }
        }
    }
}
