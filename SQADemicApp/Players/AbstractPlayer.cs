using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using SQADemicApp.BL;
using SQADemicApp.Objects;
using SQADemicApp.Players;
using SQADemicApp;

namespace SQADemicApp
{

    public abstract class AbstractPlayer
    {
        private static int MAXCUBECOUNT = 24;
        protected int MAXTURNCOUNT = 4;
        protected int MAXHANDSIZE = 7;

        public int getMaxTurnCount()
        {
            return MAXTURNCOUNT;
        }

        public Dictionary<String, AbstractSpecialAction> specialActions = new Dictionary<String, AbstractSpecialAction>();

        public List<Card> hand { get; set; }
        public City currentCity { get; set; }
        public AbstractPlayer()
        {
            hand = new List<Card>();
            currentCity = Create.cityDictionary["Atlanta"];
        }

        public List<Object> HandStringList()
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

        /// <summary>
        /// Determins if the player can use a Charter Flight
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="currentCity"></param>
        /// <returns></returns>
        public static bool CharterFlightOption(List<Card> hand, City currentCity)
        {
            return (hand.Count(c => c.CityName == currentCity.Name) == 1);
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
        /// <param name="city">City to move to</param>
        /// <returns>Success Flag/returns>
        public virtual bool MovePlayer(City city)
        {
            // TODO: Further refactoring is possible but has less priority.
            if (DriveOptions(currentCity).Any(c => c.Name.Equals(city.Name)))
            {
                //Do Nothing
            }
            else if (ShuttleFlightOption(currentCity).Contains(city.Name))
            {
                //Do Nothing
            }
            else if (DirectFlightOption(hand, currentCity).Contains(city.Name))
            {
                GameBoardModels.DiscardCard(hand.First(c => c.CityName.Equals(city.Name)));
                hand.RemoveAll(x => x.CityName.Equals(city.Name));
            }
            else if (CharterFlightOption(hand, currentCity))
            {
                GameBoardModels.DiscardCard(hand.First(c => c.CityName.Equals(currentCity.Name)));
                hand.RemoveAll(x => x.CityName.Equals(currentCity.Name));
            }
            else
            {
                return false;
            }
            currentCity = city;
            return true;
        }

        /// <summary>
        /// Moves the player to the destination city
        /// </summary>
        /// <param name="players">List of Players</param>
        /// <param name="destination">Name of the City to be moved to</param>
        /// <returns>Status Flag</returns>
        public virtual bool DispatcherMovePlayer(List<AbstractPlayer> players, City destination)
        {
            // TODO: Fix test cases, replace current codes with commented blocks
            if (!(DriveOptions(currentCity).Any(p => p.Name.Equals(destination.Name)) ||
                    players.Any(p => p.currentCity.Name.Equals(destination.Name)) || 
                    ShuttleFlightOption(currentCity).Contains(destination.Name)))
            {
                return false;
            }
            currentCity = destination;
            return true;

            /**
            // This won't work. Must return false to indicate not supported operation.
            // throw new NotSupportedException("Only dispatcher can perform this operation.");   
            return false;
            */
        }

        /// <summary>
        ///  Builds a Research Station should the conditions be meet
        /// </summary>
        /// <returns>Success Flag</returns>
        /// TODO: If research stations on board = 6, call something to move research station
        public virtual bool BuildAResearchStationOption()
        {
            if (CityBL.getCitiesWithResearchStations().Contains(currentCity))
                return false;
            if (hand.Any(c => c.CityName.Equals(currentCity.Name)))
            {
                hand.RemoveAll(x => x.CityName.Equals(currentCity.Name));
                currentCity.researchStation = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Cures the color if possible
        /// </summary>
        /// <param name="cardsToSpend"></param>
        /// <param name="color"></param>
        /// <returns>Success Flag</returns>
        public bool Cure(List<String> cardsToSpend, COLOR color)
        {
            var cards = hand.Where(x => x.CityColor == color && cardsToSpend.Contains(x.CityName));

            if (!CanCure(cards.Count(), color))
                return false;

            var discard = hand.Where(c => cards.Contains(c));
            foreach (Card card in discard)
                GameBoardModels.DiscardCard(card);

            hand.RemoveAll(x => cards.Contains(x));
            GameBoardModels.SetCureStatus(color, Cures.CURESTATE.Cured);
            AllCureCheck();

            return true;
        }

        public bool CanCure(int numberOfAvailableCards, COLOR color)
        {
            if (!currentCity.researchStation || GameBoardModels.GetCureStatus(color) != Cures.CURESTATE.NotCured)
                return false;
            if (GetType() == typeof(ScientistPlayer))
            {
                if (numberOfAvailableCards != 4)
                    return false;
            }
            else if (numberOfAvailableCards != 5)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if every disease has been cured and throws the win exception if true,
        /// otherwise, do nothing.
        /// </summary>
        public void AllCureCheck()
        {
            if (GameBoardModels.GetCureStatus(COLOR.black) != Cures.CURESTATE.NotCured &&
                   GameBoardModels.GetCureStatus(COLOR.blue) != Cures.CURESTATE.NotCured &&
                   GameBoardModels.GetCureStatus(COLOR.red) != Cures.CURESTATE.NotCured &&
                   GameBoardModels.GetCureStatus(COLOR.yellow) != Cures.CURESTATE.NotCured)
            {
                throw new InvalidOperationException("Game Over You Win");
            }
        }

        /// <summary>
        /// Helper method for Treat Disease Option
        /// </summary>
        /// <param name="city"></param>
        /// <param name="color"></param>
        /// <returns>number of disease cubes in the city</returns>
        public static int GetDiseaseCubes(City city, COLOR color)
        {
            return city.Cubes.GetCubeCount(color);
        }

        /// <summary>
        /// Helper method for Treat Disease Option
        /// </summary>
        /// <param name="city"></param>
        /// <param name="color"></param>
        /// <param name="numberAfterCure"></param>
        /// <param name="numberBeforeCure"></param>
        public static bool SetDiseaseCubes(City city, COLOR color, int numberAfterCure, int numberBeforeCure)
        {
            numberAfterCure = GameBoardModels.GetCureStatus(color) == Cures.CURESTATE.Cured ? 0 : numberAfterCure;
            GameBoardModels.AddInfectionCubes(color, numberBeforeCure - numberAfterCure);
            city.Cubes.SetCubeCount(color, numberAfterCure);
            if (GameBoardModels.GetInfectionCubeCount(color) == MAXCUBECOUNT && GameBoardModels.GetCureStatus(color) == Cures.CURESTATE.Cured)
                GameBoardModels.SetCureStatus(color, Cures.CURESTATE.Sunset);
            return true;
        }

        /// <summary>
        /// Treates the Diesease if possible
        /// </summary>
        /// <param name="player"></param>
        /// <param name="color"></param>
        /// <returns>Success Flag</returns>
        public virtual bool TreatDiseaseOption(COLOR color)
        {
            int number = GetDiseaseCubes(currentCity, color);
            if (number < 1)
                return false;
            return SetDiseaseCubes(currentCity, color, number - 1, number);
        }

        /// <summary>
        /// Allows Players to Trade Cards (from current player to receiver)
        /// </summary>
        /// <param name="reciver">Player who will recive the card</param>
        /// <param name="cityname">Name on the card to be traded</param>
        /// <returns>Sucess Flag</returns>
        public bool ShareKnowledgeOption(AbstractPlayer reciver, string cityname)
        {
            if (currentCity != reciver.currentCity ||
                (!reciver.currentCity.Name.Equals(cityname) && GetType() != typeof(ResearcherPlayer)))
                return false;
            int index = hand.FindIndex(x => x.CityName.Equals(cityname));
            if (index == -1)
                return false;
            reciver.hand.Add(hand[index]);
            hand.RemoveAt(index);
            return true;
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public List<String> GetSpecialActions()
        {
            return new List<String>(this.specialActions.Keys);
        }


        public bool PreformSpecialAction(String actionName)
        {
            if (this.specialActions.ContainsKey(actionName))
            {
                return this.specialActions[actionName].PreformAction();
            }

            return false;
        }

        public bool addCardToHand(Card card)
        {
            //right now the drawn card will just be discarded if there is not enough room for it.
            //This should be changed... right?
            if (this.hand.Count < MAXHANDSIZE)
            {
                this.hand.Add(card);
                return true;
            }
            return false;
        }

        public virtual Dictionary<Button, LambdaExpression> getAvailableButton()
        {
            return null;
        }
    }
}
