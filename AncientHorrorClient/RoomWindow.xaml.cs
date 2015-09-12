using AncientHorrorShared;
using AncientHorrorShared.Messaging;
using AncientHorrorShared.Messaging.InfoMessage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AncientHorrorClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class RoomWindow : BaseWindow
    {
        public Boolean IsLobby 
        { 
            get
            {
                if (Room.Id == 0)
                    return true;
                else
                    return false;
            }
        }
        private string error = String.Empty;
        public String Error
        {
            get
            {
                return error;
            }
            set
            {
                if (error!=value)
                {
                    error = value;
                    OnPropertyChanged("Error");
                }
            }
        }
        private GameAbonentInfo abon = null;
        public GameAbonentInfo Abonent
        {
            get
            {
                return abon;
            }
            set
            {
                if (abon!=value)
                {
                    abon = value;
                    OnPropertyChanged("Abonent");
                }
            }
        }
        private GameRoomInfo room = null;
        public GameRoomInfo Room
        {

            get
            {
                return room;
            }
            set
            {
                if (room != value)
                {
                    room = value;
                    OnPropertyChanged("Room");
                    OnPropertyChanged("IsLobby");
                }
            }
        }
        public ObservableCollection<GameRoomInfo> Rooms { get; set; }
        public ObservableCollection<GameAbonentInfo> Abonents { get; set; }
        public ObservableCollection<ChatMessage> Messages { get; set; }
        public RoomWindow()
            : base(false)
        {
            Global.NetworkClient.AbonentChanged += NetworkClient_AbonentChanged;
            Global.NetworkClient.RoomChanged+=NetworkClient_RoomChanged;
            Global.NetworkClient.RoomsMessageRecieved+=NetworkClient_RoomsMessageRecieved;
            InitializeComponent();
        }

        private void NetworkClient_RoomChanged(GameRoomInfo rm)
        {
            Messages.Add(new ChatMessage() { Abonent = Abonent, Roomid = Room.Id, RoomName = Room.Name, Time = DateTime.Now, Message = "вышел из комнаты" });
            Room = rm;
            Messages.Add(new ChatMessage() { Abonent = Abonent, Roomid = Room.Id, RoomName = Room.Name, Time = DateTime.Now, Message = "вошел в комнату" });
        }

        private void NetworkClient_AbonentChanged(GameAbonentInfo ab)
        {
            Messages.Add(new ChatMessage(){ Abonent = Abonent, Roomid = Room.Id, RoomName = Room.Name, Time = DateTime.Now, Message = "сменил имя на "+ab.Name });
            Abonent = ab;
            
        }

        private void NetworkClient_RoomsMessageRecieved(BaseMessage msg)
        {
            ServerInfoMessage simsg = (ServerInfoMessage)msg;
            switch (simsg.Type)
            {
                case SIMessageType.Error:
                    {
                        var errmsg = (ServerInfoErrorMessage)msg.GetInnerMessage();
                        Error = errmsg.Error;
                        break;
                    }
                case SIMessageType.Abonents:
                    {
                        var abnmsg = (ServerInfoAbonentsMessage)msg.GetInnerMessage();
                        AfterAbonentsInfoRecieved(abnmsg);
                        break;
                    }
                case SIMessageType.Rooms:
                    {
                        var rmsmsg = (ServerInfoRoomsMessage)msg.GetInnerMessage();
                        AfterRoomsInfoRecived(rmsmsg);
                        break;
                    }
            }
        }

        private void AfterRoomsInfoRecived(ServerInfoRoomsMessage rmsmsg)
        {
            //сравнение что было с тем что есть + запись в чат если лобби
            //обновление списка комнат
            throw new NotImplementedException();
        }

        private void AfterAbonentsInfoRecieved(ServerInfoAbonentsMessage abnmsg)
        {
            //сравнение что было с тем что есть + запись в чат
            //обновление списка абонентов
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            Global.NetworkClient.AbonentChanged -= NetworkClient_AbonentChanged;
            Global.NetworkClient.RoomChanged -= NetworkClient_RoomChanged;
            Global.NetworkClient.RoomsMessageRecieved -= NetworkClient_RoomsMessageRecieved;
        }
    }
}
