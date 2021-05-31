﻿namespace Engine.Models.Items
{
    public class Weapon : Item
    {
        public int MinDamage { get; }
        public int MaxDamage { get; }

        public Weapon(int id, string name, int price, int minDamage, int maxDamage) : base(id, name, price, true)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public new object Clone() => new Weapon(Id, Name, Price, MinDamage, MaxDamage);
    }
}
