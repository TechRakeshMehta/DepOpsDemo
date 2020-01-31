using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
namespace ClientServiceLibrary
{
   public class ThirdPartyDataUploadResponse
    {
        public String ReposnseXML { get; set; }

        public List<ThirdPartyDataUploadBatchResponse> ThirdPartyBatchResponse { get; set; }
    }
}
