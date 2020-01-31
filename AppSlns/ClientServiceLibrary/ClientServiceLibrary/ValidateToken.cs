using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientServiceLibrary
{
    public class ValidateToken
    {

        public virtual Boolean ValidateClientToken(Int32 schoolId, String entityTypeCode, String mappingCode, String txnToken)
        {
            Boolean isVerifiedRequest = false;
            return isVerifiedRequest;
        }
    }
}
