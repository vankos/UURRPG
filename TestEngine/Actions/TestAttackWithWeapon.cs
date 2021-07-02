using Engine.Models.Items;
using Engine.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Actions;
using System;

namespace TestEngine.Actions
{
    [TestClass]
    public class TestAttackWithWeapon
    {
        [TestMethod]
        public void Test_Constructor_GoodParametrs()
        {
            Item pen = ItemFactory.CreateItem(1);
            AttackWithWeapon attackWithWeapon = new AttackWithWeapon(pen, 1, 5);
            Assert.IsNotNull(attackWithWeapon);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Constructor_ItemIsNotAWeapon()
        {
            Item pen = ItemFactory.CreateItem(6);
            _ = new AttackWithWeapon(pen, 1, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Constructor_MinDamageIsLessThanZero()
        {
            Item pen = ItemFactory.CreateItem(1);
            _ = new AttackWithWeapon(pen, -1, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Constructor_MaxDamageLessThanMin()
        {
            Item pen = ItemFactory.CreateItem(1);
            _ = new AttackWithWeapon(pen, 2, 1);
        }
    }
}
