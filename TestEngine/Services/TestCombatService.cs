using Engine.Models;
using Engine.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestEngine.Services
{
    [TestClass]
    public class TestCombatService
    {
        [TestMethod]
        public void Test_FirstAttacker()
        {
            Player player = new Player("", "", 0, 0, 0, 18, 0);
            Enemy enemy = new Enemy(0, "", "", 0, 12, null, 0, 0);

            CombatService.FirstAttacker(player, enemy);
        }
    }
}
