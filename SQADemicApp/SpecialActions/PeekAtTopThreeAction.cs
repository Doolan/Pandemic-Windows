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
            for (int i = 0; i < 3; i++ )
            {
                cards = cards + deck.Pop().CityName + "\n";
            }
            MessageBox.Show(cards, "Top 3 Cards", MessageBoxButtons.OK, MessageBoxIcon.None);
            return true;
        }

        

    }
}
