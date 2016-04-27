using SQADemicApp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SQADemicApp.BL
{
    public class CityBL
    {
        //Not sure what this does for us
        /// <summary>
        /// Gets the Neighboring Cities
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns>List of Neigboring Cities names</returns>
        public static List<string> GetNeighborNames(string cityName)
        {
            var cityls = Create.CityDictionary[cityName].GetAdjacentCities();
            return cityls.Select(city => city.Name).ToList();
        }

        //I would prefer to use a static list that gets updated as opposed to continualy 
        // finding the cities. This function has a heavy runtime and the amount of times it
        // will get called makes it burndensome
        public static  List<City> GetCitiesWithResearchStations()
        {
            return (from key in Create.CityDictionary.Keys where Create.CityDictionary[key].ResearchStation == true select Create.CityDictionary[key]).ToList();
        }

        //Not sure what this does for us
        public static HashSet<City> GetAdjacentCities(string name)
        {
            var city = Create.CityDictionary[name];
            return city.GetAdjacentCities();
        }

    }
}
