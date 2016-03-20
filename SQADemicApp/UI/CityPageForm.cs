using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQADemicApp
{
    public partial class CityPageForm : Form
    {
        public CityPageForm(City city)
        {
            InitializeComponent();
            CityName.Text = city.Name;
            CityColor.Text = city.color.ToString();
            RedCubeCount.Text = city.Cubes.GetCubeCount(COLOR.red).ToString();
            BlueCubeCount.Text = city.Cubes.GetCubeCount(COLOR.blue).ToString();
            BlackCubeCount.Text = city.Cubes.GetCubeCount(COLOR.black).ToString();
            YellowCubeCount.Text = city.Cubes.GetCubeCount(COLOR.yellow).ToString();
            HasResearchStation.Checked = city.researchStation;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        

    }
}
