using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SharpDX.XInput;

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

        public MainWindow()
        {
            InitializeComponent();
            controllerHandler = new ControllerHandler(UpdateUI);
            databaseHandler = new DatabaseHandler();
            buttonMapper = new ButtonMapper();
            controllerHandler.StateChanged += OnControllerStateChanged;
            StartReadingInput();
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
                string stateJson = Newtonsoft.Json.JsonConvert.SerializeObject(state);
                string commandJson = "{\"command\": \"test\"}";

                databaseHandler.AddToDatabase(1, stateJson, commandJson);
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
                    MessageBox.Show($"Button to remap from: {buttonToRemapFrom}");
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
                    MessageBox.Show($"Button to remap to: {buttonToRemapTo}");
                    break;
                }
                await Task.Delay(100);
            }
        }

        private void ApplyRemapButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyButtonRemap(buttonToRemapFrom, buttonToRemapTo);
            MessageBox.Show($"Remapped {buttonToRemapFrom} to {buttonToRemapTo}");
        }

        private void ApplyButtonRemap(GamepadButtonFlags from, GamepadButtonFlags to)
        {
            controllerHandler.PauseUpdates();
            controllerHandler.StateChanged -= OnControllerStateChanged;
            controllerHandler.StateChanged += (state) =>
            {
                var remappedState = buttonMapper.MapButtons(state, from, to);
                UpdateUI(remappedState);

                try
                {
                    string stateJson = Newtonsoft.Json.JsonConvert.SerializeObject(remappedState);
                    string commandJson = "{\"command\": \"remap\"}";

                    databaseHandler.AddToDatabase(1, stateJson, commandJson);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating database: {ex.Message}");
                }
            };
            controllerHandler.ResumeUpdates();
        }
    }
}
