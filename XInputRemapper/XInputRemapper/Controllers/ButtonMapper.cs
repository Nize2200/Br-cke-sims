using SharpDX.XInput;

namespace XInputRemapper
{
    public class ButtonMapper
    {
        private Gamepad changedController;

        public ButtonMapper()
        {
            changedController = new Gamepad();
        }

        public Gamepad MapButtons(Gamepad originalController, GamepadButtonFlags fromButton, GamepadButtonFlags toButton)
        {
            
            changedController = originalController;

            if ((originalController.Buttons & fromButton) != 0)
            {
                changedController.Buttons |= toButton;
                if (fromButton != toButton)
                {
                    changedController.Buttons &= ~fromButton;
                }
            }

            return changedController;
        }
    }
}
