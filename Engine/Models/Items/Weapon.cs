namespace Engine.Models.Items
{
    public class Weapon : Item
    {
        public double MinDamage { get; set; }
        public double MaxDamage { get; set; }

        public Weapon(int id, string name, int price, double minDamage, double maxDamage) : base(id, name, price)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public new object Clone() => new Weapon(Id, Name, Price, MinDamage,MaxDamage);
    }
}
