using System;
using System.Linq;

namespace SQADemicApp
{
    public class InfectionCubesBoard : InfectionCubes
    {
        public InfectionCubesBoard() : this(0)
        {
        }


        public InfectionCubesBoard(int startingValue) : base(startingValue)
        {
        }


        public override void DecrementCubeCount(Color color)
        {
            CubesSet[color] --;
            if (CubesSet[color] <= 0)
            {
                throw new InvalidOperationException("Game Over");
            }
        }
    }
}