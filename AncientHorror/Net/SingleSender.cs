using AncientHorrorShared.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Net
{
    public class SingleSender
    {
        public Socket Sock { get; set; }
        public byte[] buffer = new byte[4096];
        public void SendMessage(TransportContainer msg)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var data = Encoding.UTF8.GetBytes(msg.UTFSerialize());
                ms.Write(data, 0, data.Length);
                this.Sock.Send(ms.ToArray());
            }
        }
        public SingleSender(Socket sock)
        {
            this.Sock = sock;
        }
        public void ClearBuffer()
        {
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = 0;
        }
    }
}
