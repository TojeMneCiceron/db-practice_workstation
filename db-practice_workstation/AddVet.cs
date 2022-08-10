using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arm
{
    public partial class AddVet : Form
    {
        public int id;
        bool isNameValid, isWorkExpValid, isPhoneValid;

        private static readonly Regex phoneRegex = new Regex("^8[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]$");

        public AddVet()
        {
            InitializeComponent();
            bAdd.Enabled = false;

            cmbVetclinic.DisplayMember = "vc_address";
            cmbVetclinic.ValueMember = "vc_id";
            cmbVetclinic.DataSource = Database.VetClinics();

            cmbCategory.DisplayMember = "category_name";
            cmbCategory.ValueMember = "category_id";
            cmbCategory.DataSource = Database.Categories();
        }

        private void CheckButton()
        {
            bAdd.Enabled = isNameValid & isWorkExpValid & isPhoneValid;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            isNameValid = EditAndHash.DeleteExtraSpaces(tbName.Text) != "";
            CheckButton();
        }

        private void tbWorkExp_TextChanged(object sender, EventArgs e)
        {
            int we = 0;
            isWorkExpValid = int.TryParse(tbWorkExp.Text, out we);
            if (!isWorkExpValid)
                ep.SetError(tbWorkExp, "Введите число");
            CheckButton();
        }

        private void tbPhone_TextChanged(object sender, EventArgs e)
        {
            string phone = EditAndHash.DeleteExtraSpaces(tbPhone.Text);
            if (phone == "")
            {
                isPhoneValid = false;
                ep.SetError(tbPhone, "Введите номер телефона");
            }
            else
            if (!phoneRegex.IsMatch(phone))
            {
                isPhoneValid = false;
                ep.SetError(tbPhone, "Некорректный номер телефона");
            }
            else
            if (Database.CheckVetPhone(phone))
            {
                isPhoneValid = true;
                ep.Clear();
            }
            else
            {
                isPhoneValid = false;
                ep.SetError(tbPhone, "Номер телефона уже существует");
            }
            CheckButton();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            id = Database.InsertVet(EditAndHash.DeleteExtraSpaces(tbName.Text), Convert.ToInt32(cmbCategory.SelectedValue), int.Parse(tbWorkExp.Text), EditAndHash.DeleteExtraSpaces(tbPhone.Text), Convert.ToInt32(cmbVetclinic.SelectedValue));
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
