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
    public partial class Reg : Form
    {

        bool isLoginValid, isPasswordValid;
        public Reg()
        {
            InitializeComponent();
        }

        private void checkButton()
        {
            bSignUp.Enabled = isLoginValid && isPasswordValid;
        }

        private void bSignUp_Click(object sender, EventArgs e)
        {
            string login = EditAndHash.DeleteExtraSpaces(tbLogin.Text);
            string password = EditAndHash.DeleteBorderSpaces(tbPassword.Text);
            Database.NewUserReg(login, password);
            MessageBox.Show("Регистрация прошла успешно");
            Close();
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
                if (!Database.CheckLogin(login))
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
    }
}
