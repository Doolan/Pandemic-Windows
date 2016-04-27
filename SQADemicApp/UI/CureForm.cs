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
    public partial class CureForm : Form
    {
        private readonly GameBoard _board;
        public CureForm(GameBoard board)
        {
            InitializeComponent();
            this._board = board;
            listBox1.Items.Clear();
            listBox1.Items.AddRange(GameBoardModels.GetCurrentPlayer().HandStringList().ToArray());
        }

        private void Cure_Click(object sender, EventArgs e)
        {
            var selectedCards = new List<string>();
            var citynamesWithColors = listBox2.Items;
            var cityNames  = (from object o in citynamesWithColors let index = o.ToString().IndexOf('(') - 1 select o.ToString().Substring(0, index)).ToList();

            var cured = false;
            try
            {
                cured = GameBoardModels.GetCurrentPlayer().Cure(cityNames, Create.CityDictionary[cityNames[0]].Color);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("You Win");
                return;
            }

            if (!cured)
                MessageBox.Show("Invalid card selection", "Invalid Selection");
            else
            {
            if (this._board.BoardModel.IncTurnCount())
                GameBoard.CurrentTurnPart = GameBoard.Turnpart.Draw;     
            this.Close();
            _board.UpdatePlayerForm();
        }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            var selectedCards = listBox2.SelectedItems.Cast<string>().ToList();
            listBox1.Items.AddRange(selectedCards.ToArray());
            foreach(var item in selectedCards)
            {
                listBox2.Items.Remove(item);
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            var selectedCards = listBox1.SelectedItems.Cast<string>().ToList();
            listBox2.Items.AddRange(selectedCards.ToArray());
            foreach (var item in selectedCards)
            {
                listBox1.Items.Remove(item);
            }
        }
    }
}
