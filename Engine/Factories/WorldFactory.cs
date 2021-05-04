using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal class WorldFactory
    {
        internal World CreateWorld()
        {
            World newWorld = new World();
            newWorld.AddLocation(0, 0, "Basement Realm", "Dark moist seemingly endless basement", "/Engine;component/Images/Locations/basement.png");
            return newWorld;
        }
    }
}
