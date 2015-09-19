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
    public partial class CreateRoomWindow : Window, INotifyPropertyChanged
    {
        #region Свойства
        public bool Result { get; set; }
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
        public String RoomName { get; set; }
        public String Capability { get; set; }
        public String Password { get; set; }
        #endregion Свойства
        public CreateRoomWindow()
        {
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
            Capability = "8";
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
            Error = String.Empty;
            bool check = true;
            if (String.IsNullOrWhiteSpace(RoomName))
            {
                Error = "Не правильное имя комнаты";
                check = false;
            }
            int capa = 8;
            if (!int.TryParse(Capability, out capa))
            {
                Capability = capa.ToString();
                Error = "Не правильное значение вместительности комнаты";
                check = false;
            }
            if (check&&capa<1)
            {
                Error = "Вместительность не может быть меньше 1";
                check = false;
            }
            if (check)
            {
                Result = true;
                this.Close();
            }
            
                OnPropertyChanged("Error");
                OnPropertyChanged("HasError");
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Result = false;
            this.Close();

        }
    }
}
