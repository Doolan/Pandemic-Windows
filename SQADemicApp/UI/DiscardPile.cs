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
    public partial class DiscardPile : Form
    {
        public DiscardPile()
        {
            InitializeComponent();
        }
        public DiscardPile(bool resilientPopulation)
        {
            InitializeComponent();
            if(!resilientPopulation)
            {
                Discard.Hide();
            }
            SetCards();
        }
        private void SetCards()
        {
            listBox1.Items.Clear();
            var discarded = new List<string>(GameBoardModels.GetInfectionPile());
            listBox1.Items.AddRange(discarded.ToArray());
        }

        private void Discard_Click(object sender, EventArgs e)
        {
            try
            {
                var selecteditem = listBox1.SelectedItem.ToString();
                GameBoardModels.RemoveFromInfectionPile(selecteditem);
                this.Close();
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("No card selected");
            }

        }
    }
}
