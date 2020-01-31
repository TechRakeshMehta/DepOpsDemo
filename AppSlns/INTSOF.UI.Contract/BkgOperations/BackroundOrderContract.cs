using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class BackroundOrderContract
    {
        public Int32 OrganizationUserId
        {
            get;
            set;
        }

        public Int32 OrderID
        {
            get;
            set;
        }

        public String HierarchyLabel
        {
            get;
            set;
        }
        public Int32 HierarchyNodeID
        {
            get;
            set;
        }

        public Boolean DPM_IsEmploymentType //UAT-3429 
        {
            get;
            set;
        }
        public Decimal OrderPrice
        {
            get;
            set;
        }
        public String ApplicantFirstName
        {
            get;
            set;
        }
        public String ApplicantLastName
        {
            get;
            set;
        }
        public String SSN
        {
            get;
            set;
        }
        public String OrderStatus
        {
            get;
            set;
        }
        public String PaymentStatus
        {
            get;
            set;
        }

        public Boolean OrderFlag
        {
            get;
            set;
        }
        public String ClearStarStatus
        {
            get;
            set;
        }
        public Int32 TotalCount
        {
            get;
            set;
        }
        public Int32 OrderClientStatusID
        {
            get;
            set;
        }
        public String OrderClientStatusTypeName
        {
            get;
            set;
        }
        public DateTime? OrderCreatedDate
        {
            get;
            set;
        }
        public DateTime? OrderCompletedDate
        {
            get;
            set;
        }
        public DateTime? DOB
        {
            get;
            set;
        }
        public Boolean? ArchiveStatus
        {
            get;
            set;
        }
        public Int32 InstitutionStatusColorID
        {
            get;
            set;
        }
        public String BkgOrderStatus
        {
            get;
            set;
        }
        public Boolean IsOrderItemsComplete
        {
            get;
            set;
        }
        public String CustomAttributes
        {
            get;
            set;
        }
        public String ManualServiceForms
        {
            get;
            set;
        }
        public String UserGroupNames
        {
            get;
            set;
        }

        public String OrderNote
        {
            get;
            set;
        }

        public String OrderNumber
        {
            get;
            set;
        }

        /// <summary>
        /// UAT:2110, Add Package Name(s) to Background Order Search for client admins
        /// </summary>
        public string BkgPackageNames
        {
            get;
            set;
        }
        public Int32 ApplicantFPImgId
        {
            get;
            set;
        }

        public string ApplicantFPImgPath
        {
            get;
            set;
        }

        public string ApplicantFPImgName
        {
            get;
            set;
        }
    }
}
