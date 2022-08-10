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
    public partial class Edit : Form
    {
        string oldLogin;
        bool isLoginValid, isPasswordValid;
        public string newLogin, password_hash, role;
        public DateTime date;
        public int role_id;
        public Edit(string login, DateTime date, int role_id)
        {
            InitializeComponent();
            tbLogin.Text = oldLogin = login;
            dtpReg.Value = date;
            bSave.Enabled = true;

            cmbRole.DisplayMember = "name";
            cmbRole.ValueMember = "id";
            cmbRole.DataSource = Database.Roles();
            cmbRole.SelectedValue = role_id;
        }

        private void checkButton()
        {
            bSave.Enabled = isLoginValid && bChangePassword.Visible || isLoginValid && isPasswordValid;
        }

        private void tbLogin_TextChanged(object sender, EventArgs e)
        {
            string login = EditAndHash.DeleteExtraSpaces(tbLogin.Text);
            if (!EditAndHash.CheckLogin(login))
            {
                isLoginValid = false;
                ep.SetError(tbLogin, "Логин должен содержать символы, изображённые на классической русско-английской раскладке клавиатуре и любые пробельные символы");
            }
            else
                if (!Database.CheckLogin(login) && oldLogin != login)
                {
                    isLoginValid = false;
                    ep.SetError(tbLogin, "Логин уже занят");
                }
                else
                {
                    isLoginValid = true;
                    ep.SetError(tbLogin, "");
                }
            checkButton();
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            string password = tbPassword.Text;
            if (EditAndHash.CheckPassword(password))
            {
                isPasswordValid = true;
                ep.SetError(tbPassword, "");
            }
            else
            {
                isPasswordValid = false;
                ep.SetError(tbPassword, "Пароль должен содержать не менее 8 символов, отличных от пробельных");
            }
            checkButton();
        }

        private void bChangePassword_Click(object sender, EventArgs e)
        {
            tbPassword.Visible = true;
            bChangePassword.Visible = false;
            checkButton();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            newLogin = EditAndHash.DeleteExtraSpaces(tbLogin.Text);
            date = dtpReg.Value;
            role_id = (int)cmbRole.SelectedValue;
            role = cmbRole.Text;
            if (bChangePassword.Visible)
            {
                Database.EditUser(newLogin, oldLogin, date, role_id);
                password_hash = "";
            }
            else
            {
                string password = EditAndHash.DeleteBorderSpaces(tbPassword.Text);
                Database.EditUser(newLogin, oldLogin, password, date, ref password_hash, role_id);
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
