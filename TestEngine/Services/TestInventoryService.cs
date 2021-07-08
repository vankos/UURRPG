using Engine;
using Engine.Factories;
using Engine.Models.Items;
using Engine.Models.Quests;
using Engine.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestEngine.Services
{
    [TestClass]
    public class TestAttackWithWeapon
    {
        [TestMethod]
        public void Test_Init()
        {
            Inventory inventory = new Inventory();

            Assert.AreEqual(0, inventory.Items.Count);
        }

        [TestMethod]
        public void Test_AddItem()
        {
            Inventory inventory = new Inventory();
            inventory = inventory.AddItemFromFactory(1);

            Assert.AreEqual(1, inventory.Items.Count);
        }

        [TestMethod]
        public void Test_AddItems()
        {
            Inventory inventory = new Inventory();
            List<Item> itemsToAdd = new List<Item>
            {
                ItemFactory.CreateItem(1),
                ItemFactory.CreateItem(2)
            };
            inventory = inventory.AddItems(itemsToAdd);
            Assert.AreEqual(2, inventory.Items.Count);
        }

        [TestMethod]
        public void Test_AddItemQuantity()
        {
            Inventory inventory = new Inventory();
            inventory = inventory.AddItems(new List<ItemQuantity> { new ItemQuantity(1, 3) });
            Assert.AreEqual(3, inventory.Items.Count);
        }

        [TestMethod]
        public void Test_RemoveItems()
        {
            Inventory inventory = new Inventory();
            inventory = inventory.AddItemFromFactory(1);
            inventory = inventory.AddItemFromFactory(1);
            inventory = inventory.AddItemFromFactory(2);
            inventory = inventory.AddItemFromFactory(3);
            inventory = inventory.RemoveItems(new List<Item> { inventory.Items[0] });

            Assert.AreEqual(1, inventory.Items.Count(i => i.Id == 1));
        }

        [TestMethod]
        public void Test_CategorizeItems()
        {
            Inventory inventory = new Inventory();
            inventory = inventory.AddItemFromFactory(1);
            inventory = inventory.AddItemFromFactory(1);

            Assert.AreEqual(2, inventory.Weapons.Count);

            inventory = inventory.AddItemFromFactory(6);

            Assert.AreEqual(1, inventory.Consumables.Count);
            Assert.AreEqual(2, inventory.Weapons.Count);
        }

        [TestMethod]
        public void Test_RemoveItemQuantities()
        {
            Inventory inventory = new Inventory();
            inventory = inventory.AddItemFromFactory(1);
            inventory = inventory.AddItemFromFactory(1);
            inventory = inventory.AddItemFromFactory(2);
            inventory = inventory.AddItemFromFactory(3);
            inventory = inventory.RemoveItems(new List<ItemQuantity> { new ItemQuantity(1, 2) });

            Assert.AreEqual(2, inventory.Items.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_RemoveItemQuantities_TooMany()
        {
            Inventory inventory = new Inventory();
            inventory = inventory.AddItemFromFactory(1);
            inventory = inventory.AddItemFromFactory(1);
            inventory = inventory.AddItemFromFactory(2);
            inventory = inventory.AddItemFromFactory(3);
            inventory.RemoveItems(new List<ItemQuantity> { new ItemQuantity(1, 3) });
        }
    }
}
