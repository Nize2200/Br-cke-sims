using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Text;

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

        public List<(GamepadButtonFlags from, GamepadButtonFlags to)> GetRemaps()
        {
            return remaps;
        }
   
        private static readonly Dictionary<string, int> ButtonNumberMap = new Dictionary<string, int>
        {
            { "A", 3 },
            { "B", 4 },
            { "X", 1 },
            { "Y", 2 },
            { "DPad_Up",6 },
            { "DPad_Down", 7 },
            { "DPad_Left", 5 },
            { "DPad_Right", 8 },
            { "Start", 9 },
            { "Back", 10 },
            { "Left_Thumb",11},
            { "Right_Thumb", 12 },
            { "Left_Shoulder", 13 },
            { "Right_Shoulder", 14 },
            { "None", 15 }
        };


        public static readonly Dictionary<string, GamepadButtonFlags> ButtonMap = new Dictionary<string, GamepadButtonFlags>
        {
            { "A", GamepadButtonFlags.A },
            { "B", GamepadButtonFlags.B },
            { "X", GamepadButtonFlags.X },
            { "Y", GamepadButtonFlags.Y },
            { "DPad_Up", GamepadButtonFlags.DPadUp },
            { "DPad_Down", GamepadButtonFlags.DPadDown },
            { "DPad_Left", GamepadButtonFlags.DPadLeft },
            { "DPad_Right", GamepadButtonFlags.DPadRight },
            { "Start", GamepadButtonFlags.Start },
            { "Back", GamepadButtonFlags.Back },
            { "Left_Thumb", GamepadButtonFlags.LeftThumb },
            { "Right_Thumb", GamepadButtonFlags.RightThumb },
            { "Left_Shoulder", GamepadButtonFlags.LeftShoulder },
            { "Right_Shoulder", GamepadButtonFlags.RightShoulder },
            { "None", GamepadButtonFlags.None }
        };

        public static GamepadButtonFlags GetButtonFlag(string buttonName)
        {
            if (ButtonMap.TryGetValue(buttonName, out var flag))
            {
                return flag;
            }
            throw new ArgumentException($"Button {buttonName} not found in mapping.");
        }

        public static string GetButtonName(GamepadButtonFlags buttonFlag)
        {
            foreach (var kvp in ButtonMap)
            {
                if (kvp.Value == buttonFlag)
                {
                    return kvp.Key;
                }
            }

                return buttonFlag.ToString();
        }
        public string GenerateBindingsTable()
        {
            var sb = new StringBuilder();

            // Sort the ButtonMap by the button number
            var sortedButtonMap = ButtonMap.OrderBy(kvp => ButtonNumberMap[kvp.Key]);

            foreach (var kvp in sortedButtonMap)
            {
                var fromButton = kvp.Value;
                var toButton = fromButton; // Default to the same button

                foreach (var remap in remaps)
                {
                    if (remap.from == fromButton)
                    {
                        toButton = remap.to;
                        break;
                    }
                }

                sb.AppendLine($"Button {ButtonNumberMap[kvp.Key]} remapped to {GetButtonName(toButton)}");
            }

            return sb.ToString();
        }

    }

}




