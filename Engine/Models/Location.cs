﻿using System.Collections.Generic;
using Engine.Models.Quests;

namespace Engine.Models
{
    public class Location
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackgroundImageName { get; set; }

        public List<Quest> AvailibleQuests { get; set; } = new List<Quest>();
    }
}
