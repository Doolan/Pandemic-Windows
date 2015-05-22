﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        public static List<String> InfectCities(LinkedList<String> deck, LinkedList<String> pile, int infectionRate)
        {
            List<string> returnList = new List<string>();

            for (int i = 0; i < infectionRate; i++)
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
        public static string Epidemic(LinkedList<String> deck, LinkedList<String> pile, ref int infectionRateIndex, ref int infectionRate)
        {
            //infection rate stuff
            infectionRate = infectionRateIndex > 1 ? (infectionRateIndex > 3 ? 4 :3) : 2;
            infectionRateIndex += 1;

            //draw Last card
            string epidmicCity = deck.Last.Value;
            deck.RemoveLast();
            pile.AddFirst(epidmicCity);

            //shuffle remains back on to the deck
            string[] pilearray = pile.ToArray<string>();
            pilearray = HelperBL.shuffleArray(pilearray);
            for (int i = 0; i < pilearray.Length; i++)
            {
                deck.AddFirst(pilearray[i]);
            }
            pile.Clear();
            return epidmicCity;
		}

        public static void InfectCities(List<string> citiesToInfect)
        {
            foreach (string name in citiesToInfect)
            {
                InfectCity(Create.cityDictionary[name], new HashSet<City>(), false, Create.cityDictionary[name].color);
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
        public static int InfectCity(SQADemicApp.City city, HashSet<City> alreadyInfected, bool causedByOutbreak, COLOR outbreakColor)
        {

            if (!causedByOutbreak)
            {
                switch (city.color)
                {
                    case COLOR.blue:
                        if (city.blueCubes < 3)
                        {
                            GameBoardModels.cubeCount.blueCubes--;
                            city.blueCubes++;
                            if (GameBoardModels.cubeCount.blueCubes <= 0)
                            {
                                throw new InvalidOperationException("Game Over");
                            }
                            return city.blueCubes;
                        }
                        Outbreak(city, city.color, city.adjacentCities, alreadyInfected);
                        return city.blueCubes;

                    case COLOR.yellow:
                        if (city.yellowCubes < 3)
                        {
                            GameBoardModels.cubeCount.yellowCubes--;
                            city.yellowCubes++;
                            if (GameBoardModels.cubeCount.yellowCubes <= 0)
                            {
                                throw new InvalidOperationException("Game Over");
                            }
                            return city.yellowCubes;

                        }
                        Outbreak(city, city.color, city.adjacentCities, alreadyInfected);
                        return city.yellowCubes;

                    case COLOR.black:
                        if (city.blackCubes < 3)
                        {
                            GameBoardModels.cubeCount.blackCubes--;
                            city.blackCubes++;
                            if (GameBoardModels.cubeCount.blackCubes <= 0)
                            {
                                throw new InvalidOperationException("Game Over");
                            }
                            return city.blackCubes;

                        }
                        Outbreak(city, city.color, city.adjacentCities, alreadyInfected);
                        return city.blackCubes;

                    default:
                        if (city.redCubes < 3)
                        {
                            GameBoardModels.cubeCount.redCubes--;
                            city.redCubes++;
                            if (GameBoardModels.cubeCount.redCubes <= 0)
                            {
                                throw new InvalidOperationException("Game Over");
                            }
                            return city.redCubes;

                        }
                        Outbreak(city, city.color, city.adjacentCities, alreadyInfected);
                        return city.redCubes;
                }
            } // will reach here if this infection was caused by an outbreak.
            //need to increment cubes of outbreak color, which aren't necessarily the city color
            switch (outbreakColor)
            {
                case COLOR.blue:
                    if (city.blueCubes < 3)
                        {
                            GameBoardModels.cubeCount.blueCubes--;
                            city.blueCubes++;
                            if (GameBoardModels.cubeCount.blueCubes <= 0)
                            {
                                throw new InvalidOperationException("Game Over");
                            }
                            return city.blueCubes;
                        }
                        Outbreak(city, city.color, city.adjacentCities, alreadyInfected);
                        return city.blueCubes;

                case COLOR.yellow:
                    if (city.yellowCubes < 3)
                        {
                            GameBoardModels.cubeCount.yellowCubes--;
                            city.yellowCubes++;
                            if (GameBoardModels.cubeCount.yellowCubes <= 0)
                            {
                                throw new InvalidOperationException("Game Over");
                            }
                            return city.yellowCubes;

                        }
                        Outbreak(city, city.color, city.adjacentCities, alreadyInfected);
                        return city.yellowCubes;

                case COLOR.black:
                        if (city.blackCubes < 3)
                        {
                            GameBoardModels.cubeCount.blackCubes--;
                            city.blackCubes++;
                            if (GameBoardModels.cubeCount.blackCubes <= 0)
                            {
                                throw new InvalidOperationException("Game Over");
                            }
                            return city.blackCubes;

                        }
                        Outbreak(city, city.color, city.adjacentCities, alreadyInfected);
                        return city.blackCubes;

                default:
                    if (city.redCubes < 3)
                    {
                        GameBoardModels.cubeCount.redCubes--;
                        city.redCubes++;
                        if (GameBoardModels.cubeCount.redCubes <= 0)
                        {
                            throw new InvalidOperationException("Game Over");
                        }
                        return city.redCubes;
                    }
                    Outbreak(city, city.color, city.adjacentCities, alreadyInfected);
                    return city.redCubes;
            }
        }

        //returns a list of the cities that have already been infected
        public static HashSet<City> Outbreak(City city, COLOR color, HashSet<City> adjacentCities, HashSet<City> alreadyInfected)
        {
            new PicForm(true, city.Name).Show();
            alreadyInfected.Add(city);
            foreach (var neighbor in adjacentCities)
            {
                if (!alreadyInfected.Contains(neighbor))
                {
                    //alreadyInfected.Add(neighbor);
                    InfectCity(neighbor, alreadyInfected, true, color);                   
                }
            }
            GameBoardModels.outbreakMarker++;
            if (GameBoardModels.outbreakMarker == 8)
            {
                throw new InvalidOperationException("Game Over");
            }
            return alreadyInfected;
        }
    }
}
