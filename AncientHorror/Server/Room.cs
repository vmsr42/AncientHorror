using AncientHorror.Server.Messaging;
using AncientHorror.Server.Messaging.InfoMessage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorror.Server.Rooms
{
    public class Room
    {
        public int Id { get; set; }
        public String Password { get; set; }
        public String Name{get;set;}
        private BlockingCollection<Abonent> abntsList = new BlockingCollection<Abonent>();

        public void SendMessage(ServerMessage msg)
        {
            foreach (var abn in abntsList)
            {
               
                    try
                    {
                        XmlSerializer writer = new XmlSerializer(typeof(ServerMessage));
                        using (MemoryStream ms = new MemoryStream())
                        {
                            writer.Serialize(ms, msg);
                            abn.Sock.Send(ms.ToArray());
                            
                        }
                    }
                    catch
                    {

                    }
          
            }
        }
        public void AddAbonent(Abonent ab)
        {
            abntsList.Add(ab);
            ServerInfoAbonentsMessage abinfo = new ServerInfoAbonentsMessage() { Abonents = new List<GameAbonent>() };
            foreach (var abon in  abntsList)
            {
                abinfo.Abonents.Add(abon.Gamer);
            }
            this.SendMessage(abinfo.GetServerMessage());
        }
    }
}
