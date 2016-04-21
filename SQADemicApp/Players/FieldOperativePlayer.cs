using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQADemicApp.Objects;
using SQADemicApp.SpecialActions;

namespace SQADemicApp.Players
{
    
    public class FieldOperativePlayer : AbstractPlayer
    {
        private MoveCubeToCard action;
        public FieldOperativePlayer()
        {
            action = new MoveCubeToCard(this);
            this.specialActions.Add("Add Cube to Card", action);
        }

        public override bool CanCure(int numberOfAvailableCards, COLOR color)
        {
            Dictionary<COLOR, String> colorToString = new Dictionary<COLOR, string>();
            colorToString.Add(COLOR.black, "Black");
            colorToString.Add(COLOR.blue, "Blue");
            colorToString.Add(COLOR.red, "Red");
            colorToString.Add(COLOR.yellow, "Yellow");

            if (!IsCurable(color))
                return false;
            if (HaveEnoughCardsToCure(numberOfAvailableCards))
                return true;
            return numberOfAvailableCards == 3 && action.haveEnoughCubes(colorToString[color]);
        }
        
    }
}
