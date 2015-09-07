using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorShare
{
    [DataContract]
    public class GameAbonent
    {
        [DataMember]
        public int Id { get; set; }
        [IgnoreDataMember]
        public int UserId;
        [DataMember]
        public String Name { get; set; }
    }
}
