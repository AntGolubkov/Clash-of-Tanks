namespace ClashOfTanks.Core.Gameplay.Models
{
    public class Tank : GameplayElement
    {
        private static double baseRadius = 10;

        public static double BaseRadius
        {
            get => baseRadius;
        }

        internal Tank(double x = 0, double y = 0, double angle = 0) : base(x, y, angle)
        {

        }
    }
}
