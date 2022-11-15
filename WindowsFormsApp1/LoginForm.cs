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
    public partial class LoginForm : Form
    {
        AccessFlags flags = new AccessFlags();
        BlockedUsers blockedUsers = new BlockedUsers();
        Restricted restricted = new Restricted();
        Form initial;
        int entryCount;
        public LoginForm(Form form)
        {
            InitializeComponent();
            initial = form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (Credentials.userExists(username))
            {
                if (Credentials.passwordCorrect(username, password))
                {
                    if (!blockedUsers.checkBlocked(username)) 
                    {
                        if (flags.checkAdmin(username))
                        {
                            var adminForm = new AdminForm(this);
                            adminForm.Show();
                            Hide();
                        }
                        else
                        {
                            var userForm = new UserForm(this, username);
                            userForm.Show();
                            Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ваша учетная запись заблокирована", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                else
                {
                    entryCount--;
                    if (entryCount < 1)
                    {
                        initial.Close();
                    }
                    else
                    {
                        MessageBox.Show(String.Format("Вы ввели неверный пароль, осталось {0} попыток", entryCount), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Clear();
                    }
                }
            }
            else
            {
                MessageBox.Show("Такого пользователя не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_VisibleChanged(object sender, EventArgs e)
        {
            Credentials.loadUsers();
            flags = new AccessFlags();
            blockedUsers = new BlockedUsers();
            restricted = new Restricted();
            entryCount = 3;
            textBox1.Clear();
            textBox2.Clear();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Credentials.saveUsers();
            Credentials.encryptBase();
            initial.Close();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Демушкин А.С. ИДБ-18-06, Варинат 6", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
