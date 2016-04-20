using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQADemicApp.SpecialActions;

namespace SQADemicApp.Players
{
    class Troubleshooter : AbstractPlayer
    {
        public Troubleshooter()
        {
            this.specialActions.Add("PeekAtTopThreeAction", new PeekAtTopThreeAction(this));
        }

        /**
        public override List<String> DirectFlightOption(List<Card> hand, City currentCity)
        {
            //Question here, why do we need to return a List<String> here?
            var returnlist = new List<String>();
            foreach (Card card in hand)
            {
                returnlist.Add(card.CityName);
            }

            return returnlist;
        }
        */
    }
}
