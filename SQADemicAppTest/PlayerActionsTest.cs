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
        AbstractPlayer dispatcher, medic, opExpert, researcher, scientist, containmentSpecialst, troubleshooter, fieldOperative, generalist, archivist;
        GameBoardModels gameBoardModels;


        [TestInitialize]
        public void SetupPlayer()
        {
            //set up GameboardModels if not already
            gameBoardModels = new GameBoardModels(new string[] { "Dispatcher", "Medic" });
            //cities
            Create.CreateDictionary();
            Create.SetAdjacentCities(new StringReader("Chicago;San Francisco,Los Angeles,Atlanta,Montreal"));
            Create.SetAdjacentCities(new StringReader("Bangkok;Kolkata,Hong Kong,Ho Chi Minh City,Jakarta,Chennai"));
            Create.SetAdjacentCities(new StringReader("Kolkata;Delhi,Chennai,Bangkok,Hong Kong"));
            Create.SetAdjacentCities(new StringReader("San Francisco;Tokyo,Manila,Chicago,Los Angeles"));
            if (!Create.CityDictionary.TryGetValue("Chicago", out chicagoCity) ||
                !Create.CityDictionary.TryGetValue("Bangkok", out bangkok) ||
                !Create.CityDictionary.TryGetValue("Kolkata", out kolkata) ||
                !Create.CityDictionary.TryGetValue("San Francisco", out sanFran))
            {
                throw new InvalidOperationException("Set up failed");
            }
            //Cards
            chennai = new Card("Chennai", Card.Cardtype.City, Color.Black);
            newYork = new Card("New York", Card.Cardtype.City, Color.Blue);
            atlanta = new Card("Atlanta", Card.Cardtype.City, Color.Blue);
            chicagoCard = new Card("Chicago", Card.Cardtype.City, Color.Blue);
            paris = new Card("Paris", Card.Cardtype.City, Color.Blue);
            london = new Card("London", Card.Cardtype.City, Color.Blue);
            milan = new Card("Milan", Card.Cardtype.City, Color.Blue);
            airlift = new Card("Airlift",Card.Cardtype.Special);
           
            //Players
            scientist = new ScientistPlayer();
            opExpert = new OpExpertPlayer();
            researcher = new ResearcherPlayer();
            medic = new MedicPlayer();
            containmentSpecialst = new ContainmentSpecialstPlayer();
            troubleshooter = new TroubleshooterPlayer();
            fieldOperative = new FieldOperativePlayer();
            generalist = new GeneralistPlayer();
            archivist = new ArchivistPlayer();
            players = new List<AbstractPlayer> { scientist, opExpert, researcher, medic };
        }

        [TestMethod]
        public void TestDriveOptions()
        {
            HashSet<City> returnedSet = AbstractPlayer.DriveOptions(chicagoCity);
            HashSet<City> correctSet = chicagoCity.GetAdjacentCities();
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
            scientist.CurrentCity = kolkata;
            List<String> result = AbstractPlayer.ShuttleFlightOption(kolkata);
            CollectionAssert.Equals(result, new List<String>());
        }

        [TestMethod]
        public void TestShuttleFlightNoOptions()
        {
            scientist.CurrentCity = kolkata;
            kolkata.ResearchStation = true;
            List<String> result = AbstractPlayer.ShuttleFlightOption(kolkata);
            kolkata.ResearchStation = false;
            List<String> expected = new List<String> { "Atlanta"};
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestShuttleFlightOneOption()
        {
            scientist.CurrentCity = kolkata;
            kolkata.ResearchStation = true;
            bangkok.ResearchStation = true;
            List<String> result = AbstractPlayer.ShuttleFlightOption(kolkata);
            kolkata.ResearchStation = false;
            bangkok.ResearchStation = false;
            List<String> expected = new List<String> { "Atlanta", "Bangkok" };
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestShuttleFlightMultipleOptions()
        {
            scientist.CurrentCity = chicagoCity;
            chicagoCity.Name = "Chicago";
            chicagoCity.ResearchStation = true;
            kolkata.ResearchStation = true;
            bangkok.ResearchStation = true;
            List<String> result = AbstractPlayer.ShuttleFlightOption(chicagoCity);
            chicagoCity.ResearchStation = false;
            kolkata.ResearchStation = false;
            bangkok.ResearchStation = false;
            List<String> expected = new List<String> { "Atlanta", "Kolkata", "Bangkok" };
            CollectionAssert.AreEqual(expected, result);
        }
        #endregion

        #region Move Player

        public void assertMovePlayerCorrect(City currnetCity, List<Card> hand, List<Card> correctHand, City moveTo)
        {
            scientist.CurrentCity = currnetCity;
            scientist.Hand = hand;
            Assert.AreEqual(true, scientist.MovePlayer(moveTo));
            Assert.AreEqual(scientist.CurrentCity.Name, moveTo.Name);
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
            chicagoCity.ResearchStation = true;
            bangkok.ResearchStation = true;
            assertMovePlayerCorrect(chicagoCity, new List<Card> { airlift, atlanta, chennai, chicagoCard },
                new List<Card> { airlift, atlanta, chennai, chicagoCard }, bangkok);
            chicagoCity.ResearchStation = false;
            bangkok.ResearchStation = false;
        }

        [TestMethod]
        public void TestMoverPlayerInvalid()
        {
            scientist.CurrentCity = chicagoCity;
            hand = new List<Card> { airlift, atlanta, chennai };
            pile = new List<Card>();
            scientist.Hand = hand;
            Assert.AreEqual(false, scientist.MovePlayer(bangkok));
            List<Card> correctHand = new List<Card> { airlift, atlanta, chennai };
            Assert.AreEqual(scientist.CurrentCity.Name, chicagoCity.Name);
            CollectionAssert.AreEqual(correctHand, hand);
        }

        #endregion

        #region BuildAResearchStation

        public void assertBuildStationCorrect(AbstractPlayer player, City currentCity, List<Card> hand, List<Card> correctHand, bool success)
        {
            player.CurrentCity = currentCity;
            currentCity.ResearchStation = !success && currentCity.ResearchStation;
            player.Hand = hand;
            Assert.AreEqual(success, player.BuildAResearchStationOption());
            CollectionAssert.AreEqual(correctHand, hand);
            if (success)
            {
                Assert.AreEqual(true, chicagoCity.ResearchStation);
            }
            chicagoCity.ResearchStation = false;
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
            chicagoCity.ResearchStation = true;
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
            opExpert.Hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName };
            List<Card> correctHand = new List<Card> { chennai, airlift };
            opExpert.CurrentCity = chicagoCity;
            chicagoCity.ResearchStation = true;
            Assert.AreEqual(true, opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.ResearchStation = false;
            Assert.AreEqual(GameBoardModels.GetCureStatus(Color.Blue), Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Blue, Cures.Curestate.NotCured);
            
        }

        [TestMethod]
        public void TestCureNotEnoughCards()
        {
            hand = new List<Card> { newYork, chennai, atlanta, chicagoCard, airlift };
            opExpert.Hand = hand;
            opExpert.CurrentCity = chicagoCity;
            chicagoCity.ResearchStation = true;
            List<String> cardsToSpend = new List<String> { newYork.CityName, chicagoCard.CityName };
            List<Card> correctHand = new List<Card> { newYork, chennai, atlanta, chicagoCard, airlift };
            Assert.AreEqual(false, opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.ResearchStation = false;
        }

        [TestMethod]
        public void TestCureToManyValidCards()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, milan, airlift };
            opExpert.Hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName, milan.CityName };
            List<Card> correctHand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, milan, airlift };
            opExpert.CurrentCity = chicagoCity;
            chicagoCity.ResearchStation = true;
            Assert.AreEqual(false, opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.ResearchStation = false;
        }

        [TestMethod]
        public void TestCureInvalidCards()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.Hand = hand;
            List<String> cardsToSpend = new List<String> { chennai.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName };
            List<Card> correctHand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.CurrentCity = chicagoCity;
            chicagoCity.ResearchStation = true;
            Assert.AreEqual(false, opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.ResearchStation = false;
        }

        [TestMethod]
        public void TestCureNotInResearchStation()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.Hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName };
            List<Card> correctHand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.CurrentCity = chicagoCity;
            chicagoCity.ResearchStation = false;
            Assert.AreEqual(false, opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, hand);
        }

        [TestMethod]
        public void TestCureSimpleScientist()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            scientist.Hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName };
            List<Card> correctHand = new List<Card> { chennai, paris, airlift };
            scientist.CurrentCity = chicagoCity;
            chicagoCity.ResearchStation = true;
            Assert.AreEqual(true, scientist.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.ResearchStation = false;
            Assert.AreEqual(GameBoardModels.GetCureStatus(Color.Blue), Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Blue, Cures.Curestate.NotCured);
        }

        [TestMethod]
        public void TestCureToManyValidCardsScientist()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            scientist.Hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName};
            List<Card> correctHand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            scientist.CurrentCity = chicagoCity;
            chicagoCity.ResearchStation = true;
            Assert.AreEqual(false, scientist.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.ResearchStation = false;
        }

        [TestMethod]
        public void TestCureSimpleAlreadyCured()
        {
            hand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.Hand = hand;
            List<String> cardsToSpend = new List<String> { newYork.CityName, atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName };
            List<Card> correctHand = new List<Card> { chennai, newYork, atlanta, chicagoCard, london, paris, airlift };
            opExpert.CurrentCity = chicagoCity;
            chicagoCity.ResearchStation = true;
            GameBoardModels.SetCureStatus(Color.Blue, Cures.Curestate.Cured);
            Assert.AreEqual(false, opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, hand);
            chicagoCity.ResearchStation = false;
            GameBoardModels.SetCureStatus(Color.Blue,Cures.Curestate.NotCured);

        }


        #endregion

        #region Treat Diseases

        [TestMethod]
        public void TestTreatDiseaseBasicBlue()
        {
            opExpert.CurrentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Blue) , 1);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicRed()
        {
            opExpert.CurrentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(Color.Red, 2);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Red), 1);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicONLYellow()
        {
            opExpert.CurrentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(Color.Red, 2);
            chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            chicagoCity.Cubes.SetCubeCount(Color.Yellow, 2);
            chicagoCity.Cubes.SetCubeCount(Color.Black, 3);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Yellow));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Red), 2);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Blue), 2);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Yellow), 1);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Black), 3);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicDecreaseAll()
        {
            opExpert.CurrentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(Color.Red, 1);
            chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            chicagoCity.Cubes.SetCubeCount(Color.Yellow, 2);
            chicagoCity.Cubes.SetCubeCount(Color.Black, 1);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Yellow));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Black));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Red), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Blue), 1);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Yellow), 1);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Black), 0);
        }

        [TestMethod]
        public void TestTreateDiesaseCuresExist()
        {
            opExpert.CurrentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(Color.Red, 1);
            chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            chicagoCity.Cubes.SetCubeCount(Color.Yellow, 2);
            chicagoCity.Cubes.SetCubeCount(Color.Black, 3);
            GameBoardModels.SetCureStatus(Color.Black, Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Blue, Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Red, Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Yellow, Cures.Curestate.Cured);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Yellow));
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Black));
            GameBoardModels.SetCureStatus(Color.Black, Cures.Curestate.NotCured);
            GameBoardModels.SetCureStatus(Color.Blue, Cures.Curestate.NotCured);
            GameBoardModels.SetCureStatus(Color.Red, Cures.Curestate.NotCured);
            GameBoardModels.SetCureStatus(Color.Yellow, Cures.Curestate.NotCured);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Red), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Blue), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Yellow), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Black), 0);
        }

        [TestMethod]
        public void TestTreatDiseaseMedicDecreaseAll()
        {
            medic.CurrentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(Color.Red, 1);
            chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            chicagoCity.Cubes.SetCubeCount(Color.Yellow, 3);
            chicagoCity.Cubes.SetCubeCount(Color.Black, 1);
            Assert.AreEqual(true, medic.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(true, medic.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(true, medic.TreatDiseaseOption(Color.Yellow));
            Assert.AreEqual(true, medic.TreatDiseaseOption(Color.Black));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Red), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Blue), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Yellow), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Black), 0);
        }

        [TestMethod]
        public void TestTreatDiseaseZero()
        {
            opExpert.CurrentCity = chicagoCity;
            medic.CurrentCity = chicagoCity;
            chicagoCity.Cubes.SetCubeCount(Color.Red, 0);
            chicagoCity.Cubes.SetCubeCount(Color.Blue, 0);
            chicagoCity.Cubes.SetCubeCount(Color.Yellow, 0);
            chicagoCity.Cubes.SetCubeCount(Color.Black, 0);
            Assert.AreEqual(false, opExpert.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(false, opExpert.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(false, medic.TreatDiseaseOption(Color.Yellow));
            Assert.AreEqual(false, medic.TreatDiseaseOption(Color.Black));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Red), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Blue), 0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Yellow),0);
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Black), 0);
        }

        #endregion

        #region TradeCards

        public void assertTradeCardWorks(AbstractPlayer from, AbstractPlayer to, List<Card> fromHand, List<Card> toHand, 
            City fromCity, City toCity, bool success)
        {
            to.Hand = toHand;
            from.Hand = fromHand;
            to.CurrentCity = toCity;
            from.CurrentCity = fromCity;
            Assert.AreEqual(success, from.ShareKnowledgeOption(to, chicagoCard.CityName));
            if (success)
            {
                fromHand.Remove(chicagoCard);
                toHand.Add(chicagoCard);
            }
            CollectionAssert.AreEqual(to.Hand, toHand);
            CollectionAssert.AreEqual(from.Hand, fromHand);
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
            scientist.CurrentCity = chicagoCity;
            Assert.AreEqual(scientist.DispatcherMovePlayer(players, toCity), success);
            Assert.AreEqual(scientist.CurrentCity.Name, success ? toCity.Name : chicagoCity.Name);
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
            opExpert.CurrentCity = bangkok;
            assertScientistMoveCorrect(bangkok, true);
            opExpert.CurrentCity = chicagoCity;
        }

        [TestMethod]
        public void TestDispatcherMoveShuttleFlight()
        {
            chicagoCity.ResearchStation = true;
            bangkok.ResearchStation = true;
            assertScientistMoveCorrect(bangkok, true);
            chicagoCity.ResearchStation = false;
            bangkok.ResearchStation = false;
        }


        #endregion

        [TestMethod]
        public void TestIncrementAfterCureDiseaseBasicBlue()
        {
            opExpert.CurrentCity = chicagoCity;
            GameBoardModels.SetInfectionCubeCount(Color.Blue, 22);
            chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Blue), 1);
            Assert.AreEqual(GameBoardModels.GetInfectionCubeCount(Color.Blue), 23);
        }

        [TestMethod]
        public void TestIncrementAfterCureDiseaseRed()
        {
            opExpert.CurrentCity = chicagoCity;
            GameBoardModels.SetInfectionCubeCount(Color.Red, 22);
            chicagoCity.Cubes.SetCubeCount(Color.Red, 2);
            Assert.AreEqual(true, opExpert.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Red), 1);
            Assert.AreEqual(GameBoardModels.GetInfectionCubeCount(Color.Red), 23);
        }

        [TestMethod]
        public void TestIncrementAfterMedicCureDiseaseRed()
        {
            medic.CurrentCity = chicagoCity;
            GameBoardModels.SetInfectionCubeCount(Color.Red, 22);
            chicagoCity.Cubes.SetCubeCount(Color.Red, 2);
            Assert.AreEqual(true, medic.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(chicagoCity.Cubes.GetCubeCount(Color.Red), 0);
            Assert.AreEqual(GameBoardModels.GetInfectionCubeCount(Color.Red), 24);
        }

        [TestMethod]
        public void TestDiseaseContainmentOnMove()
        {
            containmentSpecialst.CurrentCity = chicagoCity;
            GameBoardModels.SetInfectionCubeCount(Color.Red, 10);
            sanFran.Cubes.SetCubeCount(Color.Red, 4);
            Assert.AreEqual(4, sanFran.Cubes.GetCubeCount(Color.Red));
            bool success = containmentSpecialst.MovePlayer(sanFran);
            Assert.IsTrue(success);
            Assert.AreEqual(3, sanFran.Cubes.GetCubeCount(Color.Red));

        }

        [TestMethod]
        public void Archivist8CardHand()
        {
            archivist.Hand = new List<Card> { atlanta, chicagoCard, london, paris, milan, chennai, newYork };
            archivist.AddCardToHand(airlift);

            Assert.IsTrue(archivist.Hand.Count == 8);
        }

        [TestMethod]
        public void TestDrawCardFromDiscardArchivist()
        {
            archivist.CurrentCity = Create.CityDictionary["Delhi"];
            archivist.Hand = new List<Card> {chicagoCard};
            archivist.MovePlayer(chicagoCity);
            archivist.Hand = new List<Card>();
            Assert.IsTrue(archivist.Hand.Count == 0);
            
            archivist.PreformSpecialAction("ReclaimCityCard", new MockGameBoardView());
            Assert.IsTrue(archivist.Hand.Contains(chicagoCard));
        }

        /**
        [TestMethod]
        public void TestFieldOperativeCure()
        {
            fieldOperative.currentCity = Create.cityDictionary["delhi"];
            Create.cityDictionary["delhi"].Cubes.AddCubes(COLOR.black, 3);
            fieldOperative.PreformSpecialAction("Add Cube to Card", new MockGameBoardView());

        }*/



        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestWinCon()
        {
            List<String> cardsToSpend = new List<String> { atlanta.CityName, chicagoCard.CityName, london.CityName, paris.CityName };
            GameBoardModels.SetCureStatus(Color.Blue , Cures.Curestate.NotCured);
            GameBoardModels.SetCureStatus(Color.Yellow, Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Red, Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Black, Cures.Curestate.Cured);
            scientist.Hand = new List<Card> { atlanta, chicagoCard, london, paris };
            scientist.Cure(cardsToSpend, Color.Blue);

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
