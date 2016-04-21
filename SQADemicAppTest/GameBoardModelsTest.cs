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
        List<Card> returnedList = new List<Card>();
        List<Card> cardList = new List<Card>();
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
            cardList = new List<Card>();
            cardList.Add(new Card("test", Card.CARDTYPE.City, COLOR.blue));
            cardList.Add(new Card("Airlift", Card.CARDTYPE.Special));
            cardList.Add(new Card("One Quiet Night", Card.CARDTYPE.Special));
            cardList.Add(new Card("Resilient Population", Card.CARDTYPE.Special));
            cardList.Add(new Card("Government Grant", Card.CARDTYPE.Special));
            cardList.Add(new Card("Forecast", Card.CARDTYPE.Special));
            returnedList = Create.makeCardList(new System.IO.StringReader("test; blue"));
            Assert.IsTrue(cardList[0].Equals(returnedList[0]));
        }
        [TestMethod]
        public void TestThatCardListCorrectTwoItem()
        {
            cardList = new List<Card>();
            cardList.Add(new Card("test", Card.CARDTYPE.City, COLOR.blue));
            cardList.Add(new Card("test2", Card.CARDTYPE.City, COLOR.blue));
            cardList.Add(new Card("Airlift", Card.CARDTYPE.Special));
            cardList.Add(new Card("One Quiet Night", Card.CARDTYPE.Special));
            cardList.Add(new Card("Resilient Population", Card.CARDTYPE.Special));
            cardList.Add(new Card("Government Grant", Card.CARDTYPE.Special));
            cardList.Add(new Card("Forecast", Card.CARDTYPE.Special));
            returnedList = Create.makeCardList(new System.IO.StringReader("test; blue\ntest2; blue"));
            CollectionAssert.AreEqual(cardList, returnedList);
        }
        [TestMethod]
        public void TestThatCardListCorrectThreeItem()
        {
            cardList = new List<Card>();
            cardList.Add(new Card("test", Card.CARDTYPE.City, COLOR.blue));
            cardList.Add(new Card("test2", Card.CARDTYPE.City, COLOR.blue));
            cardList.Add(new Card("test3", Card.CARDTYPE.City, COLOR.blue));
            cardList.Add(new Card("Airlift", Card.CARDTYPE.Special));
            cardList.Add(new Card("One Quiet Night", Card.CARDTYPE.Special));
            cardList.Add(new Card("Resilient Population", Card.CARDTYPE.Special));
            cardList.Add(new Card("Government Grant", Card.CARDTYPE.Special));
            cardList.Add(new Card("Forecast", Card.CARDTYPE.Special));
            returnedList = Create.makeCardList(new System.IO.StringReader("test; blue\ntest2; blue\ntest3; blue"));
            CollectionAssert.AreEqual(cardList, returnedList);
        }
#endregion

        #region testColor
        [TestMethod]
        public void TestThatGetsCorrectColorRed()
        {
            Assert.AreEqual(COLOR.red, HelperBL.getColor("red"));
        }
        [TestMethod]
        public void TestThatGetsCorrectColorsRedBlue()
        {
            Assert.AreEqual(COLOR.red, HelperBL.getColor("red"));
            Assert.AreEqual(COLOR.blue, HelperBL.getColor("blue"));
        }
        [TestMethod]
        public void TestThatGetsCorrectThreeColors()
        {
            Assert.AreEqual(COLOR.red, HelperBL.getColor("red"));
            Assert.AreEqual(COLOR.blue, HelperBL.getColor("blue"));
            Assert.AreEqual(COLOR.black, HelperBL.getColor("black"));
        }
        [TestMethod]
        public void TestThatGetsCorrectAllColors()
        {
            Assert.AreEqual(COLOR.red, HelperBL.getColor("red"));
            Assert.AreEqual(COLOR.blue, HelperBL.getColor("blue"));
            Assert.AreEqual(COLOR.black, HelperBL.getColor("black"));
            Assert.AreEqual(COLOR.yellow, HelperBL.getColor("yellow"));
        }
        #endregion

        #region incrementPlayerTurn
        [TestMethod]
        public void TestThatPlayerTurnIncremented()
        {
            string[] players = {"Dispatcher","Scientist"};
            GameBoardModels model = new GameBoardModels(players);
            model.IncTurnCount();
            Assert.AreEqual(1, model.currentPlayerTurnCounter);
        }
        [TestMethod]
        public void TestThatPlayerTurnsResetAfter4moves()
        {
            string[] players = { "Dispatcher", "Scientist" };
            GameBoardModels model = new GameBoardModels(players);
            // four moves in a player turn
            model.IncTurnCount();
            model.IncTurnCount();
            model.IncTurnCount();
            model.IncTurnCount();
            Assert.AreEqual(0, model.currentPlayerTurnCounter);
        }
        #endregion

        #region nextPlayer
        
        [TestMethod]
        public void TestThatPlayerSwitches()
        {
            string[] players = { "Dispatcher", "Scientist" };
            GameBoardModels model = new GameBoardModels(players);
            
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
            GameBoardModels model = new GameBoardModels(players);
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
            List<String> ls;
            StringReader r = new StringReader("New York,Montreal,Washington,London,Madrid");
            ls = Create.makeInfectionDeck(r);
            Assert.AreEqual(5, ls.Count);
        }

        [TestMethod]
        public void TestInfectionDeckDoesntHaveDuplicates()
        {
            List<String> ls;
            StringReader r = new StringReader("New York,Montreal,Washington,London,Madrid");
            ls = Create.makeInfectionDeck(r);
            HashSet<String> hash = new HashSet<String>(ls);
            Assert.AreEqual(5, hash.Count);
        }
        #endregion
    }
}
