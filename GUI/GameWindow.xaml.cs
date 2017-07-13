using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using ClashOfTanks.GUI.Service;

namespace ClashOfTanks.GUI
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private TimeSpan LastRenderingTime { get; set; }

        public GameWindow()
        {
            InitializeComponent();
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

            FrameProcessor.UpdateFrame((currentRenderingTime - LastRenderingTime).TotalSeconds);
            LastRenderingTime = currentRenderingTime;
        }

        private void Window_KeyEvent(object sender, KeyEventArgs e) => InputProcessor.UpdateKeyInput(e);
    }
}
