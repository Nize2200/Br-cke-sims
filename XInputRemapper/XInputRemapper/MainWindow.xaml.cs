using System;
using System.Windows;
using System.Windows.Threading;
using SharpDX.XInput;

namespace XInputRemapper
{
    public partial class MainWindow : Window
    {
        private ControllerHandler controllerHandler;
        private DatabaseHandler databaseHandler;

        public MainWindow()
        {
            InitializeComponent();
            controllerHandler = new ControllerHandler(UpdateUI);
            databaseHandler = new DatabaseHandler();
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
            });
        }

        private void AddToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                databaseHandler.AddToDatabase(1, "{\"test\": \"state\"}", "{\"test\": \"command\"}");
                MessageBox.Show("Value added to database successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding value to database: {ex.Message}");
            }
        }
    }
}
