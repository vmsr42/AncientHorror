using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Game.GameObjects
{
    public class GameObject
    {
        public int Id { get; private set; }
        public GameObject(int id)
        {
            Id = id;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            GameObject second = (GameObject)obj;
            return second.Id == this.Id;
        }
        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
