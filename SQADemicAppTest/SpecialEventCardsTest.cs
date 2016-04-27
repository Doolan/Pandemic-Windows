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
        private City _chicagoCity;
        private City _bangkok;
        private City _kolkata;
        private City _sanFran;
        private LinkedList<string> _deck;
        private LinkedList<string> _pile;
        private LinkedList<string> _answer;
        private AbstractPlayer _dispatcher;

        [TestInitialize]
        public void SetUpCitiesandStations()
        {
            //set up GameboardModels if not already
            var g = new GameBoardModels(new string[] { "Dispatcher", "Medic" });
            //cities
            Create.CreateDictionary();
            Create.SetAdjacentCities(new StringReader("Chicago;San Francisco,Los Angeles,Atlanta,Montreal"));
            Create.SetAdjacentCities(new StringReader("Bangkok;Kolkata,Hong Kong,Ho Chi Minh City,Jakarta,Chennai"));
            Create.SetAdjacentCities(new StringReader("Kolkata;Delhi,Chennai,Bangkok,Hong Kong"));
            Create.SetAdjacentCities(new StringReader("San Francisco;Tokyo,Manila,Chicago,Los Angeles"));
            if (!Create.CityDictionary.TryGetValue("Chicago", out _chicagoCity) ||
                !Create.CityDictionary.TryGetValue("Bangkok", out _bangkok) ||
                !Create.CityDictionary.TryGetValue("Kolkata", out _kolkata) ||
                !Create.CityDictionary.TryGetValue("San Francisco", out _sanFran))
            {
                throw new InvalidOperationException("Set up failed");
            }
            //players
            _dispatcher = new DispatcherPlayer();
            _pile = new LinkedList<string>();
        }

        #region GovernmentGrant

        [TestMethod]
        public void TestGovernmentGrantChicago()
        {
            Assert.AreEqual(true, SpecialEventCardsBL.GovernmentGrant(_chicagoCity.Name));
            Assert.AreEqual(true, _chicagoCity.ResearchStation);
            _chicagoCity.ResearchStation = false;
        }

        [TestMethod]
        public void TestGovernmentGrantKolkataFAILED()
        {
            //already has a research station should fail
            _kolkata.ResearchStation = true;
            Assert.AreEqual(false, SpecialEventCardsBL.GovernmentGrant(_kolkata.Name));
            Assert.AreEqual(true, _kolkata.ResearchStation);
            _kolkata.ResearchStation = false;
        }

        #endregion

        #region AirLift
        [TestMethod]
        public void TestAirliftBankokToChicago()
        {
            _dispatcher.CurrentCity = _bangkok;
            Assert.AreEqual(true, SpecialEventCardsBL.Airlift(_dispatcher, _chicagoCity));
            Assert.AreEqual(_dispatcher.CurrentCity, _chicagoCity);
        }

        [TestMethod]
        public void TestAirliftChicagoToChicagoFail()
        {
            _dispatcher.CurrentCity = _chicagoCity;
            Assert.AreEqual(false, SpecialEventCardsBL.Airlift(_dispatcher, _chicagoCity));
            Assert.AreEqual(_dispatcher.CurrentCity, _chicagoCity);
        }

        #endregion

        #region ResilientPopulation
        [TestMethod]
        public void TestRPopNewYork()
        {
            _pile.Clear();
            _pile.AddFirst("New York");
            _pile.AddFirst("Sydney");
            _pile.AddFirst("Saint Petersburg");
            _answer = new LinkedList<string>();
            _answer.AddFirst("New York");
            _answer.AddFirst("Sydney");
            const string city = "Saint Petersburg";
            Assert.AreEqual(true, SpecialEventCardsBL.ResilientPopulation(_pile, city));
            CollectionAssert.AreEqual(_answer, _pile);
        }

        [TestMethod]
        public void TestRPopNewYorkMiddleCard()
        {
            _pile.Clear();
            _pile.AddFirst("New York");
            _pile.AddFirst("Saint Petersburg");
            _pile.AddFirst("Sydney");
            _answer = new LinkedList<string>();
            _answer.AddFirst("New York");
            _answer.AddFirst("Sydney");
            const string city = "Saint Petersburg";
            Assert.AreEqual(true, SpecialEventCardsBL.ResilientPopulation(_pile, city));
            CollectionAssert.AreEqual(_answer, _pile);
        }

        [TestMethod]
        public void TestRPopNewYorkNotInPileFail()
        {
            _pile.Clear();
            _pile.AddFirst("New York");
            _pile.AddFirst("Sydney");
            _answer = new LinkedList<string>();
            _answer.AddFirst("New York");
            _answer.AddFirst("Sydney");
            const string city = "Saint Petersburg";
            Assert.AreEqual(false, SpecialEventCardsBL.ResilientPopulation(_pile, city));
            CollectionAssert.AreEqual(_answer, _pile);
        }
        #endregion


        #region Forecast
        [TestMethod]
        public void TestGetForcastCardsNoIssues()
        {
            _deck = new LinkedList<string>(
                    new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago", "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            _answer = new LinkedList<string>(
                    new List<string> { "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            var returnedListAnswer = new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago" };

            var returnedList = SpecialEventCardsBL.GetForcastCards(_deck);
            CollectionAssert.AreEqual(returnedListAnswer, returnedList);
            CollectionAssert.AreEqual(_answer, _deck);

        }

        [TestMethod]
        public void TestCommitForcastNoIssues()
        {
            _answer = new LinkedList<string>(
                    new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago", "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            _deck = new LinkedList<string>(
                    new List<string> { "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            var orderedCards = new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago" };

            Assert.AreEqual(true, SpecialEventCardsBL.CommitForcast(_deck, orderedCards));
            CollectionAssert.AreEqual(_answer, _deck);
        }


        [TestMethod]
        public void TestCommitForcastTooManyOrFewCards()
        {
            _answer = new LinkedList<string>(
                    new List<string> { "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            _deck = new LinkedList<string>(
                    new List<string> { "San Francisco", "Los Angeles", "Atlanta", "Montreal" });

            //Too Few
            var orderedCards1 = new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong" };
            Assert.AreEqual(false, SpecialEventCardsBL.CommitForcast(_deck, orderedCards1));
            CollectionAssert.AreEqual(_answer, _deck);

            //Too Many
            var orderedCards2 = new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "San Francisco", "Los Angeles" };
            Assert.AreEqual(false, SpecialEventCardsBL.CommitForcast(_deck, orderedCards2));
            CollectionAssert.AreEqual(_answer, _deck);
        }

        [TestMethod]
        public void TestFocastFullCircle()
        {
            _deck = new LinkedList<string>(
                new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago", "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            _answer = new LinkedList<string>(
                new List<string> { "Kolkata", "Delhi", "Chennai", "Bangkok", "Hong Kong", "Chicago", "San Francisco", "Los Angeles", "Atlanta", "Montreal" });
            var returnedList = SpecialEventCardsBL.GetForcastCards(_deck);
            Assert.AreEqual(true, SpecialEventCardsBL.CommitForcast(_deck, returnedList));
            CollectionAssert.AreEqual(_answer, _deck);
        }
        #endregion
    }
}
