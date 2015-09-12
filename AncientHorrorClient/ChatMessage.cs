using AncientHorrorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorClient
{
    public class ChatMessage
    {
        public GameAbonentInfo Abonent { get; set; }
        public DateTime Time { get; set; }
        public String Message { get; set; }
        public int Roomid { get; set; }
        public String RoomName { get; set; }
    }
}
