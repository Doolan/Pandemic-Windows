﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SQADemicApp.BL;
//using SQADemicApp;
//using System.IO;

//namespace SQADemicAppTest
//{
//    [TestClass]
//    public class PlayerActionsTest
//    {
//        City _chicagoCity, _bangkok, _kolkata, _sanFran;
//        List<Cards> _hand, _pile;
//        List<Player> _players;
//        Card _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _milan, _airlift;
//        Player _dispatcher, _medic, _opExpert, _researcher, _scientist;


//        [TestInitialize]
//        public void SetupPlayer()
//        {
//            //set up GameboardModels if not already
//            var g = new GameBoardModels(new string[] { "Dispatcher", "Medic" });
//            //cities
//            Create.CreateDictionary();
//            Create.SetAdjacentCities(new StringReader("Chicago;San Francisco,Los Angeles,Atlanta,Montreal"));
//            Create.SetAdjacentCities(new StringReader("Bangkok;Kolkata,Hong Kong,Ho Chi Minh City,Jakarta,Chennai"));
//            Create.SetAdjacentCities(new StringReader("Kolkata;Delhi,Chennai,Bangkok,Hong Kong"));
//            Create.SetAdjacentCities(new StringReader("San Francisco;Tokyo,Manila,Chicago,Los Angeles"));
//            if (!Create.CityDictionary.TryGetValue("Chicago", out _chicagoCity) ||
//                !Create.CityDictionary.TryGetValue("Bangkok", out _bangkok) ||
//                !Create.CityDictionary.TryGetValue("Kolkata", out _kolkata) ||
//                !Create.CityDictionary.TryGetValue("San Francisco", out _sanFran))
//            {
//                throw new InvalidOperationException("Set up failed");
//            }
//            //Cards
//            _chennai = new Card("Chennai", Card.Cardtype.City, Color.Black);
//            _newYork = new Card("New York", Card.Cardtype.City, Color.Blue);
//            _atlanta = new Card("Atlanta", Card.Cardtype.City, Color.Blue);
//            _chicagoCard = new Card("Chicago", Card.Cardtype.City, Color.Blue);
//            _paris = new Card("Paris", Card.Cardtype.City, Color.Blue);
//            _london = new Card("London", Card.Cardtype.City, Color.Blue);
//            _milan = new Card("Milan", Card.Cardtype.City, Color.Blue);
//            _airlift = new Card("Airlift", Card.Cardtype.Special);

//            //Players
//            _scientist = new Player(Role.Scientist);
//            _opExpert = new Player(Role.OpExpert);
//            _researcher = new Player(Role.Researcher);
//            _medic = new Player(Role.Medic);
//            _players = new List<Player> { _scientist, _opExpert, _researcher, _medic };
//        }

//        [TestMethod]
//        public void TestDriveOptions()
//        {
//            HashSet<City> returnedSet = PlayerActionsBl.DriveOptions(_chicagoCity);
//            HashSet<City> correctSet = _chicagoCity.GetAdjacentCities();
//            CollectionAssert.AreEqual(returnedSet.ToArray(), correctSet.ToArray());
//        }

//        #region Direct Flight
//        [TestMethod]
//        public void TestDirectFlightOptionsNone()
//        {
//            _hand = new List<Cards>();
//            List<string> returnList = PlayerActionsBl.DirectFlightOption(_hand, _chicagoCity);
//            List<string> correctList = new List<string>();
//            CollectionAssert.AreEqual(correctList, returnList);
//        }

//        [TestMethod]
//        public void TestDirectFlightOptionNewYork()
//        {
//            _hand = new List<Cards> { _newYork };
//            List<string> returnList = PlayerActionsBl.DirectFlightOption(_hand, _chicagoCity);
//            List<string> correctList = new List<string> { _newYork.CityName };
//            CollectionAssert.AreEqual(correctList, returnList);
//        }

//        [TestMethod]
//        public void TestDirectFlightCurrentCityChicago()
//        {
//            _hand = new List<Cards> { _chicagoCard };
//            List<string> returnList = PlayerActionsBl.DirectFlightOption(_hand, _chicagoCity);
//            List<string> correctList = new List<string>();
//            CollectionAssert.AreEqual(correctList, returnList);
//        }

//        [TestMethod]
//        public void TestDirectFlightMultipleCities()
//        {
//            _hand = new List<Cards> { _chicagoCard, _atlanta, _chennai };
//            List<string> returnList = PlayerActionsBl.DirectFlightOption(_hand, _chicagoCity);
//            List<string> correctList = new List<string> { _atlanta.CityName, _chennai.CityName };
//            CollectionAssert.AreEqual(correctList, returnList);
//        }

//        [TestMethod]
//        public void TestDirectFlightWithNonCityCardInHand()
//        {
//            _hand = new List<Card> { _airlift, _atlanta, _chennai };
//            List<string> returnList = PlayerActionsBl.DirectFlightOption(_hand, _chicagoCity);
//            List<string> correctList = new List<string> { _atlanta.CityName, _chennai.CityName };
//            CollectionAssert.AreEqual(correctList, returnList);

//        }
//        #endregion

//        #region Charter Flight
//        [TestMethod]
//        public void TestCharterFlightFalseOption()
//        {
//            _hand = new List<Card> { _airlift, _atlanta, _chennai };
//            bool returendBool = PlayerActionsBl.CharterFlightOption(_hand, _chicagoCity);
//            bool correctBool = false;
//            Assert.AreEqual(correctBool, returendBool);
//        }

//        [TestMethod]
//        public void TestCharterFlightTrue()
//        {
//            _hand = new List<Card> { _airlift, _atlanta, _chicagoCard };
//            bool returendBool = PlayerActionsBl.CharterFlightOption(_hand, _chicagoCity);
//            bool correctBool = true;
//            Assert.AreEqual(correctBool, returendBool);
//        }
//        #endregion

//        #region Shuttle Flight
//        [TestMethod]
//        public void TestShuttleFlightNotInStation()
//        {
//            _scientist.CurrentCity = _kolkata;
//            List<string> result = PlayerActionsBl.ShuttleFlightOption(_kolkata);
//            CollectionAssert.Equals(result, new List<string>());
//        }

//        [TestMethod]
//        public void TestShuttleFlightNoOptions()
//        {
//            _scientist.CurrentCity = _kolkata;
//            _kolkata.ResearchStation = true;
//            List<string> result = PlayerActionsBl.ShuttleFlightOption(_kolkata);
//            _kolkata.ResearchStation = false;
//            List<string> expected = new List<string> { "Atlanta" };
//            CollectionAssert.AreEqual(expected, result);
//        }

//        [TestMethod]
//        public void TestShuttleFlightOneOption()
//        {
//            _scientist.CurrentCity = _kolkata;
//            _kolkata.ResearchStation = true;
//            _bangkok.ResearchStation = true;
//            List<string> result = PlayerActionsBl.ShuttleFlightOption(_kolkata);
//            _kolkata.ResearchStation = false;
//            _bangkok.ResearchStation = false;
//            List<string> expected = new List<string> { "Atlanta", "Bangkok" };
//            CollectionAssert.AreEqual(expected, result);
//        }

//        [TestMethod]
//        public void TestShuttleFlightMultipleOptions()
//        {
//            _scientist.CurrentCity = _chicagoCity;
//            _chicagoCity.Name = "Chicago";
//            _chicagoCity.ResearchStation = true;
//            _kolkata.ResearchStation = true;
//            _bangkok.ResearchStation = true;
//            List<string> result = PlayerActionsBl.ShuttleFlightOption(_chicagoCity);
//            _chicagoCity.ResearchStation = false;
//            _kolkata.ResearchStation = false;
//            _bangkok.ResearchStation = false;
//            List<string> expected = new List<string> { "Atlanta", "Kolkata", "Bangkok" };
//            CollectionAssert.AreEqual(expected, result);
//        }
//        #endregion

//        #region Move Player
//        [TestMethod]
//        public void TestMoverPlayerAdjacentCitySanFran()
//        {
//            _scientist.CurrentCity = _chicagoCity;
//            _hand = new List<Card> { _airlift, _chicagoCard, _chennai };
//            _scientist.Hand = _hand;
//            PlayerActionsBl.Moveplayer(_scientist, _sanFran);
//            Assert.AreEqual(_scientist.CurrentCity.Name, _sanFran.Name);
//        }

//        [TestMethod]
//        public void TestMoverPlayerAdjacentCityChicagoWithCard()
//        {
//            _scientist.CurrentCity = _sanFran;
//            _hand = new List<Card> { _airlift, _chicagoCard, _chennai };
//            List<Card> correctHand = new List<Card> { _airlift, _chicagoCard, _chennai };
//            _scientist.Hand = _hand;
//            PlayerActionsBl.Moveplayer(_scientist, _chicagoCity);
//            Assert.AreEqual(_scientist.CurrentCity.Name, _chicagoCity.Name);
//            CollectionAssert.AreEqual(correctHand, _hand);
//        }

//        [TestMethod]
//        public void TestMoverPlayerDirectFlight()
//        {
//            _scientist.CurrentCity = _bangkok;
//            _hand = new List<Card> { _airlift, _chicagoCard, _chennai, _atlanta };
//            _pile = new List<Card>();
//            _scientist.Hand = _hand;
//            PlayerActionsBl.Moveplayer(_scientist, _chicagoCity);
//            List<Card> correctHand = new List<Card> { _airlift, _chennai, _atlanta };
//            Assert.AreEqual(_scientist.CurrentCity.Name, _chicagoCity.Name);
//            CollectionAssert.AreEqual(correctHand, _hand);
//        }

//        [TestMethod]
//        public void TestMoverPlayerCharterFlight()
//        {
//            _scientist.CurrentCity = _chicagoCity;
//            _hand = new List<Card> { _airlift, _chicagoCard, _chennai };
//            _pile = new List<Card>();
//            _scientist.Hand = _hand;
//            PlayerActionsBl.Moveplayer(_scientist, _bangkok);
//            List<Card> correctHand = new List<Card> { _airlift, _chennai };
//            Assert.AreEqual(_scientist.CurrentCity.Name, _bangkok.Name);
//            CollectionAssert.AreEqual(correctHand, _hand);
//        }

//        [TestMethod]
//        public void TestMoverPlayerShuttleFlightPreemptCard()
//        {
//            _chicagoCity.ResearchStation = true;
//            _bangkok.ResearchStation = true;
//            _scientist.CurrentCity = _chicagoCity;
//            _hand = new List<Card> { _airlift, _atlanta, _chennai, _chicagoCard };
//            _pile = new List<Card>();
//            _scientist.Hand = _hand;
//            Assert.AreEqual(true, PlayerActionsBl.Moveplayer(_scientist, _bangkok));
//            List<Card> correctHand = new List<Card> { _airlift, _atlanta, _chennai, _chicagoCard };
//            Assert.AreEqual(_scientist.CurrentCity.Name, _bangkok.Name);
//            CollectionAssert.AreEqual(correctHand, _hand);
//            _chicagoCity.ResearchStation = false;
//            _bangkok.ResearchStation = false;
//        }

//        [TestMethod]
//        public void TestMoverPlayerInvalid()
//        {
//            _scientist.CurrentCity = _chicagoCity;
//            _hand = new List<Card> { _airlift, _atlanta, _chennai };
//            _pile = new List<Card>();
//            _scientist.Hand = _hand;
//            Assert.AreEqual(false, PlayerActionsBl.Moveplayer(_scientist, _bangkok));
//            List<Card> correctHand = new List<Card> { _airlift, _atlanta, _chennai };
//            Assert.AreEqual(_scientist.CurrentCity.Name, _chicagoCity.Name);
//            CollectionAssert.AreEqual(correctHand, _hand);
//        }

//        #endregion

//        #region BuildAResearchStation

//        [TestMethod]
//        public void BuildStationNormal()
//        {
//            _scientist.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = false;
//            _hand = new List<Card> { _airlift, _chicagoCard, _chennai };
//            _scientist.Hand = _hand;
//            List<Card> correctHand = new List<Card> { _airlift, _chennai };
//            Assert.AreEqual(true, PlayerActionsBl.BuildAResearchStationOption(_scientist));
//            CollectionAssert.AreEqual(correctHand, _hand);
//            Assert.AreEqual(true, _chicagoCity.ResearchStation);
//            _chicagoCity.ResearchStation = false;
//        }

//        [TestMethod]
//        public void BuildStationFailLackCard()
//        {
//            _scientist.CurrentCity = _chicagoCity;
//            _hand = new List<Card> { _airlift, _chennai };
//            _scientist.Hand = _hand;
//            List<Card> correctHand = new List<Card> { _airlift, _chennai };
//            Assert.AreEqual(false, PlayerActionsBl.BuildAResearchStationOption(_scientist));
//            CollectionAssert.AreEqual(correctHand, _hand);
//        }

//        [TestMethod]
//        public void BuildStationFailExisting()
//        {
//            _scientist.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = true;
//            _hand = new List<Card> { _airlift, _chicagoCard, _chennai };
//            _scientist.Hand = _hand;
//            List<Card> correctHand = new List<Card> { _airlift, _chicagoCard, _chennai };
//            Assert.AreEqual(false, PlayerActionsBl.BuildAResearchStationOption(_scientist));
//            CollectionAssert.AreEqual(correctHand, _hand);
//            _chicagoCity.ResearchStation = false;
//        }

//        [TestMethod]
//        public void BuildStationOpExpert()
//        {
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = false;
//            _hand = new List<Card> { _airlift, _chicagoCard, _chennai };
//            _opExpert.Hand = _hand;
//            List<Card> correctHand = new List<Card> { _airlift, _chicagoCard, _chennai };
//            Assert.AreEqual(true, PlayerActionsBl.BuildAResearchStationOption(_opExpert));
//            CollectionAssert.AreEqual(correctHand, _hand);
//            Assert.AreEqual(true, _chicagoCity.ResearchStation);
//            _chicagoCity.ResearchStation = false;
//        }

//        [TestMethod]
//        public void BuildStationOpExpertWithoutCard()
//        {
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = false;
//            _hand = new List<Card> { _airlift, _chennai };
//            _opExpert.Hand = _hand;
//            List<Card> correctHand = new List<Card> { _airlift, _chennai };
//            Assert.AreEqual(true, PlayerActionsBl.BuildAResearchStationOption(_opExpert));
//            CollectionAssert.AreEqual(correctHand, _hand);
//            Assert.AreEqual(true, _chicagoCity.ResearchStation);
//            _chicagoCity.ResearchStation = false;
//        }

//        #endregion

//        #region Cure
//        //To test:
//        //DONE: Not enough cards
//        //TODO: already cured
//        //DONE: enough cards 5 cards
//        //DONE: enougn cured with scientist 4 cards
//        //DONE: not at reasearch center
//        //DONE: Test too many blue card

//        [TestMethod]
//        public void TestCureSimple()
//        {
//            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
//            _opExpert.Hand = _hand;
//            List<string> cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName };
//            List<Card> correctHand = new List<Card> { _chennai, _airlift };
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = true;
//            Assert.AreEqual(true, PlayerActionsBl.Cure(_opExpert, cardsToSpend, Color.Blue));
//            CollectionAssert.AreEqual(correctHand, _hand);
//            _chicagoCity.ResearchStation = false;
//            Assert.AreEqual(GameBoardModels.Curestatus.GetCureStatus(Color.Blue), GameBoardModels.Cures.Curestate.Cured);
//            GameBoardModels.Curestatus.SetCureStatus(Color.Blue, GameBoardModels.Cures.Curestate.NotCured);

//        }

//        [TestMethod]
//        public void TestCureNotEnoughCards()
//        {
//            _hand = new List<Card> { _newYork, _chennai, _atlanta, _chicagoCard, _airlift };
//            _opExpert.Hand = _hand;
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = true;
//            List<string> cardsToSpend = new List<string> { _newYork.CityName, _chicagoCard.CityName };
//            List<Card> correctHand = new List<Card> { _newYork, _chennai, _atlanta, _chicagoCard, _airlift };
//            Assert.AreEqual(false, PlayerActionsBl.Cure(_opExpert, cardsToSpend, Color.Blue));
//            CollectionAssert.AreEqual(correctHand, _hand);
//            _chicagoCity.ResearchStation = false;
//        }

//        [TestMethod]
//        public void TestCureToManyValidCards()
//        {
//            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _milan, _airlift };
//            _opExpert.Hand = _hand;
//            List<string> cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName, _milan.CityName };
//            List<Card> correctHand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _milan, _airlift };
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = true;
//            Assert.AreEqual(false, PlayerActionsBl.Cure(_opExpert, cardsToSpend, Color.Blue));
//            CollectionAssert.AreEqual(correctHand, _hand);
//            _chicagoCity.ResearchStation = false;
//        }

//        [TestMethod]
//        public void TestCureInvalidCards()
//        {
//            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
//            _opExpert.Hand = _hand;
//            List<string> cardsToSpend = new List<string> { _chennai.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName };
//            List<Card> correctHand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = true;
//            Assert.AreEqual(false, PlayerActionsBl.Cure(_opExpert, cardsToSpend, Color.Blue));
//            CollectionAssert.AreEqual(correctHand, _hand);
//            _chicagoCity.ResearchStation = false;
//        }

//        [TestMethod]
//        public void TestCureNotInResearchStation()
//        {
//            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
//            _opExpert.Hand = _hand;
//            List<string> cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName };
//            List<Card> correctHand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = false;
//            Assert.AreEqual(false, PlayerActionsBl.Cure(_opExpert, cardsToSpend, Color.Blue));
//            CollectionAssert.AreEqual(correctHand, _hand);
//        }

//        [TestMethod]
//        public void TestCureSimpleScientist()
//        {
//            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
//            _scientist.Hand = _hand;
//            List<string> cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName };
//            List<Card> correctHand = new List<Card> { _chennai, _paris, _airlift };
//            _scientist.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = true;
//            Assert.AreEqual(true, PlayerActionsBl.Cure(_scientist, cardsToSpend, Color.Blue));
//            CollectionAssert.AreEqual(correctHand, _hand);
//            _chicagoCity.ResearchStation = false;
//            Assert.AreEqual(GameBoardModels.Curestatus.GetCureStatus(Color.Blue), GameBoardModels.Cures.Curestate.Cured);
//            GameBoardModels.Curestatus.SetCureStatus(Color.Blue, GameBoardModels.Cures.Curestate.NotCured);
//        }

//        [TestMethod]
//        public void TestCureToManyValidCardsScientist()
//        {
//            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
//            _scientist.Hand = _hand;
//            List<string> cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName };
//            List<Card> correctHand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
//            _scientist.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = true;
//            Assert.AreEqual(false, PlayerActionsBl.Cure(_scientist, cardsToSpend, Color.Blue));
//            CollectionAssert.AreEqual(correctHand, _hand);
//            _chicagoCity.ResearchStation = false;
//        }

//        [TestMethod]
//        public void TestCureSimpleAlreadyCured()
//        {
//            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
//            _opExpert.Hand = _hand;
//            List<string> cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName };
//            List<Card> correctHand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.ResearchStation = true;
//            GameBoardModels.Curestatus.BlueCure = GameBoardModels.Cures.Curestate.Cured;
//            Assert.AreEqual(false, PlayerActionsBl.Cure(_opExpert, cardsToSpend, Color.Blue));
//            CollectionAssert.AreEqual(correctHand, _hand);
//            _chicagoCity.ResearchStation = false;
//            GameBoardModels.Curestatus.BlueCure = GameBoardModels.Cures.Curestate.NotCured;

//        }


//        #endregion

//        #region Treat Diseases

//        [TestMethod]
//        public void TestTreatDiseaseBasicBlue()
//        {
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.BlueCubes = 2;
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Blue));
//            Assert.AreEqual(_chicagoCity.BlueCubes, 1);
//        }

//        [TestMethod]
//        public void TestTreatDiseaseBasicRed()
//        {
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.RedCubes = 2;
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Red));
//            Assert.AreEqual(_chicagoCity.RedCubes, 1);
//        }

//        [TestMethod]
//        public void TestTreatDiseaseBasicOnlYellow()
//        {
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.RedCubes = 2;
//            _chicagoCity.BlueCubes = 2;
//            _chicagoCity.YellowCubes = 2;
//            _chicagoCity.BlackCubes = 3;
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Yellow));
//            Assert.AreEqual(_chicagoCity.RedCubes, 2);
//            Assert.AreEqual(_chicagoCity.BlueCubes, 2);
//            Assert.AreEqual(_chicagoCity.YellowCubes, 1);
//            Assert.AreEqual(_chicagoCity.BlackCubes, 3);
//        }

//        [TestMethod]
//        public void TestTreatDiseaseBasicDecreaseAll()
//        {
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.RedCubes = 1;
//            _chicagoCity.BlueCubes = 2;
//            _chicagoCity.YellowCubes = 2;
//            _chicagoCity.BlackCubes = 1;
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Red));
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Blue));
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Yellow));
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Black));
//            Assert.AreEqual(_chicagoCity.RedCubes, 0);
//            Assert.AreEqual(_chicagoCity.BlueCubes, 1);
//            Assert.AreEqual(_chicagoCity.YellowCubes, 1);
//            Assert.AreEqual(_chicagoCity.BlackCubes, 0);
//        }

//        [TestMethod]
//        public void TestTreateDiesaseCuresExist()
//        {
//            _opExpert.CurrentCity = _chicagoCity;
//            _chicagoCity.RedCubes = 1;
//            _chicagoCity.BlueCubes = 2;
//            _chicagoCity.YellowCubes = 2;
//            _chicagoCity.BlackCubes = 3;
//            GameBoardModels.Curestatus.BlackCure = GameBoardModels.Cures.Curestate.Cured;
//            GameBoardModels.Curestatus.BlueCure = GameBoardModels.Cures.Curestate.Cured;
//            GameBoardModels.Curestatus.RedCure = GameBoardModels.Cures.Curestate.Cured;
//            GameBoardModels.Curestatus.YellowCure = GameBoardModels.Cures.Curestate.Cured;
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Red));
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Blue));
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Yellow));
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Black));
//            GameBoardModels.Curestatus.BlackCure = GameBoardModels.Cures.Curestate.NotCured;
//            GameBoardModels.Curestatus.BlueCure = GameBoardModels.Cures.Curestate.NotCured;
//            GameBoardModels.Curestatus.RedCure = GameBoardModels.Cures.Curestate.NotCured;
//            GameBoardModels.Curestatus.YellowCure = GameBoardModels.Cures.Curestate.NotCured;
//            Assert.AreEqual(_chicagoCity.RedCubes, 0);
//            Assert.AreEqual(_chicagoCity.BlueCubes, 0);
//            Assert.AreEqual(_chicagoCity.YellowCubes, 0);
//            Assert.AreEqual(_chicagoCity.BlackCubes, 0);
//        }

//        [TestMethod]
//        public void TestTreatDiseaseMedicDecreaseAll()
//        {
//            _medic.CurrentCity = _chicagoCity;
//            _chicagoCity.RedCubes = 1;
//            _chicagoCity.BlueCubes = 2;
//            _chicagoCity.YellowCubes = 3;
//            _chicagoCity.BlackCubes = 1;
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_medic, Color.Red));
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_medic, Color.Blue));
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_medic, Color.Yellow));
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_medic, Color.Black));
//            Assert.AreEqual(_chicagoCity.RedCubes, 0);
//            Assert.AreEqual(_chicagoCity.BlueCubes, 0);
//            Assert.AreEqual(_chicagoCity.YellowCubes, 0);
//            Assert.AreEqual(_chicagoCity.BlackCubes, 0);
//        }

//        [TestMethod]
//        public void TestTreatDiseaseZero()
//        {
//            _opExpert.CurrentCity = _chicagoCity;
//            _medic.CurrentCity = _chicagoCity;
//            _chicagoCity.RedCubes = 0;
//            _chicagoCity.BlueCubes = 0;
//            _chicagoCity.YellowCubes = 0;
//            _chicagoCity.BlackCubes = 0;
//            Assert.AreEqual(false, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Red));
//            Assert.AreEqual(false, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Blue));
//            Assert.AreEqual(false, PlayerActionsBl.TreatDiseaseOption(_medic, Color.Yellow));
//            Assert.AreEqual(false, PlayerActionsBl.TreatDiseaseOption(_medic, Color.Black));
//            Assert.AreEqual(_chicagoCity.RedCubes, 0);
//            Assert.AreEqual(_chicagoCity.BlueCubes, 0);
//            Assert.AreEqual(_chicagoCity.YellowCubes, 0);
//            Assert.AreEqual(_chicagoCity.BlackCubes, 0);
//        }

//        #endregion

//        #region TradeCards
//        [TestMethod]
//        public void TestShareChicagosimple()
//        {
//            List<Card> hand1 = new List<Card> { _chennai, _newYork, _chicagoCard };
//            List<Card> hand2 = new List<Card> { _atlanta, _london };
//            _scientist.Hand = new List<Card> { _chennai, _newYork };
//            _opExpert.Hand = new List<Card> { _atlanta, _london, _chicagoCard };
//            _scientist.CurrentCity = _chicagoCity;
//            _opExpert.CurrentCity = _chicagoCity;
//            Assert.AreEqual(true, PlayerActionsBl.ShareKnowledgeOption(_opExpert, _scientist, _chicagoCard.CityName));
//            CollectionAssert.AreEqual(_scientist.Hand, hand1);
//            CollectionAssert.AreEqual(_opExpert.Hand, hand2);
//        }

//        [TestMethod]
//        public void TestShareChicagoDiffrentCityFail()
//        {
//            List<Card> hand1 = new List<Card> { _chennai, _newYork };
//            List<Card> hand2 = new List<Card> { _atlanta, _london, _chicagoCard };
//            _scientist.Hand = new List<Card> { _chennai, _newYork };
//            _opExpert.Hand = new List<Card> { _atlanta, _london, _chicagoCard };
//            _scientist.CurrentCity = _chicagoCity;
//            _opExpert.CurrentCity = _bangkok;
//            Assert.AreEqual(false, PlayerActionsBl.ShareKnowledgeOption(_opExpert, _scientist, _chicagoCard.CityName));
//            CollectionAssert.AreEqual(_scientist.Hand, hand1);
//            CollectionAssert.AreEqual(_opExpert.Hand, hand2);
//        }


//        [TestMethod]
//        public void TestShareChicagoinBangkokFail()
//        {
//            List<Card> hand1 = new List<Card> { _chennai, _newYork };
//            List<Card> hand2 = new List<Card> { _atlanta, _london, _chicagoCard };
//            _scientist.Hand = new List<Card> { _chennai, _newYork };
//            _opExpert.Hand = new List<Card> { _atlanta, _london, _chicagoCard };
//            _scientist.CurrentCity = _bangkok;
//            _opExpert.CurrentCity = _bangkok;
//            Assert.AreEqual(false, PlayerActionsBl.ShareKnowledgeOption(_opExpert, _scientist, _chicagoCard.CityName));
//            CollectionAssert.AreEqual(_scientist.Hand, hand1);
//            CollectionAssert.AreEqual(_opExpert.Hand, hand2);
//        }

//        [TestMethod]
//        public void TestShareChicagoinBangkokResearcherPass()
//        {
//            List<Card> hand1 = new List<Card> { _chennai, _newYork, _chicagoCard };
//            List<Card> hand2 = new List<Card> { _atlanta, _london };
//            _scientist.Hand = new List<Card> { _chennai, _newYork };
//            _researcher.Hand = new List<Card> { _atlanta, _london, _chicagoCard };
//            _scientist.CurrentCity = _bangkok;
//            _researcher.CurrentCity = _bangkok;
//            Assert.AreEqual(true, PlayerActionsBl.ShareKnowledgeOption(_researcher, _scientist, _chicagoCard.CityName));
//            CollectionAssert.AreEqual(_scientist.Hand, hand1);
//            CollectionAssert.AreEqual(_researcher.Hand, hand2);
//        }

//        [TestMethod]
//        public void TestShareChicagoMissingCardFail()
//        {
//            List<Card> hand1 = new List<Card> { _chennai, _newYork };
//            List<Card> hand2 = new List<Card> { _atlanta, _london };
//            _scientist.Hand = new List<Card> { _chennai, _newYork };
//            _opExpert.Hand = new List<Card> { _atlanta, _london };
//            _scientist.CurrentCity = _chicagoCity;
//            _opExpert.CurrentCity = _chicagoCity;
//            Assert.AreEqual(false, PlayerActionsBl.ShareKnowledgeOption(_opExpert, _scientist, _chicagoCard.CityName));
//            CollectionAssert.AreEqual(_scientist.Hand, hand1);
//            CollectionAssert.AreEqual(_opExpert.Hand, hand2);
//        }

//        #endregion

//        #region DispatcherMove

//        [TestMethod]
//        public void TestDispatcherMoveAdjacentCitySanFran()
//        {
//            _scientist.CurrentCity = _chicagoCity;
//            Assert.AreEqual(PlayerActionsBl.DispatcherMovePlayer(_scientist, _players, _sanFran), true);
//            Assert.AreEqual(_scientist.CurrentCity.Name, _sanFran.Name);
//        }


//        [TestMethod]
//        public void TestDispatcherMoveInvalidCityKolkata()
//        {
//            _scientist.CurrentCity = _chicagoCity;
//            Assert.AreEqual(PlayerActionsBl.DispatcherMovePlayer(_scientist, _players, _kolkata), false);
//            Assert.AreEqual(_scientist.CurrentCity.Name, _chicagoCity.Name);
//        }

//        [TestMethod]
//        public void TestDispatcherMoveToOtherPlayer()
//        {
//            _scientist.CurrentCity = _chicagoCity;
//            _opExpert.CurrentCity = _bangkok;
//            Assert.AreEqual(PlayerActionsBl.DispatcherMovePlayer(_scientist, _players, _bangkok), true);
//            Assert.AreEqual(_scientist.CurrentCity.Name, _bangkok.Name);
//            _opExpert.CurrentCity = _chicagoCity;
//        }

//        [TestMethod]
//        public void TestDispatcherMoveShuttleFlight()
//        {
//            _chicagoCity.ResearchStation = true;
//            _bangkok.ResearchStation = true;
//            _scientist.CurrentCity = _chicagoCity;
//            Assert.AreEqual(true, PlayerActionsBl.DispatcherMovePlayer(_scientist, _players, _bangkok));
//            Assert.AreEqual(_scientist.CurrentCity.Name, _bangkok.Name);
//            _chicagoCity.ResearchStation = false;
//            _bangkok.ResearchStation = false;
//        }


//        #endregion

//        [TestMethod]
//        public void TestIncrementAfterCureDiseaseBasicBlue()
//        {
//            _opExpert.CurrentCity = _chicagoCity;
//            GameBoardModels.CubeCount.BlueCubes = 22;
//            _chicagoCity.BlueCubes = 2;
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Blue));
//            Assert.AreEqual(_chicagoCity.BlueCubes, 1);
//            Assert.AreEqual(GameBoardModels.CubeCount.BlueCubes, 23);
//        }

//        [TestMethod]
//        public void TestIncrementAfterCureDiseaseRed()
//        {
//            _opExpert.CurrentCity = _chicagoCity;
//            GameBoardModels.CubeCount.RedCubes = 22;
//            _chicagoCity.RedCubes = 2;
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_opExpert, Color.Red));
//            Assert.AreEqual(_chicagoCity.RedCubes, 1);
//            Assert.AreEqual(GameBoardModels.CubeCount.RedCubes, 23);
//        }

//        [TestMethod]
//        public void TestIncrementAfterMedicCureDiseaseRed()
//        {
//            _medic.CurrentCity = _chicagoCity;
//            GameBoardModels.CubeCount.RedCubes = 22;
//            _chicagoCity.RedCubes = 2;
//            Assert.AreEqual(true, PlayerActionsBl.TreatDiseaseOption(_medic, Color.Red));
//            Assert.AreEqual(_chicagoCity.RedCubes, 0);
//            Assert.AreEqual(GameBoardModels.CubeCount.RedCubes, 24);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(InvalidOperationException))]
//        public void TestWinCon()
//        {
//            List<string> cardsToSpend = new List<string> { _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName };
//            GameBoardModels.Curestatus.SetCureStatus(Color.Blue, GameBoardModels.Cures.Curestate.NotCured);
//            GameBoardModels.Curestatus.SetCureStatus(Color.Yellow, GameBoardModels.Cures.Curestate.Cured);
//            GameBoardModels.Curestatus.SetCureStatus(Color.Red, GameBoardModels.Cures.Curestate.Cured);
//            GameBoardModels.Curestatus.SetCureStatus(Color.Black, GameBoardModels.Cures.Curestate.Cured);
//            _scientist.Hand = new List<Card> { _atlanta, _chicagoCard, _london, _paris };
//            PlayerActionsBl.Cure(_scientist, cardsToSpend, Color.Blue);

//        }
//    }
//    /** PRINTING STUFF
//    //Print Statment
//    foreach (String name in returnList)
//    {
//        Console.Out.Write(name);
//    }**/
//}
