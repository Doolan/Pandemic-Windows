using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.Players
{
    public class MedicPlayer : AbstractPlayer
    {
        public override bool TreatDiseaseOption(COLOR color)
        {
            int number = GetDiseaseCubes(currentCity, color);
            if (number < 1)
                return false;
            return SetDiseaseCubes(currentCity, color, 0, number);
        }
    }
}
