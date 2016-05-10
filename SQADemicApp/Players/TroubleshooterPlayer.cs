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
            this.SpecialActions.Add("Peek At Top", new PeekAtTopAction(this));
        }

        public override void RemoveMovementCards(City city) {}
        
    }
}
