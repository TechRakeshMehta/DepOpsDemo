using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    public class ApplicantBackgroundSummaryContract
    {
        public Int32 OrderID { get; set; }
        public String DPM_Label { get; set; }
        public Boolean? IsServiceGroupFlagged { get; set; }
        public String ServiceGroupName { get; set; }
        public String SvcGrpReviewStatusName { get; set; }
        public String SvcGrpStatusName { get; set; }
        public String OrderNumber { get; set; }
        public String SvcGrpFlaggedStatusImgPath { get; set; }
        public String svcGroupFlaggedStatusAltText { get; set; }
        public Int32 RecentlyCompletedOrderID { get; set; }
        public Int32 OrdPkgSvcGroupID { get; set; }

    }
}
