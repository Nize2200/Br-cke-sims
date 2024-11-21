using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;

namespace XInputRemapper
{
    public class ControllerHandler
    {
        private Controller controller;
        private Gamepad previousState;
        private Action<Gamepad> onStateChanged;
        private bool isPaused;

        public event Action<Gamepad> StateChanged;

        public ControllerHandler(Action<Gamepad> stateChangedCallback)
        {
            this.onStateChanged = stateChangedCallback;
            controller = new Controller(UserIndex.One);
            previousState = new Gamepad();
            isPaused = false;
        }

        public async Task StartReadingInput()
        {
            await Task.Run(async () =>
            {
                while (!isPaused)
                {
                    if (controller.IsConnected)
                    {
                        var currentState = controller.GetState().Gamepad;
                        if (!currentState.Equals(previousState))
                        {
                            onStateChanged?.Invoke(currentState);
                            previousState = currentState;
                        }
                    }
                    await Task.Delay(100); // Poll every 100 ms
                }
            });
        }

        public void PauseUpdates()
        {
            isPaused = true;
        }
    }
}