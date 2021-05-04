using Engine.Models;

namespace Engine.Factories
{
    internal class WorldFactory
    {
        internal World CreateWorld()
        {
            World newWorld = new World();
            newWorld.AddLocation(0, 0, "Basement Realm", "Dark moist seemingly endless basement", "/Engine;component/Images/Locations/basement.png");
            newWorld.AddLocation(0, -1, "School Realm", "Fast training courses's enterprise", "/Engine;component/Images/Locations/school.png");
            newWorld.AddLocation(0, -2, "Hub Realm", "Busy Realm Hub", "/Engine;component/Images/Locations/hub.png");
            return newWorld;
        }
    }
}
