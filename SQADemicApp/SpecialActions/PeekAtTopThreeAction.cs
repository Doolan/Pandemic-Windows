using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQADemicApp.SpecialActions
{
    class PeekAtTopThreeAction : AbstractSpecialAction
    {

        public PeekAtTopThreeAction(AbstractPlayer player) : base(player) { }
        public override bool PreformAction()
        {
            Stack<Card> deck = GameBoardModels.playerDeck;
            String cards = "";
            while (deck.Count != 0)
            {
                cards = cards + deck.Pop().ToString() + "\n";
            }
            MessageBox.Show("Top 3 Cards", cards, MessageBoxButtons.OK, MessageBoxIcon.None);
            return true;
        }

    }
}
