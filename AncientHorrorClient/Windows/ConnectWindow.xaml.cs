using AncientHorrorShared;
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
        public String Error { get; set; }
        public Visibility HasError
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Error))
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
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

            RoomWindow rw = new RoomWindow();
            var answer = await Global.NetworkClient.ConnectToServer(Login, this.pass.Password);

            if (!answer.Result)
            {
                Error = answer.Message;
                rw.CloseWindow();
                
            }
            else
            {
                Error = String.Empty;
                rw.ShowWindow();
            }
            OnPropertyChanged("Error");
            OnPropertyChanged("HasError");
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.CloseWindow();
        }
    }
}
