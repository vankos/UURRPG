using System.Collections.Generic;

namespace Engine.Models
{
    public class World
    {
        private readonly List<Location> _locations = new List<Location>();

        internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        {
            Location location = new Location
            {
                XCoordinate = xCoordinate,
                YCoordinate = yCoordinate,
                Name = name,
                Description = description,
                BackgroundImageName = $"/Engine;component/Images/Locations/{imageName}"
            };

            _locations.Add(location);
        }

        public Location LocationAt(int x, int y) => _locations.Find((l) => (l.XCoordinate == x && l.YCoordinate == y));
    }
}
