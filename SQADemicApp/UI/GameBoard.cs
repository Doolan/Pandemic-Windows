using System;
using System.Windows.Forms;
using SQADemicApp.BL;
using System.Collections.Generic;

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
        //    Card drawnCard = boardModel.drawCard();
        //    GameBoardModels.players[GameBoardModels.CurrentPlayerIndex].hand.Add(drawnCard);
        //    button49.Text = String.Format("Draw\n{0}", boardModel.playerDeckSize());
        //    UpdatePlayerForm();
        //}

        private void City_Click(object sender, EventArgs e)
        {
            Button pressed = sender as Button;
            var cityName = pressed.Text.Substring(3);
            switch (CurrentState)
            {
                case STATE.Airlift:
                    SpecialEventCardsBL.Airlift(GameBoardModels.players[GameBoardModels.CurrentPlayerIndex], Create.cityDictionary[cityName]);
                    break;
                case STATE.GovGrant:
                    SpecialEventCardsBL.GovernmentGrant(cityName);
                    break;
                case STATE.Dispatcher:
                    if (PlayerActionsBL.DispatcherMovePlayer(GameBoardModels.players[dispatcherMoveIndex],new List<Player>(GameBoardModels.players), Create.cityDictionary[cityName]))
                    {
                        switch (dispatcherMoveIndex)
                        {
                            case 3:
                                form2.Player4.Text = "Player 4\n" + GameBoardModels.players[3].role.ToString() + "\n" + cityName;
                                break;
                            case 2:
                                form2.Player3.Text = "Player 3\n" + GameBoardModels.players[2].role.ToString() + "\n" + cityName;
                                break;
                            case 1:
                                form2.Player2.Text = "Player 2\n" + GameBoardModels.players[1].role.ToString() + "\n" + cityName;
                                break;
                            default:
                                form2.Player1.Text = "Player 1\n" + GameBoardModels.players[0].role.ToString() + "\n" + cityName;
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
                    if (PlayerActionsBL.moveplayer(GameBoardModels.players[GameBoardModels.CurrentPlayerIndex], Create.cityDictionary[cityName]))
                    {
                        switch (GameBoardModels.CurrentPlayerIndex)
                        {
                            case 3:
                                form2.Player4.Text = "Player 4\n" + GameBoardModels.players[3].role.ToString() + "\n" + cityName;
                                break;
                            case 2:
                                form2.Player3.Text = "Player 3\n" + GameBoardModels.players[2].role.ToString() + "\n" + cityName;
                                break;
                            case 1:
                                form2.Player2.Text = "Player 2\n" + GameBoardModels.players[1].role.ToString() + "\n" + cityName;
                                break;
                            default:
                                form2.Player1.Text = "Player 1\n" + GameBoardModels.players[0].role.ToString() + "\n" + cityName;
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
            playerForm.progressBar1.Value = 100 * (boardModel.currentPlayerTurnCounter) / 4;
            playerForm.label1.Text = playerForm.label1.Text.Substring(0, playerForm.label1.Text.Length - 3) + (Convert.ToInt32(boardModel.currentPlayerTurnCounter)) + "/" + 4;
            playerForm.listBox1.Items.Clear();
            playerForm.listBox1.Items.AddRange(GameBoardModels.players[GameBoardModels.CurrentPlayerIndex].handStringList().ToArray());
            if (GameBoardModels.players[GameBoardModels.CurrentPlayerIndex].role == ROLE.Dispatcher)
            {
                playerForm.DispatcherMove.Show();
                playerForm.AAButton.Location = new System.Drawing.Point(159, 82);
            }
            else
            {
                playerForm.DispatcherMove.Hide();
                playerForm.AAButton.Location = new System.Drawing.Point(91, 81);
            }
            if (turnpart == TURNPART.Draw)
            {
                playerForm.EndSequenceBtn.Text = "Draw Cards";
                playerForm.EndSequenceBtn.Show();
            }
            else if (turnpart == TURNPART.Infect)
            {
                playerForm.EndSequenceBtn.Text = "Infect";
                playerForm.EndSequenceBtn.Show();
            }
            else
            {
                playerForm.EndSequenceBtn.Hide();
            }
            updateCharacterForm(GameBoardModels.CurrentPlayerIndex);
            updateCubeCounts();
            updateCounters();
            updateCureStatus();
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
            switch(GameBoardModels.players.Length)
            {
                case 4:
                    form2.Player4.Text = "Player 4\n" + GameBoardModels.players[3].role.ToString() + "\n" + GameBoardModels.players[3].currentCity.Name;
                    goto case 3;
                case 3:
                    form2.Player3.Text = "Player 3\n" + GameBoardModels.players[2].role.ToString() + "\n" + GameBoardModels.players[2].currentCity.Name;
                    goto case 2;
                case 2:
                    form2.Player1.Text = "Player 1\n" + GameBoardModels.players[0].role.ToString() + "\n" + GameBoardModels.players[0].currentCity.Name;
                    form2.Player2.Text = "Player 2\n" + GameBoardModels.players[1].role.ToString() + "\n" + GameBoardModels.players[1].currentCity.Name;
                    break;       
            }
        }
        private void updateCubeCounts()
        {
            playerForm.RedCubes.Text = String.Format("Red Cubes Remaining:    {0,-2}/24", GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(COLOR.red));
            playerForm.BlueCubes.Text = String.Format("Blue Cubes Remaining:   {0,-2}/24", GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(COLOR.blue));
            playerForm.BlackCubes.Text = String.Format("Black Cubes Remaining:  {0,-2}/24", GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(COLOR.black));
            playerForm.YellowCubes.Text = String.Format("Yellow Cubes Remaining: {0,-2}/24", GameBoardModels.cubeCount.InfectionCubes.GetCubeCount(COLOR.yellow));
        }
        private void updateCounters()
        {
            playerForm.InfectionRate.Text = string.Format("Infection Rate: {0}", GameBoardModels.InfectionRate);
            playerForm.OutbreakCount.Text = string.Format("Outbreak Count: {0}", GameBoardModels.outbreakMarker);
        }
        private void updateCureStatus()
        {
            // set value of cure label to status in game board
            // if status is NotCured, change to No Cure for nicer appearance
            playerForm.RedCure.Text = String.Format(   "Red:  {0}", GameBoardModels.CURESTATUS.RedCure.ToString().Replace("NotCured", "No Cure"));
            playerForm.BlueCure.Text = String.Format(  "Blue: {0}", GameBoardModels.CURESTATUS.BlueCure.ToString().Replace("NotCured", "No Cure"));
            playerForm.BlackCure.Text = String.Format( "Black:  {0}", GameBoardModels.CURESTATUS.BlackCure.ToString().Replace("NotCured", "No Cure"));
            playerForm.YellowCure.Text = String.Format("Yellow: {0}", GameBoardModels.CURESTATUS.YellowCure.ToString().Replace("NotCured", "No Cure"));
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
