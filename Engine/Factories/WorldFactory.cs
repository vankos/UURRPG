using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        internal static World CreateWorld()
        {
            World newWorld = new World();
            newWorld.AddLocation(0, 0, "Basement Realm", "Dark moist seemingly endless basement", "basement.png");
            newWorld.LocationAt(0, 0).AvailibleQuests.Add(QuestFactory.GetQuestByID(1));
            newWorld.LocationAt(0, 0).AddMonster(1, 100);
            newWorld.AddLocation(0, -1, "School Realm", "Fast training courses's enterprise", "school.png");
            newWorld.AddLocation(0, -2, "Hub Realm", "Busy Realm Hub", "hub.png");
            newWorld.LocationAt(0, -2).LocalTrader = TraderFactory.GetTraderByName("Broker");
            return newWorld;
        }
    }
}
