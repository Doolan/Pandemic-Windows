using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.BL
{
    
    public class PlayerActionsBL
    {
        private static int MAXCUBECOUNT = 24;
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

        /// <summary>
        /// Determins if the player can use a Charter Flight
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="currentCity"></param>
        /// <returns></returns>
        public static bool CharterFlightOption(List<Card> hand, City currentCity)
        {
            return (hand.Where(c => c.CityName == currentCity.Name).Count() == 1);
        }

        /// <summary>
        /// Determines if the player can use a Shuttle Flight and returns list of possibilities
        /// </summary>
        /// <param name="currentCity"></param>
        /// <returns></returns>
        public static List<String> ShuttleFlightOption(City currentCity)
        {
            if (!currentCity.researchStation)
            {
                return new List<String>();
            }
            List<String> options = new List<String>();
            List<City> stations = CityBL.getCitiesWithResearchStations();
            foreach (var city in stations)
            {
                if (city.Name != currentCity.Name)
                {
                    options.Add(city.Name);
                }
            }
            return options;
        }

        /// <summary>
        /// Moves the player to the given city, updating the hand if needed
        /// </summary>
        /// <param name="player">Current Player or if dispatcher, player trying to move</param>
        /// <param name="city">City to move to</param>
        /// <returns>Success Flag/returns>
        public static bool moveplayer(Player player, City city)
        {
            if (DriveOptions(player.currentCity).Any(c => c.Name.Equals(city.Name))){
                //Do Nothing
            }
            else if(ShuttleFlightOption(player.currentCity).Contains(city.Name)){
                //Do Nothing
            }
            else if (DirectFlightOption(player.hand, player.currentCity).Contains(city.Name))
            {
                player.hand.RemoveAll(x => x.CityName.Equals(city.Name));
            }
            else if (CharterFlightOption(player.hand, player.currentCity))
            {
                player.hand.RemoveAll(x => x.CityName.Equals(player.currentCity.Name));
            }
            else
            {
                return false;
            }
            player.currentCity = city;
            return true;
        }

        /// <summary>
        /// Moves the player to the destination city
        /// </summary>
        /// <param name="player">Player to be moved</param>
        /// <param name="players">List of Players</param>
        /// <param name="destination">Name of the City to be moved to</param>
        /// <returns>Status Flag</returns>
        public static bool DispatcherMovePlayer(Player player, List<Player> players, City destination)
        {
            if (DriveOptions(player.currentCity).Any(p => p.Name.Equals(destination.Name)))
            {
                //Do nothing
            }
            else if(players.Any(p => p.currentCity.Name.Equals(destination.Name))){
                //Do nothing
            }
            else if (ShuttleFlightOption(player.currentCity).Contains(destination.Name))
            {
                //Do Nothing
            }
            else{
                return false;
            }
            player.currentCity = destination;
            return true;
        }

        /// <summary>
        ///  Builds a Research Station should the conditions be meet
        /// </summary>
        /// <param name="player">Current Player</param>
        /// <returns>Success Flag</returns>
        public static bool BuildAResearchStationOption(Player player)
        {
            if (CityBL.getCitiesWithResearchStations().Contains(player.currentCity))
                return false;
            else if (player.role == ROLE.OpExpert)
            {
                player.currentCity.researchStation = true;
                return true;
            }
            else if (player.hand.Any(c => c.CityName.Equals(player.currentCity.Name)))
            {
                player.hand.RemoveAll(x => x.CityName.Equals(player.currentCity.Name));
                player.currentCity.researchStation = true;
                return true;
            }
            return false;

        }


        /// <summary>
        /// Cures the color if possible
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="currentCity"></param>
        /// <param name="role"></param>
        /// <returns>Success Flag</returns>
        public static bool Cure(Player player, List<String> cardsToSpend, COLOR color)
        {
            if (!player.currentCity.researchStation || GameBoardModels.CURESTATUS.getCureStatus(color) != GameBoardModels.Cures.CURESTATE.NotCured)
                return false;
            var cards = player.hand.Where(x => x.CityColor == color && cardsToSpend.Contains(x.CityName));
            if (player.role == ROLE.Scientist)
            {
                if (cards.Count() != 4)
                    return false;
            }
            else if (cards.Count() != 5)
                return false;
            player.hand.RemoveAll(x => cards.Contains(x));
            GameBoardModels.CURESTATUS.setCureStatus(color, GameBoardModels.Cures.CURESTATE.Cured);
            if (GameBoardModels.CURESTATUS.BlackCure != GameBoardModels.Cures.CURESTATE.NotCured &&
                   GameBoardModels.CURESTATUS.BlueCure != GameBoardModels.Cures.CURESTATE.NotCured &&
                   GameBoardModels.CURESTATUS.RedCure != GameBoardModels.Cures.CURESTATE.NotCured &&
                   GameBoardModels.CURESTATUS.YellowCure != GameBoardModels.Cures.CURESTATE.NotCured)
            {
                throw new InvalidOperationException("Game Over You Win");
            }

            return true;
        }

        /// <summary>
        /// Treates the Diesease if possible
        /// </summary>
        /// <param name="player"></param>
        /// <param name="color"></param>
        /// <returns>Success Flag</returns>
        public static bool TreatDiseaseOption(Player player, COLOR color)
        {
            int number =  getDiseaseCubes(player.currentCity, color);
            if (number < 1)
                return false;
            return SetDiseaseCubes(player.currentCity, color, player.role == ROLE.Medic ? 0 : (number - 1), number);
        }

        /// <summary>
        /// Helper method for Treat Disease Option
        /// </summary>
        /// <param name="city"></param>
        /// <param name="color"></param>
        /// <returns>number of disease cubes in the city</returns>
        private static int getDiseaseCubes(City city, COLOR color){
            switch (color)
            {
                case COLOR.red:
                   return city.redCubes;
                case COLOR.blue:
                   return city.blueCubes;
                case COLOR.yellow:
                   return city.yellowCubes;
                case COLOR.black:
                   return city.blackCubes;
                default:
                    throw new ArgumentException("invalid color"); 
            }
        }

        /// <summary>
        /// Helper method for Treat Disease Option
        /// </summary>
        /// <param name="city"></param>
        /// <param name="color"></param>
        /// <param name="numberAfterCure"></param>
        /// <param name="numberBeforeCure"></param>
        private static bool SetDiseaseCubes(City city, COLOR color, int numberAfterCure, int numberBeforeCure)
        {
            switch (color)
            {
                case COLOR.red:
                    numberAfterCure = GameBoardModels.CURESTATUS.RedCure == GameBoardModels.Cures.CURESTATE.Cured ? 0 : numberAfterCure;
                    GameBoardModels.cubeCount.InfectionCubes.AddCubes(color, numberBeforeCure - numberAfterCure);
                    city.redCubes = numberAfterCure;
                    if (GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(color) == MAXCUBECOUNT && GameBoardModels.CURESTATUS.RedCure == GameBoardModels.Cures.CURESTATE.Cured)
                        GameBoardModels.CURESTATUS.RedCure = GameBoardModels.Cures.CURESTATE.Sunset;
                    break;
                case COLOR.blue:
                    numberAfterCure = GameBoardModels.CURESTATUS.BlueCure == GameBoardModels.Cures.CURESTATE.Cured ? 0 : numberAfterCure;
                    GameBoardModels.cubeCount.InfectionCubes.AddCubes(color, numberBeforeCure - numberAfterCure);
                    city.blueCubes = numberAfterCure;
                    if (GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(color) == MAXCUBECOUNT && GameBoardModels.CURESTATUS.BlueCure == GameBoardModels.Cures.CURESTATE.Cured)
                        GameBoardModels.CURESTATUS.BlueCure = GameBoardModels.Cures.CURESTATE.Sunset;                    
                    break;
                case COLOR.yellow:
                    numberAfterCure = GameBoardModels.CURESTATUS.YellowCure == GameBoardModels.Cures.CURESTATE.Cured ? 0 : numberAfterCure;
                    GameBoardModels.cubeCount.InfectionCubes.AddCubes(color, numberBeforeCure - numberAfterCure);
                    city.yellowCubes = numberAfterCure;
                    if (GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(color) == MAXCUBECOUNT && GameBoardModels.CURESTATUS.YellowCure == GameBoardModels.Cures.CURESTATE.Cured)
                        GameBoardModels.CURESTATUS.YellowCure = GameBoardModels.Cures.CURESTATE.Sunset;
                    break;
                case COLOR.black:
                    numberAfterCure = GameBoardModels.CURESTATUS.BlackCure == GameBoardModels.Cures.CURESTATE.Cured ? 0 : numberAfterCure;
                    GameBoardModels.cubeCount.InfectionCubes.AddCubes(color, numberBeforeCure - numberAfterCure);
                    city.blackCubes = numberAfterCure;
                    if (GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(color) == MAXCUBECOUNT && GameBoardModels.CURESTATUS.BlackCure == GameBoardModels.Cures.CURESTATE.Cured)
                        GameBoardModels.CURESTATUS.BlackCure = GameBoardModels.Cures.CURESTATE.Sunset;
                    break;
                default:
                    throw new ArgumentException("invalid color");
            }
            return true;
        }
        
        /// <summary>
        /// Allows Players to Trade Cards
        /// </summary>
        /// <param name="sender">Player who holds the card</param>
        /// <param name="reciver">Player who will recive the card</param>
        /// <param name="cityname">Name on the card to be traded</param>
        /// <returns>Sucess Flag</returns>
        public static bool ShareKnowledgeOption(Player sender, Player reciver, string cityname)
        {
            if (sender.currentCity != reciver.currentCity || 
                (!reciver.currentCity.Name.Equals(cityname) && sender.role != ROLE.Researcher))
                return false;
            int index = sender.hand.FindIndex(x => x.CityName.Equals(cityname));
            if (index == -1)
                return false;
            reciver.hand.Add(sender.hand[index]);
            sender.hand.RemoveAt(index);
            return true;
        }

    }
}
