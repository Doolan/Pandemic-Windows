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
        public CubeSelector(InfectionCubes available)
        {
            InitializeComponent();

            var cubeList = new List<string>();
            //This looks bad because it is
            if (available.GetCubeCount(Color.Black) > 0) { cubeList.Add("Black"); };
            if (available.GetCubeCount(Color.Blue) > 0) { cubeList.Add("Blue"); };
            if (available.GetCubeCount(Color.Red) > 0) { cubeList.Add("Red"); };
            if (available.GetCubeCount(Color.Yellow) > 0) { cubeList.Add("Yellow"); };

            this.ColorBox.DataSource = cubeList;           
        }

       

        public string GetColor()
        {
            return (string)this.ColorBox.SelectedItem;
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
