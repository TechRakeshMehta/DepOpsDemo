using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class ItemDocNotificationRequestDataContract
    {
        public Int32 TenantID { get; set; }
        public String CategoryIds { get; set; }
        public Int32 ApplicantOrgUserID { get; set; }
        public String ApprovedCategoryIds { get; set; }
        public String RequestTypeCode { get; set; }
        public Int32? PackageSubscriptionID { get; set; }
        public Int32? RPS_ID { get; set; }
        public Int32 CurrentLoggedInUserID { get; set; }
    }
}
