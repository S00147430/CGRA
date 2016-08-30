using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MonoGameClient;

namespace AFMTask2Server
{
    public class PlayerRegister : Hub
    {
        List<PlayerData> players = new List<PlayerData>()
        { new PlayerData { playerID = Guid.NewGuid(), GamerTag = "player 1", UserName = "p1" }, };

        List<PlayerData> joined = new List<PlayerData>();

        bool playing = false;

        public PlayerData join(string username)
        {
            if (!playing)
            {
                PlayerData p = players.FirstOrDefault(pl => pl.UserName == username);
                if (p != null && !joined.Contains(p)) // Valid player
                {
                    p.clientID = Context.ConnectionId;
                    joined.Add(p);
                    if (joined.Count() > 1)
                    {
                        Clients.All.play(joined); // Note clients must subscribe to this event
                        playing = true;
                    }
                    return p;
                }
                else return null;
            }
            else return null;
        }

    }
}