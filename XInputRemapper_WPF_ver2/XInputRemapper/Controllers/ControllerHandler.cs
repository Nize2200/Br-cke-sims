<<<<<<< Updated upstream
﻿using System;
using System.Threading;
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

        // Define the event
        public event Action<Gamepad> StateChanged;

        // Constructor with Action<Gamepad> parameter
        public ControllerHandler(Action<Gamepad> updateUI)
        {
            this.updateUI = updateUI;
            controller = new Controller(UserIndex.One);
            previousState = new Gamepad();
            isPaused = false;
        }

        public async Task StartReadingInput()
        {
            await Task.Run(async () =>
            {
                while (true)
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
                    //await Task.Delay(100); // Delay or we write alot of inputs to database, might need to consider doing a deadzone or something where we dont get so many joystick inputs
                }
            });
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
    }
}
=======
﻿using System;
using System.Threading;
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

        // Define the event
        public event Action<Gamepad> StateChanged;

        // Constructor with Action<Gamepad> parameter
        public ControllerHandler(Action<Gamepad> updateUI)
        {
            this.updateUI = updateUI;
            controller = new Controller(UserIndex.One);
            previousState = new Gamepad();
            isPaused = false;
        }

        public async Task StartReadingInput()
        {
            await Task.Run(async () =>
            {
                while (true)
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
                    //await Task.Delay(100); // Delay or we write alot of inputs to database, might need to consider doing a deadzone or something where we dont get so many joystick inputs
                }
            });
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
    }
}
>>>>>>> Stashed changes
