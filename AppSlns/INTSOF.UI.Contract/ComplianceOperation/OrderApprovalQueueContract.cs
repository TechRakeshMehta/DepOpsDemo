using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class OrderApprovalQueueContract
    {
        public Int32 ClientID { get; set; }
        public Int32 TotalCount { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String SSN { get; set; }
        public String OrderStatusCode { get; set; }
        public String PaymentTypeCode { get; set; }
        public String OrderPackageTypeCode { get; set; }

        public List<String> LstStatusCode
        {
            get;
            set;
        }

        public List<String> LstPaymentType
        {
            get;
            set;
        }

        public List<DateTime> LstOrderCreatedDate
        {
            get;
            set;
        }

        public List<DateTime> LstOrderPaidDate
        {
            get;
            set;
        }
        
        /// <summary>
        /// Codes of the Order types i.e. Compliance, Background or Both
        /// </summary>
        public List<String> lstOrderPackageTypes
        {
            get;
            set;
        }
        
        public String NodeLabel
        {
            get;
            set;
        }

        public Int32? OrderID
        {
            get;
            set;
        }

        public String HierarchyLabel { get; set; }
        

        public DateTime? OrderFromDate
        {
            get;
            set;
        }

        public DateTime? OrderToDate
        {
            get;
            set;
        }

        public DateTime? OrderPaidFromDate
        {
            get;
            set;
        }

        public DateTime? OrderPaidToDate
        {
            get;
            set;
        }

        public DateTime? SvcGrpUpdatedFromDate
        {
            get;
            set;
        }

        public String SvcGrpReviewStatusCode
        {
            get;
            set;
        }

        public String SvcGrpStatusCode
        {
            get;
            set;
        }

        public Int32? LoggedInUserId
        {
            get;
            set;
        }
        
        public String DeptProgramMappingIDs
        {
            get;
            set;
        }

        public Boolean? ShowOnlyRushOrder
        {
            get;
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPagingArguments
        {
            get;
            set;
        }

        public String OrderNumber
        {
            get;
            set;
        }
    }
}


