using Engine.Models;

namespace Engine.ViewModels
{
    public class GameSession
    {
        public Player CurrentPlayer { get; set; }
        public Location CurrentLocation { get; set; }
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

            CurrentLocation = new Location()
            {
                Name = "Basement Realm",
                XCoordinate = 0,
                YCoordinate = 0,
                Description  = "Dark moist seemingly endless basement",
                BackgroundImageName = "/Engine;component/Images/Locations/basement.png"

            };
        }
    }
}
