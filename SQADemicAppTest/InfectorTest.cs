using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQADemicApp.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using SQADemicApp;

namespace SQADemicAppTest
{
    [TestClass]
    public class InfectorTest
    {
        private LinkedList<string> _deck;
        private LinkedList<string> _pile;
        private int _infectionRate;
        private int _infectionIndex;
        private const int Maxcubecount = 24;

        [TestInitialize]
        public void SetUpArrays()
        {
            new GameBoardModels(new string[] { "Dispatcher", "Operations Expert" });
            GameBoardModels.SetOutbreakMarker(0);
            GameBoardModels.SetInfectionCubeCount(Color.Red, Maxcubecount);
            GameBoardModels.SetInfectionCubeCount(Color.Black, Maxcubecount);
            GameBoardModels.SetInfectionCubeCount(Color.Blue, Maxcubecount);
            GameBoardModels.SetInfectionCubeCount(Color.Yellow, Maxcubecount);
            _deck = new LinkedList<string>();
            _pile = new LinkedList<string>();
            _infectionRate = 3;
            _infectionIndex = 4;
        }

        [TestMethod]
        public void TestInfectTwoCities()
        {
            _deck.Clear(); 
            _deck.AddFirst("Saint Petersburg");
            _deck.AddFirst("Sydney");
            var removedCities = InfectorBL.InfectCities(_deck, _pile, 2);
            var answer = new List<string> { "Sydney", "Saint Petersburg" };
            CollectionAssert.AreEqual(answer, removedCities);
        }

        [TestMethod]
        public void TestInfectThreeCities()
        {
            _deck.Clear();
            _deck.AddFirst("Saint Petersburg");
            _deck.AddFirst("Sydney");
            _deck.AddFirst("New York");
            var removedCities = InfectorBL.InfectCities(_deck, _pile, 3);
            var answer = new List<string> { "New York", "Sydney", "Saint Petersburg" };
            CollectionAssert.AreEqual(answer, removedCities);
        }

        [TestMethod]
        public void TestInfectTwoCitiesTwice()
        {
            _deck.Clear();
            _deck.AddFirst("Saint Petersburg");
            _deck.AddFirst("Sydney");
            var removedCities = InfectorBL.InfectCities(_deck, _pile, 2);
            var answer = new List<string> { "Sydney", "Saint Petersburg" };
            CollectionAssert.AreEqual(answer, removedCities);
            _deck.AddFirst("New York");
            _deck.AddFirst("Chicago");
            removedCities = InfectorBL.InfectCities(_deck, _pile, 2);
            answer = new List<string> { "Chicago", "New York" };
            CollectionAssert.AreEqual(answer, removedCities);

        }

        [TestMethod]
        public void TestInfectTwoCitiesUpdatePile()
        {
            _deck.Clear(); 
            _deck.AddFirst("Saint Petersburg");
            _deck.AddFirst("Sydney");
            _pile = new LinkedList<string>();
            var removedCities = InfectorBL.InfectCities(_deck, _pile, 2);
            var answer = new LinkedList<string>();
            answer.AddFirst("Sydney");
            answer.AddFirst("Saint Petersburg");
            CollectionAssert.AreEqual(answer, _pile);
        }

        [TestMethod]
        public void TestEpidemicIncreaseInfectionCounter2To2()
        {
            _deck.Clear();
            //InfectionRates = { 2, 2, 2, 3, 3, 4, 4 };
            _deck.AddFirst("Chicago");
            _infectionIndex = 1;
            GameBoardModels.SetOutbreakMarker(0);
            InfectorBL.Epidemic(_deck, _pile, ref _infectionIndex, ref _infectionRate);
            Assert.AreEqual(2, _infectionRate);
        }

        [TestMethod]
        public void TestEpidemicIncreaseInfectionCounter2To3()
        {
            //InfectionRates = { 2, 2, 2, 3, 3, 4, 4 };
            _deck.Clear(); 
            _deck.AddFirst("Chicago");
            _infectionIndex = 2;
            GameBoardModels.SetOutbreakMarker(0);
            InfectorBL.Epidemic(_deck, _pile, ref _infectionIndex, ref _infectionRate);
            Assert.AreEqual(3, _infectionRate);
        }

        [TestMethod]
        public void TestEpidemicIncreaseInfectionCounter3To4()
        {
            //InfectionRates = { 2, 2, 2, 3, 3, 4, 4 };
            _deck.AddFirst("Chicago");
            _infectionRate = 3;
            _infectionIndex = 4;
            GameBoardModels.SetOutbreakMarker(0);
            InfectorBL.Epidemic(_deck, _pile, ref _infectionIndex, ref _infectionRate);
            Assert.AreEqual(4, _infectionRate);
        }

        [TestMethod]
        public void TestEpidemicIncreaseInfectionIndex()
        {
            _deck.AddFirst("Chicago");
            _infectionRate = 3;
            _infectionIndex = 4;
            GameBoardModels.SetOutbreakMarker(0);
            InfectorBL.Epidemic(_deck, _pile, ref _infectionIndex, ref _infectionRate);
            Assert.AreEqual(5, _infectionIndex);
        }

        [TestMethod]
        public void TestEpidemicDrawLastCardChicago()
        {
            _deck.AddFirst("Chicago");
            _deck.AddFirst("New York");
            GameBoardModels.SetOutbreakMarker(0);
            var lastCity = InfectorBL.Epidemic(_deck, _pile, ref _infectionIndex, ref _infectionRate);
            Assert.AreEqual("Chicago", lastCity);
        }

        [TestMethod]
        public void TestEpidemicDrawLastCardNewYork()
        {
            _deck.AddFirst("New York");
            _deck.AddFirst("Chicago");
            GameBoardModels.SetOutbreakMarker(0);
            var lastCity = InfectorBL.Epidemic(_deck, _pile, ref _infectionIndex, ref _infectionRate);
            Assert.AreEqual("New York", lastCity);
        }

        [TestMethod]
        public void TestEpidemicEmptyPileOnTopDeck()
        {
            _deck = new LinkedList<string>();
            _pile = new LinkedList<string>();
            _deck.AddFirst("New York");
            _deck.AddFirst("Chicago");
            _pile.AddFirst("Saint Petersburg");
            _pile.AddFirst("Sydney");
            GameBoardModels.SetOutbreakMarker(0);
            InfectorBL.Epidemic(_deck, _pile, ref _infectionIndex, ref _infectionRate);
            Assert.AreEqual(0, _pile.Count);
            Assert.AreEqual(4, _deck.Count);

            //Look at print statments to manualy asses random/diffrent placing
            var deckarray = _deck.ToArray();
            for (var i = 0; i < 4; i++)
            {
                Console.WriteLine(deckarray[i]);
            }
        }

        [TestMethod]
        public void TestEpidemicLastCityMixedIn()
        {
            _deck = new LinkedList<string>();
            _pile = new LinkedList<string>();
            _deck.AddFirst("Saint Petersburg");
            _deck.AddFirst("Sydney");
            _deck.AddFirst("New York");
            _deck.AddFirst("Chicago");
            GameBoardModels.SetOutbreakMarker(0);
            var lastcity = InfectorBL.Epidemic(_deck, _pile, ref _infectionIndex, ref _infectionRate);
            Assert.AreEqual(lastcity, _deck.First.Value);
        }

      [TestMethod]
        public void TestInfectCityWithNoBlocks()
        {
            var chicago = new City(Color.Blue, "Chicago");
            var numOfBlueCubes = InfectorBL.InfectCity(chicago, new HashSet<City>(), false, Color.Blue);
            Assert.AreEqual(1, numOfBlueCubes);
        }

        [TestMethod]
        public void TestInfectCityWithOneCube()
        {
            var chicago = new City(Color.Blue, "Chicago");
            chicago.Cubes.SetCubeCount(Color.Blue, 1);
            var numBlueCubes = InfectorBL.InfectCity(chicago, new HashSet<City>(), false, Color.Blue);
            Assert.AreEqual(2, numBlueCubes);
        }

        [TestMethod]
        public void TestInfectCityWithTwoCubes()
        {
            var chicago = new SQADemicApp.City(Color.Blue, "Chicago");
            chicago.Cubes.SetCubeCount(Color.Blue, 2);
            var numBlueCubes = InfectorBL.InfectCity(chicago, new HashSet<City>(), false, Color.Blue);
            Assert.AreEqual(3, numBlueCubes);
        }

        [TestMethod]
        public void TestDiffColorInfectCityWithOneCube()
        {
            var lima = new City(Color.Yellow, "Lima");
            lima.Cubes.SetCubeCount(Color.Yellow, 1);
            var numYellowCubes = InfectorBL.InfectCity(lima, new HashSet<City>(), false, Color.Yellow);
            Assert.AreEqual(2, numYellowCubes);
            
        }

        [TestMethod]
        public void TestRedWithTwoInfect()
        {
            var tokyo = new City(Color.Red, "Tokyo");
            tokyo.Cubes.SetCubeCount(Color.Red, 2);
            var numRedCubes = InfectorBL.InfectCity(tokyo, new HashSet<City>(), false, Color.Red);
            Assert.AreEqual(3, numRedCubes);
        }

        [TestMethod]
        public void TestBlueInfectAndOutbreak()
        {
            var chicago = new City(Color.Blue, "Chicago");
            chicago.Cubes.SetCubeCount(Color.Blue, 3);
            GameBoardModels.SetOutbreakMarker(0);
            var numBlueCubes = InfectorBL.InfectCity(chicago, new HashSet<City>(), false, Color.Blue);
            Assert.AreEqual(3, numBlueCubes);
        }

        [TestMethod]
        public void TestOutbreakSimple()
        {
            var infected = new HashSet<City>();
            var lima = new City(Color.Yellow, "Lima");
            var santiago = new City(Color.Yellow, "Santiago");
            infected.Add(santiago);
            santiago.AdjacentCities.Add(lima);
            santiago.Cubes.SetCubeCount(Color.Yellow, 3);
            GameBoardModels.SetOutbreakMarker(0);
            InfectorBL.Outbreak(santiago, Color.Yellow, santiago.AdjacentCities, infected);
            Assert.AreEqual(1, lima.Cubes.GetCubeCount(Color.Yellow));
        }

        [TestMethod]
        public void TestIncrementOutbreakMarker()
        {
            var infected = new HashSet<City>();
            var lima = new City(Color.Yellow, "Lima");
            var santiago = new City(Color.Yellow, "Santiago");
            infected.Add(santiago);
            santiago.AdjacentCities.Add(lima);
            santiago.Cubes.SetCubeCount(Color.Yellow, 3);
            GameBoardModels.SetOutbreakMarker(0);
            InfectorBL.Outbreak(santiago, Color.Yellow, santiago.AdjacentCities, infected);
            Assert.AreEqual(1, GameBoardModels.GetOutbreakMarker());
        }

        [TestMethod]
        public void TestIncrementOutbreakMarker2()
        {
            var infected = new HashSet<City>();
            var lima = new SQADemicApp.City(Color.Yellow, "Lima");
            var santiago = new SQADemicApp.City(Color.Yellow, "Santiago");
            infected.Add(santiago);
            santiago.AdjacentCities.Add(lima);
            santiago.Cubes.SetCubeCount(Color.Yellow, 3);
            //GameBoardModels.outbreakMarker += 5;
            GameBoardModels.SetOutbreakMarker(5);
            InfectorBL.Outbreak(santiago, Color.Yellow, santiago.AdjacentCities, infected);
            Assert.AreEqual(6, GameBoardModels.GetOutbreakMarker());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRedWithTwoInfectWithNewException()
        {
            var tokyo = new SQADemicApp.City(Color.Red, "Tokyo");
            tokyo.Cubes.SetCubeCount(Color.Red, 2);
            GameBoardModels.SetInfectionCubeCount(Color.Red, 1);
            var numRedCubes = SQADemicApp.BL.InfectorBL.InfectCity(tokyo, new HashSet<City>(), false, Color.Red);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestOutbreakSimpleException()
        {
            var infected = new HashSet<City>();
            var lima = new City(Color.Yellow, "Lima");
            var santiago = new City(Color.Yellow, "Santiago");
            infected.Add(santiago);
            santiago.AdjacentCities.Add(lima);
            GameBoardModels.SetInfectionCubeCount(Color.Yellow, 1);
            santiago.Cubes.SetCubeCount(Color.Yellow, 3);
            GameBoardModels.SetOutbreakMarker(0);
            InfectorBL.Outbreak(santiago, Color.Yellow, santiago.AdjacentCities, infected);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestIncrementOutbreakMarkerToThrowException()
        {
            var infected = new HashSet<City>();
            var lima = new City(Color.Yellow, "Lima");
            var santiago = new City(Color.Yellow, "Santiago");
            infected.Add(santiago);
            santiago.AdjacentCities.Add(lima);
            santiago.Cubes.SetCubeCount(Color.Yellow, 3);
            GameBoardModels.SetOutbreakMarker(7);
            InfectorBL.Outbreak(santiago, Color.Yellow, santiago.AdjacentCities, infected);
            //Throw exception
        }

        private static void DecrementCubesPrimer(string cityName, Color color, int cityCount, int pileCount)
        {
            var city = new City(color, cityName);
            city.Cubes.SetCubeCount(color, cityCount);
            GameBoardModels.SetInfectionCubeCount(color, pileCount);
            InfectorBL.InfectCity(city, new HashSet<City>(), false, color);
        }

        [TestMethod]
        public void TestDecrementTotalRedCubes()
        {
            DecrementCubesPrimer("Tokyo", Color.Red, 2, 24);
            Assert.AreEqual(23, GameBoardModels.GetInfectionCubeCount(Color.Red));
        }

        [TestMethod]
        public void TestYellowTotalCubeDecrement()
        {
            DecrementCubesPrimer("Lima", Color.Yellow, 2, 23);
            Assert.AreEqual(22, GameBoardModels.GetInfectionCubeCount(Color.Yellow));

        }

        [TestMethod]
        public void TestDecrementTotalBlueCubes()
        {
            DecrementCubesPrimer("Chicago", Color.Blue, 2, 22);
            Assert.AreEqual(21, GameBoardModels.GetInfectionCubeCount(Color.Blue));
        }

        [TestMethod]
        public void TestDecrementTotalBlackCubes()
        {
            DecrementCubesPrimer("Kolkata", Color.Black, 2, 22);
            Assert.AreEqual(21, GameBoardModels.GetInfectionCubeCount(Color.Black));
        }
        /** Experimental new tests 
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDecrementTotalRedCubesEndGame()
        {
            DecrementCubesPrimer("Tokyo", COLOR.red, 2, 1);
            //Assert.AreEqual(23, GameBoardModels.GetInfectionCubeCount(COLOR.red));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]

        public void TestYellowTotalCubeDecrementEndGame()
        {
            DecrementCubesPrimer("Lima", COLOR.yellow, 2, 1);
            //Assert.AreEqual(22, GameBoardModels.GetInfectionCubeCount(COLOR.yellow));

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDecrementTotalBlueCubesEndGame()
        {
            DecrementCubesPrimer("Chicago", COLOR.blue, 2, 1);
          //  Assert.AreEqual(21, GameBoardModels.GetInfectionCubeCount(COLOR.blue));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDecrementTotalBlackCubesEndGame()
        {
            DecrementCubesPrimer("Kolkata", COLOR.black, 2, 1);
        //    Assert.AreEqual(21, GameBoardModels.GetInfectionCubeCount(COLOR.black));
        }**/


    }
}
