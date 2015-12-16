﻿using SQADemicApp.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQADemicApp
{
    public enum Color { Red, Black, Blue, Yellow }

    public class GameBoardModels
    {
        #region Public Static Vars

        public static List<string> CitiesWithResearchStations;
        public static InfectionCubeCount CubeCount;
        public static Cures Curestatus;
        public static int CurrentPlayerIndex;
        public static List<Card> EventCards;
        public static LinkedList<string> InfectionDeck;
        public static LinkedList<string> InfectionPile;
        public static int InfectionRate;
        public static int InfectionRateIndex;
        public static int OutbreakMarker = 0;
        public static Player[] Players;
        #endregion Public Static Vars

        #region Public Vars

        public int CurrentPlayerTurnCounter;

        #endregion Public Vars

        #region private vars

        public static Stack<Card> PlayerDeck;
        private static bool _alreadySetUp = false;
        #endregion private vars

        /// <summary>
        /// Acts as a Main function. Sets up the game and the board state
        /// </summary>
        /// <param name="playersroles"></param>
        public GameBoardModels(string[] playersroles)
        {
            //Keep from making duplicates (this is for testing porposes)
            if (!_alreadySetUp)
            {
                //set vars
                OutbreakMarker = 0;
                CubeCount = new InfectionCubeCount();
                CubeCount.BlackCubes = CubeCount.RedCubes = CubeCount.BlueCubes = CubeCount.YellowCubes = 24;
                Curestatus = new Cures();
                Curestatus.BlackCure = Curestatus.BlueCure = Curestatus.RedCure = Curestatus.YellowCure = Cures.Curestate.NotCured;
                Card[] playerDeckArray;
                List<string> infectionDeckList;
                Create.SetUpCreate(out playerDeckArray, out infectionDeckList);
                PlayerDeck = new Stack<Card>(playerDeckArray);
                EventCards = new List<Card>();
                InfectionPile = new LinkedList<string>();
                InfectionDeck = new LinkedList<string>(Create.MakeInfectionDeck(new StringReader(SQADemicApp.Properties.Resources.InfectionDeck)));
            }

            //Players setup allows existing players to be overwritten
            Players = new Player[playersroles.Length];
            CurrentPlayerTurnCounter = 0;
            CurrentPlayerIndex = 0;
            for (int i = 0; i < playersroles.Count(); i++)
            {
                switch (playersroles[i])
                {
                    case "Dispatcher":
                        Players[i] = new Player(Role.Dispatcher);
                        break;

                    case "Operations Expert":
                        Players[i] = new Player(Role.OpExpert);
                        break;

                    case "Scientist":
                        Players[i] = new Player(Role.Scientist);
                        break;

                    case "Medic":
                        Players[i] = new Player(Role.Medic);
                        break;

                    case "Researcher":
                        Players[i] = new Player(Role.Researcher);
                        break;

                    default:
                        Players[i] = null;
                        break;
                }
            }
            InfectionRate = 2;
            InfectionRateIndex = 0;
            if (!_alreadySetUp)
            {
                StartGameInfection();
                SetUpPlayerHands();
            }

            _alreadySetUp = true;
        }

        public static Card DrawCard()
        {
            try
            {
                return PlayerDeck.Pop();
            }
            catch (InvalidOperationException e)
            {
                throw new Exception("Game Over");
            }
        }

        public bool IncTurnCount()
        {
            if (CurrentPlayerTurnCounter == 3)
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

        public int PlayerDeckSize()
        {
            return PlayerDeck.Count();
        }

        private static void SetUpPlayerHands()
        {
            int cardsPerPlayer = Players.Count() == 4 ? 2 : Players.Count() == 3 ? 3 : 4;
            foreach (Player player in Players)
            {
                for (int i = 0; i < cardsPerPlayer; i++)
                {
                    Card card = DrawCard();
                    if (card.CardType.Equals(Card.Cardtype.Epidemic))
                    {
                        string infectcityname = InfectorBl.Epidemic(GameBoardModels.InfectionDeck, GameBoardModels.InfectionPile, ref GameBoardModels.InfectionRateIndex, ref GameBoardModels.InfectionRate);
                        new PicForm(false, infectcityname).Show();
                        for (int j = 0; j < 3; j++)
                        {
                            InfectorBl.InfectCities(new List<string> { infectcityname });
                        }
                    }
                    else if (card.CardType == Card.Cardtype.Special)
                        EventCards.Add(card);
                    else
                        player.Hand.Add(card);
                }
            }
        }

        private void StartGameInfection()
        {
            for (int i = 3; i > 0; i--)
            {
                List<string> infectedcites = InfectorBl.InfectCities(InfectionDeck, InfectionPile, 3);
                for (int j = 0; j < i; j++)
                {
                    InfectorBl.InfectCities(infectedcites);
                }
            }
        }
        #region Storage Classes

        public class Cures
        {
            public enum Curestate { NotCured, Cured, Sunset }

            public Curestate BlackCure { get; set; }
            public Curestate BlueCure { get; set; }
            public Curestate RedCure { get; set; }
            public Curestate YellowCure { get; set; }

            public Curestate GetCureStatus(Color color)
            {
                switch (color)
                {
                    case Color.Red:
                        return RedCure;

                    case Color.Blue:
                        return BlueCure;

                    case Color.Yellow:
                        return YellowCure;

                    case Color.Black:
                        return BlackCure;

                    default:
                        throw new ArgumentException("Not a vaild color");
                }
            }

            public void SetCureStatus(Color color, Curestate curestate)
            {
                switch (color)
                {
                    case Color.Red:
                        RedCure = curestate;
                        break;

                    case Color.Blue:
                        BlueCure = curestate;
                        break;

                    case Color.Yellow:
                        YellowCure = curestate;
                        break;

                    case Color.Black:
                        BlackCure = curestate;
                        break;

                    default:
                        throw new ArgumentException("Not a vaild color");
                }
            }
        }

        public class InfectionCubeCount
        {
            public int BlackCubes { get; set; }
            public int BlueCubes { get; set; }
            public int RedCubes { get; set; }
            public int YellowCubes { get; set; }
        }
        #endregion Storage Classes
    }
}