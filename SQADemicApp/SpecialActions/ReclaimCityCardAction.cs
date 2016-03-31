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
            if (discardPlayerDeckContain(player.currentCity.Name))
            {
                player.addCardToHand(GameBoardModels.discardPlayerDeck.First(c => c.CityName.Equals(player.currentCity.Name)));
                return true;
            }
            return false;
        }

        private bool discardPlayerDeckContain(String cityName)
        {
            foreach(Card card in GameBoardModels.discardPlayerDeck) {
                if (card.CityName.Equals(cityName))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
