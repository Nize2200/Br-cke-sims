using System;
using System.Threading.Tasks;
using SharpDX.XInput;

namespace XInputRemapper
{
    public class ControllerHandler
    {
        private Controller controller;
        private Gamepad previousState;
        private Action<Gamepad> updateUI;
        private bool isPaused;
        private bool isReadingInput;

        // Define the event
        public event Action<Gamepad> StateChanged;

        // Constructor with Action<Gamepad> parameter and UserIndex
        public ControllerHandler(Action<Gamepad> updateUI, UserIndex userIndex)
        {
            this.updateUI = updateUI;
            controller = new Controller(userIndex);
            previousState = new Gamepad();
            isPaused = false;
            isReadingInput = false;
        }

        public async Task StartReadingInput()
        {
            isReadingInput = true;
            await Task.Run(async () =>
            {
                while (isReadingInput)
                {
                    if (!isPaused && controller.IsConnected)
                    {
                        var state = controller.GetState().Gamepad;
                        if (!state.Equals(previousState))
                        {
                            StateChanged?.Invoke(state);
                            updateUI?.Invoke(state);
                            previousState = state;
                        }
                    }
                    await Task.Delay(100); // Adding a delay to avoid excessive database writes
                }
            });
        }

        public void StopReadingInput()
        {
            isReadingInput = false;
        }

        public void PauseUpdates()
        {
            isPaused = true;
        }

        public void ResumeUpdates()
        {
            isPaused = false;
        }

        public Gamepad GetCurrentState()
        {
            if (controller.IsConnected)
            {
                return controller.GetState().Gamepad;
            }
            return new Gamepad();
        }

        public bool IsConnected()
        {
            return controller.IsConnected;
        }

        public bool IsReadingInput => isReadingInput;
    }
}
