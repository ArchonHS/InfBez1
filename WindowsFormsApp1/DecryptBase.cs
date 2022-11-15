using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class DecryptBase : Form
    {
        static readonly string PASS_PHRASE = "admin";
        public DecryptBase()
        {
            InitializeComponent();
        }

        private void decryptBaseButton_Click(object sender, EventArgs e)
        {
            if(passPhraseTextBox.Text == PASS_PHRASE)
            {
                Credentials.decryptBase();
                var loginForm = new LoginForm(this);
                loginForm.Show();
                Hide();
            }
            else
            {
                DialogResult res = MessageBox.Show("Неверная парольная фраза, работа будет завершена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if(res == DialogResult.OK)
                {
                    this.Close();
                }
            }
        }
    }
}
