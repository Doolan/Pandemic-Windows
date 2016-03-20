using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.Objects
{
    public class InfectionCubesCity : InfectionCubes
    {
        public InfectionCubesCity() : this(0)
        {
        }


        public InfectionCubesCity(int startingValue) : base(startingValue)
        {
        }

        public override void DecrementCubeCount(COLOR color)
        {
            cubesSet[color]--;
        }

        public int GetTotalCubeCount()
        {
            return cubesSet.Sum(cube => cube.Value);
        }
    }
}
