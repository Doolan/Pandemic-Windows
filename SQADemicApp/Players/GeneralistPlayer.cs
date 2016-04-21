using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.Players
{
    public class GeneralistPlayer : AbstractPlayer
    {
        public GeneralistPlayer()
        {
            base.MAXTURNCOUNT = 5;
        }
    }
}
