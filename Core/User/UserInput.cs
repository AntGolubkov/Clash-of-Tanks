using System.Collections.Generic;

namespace ClashOfTanks.Core.User
{
    public static class UserInput
    {
        private static Dictionary<string, Player> patterns = new Dictionary<string, Player>()
        {
            { "W_S_A_D_Space", null },
            { "U_D_L_R_Enter", null }
        };

        private static bool keyWPressed;
        private static bool keySPressed;
        private static bool keyAPressed;
        private static bool keyDPressed;
        private static bool keySpacePressed;

        private static bool keyUpPressed;
        private static bool keyDownPressed;
        private static bool keyLeftPressed;
        private static bool keyRightPressed;
        private static bool keyEnterPressed;

        public static Dictionary<string, Player> Patterns
        {
            get => patterns;
            set => patterns = value;
        }

        public static bool KeyWPressed
        {
            private get => keyWPressed;
            set
            {
                keyWPressed = value;

                if (Patterns.ContainsKey("W_S_A_D_Space"))
                {
                    Patterns["W_S_A_D_Space"].Actions.MoveForward = KeyWPressed;
                }
            }
        }
        public static bool KeySPressed
        {
            private get => keySPressed;
            set
            {
                keySPressed = value;

                if (Patterns.ContainsKey("W_S_A_D_Space"))
                {
                    Patterns["W_S_A_D_Space"].Actions.MoveBackward = KeySPressed;
                }
            }
        }
        public static bool KeyAPressed
        {
            private get => keyAPressed;
            set
            {
                keyAPressed = value;

                if (Patterns.ContainsKey("W_S_A_D_Space"))
                {
                    Patterns["W_S_A_D_Space"].Actions.TurnLeft = KeyAPressed;
                }
            }
        }
        public static bool KeyDPressed
        {
            private get => keyDPressed;
            set
            {
                keyDPressed = value;

                if (Patterns.ContainsKey("W_S_A_D_Space"))
                {
                    Patterns["W_S_A_D_Space"].Actions.TurnRight = KeyDPressed;
                }
            }
        }
        public static bool KeySpacePressed
        {
            private get => keySpacePressed;
            set
            {
                keySpacePressed = value;

                if (Patterns.ContainsKey("W_S_A_D_Space"))
                {
                    Patterns["W_S_A_D_Space"].Actions.Shoot = KeySpacePressed;
                }
            }
        }

        public static bool KeyUpPressed
        {
            private get => keyUpPressed;
            set
            {
                keyUpPressed = value;

                if (Patterns.ContainsKey("U_D_L_R_Enter"))
                {
                    Patterns["U_D_L_R_Enter"].Actions.MoveForward = KeyUpPressed;
                }
            }
        }
        public static bool KeyDownPressed
        {
            private get => keyDownPressed;
            set
            {
                keyDownPressed = value;

                if (Patterns.ContainsKey("U_D_L_R_Enter"))
                {
                    Patterns["U_D_L_R_Enter"].Actions.MoveBackward = KeyDownPressed;
                }
            }
        }
        public static bool KeyLeftPressed
        {
            private get => keyLeftPressed;
            set
            {
                keyLeftPressed = value;

                if (Patterns.ContainsKey("U_D_L_R_Enter"))
                {
                    Patterns["U_D_L_R_Enter"].Actions.TurnLeft = KeyLeftPressed;
                }
            }
        }
        public static bool KeyRightPressed
        {
            private get => keyRightPressed;
            set
            {
                keyRightPressed = value;

                if (Patterns.ContainsKey("U_D_L_R_Enter"))
                {
                    Patterns["U_D_L_R_Enter"].Actions.TurnRight = KeyRightPressed;
                }
            }
        }
        public static bool KeyEnterPressed
        {
            private get => keyEnterPressed;
            set
            {
                keyEnterPressed = value;

                if (Patterns.ContainsKey("U_D_L_R_Enter"))
                {
                    Patterns["U_D_L_R_Enter"].Actions.Shoot = KeyEnterPressed;
                }
            }
        }
    }
}
