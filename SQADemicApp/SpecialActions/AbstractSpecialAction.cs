using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp
{
    public abstract class AbstractSpecialAction
    {
        protected AbstractPlayer player;
        protected GameBoard board;

        public AbstractSpecialAction(AbstractPlayer player)
        {
            this.player = player;
        }

        public void setGameBoard(GameBoard board) 
        {
            this.board = board;
        }

        public abstract bool PreformAction();
    }
}
