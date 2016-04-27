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
        private List<ComboBox> _playerComboBoxes = new List<ComboBox>();
        private List<CheckBox> _playerCheckBoxes = new List<CheckBox>();
        private readonly List<string> _playerStrings = new List<string>() // TODO: reroute this
        {
            "Dispatcher",
            "Operations Expert",
            "Scientist",
            "Medic",
            "Researcher",
            "Archivist",
            "Generalist",
            "Containment Specialist",
            "Troubleshooter",
            "Field Operative"
        };

        public SetupGameForm()
        {
            InitializeComponent();
            var playerStringArray = _playerStrings.ToArray();

            Player1ComboBox.Items.Clear();
            Player2ComboBox.Items.Clear();
            Player3ComboBox.Items.Clear();
            Player4ComboBox.Items.Clear();
            Player1ComboBox.Items.AddRange(_playerStrings.ToArray());
            Player2ComboBox.Items.AddRange(_playerStrings.ToArray());
            Player3ComboBox.Items.AddRange(_playerStrings.ToArray());
            Player4ComboBox.Items.AddRange(_playerStrings.ToArray());
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            _playerComboBoxes = new List<ComboBox>();
            _playerCheckBoxes = new List<CheckBox>();

            _playerComboBoxes.Add(Player1ComboBox);
            _playerComboBoxes.Add(Player2ComboBox);
            _playerComboBoxes.Add(Player3ComboBox);
            _playerComboBoxes.Add(Player4ComboBox);

            _playerCheckBoxes.Add(Player1CheckBox);
            _playerCheckBoxes.Add(Player2CheckBox);
            _playerCheckBoxes.Add(Player3CheckBox);
            _playerCheckBoxes.Add(Player4CheckBox);

            var rolesList = new List<string>();
            for (var i = 0; i < _playerCheckBoxes.Count; i++)
            {
                if (_playerCheckBoxes[i].Checked)
                {
                    rolesList.Add(_playerComboBoxes[i].SelectedItem.ToString());
                }
            }


            Program.RolesArray = rolesList.ToArray<string>();
            this.Close();
        }

        public bool CheckForDuplicateRoles(List<string> rolesList)
        {
            var duplicates = rolesList.GroupBy(z => z).Where(g => g.Count() > 1).Select(g => g.Key);
            if (!duplicates.Any()) return false;
            var duplicateWords = duplicates.Aggregate("", (current, dup) => current + (dup + ", "));

            MessageBox.Show("You cannot have more than one: " + duplicateWords.Substring(0, duplicateWords.Length - 2));
            return true;
        }
    }
}
