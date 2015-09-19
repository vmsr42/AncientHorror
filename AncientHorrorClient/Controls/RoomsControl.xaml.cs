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
using AncientHorrorShared.Messaging.AbonentsCommand;
using AncientHorrorClient.Network;

namespace AncientHorrorClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для AbonentsControl.xaml
    /// </summary>
    public partial class RoomsControl : HeaderedControl
    {
        #region Команды интервейса
        public bool IsOwner
        {
            get
            {
                if (Room == null||Room.Owner==null)
                    return false;
                if (Room.Owner.Id == Global.NetworkClient.Abonent.Id)
                    return true;
                return false;
            }
        }
        public string OwnerName
        {
            get
            {
                if (Room.Owner == null)
                    return String.Empty;
                return Room.Owner.Name;
            }
        }
        public int ButtonsRow
        {
            get
            {
                if (!Room.IsLobby)
                    return 2;
                else
                    return 1;
            }
        }
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
        #endregion Команды интервейса
        public RoomsControl()
        {
            InitializeComponent();
            Global.NetworkClient.RoomChanged+=NetworkClient_RoomChanged;
            
        }

        private void NetworkClient_RoomChanged(GameRoomInfo ab)
        {
            OnPropertyChanged("Room");
            OnPropertyChanged("OwnerName");
            OnPropertyChanged("IsJoinRVisible");
            OnPropertyChanged("IsCreateRVisible");
            OnPropertyChanged("ButtonsRow");
            OnPropertyChanged("IsOwner");   
            
        }
        //позже перенести в статику чтоб единообразно было
        private void CreateRoomClick(object sender, RoutedEventArgs e)
        {
            Error = String.Empty;
            if (IsCreateRVisible)
            {
                CreateRoomWindow crw = new CreateRoomWindow();
                crw.Closed += crw_Closed;
                crw.ShowDialog();
            }
        }

        private void JoinRoomClick(object sender, RoutedEventArgs e)
        {
            Error = String.Empty;
            if (IsJoinRVisible)
            {
                if (Selected.HavePassword)
                {
                    PasswordWindow passWind = new PasswordWindow(selected);
                    passWind.Closed+=passWind_Closed;
                    passWind.ShowDialog();
                }
                else
                {
                    SendJoinMessage(Selected.Id, String.Empty);
                }
            }
        }

        private void passWind_Closed(object sender, EventArgs e)
        {
            PasswordWindow passWind = (PasswordWindow)sender;
            if (passWind.Result)
            {
                SendJoinMessage(passWind.Room.Id, passWind.Password);
            }
        }
        private async void SendJoinMessage(int roomId, String password)
        {
            JoinRoomMessage jrm = new JoinRoomMessage() { Password = password, RoomId = roomId };
            Global.Status = BusyMessageEnum.JoiningRoom;
            var answer = await Global.NetworkClient.SendMessage(jrm);
            if (!answer.Result)
                Error = answer.Message;
            OnPropertyChanged("Error");
            OnPropertyChanged("HasError");
            Global.Status = BusyMessageEnum.ExitingRoom;
        }
        private async void crw_Closed(object sender, EventArgs e)
        {
            CreateRoomWindow crw = (CreateRoomWindow)sender;
            CreateRoomMessage crm = new CreateRoomMessage();
            if (crw.Result==true)
            {
                crm.Name = crw.RoomName;
                crm.Password = crw.Password;
                crm.Capability = int.Parse(crw.Capability);
                Global.Status = BusyMessageEnum.CreatingRoom;
                ClientAnswer answer = await Global.NetworkClient.SendMessage(crm);
                Global.Status = BusyMessageEnum.None;
                if (!answer.Result)
                    Error = answer.Message;
                    
            }
            OnPropertyChanged("Error");
            OnPropertyChanged("HasError");

        }

        private async void ExitRoomClick(object sender, RoutedEventArgs e)
        {
            Error = String.Empty;
            if (!Room.IsLobby)
            {
                ExitRoomMessage em = new ExitRoomMessage();
                Global.Status = BusyMessageEnum.ExitingRoom;
                await Global.NetworkClient.SendMessage(em);
                Global.Status = BusyMessageEnum.None;
            }
            OnPropertyChanged("Error");
            OnPropertyChanged("HasError");
        }

        private void StartRoomClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ну типа стартовал");
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Selected = null;
        }
       

    }
}
