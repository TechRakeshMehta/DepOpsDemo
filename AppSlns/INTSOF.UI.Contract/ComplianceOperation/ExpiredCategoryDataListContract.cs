using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class ExpiredCategoryDataListContract
    {
        public Int32 ApplicantComplianceCategoryID
        {
            get;
            set;
        }

        public Int32 ApplicantComplianceItemID
        {
            get;
            set;
        }

        public Int32 ComplianceItemID
        {
            get;
            set;
        }

        public String CategoryName
        {
            get;
            set;
        }

        public DateTime ExpiryDate
        {
            get;
            set;
        }

        public Int32 ComplianceCategoryId
        {
            get;
            set;
        }

        public Int32 CompliancePackageId
        {
            get;
            set;
        }

        public Int32 OrgUserId
        {
            get;
            set;
        }

        public String PrimaryEmailaddress
        {
            get;
            set;
        }

        public String UserFirstName
        {
            get;
            set;
        }

        public String UserLastName
        {
            get;
            set;
        }

        public String NodeHierarchy
        {
            get;
            set;
        }

        public Int32 ComplianceStatusId
        {
            get;
            set;
        }

        public String ComplianceStatus
        {
            get;
            set;
        }

        public Int32 PackageSubscriptionID
        {
            get;
            set;
        }

        public Int32 HierarchyNodeID
        {
            get;
            set;
        }

        public String ComplianceItemCode
        {
            get;
            set;
        }

        public Boolean IsApproveByOverrideDisableStatus
        {
            get;
            set;
        }
    }
}
