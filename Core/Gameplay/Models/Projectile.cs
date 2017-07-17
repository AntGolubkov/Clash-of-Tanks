namespace ClashOfTanks.Core.Gameplay.Models
{
    sealed class Projectile : GameplayElement
    {
        private double MoveSpeed { get; set; }

        public Projectile(double x, double y, double radius, double angle, double moveSpeed) : base(Types.Projectile, null, x, y, radius, angle) => MoveSpeed = moveSpeed;

        public Projectile SetupProjectile(double offset)
        {
            Projectile projectile = this;
            UpdatePosition(offset, 0, 1);

            if (projectile.CheckCollision(0).HasValue)
            {
                projectile = null;
            }

            return projectile;
        }

        public double? UpdatePosition(double timeInterval)
        {
            UpdatePosition(MoveSpeed, 0, timeInterval);
            return CheckCollision(timeInterval);
        }

        private double? CheckCollision(double timeInterval)
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

                X -= XIncrement * overflowToIncrement;
                Y -= YIncrement * overflowToIncrement;

                CheckCollision();

                return timeInterval * overflowToIncrement;
            }
        }
    }
}
