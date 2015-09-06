using AncientHorror.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;
using AncientHorror.Server.Rooms;
using AncientHorror.Server.Messaging;

namespace AncientHorror
{
    class Program
    {
        static int number = 0;
        internal static Room mainRoom = new Room() { Id= 0, Name = "Лобби" };
        internal static List<Room> GameRooms = new List<Room>();
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        static void Main(string[] args)
        {
        try
            {
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 7777);
                Socket servSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                servSock.Bind(localEndPoint);
                servSock.Listen(50);

                while (true)
                {
                    allDone.Reset();

                    servSock.BeginAccept(new AsyncCallback(Listen_Callback), servSock);
                    allDone.WaitOne();
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("приложение не смогло запуститься по причине "+ex.Message);
            }
        }

        private static void Listen_Callback(IAsyncResult ar)
        {
            Socket servSock = (Socket)ar.AsyncState;
            Socket clientSock = servSock.EndAccept(ar);
            Abonent ab = new Abonent() { Status = AbonentStatusEnum.Guest, Gamer = new GameAbonent() { Name = "Guest" + number, Id=0 }, Sock = clientSock };
            number++;
            mainRoom.AddAbonent(ab);
            
            clientSock.BeginReceive(ab.buffer, 0, 4096, SocketFlags.None,  new AsyncCallback(AfterRecieve), ab);
            servSock.BeginAccept(new AsyncCallback(Listen_Callback), servSock);
        }


        private static void AfterRecieve(IAsyncResult ar)
        {
            Abonent ab = (Abonent)ar.AsyncState;
            try
            {
                
                ab.Sock.EndReceive(ar);
                if (ab.Status != AbonentStatusEnum.Disconnected)
                {
                    using (MemoryStream ms = new MemoryStream(ab.buffer))
                    {
                        XmlSerializer reader = new XmlSerializer(typeof(ServerMessage));
                        var msg = (ServerMessage)reader.Deserialize(ms);
                        switch (msg.MsgType)
                        {
                            case ServerMessageTypeEnum.AbonentCommand:
                                if (msg.Message =="Authorize")
                                {
                                    
                                }
                                else if (msg.Message=="UnAuthorize")
                                {
                                    ab.Id = 0;
                                    ab.Status = AbonentStatusEnum.Guest;
                                    ab.Nick = "Guest" + number;
                                    number++; 
                                }
                                else if (msg.Message == "Exit")
                                {
                                    ab.Id = 0;
                                    ab.Status = AbonentStatusEnum.Disconnected;
                                    ab.Nick = String.Empty;                               
                                }
                                else if (msg.Message == "JoinRoom")
                                {
                                    try
                                }
                                break;
                        }
                        ab.buffer = new byte[4096];
                        ab.Sock.BeginReceive(ab.buffer, 0, 4096, 0, new AsyncCallback(AfterRecieve), ab);
                    }
                }
            }
            catch
            {

                    ab.Status = AbonentStatusEnum.Disconnected;
                    ab.Nick = String.Empty;
            }
            try
            {
                if (ab.Status == AbonentStatusEnum.Disconnected)
                {
                    ab.Sock.Close();
                    
                    mainRoom.Abonents.TryTake(out ab);
                }
            }
            catch { }
            
        }
    }
}