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
        public Color Color;
        public bool ResearchStation = false;
        public InfectionCubesCity Cubes;
        public HashSet<City> AdjacentCities;
        //public List<City> adjacentCities;

        public City(Color color, string name)
        {
            this.Color = color;
            this.Name = name;
            Cubes = new InfectionCubesCity(0);
            AdjacentCities = new HashSet<City>();
        }

        public int AllCubeCount()
        {
            return Cubes.GetTotalCubeCount();
        }

        public void SetAdjacentCities(HashSet<City> cities)
        {
            this.AdjacentCities = cities;
        }

        public HashSet<City> GetAdjacentCities()
        {
            return AdjacentCities;
        }

        public override bool Equals(object obj)
        {
            var temp = (City) obj;
            return (this.Name == temp.Name) && (this.Color == temp.Color);
        }
    }



}