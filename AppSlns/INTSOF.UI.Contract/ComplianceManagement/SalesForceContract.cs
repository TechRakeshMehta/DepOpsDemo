using System;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    /// <summary>
    /// Contract to store the Authentication response from the SalesForcce web service
    /// </summary>
    public class SFAuthenticationResponse
    {
        public String id { get; set; }
        public String issued_at { get; set; }
        public String access_token { get; set; }
        public String instance_url { get; set; }
        public String signature { get; set; }
        public String token_type { get; set; }
    }

    /// <summary>
    /// Contract to store the Response received from SalesForce web service, for updating any record
    /// </summary>
    public class SFUpdateResponse
    {
        public String response { get; set; }
    }
}
