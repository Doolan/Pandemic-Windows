using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQADemicApp.BL;
using SQADemicApp;
using System.IO;
using SQADemicApp.Players;


namespace SQADemicAppTest
{
    [TestClass]
    public class SpecialEventCardsTest
    {
        City chicagoCity, bangkok, kolkata, sanFran;
        LinkedList<String> deck, pile, answer;
        AbstractPlayer dispatcher;

        [TestInitialize]
        public void setUpCitiesandStations()
        {
            //set up GameboardModels if not already
            var g = new GameBoardModels(new string[] { "Dispatcher", "Medic" });
            //cities
            Create.createDictionary();
            Create.setAdjacentCities(new StringReader("Chicago;San Francisco,Los Angeles,Atlanta,Montreal"));
            Create.setAdjacentCities(new StringReader("Bangkok;Kolkata,Hong Kong,Ho Chi Minh City,Jakarta,Chennai"));
            Create.setAdjacentCities(new StringReader("Kolkata;Delhi,Chennai,Bangkok,Hong Kong"));
            Create.setAdjacentCities(new StringReader("San Francisco;Tokyo,Manila,Chicago,Los Angeles"));
            if (!Create.cityDictionary.TryGetValue("Chicago", out chicagoCity) ||
                !Create.cityDictionary.TryGetValue("Bangkok", out bangkok) ||
                !Create.cityDictionary.TryGetValue("Kolkata", out kolkata) ||
                !Create.cityDictionary.TryGetValue("San Francisco", out sanFran))
            {
                throw new InvalidOperationException("Set up failed");
            }
            //players
            dispatcher = new DispatcherPlayer();
            pile = new LinkedList<String>();
        }

        #region GovernmentGrant

        [TestMethod]
        public void TestGovernmentGrantChicago()
        {
            Assert.AreEqual(true, SpecialEventCardsBL.GovernmentGrant(chicagoCity.Name));
            Assert.AreEqual(true, chicagoCity.researchStation);
            chicagoCity.researchStation = false;
        }

        [TestMethod]
        public void TestGovernmentGrantKolkataFAILED()
        {
            //already has a research station should fail
            kolkata.researchStation = true;
            Assert.AreEqual(false, SpecialEventCardsBL.GovernmentGrant(kolkata.Name));
            Assert.AreEqual(true, kolkata.researchStation);
            kolkata.researchStation = false;
        }

        #endregion

        #region AirLift
        [TestMethod]
        public void TestAirliftBankokToChicago()
        {
            dispatcher.currentCity = bangkok;
            Assert.AreEqual(true, SpecialEventCardsBL.Airlift(dispatcher, chicagoCity));
            Assert.AreEqual(dispatcher.currentCity, chicagoCity);
        }

        [TestMethod]
        public void TestAirliftChicagoToChicagoFAIL()
        {
            dispatcher.currentCity = chicagoCity;
            Assert.AreEqual(false, SpecialEventCardsBL.Airlift(dispatcher, chicagoCity));
            Assert.AreEqual(dispatcher.currentCity, chicagoCity);
        }

        #endregion

        #region ResilientPopulation
        [TestMethod]
        public void TestRPopNewYork()
        {
            pile.Clear();
            pile.AddFirst("New York");
            pile.AddFirst("Sydney");
            pile.AddFirst("Saint Petersburg");
            answer = new LinkedList<string>();
            answer.AddFirst("New York");
            answer.AddFirst("Sydney");
            string city = "Saint Petersburg";
            Assert.AreEqual(true, SpecialEventCardsBL.ResilientPopulation(pile, city));
            CollectionAssert.AreEqual(answer, pile);
        }

        [TestMethod]
        public void TestRPopNewYorkMiddleCard()
        {
            pile.Clear();
            pile.AddFirst("New York");
            pile.AddFirst("Saint Petersburg");
            pile.AddFirst("Sydney");
            answer = new LinkedList<string>();
            answer.AddFirst("New York");
            answer.AddFirst("Sydney");
            string city = "Saint Petersburg";
            Assert.AreEqual(true, SpecialEventCardsBL.ResilientPopulation(pile, city));
            CollectionAssert.AreEqual(answer, pile);
        }

        [TestMethod]
        public void TestRPopNewYorkNotInPileFAIL()
        {
            pile.Clear();
            pile.AddFirst("New York");
            pile.AddFirst("Sydney");
            answer = new LinkedList<string>();
            answer.AddFirst("New York");
            answer.AddFirst("Sydney");
            string city = "Saint Petersburg";
            Assert.AreEqual(false, SpecialEventCardsBL.ResilientPopulation(pile, city));
            CollectionAssert.AreEqual(answer, pile);
        }
        #endregion


        #region Forecast
        [TestMethod]
        public void TestGetForcastCardsNoIssues()
        {
            deck = new LinkedList<String>(
                    new List<String> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago", "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            answer = new LinkedList<String>(
                    new List<String> { "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            List<string> returnedListAnswer = new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago" };

            List<string> returnedList = SpecialEventCardsBL.GetForcastCards(deck);
            CollectionAssert.AreEqual(returnedListAnswer, returnedList);
            CollectionAssert.AreEqual(answer, deck);

        }

        [TestMethod]
        public void TestCommitForcastNoIssues()
        {
            answer = new LinkedList<String>(
                    new List<String> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago", "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            deck = new LinkedList<String>(
                    new List<String> { "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            List<string> orderedCards = new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago" };

            Assert.AreEqual(true, SpecialEventCardsBL.CommitForcast(deck, orderedCards));
            CollectionAssert.AreEqual(answer, deck);
        }


        [TestMethod]
        public void TestCommitForcastTooManyOrFewCards()
        {
            answer = new LinkedList<String>(
                    new List<String> { "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            deck = new LinkedList<String>(
                    new List<String> { "San Francisco", "Los Angeles", "Atlanta", "Montreal" });

            //Too Few
            List<string> orderedCards1 = new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong" };
            Assert.AreEqual(false, SpecialEventCardsBL.CommitForcast(deck, orderedCards1));
            CollectionAssert.AreEqual(answer, deck);

            //Too Many
            List<string> orderedCards2 = new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "San Francisco", "Los Angeles" };
            Assert.AreEqual(false, SpecialEventCardsBL.CommitForcast(deck, orderedCards2));
            CollectionAssert.AreEqual(answer, deck);
        }

        [TestMethod]
        public void TestFocastFullCircle()
        {
            deck = new LinkedList<String>(
                new List<String> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago", "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            answer = new LinkedList<String>(
                new List<String> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago", "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            List<string> returnedList = SpecialEventCardsBL.GetForcastCards(deck);
            Assert.AreEqual(true, SpecialEventCardsBL.CommitForcast(deck, returnedList));
            CollectionAssert.AreEqual(answer, deck);
        }
        #endregion
    }
}
