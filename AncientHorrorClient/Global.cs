using AncientHorrorClient.Helpers;
using AncientHorrorClient.Network;
using AncientHorrorClient.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private static BusyMessageEnum status = BusyMessageEnum.None;
        public static BusyMessageEnum Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status!=value)
                {
                    status = value;
                    OnStaticPropertyChanged("Status");
                }
            }
        }

        #region StaticNotifyPropertyChanged
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void OnStaticPropertyChanged(string property)
        {
            var handler = StaticPropertyChanged;
            if (handler != null)
            {
                handler(null, new PropertyChangedEventArgs(property));
            }
        }
        #endregion StaticNotifyPropertyChanged
    }
}
