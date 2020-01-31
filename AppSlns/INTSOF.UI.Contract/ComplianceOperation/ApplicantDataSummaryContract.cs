using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class ApplicantDataSummaryContract
    {
        public Int32 PendingReviewItemCount { get; set; }
        public Int32 ApprovedItemCount { get; set; }
        public Int32 RejectedItemCount { get; set; }
        public String IncompleteCategoryName { get; set; }
    }
}
