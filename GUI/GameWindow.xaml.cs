using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using ClashOfTanks.GUI.Service;
using ClashOfTanks.Core;

namespace ClashOfTanks.GUI
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public static bool IsClient { get; private set; }

        private TimeSpan LastRenderingTime { get; set; }

        public GameWindow(bool isClient)
        {
            InitializeComponent();
            IsClient = isClient;
            LastRenderingTime = TimeSpan.Zero;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ControlProcessor.SetupControls(BattlefieldCanvas);
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            TimeSpan currentRenderingTime = (e as RenderingEventArgs).RenderingTime;

            if (currentRenderingTime.Ticks == LastRenderingTime.Ticks)
            {
                return;
            }

            try
            {
                FrameProcessor.UpdateFrame((currentRenderingTime - LastRenderingTime).TotalSeconds);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.StackTrace}", $"{ex.Message}");
            }
            
            LastRenderingTime = currentRenderingTime;
        }

        private void Window_KeyEvent(object sender, KeyEventArgs e)
        {
            try
            {
                if (IsClient)
                {
                    InputProcessor.UpdateKeyInput(NetworkController.Client.Player, e);
                }
                else
                {
                    InputProcessor.UpdateKeyInput(GameSession.Current.Players[0], e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.StackTrace}", $"{ex.Message}");
            }
        }
    }
}
