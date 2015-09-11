using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using StartRoom;

namespace StartRoom
{
    public static class Party // пока не нужно удалить если не пригодится
    {
        public static List<string> Room { get; set; }

        public static int RoomNumber { get; set; }

        public static int RoomMember { get; set; }
    }
}
