namespace ClashOfTanks.Core.Gameplay.Models
{
    sealed class Explosion : GameplayElement
    {
        private double InitialRadius { get; set; }
        private double MaxRadius { get; set; }

        public Explosion(double x, double y, double radius) : base(Types.Explosion, null, x, y, radius, 0)
        {
            InitialRadius = Radius;
            MaxRadius = InitialRadius * 4;
        }

        public bool Animate(double timeInterval)
        {
            bool isAnimationFinished = false;
            Radius += (MaxRadius - InitialRadius) * timeInterval;

            if (Radius >= MaxRadius)
            {
                Radius = MaxRadius;
                isAnimationFinished = true;
            }

            return isAnimationFinished;
        }
    }
}
