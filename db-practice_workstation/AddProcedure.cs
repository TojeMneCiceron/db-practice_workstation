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
    public partial class AddProcedure : Form
    {
        public int id;
        bool isNameValid, isCostValid;
        public AddProcedure()
        {
            InitializeComponent();
            bAdd.Enabled = false;
        }

        private void CheckButton()
        {
            bAdd.Enabled = isNameValid & isCostValid;
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
                ep.SetError(tbName, "Процедура уже существует");
            }
            CheckButton();
        }

        private void tbCost_TextChanged(object sender, EventArgs e)
        {
            int cost = 0;
            isCostValid = int.TryParse(tbCost.Text, out cost);
            if (!isCostValid)
                ep.SetError(tbCost, "Введите число");
            CheckButton();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            id = Database.InsertProcedure(EditAndHash.DeleteExtraSpaces(tbName.Text), int.Parse(tbCost.Text));
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
