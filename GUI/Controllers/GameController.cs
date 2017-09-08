using ClashOfTanks.Core.GameModels;
using ClashOfTanks.Core.NetworkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClashOfTanks.GUI.Controllers
{
    public static class GameController
    {
        public static void AnnounceGame(this Game game)
        {
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        Network.Current.SendBroadcast(Encoding.ASCII.GetBytes($"{game.Name}:{game.Players.Count}"));
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception) { }
            });
        }

        public static void ScanGames(this Game game, Action<string, string, string> resultAction)
        {
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        byte[] data;
                        string ipAddress = null;

                        if ((data = Network.Current.ReceiveBroadcast(ref ipAddress)) != null)
                        {
                            string[] properties = Encoding.ASCII.GetString(data).Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            resultAction.Invoke(properties[0], ipAddress, properties[1]);
                        }
                    }
                }
                catch (Exception) { }
            });
        }
    }
}
