using Engine.Factories;

namespace Engine.Models.Quests
{
    public struct ItemQuantity
    {
        public int ItemId { get; }
        public int Quantity { get; }

        public ItemQuantity(int itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }

        public override bool Equals(object obj)
        {
            return obj is ItemQuantity quantity &&
                   ItemId == quantity.ItemId &&
                   Quantity == quantity.Quantity;
        }

        public override int GetHashCode()
        {
            int hashCode = -1376027413;
            hashCode = (hashCode * -1521134295) + ItemId.GetHashCode();
            hashCode = (hashCode * -1521134295) + Quantity.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"{Quantity}x{ItemFactory.GetItemNameById(ItemId)}";

        public static bool operator ==(ItemQuantity left, ItemQuantity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ItemQuantity left, ItemQuantity right)
        {
            return !(left == right);
        }
    }
}
