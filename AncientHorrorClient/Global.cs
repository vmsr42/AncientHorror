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
        public static readonly AHNetworkClient NetworkClient = new AHNetworkClient(20);
        public static BaseWindow CurrentWindow;
        public static readonly String servLink = "127.0.0.1";
        public static readonly int servPort = 7777;
    }
}
