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
        private readonly GameBoard _board;
        public TreatDiseaseForm(GameBoard board)
        {
            this._board = board;
            InitializeComponent();
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            Color colorChoice;
            switch(button.Text)
            {
                case "Blue":
                    colorChoice = Color.Blue;
                    break;
                case "Black":
                    colorChoice = Color.Black;
                    break;
                case "Red":
                    colorChoice = Color.Red;
                    break;
                default:
                    colorChoice = Color.Yellow;
                    break;
            }
            GameBoardModels.GetCurrentPlayer().TreatDiseaseOption(colorChoice);

            if (this._board.BoardModel.IncTurnCount())
                GameBoard.CurrentTurnPart = GameBoard.Turnpart.Draw;
            this.Close();
            _board.UpdatePlayerForm();
            _board.UpdateCityButtons(false);
        }
    }
}
