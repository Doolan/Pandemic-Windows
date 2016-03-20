using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using SQADemicApp;
using SQADemicApp.BL;


namespace SQADemicAppTest
{
    [TestClass]
    public class CitiesTest
    {
        static CityBL CityBL = new CityBL();

        [TestInitialize()]
        public void TestInitialize()
        {
            //Create.setUpCreate();
            //create.createDictionary();
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //the following tests check the getters and setters for 
        //adjacent cities list and also verify the integrity of the dictionary
        /** Works with private methods deal with later
        [TestMethod]
        public void TestSettingAndGettingAdjacentCitiesSF()
        {
            StringReader r = new StringReader("San Francisco;Tokyo,Manila,Chicago,Los Angeles");
            create.setAdjacentCities(r);
            HashSet<City> result = CityBL.getAdjacentCities("San Francisco");
            HashSet<City> expected = new HashSet<City>();
            City tokyo = new City(COLOR.red, "Tokyo");
            City manila = new City(COLOR.red, "Manila");
            City chicago = new City(COLOR.blue, "Chicago");
            City losAngeles = new City(COLOR.yellow, "Los Angeles");
            expected.Add(tokyo);
            expected.Add(manila);
            expected.Add(chicago);
            expected.Add(losAngeles);
            CollectionAssert.AreEqual(result.ToList<City>(), expected.ToList<City>());
        }

        [TestMethod]
        public void TestNYNeighbors()
        {
            StringReader r = new StringReader("New York;Montreal,Washington,London,Madrid");
            create.setAdjacentCities(r);
            HashSet<City> result = CityBL.getAdjacentCities("New York");
            HashSet<City> expected = new HashSet<City>();
            City montreal = new City(COLOR.blue, "Montreal");
            City washington = new City(COLOR.blue, "Washington");
            City london = new City(COLOR.blue, "London");
            City madrid = new City(COLOR.blue, "Madrid");
            expected.Add(montreal);
            expected.Add(washington);
            expected.Add(london);
            expected.Add(madrid);
            CollectionAssert.AreEqual(result.ToList<City>(), expected.ToList<City>());
        }

        [TestMethod]
        public void TestMontrealNeighbors()
        {
            StringReader r = new StringReader("Montreal;New York,Washington,Chicago");
            create.setAdjacentCities(r);
            HashSet<City> result = CityBL.getAdjacentCities("Montreal");
            HashSet<City> e = new HashSet<City>();
            City newYork = new City(COLOR.blue, "New York");
            City washington = new City(COLOR.blue, "Washington");
            City chicago = new City(COLOR.blue, "Chicago");
            e.Add(newYork);
            e.Add(washington);
            e.Add(chicago);
            CollectionAssert.AreEqual(result.ToList<City>(), e.ToList<City>());
        }

        [TestMethod]
        public void TestChicagoNeighbors()
        {
            StringReader r = new StringReader("Chicago;San Francisco,Los Angeles,Atlanta,Montreal");
            create.setAdjacentCities(r);
            HashSet<City> res = CityBL.getAdjacentCities("Chicago");
            HashSet<City> e = new HashSet<City>();
            City sanFrancisco = new City(COLOR.blue, "San Francisco");
            City losAngeles = new City(COLOR.yellow, "Los Angeles");
            City atlanta = new City(COLOR.blue, "Atlanta");
            City montreal = new City(COLOR.blue, "Montreal");
            e.Add(sanFrancisco);
            e.Add(losAngeles);
            e.Add(atlanta);
            e.Add(montreal);
            CollectionAssert.AreEqual(res.ToList<City>(), e.ToList<City>());
        }

        [TestMethod]
        public void TestAtlantaNeighbors()
        {
            StringReader r = new StringReader("Atlanta;Chicago,Washington,Miami");
            create.setAdjacentCities(r);
            HashSet<City> res = CityBL.getAdjacentCities("Atlanta");
            HashSet<City> e = new HashSet<City>();
            City chicago = new City(COLOR.blue, "Chicago");
            City washington = new City(COLOR.blue, "Washington");
            City miami = new City(COLOR.yellow, "Miami");
            e.Add(chicago);
            e.Add(washington);
            e.Add(miami);
            CollectionAssert.AreEqual(res.ToList<City>(), e.ToList<City>());
        }

        [TestMethod]
        public void TestWashingtonNeighbors()
        {
            StringReader r = new StringReader("Washington;Miami,Atlanta,Montreal,New York");
            create.setAdjacentCities(r);
            HashSet<City> res = CityBL.getAdjacentCities("Washington");
            HashSet<City> e = new HashSet<City>();
            City miami = new City(COLOR.yellow, "Miami");
            City atlanta = new City(COLOR.blue, "Atlanta");
            City montreal = new City(COLOR.blue, "Montreal");
            City newYork = new City(COLOR.blue, "New York");
            e.Add(miami);
            e.Add(atlanta);
            e.Add(montreal);
            e.Add(newYork);
            CollectionAssert.AreEqual(res.ToList<City>(), e.ToList<City>());

        }
        **/

        //might fail if other tests fail and don't reset research stations

        /**

            FRAGILE DEPENDANT ON STARTING STATE FROM OTHER FILES --- ignore until refactoring city load in
    **/


        //[TestMethod]
        //public void TestResearchCityList()
        //{
        //    Create.cityDictionary["Atlanta"].researchStation = true;
        //    List<City> ls = new List<City>();
        //    City a = new City(COLOR.blue, "Atlanta");
        //    a.researchStation = true;
        //    ls.Add(a);
        //    List<City> result = CityBL.getCitiesWithResearchStations();
        //    CollectionAssert.AreEqual(ls, result);
        //}

        //[TestMethod]
        //public void TestGetNeighborNames()
        //{
        //    List<String> e = new List<String>();
        //    City miami = new City(COLOR.yellow, "Miami");
        //    City atlanta = new City(COLOR.blue, "Atlanta");
        //    City montreal = new City(COLOR.blue, "Montreal");
        //    City newYork = new City(COLOR.blue, "New York");
        //    e.Add(miami.Name);
        //    e.Add(atlanta.Name);
        //    e.Add(montreal.Name);
        //    e.Add(newYork.Name);
        //    List<String> result = CityBL.getNeighborNames("Washington");
        //    CollectionAssert.AreEqual(e, result);
        //}

    }
}
