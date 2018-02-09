using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ITI4InARow.Module.Core
{
    public class ListofRoomsMessage : MessageBase
    {
        public List<ServerRoom> availableRooms;

        public ListofRoomsMessage(List<ServerRoom> availableRooms)
        {
            this.availableRooms = availableRooms;
        }

    }
}

