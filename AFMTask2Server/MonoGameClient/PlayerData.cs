using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameClient
{
    [Serializable]
    public class PlayerData
    {
        public Guid playerID;
        public string GamerTag;
        public string UserName;
        public string clientID;
    }
}
