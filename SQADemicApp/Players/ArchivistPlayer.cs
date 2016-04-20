using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQADemicApp;
using SQADemicApp.specialActions;

namespace SQADemicApp.Players
{
    class ArchivistPlayer : AbstractPlayer
    {
        public ArchivistPlayer() {
            base.MAXHANDSIZE = 8;
            this.specialActions.Add("ReclaimCityCard", new ReclaimCityCardAction(this));
        }

        public static List<String> DirectFlightOption(List<Card> hand, City currentCity)
        {
            //Question here, why do we need to return a List<String> here?
            var returnlist = new List<String>();
            foreach (Card card in hand)
            {
                returnlist.Add(card.CityName);
            }

            return returnlist;
        }
    }
}
