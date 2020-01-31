using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INTSOF.Complio.API
{
    public class TokenValidateResponse
    {
        public String success { get; set; }
        public String txn { get; set; }
        public String message { get; set; }
    }
}