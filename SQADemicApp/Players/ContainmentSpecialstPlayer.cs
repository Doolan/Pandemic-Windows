using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQADemicApp.Players
{
    public class ContainmentSpecialstPlayer : AbstractPlayer
    {
        public override bool MovePlayer(City city) 
        {
            if(base.MovePlayer(city)) {
                //Ok so this is how we have to do it for now because I 
                //can't get a list/anything I can call a foreach
                //loop on to make this look less dumb
                DiseaseContainment(city, COLOR.black);
                DiseaseContainment(city, COLOR.blue);
                DiseaseContainment(city, COLOR.yellow);
                DiseaseContainment(city, COLOR.red);
                return true;
            }
            return false;
        }

        private void DiseaseContainment(City city, COLOR color)
        {
            if (GetDiseaseCubes(city, color) >= 2)
            {
                SetDiseaseCubes(city, color, GetDiseaseCubes(city, color) - 1, GetDiseaseCubes(city, color));
            }
        }

    }
}
