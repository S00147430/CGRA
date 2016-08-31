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
        public string outcome;
        public int collectableInteractions;
    }

    public class Check
    {
        public bool WriteNote;
        public Point PosNote;
    }

    public static class HubState
    {
        public static List<PlayerData> players = new List<PlayerData>()
        {
                new PlayerData { PlayerID = "Player User", score = 20, outcome = "Win", collectableInteractions = 25},
                new PlayerData { PlayerID = "Player User", score = 20, outcome = "Win", collectableInteractions = 25},
        };

        public static List<Check> Notes = new List<Check>()
        {
            new Check { WriteNote = true, PosNote = new Point(20, 20)},
        };
    }

    public class MoveCharacterHub : Hub
    {
        static Timer t, c = new Timer();
        bool incBool = false;
        public MoveCharacterHub() : base()
        {
            Random rand = new Random();
            c.Elapsed += C_Elapsed;
            c = new Timer(rand.Next(1000, 90000));
            
            if (incBool == true)
            {
                c.Start();
            }

            t = new Timer(10000);
            t.Elapsed += T_Elasped;
            t.Start();
        }

        private void T_Elasped(object sender, ElapsedEventArgs e)
        {
            int end = 0;
            int x, y;
            Random r = new Random();
            x = r.Next(0, 500);
            y = r.Next(0, 500);

            Point pos = new Point(x, y);

            Clients.All.BroadcastMessage(pos);
        }

        private void C_Elapsed(object sender, ElapsedEventArgs e)
        {
            
        }

        //public void note()
        //{
        //    Vector2 i;
        //    i = new Vector2(200, 500);

        //    Point incPos = new Point(Convert.ToInt32(i));

        //    if (incBool == true)
        //    {
        //        Clients.All.BroadcastMessage(incPos);
        //    }
        //}

        public List<Check> getNote()
        {
            return HubState.Notes;
        }

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
