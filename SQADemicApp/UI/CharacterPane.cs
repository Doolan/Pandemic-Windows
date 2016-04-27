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
        private List<Button> _playerBtns;
         
        public CharacterPane()
        {
            InitializeComponent();
            SetPlayerBtns(new List<Button> { Player1, Player2, Player3, Player4 });
        }

        private void SetPlayerBtns(List<Button> btns)
        {
            _playerBtns = btns;
        }

        public List<Button> GetPlayerBtns()
        {
            return _playerBtns;
        }

        public CharacterPane(string[] playerRoles)
        {
            InitializeComponent();
            SetPlayerBtns(new List<Button> { Player1, Player2, Player3, Player4 });
            HidePlayersByCount(playerRoles.Count());
        }

        public void HidePlayersByCount(int count)
        {
            for (var i = count; i < 4; i++)
            {
                _playerBtns[i].Hide();
            }
        }

        private void Player_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if(GameBoard.CurrentState==GameBoard.State.Dispatcher)
            {
                GameBoard.DispatcherMoveIndex = Convert.ToInt32(btn.Text.Substring(7, 1)) -1;
            }
        }

        //
        public void UpdateCurrentPlayer(int currentPlayerIndex)
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

        public void UpdatePlayerCount(int playerCount)
        {
            switch (playerCount)
            {
                case 4:
                    this.Player4.Text = "Player 4\n" + GameBoardModels.GetPlayerByIndex(3) + "\n" + GameBoardModels.GetPlayerByIndex(3).CurrentCity.Name;
                    goto case 3;
                case 3:
                    this.Player3.Text = "Player 3\n" + GameBoardModels.GetPlayerByIndex(2) + "\n" + GameBoardModels.GetPlayerByIndex(2).CurrentCity.Name;
                    goto case 2;
                case 2:
                    this.Player1.Text = "Player 1\n" + GameBoardModels.GetPlayerByIndex(0) + "\n" + GameBoardModels.GetPlayerByIndex(0).CurrentCity.Name;
                    this.Player2.Text = "Player 2\n" + GameBoardModels.GetPlayerByIndex(1) + "\n" + GameBoardModels.GetPlayerByIndex(1).CurrentCity.Name;
                    break;
            }
        }

        public void UpdatePlayerCity(int playerIndex, string cityName)
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
