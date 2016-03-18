using System;
using System.Collections.Generic;
using System.Linq;

namespace SQADemicApp
{
    public enum ROLE { Dispatcher, Medic, OpExpert, Researcher, Scientist };
    public class Player
    {
        public readonly ROLE role;
        public List<Card> hand {get; set;}
        public City currentCity { get; set; }
        public Player(ROLE role)
        {
            this.role = role;
            hand = new List<Card>();
            currentCity = Create.cityDictionary["Atlanta"];
        }

        public List<Object> handStringList()
        {
            List<Object> stringHand = new List<Object>();
            if(hand.Equals(null))
            {
                return stringHand;
            }
            foreach(var card in hand)
            {
                stringHand.Add(card.CityName + " (" + card.CityColor.ToString() +")");
            }
            return stringHand;
        }
    }

}