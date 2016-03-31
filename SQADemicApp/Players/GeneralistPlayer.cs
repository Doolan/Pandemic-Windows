using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.Players
{
    class GeneralistPlayer : AbstractPlayer
    {
        public GeneralistPlayer()
        {
            base.MAXTURNCOUNT = 5;
        }
    }
}
