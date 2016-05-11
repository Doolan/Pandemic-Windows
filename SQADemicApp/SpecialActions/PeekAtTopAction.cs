using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQADemicApp.SpecialActions
{
    class PeekAtTopAction : AbstractSpecialAction
    {

        public PeekAtTopAction(AbstractPlayer player) : base(player) { }
        public override bool PreformAction()
        {
            Stack<Card> deck = GameBoardModels.GetPlayerDeck();
            String cards = "";
            for (int i = 0; i < GameBoardModels.InfectionRate; i++ )
            {
                cards = cards + deck.Pop().CityName + "\n";
            }
            MessageBox.Show(cards, "Peek", MessageBoxButtons.OK, MessageBoxIcon.None);
            return true;
        }

        

    }
}
