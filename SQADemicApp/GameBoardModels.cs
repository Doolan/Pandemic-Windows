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
    public enum COLOR { red, black, blue, yellow }
    public class GameBoardModels
    {
        #region Public Static Vars 
        private static List<String> citiesWithResearchStations;
        private static int outbreakMarker = 0;
        private static int CurrentPlayerIndex;
        public static List<Card> eventCards;
        public static int InfectionRate;
        public static int InfectionRateIndex;
        #endregion

        #region Public Vars
        public int currentPlayerTurnCounter;
        #endregion

        #region private vars
        private static AbstractPlayer[] players;
        private static Cures CURESTATUS;
        private static InfectionCubes cubeCount;
        private static bool alreadySetUp = false;
        private static LinkedList<String> infectionDeck;
        private static LinkedList<String> infectionPile;
        private static Stack<Card> playerDeck;
        private static Stack<Card> discardPlayerDeck;
        private static int MAXCUBECOUNT = 24;
        #endregion

        /// <summary>
        /// Acts as a Main function. Sets up the game and the board state
        /// </summary>
        /// <param name="playersroles"></param>
        public GameBoardModels(IReadOnlyList<string> playersroles)
        {
            //Keep from making duplicates
            if (!alreadySetUp)
            {
                //set vars
                outbreakMarker = 0;
                CreateInfectionCubes(MAXCUBECOUNT);
                CURESTATUS = new Cures();
              //  CURESTATUS.BlackCure = CURESTATUS.BlueCure = CURESTATUS.RedCure = CURESTATUS.YellowCure = Cures.CURESTATE.NotCured;
                Card[] playerDeckArray;
                List<String> infectionDeckList;
                Create.setUpCreate(out playerDeckArray, out infectionDeckList);
                playerDeck = new Stack<Card>(playerDeckArray);
                discardPlayerDeck = new Stack<Card>();
                eventCards = new List<Card>();
                infectionPile = new LinkedList<String>();
                infectionDeck = new LinkedList<string>(Create.makeInfectionDeck(new StringReader(SQADemicApp.Properties.Resources.InfectionDeck)));
            }

            currentPlayerTurnCounter = 0;
            CurrentPlayerIndex = 0;
            
            GameBoardModels.players = CreatePlayers(playersroles);
            
            InfectionRate = 2;
            InfectionRateIndex = 0;
            if (!alreadySetUp)
            {
                startGameInfection();
                setUpPlayerHands();
            }

            alreadySetUp = true;

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

        private void startGameInfection()
        {
            for (int i = 3; i > 0; i--)
            {
                List<string> infectedcites = InfectorBL.InfectCities(infectionDeck, infectionPile, 3);
                for (int j = 0; j < i; j++)
                {
                    InfectorBL.InfectCities(infectedcites);
                }
            }
        }

        private void setUpPlayerHands()
        {
            int cardsPerPlayer = players.Count() == 4 ? 2 : players.Count() == 3 ? 3 : 4;
            foreach (AbstractPlayer player in players)
            {
                for (int i = 0; i < cardsPerPlayer; i++)
                {
                    Card card = DrawCard();
                    if (card.CardType.Equals(Card.CARDTYPE.EPIDEMIC))
                    {
                        string infectcityname = InfectorBL.Epidemic(GameBoardModels.infectionDeck, GameBoardModels.infectionPile, ref GameBoardModels.InfectionRateIndex, ref GameBoardModels.InfectionRate);
                        new PicForm(false, infectcityname).Show();
                        for (int j = 0; j < 3; j++)
                        {
                            InfectorBL.InfectCities(new List<string> { infectcityname });
                        }
                    }
                    else if (card.CardType == Card.CARDTYPE.Special)
                        eventCards.Add(card);
                    else
                        player.addCardToHand(card);
                }
            }
        }

        #endregion

        /** BEGIN Command Pattern Methods **/
        #region CureStatus

        private static void GenerateCures()
        {
            CURESTATUS = new Cures(Cures.CURESTATE.NotCured);
        }

        public static Cures.CURESTATE GetCureStatus(COLOR color)
        {
            return CURESTATUS.GetCureStatus(color);
        }

        public static void SetCureStatus(COLOR color, Cures.CURESTATE curestate)
        {
            CURESTATUS.SetCureStatus(color, curestate);
        }
        #endregion

        #region Infection Cubes 

        private static void CreateInfectionCubes(int startingValue)
        {
            cubeCount = new InfectionCubesBoard(startingValue);
        }

        public static void IncrementInfectionCubes(COLOR color)
        {
            cubeCount.IncrementCubes(color);
        }

        public static void DecrementInfectionCubeCount(COLOR color)
        {
            cubeCount.DecrementCubeCount(color);
        }

        public static void AddInfectionCubes(COLOR color, int value)
        {
            cubeCount.AddCubes(color, value);
        }

        public static void SetInfectionCubeCount(COLOR color, int value)
        {
            cubeCount.SetCubeCount(color, value);
        }

        public static int GetInfectionCubeCount(COLOR color)
        {
            return cubeCount.GetCubeCount(color);
        }
        #endregion

        #region Infection and Outbreak 

        public static void IncrementOutbreakMarker()
        {
            GameBoardModels.outbreakMarker ++;
            if (GameBoardModels.outbreakMarker == 8)
            {
                throw new InvalidOperationException("Game Over");
            }
        }

        public static int GetOutbreakMarker()
        {
            return GameBoardModels.outbreakMarker;
        }

        public static void SetOutbreakMarker(int marker)
        {
            GameBoardModels.outbreakMarker = marker;
        }


        #endregion

        #region Players
        public static AbstractPlayer GetCurrentPlayer()
        {
            return players[CurrentPlayerIndex];
        }

        public static AbstractPlayer[] GetPlayers()
        {
            return players;
        }

        public static AbstractPlayer GetPlayerByIndex(int index)
        {
            return players[index];
        }

        public static int GetPlayerCount()
        {
            return players.Length;
        }


        public bool IncTurnCount()
        {
            if (currentPlayerTurnCounter >= GetCurrentPlayer().getMaxTurnCount() - 1)
            {
                //CurrentPlayerIndex = (CurrentPlayerIndex + 1) % players.Count();
                currentPlayerTurnCounter = 0;
                return true;
            }
            else
                currentPlayerTurnCounter++;
            return false;

            //currentPlayerTurnCounter++;
        }

        public static int GetCurrentPlayerIndex()
        {
            return GameBoardModels.CurrentPlayerIndex;
        }

        public static void MoveToNextPlayer()
        {
            GameBoardModels.CurrentPlayerIndex = (GameBoardModels.CurrentPlayerIndex  +1) % GameBoardModels.GetPlayerCount();
        }
        #endregion

        #region Decks 

        public static bool PlayerDiscardPileContains(string cityName)
        {
            return GameBoardModels.discardPlayerDeck.Count(c => c.CityName.Equals(cityName)) >0;
        }

        public static Stack<Card> getPlayerDeck()
        {
            return playerDeck;
        }

        public static Card ReclaminCityCardFromPlayerDeck(string cityName)
        {
            return GameBoardModels.discardPlayerDeck.First(c => c.CityName.Equals(cityName));
        }

        public static LinkedList<string> GetInfectionDeck()
        {
            return GameBoardModels.infectionDeck;
        }

        public static LinkedList<string> GetInfectionPile()
        {
            return GameBoardModels.infectionPile;
        }

        public static void RemoveFromInfectionPile(string cityName)
        {
            GameBoardModels.infectionPile.Remove(cityName);
        }

        public static void DiscardCard(Card card)
        {
            GameBoardModels.discardPlayerDeck.Push(card);
        }


        #endregion
        

        public static Card DrawCard()
        {
            try
            {
                return playerDeck.Pop();
            }
            catch (InvalidOperationException e)
            {
                throw new Exception("Game Over");
            }
        }

        public int GetPlayerDeckSize()
        {
            return playerDeck.Count();
        }
    }
}