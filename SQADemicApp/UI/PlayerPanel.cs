using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQADemicApp.BL;

namespace SQADemicApp
{
    public partial class PlayerPanel : Form
    {
        private GameBoard board;
        public static bool quietNight = false;
        public PlayerPanel(GameBoard board)
        {
            this.board = board;
            InitializeComponent();
        }

        private void MoveButton_Click(object sender, EventArgs e)
        {
            GameBoard.CurrentState = GameBoard.STATE.Move;
        }

        private void CureCityButton_Click(object sender, EventArgs e)
        {
            TreatDiseaseForm TDForm = new TreatDiseaseForm(board);
            TDForm.Show();
        }

        private void AAButton_Click(object sender, EventArgs e)
        {
            AdvancedActions AAForm = new AdvancedActions(board);

            // TODO check and get list of actions
            String[] buttonLabels = { "button test 1", "button test 2" };
            AddAdvancedActionButtonsForRole(AAForm, buttonLabels);

            AAForm.Show();
        }

        private void AddAdvancedActionButtonsForRole(AdvancedActions AAForm, String[] buttonLabels)
        {
            List<Button> buttons = new List<Button>();
            foreach (String s in buttonLabels)
            {
                Button newButton = new Button();
                newButton.Text = s;
                buttons.Add(newButton);
                AAForm.ButtonPanel.Controls.Add(newButton);
            }
        }

        private void DispatcherMove_Click(object sender, EventArgs e)
        {
            GameBoard.CurrentState = SQADemicApp.GameBoard.STATE.Dispatcher;
        }

        private void EndSequenceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (GameBoard.turnpart == GameBoard.TURNPART.Draw)
                    drawcards();
                else if (GameBoard.turnpart == GameBoard.TURNPART.Infect)
                    infectCities();
                board.UpdatePlayerForm();
            }
            catch (InvalidOperationException exc)
            {
                //END OF GAME STUFF
                MessageBox.Show("You Lost. That must suck...");
                MessageBox.Show("Have a great rest of your day");
            }
        }

        private void infectCities()
        {
            if (!quietNight)
            {
                List<string> infectedcites = InfectorBL.InfectCities(GameBoardModels.GetInfectionDeck(), GameBoardModels.GetInfectionPile(), GameBoardModels.InfectionRate);
                InfectorBL.InfectCities(infectedcites);
            }
            else
                quietNight = true;
            GameBoard.turnpart = GameBoard.TURNPART.Action;
            GameBoardModels.MoveToNextPlayer();
            board.UpdateCityButtons(false);
        }

        private void drawcards()
        {
            //Draw Two cards
            Card drawCard1 = GameBoardModels.DrawCard();
            Card drawCard2 = GameBoardModels.DrawCard();

            //Epidemic code
            if (drawCard1.CardType.Equals(Card.CARDTYPE.EPIDEMIC))
            {
                string infectcityname = InfectorBL.Epidemic(GameBoardModels.GetInfectionDeck(), GameBoardModels.GetInfectionPile(), ref GameBoardModels.InfectionRateIndex, ref GameBoardModels.InfectionRate);
                new PicForm(false, infectcityname).Show();
                for (int i = 0; i < 3; i++)
                {
                    InfectorBL.InfectCities(new List<string> { infectcityname });
                }
            }
            else if (drawCard1.CardType == Card.CARDTYPE.Special)
                GameBoardModels.eventCards.Add(drawCard1);
            else
                GameBoardModels.GetCurrentPlayer().addCardToHand(drawCard1);
                

            if (drawCard2.CardType.Equals(Card.CARDTYPE.EPIDEMIC))
            {
                string infectcityname = InfectorBL.Epidemic(GameBoardModels.GetInfectionDeck(), GameBoardModels.GetInfectionPile(), ref GameBoardModels.InfectionRateIndex, ref GameBoardModels.InfectionRate);
                new PicForm(false, infectcityname).Show();
                for (int i = 0; i < 3; i++)
                {
                    InfectorBL.InfectCities(new List<string> { infectcityname });
                }
            }
            else if (drawCard2.CardType == Card.CARDTYPE.Special)
                GameBoardModels.eventCards.Add(drawCard2);
            else
                GameBoardModels.GetCurrentPlayer().addCardToHand(drawCard2);

            //Move to infection phase
            if (!quietNight)
                GameBoard.turnpart = GameBoard.TURNPART.Infect;
            else
            {
                quietNight = false;
                GameBoard.turnpart = GameBoard.TURNPART.Action;
                GameBoardModels.MoveToNextPlayer();
            }
        }

        public void AddDispatcherButton()
        {
            this.DispatcherMove.Show();
            this.AAButton.Location = new System.Drawing.Point(159, 82);
        }

        public void RemoveDispatcherButton()
        {
            this.DispatcherMove.Hide();
            this.AAButton.Location = new System.Drawing.Point(91, 81);
        }

        public void ShowDrawButton()
        {
            this.EndSequenceBtn.Text = "Draw Cards";
            this.EndSequenceBtn.Show();
        }

        public void ShowInfectButton()
        {
            this.EndSequenceBtn.Text = "Infect";
            this.EndSequenceBtn.Show();
        }

        public void HideDrawInfectButton()
        {
            this.EndSequenceBtn.Hide();
        }

        public void UpdateTurnProgressBoard(int turnCount, int maxTurns)
        {
            this.MoveProgressBar.Value = 100 * turnCount / maxTurns;
            this.MoveProgressBarLabel.Text = this.MoveProgressBarLabel.Text.Substring(0, this.MoveProgressBarLabel.Text.Length - 3) +
                                     turnCount + "/" + maxTurns;
        }

        public void UpdatePlayerHand(Object[] playerHandArray)
        {
            this.listBox1.Items.Clear();
            this.listBox1.Items.AddRange(playerHandArray);
        }

        public void updateCubeCounts(int redCubeCount, int blueCubeCount, int blackCubeCount, int YellowCubeCount)
        {
            this.RedCubes.Text = String.Format("Red Cubes Remaining:    {0,-2}/24", redCubeCount);
            this.BlueCubes.Text = String.Format("Blue Cubes Remaining:   {0,-2}/24", blueCubeCount);
            this.BlackCubes.Text = String.Format("Black Cubes Remaining:  {0,-2}/24", blackCubeCount);
            this.YellowCubes.Text = String.Format("Yellow Cubes Remaining: {0,-2}/24", YellowCubeCount);
        }

        public void updateCounters(int infectionRate, int outbreakMarker)
        {
            this.InfectionRate.Text = string.Format("Infection Rate: {0}", infectionRate);
            this.OutbreakCount.Text = string.Format("Outbreak Count: {0}", outbreakMarker);
        }

        public void updateCureStatus(String redCureStatus, String blueCureStatus, String blackCureStatus, String yellowCureStatus)
        {
            // set value of cure label to status in game board
            // if status is NotCured, change to No Cure for nicer appearance
            this.RedCure.Text = String.Format("Red:  {0}", redCureStatus.Replace("NotCured", "No Cure"));
            this.BlueCure.Text = String.Format("Blue: {0}", blueCureStatus.Replace("NotCured", "No Cure"));
            this.BlackCure.Text = String.Format("Black:  {0}", blackCureStatus.Replace("NotCured", "No Cure"));
            this.YellowCure.Text = String.Format("Yellow: {0}", yellowCureStatus.Replace("NotCured", "No Cure"));
        }

    }
}
