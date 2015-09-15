using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AncientHorrorClient
{
    /// <summary>
    /// Логика взаимодействия для ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window, INotifyPropertyChanged
    {
        private string error = String.Empty;
        public String Error
        {
            get
            {
                return error;
            }
            private set
            {
                if (value!=error)
                {
                    error = value.Trim();
                    OnPropertyChanged("Error");
                    OnPropertyChanged("HasError");
                }
            }
        }
        public bool HasError
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Error))
                    return false;
                else
                    return true;
            }
        }
        public String Login { get; set; }
        public ConnectWindow()
        {
            InitializeComponent();
            Global.CurrentWindow = this;
        }

        private void ConnectClick(object sender, RoutedEventArgs e)
        {
            string err= String.Empty;
            if (!Global.NetworkClient.ConnectToServer(Global.servLink, Global.servPort, out err))
                Error = err;
            else
            {
                var wind = new RoomWindow();
                this.Close();
                wind.Show();
            }
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
