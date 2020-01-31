using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MobileWebApi
{
    [DataContract]
    public class TenantContract
    {
        [DataMember]
        public Int32 TenantId { get; set; }
        [DataMember]
        public String TenantName { get; set; }
        [DataMember]
        public String TenantUrl { get; set; }
    }

    [DataContract]
    public class ApplicantAccountDetails
    {
        [DataMember]
        public OrganizationUser organizationUserContract { get; set; }
        [DataMember]
        public Boolean IsUserAuthenticated { get; set; }
        [DataMember]
        public String ResponseMessage { get; set; } 

        [DataMember]
        public String UserRole { get; set; }
    }

    public class ApiBaseResponse
    {
        public String token { get; set; }
        public String refresh_token { get; set; }
    }

     [DataContract]
    public class LanguageContract
    {
        [DataMember]
        public int LanguageID { get; set; }

        [DataMember]
        public string LanguageName { get; set; }

        [DataMember]
        public string LanguageCode { get; set; }

        [DataMember]
        public string LanguageCulture { get; set; }
    }

    [DataContract]
    public class ApiResourceContract
    {
        [DataMember]
        public String Key { get; set; }

        [DataMember]
        public String value { get; set; }
    }

    [DataContract]
    public class LanguageTranslatedContract
    {
        [DataMember]
        public List<ApiResourceContract> lstApiResourceContract { get; set; }
        [DataMember]
        public String languageCode { get; set; }
    }

}