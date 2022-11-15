using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class Credentials
    {
        static Dictionary<string, string> credentials = new Dictionary<string, string>();
        private static readonly string PASS_PHRASE = "admin"; 
        public static bool userExists(string username)
        {
            return credentials.ContainsKey(username);
        }
        public static bool passwordCorrect(string username, string inputPassword)
        {
            string correctPassword = credentials[username];
            return correctPassword == inputPassword;
        }
        public static void saveUsers()
        {
            List<User> convert = new List<User>();
            foreach(var entry in credentials)
            {
                convert.Add(new User(entry.Key, entry.Value));
            }
            string json = JsonConvert.SerializeObject(convert, Formatting.Indented);
            File.WriteAllText("../../Credentials_temp.json", json);
        }
        public static void loadUsers()
        {
            credentials.Clear();
            var data = File.ReadAllText("../../Credentials_temp.json");
            try
            {
                List<User> users = JsonConvert.DeserializeObject<List<User>>(data);

                foreach (var user in users)
                {
                    credentials.Add(user.Username, user.Password);
                }
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                DialogResult res = MessageBox.Show("Шифр нарушен, программа завершит работу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (res == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }
        public static void decryptBase()
        {
            using(StreamWriter sw = new StreamWriter("../../Credentials_temp.json"))
            {
                if (CheckEmptiness("../../Credentials.json"))
                {
                    CreateAdminCredentials(sw);
                }
                else
                {
                    sw.WriteLine(CryptoProvider.decryptFile(File.ReadAllText("../../Credentials.json"), PASS_PHRASE));
                }
            }
        }
        public static void encryptBase()
        {
            using (StreamWriter sw = new StreamWriter("../../Credentials.json"))
            {
                sw.WriteLine(CryptoProvider.encryptFile(File.ReadAllText("../../Credentials_temp.json"), PASS_PHRASE));
            }
            File.Delete("../../Credentials_temp.json");
        }
        public static List<string> getUsers()
        {
            AccessFlags access = new AccessFlags();
            List<string> users = new List<string>();
            foreach(string key in credentials.Keys)
            {
                if (!access.checkAdmin(key))
                {
                    users.Add(key);
                };
            }
            return users;
        }
        public static bool PasswordIsEmpty(string username)
        {
            /*if(credentials[username] == "")
            {
                return true;
            }
            else
            {
                return false;
            }*/
            return credentials[username] == "";
        }
        public static void changePassword(string username, string newPassword)
        {
            credentials[username] = newPassword;
        }
        public static void addNewUser(string username)
        {
            credentials.Add(username, "");
        }
        private static void CreateAdminCredentials(StreamWriter sw)
        {
            User admin = new User("admin", "admin");
            List<User> temp = new List<User> { admin };
            string json = JsonConvert.SerializeObject(temp, Formatting.Indented);
            sw.WriteLine(json);
        }
        private static bool CheckEmptiness(string filepath)
        {
            string[] containements = File.ReadAllLines(filepath);
            return containements.Length == 0;
        }
    }
}
