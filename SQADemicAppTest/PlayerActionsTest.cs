using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQADemicApp.BL;
using SQADemicApp;
using System.IO;
using SQADemicApp.Players;
using SQADemicApp.Objects;

namespace SQADemicAppTest
{
    [TestClass]
    public class PlayerActionsTest
    {
        City chicagoCity, bangkok, kolkata, sanFran;
        List<Card> hand, pile;
        List<AbstractPlayer> players;
        Card chennai, newYork, atlanta, chicagoCard, london, paris, milan, airlift;
        AbstractPlayer dispatcher, medic, opExpert, researcher, scientist, containmentSpecialst, troubleshooter, FieldOperative, Generalist, archivist;
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
            scientist = new ScientistPlayer();
            opExpert = new OpExpertPlayer();
            researcher = new ResearcherPlayer();
            medic = new MedicPlayer();
            containmentSpecialst = new ContainmentSpecialstPlayer();
            troubleshooter = new TroubleshooterPlayer();
            FieldOperative = new FieldOperativePlayer();
            Generalist = new GeneralistPlayer();
            archivist = new ArchivistPlayer();
            players = new List<AbstractPlayer> { scientist, opExpert, researcher, medic };
        }

        [TestMethod]
        public void TestDriveOptions()
        {
            HashSet<City> returnedSet = AbstractPlayer.DriveOptions(chicagoCity);
            HashSet<City> correctSet = chicagoCity.getAdjacentCities();
            CollectionAssert.AreEqual(returnedSet.ToArray(), correctSet.ToArray());
        }

        #region Direct Flight

        public void assertDirectFlightOptionIsCorrect(List<Card> hand, List<string> correctList)
        {
            List<String> returnList = AbstractPlayer.DirectFlightOption(hand, chicagoCity);
            CollectionAssert.AreEqual(correctList, returnList);
        }

        [TestMethod]
        public void TestDirectFlightOptionsNone()
        {
            assertDirectFlightOptionIsCorrect(new List<Card>(), new List<string>());
        }

        [TestMethod]
        public void TestDirectFlightOptionNewYork()
        {
            assertDirectFlightOptionIsCorrect(new List<Card> { newYork }, new List<string> { newYork.CityName });
        }

        [TestMethod]
        public void TestDirectFlightCurrentCityChicago()
        {
            assertDirectFlightOptionIsCorrect(new List<Card> { chicagoCard }, new List<string>());
        }

        [TestMethod]
        public void TestDirectFlightMultipleCities()
        {
            assertDirectFlightOptionIsCorrect(new List<Card> { chicagoCard, atlanta, chennai }, 
                new List<string> { atlanta.CityName, chennai.CityName });
        }

        [TestMethod]
        public void TestDirectFlightWithNonCityCardInHand()
        {
            assertDirectFlightOptionIsCorrect(new List<Card> { airlift, atlanta, chennai }, 
                new List<string> { atlanta.CityName, chennai.CityName });
        }
        #endregion

        #region Charter Flight
        [TestMethod]
        public void TestCharterFlightFalseOption()
        {
            hand = new List<Card> { airlift, atlanta, chennai };
            bool returendBool = AbstractPlayer.CharterFlightOption(hand, chicagoCity);
            bool correctBool = false;
            Assert.AreEqual(correctBool, returendBool);
        }

        [TestMethod]
        public void TestCharterFlightTrue()
        {
            hand = new List<Card> { airlift, atlanta, chicagoCard };
            bool returendBool = AbstractPlayer.CharterFlightOption(hand, chicagoCity);
            bool correctBool = true;
            Assert.AreEqual(correctBool, returendBool);
        }
        #endregion

        #region Shuttle Flight
        [TestMethod]
        public void TestShuttleFlightNotInStation()
        {
            scientist.currentCity = kolkata;
            List<String> result = AbstractPlayer.ShuttleFlightOption(kolkata);
            CollectionAssert.Equals(result, new List<String>());
        }

        [TestMethod]
        public void TestShuttleFlightNoOptions()
        {
            scientist.currentCity = kolkata;
            kolkata.researchStation = true;
            List<String> result = AbstractPlayer.ShuttleFlightOption(kolkata);
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
            List<String> result = AbstractPlayer.ShuttleFlightOption(kolkata);
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
            List<String> result = AbstractPlayer.ShuttleFlightOption(chicagoCity);
            chicagoCity.researchStation = false;
            kolkata.researchStation = false;
            bangkok.researchStation = false;
            List<String> expected = new List<String> { "Atlanta", "Kolkata", "Bangkok" };
            CollectionAssert.AreEqual(expected, result);
        }
        #endregion

        #region Move Player

        public void assertMovePlayerCorrect(City currnetCity, List<Card> hand, List<Card> correctHand, City moveTo)
        {
            scientist.currentCity = currnetCity;
            scientist.hand = hand;
            Assert.AreEqual(true, scientist.MovePlayer(moveTo));
            Assert.AreEqual(scientist.currentCity.Name, moveTo.Name);
            CollectionAssert.AreEqual(correctHand, hand);
        }

        [TestMethod]
        public void TestMoverPlayerAdjacentCitySanFran()
        {
            assertMovePlayerCorrect(chicagoCity, new List<Card> { airlift, chicagoCard, chennai },
                new List<Card> { airlift, chicagoCard, chennai }, sanFran);
        }

        [TestMethod]
        public void TestMoverPlayerAdjacentCityChicagoWithCard()
        {
            assertMovePlayerCorrect(sanFran, new List<Card> { airlift, chicagoCard, chennai },
                new List<Card> { airlift, chicagoCard, chennai }, chicagoCity);
        }

        [TestMethod]
        public void TestMoverPlayerDirectFlight()
        {
            assertMovePlayerCorrect(bangkok, new List<Card> { airlift, chicagoCard, chennai, atlanta },
                new List<Card> { airlift, chennai, atlanta }, chicagoCity);
        }

        [TestMethod]
        public void TestMoverPlayerCharterFlight()
        {
            assertMovePlayerCorrect(chicagoCity, new List<Card> { airlift, chicagoCard, chennai },
                new List<Card> { airlift, chennai }, bangkok);
        }

        [TestMethod]
        public void TestMoverPlayerShuttleFlightPreemptCard()
        {
            chicagoCity.researchStation = true;
            bangkok.researchStation = true;
            assertMovePlayerCorrect(chicagoCity, new List<Card> { airlift, atlanta, chennai, chicagoCard },
                new List<Card> { airlift, atlanta, chennai, chicagoCard }, bangkok);
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
            Assert.AreEqual(false, scientist.MovePlayer(bangkok));
            List<Card> correctHand = new List<Card> { airlift, atlanta, chennai };
            Assert.AreEqual(scientist.currentCity.Name, chicagoCity.Name);
            CollectionAssert.AreEqual(correctHand, hand);
        }

        #endregion

        #region BuildAResearchStation

        public void assertBuildStationCorrect(AbstractPlayer player, City currentCity, List<Card> hand, List<Card> correctHand, bool success)
        {
            player.currentCity = currentCity;
            currentCity.researchStation = !success && currentCity.researchStation;
            player.hand = hand;
            Assert.AreEqual(success, player.BuildAResearchStationOption());
            CollectionAssert.AreEqual(correctHand, hand);
            if (success)
            {
                Assert.AreEqual(true, chicagoCity.researchStation);
            }
            chicagoCity.researchStation = false;
        }

        [TestMethod]
        public void buildStationNormal()
        {
            assertBuildStationCorrect(scientist, chicagoCity, new List<Card> { airlift, chicagoCard, chennai },
                new List<Card> { airlift, chennai }, true);
        }

        [TestMethod]
        public void buildStationFailLackCard()
        {
            assertBuildStationCorrect(scientist, chicagoCity, new List<Card> { airlift, chennai },
                new List<Card> { airlift, chennai }, false);
        }

        [TestMethod]
        public void buildStationFailExisting()
        {
            chicagoCity.researchStation = true;
            assertBuildStationCorrect(scientist, chicagoCity, new List<Card> { airlift, chicagoCard, chennai },
                new List<Card> { airlift, chicagoCard, chennai }, false);
        }

        [TestMethod]
        public void buildStationOpExpert()
        {
            assertBuildStationCorrect(opExpert, chicagoCity, new List<Card> { airlift, chicagoCard, chennai },
                new List<Card> { airlift, chicagoCard, chennai }, true);
        }

        [TestMethod]
        public void buildStationOpExpertWithoutCard()
        {
            assertBuildStationCorrect(opExpert, chicagoCity, new List<Card> { airlift, chennai },
                new List<Card> { airlift, chennai }, true);
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
            Assert.AreEqual(true, opExpert.Cure(cardsToSpend, COLOR.blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
            Assert.AreEqual(GameBoardModels.GetCureStatus(COLOR.blue), Cures.CURESTATE.Cured);
            GameBoardModels.SetCureStatus(COLOR.blue, Cures.CURESTATE.NotCured);
            
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
            Assert.AreEqual(false, opExpert.Cure(cardsToSpend, COLOR.blue));
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
            Assert.AreEqual(false, opExpert.Cure(cardsToSpend, COLOR.blue));
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
            Assert.AreEqual(false, opExpert.Cure(cardsToSpend, COLOR.blue));
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
            Assert.AreEqual(false, opExpert.Cure(cardsToSpend, COLOR.blue));
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
            Assert.AreEqual(true, scientist.Cure(cardsToSpend, COLOR.blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
            Assert.AreEqual(GameBoardModels.GetCureStatus(COLOR.blue), Cures.CURESTATE.Cured);
            GameBoardModels.SetCureStatus(COLOR.blue, Cures.CURESTATE.NotCured);
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
            Assert.AreEqual(false, scientist.Cure(cardsToSpend, COLOR.blue));
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
            GameBoardModels.SetCureStatus(COLOR.blue, Cures.CURESTATE.Cured);
            Assert.AreEqual(false, opExpert.Cure(cardsToSpend, COLOR.blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.researchStation = false;
            GameBoardModels.SetCureStatus(COLOR.blue,Cures.CURESTATE.NotCured);

        }


        #endregion

        #region Treat Diseases

        [TestMethod]
        public void TestTreatDiseaseBasicBlue()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(COLOR.blue, 2);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.blue));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.blue) , 1);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicRed()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(COLOR.red, 2);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.red));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.red), 1);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicONLYellow()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(COLOR.red, 2);
            chicagoCity.Cubes.SetCubeCount(COLOR.blue, 2);
            chicagoCity.Cubes.SetCubeCount(COLOR.yellow, 2);
            chicagoCity.Cubes.SetCubeCount(COLOR.black, 3);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.yellow));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.red), 2);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.blue), 2);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.yellow), 1);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.black), 3);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicDecreaseAll()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(COLOR.red, 1);
            chicagoCity.Cubes.SetCubeCount(COLOR.blue, 2);
            chicagoCity.Cubes.SetCubeCount(COLOR.yellow, 2);
            chicagoCity.Cubes.SetCubeCount(COLOR.black, 1);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.red));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.blue));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.yellow));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.black));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.red), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.blue), 1);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.yellow), 1);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.black), 0);
        }

        [TestMethod]
        public void TestTreateDiesaseCuresExist()
        {
            opExpert.currentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(COLOR.red, 1);
            chicagoCity.Cubes.SetCubeCount(COLOR.blue, 2);
            chicagoCity.Cubes.SetCubeCount(COLOR.yellow, 2);
            chicagoCity.Cubes.SetCubeCount(COLOR.black, 3);
            GameBoardModels.SetCureStatus(COLOR.black, Cures.CURESTATE.Cured);
            GameBoardModels.SetCureStatus(COLOR.blue, Cures.CURESTATE.Cured);
            GameBoardModels.SetCureStatus(COLOR.red, Cures.CURESTATE.Cured);
            GameBoardModels.SetCureStatus(COLOR.yellow, Cures.CURESTATE.Cured);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.red));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.blue));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.yellow));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.black));
            GameBoardModels.SetCureStatus(COLOR.black, Cures.CURESTATE.NotCured);
            GameBoardModels.SetCureStatus(COLOR.blue, Cures.CURESTATE.NotCured);
            GameBoardModels.SetCureStatus(COLOR.red, Cures.CURESTATE.NotCured);
            GameBoardModels.SetCureStatus(COLOR.yellow, Cures.CURESTATE.NotCured);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.red), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.blue), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.yellow), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.black), 0);
        }

        [TestMethod]
        public void TestTreatDiseaseMedicDecreaseAll()
        {
            medic.currentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(COLOR.red, 1);
            chicagoCity.Cubes.SetCubeCount(COLOR.blue, 2);
            chicagoCity.Cubes.SetCubeCount(COLOR.yellow, 3);
            chicagoCity.Cubes.SetCubeCount(COLOR.black, 1);
            Assert.AreEqual(true, medic.TreatDiseaseOption(COLOR.red));
            Assert.AreEqual(true, medic.TreatDiseaseOption(COLOR.blue));
            Assert.AreEqual(true, medic.TreatDiseaseOption(COLOR.yellow));
            Assert.AreEqual(true, medic.TreatDiseaseOption(COLOR.black));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.red), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.blue), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.yellow), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.black), 0);
        }

        [TestMethod]
        public void TestTreatDiseaseZero()
        {
            opExpert.currentCity = chicagoCity;
            medic.currentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(COLOR.red, 0);
            chicagoCity.Cubes.SetCubeCount(COLOR.blue, 0);
            chicagoCity.Cubes.SetCubeCount(COLOR.yellow, 0);
            chicagoCity.Cubes.SetCubeCount(COLOR.black, 0);
            Assert.AreEqual(false, opExpert.TreatDiseaseOption(COLOR.red));
            Assert.AreEqual(false, opExpert.TreatDiseaseOption(COLOR.blue));
            Assert.AreEqual(false, medic.TreatDiseaseOption(COLOR.yellow));
            Assert.AreEqual(false, medic.TreatDiseaseOption(COLOR.black));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.red), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.blue), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.yellow),0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.black), 0);
        }

        #endregion

        #region TradeCards

        public void assertTradeCardWorks(AbstractPlayer from, AbstractPlayer to, List<Card> fromHand, List<Card> toHand, 
            City fromCity, City toCity, bool success)
        {
            to.hand = toHand;
            from.hand = fromHand;
            to.currentCity = toCity;
            from.currentCity = fromCity;
            Assert.AreEqual(success, from.ShareKnowledgeOption(to, chicagoCard.CityName));
            if (success)
            {
                fromHand.Remove(chicagoCard);
                toHand.Add(chicagoCard);
            }
            CollectionAssert.AreEqual(to.hand, toHand);
            CollectionAssert.AreEqual(from.hand, fromHand);
        }

        [TestMethod]
        public void TestShareChicagosimple()
        {
            assertTradeCardWorks(opExpert, scientist, new List<Card> { atlanta, london, chicagoCard },
                new List<Card> { chennai, newYork }, chicagoCity, chicagoCity, true);
        }

        [TestMethod]
        public void TestShareChicagoDiffrentCityFAIL()
        {
            assertTradeCardWorks(opExpert, scientist, new List<Card> { atlanta, london, chicagoCard },
                new List<Card> { chennai, newYork }, bangkok, chicagoCity, false);
        }

        [TestMethod]
        public void TestShareChicagoinBangkokFAIL()
        {
            assertTradeCardWorks(opExpert, scientist, new List<Card> { atlanta, london, chicagoCard },
                new List<Card> { chennai, newYork }, bangkok, bangkok, false);
        }

        [TestMethod]
        public void TestShareChicagoinBangkokResearcherPASS()
        {
            assertTradeCardWorks(researcher, scientist, new List<Card> { atlanta, london, chicagoCard },
                new List<Card> { chennai, newYork }, bangkok, bangkok, true);
        }

        [TestMethod]
        public void TestShareChicagoMissingCardFail()
        {
            assertTradeCardWorks(opExpert, scientist, new List<Card> { atlanta, london },
                new List<Card> { chennai, newYork }, chicagoCity, chicagoCity, false);
        }

        #endregion

        #region DispatcherMove

        public void assertScientistMoveCorrect(City toCity, bool success)
        {
            scientist.currentCity = chicagoCity;
            Assert.AreEqual(scientist.DispatcherMovePlayer(players, toCity), success);
            Assert.AreEqual(scientist.currentCity.Name, success ? toCity.Name : chicagoCity.Name);
        }

        [TestMethod]
        public void TestDispatcherMoveAdjacentCitySanFran()
        {
            assertScientistMoveCorrect(chicagoCity, true);
        }

        [TestMethod]
        public void TestDispatcherMoveInvalidCityKolkata()
        {
            assertScientistMoveCorrect(kolkata, false);
        }

        [TestMethod]
        public void TestDispatcherMoveToOtherPlayer()
        {
            opExpert.currentCity = bangkok;
            assertScientistMoveCorrect(bangkok, true);
            opExpert.currentCity = chicagoCity;
        }

        [TestMethod]
        public void TestDispatcherMoveShuttleFlight()
        {
            chicagoCity.researchStation = true;
            bangkok.researchStation = true;
            assertScientistMoveCorrect(bangkok, true);
            chicagoCity.researchStation = false;
            bangkok.researchStation = false;
        }


        #endregion

        [TestMethod]
        public void TestIncrementAfterCureDiseaseBasicBlue()
        {
            opExpert.currentCity = chicagoCity;
            GameBoardModels.SetInfectionCubeCount(COLOR.blue, 22);
            chicagoCity.Cubes.SetCubeCount(COLOR.blue, 2);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.blue));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.blue), 1);
            Assert.AreEqual(GameBoardModels.GetInfectionCubeCount(COLOR.blue), 23);
        }

        [TestMethod]
        public void TestIncrementAfterCureDiseaseRed()
        {
            opExpert.currentCity = chicagoCity;
            GameBoardModels.SetInfectionCubeCount(COLOR.red, 22);
            chicagoCity.Cubes.SetCubeCount(COLOR.red, 2);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(COLOR.red));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.red), 1);
            Assert.AreEqual(GameBoardModels.GetInfectionCubeCount(COLOR.red), 23);
        }

        [TestMethod]
        public void TestIncrementAfterMedicCureDiseaseRed()
        {
            medic.currentCity = chicagoCity;
            GameBoardModels.SetInfectionCubeCount(COLOR.red, 22);
            chicagoCity.Cubes.SetCubeCount(COLOR.red, 2);
            Assert.AreEqual(true, medic.TreatDiseaseOption(COLOR.red));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(COLOR.red), 0);
            Assert.AreEqual(GameBoardModels.GetInfectionCubeCount(COLOR.red), 24);
        }

        [TestMethod]
        public void TestDiseaseContainmentOnMove()
        {
            containmentSpecialst.currentCity = chicagoCity;
            GameBoardModels.SetInfectionCubeCount(COLOR.red, 10);
            sanFran.Cubes.SetCubeCount(COLOR.red, 4);
            Assert.AreEqual(4, sanFran.Cubes.GetCubeCount(COLOR.red));
            bool success = containmentSpecialst.MovePlayer(sanFran);
            Assert.IsTrue(success);
            Assert.AreEqual(3, sanFran.Cubes.GetCubeCount(COLOR.red));

        }

        [TestMethod]
        public void Archivist8CardHand()
        {
            archivist.hand = new List<Card> { atlanta, chicagoCard, london, paris, milan, chennai, newYork };
            archivist.addCardToHand(airlift);

            Assert.IsTrue(archivist.hand.Count == 8);
        }

        [TestMethod]
        public void TestDrawCardFromDiscardArchivist()
        {
            archivist.currentCity = Create.cityDictionary["Delhi"];
            archivist.hand = new List<Card> {chicagoCard};
            archivist.MovePlayer(chicagoCity);
            archivist.hand = new List<Card>();
            Assert.IsTrue(archivist.hand.Count == 0);
            
            archivist.PreformSpecialAction("ReclaimCityCard", new MockGameBoardView());
            Assert.IsTrue(archivist.hand.Contains(chicagoCard));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestWinCon()
        {
            List<String> cardsToSpend = new List<String> { atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName };
            GameBoardModels.SetCureStatus(COLOR.blue , Cures.CURESTATE.NotCured);
            GameBoardModels.SetCureStatus(COLOR.yellow, Cures.CURESTATE.Cured);
            GameBoardModels.SetCureStatus(COLOR.red, Cures.CURESTATE.Cured);
            GameBoardModels.SetCureStatus(COLOR.black, Cures.CURESTATE.Cured);
            scientist.hand = new List<Card> { atlanta, chicagoCard, london, paris };
            scientist.Cure(cardsToSpend, COLOR.blue);

        }


        //[TestMethod]
        //public void TestSetDiseaseCubesNoCure()
        //{
        //    GameBoardModels.CURESTATUS.RedCure = GameBoardModels.Cures.CURESTATE.NotCured;
        //    var startingCount = GameBoardModels.cubeCount.InfectionCubesBoard.GetCubeCount(COLOR.red);
        //    chicagoCity.redCubes = 2;

        //    PrivateObject accessor = new PrivateObject(new PlayerActionsBL());
        //    accessor.Invoke("SetDiseaseCubes", new Object[] {chicagoCity, COLOR.red, 2, 1});
        //    Assert.AreEqual(GameBoardModels.cubeCount.InfectionCubesBoard.GetCubeCount(COLOR.red), (startingCount -1));
        //}

       

    }

    class MockGameBoardView : GameBoard
    {

    }

    /** PRINTING STUFF
    //Print Statment
    foreach (String name in returnList)
    {
        Console.Out.Write(name);
    }**/
}
