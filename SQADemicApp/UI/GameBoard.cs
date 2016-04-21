using System;
using System.Windows.Forms;
using SQADemicApp.BL;
using System.Collections.Generic;
using SQADemicApp.Players;

namespace SQADemicApp
{
    public partial class GameBoard : Form
    {
        private GameBoardModels boardModel;

        public enum STATE { Dispatcher, Initializing, Move, Cure, Default, Airlift, GovGrant}
        public static STATE CurrentState;
        public enum TURNPART { Action, Draw, Infect };
        public static TURNPART turnpart;

        private static int dispatcherMoveIndex;
        private CharacterPane characterPane;
        private PlayerPanel playerPanel;
        private EventCardForm ECForm;
        
        public GameBoard()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            CurrentState = STATE.Initializing;
            string[] rolesDefault = { "Dispatcher", "Scientist" };
            boardModel = new GameBoardModels(rolesDefault);
            playerPanel = new PlayerPanel(this);
            characterPane = new CharacterPane(rolesDefault);
            ECForm = new EventCardForm();
            InitializeComponent();
            ECForm.Show();
            characterPane.Show();
            playerPanel.Show();
            UpdatePlayerForm();
            UpdateCityButtons(true);
            

            CurrentState = STATE.Default;
            turnpart = TURNPART.Action;
        }
        public GameBoard(string[] playerRoles)
        {

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            boardModel = new GameBoardModels(playerRoles);
            playerPanel = new PlayerPanel(this);
            characterPane = new CharacterPane(playerRoles);
            ECForm = new EventCardForm();
            InitializeComponent();
            ECForm.Show();
            characterPane.Show();
            playerPanel.Show();
            UpdatePlayerForm();
            UpdateCityButtons(true);
            CurrentState = STATE.Default;
            turnpart = TURNPART.Action; 
        }

        //private void DrawBtn_Click(object sender, EventArgs e)
        //{
        //    Card drawnCard = boardModel.DrawCard();
        //    GameBoardModels.GetCurrentPlayer().hand.Add(drawnCard);
        //    button49.Text = String.Format("Draw\n{0}", boardModel.GetPlayerDeckSize());
        //    UpdatePlayerForm();
        //}

        private void City_Click(object sender, EventArgs e)
        {
            Button pressed = sender as Button;
            var cityName = pressed.Text.Substring(3);
            switch (CurrentState)
            {
                case STATE.Airlift:
                    SpecialEventCardsBL.Airlift(GameBoardModels.GetCurrentPlayer(), Create.cityDictionary[cityName]);
                    break;
                case STATE.GovGrant:
                    SpecialEventCardsBL.GovernmentGrant(cityName);
                    break;
                case STATE.Dispatcher:
                    if (GameBoardModels.GetPlayerByIndex(dispatcherMoveIndex).DispatcherMovePlayer(new List<AbstractPlayer>(GameBoardModels.GetPlayers()), Create.cityDictionary[cityName]))
                    {
                        characterPane.updatePlayerCity(dispatcherMoveIndex, cityName);
                        if (boardModel.incTurnCount())
                            turnpart = TURNPART.Draw;
                        UpdatePlayerForm();
                        UpdateCityButtons(false);
                    }
                    else
                    {
                        MessageBox.Show("Invalid City", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case STATE.Move:
                    if (GameBoardModels.GetCurrentPlayer().MovePlayer(Create.cityDictionary[cityName]))
                    {
                        characterPane.updatePlayerCity(GameBoardModels.CurrentPlayerIndex, cityName);
                        bool endofturn = boardModel.incTurnCount();
                        if (endofturn)
                            turnpart = TURNPART.Draw;
                        UpdatePlayerForm();
                        UpdateCityButtons(false);
                    }
                    else
                    {
                        MessageBox.Show("Invalid City", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                default:
                    CityPageForm CPForm = new CityPageForm(Create.cityDictionary[cityName]);
                    CPForm.Show();
                    break;
            }
            CurrentState = STATE.Default;
        }
        public void UpdatePlayerForm()
        {
            playerPanel.UpdateTurnProgressBoard(boardModel.currentPlayerTurnCounter, GameBoardModels.GetCurrentPlayer().getMaxTurnCount());
            playerPanel.UpdatePlayerHand(GameBoardModels.GetCurrentPlayer().HandStringList().ToArray());
            if (GameBoardModels.GetCurrentPlayer().GetType() == typeof(DispatcherPlayer))
            {
                playerPanel.AddDispatcherButton();
            }
            else
            {
                playerPanel.RemoveDispatcherButton();
            }
            if (turnpart == TURNPART.Draw)
            {
                playerPanel.ShowDrawButton();
            }
            else if (turnpart == TURNPART.Infect)
            {
                playerPanel.ShowInfectButton();
            }
            else
            {
                playerPanel.HideDrawInfectButton();
            }
            characterPane.updateCurrentPlayer(GameBoardModels.CurrentPlayerIndex);
            characterPane.updatePlayerCount(GameBoardModels.GetPlayerCount());

            playerPanel.updateCubeCounts(GameBoardModels.GetInfectionCubeCount(COLOR.red), GameBoardModels.GetInfectionCubeCount(COLOR.blue),
                GameBoardModels.GetInfectionCubeCount(COLOR.black), GameBoardModels.GetInfectionCubeCount(COLOR.yellow));
            playerPanel.updateCounters(GameBoardModels.InfectionRate, GameBoardModels.outbreakMarker);
            playerPanel.updateCureStatus(GameBoardModels.GetCureStatus(COLOR.red).ToString(), GameBoardModels.GetCureStatus(COLOR.blue).ToString(),
                GameBoardModels.GetCureStatus(COLOR.black).ToString(), GameBoardModels.GetCureStatus(COLOR.yellow).ToString());
            ECForm.UpdateEventCards();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DiscardPile dp = new DiscardPile(false);
            dp.Show();
        }
        public void UpdateCityButtons(bool firstRun)
        {
            foreach (var control in this.Controls)
            {
                if(control is Button)
                {
                    var button = control as Button;
                    string cityName = button.Text.Substring(3);
                    try
                    {
                        var city = Create.cityDictionary[cityName];
                        button.Text = String.Format("{0,2} " + city.Name, city.allCubeCount());
                        if(firstRun)
                            button.Font = new System.Drawing.Font(button.Font.FontFamily, 5);
                    }
                    catch(KeyNotFoundException)
                    {
                        // not a button that needs to be updated
                    }
                }
            }
        }

        public static void SetDispatcherMoveIndex(int index)
        {
            dispatcherMoveIndex = index;
        }

        public bool IncrementTurnCount()
        {
            return boardModel.incTurnCount();
        }
    }
}
