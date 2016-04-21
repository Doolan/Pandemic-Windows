using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQADemicApp.Objects;

namespace SQADemicApp
{
    public class City
    {

        public string Name;
        public COLOR color;
        public bool researchStation = false;
        public InfectionCubesCity Cubes;
        public HashSet<City> adjacentCities;
        //public List<City> adjacentCities;

        public City(COLOR color, String name)
        {
            this.color = color;
            this.Name = name;
            Cubes = new InfectionCubesCity(0);
            adjacentCities = new HashSet<City>();
        }

        public int allCubeCount()
        {
            return Cubes.GetTotalCubeCount();
        }

        public void setAdjacentCities(HashSet<City> cities)
        {
            this.adjacentCities = cities;
        }

        public HashSet<City> getAdjacentCities()
        {
            return adjacentCities;
        }

        public override bool Equals(object obj)
        {
            City temp = (City) obj;
            return (this.Name == temp.Name) && (this.color == temp.color);
        }
    }



}