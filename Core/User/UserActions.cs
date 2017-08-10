using System;

namespace ClashOfTanks.Core.User
{
    [Serializable]
    public class UserActions
    {
        public bool MoveForward { get; set; }
        public bool MoveBackward { get; set; }
        public bool TurnLeft { get; set; }
        public bool TurnRight { get; set; }
        public bool Shoot { get; set; }

        public UserActions()
        {
            MoveForward = false;
            MoveBackward = false;
            TurnLeft = false;
            TurnRight = false;
            Shoot = false;
        }
    }
}
