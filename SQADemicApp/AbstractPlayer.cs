using System;
using System.Collections.Generic;
using System.Linq;
using SQADemicApp.BL;
using SQADemicApp.Objects;

namespace SQADemicApp
{
    public enum ROLE { Dispatcher, Medic, OpExpert, Researcher, Scientist };

    abstract class AbstractPlayer
    {
        public readonly ROLE role;
        private static int MAXCUBECOUNT = 24;

        public List<Card> hand { get; set; }
        public City currentCity { get; set; }
        public AbstractPlayer()
        {
            hand = new List<Card>();
            currentCity = Create.cityDictionary["Atlanta"];
        }

        public List<Object> handStringList()
        {
            List<Object> stringHand = new List<Object>();
            if (hand.Equals(null))
            {
                return stringHand;
            }
            foreach (var card in hand)
            {
                stringHand.Add(card.CityName + " (" + card.CityColor.ToString() + ")");
            }
            return stringHand;
        }

        /// <summary>
        /// Finds the possible cities a player can move to
        /// </summary>
        /// <param name="currentCity"></param>
        /// <returns>List of Cities</returns>
        public static HashSet<City> DriveOptions(City currentCity)
        {
            return currentCity.getAdjacentCities();
        }

        /// <summary>
        /// Finds the possible cities a player can fly to by burning a card
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="currentCity"></param>
        /// <returns></returns>
        public static List<String> DirectFlightOption(List<Card> hand, City currentCity)
        {
            var reducedHand = hand.Where(c => !c.CityName.Equals(currentCity.Name) && c.CardType == Card.CARDTYPE.City);

            var returnlist = new List<String>();
            foreach (Card card in reducedHand)
            {
                returnlist.Add(card.CityName);
            }

            return returnlist;
        }

    }
}
