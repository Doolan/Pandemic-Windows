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
    public partial class Forecast : Form
    {
        public Forecast()
        {
            InitializeComponent();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(SQADemicApp.BL.SpecialEventCardsBL.GetForcastCards(GameBoardModels.infectionDeck).ToArray());
        }

        private void Reorder_Click(object sender, EventArgs e)
        {
            List<string> selectedCards = new List<string>();
            foreach (var select in listBox2.SelectedItems)
            {
                selectedCards.Add(select.ToString());
            }
            SQADemicApp.BL.SpecialEventCardsBL.CommitForcast(GameBoardModels.infectionDeck, selectedCards);
            this.Close();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            List<string> selectedCards = new List<string>();
            selectedCards = listBox2.SelectedItems.Cast<String>().ToList();
            listBox1.Items.AddRange(selectedCards.ToArray());
            foreach(var item in selectedCards)
            {
                listBox2.Items.Remove(item);
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            List<string> selectedCards = new List<string>();
            selectedCards = listBox1.SelectedItems.Cast<String>().ToList();
            listBox2.Items.AddRange(selectedCards.ToArray());
            foreach (var item in selectedCards)
            {
                listBox1.Items.Remove(item);
            }
        }
    }
}
