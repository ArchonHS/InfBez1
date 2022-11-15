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
    public partial class UserForm : Form
    {

        Form parentForm;
        string currentUser;
        Restricted restricted = new Restricted();
        public UserForm(LoginForm form, string username)
        {
            InitializeComponent();
            parentForm = form;
            currentUser = username;
        }

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parentForm.Show();
        }
        private void changePasswordButton_Click(object sender, EventArgs e)
        {
            if (Credentials.passwordCorrect(currentUser, oldPasswordTextBox.Text)) 
            {
                if (lastPasswordTextBox.Text == newPasswordTextBox.Text)
                {
                    if (restricted.checkRestricted(currentUser))
                    {
                        if (!restricted.passwordCheck(newPasswordTextBox.Text))
                        {
                            Credentials.changePassword(currentUser, newPasswordTextBox.Text);
                            Credentials.saveUsers();
                            DialogResult res = MessageBox.Show("Пароль успешно изменен, войдите заново с новым паролем", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (res == DialogResult.OK)
                            {
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ваш пароль не соответствует ограничениям, попробуйте заново", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        Credentials.changePassword(currentUser, newPasswordTextBox.Text);
                        Credentials.saveUsers();
                        DialogResult res = MessageBox.Show("Пароль успешно изменен, войдите заново с новым паролем", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (res == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Новые пароли не совпадают", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Старый пароль неправильный", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UserForm_Shown(object sender, EventArgs e)
        {
            if (Credentials.PasswordIsEmpty(currentUser))
            {
                MessageBox.Show("У вас пустой пароль, установите пароль для учетной записи", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
