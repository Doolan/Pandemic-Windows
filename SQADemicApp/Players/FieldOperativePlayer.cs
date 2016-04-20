using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQADemicApp.Objects;

namespace SQADemicApp.Players
{
    
    class FieldOperativePlayer : AbstractPlayer
    {
        Dictionary<String, int> cubes;

        public FieldOperativePlayer()
        {
            cubes = new Dictionary<string, int>();
            cubes.Add("Black", 0);
            cubes.Add("Red", 0);
            cubes.Add("Yellow", 0);
            cubes.Add("Blue", 0);            
        }

        public bool AddCubeToCard(String cube) 
        {
            if (this.cubes[cube] >= 3) { return false; }

            this.cubes[cube] = this.cubes[cube] + 1;
            return true;
        }

        public int getCubeCount(String color)
        {
            return this.cubes[color];
        }
    }
}
