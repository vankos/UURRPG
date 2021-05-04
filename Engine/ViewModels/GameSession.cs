using Engine.Models;
using Engine.Factories;

namespace Engine.ViewModels
{
    public class GameSession
    {
        public Player CurrentPlayer { get; set; }
        public Location CurrentLocation { get; set; }

        public World CurrentWorld { get; set; }
        public GameSession()
        {
            CurrentPlayer = new Player()
            {
                Name = "John Doe",
                CharacterClass = "Scientist",
                HitPoints = 1,
                Credits = 100,
                Experience = 0,
                Level = 1
            };

            WorldFactory factory = new WorldFactory();
            CurrentWorld = factory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, 0);
        }
    }
}
