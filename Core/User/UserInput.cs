namespace ClashOfTanks.Core.User
{
    public static class UserInput
    {
        private static bool keyWPressed;
        private static bool keySPressed;
        private static bool keyAPressed;
        private static bool keyDPressed;

        public static bool KeyWPressed
        {
            private get => keyWPressed;
            set
            {
                keyWPressed = value;
                UserActions.MoveForward = KeyWPressed;
            }
        }
        public static bool KeySPressed
        {
            private get => keySPressed;
            set
            {
                keySPressed = value;
                UserActions.MoveBackward = KeySPressed;
            }
        }
        public static bool KeyAPressed
        {
            private get => keyAPressed;
            set
            {
                keyAPressed = value;
                UserActions.TurnLeft = KeyAPressed;
            }
        }
        public static bool KeyDPressed
        {
            private get => keyDPressed;
            set
            {
                keyDPressed = value;
                UserActions.TurnRight = KeyDPressed;
            }
        }
    }
}
