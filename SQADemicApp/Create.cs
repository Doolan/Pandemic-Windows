using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using SQADemicApp.BL;

namespace SQADemicApp
{
    public class Create
    {
        public static Dictionary<string, City> CityDictionary = new Dictionary<string, City>();
        private static bool _alreadySetUp = false;

        /// <summary>
        /// Sets up all of the dictionaries
        /// </summary>
        /// <param name="playerdeck"></param>
        /// <param name="infectionDeck"></param>
        /// <returns>status flag</returns>
        public static bool SetUpCreate(out Card[] playerdeck, out List<string> infectionDeck)
        {
            //Keep from making duplicates
            //if (alreadySetUp)
              //  return false;
            CreateDictionary();
            SetAdjacentCities(new StringReader(Properties.Resources.AdjacentNeighbors));
            playerdeck = MakePlayerDeck();
            infectionDeck = MakeInfectionDeck(new StringReader(Properties.Resources.InfectionDeck));
            CityDictionary["Atlanta"].ResearchStation = true;
            return true;
        }

        /// <summary>
        /// PUBLIC FOR TESTING ONLY
        /// </summary>
        public static void CreateDictionary()
        {
            #region Createcities
            //create the blues
            var sanFrancisco = new City(Color.Blue, "San Francisco");
            var chicago = new City(Color.Blue, "Chicago");
            var montreal = new City(Color.Blue, "Montreal");
            var newYork = new City(Color.Blue, "New York");
            var washington = new City(Color.Blue, "Washington");
            var atlanta = new City(Color.Blue, "Atlanta");
            var london = new City(Color.Blue, "London");
            var madrid = new City(Color.Blue, "Madrid");
            var paris = new City(Color.Blue, "Paris");
            var milan = new City(Color.Blue, "Milan");
            var stPetersburg = new City(Color.Blue, "Saint Petersburg");
            var essen = new City(Color.Blue, "Essen");

            //create the yellows
            var losAngeles = new City(Color.Yellow, "Los Angeles");
            var mexicoCity = new City(Color.Yellow, "Mexico City");
            var miami = new City(Color.Yellow, "Miami");
            var bogota = new City(Color.Yellow, "Bogota");
            var lima = new City(Color.Yellow, "Lima");
            var saoPaulo = new City(Color.Yellow, "Sao Paulo");
            var buenosAires = new City(Color.Yellow, "Buenos Aires");
            var santiago = new City(Color.Yellow, "Santiago");
            var lagos = new City(Color.Yellow, "Lagos");
            var khartoum = new City(Color.Yellow, "Khartoum");
            var kinshasa = new City(Color.Yellow, "Kinshasa");
            var johannesburg = new City(Color.Yellow, "Johannesburg");

            //create the blacks
            var algiers = new City(Color.Black, "Algiers");
            var cairo = new City(Color.Black, "Cairo");
            var istanbul = new City(Color.Black, "Istanbul");
            var moscow = new City(Color.Black, "Moscow");
            var baghdad = new City(Color.Black, "Baghdad");
            var riyadh = new City(Color.Black, "Riyadh");
            var tehran = new City(Color.Black, "Tehran");
            var karachi = new City(Color.Black, "Karachi");
            var delhi = new City(Color.Black, "Delhi");
            var mumbai = new City(Color.Black, "Mumbai");
            var chennai = new City(Color.Black, "Chennai");
            var kolkata = new City(Color.Black, "Kolkata");

            //create the reds
            var beijing = new City(Color.Red, "Beijing");
            var seoul = new City(Color.Red, "Seoul");
            var shanghai = new City(Color.Red, "Shanghai");
            var tokyo = new City(Color.Red, "Tokyo");
            var osaka = new City(Color.Red, "Osaka");
            var taipei = new City(Color.Red, "Taipei");
            var hongKong = new City(Color.Red, "Hong Kong");
            var bangkok = new City(Color.Red, "Bangkok");
            var manila = new City(Color.Red, "Manila");
            var hoChiMinhCity = new City(Color.Red, "Ho Chi Minh City");
            var jakarta = new City(Color.Red, "Jakarta");
            var sydney = new City(Color.Red, "Sydney");
            #endregion

            try
            {
                #region loadDictionary
                CityDictionary.Add("San Francisco", sanFrancisco);
                CityDictionary.Add("Chicago", chicago);
                CityDictionary.Add("Montreal", montreal);
                CityDictionary.Add("New York", newYork);
                CityDictionary.Add("Atlanta", atlanta);
                CityDictionary.Add("Washington", washington);
                CityDictionary.Add("London", london);
                CityDictionary.Add("Essen", essen);
                CityDictionary.Add("Saint Petersburg", stPetersburg);
                CityDictionary.Add("Milan", milan);
                CityDictionary.Add("Paris", paris);
                CityDictionary.Add("Madrid", madrid);
                CityDictionary.Add("Los Angeles", losAngeles);
                CityDictionary.Add("Mexico City", mexicoCity);
                CityDictionary.Add("Miami", miami);
                CityDictionary.Add("Bogota", bogota);
                CityDictionary.Add("Lima", lima);
                CityDictionary.Add("Santiago", santiago);
                CityDictionary.Add("Buenos Aires", buenosAires);
                CityDictionary.Add("Sao Paulo", saoPaulo);
                CityDictionary.Add("Lagos", lagos);
                CityDictionary.Add("Kinshasa", kinshasa);
                CityDictionary.Add("Johannesburg", johannesburg);
                CityDictionary.Add("Khartoum", khartoum);
                CityDictionary.Add("Moscow", moscow);
                CityDictionary.Add("Tehran", tehran);
                CityDictionary.Add("Delhi", delhi);
                CityDictionary.Add("Kolkata", kolkata);
                CityDictionary.Add("Istanbul", istanbul);
                CityDictionary.Add("Baghdad", baghdad);
                CityDictionary.Add("Karachi", karachi);
                CityDictionary.Add("Algiers", algiers);
                CityDictionary.Add("Cairo", cairo);
                CityDictionary.Add("Riyadh", riyadh);
                CityDictionary.Add("Mumbai", mumbai);
                CityDictionary.Add("Chennai", chennai);
                CityDictionary.Add("Beijing", beijing);
                CityDictionary.Add("Seoul", seoul);
                CityDictionary.Add("Shanghai", shanghai);
                CityDictionary.Add("Tokyo", tokyo);
                CityDictionary.Add("Osaka", osaka);
                CityDictionary.Add("Taipei", taipei);
                CityDictionary.Add("Hong Kong", hongKong);
                CityDictionary.Add("Bangkok", bangkok);
                CityDictionary.Add("Manila", manila);
                CityDictionary.Add("Ho Chi Minh City", hoChiMinhCity);
                CityDictionary.Add("Jakarta", jakarta);
                CityDictionary.Add("Sydney", sydney);
                #endregion
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// PUBLIC FOR TESTING ONLY 
        /// </summary>
        /// <param name="reader"></param>
        public static void SetAdjacentCities(StringReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var cityname = line.Substring(0, line.IndexOf(";"));
                var adjcities = line.Substring(line.IndexOf(";") + 1);
                var adjCityList = adjcities.Split(',');

                foreach (var city in adjCityList)
                {
                    CityDictionary[cityname].AdjacentCities.Add(CityDictionary[city]);
                }
            }

        }

        /// <summary>
        /// PUBLIC FOR TESTING ONLY
        /// </summary>
        /// <returns></returns>
        public  static Card[] MakePlayerDeck()
        {
            var deck = new Card[57];
            var rand = new Random();
            deck[rand.Next(0, 9)] = new Card("EPIDEMIC", Card.Cardtype.Epidemic);
            deck[rand.Next(10, 19)] = new Card("EPIDEMIC", Card.Cardtype.Epidemic);
            deck[rand.Next(20, 29)] = new Card("EPIDEMIC", Card.Cardtype.Epidemic);
            deck[rand.Next(30, 39)] = new Card("EPIDEMIC", Card.Cardtype.Epidemic);
            var cardList = MakeCardList(new StringReader(SQADemicApp.Properties.Resources.CityList));
            cardList = HelperBL.shuffleArray(cardList);
            var j = 0;
            for (var i = 0; i < 57; i++)
            {
                if (deck[i] != null) continue;
                deck[i] = cardList[j];
                j++;
            }

            return deck;
        }

        /// <summary>
        /// PUBLIC FOR TESTING ONLY
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<string> MakeInfectionDeck(StringReader r)
        {
            string line;
            string[] infectionDeckArray = null;
            while ((line = r.ReadLine()) != null)
            {
                infectionDeckArray = line.Split(',');
            }
            var shuffledDeck = HelperBL.ShuffleArray(infectionDeckArray);
            return shuffledDeck.ToList();
        }

        /// <summary>
        /// PUBLIC FOR TESTING ONLY
        /// </summary>
        /// <param name="stringReader"></param>
        /// <returns></returns>
        public static List<Card> MakeCardList(StringReader stringReader)
        {
            var cardList = new List<Card>();
            string line;
            while ((line = stringReader.ReadLine()) != null)
            {
                var cardName = line.Substring(0, line.IndexOf(";"));
                var cardColor = line.Substring(line.IndexOf(";") + 2);
                var color = GetColor(cardColor);
                cardList.Add(new Card(cardName, Card.Cardtype.City, color));
            }
            cardList.Add(new Card("Airlift", Card.Cardtype.Special));
            cardList.Add(new Card("One Quiet Night", Card.Cardtype.Special));
            cardList.Add(new Card("Resilient Population", Card.Cardtype.Special));
            cardList.Add(new Card("Government Grant", Card.Cardtype.Special));
            cardList.Add(new Card("Forecast", Card.Cardtype.Special));
            return cardList;
        }

        private static Color GetColor(string color)
        {
            switch (color.ToLower())
            {
                case "red":
                    return Color.Red;
                case "black":
                    return Color.Black;
                case "yellow":
                    return Color.Yellow;
                default:
                    return Color.Blue;
            }

        }

    }

}