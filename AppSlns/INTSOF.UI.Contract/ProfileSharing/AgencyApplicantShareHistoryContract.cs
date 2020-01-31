using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    public class AgencyApplicantShareHistoryContract
    {
        public Int32 ApplicantId { get; set; }
        public Int32 AgencyOrgUserID { get; set; }
        public Int32 TenantId { get; set; }
        public String AgencyName { get; set; }
        public String InvitationDate { get; set; }
        public String ReviewStatus { get; set; }
        public String SharingType { get; set; }
        public Int32 TotalCount { get; set; }
        public Int32 InvitationID { get; set; }
        
        public DateTime? PSIExpirationDate { get; set; }
        public Int32? PSIMaxViews { get; set; }

        public Int32 IsInvSharedByAppByAgencyDDl { get; set; } //UAT-3487        
    }
}
