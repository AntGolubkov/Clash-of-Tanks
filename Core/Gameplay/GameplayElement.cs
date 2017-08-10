using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ClashOfTanks.Core.User;

namespace ClashOfTanks.Core.Gameplay
{
    [Serializable]
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

        public Player Player { get; private set; }

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

        internal GameplayElement LinkedGameplayElement { get; private set; }

        public bool HasControl { get; set; }
        public bool HasClientControl { get; set; }
        public bool IsRemoved { get; set; }

        protected GameplayElement(Types type, Player player, double x, double y, double radius, double angle)
        {
            Type = type;

            Player = player;

            X = x;
            Y = y;

            XIncrement = 0;
            YIncrement = 0;

            Radius = radius;
            Angle = angle;

            LinkedGameplayElement = null;
            HasControl = false;
            HasClientControl = false;
            IsRemoved = false;
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

        protected void CheckBorderCollision()
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

        protected double? CheckGameplayElementCollision(GameplayElement sourceElement, List<GameplayElement> destinationElements)
        {
            destinationElements = destinationElements.OrderBy(e => e.X - e.Radius).ToList();
            double squareCollisionLength = double.MaxValue;

            for (int i = 0; i < destinationElements.Count(); i++)
            {
                if (sourceElement.X + sourceElement.Radius < destinationElements[i].X - destinationElements[i].Radius)
                {
                    break;
                }
                else
                {
                    double deltaX = destinationElements[i].X - sourceElement.X;
                    double deltaY = destinationElements[i].Y - sourceElement.Y;
                    double squareLength = Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2);
                    double squareRadiusSum = Math.Pow(sourceElement.Radius + destinationElements[i].Radius, 2);

                    if (squareLength <= squareRadiusSum && squareRadiusSum - squareLength < squareCollisionLength)
                    {
                        sourceElement.LinkedGameplayElement = destinationElements[i];
                        squareCollisionLength = squareRadiusSum - squareLength;
                    }
                }
            }

            if (squareCollisionLength != double.MaxValue)
            {
                return Math.Sqrt(squareCollisionLength);
            }
            else
            {
                return null;
            }
        }
    }
}
