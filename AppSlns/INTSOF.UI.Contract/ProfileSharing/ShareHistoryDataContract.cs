using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class ShareHistoryDataContract
    {
        public Int32 InvitationGroupId { get; set; }
        public Int32 InvitationId { get; set; }
        public Int32 AgencyId { get; set; }//UAT-2784
        public String AgencyName { get; set; }
        public String InviteeName { get; set; }
        public String SchoolRepresentative { get; set; }
        public String ViewedStatus { get; set; }
        public DateTime? InvitationDate { get; set; }
        public String ExpirationDateOrNumberOfViews { get; set; }
        public String RotationID_Name { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public String RotationDays { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String AgencyReviewStatus { get; set; }
        public String DetailShared { get; set; }
        public Int32 TotalCount { get; set; }
        //UAT-1895 Added two new column in it 

        public String AuditRequestedDate { get; set; }
        public Boolean? IsAuditRequested { get; set; }

        public Boolean AgencyExpirationSetting { get; set; }
    }
}
