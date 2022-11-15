using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace WindowsFormsApp1
{
    internal class Restricted
    {
        List<string> restricted = new List<string>(File.ReadLines("../../Restricted.txt"));
        ArrayList allowedCharacters = new ArrayList();
        char[] uppercases = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        char[] lowercases = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        char[] operators = "*/%-+=".ToCharArray();
        public bool passwordCheck(string password)
        {
            char[] passwordChars = password.ToCharArray();
            foreach(char c in passwordChars)
            {
                if (allowedCharacters.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }        
        public bool checkRestricted(string username)
        {
            return restricted.Contains(username);
        }
        public void saveUsers()
        {
            using (StreamWriter file = new StreamWriter("../../Restricted.txt"))
            {
                foreach (var entry in restricted)
                {
                    file.WriteLine(entry);
                }
            }
        }
        public void restrictUser(string username)
        {
            restricted.Add(username);
        }
        public void unrestrictUser(string username)
        {
            restricted.Remove(username);
        }
        public Restricted()
        {
            allowedCharacters.AddRange(lowercases);
            allowedCharacters.AddRange(operators);
            allowedCharacters.AddRange(uppercases);
        }
    }
}
