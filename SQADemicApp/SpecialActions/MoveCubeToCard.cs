using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQADemicApp.UI;
using SQADemicApp.Players;

namespace SQADemicApp.SpecialActions
{
    class MoveCubeToCard : AbstractSpecialAction
    {
        public MoveCubeToCard(FieldOperativePlayer player) : base(player) { }


        public override bool PreformAction()
        {
            CubeSelector selector = new CubeSelector(player.currentCity.Cubes);
            selector.ShowDialog();
            player.addCubeToCard(selector.getColor());

            return true;
        }
    }
}
