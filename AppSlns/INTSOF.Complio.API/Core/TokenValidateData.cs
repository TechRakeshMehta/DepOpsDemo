using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INTSOF.Complio.API
{
    public class TokenValidateData
    {
        public String TokenValidateURL { get; set; }
        public String TokenValidateFormat { get; set; }
        public Boolean ValidateToken { get; set; }
        public String ValidateTypeCode { get; set; }
    }
}