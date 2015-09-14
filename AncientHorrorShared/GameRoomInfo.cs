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
        public override bool Equals(object obj)
        {
            try
            {
                GameRoomInfo room = (GameRoomInfo)obj;
                return this.Id == room.Id;
            }
            catch 
            {
                return false;
            }
        }
        [IgnoreDataMember]
        public Boolean IsLobby
        {
            get
            {
                if (this.Id == 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
