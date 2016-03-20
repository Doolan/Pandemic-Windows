using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQADemicApp.BL;
using SQADemicApp;
using System.IO;

namespace SQADemicAppTest
{
    [TestClass]
    public class PlayerActionsTest
    {
        City chicagoCity, bangkok, kolkata, sanFran;
        List<Card> hand, pile;
        List<Player> players;
        Card chennai, newYork, atlanta, chicagoCard, london, paris, milan, airlift;
        Player dispatcher, medic, opExpert, researcher, scientist;
        GameBoardModels gameBoardModels;


        [TestInitialize]
        public void SetupPlayer()
        {
            //set up GameboardModels if not already
            gameBoardModels = new GameBoardModels(new string[] { "Dispatcher", "Medic" });
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
            //Cards
            chennai = new Card("Chennai", Card.CARDTYPE.City, COLOR.black);
            newYork = new Card("New York", Card.CARDTYPE.City, COLOR.blue);
            atlanta = new Card("Atlanta", Card.CARDTYPE.City, COLOR.blue);
            chicagoCard = new Card("Chicago", Card.CARDTYPE.City, COLOR.blue);
            paris = new Card("Paris", Card.CARDTYPE.City, COLOR.blue);
            london = new Card("London", Card.CARDTYPE.City, COLOR.blue);
            milan = new Card("Milan", Card.CARDTYPE.City, COLOR.blue);
            airlift = new Card("Airlift",Card.CARDTYPE.Special);
           
            //Players
            scientist = new Player(ROLE.Scientist);
            opExpert = new Player(ROLE.OpExpert);
            researcher = new Player(ROLE.Researcher);
            medic = new Player(ROLE.Medic);
            players = new List<Player> { scientist, opExpert, researcher, medic };
        }

        [TestMethod]
        public void TestDriveOptions()
        {
            HashSet<City> returnedSet = PlayerActionsBL.DriveOptions(chicagoCity);
            HashSet<City> correctSet = chicagoCity.getAdjacentCities();
            CollectionAssert.AreEqual(returnedSet.ToArray(), correctSet.ToArray());
        }

        #region Direct Flight
        [TestMethod]
        public void TestDirectFlightOptionsNone()
        {
            hand = new List<Card>();
            List<String> returnList = PlayerActionsBL.DirectFlightOption(hand, chicagoCity);
            List<String> correctList = new List<String>();
            CollectionAssert.AreEqual(correctList, returnList);
        }

        [TestMethod]
        public void TestDirectFlightOptionNewYork()
        {
            hand = new List<Card> { newYork };
            List<String> returnList = PlayerActionsBL.DirectFlightOption(hand, chicagoCity);
            List<String> correctList = new List<String> { newYork.CityName };
            CollectionAssert.AreEqual(correctList, returnList);
        }

        [TestMethod]
        public void TestDirectFlightCurrentCityChicago()
        {
            hand = new List<Card> { chicagoCard };
            List<String> returnList = PlayerActionsBL.DirectFlightOption(hand, chicagoCity);
            List<String> correctList = new List<String>();
            CollectionAssert.AreEqual(correctList, returnList);
        }

        [TestMethod]
        public void TestDirectFlightMultipleCities()
        {
            hand = new List<Card> { chicagoCard, atlanta, chennai };
            List<String> returnList = PlayerActionsBL.DirectFlightOption(hand, chicagoCity);
            List<String> correctList = new List<String> { atlanta.CityName, chennai.CityName };
            CollectionAssert.AreEqual(correctList, returnList);
        }

        [TestMethod]
        public void TestDirectFlightWithNonCityCardInHand()
        {
            hand = new List<Card> { airlift, atlanta, chennai };
            List<String> returnList = PlayerActionsBL.DirectFlightOption(hand, chicagoCity);
            List<String> correctList = new List<String> { atlanta.CityName, chennai.CityName };
            CollectionAssert.AreEqual(correctList, returnList);

        }
        #endregion

        #region Charter Flight
        [TestMethod]
        public void TestCharterFlightFalseOption()
        {
            hand = new List<Card> { airlift, atlanta, chennai };
            bool returendBool = PlayerActionsBL.CharterFlightOption(hand, chicagoCity);
            bool correctBool = false;
            Assert.AreEqual(correctBool, returendBool);
        }

        [TestMethod]
        public void TestCharterFlightTrue()
        {
            hand = new List<Card> { airlift, atlanta, chicagoCard };
            bool returendBool = PlayerActionsBL.CharterFlightOption(hand, chicagoCity);
            bool correctBool = true;
            Assert.AreEqual(correctBool, returendBool);
        }
        #endregion

        #region Shuttle Flight
        [TestMethod]
        public void TestShuttleFlightNotInStation()
        {
            scientist.currentCity = kolkata;
            List<String> result = PlayerActionsBL.ShuttleFlightOption(kolkata);
            CollectionAssert.Equals(result, new List<String>());
        }

        [TestMethod]
        public void TestShuttleFlightNoOptions()
        {
            scientist.currentCity = kolkata;
            kolkata.researchStation = true;
            List<String> result = PlayerActionsBL.ShuttleFlightOption(kolkata);
            kolkata.researchStation = false;
            List<String> expected = new List<String> { "Atlanta"};
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestShuttleFlightOneOption()
        {
            scientist.currentCity = kolkata;
            kolkata.researchStation = true;
            bangkok.researchStation = true;
            List<String> result = PlayerActionsBL.ShuttleFlightOption(kolkata);
            kolkata.researchStation = false;
            bangkok.researchStation = false;
            List<String> expected = new List<String> { "Atlanta", "Bangkok" };
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestShuttleFlightMultipleOptions()
        {
            scientist.currentCity = chicagoCity;
            chicagoCity.Name = "Chicago";
            chicagoCity.researchStation = true;
            kolkata.researchStation = true;
            bangkok.researchStation = true;
            List<String> result = PlayerActionsBL.ShuttleFlightOption(chicagoCity);
            chicagoCity.researchStation = false;
            kolkata.researchStation = false;
            bangkok.researchStation = false;
            List<String> expected = new List<String> { "Atlanta", "Kolkata", "Bangkok" };
            CollectionAssert.AreEqual(expected, result);
        }
        #endregion

        #region Move Player
        [TestMethod]
        public void TestMoverPlayerAdjacentCitySanFran()
        {
            scientist.currentCity = chicagoCity;
            hand = new List<Card> { airlift, chicagoCard, chennai };
            scientist.hand = hand;
            PlayerActionsBL.moveplayer(scientist, sanFran);
            Assert.AreEqual(scientist.currentCity.Name, sanFran.Name);
        }

        [TestMethod]
        public void TestMoverPlayerAdjacentCityChicagoWithCard()
        {
            scientist.currentCity = sanFran;
            hand = new List<Card> { airlift, chicagoCard, chennai };
            List<Card> correctHand =new List<Card> { airlift, chicagoCard, chennai };
            scientist.hand = hand;
            PlayerActionsBL.moveplayer(scientist, chicagoCity);
            Assert.AreEqual(scientist.currentCity.Name, chicagoCity.Name);
            CollectionAssert.AreEqual(correctHand, hand);
        }

        [TestMethod]
        public void TestMoverPlayerDirectFlight()
        {
            scientist.currentCity = bangkok;
            hand = new List<Card> { airlift, chicagoCard, chennai, atlanta };
            pile = new List<Card>();
            scientist.hand = hand;
            PlayerActionsBL.moveplayer(scientist, chicagoCity);
            List<Card> correctHand = new List<Card> { airlift, chennai, atlanta };
            Assert.AreEqual(scientist.currentCity.Name, chicagoCity.Name);
            CollectionAssert.AreEqual(correctHand, hand);
        }

        [TestMethod]
        public void TestMoverPlayerCharterFlight()
        {
            scientist.currentCity = chicagoCity;
            hand = new List<Card> { airlift, chicagoCard, chennai };
            pile = new List<Card>();
            scientist.hand = hand;
            PlayerActionsBL.moveplayer(scientist, bangkok);
            List<Card> correctHand = new List<Card> { airlift, chennai };
            Assert.AreEqual(scientist.currentCity.Name, bangkok.Name);
            CollectionAssert.AreEqual(correctHand, hand);
        }

        [TestMethod]
        public void TestMoverPlayerShuttleFlightPreemptCard()
        {
            chicagoCity.researchStation = true;
            bangkok.researchStation = true;
            scientist.currentCity = chicagoCity;
            hand = new List<Card> { airlift, atlanta, chennai, chicagoCard };
            pile = new List<Card>();
            scientist.hand = hand;
            Assert.AreEqual(true, PlayerActionsBL.moveplayer(scientist, bangkok));
            List<Card> correctHand = new List<Card> { airlift, atlanta, chennai, chicagoCard };
            Assert.AreEqual(scientist.currentCity.Name, bangkok.Name);
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
            bangkok.researchStation = false;
        }

        [TestMethod]
        public void TestMoverPlayerInvalid()
        {
            scientist.currentCity = chicagoCity;
            hand = new List<Card> { airlift, atlanta, chennai };
            pile = new List<Card>();
            scientist.hand = hand;
            Assert.AreEqual(false, PlayerActionsBL.moveplayer(scientist, bangkok));
            List<Card> correctHand = new List<Card> { airlift, atlanta, chennai };
            Assert.AreEqual(scientist.currentCity.Name, chicagoCity.Name);
            CollectionAssert.AreEqual(correctHand, hand);
        }

        #endregion

        #region BuildAResearchStation

        [TestMethod]
        public void buildStationNormal()
        {
            scientist.currentCity = chicagoCity;
            chicagoCity.researchStation = false;
            hand = new List<Card> { airlift, chicagoCard, chennai };
            scientist.hand = hand;
            List<Card> correctHand = new List<Card> { airlift, chennai };
            Assert.AreEqual(true, PlayerActionsBL.BuildAResearchStationOption(scientist));
            CollectionAssert.AreEqual(correctHand, hand);
            Assert.AreEqual(true, chicagoCity.researchStation);
            chicagoCity.researchStation = false;
        }

        [TestMethod]
        public void buildStationFailLackCard()
        {
            scientist.currentCity = chicagoCity;
            hand = new List<Card> { airlift, chennai };
            scientist.hand = hand;
            List<Card> correctHand = new List<Card> { airlift, chennai };
            Assert.AreEqual(false, PlayerActionsBL.BuildAResearchStationOption(scientist));
            CollectionAssert.AreEqual(correctHand, hand);
        }

        [TestMethod]
        public void buildStationFailExisting()
        {
            scientist.currentCity = chicagoCity;
            chicagoCity.researchStation = true;
            hand = new List<Card> { airlift, chicagoCard, chennai };
            scientist.hand = hand;
            List<Card> correctHand = new List<Card> { airlift, chicagoCard, chennai };
            Assert.AreEqual(false, PlayerActionsBL.BuildAResearchStationOption(scientist));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
        }

        [TestMethod]
        public void buildStationOpExpert()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.researchStation = false;
            hand = new List<Card> { airlift, chicagoCard, chennai };
            opExpert.hand = hand;
            List<Card> correctHand = new List<Card> { airlift, chicagoCard, chennai };
            Assert.AreEqual(true, PlayerActionsBL.BuildAResearchStationOption(opExpert));
            CollectionAssert.AreEqual(correctHand, hand);
            Assert.AreEqual(true, chicagoCity.researchStation);
            chicagoCity.researchStation = false;
        }

        [TestMethod]
        public void buildStationOpExpertWithoutCard()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.researchStation = false;
            hand = new List<Card> { airlift, chennai };
            opExpert.hand = hand;
            List<Card> correctHand = new List<Card> { airlift, chennai };
            Assert.AreEqual(true, PlayerActionsBL.BuildAResearchStationOption(opExpert));
            CollectionAssert.AreEqual(correctHand, hand);
            Assert.AreEqual(true, chicagoCity.researchStation);
            chicagoCity.researchStation = false;
        }

        #endregion

        #region Cure
        //To test:
        //DONE: Not enough cards
        //TODO: already cured
        //DONE: enough cards 5 cards
        //DONE: enougn cured with scientist 4 cards
        //DONE: not at reasearch center
        //DONE: Test too many blue card

        [TestMethod]
        public void TestCureSimple()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName };
            List<Card> correctHand = new List<Card> { chennai, airlift };
            opExpert.currentCity = chicagoCity;
            chicagoCity.researchStation = true;
            Assert.AreEqual(true, PlayerActionsBL.Cure(opExpert, cardsToSpend, COLOR.blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
            Assert.AreEqual(GameBoardModels.CURESTATUS.getCureStatus(COLOR.blue), GameBoardModels.Cures.CURESTATE.Cured);
            GameBoardModels.CURESTATUS.setCureStatus(COLOR.blue, GameBoardModels.Cures.CURESTATE.NotCured);
            
        }

        [TestMethod]
        public void TestCureNotEnoughCards()
        {
            hand = new List<Card> { newYork, chennai, atlanta, chicagoCard, airlift };
            opExpert.hand = hand;
            opExpert.currentCity = chicagoCity;
            chicagoCity.researchStation = true;
            List<String> cardsToSpend = new List<String> { newYork.CityName, chicagoCard.CityName };
            List<Card> correctHand = new List<Card> { newYork, chennai, atlanta, chicagoCard, airlift };
            Assert.AreEqual(false, PlayerActionsBL.Cure(opExpert, cardsToSpend, COLOR.blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
        }

        [TestMethod]
        public void TestCureToManyValidCards()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, milan, airlift };
            opExpert.hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName, milan.CityName };
            List<Card> correctHand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, milan, airlift };
            opExpert.currentCity = chicagoCity;
            chicagoCity.researchStation = true;
            Assert.AreEqual(false, PlayerActionsBL.Cure(opExpert, cardsToSpend, COLOR.blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
        }

        [TestMethod]
        public void TestCureInvalidCards()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.hand = hand;
            List<String> cardsToSpend = new List<String> { chennai.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName };
            List<Card> correctHand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.currentCity = chicagoCity;
            chicagoCity.researchStation = true;
            Assert.AreEqual(false, PlayerActionsBL.Cure(opExpert, cardsToSpend, COLOR.blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
        }

        [TestMethod]
        public void TestCureNotInResearchStation()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName };
            List<Card> correctHand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.currentCity = chicagoCity;
            chicagoCity.researchStation = false;
            Assert.AreEqual(false, PlayerActionsBL.Cure(opExpert, cardsToSpend, COLOR.blue));
            CollectionAssert.AreEqual(correctHand, hand);
        }

        [TestMethod]
        public void TestCureSimpleScientist()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            scientist.hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName };
            List<Card> correctHand = new List<Card> { chennai, paris, airlift };
            scientist.currentCity = chicagoCity;
            chicagoCity.researchStation = true;
            Assert.AreEqual(true, PlayerActionsBL.Cure(scientist, cardsToSpend, COLOR.blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
            Assert.AreEqual(GameBoardModels.CURESTATUS.getCureStatus(COLOR.blue), GameBoardModels.Cures.CURESTATE.Cured);
            GameBoardModels.CURESTATUS.setCureStatus(COLOR.blue, GameBoardModels.Cures.CURESTATE.NotCured);
        }

        [TestMethod]
        public void TestCureToManyValidCardsScientist()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            scientist.hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName};
            List<Card> correctHand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            scientist.currentCity = chicagoCity;
            chicagoCity.researchStation = true;
            Assert.AreEqual(false, PlayerActionsBL.Cure(scientist, cardsToSpend, COLOR.blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
        }

        [TestMethod]
        public void TestCureSimpleAlreadyCured()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName };
            List<Card> correctHand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.currentCity = chicagoCity;
            chicagoCity.researchStation = true;
            GameBoardModels.CURESTATUS.BlueCure = GameBoardModels.Cures.CURESTATE.Cured;
            Assert.AreEqual(false, PlayerActionsBL.Cure(opExpert, cardsToSpend, COLOR.blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
            GameBoardModels.CURESTATUS.BlueCure = GameBoardModels.Cures.CURESTATE.NotCured;

        }


        #endregion

        #region Treat Diseases

        [TestMethod]
        public void TestTreatDiseaseBasicBlue()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.blueCubes = 2;
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.blue));
            Assert.AreEqual(chicagoCity.blueCubes, 1);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicRed()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.redCubes = 2;
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.red));
            Assert.AreEqual(chicagoCity.redCubes, 1);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicONLYellow()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.redCubes = 2; 
            chicagoCity.blueCubes = 2;
            chicagoCity.yellowCubes = 2;
            chicagoCity.blackCubes= 3;
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.yellow));
            Assert.AreEqual(chicagoCity.redCubes, 2);
            Assert.AreEqual(chicagoCity.blueCubes, 2);
            Assert.AreEqual(chicagoCity.yellowCubes, 1);
            Assert.AreEqual(chicagoCity.blackCubes, 3);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicDecreaseAll()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.redCubes = 1;
            chicagoCity.blueCubes = 2;
            chicagoCity.yellowCubes = 2;
            chicagoCity.blackCubes = 1;
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.red));
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.blue));
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.yellow));
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.black));
            Assert.AreEqual(chicagoCity.redCubes, 0);
            Assert.AreEqual(chicagoCity.blueCubes, 1);
            Assert.AreEqual(chicagoCity.yellowCubes, 1);
            Assert.AreEqual(chicagoCity.blackCubes, 0);
        }

        [TestMethod]
        public void TestTreateDiesaseCuresExist()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.redCubes = 1;
            chicagoCity.blueCubes = 2;
            chicagoCity.yellowCubes = 2;
            chicagoCity.blackCubes = 3;
            GameBoardModels.CURESTATUS.BlackCure = GameBoardModels.Cures.CURESTATE.Cured;
            GameBoardModels.CURESTATUS.BlueCure = GameBoardModels.Cures.CURESTATE.Cured;
            GameBoardModels.CURESTATUS.RedCure = GameBoardModels.Cures.CURESTATE.Cured;
            GameBoardModels.CURESTATUS.YellowCure= GameBoardModels.Cures.CURESTATE.Cured;
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.red));
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.blue));
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.yellow));
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.black));
            GameBoardModels.CURESTATUS.BlackCure = GameBoardModels.Cures.CURESTATE.NotCured;
            GameBoardModels.CURESTATUS.BlueCure = GameBoardModels.Cures.CURESTATE.NotCured;
            GameBoardModels.CURESTATUS.RedCure = GameBoardModels.Cures.CURESTATE.NotCured;
            GameBoardModels.CURESTATUS.YellowCure = GameBoardModels.Cures.CURESTATE.NotCured;
            Assert.AreEqual(chicagoCity.redCubes, 0);
            Assert.AreEqual(chicagoCity.blueCubes, 0);
            Assert.AreEqual(chicagoCity.yellowCubes, 0);
            Assert.AreEqual(chicagoCity.blackCubes, 0);
        }

        [TestMethod]
        public void TestTreatDiseaseMedicDecreaseAll()
        {
            medic.currentCity = chicagoCity;
            chicagoCity.redCubes = 1;
            chicagoCity.blueCubes = 2;
            chicagoCity.yellowCubes = 3;
            chicagoCity.blackCubes = 1;
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(medic, COLOR.red));
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(medic, COLOR.blue));
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(medic, COLOR.yellow));
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(medic, COLOR.black));
            Assert.AreEqual(chicagoCity.redCubes, 0);
            Assert.AreEqual(chicagoCity.blueCubes, 0);
            Assert.AreEqual(chicagoCity.yellowCubes, 0);
            Assert.AreEqual(chicagoCity.blackCubes, 0);
        }

        [TestMethod]
        public void TestTreatDiseaseZero()
        {
            opExpert.currentCity = chicagoCity;
            medic.currentCity = chicagoCity;
            chicagoCity.redCubes = 0;
            chicagoCity.blueCubes = 0;
            chicagoCity.yellowCubes = 0;
            chicagoCity.blackCubes = 0;
            Assert.AreEqual(false, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.red));
            Assert.AreEqual(false, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.blue));
            Assert.AreEqual(false, PlayerActionsBL.TreatDiseaseOption(medic, COLOR.yellow));
            Assert.AreEqual(false, PlayerActionsBL.TreatDiseaseOption(medic, COLOR.black));
            Assert.AreEqual(chicagoCity.redCubes, 0);
            Assert.AreEqual(chicagoCity.blueCubes, 0);
            Assert.AreEqual(chicagoCity.yellowCubes, 0);
            Assert.AreEqual(chicagoCity.blackCubes, 0);
        }

        #endregion

        #region TradeCards
        [TestMethod]
        public void TestShareChicagosimple()
        {
            List<Card> hand1 = new List<Card> { chennai, newYork, chicagoCard };
            List<Card> hand2 = new List<Card> { atlanta, london };
            scientist.hand = new List<Card> { chennai, newYork };
            opExpert.hand = new List<Card> { atlanta, london, chicagoCard };
            scientist.currentCity = chicagoCity;
            opExpert.currentCity = chicagoCity;
            Assert.AreEqual(true, PlayerActionsBL.ShareKnowledgeOption(opExpert, scientist, chicagoCard.CityName));
            CollectionAssert.AreEqual(scientist.hand, hand1);
            CollectionAssert.AreEqual(opExpert.hand, hand2);
        }

        [TestMethod]
        public void TestShareChicagoDiffrentCityFAIL()
        {
            List<Card> hand1 = new List<Card> { chennai, newYork };
            List<Card> hand2 = new List<Card> { atlanta, london, chicagoCard };
            scientist.hand = new List<Card> { chennai, newYork };
            opExpert.hand = new List<Card> { atlanta, london, chicagoCard };
            scientist.currentCity = chicagoCity;
            opExpert.currentCity = bangkok;
            Assert.AreEqual(false, PlayerActionsBL.ShareKnowledgeOption(opExpert, scientist, chicagoCard.CityName));
            CollectionAssert.AreEqual(scientist.hand, hand1);
            CollectionAssert.AreEqual(opExpert.hand, hand2);
        }


        [TestMethod]
        public void TestShareChicagoinBangkokFAIL()
        {
            List<Card> hand1 = new List<Card> { chennai, newYork };
            List<Card> hand2 = new List<Card> { atlanta, london, chicagoCard };
            scientist.hand = new List<Card> { chennai, newYork };
            opExpert.hand = new List<Card> { atlanta, london, chicagoCard };
            scientist.currentCity = bangkok;
            opExpert.currentCity = bangkok;
            Assert.AreEqual(false, PlayerActionsBL.ShareKnowledgeOption(opExpert, scientist, chicagoCard.CityName));
            CollectionAssert.AreEqual(scientist.hand, hand1);
            CollectionAssert.AreEqual(opExpert.hand, hand2);
        }

        [TestMethod]
        public void TestShareChicagoinBangkokResearcherPASS()
        {
            List<Card> hand1 = new List<Card> { chennai, newYork, chicagoCard };
            List<Card> hand2 = new List<Card> { atlanta, london };
            scientist.hand = new List<Card> { chennai, newYork };
            researcher.hand = new List<Card> { atlanta, london, chicagoCard };
            scientist.currentCity = bangkok;
            researcher.currentCity = bangkok;
            Assert.AreEqual(true, PlayerActionsBL.ShareKnowledgeOption(researcher, scientist, chicagoCard.CityName));
            CollectionAssert.AreEqual(scientist.hand, hand1);
            CollectionAssert.AreEqual(researcher.hand, hand2);
        }

        [TestMethod]
        public void TestShareChicagoMissingCardFail()
        {
            List<Card> hand1 = new List<Card> { chennai, newYork };
            List<Card> hand2 = new List<Card> { atlanta, london };
            scientist.hand = new List<Card> { chennai, newYork };
            opExpert.hand = new List<Card> { atlanta, london};
            scientist.currentCity = chicagoCity;
            opExpert.currentCity = chicagoCity;
            Assert.AreEqual(false, PlayerActionsBL.ShareKnowledgeOption(opExpert, scientist, chicagoCard.CityName));
            CollectionAssert.AreEqual(scientist.hand, hand1);
            CollectionAssert.AreEqual(opExpert.hand, hand2);
        }

        #endregion

        #region DispatcherMove

        [TestMethod]
        public void TestDispatcherMoveAdjacentCitySanFran()
        {
            scientist.currentCity = chicagoCity;
            Assert.AreEqual(PlayerActionsBL.DispatcherMovePlayer(scientist, players, sanFran), true);
            Assert.AreEqual(scientist.currentCity.Name, sanFran.Name);
        }


        [TestMethod]
        public void TestDispatcherMoveInvalidCityKolkata()
        {
            scientist.currentCity = chicagoCity;
            Assert.AreEqual(PlayerActionsBL.DispatcherMovePlayer(scientist, players, kolkata), false);
            Assert.AreEqual(scientist.currentCity.Name, chicagoCity.Name);
        }

        [TestMethod]
        public void TestDispatcherMoveToOtherPlayer()
        {
            scientist.currentCity = chicagoCity;
            opExpert.currentCity = bangkok; 
            Assert.AreEqual(PlayerActionsBL.DispatcherMovePlayer(scientist, players, bangkok), true);
            Assert.AreEqual(scientist.currentCity.Name, bangkok.Name);
            opExpert.currentCity = chicagoCity;
        }

        [TestMethod]
        public void TestDispatcherMoveShuttleFlight()
        {
            chicagoCity.researchStation = true;
            bangkok.researchStation = true;
            scientist.currentCity = chicagoCity;
            Assert.AreEqual(true, PlayerActionsBL.DispatcherMovePlayer(scientist, players, bangkok));
            Assert.AreEqual(scientist.currentCity.Name, bangkok.Name);
            chicagoCity.researchStation = false;
            bangkok.researchStation = false;
        }


        #endregion

        [TestMethod]
        public void TestIncrementAfterCureDiseaseBasicBlue()
        {
            opExpert.currentCity = chicagoCity;
            GameBoardModels.cubeCount.InfectionCubes.SetCubeCount(COLOR.blue, 22);
            chicagoCity.blueCubes = 2;
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.blue));
            Assert.AreEqual(chicagoCity.blueCubes, 1);
            Assert.AreEqual(GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(COLOR.blue), 23);
        }

        [TestMethod]
        public void TestIncrementAfterCureDiseaseRed()
        {
            opExpert.currentCity = chicagoCity;
            GameBoardModels.cubeCount.InfectionCubes.SetCubeCount(COLOR.red, 22);
            chicagoCity.redCubes = 2;
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(opExpert, COLOR.red));
            Assert.AreEqual(chicagoCity.redCubes, 1);
            Assert.AreEqual(GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(COLOR.red), 23);
        }

        [TestMethod]
        public void TestIncrementAfterMedicCureDiseaseRed()
        {
            medic.currentCity = chicagoCity;
            GameBoardModels.cubeCount.InfectionCubes.SetCubeCount(COLOR.red, 22);
            chicagoCity.redCubes = 2;
            Assert.AreEqual(true, PlayerActionsBL.TreatDiseaseOption(medic, COLOR.red));
            Assert.AreEqual(chicagoCity.redCubes, 0);
            Assert.AreEqual(GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(COLOR.red), 24);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestWinCon()
        {
            List<String> cardsToSpend = new List<String> { atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName };
            GameBoardModels.CURESTATUS.setCureStatus(COLOR.blue , GameBoardModels.Cures.CURESTATE.NotCured);
            GameBoardModels.CURESTATUS.setCureStatus(COLOR.yellow, GameBoardModels.Cures.CURESTATE.Cured);
            GameBoardModels.CURESTATUS.setCureStatus(COLOR.red, GameBoardModels.Cures.CURESTATE.Cured);
            GameBoardModels.CURESTATUS.setCureStatus(COLOR.black, GameBoardModels.Cures.CURESTATE.Cured);
            scientist.hand = new List<Card> { atlanta, chicagoCard, london, paris };
            PlayerActionsBL.Cure(scientist, cardsToSpend, COLOR.blue);

        }


        //[TestMethod]
        //public void TestSetDiseaseCubesNoCure()
        //{
        //    GameBoardModels.CURESTATUS.RedCure = GameBoardModels.Cures.CURESTATE.NotCured;
        //    var startingCount = GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(COLOR.red);
        //    chicagoCity.redCubes = 2;

        //    PrivateObject accessor = new PrivateObject(new PlayerActionsBL());
        //    accessor.Invoke("SetDiseaseCubes", new Object[] {chicagoCity, COLOR.red, 2, 1});
        //    Assert.AreEqual(GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(COLOR.red), (startingCount -1));
        //}



    }
    /** PRINTING STUFF
    //Print Statment
    foreach (String name in returnList)
    {
        Console.Out.Write(name);
    }**/
}
