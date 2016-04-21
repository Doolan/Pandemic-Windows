using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.Players
{
    public class ScientistPlayer : AbstractPlayer
    {
        public override bool HaveEnoughCardsToCure(int num)
        {
            return num == 4;
        }
    }
}
