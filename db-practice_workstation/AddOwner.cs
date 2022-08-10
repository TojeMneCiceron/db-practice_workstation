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
    public partial class AddOwner : Form
    {
        public int id;
        bool isNameValid, isPhoneValid;

        private static readonly Regex phoneRegex = new Regex("^8[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]$");

        public AddOwner()
        {
            InitializeComponent();
            bAdd.Enabled = false;
        }

        private void CheckButton()
        {
            bAdd.Enabled = isNameValid & isPhoneValid;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            isNameValid = EditAndHash.DeleteExtraSpaces(tbName.Text) != "";
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
            if (Database.CheckOwnerPhone(phone))
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
            id = Database.InsertOwner(EditAndHash.DeleteExtraSpaces(tbName.Text), EditAndHash.DeleteExtraSpaces(tbPhone.Text));
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
