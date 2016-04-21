using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQADemicApp.Objects;

namespace SQADemicApp.UI
{
    public partial class CubeSelector : Form
    {
        private List<String> cubeList;
        public CubeSelector(InfectionCubesCity available)
        {
            InitializeComponent();

            cubeList = new List<String>();
            //This looks bad because it is
            if (available.GetCubeCount(COLOR.black) > 0) { cubeList.Add("Black"); };
            if (available.GetCubeCount(COLOR.blue) > 0) { cubeList.Add("Blue"); };
            if (available.GetCubeCount(COLOR.red) > 0) { cubeList.Add("Red"); };
            if (available.GetCubeCount(COLOR.yellow) > 0) { cubeList.Add("Yellow"); };

            this.ColorBox.DataSource = cubeList;           
        }

       

        public String getColor()
        {
            return (String)this.ColorBox.SelectedItem;
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
