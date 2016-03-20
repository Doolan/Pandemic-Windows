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
            GameBoardModels.outbreakMarker = 0;
            GameBoardModels.cubeCount.SetCubeCount(COLOR.red, MAXCUBECOUNT);
            GameBoardModels.cubeCount.SetCubeCount(COLOR.black, MAXCUBECOUNT);
            GameBoardModels.cubeCount.SetCubeCount(COLOR.blue, MAXCUBECOUNT);
            GameBoardModels.cubeCount.SetCubeCount(COLOR.yellow, MAXCUBECOUNT);
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
            SQADemicApp.BL.InfectorBL.Outbreak(santiago, COLOR.yellow, santiago.adjacentCities, infected);
            Assert.AreEqual(1, GameBoardModels.outbreakMarker);
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
            GameBoardModels.outbreakMarker += 5;
            SQADemicApp.BL.InfectorBL.Outbreak(santiago, COLOR.yellow, santiago.adjacentCities, infected);
            Assert.AreEqual(6, GameBoardModels.outbreakMarker);
        }
        //This function didn't follow the rules of the game, Therefore this test is stupid
       /** [TestMethod]
        public void TestOneOutbreakSetOffAnotherAndOverFlowsToDifferentColor()
        {
            HashSet<City> infected = new HashSet<City>();
            SQADemicApp.City montreal = new SQADemicApp.City(COLOR.blue, "Montreal");
            SQADemicApp.City ny = new SQADemicApp.City(COLOR.blue, "New York");
            SQADemicApp.City washington = new SQADemicApp.City(COLOR.blue, "Washington");
            SQADemicApp.City chicago = new SQADemicApp.City(COLOR.blue, "Chicago");
            SQADemicApp.City atlanta = new SQADemicApp.City(COLOR.blue, "Atlanta");
            SQADemicApp.City miami = new SQADemicApp.City(COLOR.yellow, "Miami");
            montreal.blueCubes = 3;
            washington.blueCubes = 3;
            montreal.adjacentCities.Add(chicago);
            montreal.adjacentCities.Add(washington);
            montreal.adjacentCities.Add(ny);
            washington.adjacentCities.Add(ny);
            washington.adjacentCities.Add(montreal);
            washington.adjacentCities.Add(atlanta);
            washington.adjacentCities.Add(miami);
            infected.Add(montreal);
            SQADemicApp.BL.InfectorBL.InfectCity(montreal, infected, false, COLOR.blue);
            Assert.AreEqual(2, GameBoardModels.outbreakMarker); //counts both outbreaks
            Assert.AreEqual(1, miami.blueCubes); //ensures miami gets a blue overflow cube, even though it's a yellow city
            Assert.AreEqual(1, ny.blueCubes); //ensures new york only gets infected once
        }**/


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRedWithTwoInfectWithNewException()
        {
            SQADemicApp.City tokyo = new SQADemicApp.City(COLOR.red, "Tokyo");
            tokyo.Cubes.SetCubeCount(COLOR.red, 2);
            GameBoardModels.cubeCount.SetCubeCount(COLOR.red, 1);
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
            GameBoardModels.cubeCount.SetCubeCount(COLOR.yellow, 1);
            santiago.Cubes.SetCubeCount(COLOR.yellow, 3);
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
            GameBoardModels.outbreakMarker = 7;
            SQADemicApp.BL.InfectorBL.Outbreak(santiago, COLOR.yellow, santiago.adjacentCities, infected);
        }

        [TestMethod]
        public void TestDecrementTotalRedCubes()
        {
            SQADemicApp.City tokyo = new SQADemicApp.City(COLOR.red, "Tokyo");
            tokyo.Cubes.SetCubeCount(COLOR.red, 2);
            GameBoardModels.cubeCount.SetCubeCount(COLOR.red, 24);
            SQADemicApp.BL.InfectorBL.InfectCity(tokyo, new HashSet<City>(), false, COLOR.red);
            Assert.AreEqual(23, GameBoardModels.cubeCount.GetCubeCount(COLOR.red));
        }

        [TestMethod]
        public void TestYellowTotalCubeDecrement()
        {
            SQADemicApp.City lima = new SQADemicApp.City(COLOR.yellow, "Lima");
            lima.Cubes.SetCubeCount(COLOR.yellow, 1);
            GameBoardModels.cubeCount.SetCubeCount(COLOR.yellow, 23);
            SQADemicApp.BL.InfectorBL.InfectCity(lima, new HashSet<City>(), false, COLOR.yellow);
            Assert.AreEqual(22, GameBoardModels.cubeCount.GetCubeCount(COLOR.yellow));

        }

        [TestMethod]
        public void TestDecrementTotalBlueCubes()
        {
            SQADemicApp.City chicago = new SQADemicApp.City(COLOR.blue, "Chicago");
            chicago.Cubes.SetCubeCount(COLOR.blue, 2);
            GameBoardModels.cubeCount.SetCubeCount(COLOR.blue, 22);
            SQADemicApp.BL.InfectorBL.InfectCity(chicago, new HashSet<City>(), false, COLOR.blue);
            Assert.AreEqual(21, GameBoardModels.cubeCount.GetCubeCount(COLOR.blue));
        }

        [TestMethod]
        public void TestDecrementTotalBlackCubes()
        {
            SQADemicApp.City kolkata = new SQADemicApp.City(COLOR.black, "Kolkata");
            kolkata.Cubes.SetCubeCount(COLOR.black, 2);
            GameBoardModels.cubeCount.SetCubeCount(COLOR.black, 22);
            SQADemicApp.BL.InfectorBL.InfectCity(kolkata, new HashSet<City>(), false, COLOR.black);
            Assert.AreEqual(21, GameBoardModels.cubeCount.GetCubeCount(COLOR.black));
        }

        
    }
}
