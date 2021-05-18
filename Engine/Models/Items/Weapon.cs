namespace Engine.Models.Items
{
    public class Weapon : Item
    {
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }

        public Weapon(int id, string name, int price, int minDamage, int maxDamage) : base(id, name, price)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public new object Clone() => new Weapon(Id, Name, Price, MinDamage,MaxDamage);
    }
}
