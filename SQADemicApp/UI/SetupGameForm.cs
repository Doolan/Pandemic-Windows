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
    public partial class SetupGameForm : Form
    {
        private List<ComboBox> playerComboBoxes = new List<ComboBox>();
        private List<CheckBox> playerCheckBoxes = new List<CheckBox>();
        private List<String> playerStrings = new List<String>() // TODO: reroute this
        {
            "Dispatcher",
            "Operations Expert",
            "Scientist",
            "Medic",
            "Researcher",
            "Archivist",
            "Generalist",
            "Containment Specialist"
        };

        public SetupGameForm()
        {
            InitializeComponent();
            String[] playerStringArray = playerStrings.ToArray();

            Player1ComboBox.Items.Clear();
            Player2ComboBox.Items.Clear();
            Player3ComboBox.Items.Clear();
            Player4ComboBox.Items.Clear();
            Player1ComboBox.Items.AddRange(playerStrings.ToArray());
            Player2ComboBox.Items.AddRange(playerStrings.ToArray());
            Player3ComboBox.Items.AddRange(playerStrings.ToArray());
            Player4ComboBox.Items.AddRange(playerStrings.ToArray());
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            playerComboBoxes = new List<ComboBox>();
            playerCheckBoxes = new List<CheckBox>();

            playerComboBoxes.Add(Player1ComboBox);
            playerComboBoxes.Add(Player2ComboBox);
            playerComboBoxes.Add(Player3ComboBox);
            playerComboBoxes.Add(Player4ComboBox);

            playerCheckBoxes.Add(Player1CheckBox);
            playerCheckBoxes.Add(Player2CheckBox);
            playerCheckBoxes.Add(Player3CheckBox);
            playerCheckBoxes.Add(Player4CheckBox);

            List<String> rolesList = new List<String>();
            for (int i = 0; i < playerCheckBoxes.Count; i++)
            {
                if (playerCheckBoxes[i].Checked)
                {
                    rolesList.Add(playerComboBoxes[i].SelectedItem.ToString());
                }
            }

            CheckForDuplicateRoles(rolesList);

            Program.rolesArray = rolesList.ToArray<String>();
            this.Close();
        }

        private void CheckForDuplicateRoles(List<String> rolesList)
        {
            var duplicates = rolesList.GroupBy(z => z).Where(g => g.Count() > 1).Select(g => g.Key);
            if (duplicates.Count() > 0)
            {
                string duplicateWords = "";
                foreach (var dup in duplicates)
                {
                    duplicateWords += dup + ", ";
                }

                MessageBox.Show("You cannot have more than one: " + duplicateWords.Substring(0, duplicateWords.Length - 2));
                return;
            }
        }
    }
}
