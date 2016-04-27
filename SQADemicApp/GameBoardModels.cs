using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using SQADemicApp.BL;
using SQADemicApp.Objects;
using SQADemicApp.Players;

namespace SQADemicApp
{
    public enum Color { Red, Black, Blue, Yellow }
    public class GameBoardModels
    {
        #region Public Static Vars 
        private static List<string> _citiesWithResearchStations;
        private static int _outbreakMarker = 0;
        private static int _currentPlayerIndex;
        public static List<Card> EventCards;
        public static int InfectionRate;
        public static int InfectionRateIndex;
        #endregion

        #region Public Vars
        public int CurrentPlayerTurnCounter;
        #endregion

        #region private vars
        private static AbstractPlayer[] _players;
        private static Cures _curestatus;
        private static InfectionCubes _cubeCount;
        private static bool _alreadySetUp = false;
        private static LinkedList<string> _infectionDeck;
        private static LinkedList<string> _infectionPile;
        private static Stack<Card> _playerDeck;
        private static Stack<Card> _discardPlayerDeck;
        private const int Maxcubecount = 24;

        #endregion

        /// <summary>
        /// Acts as a Main function. Sets up the game and the board state
        /// </summary>
        /// <param name="playersroles"></param>
        public GameBoardModels(IReadOnlyList<string> playersroles)
        {
            //Keep from making duplicates
            if (!_alreadySetUp)
            {
                //set vars
                _outbreakMarker = 0;
                CreateInfectionCubes(Maxcubecount);
                _curestatus = new Cures();
              //  CURESTATUS.BlackCure = CURESTATUS.BlueCure = CURESTATUS.RedCure = CURESTATUS.YellowCure = Cures.CURESTATE.NotCured;
                Card[] playerDeckArray;
                List<string> infectionDeckList;
                Create.SetUpCreate(out playerDeckArray, out infectionDeckList);
                _playerDeck = new Stack<Card>(playerDeckArray);
                _discardPlayerDeck = new Stack<Card>();
                EventCards = new List<Card>();
                _infectionPile = new LinkedList<string>();
                _infectionDeck = new LinkedList<string>(Create.MakeInfectionDeck(new StringReader(SQADemicApp.Properties.Resources.InfectionDeck)));
            }

            CurrentPlayerTurnCounter = 0;
            _currentPlayerIndex = 0;
            
            GameBoardModels._players = CreatePlayers(playersroles);
            
            InfectionRate = 2;
            InfectionRateIndex = 0;
            if (!_alreadySetUp)
            {
                StartGameInfection();
                SetUpPlayerHands();
            }

            _alreadySetUp = true;

        }


        #region Setup & Config

        private static AbstractPlayer[] CreatePlayers(IReadOnlyList<string> playersroles)
        {
            //Players setup allows existing players to be overwritten
            var abstractPlayers = new AbstractPlayer[playersroles.Count];
            for (var i = 0; i < playersroles.Count(); i++)
            {
                switch (playersroles[i])
                {
                    case "Dispatcher":
                        abstractPlayers[i] = new DispatcherPlayer();
                        break;
                    case "Operations Expert":
                        abstractPlayers[i] = new OpExpertPlayer();
                        break;
                    case "Scientist":
                        abstractPlayers[i] = new ScientistPlayer();
                        break;
                    case "Medic":
                        abstractPlayers[i] = new MedicPlayer();
                        break;
                    case "Researcher":
                        abstractPlayers[i] = new ResearcherPlayer();
                        break;
                    case "Containment Specialist":
                        abstractPlayers[i] = new ContainmentSpecialstPlayer();
                        break;
                    case "Generalist":
                        abstractPlayers[i] = new GeneralistPlayer();
                        break;
                    case "Archivist":
                        abstractPlayers[i] = new ArchivistPlayer();
                        break;
                    case "Troubleshooter":
                        abstractPlayers[i] = new TroubleshooterPlayer();
                        break;
                    case "Field Operative":
                        abstractPlayers[i] = new FieldOperativePlayer();
                        break;
                    default:
                        abstractPlayers[i] = null;
                        break;
                }
            }
            return abstractPlayers;
        }

        private static void StartGameInfection()
        {
            for (var i = 3; i > 0; i--)
            {
                var infectedcites = InfectorBL.InfectCities(_infectionDeck, _infectionPile, 3);
                for (var j = 0; j < i; j++)
                {
                    InfectorBL.InfectCities(infectedcites);
                }
            }
        }

        private static void SetUpPlayerHands()
        {
            var cardsPerPlayer = _players.Count() == 4 ? 2 : _players.Count() == 3 ? 3 : 4;
            foreach (var player in _players)
            {
                for (var i = 0; i < cardsPerPlayer; i++)
                {
                    var card = DrawCard();
                    if (card.CardType.Equals(Card.Cardtype.Epidemic))
                    {
                        var infectcityname = InfectorBL.Epidemic(GameBoardModels._infectionDeck, GameBoardModels._infectionPile, ref GameBoardModels.InfectionRateIndex, ref GameBoardModels.InfectionRate);
                        new PicForm(false, infectcityname).Show();
                        for (var j = 0; j < 3; j++)
                        {
                            InfectorBL.InfectCities(new List<string> { infectcityname });
                        }
                    }
                    else if (card.CardType == Card.Cardtype.Special)
                        EventCards.Add(card);
                    else
                        player.AddCardToHand(card);
                }
            }
        }

        #endregion

        /** BEGIN Command Pattern Methods **/
        #region CureStatus

        private static void GenerateCures()
        {
            _curestatus = new Cures(Cures.Curestate.NotCured);
        }

        public static Cures.Curestate GetCureStatus(Color color)
        {
            return _curestatus.GetCureStatus(color);
        }

        public static void SetCureStatus(Color color, Cures.Curestate curestate)
        {
            _curestatus.SetCureStatus(color, curestate);
        }
        #endregion

        #region Infection Cubes 

        private static void CreateInfectionCubes(int startingValue)
        {
            _cubeCount = new InfectionCubesBoard(startingValue);
        }

        public static void IncrementInfectionCubes(Color color)
        {
            _cubeCount.IncrementCubes(color);
        }

        public static void DecrementInfectionCubeCount(Color color)
        {
            _cubeCount.DecrementCubeCount(color);
        }

        public static void AddInfectionCubes(Color color, int value)
        {
            _cubeCount.AddCubes(color, value);
        }

        public static void SetInfectionCubeCount(Color color, int value)
        {
            _cubeCount.SetCubeCount(color, value);
        }

        public static int GetInfectionCubeCount(Color color)
        {
            return _cubeCount.GetCubeCount(color);
        }
        #endregion

        #region Infection and Outbreak 

        public static void IncrementOutbreakMarker()
        {
            GameBoardModels._outbreakMarker ++;
            if (GameBoardModels._outbreakMarker == 8)
            {
                throw new InvalidOperationException("Game Over");
            }
        }

        public static int GetOutbreakMarker()
        {
            return GameBoardModels._outbreakMarker;
        }

        public static void SetOutbreakMarker(int marker)
        {
            GameBoardModels._outbreakMarker = marker;
        }


        #endregion

        #region Players
        public static AbstractPlayer GetCurrentPlayer()
        {
            return _players[_currentPlayerIndex];
        }

        public static AbstractPlayer[] GetPlayers()
        {
            return _players;
        }

        public static AbstractPlayer GetPlayerByIndex(int index)
        {
            return _players[index];
        }

        public static int GetPlayerCount()
        {
            return _players.Length;
        }


        public bool IncTurnCount()
        {
            if (CurrentPlayerTurnCounter >= GetCurrentPlayer().GetMaxTurnCount() - 1)
            {
                //CurrentPlayerIndex = (CurrentPlayerIndex + 1) % players.Count();
                CurrentPlayerTurnCounter = 0;
                return true;
            }
            else
                CurrentPlayerTurnCounter++;
            return false;

            //currentPlayerTurnCounter++;
        }

        public static int GetCurrentPlayerIndex()
        {
            return GameBoardModels._currentPlayerIndex;
        }

        public static void MoveToNextPlayer()
        {
            GameBoardModels._currentPlayerIndex = (GameBoardModels._currentPlayerIndex  +1) % GameBoardModels.GetPlayerCount();
        }
        #endregion

        #region Decks 

        public static bool PlayerDiscardPileContains(string cityName)
        {
            return GameBoardModels._discardPlayerDeck.Count(c => c.CityName.Equals(cityName)) >0;
        }

        public static Stack<Card> GetPlayerDeck()
        {
            return _playerDeck;
        }

        public static Card ReclaminCityCardFromPlayerDeck(string cityName)
        {
            return GameBoardModels._discardPlayerDeck.First(c => c.CityName.Equals(cityName));
        }

        public static LinkedList<string> GetInfectionDeck()
        {
            return GameBoardModels._infectionDeck;
        }

        public static LinkedList<string> GetInfectionPile()
        {
            return GameBoardModels._infectionPile;
        }

        public static void RemoveFromInfectionPile(string cityName)
        {
            GameBoardModels._infectionPile.Remove(cityName);
        }

        public static void DiscardCard(Card card)
        {
            GameBoardModels._discardPlayerDeck.Push(card);
        }


        #endregion
        

        public static Card DrawCard()
        {
            try
            {
                return _playerDeck.Pop();
            }
            catch (InvalidOperationException e)
            {
                throw new Exception("Game Over");
            }
        }

        public int GetPlayerDeckSize()
        {
            return _playerDeck.Count();
        }
    }
}