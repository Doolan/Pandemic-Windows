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

        public override void DecrementCubeCount(Color color)
        {
            CubesSet[color]--;
        }

        public int GetTotalCubeCount()
        {
            return CubesSet.Sum(cube => cube.Value);
        }
    }
}
