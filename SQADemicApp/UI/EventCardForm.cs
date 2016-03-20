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
            listBox1.Items.AddRange(eventCardNames().ToArray());
        }
        private List<object> eventCardNames()
        {
            List<object> eventCards = new List<object>();
            foreach (var eventCard in GameBoardModels.eventCards)
            {
                eventCards.Add(eventCard.CityName);
            }
            return eventCards;
        }
        public void UpdateEventCards()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(eventCardNames().ToArray());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedCard = listBox1.SelectedItem.ToString();
                switch(selectedCard)
                {
                    case "Airlift":
                        GameBoard.CurrentState = GameBoard.STATE.Airlift;
                        break;
                    case "One Quiet Night":
                        PlayerPanel.quietNight = true;
                        break;
                    case "Resilient Population":
                        DiscardPile dp = new DiscardPile(true);
                        dp.Show();
                        break;
                    case "Government Grant":
                        GameBoard.CurrentState = GameBoard.STATE.GovGrant;
                        break;
                    case "Forecast":
                        Forecast forecast = new Forecast();
                        forecast.Show();
                        break;
                }
                GameBoardModels.eventCards.RemoveAll(x=> x.CityName ==  selectedCard);
                UpdateEventCards();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You must select an event card");
            }
        }
    }
}
