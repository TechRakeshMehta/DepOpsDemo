using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class AgencyApplicantStatusContract
    {

        public Int32 AgencyUserId { get; set; }
        public Int32 CurrentLoggedInUser { get; set; }
        public Int32 TenantId { get; set; }
        public String AgencyName { get; set; }
        public Int32? AgencyId { get; set; }
        public String ApplicantName { get; set; }
        public Int32 ApplicantId { get; set; }
        public String SubscriptionStatus { get; set; }
        public String BackgroundCheckStatus { get; set; }
        public String ProfileShareStatus { get; set; }
        public Int32 TotalCount { get; set; }
        public CustomPagingArgsContract GridCustomPaging { get; set; }
        public Int32 RequirementPackageSubscriptionID { get; set; }
        public Int32 CompliancePackageID { get; set; }
        
        
    }
}
