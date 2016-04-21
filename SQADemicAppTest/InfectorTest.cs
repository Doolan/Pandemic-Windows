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
        private LinkedList<String> deck;
        private LinkedList<String> pile;
        private int infectionRate;
        private int infectionIndex;
        private static int MAXCUBECOUNT = 24;

        [TestInitialize]
        public void SetUpArrays()
        {
            new GameBoardModels(new string[] { "Dispatcher", "Operations Expert" });
            GameBoardModels.SetOutbreakMarker(0);
            GameBoardModels.SetInfectionCubeCount(COLOR.red, MAXCUBECOUNT);
            GameBoardModels.SetInfectionCubeCount(COLOR.black, MAXCUBECOUNT);
            GameBoardModels.SetInfectionCubeCount(COLOR.blue, MAXCUBECOUNT);
            GameBoardModels.SetInfectionCubeCount(COLOR.yellow, MAXCUBECOUNT);
            deck = new LinkedList<string>();
            pile = new LinkedList<string>();
            infectionRate = 3;
            infectionIndex = 4;
        }

        [TestMethod]
        public void TestInfectTwoCities()
        {
            deck.Clear(); 
            deck.AddFirst("Saint Petersburg");
            deck.AddFirst("Sydney");
            List<String> removedCities = InfectorBL.InfectCities(deck, pile, 2);
            List<String> answer = new List<string> { "Sydney", "Saint Petersburg" };
            CollectionAssert.AreEqual(answer, removedCities);
        }

        [TestMethod]
        public void TestInfectThreeCities()
        {
            deck.Clear();
            deck.AddFirst("Saint Petersburg");
            deck.AddFirst("Sydney");
            deck.AddFirst("New York");
            List<String> removedCities = InfectorBL.InfectCities(deck, pile, 3);
            List<String> answer = new List<string> { "New York", "Sydney", "Saint Petersburg" };
            CollectionAssert.AreEqual(answer, removedCities);
        }

        [TestMethod]
        public void TestInfectTwoCitiesTwice()
        {
            deck.Clear();
            deck.AddFirst("Saint Petersburg");
            deck.AddFirst("Sydney");
            List<String> removedCities = InfectorBL.InfectCities(deck, pile, 2);
            List<String> answer = new List<string> { "Sydney", "Saint Petersburg" };
            CollectionAssert.AreEqual(answer, removedCities);
            deck.AddFirst("New York");
            deck.AddFirst("Chicago");
            removedCities = InfectorBL.InfectCities(deck, pile, 2);
            answer = new List<string> { "Chicago", "New York" };
            CollectionAssert.AreEqual(answer, removedCities);

        }

        [TestMethod]
        public void TestInfectTwoCitiesUpdatePile()
        {
            deck.Clear(); 
            deck.AddFirst("Saint Petersburg");
            deck.AddFirst("Sydney");
            pile = new LinkedList<String>();
            List<String> removedCities = InfectorBL.InfectCities(deck, pile, 2);
            LinkedList<String> answer = new LinkedList<string>();
            answer.AddFirst("Sydney");
            answer.AddFirst("Saint Petersburg");
            CollectionAssert.AreEqual(answer, pile);
        }

        [TestMethod]
        public void TestEpidemicIncreaseInfectionCounter2to2()
        {
            deck.Clear();
            //InfectionRates = { 2, 2, 2, 3, 3, 4, 4 };
            deck.AddFirst("Chicago");
            infectionIndex = 1;
            InfectorBL.Epidemic(deck, pile, ref infectionIndex, ref infectionRate);
            Assert.AreEqual(2, infectionRate);
        }

        [TestMethod]
        public void TestEpidemicIncreaseInfectionCounter2to3()
        {
            //InfectionRates = { 2, 2, 2, 3, 3, 4, 4 };
            deck.Clear(); 
            deck.AddFirst("Chicago");
            infectionIndex = 2;
            InfectorBL.Epidemic(deck, pile, ref infectionIndex, ref infectionRate);
            Assert.AreEqual(3, infectionRate);
        }

        [TestMethod]
        public void TestEpidemicIncreaseInfectionCounter3to4()
        {
            //InfectionRates = { 2, 2, 2, 3, 3, 4, 4 };
            deck.AddFirst("Chicago");
            infectionRate = 3;
            infectionIndex = 4;
            InfectorBL.Epidemic(deck, pile, ref infectionIndex, ref infectionRate);
            Assert.AreEqual(4, infectionRate);
        }

        [TestMethod]
        public void TestEpidemicIncreaseInfectionIndex()
        {
            deck.AddFirst("Chicago");
            infectionRate = 3;
            infectionIndex = 4;
            InfectorBL.Epidemic(deck, pile, ref infectionIndex, ref infectionRate);
            Assert.AreEqual(5, infectionIndex);
        }

        [TestMethod]
        public void TestEpidemicDrawLastCardChicago()
        {
            deck.AddFirst("Chicago");
            deck.AddFirst("New York");
            string lastCity = InfectorBL.Epidemic(deck, pile, ref infectionIndex, ref infectionRate);
            Assert.AreEqual("Chicago", lastCity);
        }

        [TestMethod]
        public void TestEpidemicDrawLastCardNewYork()
        {
            deck.AddFirst("New York");
            deck.AddFirst("Chicago");
            string lastCity = InfectorBL.Epidemic(deck, pile, ref infectionIndex, ref infectionRate);
            Assert.AreEqual("New York", lastCity);
        }

        [TestMethod]
        public void TestEpidemicEmptyPileOnTopDeck()
        {
            deck = new LinkedList<string>();
            pile = new LinkedList<string>();
            deck.AddFirst("New York");
            deck.AddFirst("Chicago");
            pile.AddFirst("Saint Petersburg");
            pile.AddFirst("Sydney");
            InfectorBL.Epidemic(deck, pile, ref infectionIndex, ref infectionRate);
            Assert.AreEqual(0, pile.Count);
            Assert.AreEqual(4, deck.Count);

            //Look at print statments to manualy asses random/diffrent placing
            string[] deckarray = deck.ToArray<String>();
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(deckarray[i]);
            }
        }

        [TestMethod]
        public void TestEpidemicLastCityMixedIn()
        {
            deck = new LinkedList<string>();
            pile = new LinkedList<string>();
            deck.AddFirst("Saint Petersburg");
            deck.AddFirst("Sydney");
            deck.AddFirst("New York");
            deck.AddFirst("Chicago");
            string lastcity = InfectorBL.Epidemic(deck, pile, ref infectionIndex, ref infectionRate);
            Assert.AreEqual(lastcity, deck.First.Value);
        }

      [TestMethod]
        public void TestInfectCityWithNoBlocks()
        {
            SQADemicApp.City chicago = new SQADemicApp.City(COLOR.blue, "Chicago");
            int numOfBlueCubes = SQADemicApp.BL.InfectorBL.InfectCity(chicago, new HashSet<City>(), false, COLOR.blue);
            Assert.AreEqual(1, numOfBlueCubes);
        }

        [TestMethod]
        public void TestInfectCityWithOneCube()
        {
            SQADemicApp.City chicago = new SQADemicApp.City(COLOR.blue, "Chicago");
            chicago.Cubes.SetCubeCount(COLOR.blue, 1);
            int numBlueCubes = SQADemicApp.BL.InfectorBL.InfectCity(chicago, new HashSet<City>(), false, COLOR.blue);
            Assert.AreEqual(2, numBlueCubes);
        }

        [TestMethod]
        public void TestInfectCityWithTwoCubes()
        {
            SQADemicApp.City chicago = new SQADemicApp.City(COLOR.blue, "Chicago");
            chicago.Cubes.SetCubeCount(COLOR.blue, 2);
            int numBlueCubes = SQADemicApp.BL.InfectorBL.InfectCity(chicago, new HashSet<City>(), false, COLOR.blue);
            Assert.AreEqual(3, numBlueCubes);
        }

        [TestMethod]
        public void TestDiffColorInfectCityWithOneCube()
        {
            SQADemicApp.City lima = new SQADemicApp.City(COLOR.yellow, "Lima");
            lima.Cubes.SetCubeCount(COLOR.yellow, 1);
            int numYellowCubes = SQADemicApp.BL.InfectorBL.InfectCity(lima, new HashSet<City>(), false, COLOR.yellow);
            Assert.AreEqual(2, numYellowCubes);
            
        }

        [TestMethod]
        public void TestRedWithTwoInfect()
        {
            SQADemicApp.City tokyo = new SQADemicApp.City(COLOR.red, "Tokyo");
            tokyo.Cubes.SetCubeCount(COLOR.red, 2);
            int numRedCubes = SQADemicApp.BL.InfectorBL.InfectCity(tokyo, new HashSet<City>(), false, COLOR.red);
            Assert.AreEqual(3, numRedCubes);
        }

        [TestMethod]
        public void TestBlueInfectAndOutbreak()
        {
            SQADemicApp.City chicago = new SQADemicApp.City(COLOR.blue, "Chicago");
            chicago.Cubes.SetCubeCount(COLOR.blue, 3);
            int numBlueCubes = SQADemicApp.BL.InfectorBL.InfectCity(chicago, new HashSet<City>(), false, COLOR.blue);
            Assert.AreEqual(3, numBlueCubes);
        }

        [TestMethod]
        public void TestOutbreakSimple()
        {
            HashSet<City> infected = new HashSet<City>();
            SQADemicApp.City lima = new SQADemicApp.City(COLOR.yellow, "Lima");
            SQADemicApp.City santiago = new SQADemicApp.City(COLOR.yellow, "Santiago");
            infected.Add(santiago);
            santiago.adjacentCities.Add(lima);
            santiago.Cubes.SetCubeCount(COLOR.yellow, 3);
            SQADemicApp.BL.InfectorBL.Outbreak(santiago, COLOR.yellow, santiago.adjacentCities, infected);
            Assert.AreEqual(1, lima.Cubes.GetCubeCount(COLOR.yellow));
        }

        [TestMethod]
        public void TestIncrementOutbreakMarker()
        {
            HashSet<City> infected = new HashSet<City>();
            SQADemicApp.City lima = new SQADemicApp.City(COLOR.yellow, "Lima");
            SQADemicApp.City santiago = new SQADemicApp.City(COLOR.yellow, "Santiago");
            infected.Add(santiago);
            santiago.adjacentCities.Add(lima);
            santiago.Cubes.SetCubeCount(COLOR.yellow, 3);
            GameBoardModels.SetOutbreakMarker(0);
            SQADemicApp.BL.InfectorBL.Outbreak(santiago, COLOR.yellow, santiago.adjacentCities, infected);
            Assert.AreEqual(1, GameBoardModels.GetOutbreakMarker());
        }

        [TestMethod]
        public void TestIncrementOutbreakMarker2()
        {
            HashSet<City> infected = new HashSet<City>();
            SQADemicApp.City lima = new SQADemicApp.City(COLOR.yellow, "Lima");
            SQADemicApp.City santiago = new SQADemicApp.City(COLOR.yellow, "Santiago");
            infected.Add(santiago);
            santiago.adjacentCities.Add(lima);
            santiago.Cubes.SetCubeCount(COLOR.yellow, 3);
            //GameBoardModels.outbreakMarker += 5;
            GameBoardModels.SetOutbreakMarker(5);
            SQADemicApp.BL.InfectorBL.Outbreak(santiago, COLOR.yellow, santiago.adjacentCities, infected);
            Assert.AreEqual(6, GameBoardModels.GetOutbreakMarker());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRedWithTwoInfectWithNewException()
        {
            SQADemicApp.City tokyo = new SQADemicApp.City(COLOR.red, "Tokyo");
            tokyo.Cubes.SetCubeCount(COLOR.red, 2);
            GameBoardModels.SetInfectionCubeCount(COLOR.red, 1);
            int numRedCubes = SQADemicApp.BL.InfectorBL.InfectCity(tokyo, new HashSet<City>(), false, COLOR.red);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestOutbreakSimpleException()
        {
            HashSet<City> infected = new HashSet<City>();
            SQADemicApp.City lima = new SQADemicApp.City(COLOR.yellow, "Lima");
            SQADemicApp.City santiago = new SQADemicApp.City(COLOR.yellow, "Santiago");
            infected.Add(santiago);
            santiago.adjacentCities.Add(lima);
            GameBoardModels.SetInfectionCubeCount(COLOR.yellow, 1);
            santiago.Cubes.SetCubeCount(COLOR.yellow, 3);
            GameBoardModels.SetOutbreakMarker(0);
            SQADemicApp.BL.InfectorBL.Outbreak(santiago, COLOR.yellow, santiago.adjacentCities, infected);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestIncrementOutbreakMarkerToThrowException()
        {
            HashSet<City> infected = new HashSet<City>();
            SQADemicApp.City lima = new SQADemicApp.City(COLOR.yellow, "Lima");
            SQADemicApp.City santiago = new SQADemicApp.City(COLOR.yellow, "Santiago");
            infected.Add(santiago);
            santiago.adjacentCities.Add(lima);
            santiago.Cubes.SetCubeCount(COLOR.yellow, 3);
            GameBoardModels.SetOutbreakMarker(7);
            SQADemicApp.BL.InfectorBL.Outbreak(santiago, COLOR.yellow, santiago.adjacentCities, infected);
            //Throw exception
        }

        private void DecrementCubesPrimer(String cityName, COLOR color, int cityCount, int pileCount)
        {
            City city = new City(color, cityName);
            city.Cubes.SetCubeCount(color, cityCount);
            GameBoardModels.SetInfectionCubeCount(color, pileCount);
            InfectorBL.InfectCity(city, new HashSet<City>(), false, color);
        }

        [TestMethod]
        public void TestDecrementTotalRedCubes()
        {
            DecrementCubesPrimer("Tokyo", COLOR.red, 2, 24);
            Assert.AreEqual(23, GameBoardModels.GetInfectionCubeCount(COLOR.red));
        }

        [TestMethod]
        public void TestYellowTotalCubeDecrement()
        {
            DecrementCubesPrimer("Lima", COLOR.yellow, 2, 23);
            Assert.AreEqual(22, GameBoardModels.GetInfectionCubeCount(COLOR.yellow));

        }

        [TestMethod]
        public void TestDecrementTotalBlueCubes()
        {
            DecrementCubesPrimer("Chicago", COLOR.blue, 2, 22);
            Assert.AreEqual(21, GameBoardModels.GetInfectionCubeCount(COLOR.blue));
        }

        [TestMethod]
        public void TestDecrementTotalBlackCubes()
        {
            DecrementCubesPrimer("Kolkata", COLOR.black, 2, 22);
            Assert.AreEqual(21, GameBoardModels.GetInfectionCubeCount(COLOR.black));
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
