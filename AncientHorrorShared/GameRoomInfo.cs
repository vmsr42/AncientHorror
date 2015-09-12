using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorShared
{
    [DataContract]
    public class GameRoomInfo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public GameAbonentInfo Owner { get; set; }
        [DataMember]
        public bool HavePasswod { get; set; }
    }
}
