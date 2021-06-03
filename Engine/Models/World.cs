using System.Collections.Generic;

namespace Engine.Models
{
    public class World
    {
        private readonly List<Location> _locations = new List<Location>();

        internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        {
            _locations.Add(new Location(xCoordinate, yCoordinate, name, description, $"/Engine;component/Images/Locations/{imageName}"));
        }

        public Location LocationAt(int x, int y) => _locations.Find((l) => (l.XCoordinate == x && l.YCoordinate == y));
    }
}
