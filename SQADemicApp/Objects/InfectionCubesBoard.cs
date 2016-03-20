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


        public override void DecrementCubeCount(COLOR color)
        {
            cubesSet[color] --;
            if (cubesSet[color] <= 0)
            {
                throw new InvalidOperationException("Game Over");
            }
        }
    }
}