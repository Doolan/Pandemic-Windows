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
        private readonly GameBoard _board;
        public static bool QuietNight = false;
        public PlayerPanel(GameBoard board)
        {
            this._board = board;
            InitializeComponent();
        }

        private void MoveButton_Click(object sender, EventArgs e)
        {
            GameBoard.CurrentState = GameBoard.State.Move;
        }

        private void CureCityButton_Click(object sender, EventArgs e)
        {
            var tdForm = new TreatDiseaseForm(_board);
            tdForm.Show();
        }

        private void AAButton_Click(object sender, EventArgs e)
        {
            var aaForm = new AdvancedActions(_board);    
            
            AddAdvancedActionButtonsForRole(aaForm);

            aaForm.ShowDialog();
        }

        private void AddAdvancedActionButtonsForRole(AdvancedActions aaForm)
        {
            var buttons = new List<Button>();
            foreach (var action in GameBoardModels.GetCurrentPlayer().GetSpecialActions())
            {
                var newButton = new Button {Text = action};

                //Small note: I have no idea why this syntax works
                newButton.Click += (s, e) => { GameBoardModels.GetCurrentPlayer().PreformSpecialAction(action, _board); newButton.Enabled = false; };
                newButton.AutoSize = true;
                buttons.Add(newButton);
                aaForm.ButtonPanel.Controls.Add(newButton);
            }
        }

        private void DispatcherMove_Click(object sender, EventArgs e)
        {
            GameBoard.CurrentState = SQADemicApp.GameBoard.State.Dispatcher;
        }

        private void EndSequenceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (GameBoard.CurrentTurnPart == GameBoard.Turnpart.Draw)
                {
                    Drawcards();
                    if (GameBoardModels.GetCurrentPlayer().GetMaxHandSize() < GameBoardModels.GetCurrentPlayer().Hand.Count)
                    {
                        new UI.DiscardExtraCards(_board).ShowDialog();
                    }
                }
                else if (GameBoard.CurrentTurnPart == GameBoard.Turnpart.Infect)
                    InfectCities();
                _board.UpdatePlayerForm();
            }
            catch (InvalidOperationException)
            {
                //END OF GAME STUFF
                MessageBox.Show("You Lost. That must suck...");
                MessageBox.Show("Have a great rest of your day");
            }
        }

        private void InfectCities()
        {
            if (!QuietNight)
            {
                var infectedcites = InfectorBL.InfectCities(GameBoardModels.GetInfectionDeck(), GameBoardModels.GetInfectionPile(), GameBoardModels.InfectionRate);
                InfectorBL.InfectCities(infectedcites);
            }
            else
                QuietNight = true;
            GameBoard.CurrentTurnPart = GameBoard.Turnpart.Action;
            GameBoardModels.MoveToNextPlayer();
            _board.UpdateCityButtons(false);
        }

        private static void Drawcards()
        {
            //Draw Two cards
            var drawCard1 = GameBoardModels.DrawCard();
            var drawCard2 = GameBoardModels.DrawCard();

            //Epidemic code
            if (drawCard1.CardType.Equals(Card.Cardtype.Epidemic))
            {
                var infectcityname = InfectorBL.Epidemic(GameBoardModels.GetInfectionDeck(), GameBoardModels.GetInfectionPile(), ref GameBoardModels.InfectionRateIndex, ref GameBoardModels.InfectionRate);
                new PicForm(false, infectcityname).Show();
                for (var i = 0; i < 3; i++)
                {
                    InfectorBL.InfectCities(new List<string> { infectcityname });
                }
            }
            else if (drawCard1.CardType == Card.Cardtype.Special)
                GameBoardModels.EventCards.Add(drawCard1);
            else
                GameBoardModels.GetCurrentPlayer().AddCardToHand(drawCard1);
                

            if (drawCard2.CardType.Equals(Card.Cardtype.Epidemic))
            {
                var infectcityname = InfectorBL.Epidemic(GameBoardModels.GetInfectionDeck(), GameBoardModels.GetInfectionPile(), ref GameBoardModels.InfectionRateIndex, ref GameBoardModels.InfectionRate);
                new PicForm(false, infectcityname).Show();
                for (var i = 0; i < 3; i++)
                {
                    InfectorBL.InfectCities(new List<string> { infectcityname });
                }
            }
            else if (drawCard2.CardType == Card.Cardtype.Special)
                GameBoardModels.EventCards.Add(drawCard2);
            else
                GameBoardModels.GetCurrentPlayer().AddCardToHand(drawCard2);

            //Move to infection phase
            if (!QuietNight)
                GameBoard.CurrentTurnPart = GameBoard.Turnpart.Infect;
            else
            {
                QuietNight = false;
                GameBoard.CurrentTurnPart = GameBoard.Turnpart.Action;
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

        public void UpdatePlayerHand(object[] playerHandArray)
        {
            this.listBox1.Items.Clear();
            this.listBox1.Items.AddRange(playerHandArray);
        }

        public void UpdateCubeCounts(int redCubeCount, int blueCubeCount, int blackCubeCount, int yellowCubeCount)
        {
            this.RedCubes.Text = $"Red Cubes Remaining:    {redCubeCount,-2}/24";
            this.BlueCubes.Text = $"Blue Cubes Remaining:   {blueCubeCount,-2}/24";
            this.BlackCubes.Text = $"Black Cubes Remaining:  {blackCubeCount,-2}/24";
            this.YellowCubes.Text = $"Yellow Cubes Remaining: {yellowCubeCount,-2}/24";
        }

        public void UpdateCounters(int infectionRate, int outbreakMarker)
        {
            this.InfectionRate.Text = $"Infection Rate: {infectionRate}";
            this.OutbreakCount.Text = $"Outbreak Count: {outbreakMarker}";
        }

        public void updateCureStatus(string redCureStatus, string blueCureStatus, string blackCureStatus, string yellowCureStatus)
        {
            // set value of cure label to status in game board
            // if status is NotCured, change to No Cure for nicer appearance
            this.RedCure.Text = $"Red:  {redCureStatus.Replace("NotCured", "No Cure")}";
            this.BlueCure.Text = $"Blue: {blueCureStatus.Replace("NotCured", "No Cure")}";
            this.BlackCure.Text = $"Black:  {blackCureStatus.Replace("NotCured", "No Cure")}";
            this.YellowCure.Text = $"Yellow: {yellowCureStatus.Replace("NotCured", "No Cure")}";
        }

    }
}
