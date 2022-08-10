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
    public partial class Form1 : Form
    {     
        public Form1()
        {
            InitializeComponent();
            Database.Connect();
        }

        private void bLogIn_Click(object sender, EventArgs e)
        {
            Auth aw = new Auth();
            Visible = false;
            aw.ShowDialog();

            switch (aw.role)
            {
                case 0: break;
                default: Tables tab = new Tables(aw.role); tab.ShowDialog(); break;
            }

            Visible = true;
        }

        private void bGuest_Click(object sender, EventArgs e)
        {
            Visible = false;
            Tables tab = new Tables(0);
            tab.ShowDialog();

            Visible = true;
        }
    }
}
