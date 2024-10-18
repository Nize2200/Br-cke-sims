using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;

namespace XInputRemapper
{
    public class ButtonMapper
    {
        private Gamepad changedController;
        private List<(GamepadButtonFlags from, GamepadButtonFlags to)> remaps;

        public ButtonMapper()
        {
            changedController = new Gamepad();
            remaps = new List<(GamepadButtonFlags from, GamepadButtonFlags to)>();
        }

        public void AddRemap(GamepadButtonFlags fromButton, GamepadButtonFlags toButton)
        {
            remaps.Add((fromButton, toButton));
        }

        public Gamepad MapButtons(Gamepad originalController)
        {
            changedController = originalController;

            foreach (var remap in remaps)
            {
                if ((originalController.Buttons & remap.from) != 0)
                {
                    changedController.Buttons |= remap.to;
                    if (remap.from != remap.to)
                    {
                        changedController.Buttons &= ~remap.from;
                    }
                }
            }

            return changedController;
        }
    }
}