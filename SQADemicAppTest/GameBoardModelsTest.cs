using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SQADemicApp.BL;
using SQADemicApp;
using System.IO;
using SQADemicApp.UI;

namespace SQADemicAppTest
{
    [TestClass]
    public class GameBoardModelsTest
    {
        List<Card> _returnedList = new List<Card>();
        List<Card> _cardList = new List<Card>();
        //Create createTestClass = new Create();

 /**       [TestInitialize]
        public void setup()
        {
            Create.setUpCreate();
        }**/

        #region cardList
        [TestMethod]
        public void TestThatCardListCorrectOneItem()
        {
            _cardList = new List<Card>
            {
                new Card("test", Card.Cardtype.City, Color.Blue),
                new Card("Airlift", Card.Cardtype.Special),
                new Card("One Quiet Night", Card.Cardtype.Special),
                new Card("Resilient Population", Card.Cardtype.Special),
                new Card("Government Grant", Card.Cardtype.Special),
                new Card("Forecast", Card.Cardtype.Special)
            };
            _returnedList = Create.MakeCardList(new System.IO.StringReader("test; blue"));
            Assert.IsTrue(_cardList[0].Equals(_returnedList[0]));
        }
        [TestMethod]
        public void TestThatCardListCorrectTwoItem()
        {
            _cardList = new List<Card>
            {
                new Card("test", Card.Cardtype.City, Color.Blue),
                new Card("test2", Card.Cardtype.City, Color.Blue),
                new Card("Airlift", Card.Cardtype.Special),
                new Card("One Quiet Night", Card.Cardtype.Special),
                new Card("Resilient Population", Card.Cardtype.Special),
                new Card("Government Grant", Card.Cardtype.Special),
                new Card("Forecast", Card.Cardtype.Special)
            };
            _returnedList = Create.MakeCardList(new System.IO.StringReader("test; blue\ntest2; blue"));
            CollectionAssert.AreEqual(_cardList, _returnedList);
        }
        [TestMethod]
        public void TestThatCardListCorrectThreeItem()
        {
            _cardList = new List<Card>
            {
                new Card("test", Card.Cardtype.City, Color.Blue),
                new Card("test2", Card.Cardtype.City, Color.Blue),
                new Card("test3", Card.Cardtype.City, Color.Blue),
                new Card("Airlift", Card.Cardtype.Special),
                new Card("One Quiet Night", Card.Cardtype.Special),
                new Card("Resilient Population", Card.Cardtype.Special),
                new Card("Government Grant", Card.Cardtype.Special),
                new Card("Forecast", Card.Cardtype.Special)
            };
            _returnedList = Create.MakeCardList(new System.IO.StringReader("test; blue\ntest2; blue\ntest3; blue"));
            CollectionAssert.AreEqual(_cardList, _returnedList);
        }
#endregion

        #region testColor
        [TestMethod]
        public void TestThatGetsCorrectColorRed()
        {
            Assert.AreEqual(Color.Red, HelperBL.GetColor("red"));
        }
        [TestMethod]
        public void TestThatGetsCorrectColorsRedBlue()
        {
            Assert.AreEqual(Color.Red, HelperBL.GetColor("red"));
            Assert.AreEqual(Color.Blue, HelperBL.GetColor("blue"));
        }
        [TestMethod]
        public void TestThatGetsCorrectThreeColors()
        {
            Assert.AreEqual(Color.Red, HelperBL.GetColor("red"));
            Assert.AreEqual(Color.Blue, HelperBL.GetColor("blue"));
            Assert.AreEqual(Color.Black, HelperBL.GetColor("black"));
        }
        [TestMethod]
        public void TestThatGetsCorrectAllColors()
        {
            Assert.AreEqual(Color.Red, HelperBL.GetColor("red"));
            Assert.AreEqual(Color.Blue, HelperBL.GetColor("blue"));
            Assert.AreEqual(Color.Black, HelperBL.GetColor("black"));
            Assert.AreEqual(Color.Yellow, HelperBL.GetColor("yellow"));
        }
        #endregion

        #region incrementPlayerTurn
        [TestMethod]
        public void TestThatPlayerTurnIncremented()
        {
            string[] players = {"Dispatcher","Scientist"};
            var model = new GameBoardModels(players);
            model.IncTurnCount();
            Assert.AreEqual(1, model.CurrentPlayerTurnCounter);
        }
        [TestMethod]
        public void TestThatPlayerTurnsResetAfter4Moves()
        {
            string[] players = { "Dispatcher", "Scientist" };
            var model = new GameBoardModels(players);
            // four moves in a player turn
            model.IncTurnCount();
            model.IncTurnCount();
            model.IncTurnCount();
            model.IncTurnCount();
            Assert.AreEqual(0, model.CurrentPlayerTurnCounter);
        }
        #endregion

        #region nextPlayer
        
        [TestMethod]
        public void TestThatPlayerSwitches()
        {
            string[] players = { "Dispatcher", "Scientist" };
            var model = new GameBoardModels(players);
            
            // four moves in a player turn
            Assert.AreEqual(false,model.IncTurnCount());
            Assert.AreEqual(false, model.IncTurnCount());
            Assert.AreEqual(false, model.IncTurnCount());
            Assert.AreEqual(true, model.IncTurnCount());
            //Assert.AreEqual(2, GameBoardModels.CurrentPlayerIndex);
        }
        [TestMethod]
        public void TestFullPlayerRotation()
        {
            string[] players = { "Dispatcher", "Scientist" };
            var model = new GameBoardModels(players);
            // four moves in a player turn
            model.IncTurnCount();
            model.IncTurnCount();
            model.IncTurnCount();
            model.IncTurnCount();
            // moves for player 2 turn
            model.IncTurnCount();
            model.IncTurnCount();
            model.IncTurnCount();
            model.IncTurnCount();
            Assert.AreEqual(0, GameBoardModels.GetCurrentPlayerIndex());
        }
        #endregion

        #region InfectionDeck
        [TestMethod]
        public void TestInfectionDeckCorrectLength()
        {
            var r = new StringReader("New York,Montreal,Washington,London,Madrid");
            var ls = Create.MakeInfectionDeck(r);
            Assert.AreEqual(5, ls.Count);
        }

        [TestMethod]
        public void TestInfectionDeckDoesntHaveDuplicates()
        {
            var r = new StringReader("New York,Montreal,Washington,London,Madrid");
            var ls = Create.MakeInfectionDeck(r);
            var hash = new HashSet<string>(ls);
            Assert.AreEqual(5, hash.Count);
        }
        #endregion
    }
}
