using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.ViewModels;

namespace TestEngine.ViewModels
{
    [TestClass]
    public class TestGameSession
    {
        [TestMethod]
        public void TestCreateGameSession()
        {
            GameSession gameSession = new GameSession();
            gameSession.StartTheGame();

            Assert.IsNotNull(gameSession.CurrentPlayer);
            Assert.AreEqual("Basement Realm", gameSession.CurrentLocation.Name);
        }

        [TestMethod]
        public void TestPlayerMovesToStartAndIsHealedOnKilled()
        {
            GameSession gameSession = new GameSession();
            gameSession.StartTheGame();
            gameSession.CurrentPlayer.TakeDamage(999);

            Assert.AreEqual("Basement Realm", gameSession.CurrentLocation.Name);
            Assert.AreEqual(gameSession.CurrentPlayer.Health, gameSession.CurrentPlayer.MaxHealth);
        }
    }
}
