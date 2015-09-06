using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorror.Server.Messaging.AbonentsCommand
{
    public class ExitMessage
    {
        public ServerMessage GetServerMessage(Abonent ab)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ExitMessage));
            AbonentsCommandMessage ac = new AbonentsCommandMessage() { Message = String.Empty, Type = AbonentsCommandType.Exit };

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, this);
                ac.Message = Encoding.UTF8.GetString(stream.ToArray());
            }
            return ac.GetServerMessage(ab);

        }
    }
}
