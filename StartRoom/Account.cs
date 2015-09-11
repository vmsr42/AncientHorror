using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartRoom
{
    public static class Account
    {
        static Account()
        {
            YouAreLeader = false;
        }

        public static string AccountName { get; set; }

        public static bool YouAreLeader { get; set; }
    }
}
