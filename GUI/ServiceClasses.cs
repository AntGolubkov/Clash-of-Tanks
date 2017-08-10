using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using ClashOfTanks.Core.Gameplay;
using ClashOfTanks.Core.User;
using ClashOfTanks.Core;
using System.Threading.Tasks;
using System;
using System.Collections;

namespace ClashOfTanks.GUI.Service
{
    public static class ControlProcessor
    {
        private static Panel BattlefieldPanel { get; set; }

        private static List<GUIObject> GUIObjects { get; set; }

        public static void SetupControls(Panel battlefieldPanel)
        {
            BattlefieldPanel = battlefieldPanel;
            GUIObjects = new List<GUIObject>();

            if (GameWindow.IsClient)
            {

            }
            else
            {
                GameplayProcessor.SetupGameplay();
            }
        }

        public static void UpdateControls(IEnumerable<GameplayElement> gameplayElements)
        {
            BattlefieldPanel.Children.Clear();

            if (GameWindow.IsClient)
            {
                GUIObjects.Clear();
            }

            foreach (GameplayElement gameplayElement in gameplayElements)
            {
                Control control;

                if (gameplayElement.HasControl == false || (GameWindow.IsClient && gameplayElement.HasClientControl == false))
                {
                    control = new Control();

                    switch (gameplayElement.Type)
                    {
                        case GameplayElement.Types.Tank:
                            {
                                control.Template = BattlefieldPanel.FindResource("TankControlTemplate") as ControlTemplate;
                                control.Background = ConvertColor(gameplayElement.Player.Color);
                                Panel.SetZIndex(control, 1);
                                break;
                            }
                        case GameplayElement.Types.Projectile:
                            {
                                control.Template = BattlefieldPanel.FindResource("ProjectileControlTemplate") as ControlTemplate;
                                control.Background = Brushes.Silver;
                                break;
                            }
                        case GameplayElement.Types.Explosion:
                            {
                                control.Template = BattlefieldPanel.FindResource("ExplosionControlTemplate") as ControlTemplate;
                                Panel.SetZIndex(control, 2);
                                break;
                            }
                    }

                    TransformGroup transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new ScaleTransform());
                    transformGroup.Children.Add(new RotateTransform());

                    control.RenderTransform = transformGroup;
                    control.RenderTransformOrigin = new Point(0.5, 0.5);

                    gameplayElement.HasControl = true;
                    if (GameWindow.IsClient)
                    {
                        gameplayElement.HasClientControl = true;
                    }

                    GUIObject guiObject = new GUIObject(control, gameplayElement);
                    GUIObjects.Add(guiObject);
                }
            }

            List<GUIObject> forRemove = new List<GUIObject>();
            foreach (GUIObject guiObject in GUIObjects)
            {
                if (guiObject.GameObject.IsRemoved)
                {
                    forRemove.Add(guiObject);
                }
            }
            foreach (GUIObject guiObject in forRemove)
            {
                GUIObjects.Remove(guiObject);
            }

            foreach (GUIObject guiObject in GUIObjects)
            {
                Canvas.SetLeft(guiObject.Control, guiObject.GameObject.X - GameWindow.СontrolRadius);
                Canvas.SetTop(guiObject.Control, GameplayElement.Battlefield.Height - guiObject.GameObject.Y - GameWindow.СontrolRadius); // "Минус" gameplayElement.Y из-за инверсии оси Y.

                ScaleTransform scaleTransform = (guiObject.Control.RenderTransform as TransformGroup).Children[0] as ScaleTransform;
                scaleTransform.ScaleX = guiObject.GameObject.Radius;
                scaleTransform.ScaleY = guiObject.GameObject.Radius;

                RotateTransform rotateTransform = (guiObject.Control.RenderTransform as TransformGroup).Children[1] as RotateTransform;
                rotateTransform.Angle = -guiObject.GameObject.Angle; // "Минус" gameplayElement.Angle из-за инверсии угла.

                BattlefieldPanel.Children.Add(guiObject.Control);
            }

            if (!GameWindow.IsClient)
            {
                Player winner = null;
                int remainingPlayers = GameSession.Current.Players.Count;

                foreach (Player player in GameSession.Current.Players)
                {
                    if (player.IsLoser)
                    {
                        remainingPlayers--;
                    }
                    else
                    {
                        winner = player;
                    }
                }

                if (remainingPlayers == 1)
                {
                    TextBlock resultTextBlock = ((BattlefieldPanel.Parent as FrameworkElement).Parent as Grid).Children[((BattlefieldPanel.Parent as FrameworkElement).Parent as Grid).Children.Count - 1] as TextBlock;
                    resultTextBlock.Text = $"{winner.Color} Player - WINNER!";
                    resultTextBlock.Visibility = Visibility.Visible;
                }
                else if (remainingPlayers == 0)
                {
                    TextBlock resultTextBlock = ((BattlefieldPanel.Parent as FrameworkElement).Parent as Grid).Children[((BattlefieldPanel.Parent as FrameworkElement).Parent as Grid).Children.Count - 1] as TextBlock;
                    resultTextBlock.Text = "All Players - LOSERS!";
                    resultTextBlock.Visibility = Visibility.Visible;
                }

                NetworkController.Server.SendClientData(GameSession.Current.Players, gameplayElements as object);
            }
        }

        private static Brush ConvertColor(Player.Colors color)
        {
            switch (color)
            {
                case Player.Colors.Red:
                    {
                        return Brushes.Red;
                    }
                case Player.Colors.Blue:
                    {
                        return Brushes.Blue;
                    }
                case Player.Colors.White:
                    {
                        return Brushes.White;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }

    public static class FrameProcessor
    {
        private static bool isFirstFrameUpdated = false;

        private static bool IsFirstFrameUpdated
        {
            get => isFirstFrameUpdated;
            set => isFirstFrameUpdated = value;
        }

        public static void UpdateFrame(double frameTimeInterval)
        {
            IEnumerable<GameplayElement> gameplayElements = null;

            if (GameWindow.IsClient)
            {
                if (!IsFirstFrameUpdated)
                {
                    Task.Run(() => NetworkController.Client.ListenClientData());
                    if (NetworkController.Client.Data != null)
                    {
                        ControlProcessor.UpdateControls(NetworkController.Client.Data.GameObjects as List<GameplayElement>);
                    }
                    IsFirstFrameUpdated = true;
                }
                else
                {
                    if (NetworkController.Client.Data != null)
                    {
                        ControlProcessor.UpdateControls(NetworkController.Client.Data.GameObjects as List<GameplayElement>);
                    }
                }

                NetworkController.Client.SendUserActions(NetworkController.Client.Player.Actions);
            }
            else
            {
                if (!IsFirstFrameUpdated)
                {
                    Task.Run(() => NetworkController.Server.ListenUserInput());
                    gameplayElements = GameplayProcessor.UpdateGameplay(0);
                    IsFirstFrameUpdated = true;
                }
                else
                {
                    gameplayElements = GameplayProcessor.UpdateGameplay(frameTimeInterval);
                }

                ControlProcessor.UpdateControls(gameplayElements);
            }
        }
    }

    public static class InputProcessor
    {
        public static void UpdateKeyInput(Player player, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    {
                        player.Input.KeyWPressed = e.IsDown;
                        break;
                    }
                case Key.S:
                    {
                        player.Input.KeySPressed = e.IsDown;
                        break;
                    }
                case Key.A:
                    {
                        player.Input.KeyAPressed = e.IsDown;
                        break;
                    }
                case Key.D:
                    {
                        player.Input.KeyDPressed = e.IsDown;
                        break;
                    }
                case Key.Space:
                    {
                        player.Input.KeySpacePressed = e.IsDown;
                        break;
                    }
                case Key.Up:
                    {
                        player.Input.KeyUpPressed = e.IsDown;
                        break;
                    }
                case Key.Down:
                    {
                        player.Input.KeyDownPressed = e.IsDown;
                        break;
                    }
                case Key.Left:
                    {
                        player.Input.KeyLeftPressed = e.IsDown;
                        break;
                    }
                case Key.Right:
                    {
                        player.Input.KeyRightPressed = e.IsDown;
                        break;
                    }
                case Key.Enter:
                    {
                        player.Input.KeyEnterPressed = e.IsDown;
                        break;
                    }
            }
        }
    }
}
