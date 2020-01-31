using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServiceLibrary
{
    public class TokenValidateDataContract
    {
        public String TokenValidateURL { get; set; }
        public String TokenValidateFormat { get; set; }
        public Boolean ValidateToken { get; set; }
        public String ValidateTypeCode { get; set; }
    }
}
