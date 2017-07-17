using ClashOfTanks.Core.User;

namespace ClashOfTanks.Core.Gameplay.Models
{
    public sealed class Tank : GameplayElement
    {
        private static readonly double gunRadiusToTankRadius = 0.2;
        private static readonly double gunLengthToTankRadius = 1.5;

        public static double GunRadiusToTankRadius
        {
            get => gunRadiusToTankRadius;
        }
        public static double GunLengthToTankRadius
        {
            get => gunLengthToTankRadius;
        }

        private double MoveSpeed { get; set; }
        private double TurnSpeed { get; set; }

        private double CurrentMoveSpeed { get; set; }
        private double CurrentTurnSpeed { get; set; }

        internal double ShotFrequency { get; private set; }
        internal double ShotMoveSpeed { get; private set; }
        internal double ShotCooldown { get; set; }

        internal Tank(Player player, double x, double y, double radius, double angle) : base(Types.Tank, player, x, y, radius, angle)
        {
            MoveSpeed = 250;
            TurnSpeed = 180;

            CurrentMoveSpeed = 0;
            CurrentTurnSpeed = 0;

            ShotFrequency = 30;
            ShotMoveSpeed = 500;
            ShotCooldown = 0;
        }

        internal void ProcessUserActions()
        {
            CurrentMoveSpeed = 0;

            if (Player.Actions.MoveForward)
            {
                CurrentMoveSpeed += MoveSpeed;
            }
            if (Player.Actions.MoveBackward)
            {
                CurrentMoveSpeed -= MoveSpeed;
            }

            CurrentTurnSpeed = 0;

            if (Player.Actions.TurnLeft)
            {
                CurrentTurnSpeed += TurnSpeed;
            }
            if (Player.Actions.TurnRight)
            {
                CurrentTurnSpeed -= TurnSpeed;
            }
        }

        internal void UpdatePosition(double timeInterval)
        {
            UpdatePosition(CurrentMoveSpeed, CurrentTurnSpeed, timeInterval);
            CheckCollision();
        }
    }
}
