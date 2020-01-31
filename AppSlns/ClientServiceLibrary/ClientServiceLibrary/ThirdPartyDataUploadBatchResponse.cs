using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
namespace ClientServiceLibrary
{
    public class ThirdPartyDataUploadBatchResponse
    {


        public String TPDUId { get; set; }

        public HttpResponseMessage Response { get; set; }

        public String DataXml { get; set; }
    }
}
