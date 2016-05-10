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
        private const int Maxcubecount = 24;
        protected int Maxturncount = 4;
        protected int Maxhandsize = 7;

        public int GetMaxTurnCount()
        {
            return Maxturncount;
        }

        public int GetMaxHandSize()
        {
            return Maxhandsize;
        }

        public Dictionary<string, AbstractSpecialAction> SpecialActions = new Dictionary<string, AbstractSpecialAction>();

        public List<Card> Hand { get; set; }
        public City CurrentCity { get; set; }

        protected AbstractPlayer()
        {
            Hand = new List<Card>();
            CurrentCity = Create.CityDictionary["Atlanta"];
        }

        public List<object> HandStringList()
        {
            var stringHand = new List<object>();
            if (Hand.Equals(null))
            {
                return stringHand;
            }
            stringHand.AddRange(Hand.Select(card => card.ToString()).Cast<object>());
            return stringHand;
        }

        /// <summary>
        /// Finds the possible cities a player can move to
        /// </summary>
        /// <param name="currentCity"></param>
        /// <returns>List of Cities</returns>
        public static HashSet<City> DriveOptions(City currentCity)
        {
            return currentCity.GetAdjacentCities();
        }

        /// <summary>
        /// Finds the possible cities a player can fly to by burning a card
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="currentCity"></param>
        /// <returns></returns>
        public static List<string> DirectFlightOption(List<Card> hand, City currentCity)
        {
            var reducedHand = hand.Where(c => !c.CityName.Equals(currentCity.Name) && c.CardType == Card.Cardtype.City);

            return reducedHand.Select(card => card.CityName).ToList();
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
        public static List<string> ShuttleFlightOption(City currentCity)
        {
            if (!currentCity.ResearchStation)
            {
                return new List<string>();
            }
            var stations = CityBL.GetCitiesWithResearchStations();
            return (from city in stations where city.Name != currentCity.Name select city.Name).ToList();
        }

        /// <summary>
        /// Moves the player to the given city, updating the hand if needed
        /// </summary>
        /// <param name="city">City to move to</param>
        /// <returns>Success Flag</returns>
        public virtual bool MovePlayer(City city)
        {
            // TODO: Further refactoring is possible but has less priority.
            if (DriveOptions(CurrentCity).Any(c => c.Name.Equals(city.Name)))
            {
                //Do Nothing
            }
            else if (ShuttleFlightOption(CurrentCity).Contains(city.Name))
            {
                //Do Nothing
            }
            else if (DirectFlightOption(Hand, CurrentCity).Contains(city.Name))
            {
                RemoveMovementCards(city);
            }
            else if (CharterFlightOption(Hand, CurrentCity))
            {
                RemoveMovementCards(CurrentCity);
            }
            else
            {
                return false;
            }
            CurrentCity = city;
            return true;
        }

        public virtual void RemoveMovementCards(City city)
        {
            GameBoardModels.DiscardCard(Hand.First(c => c.CityName.Equals(city.Name)));
            Hand.RemoveAll(x => x.CityName.Equals(city.Name));
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
            if (!(DriveOptions(CurrentCity).Any(p => p.Name.Equals(destination.Name)) ||
                    players.Any(p => p.CurrentCity.Name.Equals(destination.Name)) || 
                    ShuttleFlightOption(CurrentCity).Contains(destination.Name)))
            {
                return false;
            }
            CurrentCity = destination;
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
            if (CityBL.GetCitiesWithResearchStations().Contains(CurrentCity))
                return false;
            if (!Hand.Any(c => c.CityName.Equals(CurrentCity.Name))) return false;
            Hand.RemoveAll(x => x.CityName.Equals(CurrentCity.Name));
            CurrentCity.ResearchStation = true;
            return true;
        }

        /// <summary>
        /// Cures the color if possible
        /// </summary>
        /// <param name="cardsToSpend"></param>
        /// <param name="color"></param>
        /// <returns>Success Flag</returns>
        public bool Cure(List<string> cardsToSpend, Color color)
        {
            var cards = Hand.Where(x => x.CityColor == color && cardsToSpend.Contains(x.CityName));

            if (!CanCure(cards.Count(), color))
                return false;

            var discard = Hand.Where(c => cards.Contains(c));
            foreach (Card card in discard)
                GameBoardModels.DiscardCard(card);

            Hand.RemoveAll(x => cards.Contains(x));
            GameBoardModels.SetCureStatus(color, Cures.Curestate.Cured);
            AllCureCheck();

            return true;
        }

        public bool IsCurable(Color color)
        {
            return CurrentCity.ResearchStation && GameBoardModels.GetCureStatus(color) == Cures.Curestate.NotCured;
        }

        public virtual bool CanCure(int numberOfAvailableCards, Color color)
        {
            return IsCurable(color) && HaveEnoughCardsToCure(numberOfAvailableCards);
        }

        public virtual bool HaveEnoughCardsToCure(int num)
        {
            return num == 5;
        }

        /// <summary>
        /// Checks if every disease has been cured and throws the win exception if true,
        /// otherwise, do nothing.
        /// </summary>
        public void AllCureCheck()
        {
            if (GameBoardModels.GetCureStatus(Color.Black) != Cures.Curestate.NotCured &&
                   GameBoardModels.GetCureStatus(Color.Blue) != Cures.Curestate.NotCured &&
                   GameBoardModels.GetCureStatus(Color.Red) != Cures.Curestate.NotCured &&
                   GameBoardModels.GetCureStatus(Color.Yellow) != Cures.Curestate.NotCured)
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
        public static int GetDiseaseCubes(City city, Color color)
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
        public static bool SetDiseaseCubes(City city, Color color, int numberAfterCure, int numberBeforeCure)
        {
            numberAfterCure = GameBoardModels.GetCureStatus(color) == Cures.Curestate.Cured ? 0 : numberAfterCure;
            GameBoardModels.AddInfectionCubes(color, numberBeforeCure - numberAfterCure);
            city.Cubes.SetCubeCount(color, numberAfterCure);
            if (GameBoardModels.GetInfectionCubeCount(color) == Maxcubecount && GameBoardModels.GetCureStatus(color) == Cures.Curestate.Cured)
                GameBoardModels.SetCureStatus(color, Cures.Curestate.Sunset);
            return true;
        }

        /// <summary>
        /// Treates the Diesease if possible
        /// </summary>
        /// <param name="color"></param>
        /// <returns>Success Flag</returns>
        public virtual bool TreatDiseaseOption(Color color)
        {
            var number = GetDiseaseCubes(CurrentCity, color);
            return number >= 1 && SetDiseaseCubes(CurrentCity, color, number - 1, number);
        }

        /// <summary>
        /// Allows Players to Trade Cards (from current player to receiver)
        /// </summary>
        /// <param name="reciver">Player who will recive the card</param>
        /// <param name="cityname">Name on the card to be traded</param>
        /// <returns>Sucess Flag</returns>
        public bool ShareKnowledgeOption(AbstractPlayer reciver, string cityname)
        {
            if (CurrentCity != reciver.CurrentCity ||
                (!reciver.CurrentCity.Name.Equals(cityname) && GetType() != typeof(ResearcherPlayer)))
                return false;
            var index = Hand.FindIndex(x => x.CityName.Equals(cityname));
            if (index == -1)
                return false;
            reciver.Hand.Add(Hand[index]);
            Hand.RemoveAt(index);
            return true;
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public List<string> GetSpecialActions()
        {
            return new List<string>(this.SpecialActions.Keys);
        }


        public bool PreformSpecialAction(string actionName, GameBoard board)
        {
            if (!this.SpecialActions.ContainsKey(actionName)) return false;
            this.SpecialActions[actionName].setGameBoard(board);
            return this.SpecialActions[actionName].PreformAction();
        }

        public bool AddCardToHand(Card card)
        {
            //we are just going to make the check at the GUI level because its the end of the project
            this.Hand.Add(card);
            return true;
        }
    }
}
