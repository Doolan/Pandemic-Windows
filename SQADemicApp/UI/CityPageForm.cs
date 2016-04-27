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
            CityColor.Text = city.Color.ToString();
            RedCubeCount.Text = city.Cubes.GetCubeCount(Color.Red).ToString();
            BlueCubeCount.Text = city.Cubes.GetCubeCount(Color.Blue).ToString();
            BlackCubeCount.Text = city.Cubes.GetCubeCount(Color.Black).ToString();
            YellowCubeCount.Text = city.Cubes.GetCubeCount(Color.Yellow).ToString();
            HasResearchStation.Checked = city.ResearchStation;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        

    }
}
