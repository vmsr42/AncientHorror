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

    }
}
