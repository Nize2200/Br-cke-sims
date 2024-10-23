using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SharpDX.XInput;
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Input;

namespace XInputRemapper
{
    public partial class MainWindow : Window
    {
        private ControllerHandler controllerHandler;
        private DatabaseHandler databaseHandler;
        private ButtonMapper buttonMapper;
        private ButtonPositionHandler buttonPositionHandler;
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
            buttonPositionHandler = new ButtonPositionHandler();
            controllerHandler.StateChanged += OnControllerStateChanged;

            ShowWindowCommand = new RelayCommand(ShowWindow);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
            DataContext = this;
            DisplayBindingsTable();

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
            await controllerHandler.StartReadingInput();
        }

        private void UpdateUI(Gamepad gamepad)
        {
            
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

                databaseHandler.AddToDatabase(1, stateJson);
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
            // Hide other button labels as needed

            switch (selectedController)
            {
                case "Xbox Controller":
                    ControllerImage.Source = new BitmapImage(new Uri("pack://application:,,,/chat_bubble_message_contact_icon_264232.ico"));
                    buttonPositionHandler.UpdateButtonPositions("Xbox Controller", ButtonA, ButtonB, ButtonX, ButtonY);
                    ButtonA.Visibility = Visibility.Visible;
                    ButtonB.Visibility = Visibility.Visible;
                    ButtonX.Visibility = Visibility.Visible;
                    ButtonY.Visibility = Visibility.Visible;
                    break;
                case "Handgrepp med finger tryckplatta":
                    ControllerImage.Source = new BitmapImage(new Uri("pack://application:,,,/XInputRemapper;component/blade.jpg"));
                    buttonPositionHandler.UpdateButtonPositions("Handgrepp med finger tryckplatta", ButtonA, ButtonB, ButtonX, ButtonY);
                    ButtonA.Visibility = Visibility.Visible;
                    ButtonB.Visibility = Visibility.Visible;
                    ButtonX.Visibility = Visibility.Visible;
                    ButtonY.Visibility = Visibility.Visible;
                    break;
                case "Simpleton":
                    ControllerImage.Source = new BitmapImage(new Uri("pack://application:,,,/simpleton.jpg"));
                    buttonPositionHandler.UpdateButtonPositions("Simpleton", ButtonA, ButtonB, ButtonX, ButtonY);
                    ButtonA.Visibility = Visibility.Visible;
                    ButtonB.Visibility = Visibility.Visible;
                    ButtonX.Visibility = Visibility.Visible;
                    ButtonY.Visibility = Visibility.Visible;
                    break;
                default:
                    ControllerImage.Source = null; // Clear the image if no controller is selected
                    break;
            }
        }

    }
}
