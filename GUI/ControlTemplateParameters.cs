using System.Windows;

using ClashOfTanks.Core.Gameplay.Models;

namespace ClashOfTanks.GUI
{
    public partial class GameWindow : Window
    {
        public class TankControl
        {
            private static double baseDiameter = Tank.BaseRadius * 2;
            private static double turretDiameter = baseDiameter * 0.6;
            private static double gunLength = baseDiameter * 0.75;
            private static Thickness gunMargin = new Thickness(0, 0, -baseDiameter / 4, 0);

            public static double BaseDiameter
            {
                get => baseDiameter;
            }
            public static double TurretDiameter
            {
                get => turretDiameter;
            }
            public static double GunLength
            {
                get => gunLength;
            }
            public static Thickness GunMargin
            {
                get => gunMargin;
            }
        }
    }
}
