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
        private List<Button> PlayerBtns;
         
        public CharacterPane()
        {
            InitializeComponent();
            setPlayerBtns(new List<Button> { Player1, Player2, Player3, Player4 });
        }

        private void setPlayerBtns(List<Button> btns)
        {
            PlayerBtns = btns;
        }

        public List<Button> getPlayerBtns()
        {
            return PlayerBtns;
        }

        public CharacterPane(string[] playerRoles)
        {
            InitializeComponent();
            setPlayerBtns(new List<Button> { Player1, Player2, Player3, Player4 });
            HidePlayersByCount(playerRoles.Count());
        }

        public void HidePlayersByCount(int count)
        {
            for (int i = count; i < 4; i++)
            {
                PlayerBtns[i].Hide();
            }
        }

        private void Player_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if(GameBoard.CurrentState==GameBoard.STATE.Dispatcher)
            {
                GameBoard.dispatcherMoveIndex = Convert.ToInt32(btn.Text.Substring(7, 1)) -1;
            }
        }
    }
}
