using System;

namespace ClashOfTanks.Core.Gameplay
{
    public class GameplayElement
    {
        public enum Types { Tank, Projectile, Explosion }

        public static class Battlefield
        {
            private static readonly double width = 500;
            private static readonly double height = 500;

            public static double Width
            {
                get => width;
            }
            public static double Height
            {
                get => height;
            }
        }

        private double angle;

        public Types Type { get; private set; }
        public object Control { get; set; }

        public double X { get; protected set; }
        public double Y { get; protected set; }

        protected double XIncrement { get; private set; }
        protected double YIncrement { get; private set; }

        public double Radius { get; protected set; }
        public double Angle
        {
            get => angle;
            private set => angle = value % 360;
        }

        protected GameplayElement(Types type, double x, double y, double radius, double angle)
        {
            Type = type;
            Control = null;

            X = x;
            Y = y;

            XIncrement = 0;
            YIncrement = 0;

            Radius = radius;
            Angle = angle;
        }

        protected void UpdatePosition(double moveSpeed, double turnSpeed, double timeInterval)
        {
            double turnSpeedInRadians = ToRadians(turnSpeed);
            double halfTurn = turnSpeedInRadians * timeInterval / 2;
            double move = turnSpeedInRadians != 0 ? 2 * moveSpeed / turnSpeedInRadians * Math.Sin(halfTurn) : moveSpeed * timeInterval;

            double angleInRadians = ToRadians(Angle);
            XIncrement = move * Math.Cos(angleInRadians + halfTurn);
            YIncrement = move * Math.Sin(angleInRadians + halfTurn);

            X += XIncrement;
            Y += YIncrement;
            Angle += turnSpeed * timeInterval;
        }

        private double ToRadians(double degrees) => degrees * Math.PI / 180;

        protected void CheckCollision()
        {
            if (X < Radius)
            {
                X = Radius;
            }
            else if (X > Battlefield.Width - Radius)
            {
                X = Battlefield.Width - Radius;
            }

            if (Y < Radius)
            {
                Y = Radius;
            }
            else if (Y > Battlefield.Height - Radius)
            {
                Y = Battlefield.Height - Radius;
            }
        }
    }
}
