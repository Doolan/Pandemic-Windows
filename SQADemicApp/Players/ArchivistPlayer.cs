using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQADemicApp;
using SQADemicApp.specialActions;

namespace SQADemicApp.Players
{
    public class ArchivistPlayer : AbstractPlayer
    {
        public ArchivistPlayer() {
            base.Maxhandsize = 8;
            this.SpecialActions.Add("ReclaimCityCard", new ReclaimCityCardAction(this));
        }
    }
}
