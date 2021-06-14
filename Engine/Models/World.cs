using System.Collections.Generic;

namespace Engine.Models
{
    public class World
    {
        private readonly List<Location> _locations = new List<Location>();

        internal void AddLocation(Location location) => _locations.Add(location);

        public Location LocationAt(int x, int y) => _locations.Find((l) => (l.XCoordinate == x && l.YCoordinate == y));
    }
}
