using System;

namespace ClashOfTanks.Core.Gameplay.Models
{
    [Serializable]
    sealed class Projectile : GameplayElement
    {
        private double MoveSpeed { get; set; }
        public double Damage { get; private set; }

        public Projectile(double x, double y, double radius, double angle, double moveSpeed, double damage) : base(Types.Projectile, null, x, y, radius, angle)
        {
            MoveSpeed = moveSpeed;
            Damage = damage;
        }

        public Projectile SetupProjectile(double offset)
        {
            Projectile projectile = this;
            UpdatePosition(offset, 0, 1);

            if (projectile.CheckBorderCollision().HasValue || projectile.CheckGameplayElementCollision().HasValue)
            {
                projectile = null;
            }

            return projectile;
        }

        public double? UpdatePosition(double timeInterval)
        {
            UpdatePosition(MoveSpeed, 0, timeInterval);

            double? borderOverflow = CheckBorderCollision();
            double? gameplayElementOverflow = CheckGameplayElementCollision();

            if (!borderOverflow.HasValue && !gameplayElementOverflow.HasValue)
            {
                return null;
            }
            else
            {
                double overflow;

                if (borderOverflow.HasValue && gameplayElementOverflow.HasValue)
                {
                    overflow = Math.Max(borderOverflow.Value, gameplayElementOverflow.Value);
                }
                else if (borderOverflow.HasValue)
                {
                    overflow = borderOverflow.Value;
                }
                else
                {
                    overflow = gameplayElementOverflow.Value;
                }

                X -= XIncrement * overflow;
                Y -= YIncrement * overflow;

                base.CheckBorderCollision();
                return timeInterval * overflow;
            }
        }

        private new double? CheckBorderCollision()
        {
            double xOverflow = 0;
            double yOverflow = 0;

            if (X < Radius)
            {
                xOverflow = X - Radius;
            }
            else if (X > Battlefield.Width - Radius)
            {
                xOverflow = X - Battlefield.Width + Radius;
            }

            if (Y < Radius)
            {
                yOverflow = Y - Radius;
            }
            else if (Y > Battlefield.Height - Radius)
            {
                yOverflow = Y - Battlefield.Height + Radius;
            }

            if (xOverflow == 0 && yOverflow == 0)
            {
                return null;
            }
            else
            {
                double xOverflowToIncrement = xOverflow != 0 ? xOverflow / XIncrement : 0;
                double yOverflowToIncrement = yOverflow != 0 ? yOverflow / YIncrement : 0;
                double overflowToIncrement = xOverflowToIncrement > yOverflowToIncrement ? xOverflowToIncrement : yOverflowToIncrement;

                return overflowToIncrement;
            }
        }

        private double? CheckGameplayElementCollision()
        {
            double? collisionLength = CheckGameplayElementCollision(this, GameplayProcessor.Tanks);

            if (collisionLength.HasValue)
            {
                double length = Math.Sqrt(Math.Pow(XIncrement, 2) + Math.Pow(YIncrement, 2));
                return collisionLength.Value / length;
            }
            else
            {
                return null;
            }
        }
    }
}
