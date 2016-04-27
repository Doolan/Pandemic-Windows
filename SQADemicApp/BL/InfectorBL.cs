using System;
using System.Collections.Generic;
using System.Linq;
using SQADemicApp.Objects;

namespace SQADemicApp.BL
{
    public class InfectorBL
    {
		/// <summary>
		///  Draws the next n cards from the infect deck and updates the pile
		/// </summary>
		/// <param name="deck">infection Deck - LinkedList</param>
        /// <param name="pile">infection Deck - LinkedList</param>
        /// <param name="infectionRate"></param>
		/// <returns>List of new infected cities</returns>
        public static List<string> InfectCities(LinkedList<string> deck, LinkedList<string> pile, int infectionRate)
        {
            var returnList = new List<string>();

            for (var i = 0; i < infectionRate; i++)
            {
                returnList.Add(deck.First.Value);
                pile.AddFirst(deck.First.Value);
                deck.RemoveFirst();
				
            }
                return returnList;
        }

		/// <summary>
		/// Handles Epidmeic card actions,
        /// Increases the infection rate, draws from the bottom of the deck, Shuffles the infection discard pile back into the infection deck 
		/// </summary>
        /// <param name="deck">infection Deck - LinkedList</param>
        /// <param name="pile">infection Deck - LinkedList</param>
        /// <param name="infectionRateIndex">infectionRateIndex - int current index in the infectionRates</param>
        /// <param name="infectionRate"></param>
       	/// <returns></returns>
        public static string Epidemic(LinkedList<string> deck, LinkedList<string> pile, ref int infectionRateIndex, ref int infectionRate)
        {
            //infection rate stuff
            infectionRate = infectionRateIndex > 1 ? (infectionRateIndex > 3 ? 4 :3) : 2;
            infectionRateIndex += 1;

            //draw Last card
            var epidmicCity = deck.Last.Value;
            deck.RemoveLast();
            pile.AddFirst(epidmicCity);

            //shuffle remains back on to the deck
            var pilearray = pile.ToArray();
            pilearray = HelperBL.ShuffleArray(pilearray);
            foreach (string eachPile in pilearray)
            {
                deck.AddFirst(eachPile);
            }
		    pile.Clear();
            return epidmicCity;
		}

        public static void InfectCities(List<string> citiesToInfect)
        {
            foreach (var name in citiesToInfect)
            {
                InfectCity(Create.CityDictionary[name], new HashSet<City>(), false, Create.CityDictionary[name].Color);
            }
        }

        /// <summary>
        /// Deals with outbreaks for Infect City
        /// </summary>
        /// <param name="city">city to infect</param>
        /// <param name="alreadyInfected"></param>
        /// <param name="causedByOutbreak"></param>
        /// <param name="outbreakColor"></param>
        /// <returns></returns>
        public static int InfectCity(SQADemicApp.City city, HashSet<City> alreadyInfected, bool causedByOutbreak, Color outbreakColor)
        {
            if (!causedByOutbreak)
            {
                if (GameBoardModels.GetCureStatus(city.Color) == Cures.Curestate.Sunset)
                    return city.Cubes.GetCubeCount(city.Color);
                if (city.Cubes.GetCubeCount(city.Color) < 3)
                {
                    GameBoardModels.DecrementInfectionCubeCount(city.Color);
                    city.Cubes.IncrementCubes(city.Color);
                    return city.Cubes.GetCubeCount(city.Color);
                }
                Outbreak(city, city.Color, city.AdjacentCities, alreadyInfected);
            } 
                // will reach here if this infection was caused by an outbreak.
                //need to increment cubes of outbreak color, which aren't necessarily the city color
                if (GameBoardModels.GetCureStatus(outbreakColor) == Cures.Curestate.Sunset)
                    return city.Cubes.GetCubeCount(outbreakColor);
                if (city.Cubes.GetCubeCount(outbreakColor) < 3)
                {
                GameBoardModels.DecrementInfectionCubeCount(outbreakColor);
                    city.Cubes.IncrementCubes(outbreakColor);
                    return city.Cubes.GetCubeCount(outbreakColor);
                }
                Outbreak(city, city.Color, city.AdjacentCities, alreadyInfected);
                return city.Cubes.GetCubeCount(outbreakColor);
        }

        //returns a list of the cities that have already been infected
        public static HashSet<City> Outbreak(City city, Color color, HashSet<City> adjacentCities, HashSet<City> alreadyInfected)
        {
            new PicForm(true, city.Name).Show();
            alreadyInfected.Add(city);
            foreach (var neighbor in adjacentCities.Where(neighbor => !alreadyInfected.Contains(neighbor)))
            {
                //alreadyInfected.Add(neighbor);
                InfectCity(neighbor, alreadyInfected, true, color);
            }
            GameBoardModels.IncrementOutbreakMarker();
            return alreadyInfected;
        }
    }
}
