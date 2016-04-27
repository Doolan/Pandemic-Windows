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
            listBox1.Items.AddRange(SpecialEventCardsBL.GetForcastCards(GameBoardModels.GetInfectionDeck()).ToArray());
        }

        private void Reorder_Click(object sender, EventArgs e)
        {
            var selectedCards = (from object @select in listBox2.SelectedItems select @select.ToString()).ToList();
            SpecialEventCardsBL.CommitForcast(GameBoardModels.GetInfectionDeck(), selectedCards);
            this.Close();
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
