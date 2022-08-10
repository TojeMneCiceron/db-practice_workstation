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
    public partial class Auth : Form
    {
        public int role;
        public Auth()
        {
            InitializeComponent();
            role = 0;
        }

        private void bLogIn_Click(object sender, EventArgs e)
        {
            string login = EditAndHash.DeleteExtraSpaces(tbLogin.Text);
            string password = EditAndHash.DeleteBorderSpaces(tbPassword.Text);
            int auth = Database.Authorize(login, password);
            switch (auth)
            {
                case 0: MessageBox.Show("Неверный логин или пароль"); break;
                case 1: MessageBox.Show("Вы вошли как admin"); role = auth; Close(); break;
                case 2: MessageBox.Show("Вы вошли как operator"); role = auth; Close(); break;
            }

        }

        private void bSIgnUp_Click(object sender, EventArgs e)
        {
            Reg rw = new Reg();
            Visible = false;
            rw.ShowDialog();
            Visible = true;
        }
    }
}
