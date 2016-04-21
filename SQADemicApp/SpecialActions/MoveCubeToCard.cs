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
        protected Dictionary<String, int> cubes;

        public MoveCubeToCard(AbstractPlayer player) : base(player) 
        {
            cubes = new Dictionary<String, int>();
            cubes.Add("Black", 0);
            cubes.Add("Blue", 0);
            cubes.Add("Red", 0);
            cubes.Add("Yellow", 0);  
        }


        public override bool PreformAction()
        {
            CubeSelector selector = new CubeSelector(player.currentCity.Cubes);
            selector.ShowDialog();
            return AddCubeToCard(selector.getColor());
        }

        public bool haveEnoughCubes(String color)
        {
            return cubes[color] == 3;
        }

       private bool AddCubeToCard(String cube)
       {


            if (cube == null) { return true; }
            if (this.cubes[cube] >= 3) { return false; }

            this.cubes[cube] = this.cubes[cube] + 1;
            RemoveCubeFromBoard(cube);
            return true;
       }

        private void RemoveCubeFromBoard(String cube) 
        {
            //This looks dumb because it is, I just dont want another if, if else statement
            Dictionary<String, COLOR> colorMap = new Dictionary<string,COLOR>();
            colorMap.Add("Black", COLOR.black);
            colorMap.Add("Blue", COLOR.blue);
            colorMap.Add("Red", COLOR.red);
            colorMap.Add("Yellow", COLOR.yellow);

            this.player.TreatDiseaseOption(colorMap[cube]);
            this.board.boardModel.IncTurnCount();
            this.board.UpdatePlayerForm();
            this.board.UpdateCityButtons(false);
        }
    }
}
