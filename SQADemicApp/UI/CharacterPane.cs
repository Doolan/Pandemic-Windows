using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQADemicApp
{
    public partial class CharacterPane : Form
    {
        private string[] playerRoles;

        public CharacterPane()
        {
            InitializeComponent();
        }

        public CharacterPane(string[] playerRoles)
        {
            InitializeComponent();
            this.playerRoles = playerRoles;
            switch(playerRoles.Count())
            {
                case 1:
                    Player2.Hide();
                    goto case 2;
                case 2:
                    Player3.Hide();
                    goto case 3;
                case 3:
                    Player4.Hide();
                    break;
            }
        }

        private void Player_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if(GameBoard.CurrentState==GameBoard.STATE.Dispatcher)
            {
                var playernum = btn.Text.Substring(7, 1);
                GameBoard.SetDispatcherMoveIndex(Convert.ToInt32(playernum)-1);
            }
        }

        //
        public void updateCurrentPlayer(int currentPlayerIndex)
        {
            switch (currentPlayerIndex)
            {
                case 3:
                    this.Player4.UseVisualStyleBackColor = false;
                    this.Player3.UseVisualStyleBackColor = true;
                    break;
                case 2:
                    this.Player3.UseVisualStyleBackColor = false;
                    this.Player2.UseVisualStyleBackColor = true;
                    break;
                case 1:
                    this.Player2.UseVisualStyleBackColor = false;
                    this.Player1.UseVisualStyleBackColor = true;
                    break;
                default:
                    this.Player1.UseVisualStyleBackColor = false;
                    this.Player4.UseVisualStyleBackColor = 
                        this.Player3.UseVisualStyleBackColor = 
                        this.Player2.UseVisualStyleBackColor = true;
                    break;
            }
        }

        public void updatePlayerCount(int playerCount)
        {
            switch (playerCount)
            {
                case 4:
                    this.Player4.Text = "Player 4\n" + GameBoardModels.GetPlayerByIndex(3) + "\n" + GameBoardModels.GetPlayerByIndex(3).currentCity.Name;
                    goto case 3;
                case 3:
                    this.Player3.Text = "Player 3\n" + GameBoardModels.GetPlayerByIndex(2) + "\n" + GameBoardModels.GetPlayerByIndex(2).currentCity.Name;
                    goto case 2;
                case 2:
                    this.Player1.Text = "Player 1\n" + GameBoardModels.GetPlayerByIndex(0) + "\n" + GameBoardModels.GetPlayerByIndex(0).currentCity.Name;
                    this.Player2.Text = "Player 2\n" + GameBoardModels.GetPlayerByIndex(1) + "\n" + GameBoardModels.GetPlayerByIndex(1).currentCity.Name;
                    break;
            }
        }

        public void updatePlayerCity(int playerIndex, String cityName)
        {
            switch (playerIndex)
            {
                case 3:
                    this.Player4.Text = "Player 4\n" + GameBoardModels.GetPlayerByIndex(3) + "\n" + cityName;
                    break;
                case 2:
                    this.Player3.Text = "Player 3\n" + GameBoardModels.GetPlayerByIndex(2) + "\n" + cityName;
                    break;
                case 1:
                    this.Player2.Text = "Player 2\n" + GameBoardModels.GetPlayerByIndex(1) + "\n" + cityName;
                    break;
                default:
                    this.Player1.Text = "Player 1\n" + GameBoardModels.GetPlayerByIndex(0) + "\n" + cityName;
                    break;
            }
        }
    }
}
