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
    public partial class AdminForm : Form
    {
        Form parentForm;
        BlockedUsers blockedUsers = new BlockedUsers();
        Restricted restricted = new Restricted();
        public AdminForm(LoginForm form)
        {
            InitializeComponent();
            parentForm = form;
            listBox1.DataSource = Credentials.getUsers();
        }
        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parentForm.Show();
        }
        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            string username = (string)listBox1.SelectedItem;
            //отработка по блокировке
            if (blockedUsers.checkBlocked(username))
            {
                label3.Text = "Да";
            }
            else
            {
                label3.Text = "Нет";
            }
            if (blockedUsers.checkBlocked(username))
            {
                blockUserButton.Text = "Разблокировать";
            }
            else
            {
                ;
                blockUserButton.Text = "Заблокировать";
            }
            //отработка по ограничением
            if (restricted.checkRestricted(username))
            {
                label4.Text = "Да";
            }
            else
            {
                label4.Text = "Нет";
            }
            if (restricted.checkRestricted(username))
            {
                restrictPasswordButton.Text = "Отменить ограничение";
            }
            else
            {
                ;
                restrictPasswordButton.Text = "Ограничить пароли";
            }

        }
        private void blockUserButton_Click(object sender, EventArgs e)
        {
            string username = (string)listBox1.SelectedItem;
            if (blockedUsers.checkBlocked(username))
            {
                blockedUsers.unblockUser(username);
                blockUserButton.Text = "Заблокировать";
                label3.Text = "Нет";
            }
            else
            {
                blockedUsers.blockUser(username);
                blockUserButton.Text = "Разблокировать";
                label3.Text = "Да";
            }
            blockedUsers.saveUsers();
        }
        private void addNewUserButton_Click(object sender, EventArgs e)
        {
            if (!Credentials.userExists(newUserTextBox.Text))
            {
                Credentials.addNewUser(newUserTextBox.Text);
                listBox1.DataSource = Credentials.getUsers();
                Credentials.saveUsers();
            }
            else
            {
                MessageBox.Show("Такой пользователь уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void changePasswordButton_Click(object sender, EventArgs e)
        {
            if (Credentials.passwordCorrect("admin", oldPasswordTextBox.Text))
            {
                if (lastPasswordTextBox.Text == newPasswordTextBox.Text)
                {
                    Credentials.changePassword("admin", newPasswordTextBox.Text);
                    Credentials.saveUsers();
                    MessageBox.Show("Пароль успешно изменен", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Старый пароль неверен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void restrictPasswordButton_Click(object sender, EventArgs e)
        {
            string username = (string)listBox1.SelectedItem;
            if (restricted.checkRestricted(username))
            {
                restricted.unrestrictUser(username);
                restrictPasswordButton.Text = "Ограничить пароли";
                label4.Text = "Нет";
            }
            else
            {
                restricted.restrictUser(username);
                restrictPasswordButton.Text = "Отменить ограничение";
                label4.Text = "Да";
            }
            restricted.saveUsers();
        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Credentials.saveUsers();
        }
    }
}
