using ClashOfTanks.Core.Gameplay.Models;

namespace ClashOfTanks.Core.Gameplay
{
    public class GameplayElement
    {
        public static class Battlefield
        {
            private static double width = 500;
            private static double height = 500;

            public static double Width
            {
                get => width;
            }
            public static double Height
            {
                get => height;
            }
        }

        public object Control { get; set; }

        public double Radius { get; private set; }

        public double X { get; internal set; }
        public double Y { get; internal set; }

        private double angle;
        public double Angle
        {
            get => angle;
            internal set
            {
                angle = value % 360;
            }
        }

        internal double MoveSpeed { get; set; }
        internal double TurnSpeed { get; set; }

        protected GameplayElement(double x, double y, double angle)
        {
            Control = null;

            Radius = Tank.BaseRadius;

            X = x;
            Y = y;
            Angle = angle;

            MoveSpeed = 0;
            TurnSpeed = 0;
        }
    }
}
