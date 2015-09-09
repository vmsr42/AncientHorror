using AncientHorrorShared.Messaging.ConfirmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorClient.Network
{
    public class TimedConfirmMessage
    {
        private DateTime time;
        public DateTime RecievedTime
        {
            get
            {
                return time;
            }
        }
        private ServerConfirmMessage msg;
        public Boolean Confirmation
        {
            get
            {
                return msg.Accept;
            }
        }
        public TimedConfirmMessage(ServerConfirmMessage message)
        {
            this.msg = message;
            time = DateTime.Now;
        }
        public Guid Id
        {
            get
            {
                return msg.MsgId;
            }
        }
        public Guid Referer
        {
            get
            {
                return msg.RefMsgId;
            }
        }
    }
}
