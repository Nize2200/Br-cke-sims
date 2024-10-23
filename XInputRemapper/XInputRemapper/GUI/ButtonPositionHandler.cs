using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace XInputRemapper
{
    public class ButtonPositionHandler
    {
        private Dictionary<string, Dictionary<string, Point>> buttonPositions;

        public ButtonPositionHandler()
        {
            InitializeButtonPositions();
        }

        private void InitializeButtonPositions()
        {
            buttonPositions = new Dictionary<string, Dictionary<string, Point>>
            {
                {
                    "Xbox Controller", new Dictionary<string, Point>
                    {
                        { "A", new Point(150, 200) },
                        { "B", new Point(200, 200) },
                        { "X", new Point(150, 150) },
                        { "Y", new Point(200, 150) }
                    }
                },
                {
                    "Handgrepp med finger tryckplatta", new Dictionary<string, Point>
                    {
                        { "A", new Point(745, 145) },
                        { "B", new Point(780, 130) },
                        { "X", new Point(745, 110) },
                        { "Y", new Point(780, 100) }
                    }
                },
                {
                    "Simpleton", new Dictionary<string, Point>
                    {
                        { "A", new Point(140, 190) },
                        { "B", new Point(190, 190) },
                        { "X", new Point(140, 140) },
                        { "Y", new Point(190, 140) }
                    }
                }
            };
        }

        public void UpdateButtonPositions(string controllerName, TextBlock buttonAText, TextBlock buttonBText, TextBlock buttonXText, TextBlock buttonYText)
        {
            if (controllerName != null && buttonPositions.ContainsKey(controllerName))
            {
                var positions = buttonPositions[controllerName];
                buttonAText.SetValue(Canvas.LeftProperty, positions["A"].X);
                buttonAText.SetValue(Canvas.TopProperty, positions["A"].Y);
                buttonBText.SetValue(Canvas.LeftProperty, positions["B"].X);
                buttonBText.SetValue(Canvas.TopProperty, positions["B"].Y);
                buttonXText.SetValue(Canvas.LeftProperty, positions["X"].X);
                buttonXText.SetValue(Canvas.TopProperty, positions["X"].Y);
                buttonYText.SetValue(Canvas.LeftProperty, positions["Y"].X);
                buttonYText.SetValue(Canvas.TopProperty, positions["Y"].Y);
            }
        }
    }
}
