using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore_G5
{
    internal class Session
    {
        public static string? Username { get; set; } = null;
        public static int LoggedInStaffId { get; set; }
        public static void Logout()
        {
            Username = null;
            Login login = new Login();
            login.Show();
        }
    }

    
}
