using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace XInputRemapper
{
    public class ButtonMapper
    {
        private List<ButtonRemap> remaps;

        public ButtonMapper()
        {
            remaps = new List<ButtonRemap>();
        }

        // Adds a remap for the current configuration
        public void AddRemap(GamepadButtonFlags fromButton, GamepadButtonFlags toButton)
        {
            remaps.Add(new ButtonRemap { From = fromButton, To = toButton });
        }

        // Maps buttons based on the current remapping configuration
        public Gamepad MapButtons(Gamepad originalController)
        {
            var changedController = originalController;

            foreach (var remap in remaps)
            {
                if ((originalController.Buttons & remap.From) != 0)
                {
                    changedController.Buttons |= remap.To;
                    if (remap.From != remap.To)
                    {
                        changedController.Buttons &= ~remap.From;
                    }
                }
            }

            return changedController;
        }

        // Retrieves the current list of remaps
        public List<ButtonRemap> GetRemaps()
        {
            return remaps;
        }

        // Generates a binding table for the current remapping configuration
        public string GenerateBindingsTable()
        {
            var sb = new StringBuilder();

            // Sort the ButtonMap by button number
            var sortedButtonMap = ButtonMap.OrderBy(kvp => ButtonNumberMap[kvp.Key]);

            foreach (var kvp in sortedButtonMap)
            {
                var fromButton = kvp.Value;
                var toButton = fromButton;

                foreach (var remap in remaps)
                {
                    if (remap.From == fromButton)
                    {
                        toButton = remap.To;
                        break;
                    }
                }

                sb.AppendLine($"Button {ButtonNumberMap[kvp.Key]} remapped to {GetButtonName(toButton)}");
            }

            return sb.ToString();
        }

        // Saves the current remapping configuration to a JSON file
        public void SaveProfile(string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(remaps, options);
            File.WriteAllText(filePath, json);
        }

        // Loads a remapping configuration from a JSON file
        public void LoadProfile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                remaps = JsonSerializer.Deserialize<List<ButtonRemap>>(json);
            }
            else
            {
                throw new FileNotFoundException($"Profile file not found: {filePath}");
            }
        }

        private static readonly Dictionary<string, int> ButtonNumberMap = new Dictionary<string, int>
        {
            { "A", 3 },
            { "B", 4 },
            { "X", 1 },
            { "Y", 2 },
            { "DPad_Up", 6 },
            { "DPad_Down", 7 },
            { "DPad_Left", 5 },
            { "DPad_Right", 8 },
            { "Start", 9 },
            { "Back", 10 },
            { "Left_Thumb", 11 },
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
    }

    public class ButtonRemap
    {
        public GamepadButtonFlags From { get; set; }
        public GamepadButtonFlags To { get; set; }
    }
}
