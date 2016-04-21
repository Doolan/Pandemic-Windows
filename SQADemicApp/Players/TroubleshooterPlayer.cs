using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQADemicApp.SpecialActions;

namespace SQADemicApp.Players
{
    public class TroubleshooterPlayer : AbstractPlayer
    {
        public TroubleshooterPlayer()
        {
            this.specialActions.Add("Peek At Top 3", new PeekAtTopThreeAction(this));
        }

        public override void RemoveDirectFlightCards(City city) {}
        
    }
}
