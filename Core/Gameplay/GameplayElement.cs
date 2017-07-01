namespace ClashOfTanks.Core.Gameplay
{
    public class GameplayElement
    {
        public object Control { get; set; }

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

        internal GameplayElement(double x = 0, double y = 0, double angle = 0)
        {
            Control = null;

            X = x;
            Y = y;
            Angle = angle;

            MoveSpeed = 0;
            TurnSpeed = 0;
        }
    }
}
