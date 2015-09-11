using AncientHorrorClient.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AncientHorrorClient
{
    public static class Global
    {
        public static readonly AHNetworkClient NetworkClient = new AHNetworkClient(2);
        public static Window CurrentWindow;
        public static readonly String servLink = "";
        public static readonly int servPort = 0;
    }
}
