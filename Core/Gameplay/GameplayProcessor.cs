using System;
using System.Collections.Generic;

using ClashOfTanks.Core.Gameplay.Models;
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
                new Tank()
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

                double angleInRadians = gameplayElement.Angle * Math.PI / 180;
                gameplayElement.X += gameplayElement.MoveSpeed * Math.Cos(angleInRadians);
                gameplayElement.Y += gameplayElement.MoveSpeed * Math.Sin(angleInRadians);

                CheckBorderCollision(gameplayElement);
            }

            return GameplayElements;
        }

        private static void CheckBorderCollision(GameplayElement gameplayElement)
        {
            if (gameplayElement.X < gameplayElement.Radius)
            {
                gameplayElement.X = gameplayElement.Radius;
            }
            else if (gameplayElement.X > GameplayElement.Battlefield.Width - gameplayElement.Radius)
            {
                gameplayElement.X = GameplayElement.Battlefield.Width - gameplayElement.Radius;
            }

            if (gameplayElement.Y < gameplayElement.Radius)
            {
                gameplayElement.Y = gameplayElement.Radius;
            }
            else if (gameplayElement.Y > GameplayElement.Battlefield.Height - gameplayElement.Radius)
            {
                gameplayElement.Y = GameplayElement.Battlefield.Height - gameplayElement.Radius;
            }
        }
    }
}
