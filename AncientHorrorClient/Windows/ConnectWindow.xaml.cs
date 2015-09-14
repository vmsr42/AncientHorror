using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AncientHorrorClient.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class ConnectWindow : BaseWindow
    {
        private String error = String.Empty;
        public String Error
        {
            get
            {
                return error;
            }
            set
            {
                if (value!=error)
                {
                    error = value;
                    OnPropertyChanged("Error");
                    OnPropertyChanged("HasError");
                }
            }
        }
        public Boolean HasError
        {
            get
            {
                return !String.IsNullOrWhiteSpace(Error);
            }
        }
        public String Login { get; set; }
        public ConnectWindow():base(true)
        {
            InitializeComponent();
            Global.CurrentWindow = this;
        }

        private async void ConnectClick(object sender, RoutedEventArgs e)
        {
            SetBusyStatus(true, "Соединяемся с сервером");
            RoomWindow rw = new RoomWindow();
            var answer = await Global.NetworkClient.ConnectToServer(Login, this.pass.Password);
            SetBusyStatus(false, String.Empty);
            if (!answer.Result)
            {
                Error = answer.Message;
                rw.CloseWindow();
            }
            else
            {
                rw.ShowWindow();
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.CloseWindow();
        }
    }
}
