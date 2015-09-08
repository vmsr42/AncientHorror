﻿using AncientHorror.Server;
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
using AncientHorrorShared;

namespace AncientHorror
{
    class Program
    {
        private static int number = 1;
        
        public static List<Room> Rooms = new List<Room>();
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private static LobbyRoom lroom = null;
        public static LobbyRoom Lobby
        {
            get
            {
                if (lroom==null)
                {
                    var rm = Rooms.Where(r => r.Id == 0).FirstOrDefault();
                    if (rm != null)
                        lroom = (LobbyRoom)rm;
                }
                return lroom;

            }
        }
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
            Abonent ab = new Abonent() { Status = AbonentStatusEnum.Guest, Gamer = new GameAbonent() { Name = "Guest" + number, Id=number }, Sock = clientSock };
            number++;
            Lobby.AddAbonent(ab);
            servSock.BeginAccept(new AsyncCallback(Listen_Callback), servSock);
        }


        
    }
}