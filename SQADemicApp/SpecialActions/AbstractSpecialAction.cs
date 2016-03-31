using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.SpecialActions
{
    abstract class AbstractSpecialAction
    {
        private AbstractPlayer player;

        public AbstractSpecialAction(AbstractPlayer player)
        {
            this.player = player;
        }

        public abstract bool PreformAction();
    }
}
