using Engine.Services;
using Engine.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TestEngine.Services
{
    [TestClass]
    public class TestSaveGameService
    {
        [TestMethod]
        public void TestRestore_ParserVers1_0()
        {
            GameSession gameSession = SaveGameService.LoadSavedOrCreateNewSession(@".\TestFiles\SavedGames\GameSaveParseerVer1.0.json");
            gameSession.StartTheGameRef.Invoke();

            Assert.AreEqual("1.0", gameSession.ParserVersion);
            Assert.AreEqual(0, gameSession.CurrentLocation.XCoordinate);
            Assert.AreEqual(-1, gameSession.CurrentLocation.YCoordinate);

            Assert.AreEqual("Scientist", gameSession.CurrentPlayer.CharacterClass);
            Assert.AreEqual("John Doe", gameSession.CurrentPlayer.Name);
            Assert.AreEqual(10, gameSession.CurrentPlayer.Dexterity);
            Assert.AreEqual(6, gameSession.CurrentPlayer.Health);
            Assert.AreEqual(10, gameSession.CurrentPlayer.MaxHealth);
            Assert.AreEqual(45, gameSession.CurrentPlayer.Experience);
            Assert.AreEqual(1, gameSession.CurrentPlayer.Level);
            Assert.AreEqual(112, gameSession.CurrentPlayer.Credits);

            Assert.AreEqual(1, gameSession.CurrentPlayer.Quests.Count);
            Assert.AreEqual(1, gameSession.CurrentPlayer.Quests[0].PlayerQuest.ID);
            Assert.IsTrue(gameSession.CurrentPlayer.Quests[0].IsComplete);

            Assert.AreEqual(1, gameSession.CurrentPlayer.Schemes.Count);
            Assert.AreEqual(1, gameSession.CurrentPlayer.Schemes[0].ID);

            Assert.AreEqual(7, gameSession.CurrentPlayer.Inventory.Items.Count);
            Assert.AreEqual(1, gameSession.CurrentPlayer.Inventory.Items.Count(i => i.Id == 1));
            Assert.AreEqual(1, gameSession.CurrentPlayer.Inventory.Items.Count(i => i.Id == 2));
            Assert.AreEqual(1, gameSession.CurrentPlayer.Inventory.Items.Count(i => i.Id == 6));
            Assert.AreEqual(4, gameSession.CurrentPlayer.Inventory.Items.Count(i => i.Id == 4));
        }
    }
}
