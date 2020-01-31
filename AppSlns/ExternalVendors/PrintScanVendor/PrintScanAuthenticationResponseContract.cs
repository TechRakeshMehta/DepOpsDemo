using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalVendors.PrintScanVendor
{
    public class PrintScanAuthenticationResponseContract
    {
        public String access_token { get; set; }
        public String token_type { get; set; }
        public Int32 expires_in { get; set; }
    }
}
