using AncientHorrorClient.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AncientHorrorClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Global.NetworkClient.Disconnected+=NetworkClient_Disconnected;
        }

        private void NetworkClient_Disconnected()
        {
            if (!Global.CurrentWindow.IsMain)
            {
                Global.CurrentWindow.CloseWindow();
                var wind = new ConnectWindow();
            }

        }
    }
}
