using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using ClashOfTanks.Core.Gameplay;
using ClashOfTanks.Core.User;

namespace ClashOfTanks.GUI.Service
{
    public static class ControlProcessor
    {
        public static void GenerateInitialControls(Panel battlefieldPanel)
        {
            IEnumerable<GameplayElement> gameplayElements = GameplayProcessor.GenerateInitialGameplayElements();

            foreach (GameplayElement gameplayElement in gameplayElements)
            {
                Control control = new Control()
                {
                    Template = battlefieldPanel.FindResource("TankControlTemplate") as ControlTemplate,
                    Background = Brushes.Red
                };

                gameplayElement.Control = control;
                battlefieldPanel.Children.Add(gameplayElement.Control as Control);
            }
        }
    }

    public static class FrameProcessor
    {
        public static void ProcessFrame(Panel battlefieldPanel)
        {
            IEnumerable<GameplayElement> gameplayElements = GameplayProcessor.ProcessGameplay();

            foreach (GameplayElement gameplayElement in gameplayElements)
            {
                Control control = gameplayElement.Control as Control;

                Canvas.SetLeft(control, gameplayElement.X);
                Canvas.SetTop(control, -gameplayElement.Y); // Минус из-за инверсии оси Y.

                RotateTransform rotateTransform = new RotateTransform(-gameplayElement.Angle); // Минус из-за инверсии угла.
                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(rotateTransform);
                control.RenderTransform = transformGroup;
                control.RenderTransformOrigin = new Point(0.5, 0.5);
            }
        }
    }

    public static class InputProcessor
    {
        public static void ProcessKeyInput(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    {
                        UserInput.KeyWPressed = e.IsDown;
                        break;
                    }
                case Key.S:
                    {
                        UserInput.KeySPressed = e.IsDown;
                        break;
                    }
                case Key.A:
                    {
                        UserInput.KeyAPressed = e.IsDown;
                        break;
                    }
                case Key.D:
                    {
                        UserInput.KeyDPressed = e.IsDown;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
