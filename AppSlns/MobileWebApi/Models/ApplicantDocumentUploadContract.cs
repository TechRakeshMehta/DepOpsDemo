using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MobileWebApi.Models
{
    [DataContract]
    public class ApplicantDocumentUploadContract
    {
        [DataMember]
        public String DocumentDescription { get; set; }
        [DataMember]
        public HttpPostedFile Document { get; set; }
    }

    [DataContract]
    public class ApplicantDocumentUploadResponse
    {
        [DataMember]
        public String SuccessMessage { get; set; }
        [DataMember]
        public String DuplicateFilesCount { get; set; }
    }
}