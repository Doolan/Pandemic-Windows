using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.Players
{
    public class MedicPlayer : AbstractPlayer
    {
        public override bool TreatDiseaseOption(Color color)
        {
            var number = GetDiseaseCubes(CurrentCity, color);
            return number >= 1 && SetDiseaseCubes(CurrentCity, color, 0, number);
        }
    }
}
