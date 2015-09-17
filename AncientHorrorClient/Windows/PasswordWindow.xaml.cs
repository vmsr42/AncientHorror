using AncientHorrorShared;
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

namespace AncientHorrorClient.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateRoomWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window, INotifyPropertyChanged
    {
        public bool Result { get; set; }
        private GameRoomInfo room;
        public GameRoomInfo Room
        {
            get
            {
                return room;
            }
        }
        public String RoomName { get; set; }
        public String Password { get; set; }
        public PasswordWindow(GameRoomInfo room)
        {
            this.room = room;
            var parent = Global.CurrentWindow;
            double top = parent.Top + parent.ActualHeight / 2-150;
            double left = parent.Left + parent.ActualWidth / 2-150;
            if (parent.WindowState != System.Windows.WindowState.Normal)
            {
                top = SystemParameters.PrimaryScreenHeight / 2 - 150;
                left = SystemParameters.PrimaryScreenWidth / 2 - 150;
            }
            this.Top = top;
            this.Left = left;
            Password = String.Empty;
            InitializeComponent();
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void OkClick(object sender, RoutedEventArgs e)
        {
                Result = true;
                this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Result = false;
            this.Close();

        }
    }
}
