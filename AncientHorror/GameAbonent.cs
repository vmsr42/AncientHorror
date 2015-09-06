using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Server
{
    [Serializable]
    public class GameAbonent
    {
        public int Id { get; set; }
        [NonSerialized]
        public int UserId;
        public String Name { get; set; }
    }
}
