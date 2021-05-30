namespace Engine.Models.Items
{
    public class GroupedInventoryItem : BaseNotificationClass
    {
        private Item _item;
        private int _quantity;

        public Item Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public GroupedInventoryItem(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}
