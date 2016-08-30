using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using Microsoft.AspNet.SignalR;
using Microsoft.Xna.Framework;

namespace MonoGameClient
{
    public class PlayerData
    {
        public string PlayerID;
        public int score;
        //public string UserName;
    }

    public static class HubState
    {
        public static List<PlayerData> players = new List<PlayerData>()
        {
                new PlayerData { PlayerID = "player1", score = 0 },
                new PlayerData { PlayerID = "player2", score = 0 },
        };
    }

    public class MoveCharacterHub : Hub
    {
        static Timer t, c = new Timer();
        public MoveCharacterHub() : base()
        {
            Random rand = new Random();
            //c.Elapsed += C_Elapsed;
            //c = new Timer(rand.Next(1000, 90000));
            //c.Start();

            t = new Timer(10000);
            t.Elapsed += T_Elasped;
            t.Start();
        }

        private void T_Elasped(object sender, ElapsedEventArgs e)
        {
            Random r = new Random();
            int x, y;
            x = r.Next(0, 500);
            y = r.Next(0, 500);

            Point pos = new Point(x, y);

            Clients.All.BroadcastMessage(pos);
        }

        //private void C_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    Vector2 i;
        //    i = new Vector2(0, 500);

        //    Point incPos = new Point(Convert.ToInt32(i));
        //    bool incBool = true;

        //    Clients.All.BroadcastMessage(incPos);
        //    Clients.All.BroadcastMessage(incBool);
        //}

       

        public void sendPlayers()
        {
            Clients.Caller.RecievePlayers(HubState.players);
        }

        public List<PlayerData> getPlayers()
        {
            return HubState.players;
        }
     }
}
