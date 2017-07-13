using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using ClashOfTanks.Core.Gameplay.Models;

namespace ClashOfTanks.GUI
{
    public partial class GameWindow : Window
    {
        private static readonly double controlRadius = 1; // Радиус единичной длины.

        public static double СontrolRadius
        {
            get => controlRadius;
        }

        public static class TankControl
        {
            private static readonly double diameter = СontrolRadius * 2;
            private static readonly double turretDiameter = Diameter * 0.6;
            private static readonly double gunDiameter = Tank.GunRadiusToTankRadius * Diameter;
            private static readonly double gunLength = Tank.GunLengthToTankRadius * Diameter / 2;
            private static readonly Thickness gunMargin = new Thickness(0, 0, Diameter / 2 - GunLength, 0);

            public static double Diameter
            {
                get => diameter;
            }
            public static double TurretDiameter
            {
                get => turretDiameter;
            }
            public static double GunDiameter
            {
                get => gunDiameter;
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

        public static class ProjectileControl
        {
            private static readonly double diameter = СontrolRadius * 2;
            private static readonly double length = Diameter * 2;
            private static readonly Thickness margin = new Thickness(Diameter / 2 - Length, 0, 0, 0);

            public static double Diameter
            {
                get => diameter;
            }
            public static double Length
            {
                get => length;
            }
            public static Thickness Margin
            {
                get => margin;
            }
        }

        public static class ExplosionControl
        {
            private static readonly double diameter = СontrolRadius * 2;

            public static double Diameter
            {
                get => diameter;
            }
        }
    }

    public sealed class DescalingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FrameworkElement control = value as FrameworkElement;
            ScaleTransform transform = (control.RenderTransform as TransformGroup).Children[0] as ScaleTransform;

            return 1 / transform.ScaleX;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
