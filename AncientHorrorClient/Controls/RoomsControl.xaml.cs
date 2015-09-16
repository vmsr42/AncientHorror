using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AncientHorrorClient.Commands;
using AncientHorrorClient.Controls;
using AncientHorrorShared;
using AncientHorrorClient.Helpers;
using AncientHorrorClient.Windows;

namespace AncientHorrorClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для AbonentsControl.xaml
    /// </summary>
    public partial class RoomsControl : HeaderedControl
    {
        public bool IsJoinRVisible
        {
            get
            {
                if (Global.NetworkClient.Room.IsLobby && Selected != null)
                    return true;
                else
                    return false;
            }

        }
        public bool IsCreateRVisible
        {
            get
            {
                if (Global.NetworkClient.Room.IsLobby)
                    return true;
                else
                    return false;
            }

        }
        public GameRoomInfo Room
        {
            get
            {
                return Global.NetworkClient.Room;
            }
        }
        private String filtertext = String.Empty;
        public String FilterText
        {
            get
            {
                return filtertext;
            }
            set
            {
                if (value!=filtertext)
                {
                    filtertext = value;
                    OnPropertyChanged("FilterText");
                    this.RmList.Items.Filter = null;
                    this.RmList.Items.Filter = delegate(object obj)
                    {
                        GameRoomInfo rm = (GameRoomInfo)obj;
                        if (rm.Name.ToUpper().Trim().Contains(filtertext.ToUpper().Trim()))
                            return true;
                        else
                            return false;
                    };
                }
            }
        }
        private GameRoomInfo selected;
        public GameRoomInfo Selected 
        {
            get
            {
                return selected;
            }
            set
            {
                if (value!=selected)
                {
                    selected = value;
                    OnPropertyChanged("Selected");
                    OnPropertyChanged("IsJoinRVisible");
                }
            }
        }
        public static readonly DependencyProperty RoomsProperty =
DependencyProperty.Register("Rooms", typeof(ObservableCollection<GameRoomInfo>), typeof(RoomsControl));
        public ObservableCollection<GameRoomInfo> Rooms
        {
            get
            {
                return (ObservableCollection<GameRoomInfo>)this.GetValue(RoomsProperty);
            }
            set
            {
                this.SetValue(RoomsProperty, value);
            }
        }
        public RoomsControl()
        {
            InitializeComponent();
            Global.NetworkClient.RoomChanged+=NetworkClient_RoomChanged;
            
        }

        private void NetworkClient_RoomChanged(GameRoomInfo ab)
        {
            OnPropertyChanged("Room");
            OnPropertyChanged("IsJoinRVisible");
            OnPropertyChanged("IsCreateRVisible");
        }

        private void CreateRoomClick(object sender, RoutedEventArgs e)
        {
            if (IsCreateRVisible)
            {
                CreateRoomWindow crw = new CreateRoomWindow();
                crw.Closed += crw_Closed;
                crw.ShowDialog();
            }
        }

        private void JoinRoomClick(object sender, RoutedEventArgs e)
        {
            if (IsJoinRVisible)
            {

            }
        }

        private void crw_Closed(object sender, EventArgs e)
        {
            CreateRoomWindow crw = (CreateRoomWindow)sender;
            
        }

        private void ExitRoomClick(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Selected = null;
        }
       

    }
}
