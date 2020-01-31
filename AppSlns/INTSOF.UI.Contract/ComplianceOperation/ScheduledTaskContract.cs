using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class ScheduledTaskContract
    {
        public Int32 OrderId
        {
            get;
            set;
        }
        public Int32 PackageId
        {
            get;
            set;
        }
        public Int32 ScheduleTaskId
        {
            get;
            set;
        }
        public Boolean IsApprovalEmailSent
        {
            get;set;
        }
        public Int32 OrganisationUserId
        {
            get;
            set;
        }

        public String ReferenceNumber
        {
            get;
            set;
        }

        public DateTime ExpiryDate
        {
            get;
            set;
        }

        public String OrderStatusCode
        {
            get;
            set;
        }

        public Int32 ApprovedBy
        {
            get;
            set;
        }

        public String ApprovedDate
        {
            get;
            set;
        }

        public String SubEventCode
        {
            get;
            set;
        }

        public String ReportName
        {
            get;
            set;
        }

        public String FileExtension
        {
            get;
            set;
        }

        public Int32 CutOffTime
        {
            get;
            set;
        }

        public Int32 OrderPaymentDetailId
        {
            get;
            set;
        }

        public String DaysOfWeeks
        {
            get;
            set;
        }
    }
}
