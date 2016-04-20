using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.specialActions
{
    public class ReclaimCityCardAction : AbstractSpecialAction
    {
        public ReclaimCityCardAction(AbstractPlayer player) : base(player) { }

        public override bool PreformAction()
        {
            if (!GameBoardModels.PlayerDiscardPileContains(player.currentCity.Name)) return false;
            player.addCardToHand(GameBoardModels.ReclaminCityCardFromPlayerDeck(player.currentCity.Name));
            return true;
        }
    }

}
