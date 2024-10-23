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

                foreach (var kvp in ButtonMap)
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

                    sb.AppendLine($" Default \"{kvp.Key}\" remapped to \"{GetButtonName(toButton)}\"");
                }


                return sb.ToString();
            }
        }
    }



