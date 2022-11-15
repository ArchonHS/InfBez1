using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class NewPassword : Form
    {
        Form initial;
        public NewPassword(Form form)
        {
            InitializeComponent();
            initial = form;
            initial.Hide();
        }

        private void NewPassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            initial.Close();
        }
    }
}
