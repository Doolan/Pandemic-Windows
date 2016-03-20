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
            RedCubeCount.Text = city.redCubes.ToString();
            BlueCubeCount.Text = city.blueCubes.ToString();
            BlackCubeCount.Text = city.blackCubes.ToString();
            YellowCubeCount.Text = city.yellowCubes.ToString();
            HasResearchStation.Checked = city.researchStation;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        

    }
}
