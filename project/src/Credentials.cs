using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.src
{
    internal class Credentials
    {
        public string username { get; set; }
        public string password { get; set; }
        public string fingerprint { get; set; }

        public Credentials(string username, string password, string fingerprint)
        {
            this.username = username;
            this.password = password;
            this.fingerprint = fingerprint;
        }
    }
}
