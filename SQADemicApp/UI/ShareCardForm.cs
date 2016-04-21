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
    public partial class ShareCardForm : Form
    {
        GameBoard board;
        public ShareCardForm(GameBoard board)
        {
            this.board = board;
            InitializeComponent();
            switch(GameBoardModels.GetCurrentPlayerIndex())
            {
                case 0:
                    P1T.Text = "Player 2";
                    P2T.Text = "Player 3";
                    P3T.Text = "Player 4";
                    break;
                case 1:
                    P1T.Text = "Player 1";
                    P2T.Text = "Player 3";
                    P3T.Text = "Player 4";
                    break;
                case 2:
                    P1T.Text = "Player 1";
                    P2T.Text = "Player 2";
                    P3T.Text = "Player 4";
                    break;
                case 3:
                    P1T.Text = "Player 1";
                    P2T.Text = "Player 2";
                    P3T.Text = "Player 3";
                    break;
                default:
                    break;
            }
            switch (GameBoardModels.GetPlayerCount())
            {
                case 2:
                    P2T.Hide();
                    goto case 3;
                case 3:
                    P3T.Hide();
                    break;
            }
            listBox1.Items.Clear();
            listBox1.Items.AddRange(GameBoardModels.GetCurrentPlayer().HandStringList().ToArray());
            List<object> allHands = new List<object>();
            foreach(var player in GameBoardModels.GetPlayers())
            {
                if (player.GetType() != GameBoardModels.GetCurrentPlayer().GetType())
                {
                    allHands.AddRange(player.HandStringList());
                }
                
            }
            listBox2.Items.Clear();
            listBox2.Items.AddRange(allHands.ToArray());
        }
        
        /// <summary>
        /// Click for give card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P1T_Click(object sender, EventArgs e)
        {
            var selectedItem = listBox1.SelectedItem.ToString();
            var selectedCard = selectedItem.Substring(0, selectedItem.IndexOf('(') - 1);
            bool success = false;
            switch (GameBoardModels.GetCurrentPlayerIndex())
            {
                case 0:
                    success = GameBoardModels.GetCurrentPlayer().ShareKnowledgeOption(GameBoardModels.GetPlayerByIndex(1), selectedCard);
                    break;
                default:
                    success = GameBoardModels.GetCurrentPlayer().ShareKnowledgeOption(GameBoardModels.GetPlayerByIndex(0), selectedCard);
                    break;
            }
            if (success)
            {
                if (this.board.boardModel.IncTurnCount())
                    GameBoard.turnpart = GameBoard.TURNPART.Draw;
            }
            this.Close();
            board.UpdatePlayerForm();
        }

        /// <summary>
        /// Click give card to the next player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P2T_Click(object sender, EventArgs e)
        {
            var selectedItem = listBox1.SelectedItem.ToString();
            var selectedCard = selectedItem.Substring(0, selectedItem.IndexOf('(') - 1);
            bool success = false;
            switch (GameBoardModels.GetCurrentPlayerIndex())
            {
                case 0:
                    success = GameBoardModels.GetCurrentPlayer().ShareKnowledgeOption(GameBoardModels.GetPlayerByIndex(2), selectedCard);
                    break;
                case 1:
                    success = GameBoardModels.GetCurrentPlayer().ShareKnowledgeOption(GameBoardModels.GetPlayerByIndex(2), selectedCard);
                    break;
                default:
                    success = GameBoardModels.GetCurrentPlayer().ShareKnowledgeOption(GameBoardModels.GetPlayerByIndex(1), selectedCard);
                    break;            
            }
            if (success)
            {
                if (this.board.boardModel.IncTurnCount())
                    GameBoard.turnpart = GameBoard.TURNPART.Draw;
            }
            this.Close();
            board.UpdatePlayerForm();
        }
        /// <summary>
        /// Click give card to the third player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P3T_Click(object sender, EventArgs e)
        {
            var selectedItem = listBox1.SelectedItem.ToString();
            var selectedCard = selectedItem.Substring(0, selectedItem.IndexOf('(') - 1);
            bool success = false;
            switch (GameBoardModels.GetCurrentPlayerIndex())
            {
                case 3:
                    success = GameBoardModels.GetCurrentPlayer().ShareKnowledgeOption(GameBoardModels.GetPlayerByIndex(3), selectedCard);
                    break;
                default:
                    success = GameBoardModels.GetCurrentPlayer().ShareKnowledgeOption(GameBoardModels.GetPlayerByIndex(2), selectedCard);
                    break;
            }
            if (success)
            {
                if (this.board.boardModel.IncTurnCount())
                    GameBoard.turnpart = GameBoard.TURNPART.Draw;
            }
            this.Close();
            board.UpdatePlayerForm();
        }

        private void StealCardButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedItem = listBox2.SelectedItem.ToString();
                var selectedCard = selectedItem.Substring(0,selectedItem.IndexOf('(')-1);
                AbstractPlayer SelectedCardHolder = GameBoardModels.GetCurrentPlayer();
                foreach(var player in GameBoardModels.GetPlayers())
                {
                    if(player.hand.Any(c=>c.CityName == selectedCard))
                    {
                        SelectedCardHolder = player;
                        break;
                    }
                }
                if(SelectedCardHolder.ShareKnowledgeOption(GameBoardModels.GetCurrentPlayer(), selectedCard))
                {
                    MessageBox.Show("Card Traded");
                    this.Close();
                    board.UpdatePlayerForm();
                }
                else
                {
                    MessageBox.Show("Card unable to be traded");
                }
            }

            catch(NullReferenceException)
            {
                MessageBox.Show("You must select a card to take");
            }
            
            
        }
    }
}
