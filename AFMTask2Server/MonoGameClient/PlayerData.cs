using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameClient
{
    public class PlayerData
    {
        public string PlayerID;
        public int score;
        public string outcome;
        public int collectableInteractions;

        public static List<PlayerData> players = new List<PlayerData>()
        {
               
        };

    }
}
