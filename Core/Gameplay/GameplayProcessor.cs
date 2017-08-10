using System.Collections.Generic;

using ClashOfTanks.Core.Gameplay.Models;
using ClashOfTanks.Core.User;

namespace ClashOfTanks.Core.Gameplay
{
    public static class GameplayProcessor
    {
        private static List<GameplayElement> GameplayElements { get; set; }
        internal static List<GameplayElement> Tanks { get; private set; }

        public static void SetupGameplay()
        {
            GameplayElements = new List<GameplayElement>();
            Tanks = new List<GameplayElement>();

            double tankRadius = 10;
            int i = 0;

            foreach (Player player in GameSession.Current.Players)
            {
                double tankX;

                if (GameSession.Current.Players.Count != 1)
                {
                    tankX = tankRadius + i * (GameplayElement.Battlefield.Width - tankRadius * 2) / (GameSession.Current.Players.Count - 1);
                }
                else
                {
                    tankX = GameplayElement.Battlefield.Width / 2;
                }

                Tank tank = new Tank(player, tankX, GameplayElement.Battlefield.Height / 2, tankRadius, 90);
                player.Tank = tank;
                GameplayElements.Add(tank);
                Tanks.Add(tank);

                i++;
            }
        }

        public static IEnumerable<GameplayElement> UpdateGameplay(double timeInterval)
        {
            List<GameplayElement> addGameplayElements = new List<GameplayElement>();
            List<GameplayElement> removeGameplayElements = new List<GameplayElement>();

            foreach (GameplayElement gameplayElement in GameplayElements)
            {
                switch (gameplayElement.Type)
                {
                    case GameplayElement.Types.Tank:
                        {
                            UpdateTank(gameplayElement as Tank, timeInterval, false, addGameplayElements, removeGameplayElements);
                            break;
                        }
                    case GameplayElement.Types.Projectile:
                        {
                            UpdateProjectile(gameplayElement as Projectile, timeInterval, addGameplayElements, removeGameplayElements);
                            break;
                        }
                    case GameplayElement.Types.Explosion:
                        {
                            UpdateExplosion(gameplayElement as Explosion, timeInterval, removeGameplayElements);
                            break;
                        }
                }
            }

            GameplayElements.AddRange(addGameplayElements);

            foreach (GameplayElement gameplayElement in removeGameplayElements)
            {
                GameplayElements.Remove(gameplayElement);
                gameplayElement.IsRemoved = true;
            }

            return GameplayElements;
        }

        private static void UpdateTank(Tank tank, double timeInterval, bool isUserActionsProcessed, List<GameplayElement> addGameplayElements, List<GameplayElement> removeGameplayElements)
        {
            if (!isUserActionsProcessed)
            {
                tank.ProcessUserActions();
                isUserActionsProcessed = true;
            }

            if (tank.Player.Actions.Shoot)
            {
                if (tank.ShotCooldown == 0)
                {
                    Projectile projectile = new Projectile(tank.X, tank.Y, Tank.GunRadiusToTankRadius * tank.Radius, tank.Angle, tank.ShotMoveSpeed, tank.ShotDamage);
                    projectile = projectile.SetupProjectile(Tank.GunLengthToTankRadius * tank.Radius);

                    if (projectile != null)
                    {
                        addGameplayElements.Add(projectile);
                        UpdateProjectile(projectile, timeInterval, addGameplayElements, removeGameplayElements);
                    }

                    tank.ShotCooldown += 1 / tank.ShotFrequency;
                    UpdateTank(tank, timeInterval, isUserActionsProcessed, addGameplayElements, removeGameplayElements);
                }
                else
                {
                    tank.ShotCooldown -= timeInterval;

                    if (tank.ShotCooldown > 0)
                    {
                        tank.UpdatePosition(timeInterval);
                    }
                    else
                    {
                        tank.UpdatePosition(tank.ShotCooldown + timeInterval);

                        timeInterval = -tank.ShotCooldown;
                        tank.ShotCooldown = 0;

                        UpdateTank(tank, timeInterval, isUserActionsProcessed, addGameplayElements, removeGameplayElements);
                    }
                }
            }
            else
            {
                tank.ShotCooldown -= timeInterval;

                if (tank.ShotCooldown < 0)
                {
                    tank.ShotCooldown = 0;
                }

                tank.UpdatePosition(timeInterval);
            }
        }

        private static void UpdateProjectile(Projectile projectile, double timeInterval, List<GameplayElement> addGameplayElements, List<GameplayElement> removeGameplayElements)
        {
            double? remainingTimeInterval = projectile.UpdatePosition(timeInterval);

            if (remainingTimeInterval.HasValue)
            {
                removeGameplayElements.Add(projectile);

                Explosion explosion = new Explosion(projectile.X, projectile.Y, projectile.Radius);
                addGameplayElements.Add(explosion);

                UpdateExplosion(explosion, remainingTimeInterval.Value, removeGameplayElements);

                if (projectile.LinkedGameplayElement != null)
                {
                    if ((projectile.LinkedGameplayElement as Tank).ProcessCollision(projectile.Damage))
                    {
                        removeGameplayElements.Add(projectile.LinkedGameplayElement);
                        Tanks.Remove(projectile.LinkedGameplayElement);

                        Explosion tankExplosion = new Explosion(projectile.LinkedGameplayElement.X, projectile.LinkedGameplayElement.Y, projectile.LinkedGameplayElement.Radius);
                        addGameplayElements.Add(tankExplosion);

                        UpdateExplosion(tankExplosion, remainingTimeInterval.Value, removeGameplayElements);
                    }
                }
            }
        }

        private static void UpdateExplosion(Explosion explosion, double timeInterval, List<GameplayElement> removeGameplayElements)
        {
            if (explosion.Animate(timeInterval))
            {
                removeGameplayElements.Add(explosion);
            }
        }
    }
}
