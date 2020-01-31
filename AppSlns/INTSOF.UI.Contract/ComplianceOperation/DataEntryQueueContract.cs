using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class DataEntryQueueContract
    {
        public Int32 FDEQ_ID { get; set; }
        public Int32 DocumentStatusID { get; set; }
        public String DocumentStatusCode { get; set; }
        public String DocumentStatusName { get; set; }
        public Int32 ApplicantDocumentID { get; set; }
        public String DocumentName { get; set; }
        public Int32 AssignToUserID { get; set; }
        public String AssignToUserName { get; set; }
        public Int32 TenantID { get; set; }
        public Int32 ApplicantOrganizationUserID { get; set; }
        public String ApplicantName { get; set; }
        public DateTime? DateUploaded { get; set; }
        public DateTime? DateUploadDatePart { get; set; }
        public Int32 TotalCount { get; set; }
        //UAT :- 2499
        public String TenantName { get; set; }
        //UAT-2456:
        public Int32 DiscardDocumentCount { get; set; }
    }

    [Serializable]
    public class DataEntryQueueFilterContract
    {
        #region  Properties

        #region Sort options

        /// <summary>
        /// Get or set grid sort expression (if any)
        /// </summary>
        public String SortExpression
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set grid sort direction, true if descending 
        /// </summary>
        public Boolean SortDirectionDescending
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set grid default sort expression (if SortExpression is not found)
        /// </summary>
        public String DefaultSortExpression
        {
            get;
            set;
        }

        public String SecondarySortExpression
        {
            get;
            set;
        }

        #endregion

        #region Paging Options

        /// <summary>
        /// Get or set current page index of grid
        /// </summary>
        public Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set page size of grid
        /// </summary>
        public Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set virtual page count of grid
        /// </summary>
        public Int32 VirtualPageCount
        {
            get;
            set;
        }

        #endregion

        #region Filter option

        /// <summary>
        /// Get or set filter fields name
        /// </summary>
        public List<String> FilterColumns
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set filter operators name
        /// </summary>
        public List<String> FilterOperators
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set filter values
        /// </summary>
        public ArrayList FilterValues
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set filter types
        /// </summary>
        public List<String> FilterTypes
        {
            get;
            set;
        }

        #endregion

        public List<Int32> SelectedTenantIds { get; set; }
        public String QueueType { get; set; }

        public String InstituteHierarchyLabel { get; set; }
        public String DepartmntPrgrmMppngIds { get; set; }
        #endregion
    }
    [Serializable]
    public class PackageSubscriptionForDataEntry
    {
        public String PackageName { get; set; }
        public String InstitutionHierarchy { get; set; }
        public Int32 OrderID { get; set; }
        public Int32 PackageSubscriptionID { get; set; }
    }
}
