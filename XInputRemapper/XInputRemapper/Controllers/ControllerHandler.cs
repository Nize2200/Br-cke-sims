using System;
using System.Threading.Tasks;
using SharpDX.XInput;
using Newtonsoft.Json;

namespace XInputRemapper
{
    public class ControllerHandler
    {
        private Controller controller;
        private Action<Gamepad> updateUI;
        private Gamepad previousState;

        public ControllerHandler(Action<Gamepad> updateUI)
        {
            this.updateUI = updateUI;
            controller = new Controller(UserIndex.One);
            previousState = new Gamepad();
        }

        public async Task StartReadingInput()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (controller.IsConnected)
                    {
                        var state = controller.GetState().Gamepad;

                        if (!state.Equals(previousState))
                        {
                            var stateJson = JsonConvert.SerializeObject(state);
                            var commandJson = JsonConvert.SerializeObject(state.Buttons);

                            updateUI(state);

                            try
                            {
                                DatabaseHandler databaseHandler = new DatabaseHandler();
                                databaseHandler.AddToDatabase(0, stateJson, commandJson);
                                Console.WriteLine("Database updated successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error updating database: {ex.Message}");
                            }

                            previousState = state;
                        }
                    }

                    System.Threading.Thread.Sleep(100);
                }
            });
        }
    }
}
