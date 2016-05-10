using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SQADemicApp.UI
{
    public partial class DiscardExtraCards : Form
    {
        private readonly GameBoard _board;
        private List<Card> _hand;
        private List<Card> _discard;


        public DiscardExtraCards(GameBoard board)
        {
            InitializeComponent();
            this._board = board;
            this._hand = GameBoardModels.GetCurrentPlayer().Hand;
            this._discard = new List<Card>();
            CurrentHandBox.Items.Clear();
            CurrentHandBox.Items.AddRange(GameBoardModels.GetCurrentPlayer().Hand.ToArray());
            CurrentHandSizeLabel.Text = CurrentHandSizeLabel.Text.Split(':')[0] + ": " + CurrentHandBox.Items.Count;
            MaxHandSizeLabel.Text = MaxHandSizeLabel.Text.Split(':')[0] + ": " + GameBoardModels.GetCurrentPlayer().GetMaxHandSize();

        }

        private void Add_Click(object sender, System.EventArgs e)
        {
            var selectedCards = CurrentHandBox.SelectedItems.Cast<Card>().ToList();
            DiscardBox.Items.AddRange(selectedCards.ToArray());
            foreach (var item in selectedCards)
            {
                CurrentHandBox.Items.Remove(item);
            }
            UpdateCurrentHandSizeLable();
            UpdateConfirmButton();
        }

        private void Remove_Click(object sender, System.EventArgs e)
        {
            
            var selectedCards = DiscardBox.SelectedItems.Cast<Card>().ToList();
            CurrentHandBox.Items.AddRange(selectedCards.ToArray());
            foreach (var item in selectedCards)
            {
                DiscardBox.Items.Remove(item);
            }
            UpdateCurrentHandSizeLable();
            UpdateConfirmButton();
        }

        private void Confirm_Click(object sender, System.EventArgs e)
        {
            //remove cards in DiscardBox from the players hand and add them to the discard pile
            var selectedCards = DiscardBox.Items.Cast<Card>().ToList();
            GameBoardModels.GetCurrentPlayer().Hand = CurrentHandBox.Items.Cast<Card>().ToList();

            foreach (var card in DiscardBox.Items.Cast<Card>().ToList())
            {
                GameBoardModels.DiscardCard(card);
            }
            _board.UpdatePlayerForm();
            this.Close();
        }

        private void UpdateCurrentHandSizeLable() 
        {
            CurrentHandSizeLabel.Text = CurrentHandSizeLabel.Text.Split(':')[0] + ": " + CurrentHandBox.Items.Count;
        }

        private void UpdateConfirmButton() {
            if(CurrentHandBox.Items.Count == GameBoardModels.GetCurrentPlayer().GetMaxHandSize()) {
                this.ConfirmButton.Enabled = true;
            } else {
                this.ConfirmButton.Enabled = false;
            }
        }

    }
}
