using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.BL
{
    public class SpecialEventCardsBL
    {
        /// <summary>
        /// Completes the special actions for the Government Grant Card
        /// Builds a research station in the named city
        /// </summary>
        /// <param name="cityName">City to build a research station</param>
        /// <returns>status flag</returns>
        public static bool GovernmentGrant(string cityName)
        {
            if (Create.cityDictionary[cityName].researchStation == true)
                return false;
            Create.cityDictionary[cityName].researchStation = true;
            return true;
        }

        /// <summary>
        /// Completes the special actions for the AirLift card
        /// Moves a pawn to any city.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="destination"></param>
        /// <returns>status flag</returns>
        public static bool Airlift(AbstractPlayer player, City destination)
        {
            if (player.currentCity.Equals(destination))
                return false;
            player.currentCity = destination;
            return true;
        }

        /// <summary>
        /// Takes a carcd form the Infection Discard Pile and removes it from the game
        /// </summary>
        /// <param name="infectionPile">Infection Discard Pile</param>
        /// <param name="cityname">Card to be removed</param>
        /// <returns>status flag</returns>
        public static bool ResilientPopulation(LinkedList<String> infectionPile, string cityname)
        {
            return infectionPile.Remove(cityname);
        }

        /// <summary>
        /// Removes the top 6 cards from the Infection Deck
        /// ~ Call this first
        /// </summary>
        /// <param name="infectionDeck"></param>
        /// <returns>Top 6 cards to be examined</returns>
        public static List<string> GetForcastCards(LinkedList<String> infectionDeck)
        {
            List<string> returnList = new List<string>();

            for (int _ = 0; _ < 6; _++)
            {
                returnList.Add(infectionDeck.First.Value);
                infectionDeck.RemoveFirst();
            }
            return returnList;
        }
        /// <summary>
        /// Replaces the 6 cards on the infection deck
        /// ~ Call this second
        /// </summary>
        /// <param name="infectionDeck"></param>
        /// <param name="orderedCards">6 cards in the order to be replaced</param>
        /// <returns>status flag</returns>
        public static bool CommitForcast(LinkedList<String> infectionDeck, List<String> orderedCards)
        {
            if (orderedCards.Count != 6)
                return false;
            for (int i = 5; i >= 0; i--)
            {

                infectionDeck.AddFirst(orderedCards[i]);
            }
            return true;
        }
    }
}
