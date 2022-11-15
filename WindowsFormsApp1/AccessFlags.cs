using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApp1
{
    internal class AccessFlags
    {
        List<string> admins = new List<string>(File.ReadAllLines("../../AccessFlags.txt"));
        public bool checkAdmin(string username)
        {
            return admins.Contains(username);
        }
    }
}
