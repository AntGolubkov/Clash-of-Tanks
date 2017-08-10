using System;
using System.Collections.Generic;

namespace ClashOfTanks.Core.User
{
    [Serializable]
    public class UserInput
    {
        private Player Player { get; set; }

        private bool keyWPressed;
        private bool keySPressed;
        private bool keyAPressed;
        private bool keyDPressed;
        private bool keySpacePressed;

        private bool keyUpPressed;
        private bool keyDownPressed;
        private bool keyLeftPressed;
        private bool keyRightPressed;
        private bool keyEnterPressed;

        public bool KeyWPressed
        {
            private get => keyWPressed;
            set
            {
                keyWPressed = value;
                Player.Actions.MoveForward = KeyWPressed;
            }
        }
        public bool KeySPressed
        {
            private get => keySPressed;
            set
            {
                keySPressed = value;
                Player.Actions.MoveBackward = KeySPressed;
            }
        }
        public bool KeyAPressed
        {
            private get => keyAPressed;
            set
            {
                keyAPressed = value;
                Player.Actions.TurnLeft = KeyAPressed;
            }
        }
        public bool KeyDPressed
        {
            private get => keyDPressed;
            set
            {
                keyDPressed = value;
                Player.Actions.TurnRight = KeyDPressed;
            }
        }
        public bool KeySpacePressed
        {
            private get => keySpacePressed;
            set
            {
                keySpacePressed = value;
                Player.Actions.Shoot = KeySpacePressed;
            }
        }

        public bool KeyUpPressed
        {
            private get => keyUpPressed;
            set
            {
                keyUpPressed = value;
                Player.Actions.MoveForward = KeyUpPressed;
            }
        }
        public bool KeyDownPressed
        {
            private get => keyDownPressed;
            set
            {
                keyDownPressed = value;
                Player.Actions.MoveBackward = KeyDownPressed;
            }
        }
        public bool KeyLeftPressed
        {
            private get => keyLeftPressed;
            set
            {
                keyLeftPressed = value;
                Player.Actions.TurnLeft = KeyLeftPressed;
            }
        }
        public bool KeyRightPressed
        {
            private get => keyRightPressed;
            set
            {
                keyRightPressed = value;
                Player.Actions.TurnRight = KeyRightPressed;
            }
        }
        public bool KeyEnterPressed
        {
            private get => keyEnterPressed;
            set
            {
                keyEnterPressed = value;
                Player.Actions.Shoot = KeyEnterPressed;
            }
        }

        public UserInput(Player player)
        {
            Player = player;

            KeyWPressed = false;
            KeySPressed = false;
            KeyAPressed = false;
            KeyDPressed = false;
            KeySpacePressed = false;
            KeyUpPressed = false;
            KeyDownPressed = false;
            KeyLeftPressed = false;
            KeyRightPressed = false;
            KeyEnterPressed = false;
        }
    }
}
