using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServiceLibrary
{
   public class SFTPConfiguration
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public String HostName { get; set; }
        public String PortNumber { get; set; }
        public String RemotePath { get; set; }
        public String SshHostKeyFingerprint { get; set; }
        public String AcceptAnySSHHostKey { get; set; }
    }
}
