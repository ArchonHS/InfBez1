using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApp1
{
    internal class BlockedUsers
    {
        List<string> blocked = new List<string>(File.ReadAllLines("../../BlockedUsers.txt"));
        public bool checkBlocked(string username)
        {
            return blocked.Contains(username);
        }
        public void saveUsers()
        {
            using (StreamWriter file = new StreamWriter("../../BlockedUsers.txt"))
            {
                foreach (var entry in blocked)
                {
                    file.WriteLine(entry);
                }
            }
        }
        public void blockUser(string username)
        {
            blocked.Add(username);
        }
        public void unblockUser(string username)
        {
            blocked.Remove(username);
        }
    }
}
