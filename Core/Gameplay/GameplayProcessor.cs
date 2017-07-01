using System;
using System.Collections.Generic;

using ClashOfTanks.Core.User;

namespace ClashOfTanks.Core.Gameplay
{
    public static class GameplayProcessor
    {
        private static IEnumerable<GameplayElement> GameplayElements { get; set; }

        public static IEnumerable<GameplayElement> GenerateInitialGameplayElements()
        {
            GameplayElements = new List<GameplayElement>()
            {
                new GameplayElement()
            };

            return GameplayElements;
        }

        public static IEnumerable<GameplayElement> ProcessGameplay()
        {
            foreach (GameplayElement gameplayElement in GameplayElements)
            {
                gameplayElement.MoveSpeed = 0;
                gameplayElement.TurnSpeed = 0;

                if (UserActions.MoveForward)
                {
                    gameplayElement.MoveSpeed += 5;
                }
                if (UserActions.MoveBackward)
                {
                    gameplayElement.MoveSpeed -= 5;
                }
                if (UserActions.TurnLeft)
                {
                    gameplayElement.TurnSpeed += 3;
                }
                if (UserActions.TurnRight)
                {
                    gameplayElement.TurnSpeed -= 3;
                }

                gameplayElement.Angle += gameplayElement.TurnSpeed;
                gameplayElement.X += gameplayElement.MoveSpeed * Math.Cos(gameplayElement.Angle * Math.PI / 180);
                gameplayElement.Y += gameplayElement.MoveSpeed * Math.Sin(gameplayElement.Angle * Math.PI / 180);
            }

            return GameplayElements;
        }
    }
}
