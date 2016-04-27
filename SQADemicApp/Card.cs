using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp
{
    public class Card
    {
        public enum Cardtype { Infection, City, Special, Epidemic }
        public Card(string cityName, Cardtype type, Color color)
        {
            this.CityName = cityName;
            this.CardType = type;
            this.CityColor = color;
        }
        public Card(string cityName, Cardtype type)
        {
            this.CityName = cityName;
            this.CardType = type;
        }
        public string CityName;
        public Cardtype CardType { get; set; }
        public Color CityColor { get; set; }

        public override bool Equals(object obj)
        {
            var objects = (Card)obj;
            return (this.CityName == objects.CityName) && (this.CardType == objects.CardType) && (this.CityColor == objects.CityColor);
        }
    }
}
