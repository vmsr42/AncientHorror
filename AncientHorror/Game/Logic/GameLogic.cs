using AncientHorror.Game.Enviroment;
using AncientHorror.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Game.Logic
{
    public abstract class GameLogic
    {
        public abstract void InitGameObjects(List<ObjectController> controllers);
        public abstract void Run();
    }
}
