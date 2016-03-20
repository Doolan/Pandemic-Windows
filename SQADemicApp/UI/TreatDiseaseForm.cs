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
    public partial class TreatDiseaseForm : Form
    {
        GameBoard board;
        public TreatDiseaseForm(GameBoard board)
        {
            this.board = board;
            InitializeComponent();
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            COLOR colorChoice;
            switch(button.Text)
            {
                case "Blue":
                    colorChoice = COLOR.blue;
                    break;
                case "Black":
                    colorChoice = COLOR.black;
                    break;
                case "Red":
                    colorChoice = COLOR.red;
                    break;
                default:
                    colorChoice = COLOR.yellow;
                    break;
            }
            PlayerActionsBL.TreatDiseaseOption(GameBoardModels.players[GameBoardModels.CurrentPlayerIndex], colorChoice);

            if (this.board.boardModel.incTurnCount())
                GameBoard.turnpart = GameBoard.TURNPART.Draw;
            this.Close();
            board.UpdatePlayerForm();
            board.UpdateCityButtons(false);
        }
    }
}
