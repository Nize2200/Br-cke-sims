using System;
using System.Windows;
using System.Windows.Input;
using SharpDX.XInput;
using Hardcodet.Wpf.TaskbarNotification;

namespace XInputRemapper
{
    public partial class MainWindow : Window
    {
        private ControllerHandler controllerHandler;
        private DatabaseHandler databaseHandler;
        private ButtonMapper buttonMapper;
        private GamepadButtonFlags buttonToRemapFrom;
        private GamepadButtonFlags buttonToRemapTo;
        private bool isRemapFromButtonPressed = false;
        private bool isRemapToButtonPressed = false;
        public ICommand ShowWindowCommand { get; }
        public ICommand ExitApplicationCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            controllerHandler = new ControllerHandler(UpdateUI);
            databaseHandler = new DatabaseHandler();
            buttonMapper = new ButtonMapper();
            controllerHandler.StateChanged += OnControllerStateChanged;

            ShowWindowCommand = new RelayCommand(ShowWindow);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
            DataContext = this;

            StartReadingInput();
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
            await controllerHandler.StartReadingInput();
        }

        private void UpdateUI(Gamepad gamepad)
        {
            Dispatcher.Invoke(() =>
            {
                ButtonPressTextBlock.Text = gamepad.Buttons.ToString();
                JoystickLeft.Text = $"leftJoy:\nX{gamepad.LeftThumbX}\nY{gamepad.LeftThumbY}";
                JoystickRight.Text = $"RightJoy:\nX{gamepad.RightThumbX}\nY{gamepad.RightThumbY}";
                TriggerLeft.Text = "Left Trigger: " + gamepad.LeftTrigger.ToString();
                TriggerRight.Text = "Right Trigger: " + gamepad.RightTrigger.ToString();
            });
        }

        private void OnControllerStateChanged(Gamepad state)
        {
            if (isRemapFromButtonPressed || isRemapToButtonPressed)
            {
                return;
            }

            try
            {
                var remappedState = buttonMapper.MapButtons(state);
                string stateJson = Newtonsoft.Json.JsonConvert.SerializeObject(remappedState);
                string commandJson = "{\"command\": \"test\"}";

                databaseHandler.AddToDatabase(1, stateJson, commandJson);
                UpdateUI(remappedState);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating database: {ex.Message}");
            }
        }

        private void RemapFromButton_Click(object sender, RoutedEventArgs e)
        {
            isRemapFromButtonPressed = true;
            RemapFromButton.Content = "Waiting for button press...";
            CaptureButtonPressFrom();
        }

        private void RemapToButton_Click(object sender, RoutedEventArgs e)
        {
            isRemapToButtonPressed = true;
            RemapToButton.Content = "Waiting for button press...";
            CaptureButtonPressTo();
        }

        private async void CaptureButtonPressFrom()
        {
            while (isRemapFromButtonPressed)
            {
                var state = controllerHandler.GetCurrentState();
                if (state.Buttons != GamepadButtonFlags.None)
                {
                    buttonToRemapFrom = state.Buttons;
                    isRemapFromButtonPressed = false;
                    RemapFromButton.Content = "Button to remap from";
                    RemapFromTextBlock.Text = buttonToRemapFrom.ToString();
                    break;
                }
                await Task.Delay(100);
            }
        }

        private async void CaptureButtonPressTo()
        {
            while (isRemapToButtonPressed)
            {
                var state = controllerHandler.GetCurrentState();
                if (state.Buttons != GamepadButtonFlags.None)
                {
                    buttonToRemapTo = state.Buttons;
                    isRemapToButtonPressed = false;
                    RemapToButton.Content = "Button to remap to";
                    RemapToTextBlock.Text = buttonToRemapTo.ToString();
                    break;
                }
                await Task.Delay(100);
            }
        }

        private void ApplyRemapButton_Click(object sender, RoutedEventArgs e)
        {
            buttonMapper.AddRemap(buttonToRemapFrom, buttonToRemapTo);
            MessageBox.Show($"Remapped {buttonToRemapFrom} to {buttonToRemapTo}");
        }
    }
}
