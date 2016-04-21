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
            base.MAXHANDSIZE = 8;
            this.specialActions.Add("ReclaimCityCard", new ReclaimCityCardAction(this));
        }
    }
}
