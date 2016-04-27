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
        private City _chicagoCity;
        private City _bangkok;
        private City _kolkata;
        private City _sanFran;
        private List<Card> _hand;
        private List<Card> _pile;
        private List<AbstractPlayer> _players;
        private Card _chennai;
        private Card _newYork;
        private Card _atlanta;
        private Card _chicagoCard;
        private Card _london;
        private Card _paris;
        private Card _milan;
        private Card _airlift;
        private AbstractPlayer _dispatcher;
        private AbstractPlayer _medic;
        private AbstractPlayer _opExpert;
        private AbstractPlayer _researcher;
        private AbstractPlayer _scientist;
        private AbstractPlayer _containmentSpecialst;
        private AbstractPlayer _troubleshooter;
        private AbstractPlayer _fieldOperative;
        private AbstractPlayer _generalist;
        private AbstractPlayer _archivist;
        private GameBoardModels _gameBoardModels;


        [TestInitialize]
        public void SetupPlayer()
        {
            //set up GameboardModels if not already
            _gameBoardModels = new GameBoardModels(new string[] { "Dispatcher", "Medic" });
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
            //Cards
            _chennai = new Card("Chennai", Card.Cardtype.City, Color.Black);
            _newYork = new Card("New York", Card.Cardtype.City, Color.Blue);
            _atlanta = new Card("Atlanta", Card.Cardtype.City, Color.Blue);
            _chicagoCard = new Card("Chicago", Card.Cardtype.City, Color.Blue);
            _paris = new Card("Paris", Card.Cardtype.City, Color.Blue);
            _london = new Card("London", Card.Cardtype.City, Color.Blue);
            _milan = new Card("Milan", Card.Cardtype.City, Color.Blue);
            _airlift = new Card("Airlift",Card.Cardtype.Special);
           
            //Players
            _scientist = new ScientistPlayer();
            _opExpert = new OpExpertPlayer();
            _researcher = new ResearcherPlayer();
            _medic = new MedicPlayer();
            _containmentSpecialst = new ContainmentSpecialstPlayer();
            _troubleshooter = new TroubleshooterPlayer();
            _fieldOperative = new FieldOperativePlayer();
            _generalist = new GeneralistPlayer();
            _archivist = new ArchivistPlayer();
            _players = new List<AbstractPlayer> { _scientist, _opExpert, _researcher, _medic };
        }

        [TestMethod]
        public void TestDriveOptions()
        {
            var returnedSet = AbstractPlayer.DriveOptions(_chicagoCity);
            var correctSet = _chicagoCity.GetAdjacentCities();
            CollectionAssert.AreEqual(returnedSet.ToArray(), correctSet.ToArray());
        }

        #region Direct Flight

        public void AssertDirectFlightOptionIsCorrect(List<Card> hand, List<string> correctList)
        {
            List<string> returnList = AbstractPlayer.DirectFlightOption(hand, _chicagoCity);
            CollectionAssert.AreEqual(correctList, returnList);
        }

        [TestMethod]
        public void TestDirectFlightOptionsNone()
        {
            AssertDirectFlightOptionIsCorrect(new List<Card>(), new List<string>());
        }

        [TestMethod]
        public void TestDirectFlightOptionNewYork()
        {
            AssertDirectFlightOptionIsCorrect(new List<Card> { _newYork }, new List<string> { _newYork.CityName });
        }

        [TestMethod]
        public void TestDirectFlightCurrentCityChicago()
        {
            AssertDirectFlightOptionIsCorrect(new List<Card> { _chicagoCard }, new List<string>());
        }

        [TestMethod]
        public void TestDirectFlightMultipleCities()
        {
            AssertDirectFlightOptionIsCorrect(new List<Card> { _chicagoCard, _atlanta, _chennai }, 
                new List<string> { _atlanta.CityName, _chennai.CityName });
        }

        [TestMethod]
        public void TestDirectFlightWithNonCityCardInHand()
        {
            AssertDirectFlightOptionIsCorrect(new List<Card> { _airlift, _atlanta, _chennai }, 
                new List<string> { _atlanta.CityName, _chennai.CityName });
        }
        #endregion

        #region Charter Flight
        [TestMethod]
        public void TestCharterFlightFalseOption()
        {
            _hand = new List<Card> { _airlift, _atlanta, _chennai };
            var returendBool = AbstractPlayer.CharterFlightOption(_hand, _chicagoCity);
            const bool correctBool = false;
            Assert.AreEqual(correctBool, returendBool);
        }

        [TestMethod]
        public void TestCharterFlightTrue()
        {
            _hand = new List<Card> { _airlift, _atlanta, _chicagoCard };
            var returendBool = AbstractPlayer.CharterFlightOption(_hand, _chicagoCity);
            const bool correctBool = true;
            Assert.AreEqual(correctBool, returendBool);
        }
        #endregion

        #region Shuttle Flight
        [TestMethod]
        public void TestShuttleFlightNotInStation()
        {
            _scientist.CurrentCity = _kolkata;
            var result = AbstractPlayer.ShuttleFlightOption(_kolkata);
            CollectionAssert.Equals(result, new List<string>());
        }

        [TestMethod]
        public void TestShuttleFlightNoOptions()
        {
            _scientist.CurrentCity = _kolkata;
            _kolkata.ResearchStation = true;
            var result = AbstractPlayer.ShuttleFlightOption(_kolkata);
            _kolkata.ResearchStation = false;
            var expected = new List<string> { "Atlanta"};
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestShuttleFlightOneOption()
        {
            _scientist.CurrentCity = _kolkata;
            _kolkata.ResearchStation = true;
            _bangkok.ResearchStation = true;
            var result = AbstractPlayer.ShuttleFlightOption(_kolkata);
            _kolkata.ResearchStation = false;
            _bangkok.ResearchStation = false;
            var expected = new List<string> { "Atlanta", "Bangkok" };
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestShuttleFlightMultipleOptions()
        {
            _scientist.CurrentCity = _chicagoCity;
            _chicagoCity.Name = "Chicago";
            _chicagoCity.ResearchStation = true;
            _kolkata.ResearchStation = true;
            _bangkok.ResearchStation = true;
            List<string> result = AbstractPlayer.ShuttleFlightOption(_chicagoCity);
            _chicagoCity.ResearchStation = false;
            _kolkata.ResearchStation = false;
            _bangkok.ResearchStation = false;
            List<string> expected = new List<string> { "Atlanta", "Kolkata", "Bangkok" };
            CollectionAssert.AreEqual(expected, result);
        }
        #endregion

        #region Move Player

        public void AssertMovePlayerCorrect(City currnetCity, List<Card> hand, List<Card> correctHand, City moveTo)
        {
            _scientist.CurrentCity = currnetCity;
            _scientist.Hand = hand;
            Assert.AreEqual(true, _scientist.MovePlayer(moveTo));
            Assert.AreEqual(_scientist.CurrentCity.Name, moveTo.Name);
            CollectionAssert.AreEqual(correctHand, hand);
        }

        [TestMethod]
        public void TestMoverPlayerAdjacentCitySanFran()
        {
            AssertMovePlayerCorrect(_chicagoCity, new List<Card> { _airlift, _chicagoCard, _chennai },
                new List<Card> { _airlift, _chicagoCard, _chennai }, _sanFran);
        }

        [TestMethod]
        public void TestMoverPlayerAdjacentCityChicagoWithCard()
        {
            AssertMovePlayerCorrect(_sanFran, new List<Card> { _airlift, _chicagoCard, _chennai },
                new List<Card> { _airlift, _chicagoCard, _chennai }, _chicagoCity);
        }

        [TestMethod]
        public void TestMoverPlayerDirectFlight()
        {
            AssertMovePlayerCorrect(_bangkok, new List<Card> { _airlift, _chicagoCard, _chennai, _atlanta },
                new List<Card> { _airlift, _chennai, _atlanta }, _chicagoCity);
        }

        [TestMethod]
        public void TestMoverPlayerCharterFlight()
        {
            AssertMovePlayerCorrect(_chicagoCity, new List<Card> { _airlift, _chicagoCard, _chennai },
                new List<Card> { _airlift, _chennai }, _bangkok);
        }

        [TestMethod]
        public void TestMoverPlayerShuttleFlightPreemptCard()
        {
            _chicagoCity.ResearchStation = true;
            _bangkok.ResearchStation = true;
            AssertMovePlayerCorrect(_chicagoCity, new List<Card> { _airlift, _atlanta, _chennai, _chicagoCard },
                new List<Card> { _airlift, _atlanta, _chennai, _chicagoCard }, _bangkok);
            _chicagoCity.ResearchStation = false;
            _bangkok.ResearchStation = false;
        }

        [TestMethod]
        public void TestMoverPlayerInvalid()
        {
            _scientist.CurrentCity = _chicagoCity;
            _hand = new List<Card> { _airlift, _atlanta, _chennai };
            _pile = new List<Card>();
            _scientist.Hand = _hand;
            Assert.AreEqual(false, _scientist.MovePlayer(_bangkok));
            var correctHand = new List<Card> { _airlift, _atlanta, _chennai };
            Assert.AreEqual(_scientist.CurrentCity.Name, _chicagoCity.Name);
            CollectionAssert.AreEqual(correctHand, _hand);
        }

        #endregion

        #region BuildAResearchStation

        public void AssertBuildStationCorrect(AbstractPlayer player, City currentCity, List<Card> hand, List<Card> correctHand, bool success)
        {
            player.CurrentCity = currentCity;
            currentCity.ResearchStation = !success && currentCity.ResearchStation;
            player.Hand = hand;
            Assert.AreEqual(success, player.BuildAResearchStationOption());
            CollectionAssert.AreEqual(correctHand, hand);
            if (success)
            {
                Assert.AreEqual(true, _chicagoCity.ResearchStation);
            }
            _chicagoCity.ResearchStation = false;
        }

        [TestMethod]
        public void BuildStationNormal()
        {
            AssertBuildStationCorrect(_scientist, _chicagoCity, new List<Card> { _airlift, _chicagoCard, _chennai },
                new List<Card> { _airlift, _chennai }, true);
        }

        [TestMethod]
        public void BuildStationFailLackCard()
        {
            AssertBuildStationCorrect(_scientist, _chicagoCity, new List<Card> { _airlift, _chennai },
                new List<Card> { _airlift, _chennai }, false);
        }

        [TestMethod]
        public void BuildStationFailExisting()
        {
            _chicagoCity.ResearchStation = true;
            AssertBuildStationCorrect(_scientist, _chicagoCity, new List<Card> { _airlift, _chicagoCard, _chennai },
                new List<Card> { _airlift, _chicagoCard, _chennai }, false);
        }

        [TestMethod]
        public void BuildStationOpExpert()
        {
            AssertBuildStationCorrect(_opExpert, _chicagoCity, new List<Card> { _airlift, _chicagoCard, _chennai },
                new List<Card> { _airlift, _chicagoCard, _chennai }, true);
        }

        [TestMethod]
        public void BuildStationOpExpertWithoutCard()
        {
            AssertBuildStationCorrect(_opExpert, _chicagoCity, new List<Card> { _airlift, _chennai },
                new List<Card> { _airlift, _chennai }, true);
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
            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
            _opExpert.Hand = _hand;
            var cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName };
            var correctHand = new List<Card> { _chennai, _airlift };
            _opExpert.CurrentCity = _chicagoCity;
            _chicagoCity.ResearchStation = true;
            Assert.AreEqual(true, _opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, _hand);
            _chicagoCity.ResearchStation = false;
            Assert.AreEqual(GameBoardModels.GetCureStatus(Color.Blue), Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Blue, Cures.Curestate.NotCured);
            
        }

        [TestMethod]
        public void TestCureNotEnoughCards()
        {
            _hand = new List<Card> { _newYork, _chennai, _atlanta, _chicagoCard, _airlift };
            _opExpert.Hand = _hand;
            _opExpert.CurrentCity = _chicagoCity;
            _chicagoCity.ResearchStation = true;
            var cardsToSpend = new List<string> { _newYork.CityName, _chicagoCard.CityName };
            var correctHand = new List<Card> { _newYork, _chennai, _atlanta, _chicagoCard, _airlift };
            Assert.AreEqual(false, _opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, _hand);
            _chicagoCity.ResearchStation = false;
        }

        [TestMethod]
        public void TestCureToManyValidCards()
        {
            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _milan, _airlift };
            _opExpert.Hand = _hand;
            var cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName, _milan.CityName };
            var correctHand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _milan, _airlift };
            _opExpert.CurrentCity = _chicagoCity;
            _chicagoCity.ResearchStation = true;
            Assert.AreEqual(false, _opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, _hand);
            _chicagoCity.ResearchStation = false;
        }

        [TestMethod]
        public void TestCureInvalidCards()
        {
            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
            _opExpert.Hand = _hand;
            var cardsToSpend = new List<string> { _chennai.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName };
            var correctHand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
            _opExpert.CurrentCity = _chicagoCity;
            _chicagoCity.ResearchStation = true;
            Assert.AreEqual(false, _opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, _hand);
            _chicagoCity.ResearchStation = false;
        }

        [TestMethod]
        public void TestCureNotInResearchStation()
        {
            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
            _opExpert.Hand = _hand;
            var cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName };
            var correctHand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
            _opExpert.CurrentCity = _chicagoCity;
            _chicagoCity.ResearchStation = false;
            Assert.AreEqual(false, _opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, _hand);
        }

        [TestMethod]
        public void TestCureSimpleScientist()
        {
            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
            _scientist.Hand = _hand;
            var cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName };
            var correctHand = new List<Card> { _chennai, _paris, _airlift };
            _scientist.CurrentCity = _chicagoCity;
            _chicagoCity.ResearchStation = true;
            Assert.AreEqual(true, _scientist.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, _hand);
            _chicagoCity.ResearchStation = false;
            Assert.AreEqual(GameBoardModels.GetCureStatus(Color.Blue), Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Blue, Cures.Curestate.NotCured);
        }

        [TestMethod]
        public void TestCureToManyValidCardsScientist()
        {
            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
            _scientist.Hand = _hand;
            var cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName};
            var correctHand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
            _scientist.CurrentCity = _chicagoCity;
            _chicagoCity.ResearchStation = true;
            Assert.AreEqual(false, _scientist.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, _hand);
            _chicagoCity.ResearchStation = false;
        }

        [TestMethod]
        public void TestCureSimpleAlreadyCured()
        {
            _hand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
            _opExpert.Hand = _hand;
            var cardsToSpend = new List<string> { _newYork.CityName, _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName };
            var correctHand = new List<Card> { _chennai, _newYork, _atlanta, _chicagoCard, _london, _paris, _airlift };
            _opExpert.CurrentCity = _chicagoCity;
            _chicagoCity.ResearchStation = true;
            GameBoardModels.SetCureStatus(Color.Blue, Cures.Curestate.Cured);
            Assert.AreEqual(false, _opExpert.Cure(cardsToSpend, Color.Blue));
            CollectionAssert.AreEqual(correctHand, _hand);
            _chicagoCity.ResearchStation = false;
            GameBoardModels.SetCureStatus(Color.Blue,Cures.Curestate.NotCured);

        }


        #endregion

        #region Treat Diseases

        [TestMethod]
        public void TestTreatDiseaseBasicBlue()
        {
            _opExpert.CurrentCity = _chicagoCity;
            _chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Blue) , 1);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicRed()
        {
            _opExpert.CurrentCity = _chicagoCity;
            _chicagoCity.Cubes.SetCubeCount(Color.Red, 2);
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Red), 1);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicOnlYellow()
        {
            _opExpert.CurrentCity = _chicagoCity;
            _chicagoCity.Cubes.SetCubeCount(Color.Red, 2);
            _chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            _chicagoCity.Cubes.SetCubeCount(Color.Yellow, 2);
            _chicagoCity.Cubes.SetCubeCount(Color.Black, 3);
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Yellow));
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Red), 2);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Blue), 2);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Yellow), 1);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Black), 3);
        }

        [TestMethod]
        public void TestTreatDiseaseBasicDecreaseAll()
        {
            _opExpert.CurrentCity = _chicagoCity;
            _chicagoCity.Cubes.SetCubeCount(Color.Red, 1);
            _chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            _chicagoCity.Cubes.SetCubeCount(Color.Yellow, 2);
            _chicagoCity.Cubes.SetCubeCount(Color.Black, 1);
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Yellow));
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Black));
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Red), 0);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Blue), 1);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Yellow), 1);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Black), 0);
        }

        [TestMethod]
        public void TestTreateDiesaseCuresExist()
        {
            _opExpert.CurrentCity = _chicagoCity;
            _chicagoCity.Cubes.SetCubeCount(Color.Red, 1);
            _chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            _chicagoCity.Cubes.SetCubeCount(Color.Yellow, 2);
            _chicagoCity.Cubes.SetCubeCount(Color.Black, 3);
            GameBoardModels.SetCureStatus(Color.Black, Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Blue, Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Red, Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Yellow, Cures.Curestate.Cured);
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Yellow));
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Black));
            GameBoardModels.SetCureStatus(Color.Black, Cures.Curestate.NotCured);
            GameBoardModels.SetCureStatus(Color.Blue, Cures.Curestate.NotCured);
            GameBoardModels.SetCureStatus(Color.Red, Cures.Curestate.NotCured);
            GameBoardModels.SetCureStatus(Color.Yellow, Cures.Curestate.NotCured);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Red), 0);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Blue), 0);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Yellow), 0);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Black), 0);
        }

        [TestMethod]
        public void TestTreatDiseaseMedicDecreaseAll()
        {
            _medic.CurrentCity = _chicagoCity;
            _chicagoCity.Cubes.SetCubeCount(Color.Red, 1);
            _chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            _chicagoCity.Cubes.SetCubeCount(Color.Yellow, 3);
            _chicagoCity.Cubes.SetCubeCount(Color.Black, 1);
            Assert.AreEqual(true, _medic.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(true, _medic.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(true, _medic.TreatDiseaseOption(Color.Yellow));
            Assert.AreEqual(true, _medic.TreatDiseaseOption(Color.Black));
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Red), 0);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Blue), 0);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Yellow), 0);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Black), 0);
        }

        [TestMethod]
        public void TestTreatDiseaseZero()
        {
            _opExpert.CurrentCity = _chicagoCity;
            _medic.CurrentCity = _chicagoCity;
            _chicagoCity.Cubes.SetCubeCount(Color.Red, 0);
            _chicagoCity.Cubes.SetCubeCount(Color.Blue, 0);
            _chicagoCity.Cubes.SetCubeCount(Color.Yellow, 0);
            _chicagoCity.Cubes.SetCubeCount(Color.Black, 0);
            Assert.AreEqual(false, _opExpert.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(false, _opExpert.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(false, _medic.TreatDiseaseOption(Color.Yellow));
            Assert.AreEqual(false, _medic.TreatDiseaseOption(Color.Black));
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Red), 0);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Blue), 0);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Yellow),0);
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Black), 0);
        }

        #endregion

        #region TradeCards

        public void AssertTradeCardWorks(AbstractPlayer from, AbstractPlayer to, List<Card> fromHand, List<Card> toHand, 
            City fromCity, City toCity, bool success)
        {
            to.Hand = toHand;
            from.Hand = fromHand;
            to.CurrentCity = toCity;
            from.CurrentCity = fromCity;
            Assert.AreEqual(success, from.ShareKnowledgeOption(to, _chicagoCard.CityName));
            if (success)
            {
                fromHand.Remove(_chicagoCard);
                toHand.Add(_chicagoCard);
            }
            CollectionAssert.AreEqual(to.Hand, toHand);
            CollectionAssert.AreEqual(from.Hand, fromHand);
        }

        [TestMethod]
        public void TestShareChicagosimple()
        {
            AssertTradeCardWorks(_opExpert, _scientist, new List<Card> { _atlanta, _london, _chicagoCard },
                new List<Card> { _chennai, _newYork }, _chicagoCity, _chicagoCity, true);
        }

        [TestMethod]
        public void TestShareChicagoDiffrentCityFail()
        {
            AssertTradeCardWorks(_opExpert, _scientist, new List<Card> { _atlanta, _london, _chicagoCard },
                new List<Card> { _chennai, _newYork }, _bangkok, _chicagoCity, false);
        }

        [TestMethod]
        public void TestShareChicagoinBangkokFail()
        {
            AssertTradeCardWorks(_opExpert, _scientist, new List<Card> { _atlanta, _london, _chicagoCard },
                new List<Card> { _chennai, _newYork }, _bangkok, _bangkok, false);
        }

        [TestMethod]
        public void TestShareChicagoinBangkokResearcherPass()
        {
            AssertTradeCardWorks(_researcher, _scientist, new List<Card> { _atlanta, _london, _chicagoCard },
                new List<Card> { _chennai, _newYork }, _bangkok, _bangkok, true);
        }

        [TestMethod]
        public void TestShareChicagoMissingCardFail()
        {
            AssertTradeCardWorks(_opExpert, _scientist, new List<Card> { _atlanta, _london },
                new List<Card> { _chennai, _newYork }, _chicagoCity, _chicagoCity, false);
        }

        #endregion

        #region DispatcherMove

        public void AssertScientistMoveCorrect(City toCity, bool success)
        {
            _scientist.CurrentCity = _chicagoCity;
            Assert.AreEqual(_scientist.DispatcherMovePlayer(_players, toCity), success);
            Assert.AreEqual(_scientist.CurrentCity.Name, success ? toCity.Name : _chicagoCity.Name);
        }

        [TestMethod]
        public void TestDispatcherMoveAdjacentCitySanFran()
        {
            AssertScientistMoveCorrect(_chicagoCity, true);
        }

        [TestMethod]
        public void TestDispatcherMoveInvalidCityKolkata()
        {
            AssertScientistMoveCorrect(_kolkata, false);
        }

        [TestMethod]
        public void TestDispatcherMoveToOtherPlayer()
        {
            _opExpert.CurrentCity = _bangkok;
            AssertScientistMoveCorrect(_bangkok, true);
            _opExpert.CurrentCity = _chicagoCity;
        }

        [TestMethod]
        public void TestDispatcherMoveShuttleFlight()
        {
            _chicagoCity.ResearchStation = true;
            _bangkok.ResearchStation = true;
            AssertScientistMoveCorrect(_bangkok, true);
            _chicagoCity.ResearchStation = false;
            _bangkok.ResearchStation = false;
        }


        #endregion

        [TestMethod]
        public void TestIncrementAfterCureDiseaseBasicBlue()
        {
            _opExpert.CurrentCity = _chicagoCity;
            GameBoardModels.SetInfectionCubeCount(Color.Blue, 22);
            _chicagoCity.Cubes.SetCubeCount(Color.Blue, 2);
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Blue));
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Blue), 1);
            Assert.AreEqual(GameBoardModels.GetInfectionCubeCount(Color.Blue), 23);
        }

        [TestMethod]
        public void TestIncrementAfterCureDiseaseRed()
        {
            _opExpert.CurrentCity = _chicagoCity;
            GameBoardModels.SetInfectionCubeCount(Color.Red, 22);
            _chicagoCity.Cubes.SetCubeCount(Color.Red, 2);
            Assert.AreEqual(true, _opExpert.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Red), 1);
            Assert.AreEqual(GameBoardModels.GetInfectionCubeCount(Color.Red), 23);
        }

        [TestMethod]
        public void TestIncrementAfterMedicCureDiseaseRed()
        {
            _medic.CurrentCity = _chicagoCity;
            GameBoardModels.SetInfectionCubeCount(Color.Red, 22);
            _chicagoCity.Cubes.SetCubeCount(Color.Red, 2);
            Assert.AreEqual(true, _medic.TreatDiseaseOption(Color.Red));
            Assert.AreEqual(_chicagoCity.Cubes.GetCubeCount(Color.Red), 0);
            Assert.AreEqual(GameBoardModels.GetInfectionCubeCount(Color.Red), 24);
        }

        [TestMethod]
        public void TestDiseaseContainmentOnMove()
        {
            _containmentSpecialst.CurrentCity = _chicagoCity;
            GameBoardModels.SetInfectionCubeCount(Color.Red, 10);
            _sanFran.Cubes.SetCubeCount(Color.Red, 4);
            Assert.AreEqual(4, _sanFran.Cubes.GetCubeCount(Color.Red));
            var success = _containmentSpecialst.MovePlayer(_sanFran);
            Assert.IsTrue(success);
            Assert.AreEqual(3, _sanFran.Cubes.GetCubeCount(Color.Red));

        }

        [TestMethod]
        public void Archivist8CardHand()
        {
            _archivist.Hand = new List<Card> { _atlanta, _chicagoCard, _london, _paris, _milan, _chennai, _newYork };
            _archivist.AddCardToHand(_airlift);

            Assert.IsTrue(_archivist.Hand.Count == 8);
        }

        [TestMethod]
        public void TestDrawCardFromDiscardArchivist()
        {
            _archivist.CurrentCity = Create.CityDictionary["Delhi"];
            _archivist.Hand = new List<Card> {_chicagoCard};
            _archivist.MovePlayer(_chicagoCity);
            _archivist.Hand = new List<Card>();
            Assert.IsTrue(_archivist.Hand.Count == 0);
            
            _archivist.PreformSpecialAction("ReclaimCityCard", new MockGameBoardView());
            Assert.IsTrue(_archivist.Hand.Contains(_chicagoCard));
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
            var cardsToSpend = new List<string> { _atlanta.CityName, _chicagoCard.CityName, _london.CityName, _paris.CityName };
            GameBoardModels.SetCureStatus(Color.Blue , Cures.Curestate.NotCured);
            GameBoardModels.SetCureStatus(Color.Yellow, Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Red, Cures.Curestate.Cured);
            GameBoardModels.SetCureStatus(Color.Black, Cures.Curestate.Cured);
            _scientist.Hand = new List<Card> { _atlanta, _chicagoCard, _london, _paris };
            _scientist.Cure(cardsToSpend, Color.Blue);

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

    internal class MockGameBoardView : GameBoard
    {

    }

    /** PRINTING STUFF
    //Print Statment
    foreach (String name in returnList)
    {
        Console.Out.Write(name);
    }**/
}
