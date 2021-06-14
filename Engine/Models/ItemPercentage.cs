namespace Engine.Models
{
    public class ItemPercentage
    {
        public int ID { get; }
        public int Pecentage { get; }

        public ItemPercentage(int iD, int pecentage)
        {
            ID = iD;
            Pecentage = pecentage;
        }
    }
}
