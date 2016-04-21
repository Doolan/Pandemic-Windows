using System;
using System.Windows.Forms;
using SQADemicApp.BL;
using System.Collections.Generic;
using SQADemicApp.Players;

namespace SQADemicApp
{
    public partial class GameBoard : Form
    {
        public GameBoardModels boardModel;
        CharacterPane form2;
        PlayerPanel playerForm;
        EventCardForm ECForm;
        public enum STATE { Dispatcher, Initializing, Move, Cure, Default, Airlift, GovGrant}
        public static STATE CurrentState;
        public enum TURNPART { Action, Draw, Infect };
        public static TURNPART turnpart;
        public static int dispatcherMoveIndex;
        

        public GameBoard()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            CurrentState = STATE.Initializing;
            string[] rolesDefault = { "Dispatcher", "Scientist" };
            boardModel = new GameBoardModels(rolesDefault);
            playerForm = new PlayerPanel(this);
            form2 = new CharacterPane(rolesDefault);
            ECForm = new EventCardForm();
            InitializeComponent();
            ECForm.Show();
            form2.Show();
            playerForm.Show();
            UpdatePlayerForm();
            UpdateCityButtons(true);
            

            CurrentState = STATE.Default;
            turnpart = TURNPART.Action;
        }
        public GameBoard(string[] playerRoles)
        {

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            boardModel = new GameBoardModels(playerRoles);
            playerForm = new PlayerPanel(this);
            form2 = new CharacterPane(playerRoles);
            ECForm = new EventCardForm();
            InitializeComponent();
            ECForm.Show();
            form2.Show();
            playerForm.Show();
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
                        switch (dispatcherMoveIndex)
                        {
                            case 3:
                                form2.Player4.Text = "Player 4\n" + GameBoardModels.GetPlayerByIndex(3) + "\n" + cityName;
                                break;
                            case 2:
                                form2.Player3.Text = "Player 3\n" + GameBoardModels.GetPlayerByIndex(2) + "\n" + cityName;
                                break;
                            case 1:
                                form2.Player2.Text = "Player 2\n" + GameBoardModels.GetPlayerByIndex(1) + "\n" + cityName;
                                break;
                            default:
                                form2.Player1.Text = "Player 1\n" + GameBoardModels.GetPlayerByIndex(0) + "\n" + cityName;
                                break;
                        }
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
                        switch (GameBoardModels.CurrentPlayerIndex)
                        {
                            case 3:
                                form2.Player4.Text = "Player 4\n" + GameBoardModels.GetPlayerByIndex(3) + "\n" + cityName;
                                break;
                            case 2:
                                form2.Player3.Text = "Player 3\n" + GameBoardModels.GetPlayerByIndex(2) + "\n" + cityName;
                                break;
                            case 1:
                                form2.Player2.Text = "Player 2\n" + GameBoardModels.GetPlayerByIndex(1) + "\n" + cityName;
                                break;
                            default:
                                form2.Player1.Text = "Player 1\n" + GameBoardModels.GetPlayerByIndex(0) + "\n" + cityName;
                                break;
                        }
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
            playerForm.UpdateTurnProgressBoard(boardModel.currentPlayerTurnCounter, GameBoardModels.GetCurrentPlayer().getMaxTurnCount());
            playerForm.UpdatePlayerHand(GameBoardModels.GetCurrentPlayer().HandStringList().ToArray());
            if (GameBoardModels.GetCurrentPlayer().GetType() == typeof(DispatcherPlayer))
            {
                playerForm.AddDispatcherButton();
            }
            else
            {
                playerForm.RemoveDispatcherButton();
            }
            if (turnpart == TURNPART.Draw)
            {
                playerForm.ShowDrawButton();
            }
            else if (turnpart == TURNPART.Infect)
            {
                playerForm.ShowInfectButton();
            }
            else
            {
                playerForm.HideDrawInfectButton();
            }
            updateCharacterForm(GameBoardModels.CurrentPlayerIndex);
            playerForm.updateCubeCounts(GameBoardModels.GetInfectionCubeCount(COLOR.red), GameBoardModels.GetInfectionCubeCount(COLOR.blue),
                GameBoardModels.GetInfectionCubeCount(COLOR.black), GameBoardModels.GetInfectionCubeCount(COLOR.yellow));
            playerForm.updateCounters(GameBoardModels.InfectionRate, GameBoardModels.outbreakMarker);
            playerForm.updateCureStatus(GameBoardModels.GetCureStatus(COLOR.red).ToString(), GameBoardModels.GetCureStatus(COLOR.blue).ToString(),
                GameBoardModels.GetCureStatus(COLOR.black).ToString(), GameBoardModels.GetCureStatus(COLOR.yellow).ToString());
            ECForm.UpdateEventCards();
        }

        private void updateCharacterForm(int p)
        {
            switch (p)
            {
                case 3:
                    form2.Player4.UseVisualStyleBackColor = false;
                    form2.Player3.UseVisualStyleBackColor = true;
                    break;
                case 2:
                    form2.Player3.UseVisualStyleBackColor = false;
                    form2.Player2.UseVisualStyleBackColor = true;
                    break;
                case 1:
                    form2.Player2.UseVisualStyleBackColor = false;
                    form2.Player1.UseVisualStyleBackColor = true;
                    break;
                default:
                    form2.Player1.UseVisualStyleBackColor = false;
                    form2.Player4.UseVisualStyleBackColor = form2.Player3.UseVisualStyleBackColor = form2.Player2.UseVisualStyleBackColor = true;
                    break;
            }
            switch(GameBoardModels.GetPlayerCount())
            {
                case 4:
                    form2.Player4.Text = "Player 4\n" + GameBoardModels.GetPlayerByIndex(3) + "\n" + GameBoardModels.GetPlayerByIndex(3).currentCity.Name;
                    goto case 3;
                case 3:
                    form2.Player3.Text = "Player 3\n" + GameBoardModels.GetPlayerByIndex(2) + "\n" + GameBoardModels.GetPlayerByIndex(2).currentCity.Name;
                    goto case 2;
                case 2:
                    form2.Player1.Text = "Player 1\n" + GameBoardModels.GetPlayerByIndex(0) + "\n" + GameBoardModels.GetPlayerByIndex(0).currentCity.Name;
                    form2.Player2.Text = "Player 2\n" + GameBoardModels.GetPlayerByIndex(1) + "\n" + GameBoardModels.GetPlayerByIndex(1).currentCity.Name;
                    break;       
            }
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
    }
}
