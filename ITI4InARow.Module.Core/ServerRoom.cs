using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI4InARow.Module.Core
{
    public class ServerRoom
    {
        public int RoomID { get; set; }
        public string name { get; set; }
        public RoomState roomState { get; set; }
        public string player1Name { get; set; }
        public string player2Name { get; set; }
        public bool isSpectator { get; set; }
        public int spectatorCount { get; set; }
        public List<int> spectatorsID { get; set; }
    }
    public enum RoomState
    {
        wating,
        complete
    }
}

