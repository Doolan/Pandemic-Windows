using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SQADemicApp.BL;
using SQADemicApp.Objects;

namespace SQADemicApp
{
    public enum COLOR { red, black, blue, yellow }
    public class GameBoardModels
    {
        #region Public Static Vars
        public static InfectionCubes cubeCount;
        public static Cures CURESTATUS;
        public static List<String> citiesWithResearchStations;
        public static int outbreakMarker = 0;
        public static Player[] players;
        public static int CurrentPlayerIndex;
        public static List<Card> eventCards;
        public static LinkedList<String> infectionDeck;
        public static LinkedList<String> infectionPile;
        public static int InfectionRate;
        public static int InfectionRateIndex;
        #endregion

        #region Public Vars
        public int currentPlayerTurnCounter;
        #endregion

        #region private vars
        private static bool alreadySetUp = false;
        public static Stack<Card> playerDeck;
        private static int MAXCUBECOUNT = 24;
        #endregion

        /// <summary>
        /// Acts as a Main function. Sets up the game and the board state
        /// </summary>
        /// <param name="playersroles"></param>
        public GameBoardModels(string[] playersroles)
        {
            //Keep from making duplicates
            if (!alreadySetUp)
            {
                //set vars
                outbreakMarker = 0;
                cubeCount = new InfectionCubesBoard(MAXCUBECOUNT);
                CURESTATUS = new Cures();
              //  CURESTATUS.BlackCure = CURESTATUS.BlueCure = CURESTATUS.RedCure = CURESTATUS.YellowCure = Cures.CURESTATE.NotCured;
                Card[] playerDeckArray;
                List<String> infectionDeckList;
                Create.setUpCreate(out playerDeckArray, out infectionDeckList);
                playerDeck = new Stack<Card>(playerDeckArray);
                eventCards = new List<Card>();
                infectionPile = new LinkedList<String>();
                infectionDeck = new LinkedList<string>(Create.makeInfectionDeck(new StringReader(SQADemicApp.Properties.Resources.InfectionDeck)));
            }

            //Players setup allows existing players to be overwritten
            players = new Player[playersroles.Length];
            currentPlayerTurnCounter = 0;
            CurrentPlayerIndex = 0;
            for (int i = 0; i < playersroles.Count(); i++)
            {
                switch (playersroles[i])
                {
                    case "Dispatcher":
                        players[i] = new Player(ROLE.Dispatcher);
                        break;
                    case "Operations Expert":
                        players[i] = new Player(ROLE.OpExpert);
                        break;
                    case "Scientist":
                        players[i] = new Player(ROLE.Scientist);
                        break;
                    case "Medic":
                        players[i] = new Player(ROLE.Medic);
                        break;
                    case "Researcher":
                        players[i] = new Player(ROLE.Researcher);
                        break;
                    default:
                        players[i] = null;
                        break;
                }
            }
            InfectionRate = 2;
            InfectionRateIndex = 0;
            if (!alreadySetUp)
            {
                startGameInfection();
                setUpPlayerHands();
            }

            alreadySetUp = true;

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
            foreach (Player player in players)
            {
                for (int i = 0; i < cardsPerPlayer; i++)
                {
                    Card card = drawCard();
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
                        player.hand.Add(card);
                }
            }
        }

        public bool incTurnCount()
        {
            if (currentPlayerTurnCounter == 3)
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

        public static Card drawCard()
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

        public int playerDeckSize()
        {
            return playerDeck.Count();
        }
    }
}