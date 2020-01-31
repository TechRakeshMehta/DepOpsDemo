using System;
using System.Collections.Generic;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    public class BkgOrderReviewQueueContract
    {
        public Int32 ClientID { get; set; }
        public Int32 OrderPackageSvcGrpID { get; set; }
        public Int32 TotalCount { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public Int32? OrderID
        {
            get;
            set;
        }
        public String SvcgrpName { get; set; }
        public String HierarchyLabel { get; set; }
        public DateTime? OrderCreatedDate { get; set; }

        //public Int32? SvcGrpReviewStatusTypeID
        //{
        //    get;
        //    set;
        //}

        public List<Int32?> SvcGrpReviewStatusTypeIDs
        {
            get;
            set;
        }

        public Int32? SvcGrpStatusTypeID
        {
            get;
            set;
        }

        public Int32? SelectedReviewCriteriaId
        {
            get;
            set;
        }

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

        public DateTime? SvcGrpUpdatedFromDate
        {
            get;
            set;
        }
        public DateTime? SvcGrpUpdatedToDate
        {
            get;
            set;
        }

        public Boolean? IsServiceGroupFlagged
        {
            get;
            set;
        }

        public String SvcGrpReviewStatusCode
        {
            get;
            set;
        }
        public String SvcGrpReviewStatusName
        {
            get;
            set;
        }
        public String SvcGrpStatusCode
        {
            get;
            set;
        }
        public String SvcGrpStatusName
        {
            get;
            set;
        }
        public DateTime? SvcGrpLastUpdatedDate { get; set; }
        public String CustomAttributes { get; set; }
        public String ReviewCriteria { get; set; }

        public Int32? LoggedInUserId
        {
            get;
            set;
        }

        //public Int32? DeptProgramMappingID
        //{
        //    get;
        //    set;
        //}

        public String DeptProgramMappingIDs
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
        //Changes related to UAT-1683
        public String SelectedArchiveStateCode
        {
            get;
            set;
        }

        public String OrderNumber
        {
            get;
            set;
        }
        public Int32 SupplementAutomationStatusID
        {
            get;
            set;
        }

        //UAT-2304: Random review of auto completed supplements
        public String SvcGrpReviewType { get; set; }

        public Int32 CurrentPageIndex { get; set; }
    }
}

