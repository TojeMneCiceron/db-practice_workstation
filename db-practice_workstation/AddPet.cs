using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arm
{
    public partial class AddPet : Form
    {
        public int id;
        bool isNameValid, isAgeValid;
        public AddPet()
        {
            InitializeComponent();

            bAdd.Enabled = false;

            cmbOwner.ValueMember = "owner_id";
            cmbOwner.DisplayMember = "owner_name";
            cmbOwner.DataSource = Database.Owners();

            cmbSpecies.ValueMember = "species_id";
            cmbSpecies.DisplayMember = "species_name";
            cmbSpecies.DataSource = Database.Species();
        }

        private void CheckButton()
        {
            bAdd.Enabled = isNameValid & isAgeValid;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            isNameValid = EditAndHash.DeleteExtraSpaces(tbName.Text) != "";
            CheckButton();
        }

        private void tbAge_TextChanged(object sender, EventArgs e)
        {
            int age = 0;
            isAgeValid = int.TryParse(tbAge.Text, out age);
            if (!isAgeValid)
                ep.SetError(tbAge, "Введите число");
            CheckButton();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            id = Database.InsertPet(EditAndHash.DeleteExtraSpaces(tbName.Text), int.Parse(tbAge.Text), (int)cmbSpecies.SelectedValue, (int)cmbOwner.SelectedValue);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
