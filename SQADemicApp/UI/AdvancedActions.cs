﻿using System;
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
    public partial class AdvancedActions : Form
    {
        GameBoard board;
        public AdvancedActions(GameBoard board)
        {
            InitializeComponent();
            this.board = board;
        }

        private void ShareKnowledge_Click(object sender, EventArgs e)
        {
            var scForm = new ShareCardForm(board);
            scForm.Show();
            this.Close();
        }

        private void BuildResearchStation_Click(object sender, EventArgs e)
        {
            if (!GameBoardModels.GetCurrentPlayer().BuildAResearchStationOption())
            {
                MessageBox.Show("Research Station unable to be built");
            }
            else
            {
                if (this.board.BoardModel.IncTurnCount())
                    GameBoard.CurrentTurnPart = GameBoard.Turnpart.Draw;                
                board.UpdatePlayerForm();
                this.Close();
            }
        }

        private void CreateCure_Click(object sender, EventArgs e)
        {
            CureForm cureForm = new CureForm(board);
            cureForm.Show();
            this.Close();
        }
    }
}
