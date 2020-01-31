using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceUtil
{
    public interface IDataUploadServiceAdapter
    {
        #region Methods
        String GenerateAuthToken(String serviceConfiguration, String serviceRequestUrl, String className, String assemblyLocation, String authRequestUrl);
        //HttpResponseMessage UploadDataUsingToken(String dataToUpload, String serviceConfiguration, String serviceRequestUrl, String className, String assemblyLocation, String contentType);
        ClientServiceLibrary.ThirdPartyDataUploadResponse UploadClientData(String dataToUpload, String serviceConfiguration, String serviceRequestUrl, String className, String assemblyLocation, String contentType, String authRequestUrl, Boolean isLocalHost, String code, Boolean isIgnoreAceMap, Boolean IsAceMappExceptionThrow, string AceMappErrorCodeAndText);
       
        #endregion
    }
}
