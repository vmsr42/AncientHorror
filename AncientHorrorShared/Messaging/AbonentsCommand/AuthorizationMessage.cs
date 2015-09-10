using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorrorShared.Messaging.AbonentsCommand
{
    [DataContract]
    public class AuthorizationMessage: BaseMessage
    {
        [DataMember]
        public String Login { get; set; }
        [DataMember]
        public String Password { get; set; }


        public AuthorizationMessage() : base(new DataContractSerializer(typeof(AuthorizationMessage)),true) { }
        protected override TransportContainer TKCreation(string text)
        {
            var simmsg = new AbonentsCommandMessage() { Message = text, Type = AbonentsCommandType.Authorization, MsgId = this.MsgId };
            return simmsg.GetTC();
        }
        protected override void CopyMessageField(BaseMessage msg)
        {
            AuthorizationMessage copymsg = (AuthorizationMessage)msg;
            this.Login = copymsg.Login;
            this.Password = copymsg.Password;
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
}
