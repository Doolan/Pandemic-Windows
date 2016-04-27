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
    public partial class EventCardForm : Form
    {
        public EventCardForm()
        {
            InitializeComponent();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(EventCardNames().ToArray());
        }
        private List<object> EventCardNames()
        {
            return GameBoardModels.EventCards.Select(eventCard => eventCard.CityName).Cast<object>().ToList();
        }

        public void UpdateEventCards()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(EventCardNames().ToArray());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedCard = listBox1.SelectedItem.ToString();
                switch(selectedCard)
                {
                    case "Airlift":
                        GameBoard.CurrentState = GameBoard.State.Airlift;
                        break;
                    case "One Quiet Night":
                        PlayerPanel.QuietNight = true;
                        break;
                    case "Resilient Population":
                        var dp = new DiscardPile(true);
                        dp.Show();
                        break;
                    case "Government Grant":
                        GameBoard.CurrentState = GameBoard.State.GovGrant;
                        break;
                    case "Forecast":
                        var forecast = new Forecast();
                        forecast.Show();
                        break;
                }
                GameBoardModels.EventCards.RemoveAll(x=> x.CityName ==  selectedCard);
                UpdateEventCards();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You must select an event card");
            }
        }
    }
}
