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
        public GameWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ControlProcessor.GenerateInitialControls(BattlefieldCanvas);
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            FrameProcessor.ProcessFrame(BattlefieldCanvas);
        }

        private void Window_KeyEvent(object sender, KeyEventArgs e)
        {
            InputProcessor.ProcessKeyInput(e);
        }
    }
}
