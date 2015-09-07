using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorrorShare.Messaging.AbonentsCommand
{
    [DataContract]
    public class KickUserMessage
    {
        [DataMember]
        public int UserId { get; set; }
        public ServerMessage GetServerMessage()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(KickUserMessage));
            AbonentsCommandMessage ac = new AbonentsCommandMessage() { Message = String.Empty, Type = AbonentsCommandType.KickUser };

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);
                byte[] data = stream.ToArray();
                ac.Message = Encoding.UTF8.GetString(data, 0, data.Length);
            }
            return ac.GetServerMessage();

        }
    }
}
