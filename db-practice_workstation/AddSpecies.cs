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
    public partial class AddSpecies : Form
    {
        public int id;
        bool isNameValid, isAvgValid;
        public AddSpecies()
        {
            InitializeComponent();
            bAdd.Enabled = false;
        }

        private void CheckButton()
        {
            bAdd.Enabled = isNameValid & isAvgValid;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            string name = EditAndHash.DeleteExtraSpaces(tbName.Text);
            if (name == "")
                isNameValid = false;
            else
                if (Database.CheckOwnerPhone(name))
                isNameValid = true;
            else
            {
                isNameValid = false;
                ep.SetError(tbName, "Вид уже существует");
            }
            CheckButton();
        }

        private void tbAvg_TextChanged(object sender, EventArgs e)
        {
            int avg = 0;
            isAvgValid = int.TryParse(tbAvg.Text, out avg);
            if (!isAvgValid)
                ep.SetError(tbAvg, "Введите число");
            CheckButton();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            id = Database.InsertSpecies(EditAndHash.DeleteExtraSpaces(tbName.Text), cbIsEnd.Checked, int.Parse(tbAvg.Text));
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
