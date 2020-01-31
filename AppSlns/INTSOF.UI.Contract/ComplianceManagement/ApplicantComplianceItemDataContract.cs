using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ApplicantComplianceItemDataContract
    {
        public Int32 ApplicantComplianceItemId { get; set; }
        public Int32 ApplicantComplianceCategoryId { get; set; }
        public Int32 ComplianceItemId { get; set; }
        public Int32 ReviewStatusTypeId { get; set; }
        public String ReviewStatusTypeCode { get; set; }
        public String Notes { get; set; }
        public Int32 ApplicantId { get; set; }

        public String ApplicantName { get; set; }
        //UAT-3355
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }

        public DateTime? SubmissionDate { get; set; }
        public String ItemName { get; set; }
        public String VerificationStatus { get; set; }
        public String SystemStatus { get; set; }
        public String PackageName { get; set; }
        public String CategoryName { get; set; }
        public String AssignedUserName { get; set; }
        public String ExceptionReason { get; set; }
        public String RushOrderStatus { get; set; }

        public List<String> FilterColumns { get; set; }
        public List<String> FilterOperators { get; set; }
        public ArrayList FilterValues { get; set; }
        public List<String> FilterTypes { get; set; }

        public Int32  CategoryId { get; set; }
        public Int32  PackageSubscriptionId{ get; set; }
        public String VerificationStatusCode { get; set; }
        public Int32? HierarchyNodeID { get; set; }
        public Int32? ReviewLevel { get; set; }
        public String CustomAttributes { get; set; }
        public Boolean IsUiRulesViolate { get; set; }
    }

    [Serializable]
    public class VerificationQueueFiltersContract
    {
        public Int32 TenantId { get; set; }
        public Int32 UserGroupId { get; set; }
        public Int32 PackageID { get; set; }
        public Int32 CategoryId { get; set; }
        public Boolean ShowIncompleteItems { get; set; }
        public Boolean ShowRushOrders { get; set; }
        public Int32 VerAssignToUserID { get; set; }
        public Int32 ExpAssignToUserID { get; set; }
        public WorkQueueType WorkQueueType { get; set; }

        public List<String> FilterColumns { get; set; }
        public List<String> FilterOperators { get; set; }
        public ArrayList FilterValues { get; set; }
        public List<String> FilterTypes { get; set; }

        #region Custom Attributes

        public Int32? NodeId
        {
            get;
            set;
        }

        public Int32? DPM_Id
        {
            get;
            set;
        }

        /// <summary>
        /// CSV DPMID's of the multiple selected nodes - UAT 1055
        /// </summary>
        public String SelectedDPMIds
        {
            get;
            set;
        }

        /// <summary>
        /// CSV NodeID's of the multiple selected nodes - UAT 1055
        /// </summary>
        public String NodeIds
        {
            get;
            set;
        }

        public String CustomFields
        {
            get;
            set;
        }

        public String NodeLabel
        {
            get;
            set;
        }


        #endregion

    }
}
