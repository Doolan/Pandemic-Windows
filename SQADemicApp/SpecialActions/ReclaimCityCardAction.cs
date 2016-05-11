using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.specialActions
{
    class ReclaimCityCardAction : AbstractSpecialAction
    {
        public ReclaimCityCardAction(AbstractPlayer player) : base(player) { }

        public override bool PreformAction()
        {
            if (!GameBoardModels.PlayerDiscardPileContains(player.CurrentCity.Name)) return false;
            player.AddCardToHand(GameBoardModels.ReclaminCityCardFromPlayerDeck(player.CurrentCity.Name));
            this.board.UpdatePlayerForm();
            return true;
        }
    }

}
