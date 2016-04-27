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
            this.SpecialActions.Add("Add Cube to Card", action);
        }

        public override bool CanCure(int numberOfAvailableCards, Color color)
        {
            Dictionary<Color, string> colorToString = new Dictionary<Color, string>
            {
                {Color.Black, "Black"},
                {Color.Blue, "Blue"},
                {Color.Red, "Red"},
                {Color.Yellow, "Yellow"}
            };

            if (!IsCurable(color))
                return false;
            if (HaveEnoughCardsToCure(numberOfAvailableCards))
                return true;
            return numberOfAvailableCards == 3 && action.haveEnoughCubes(colorToString[color]);
        }
        
    }
}
