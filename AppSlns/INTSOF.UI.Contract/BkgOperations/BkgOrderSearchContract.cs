using INTSOF.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class BkgOrderSearchContract
    {
        public Int32 ClientID
        {
            get;
            set;
        }

        public Int32 PackageID
        {
            get;
            set;
        }

        public String ApplicantFirstName
        {
            get;
            set;
        }

        public String ApplicantMiddleName
        {
            get;
            set;
        }

        public String ApplicantLastName
        {
            get;
            set;
        }

        public Int32 ProgramID
        {
            get;
            set;
        }
        public Int32? InstitutionStatusColorID
        {
            get;
            set;
        }
        public Int32? NodeId
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
        public DateTime? PaidFromDate
        {
            get;
            set;
        }
        public DateTime? PaidToDate
        {
            get;
            set;
        }
        public DateTime? OrderCompletedFromDate
        {
            get;
            set;
        }
        public DateTime? OrderCompletedToDate
        {
            get;
            set;
        }
        public Boolean? IsArchive
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

        public Int32? DPM_Id
        {
            get;
            set;
        }
        public Int32? OrderPaymentStatusID
        {
            get;
            set;
        }
        public Int32? OrderStatusTypeID
        {
            get;
            set;
        }
        public Int32? OrderStatusID
        {
            get;
            set;
        }
        public Int32? ServiceID
        {
            get;
            set;
        }
        public DateTime? DateOfBirth
        {
            get;
            set;
        }

        public Int32? ServiceGroupId
        {
            get;
            set;
        }
        public Int32? OrderClientStatusID
        {
            get;
            set;
        }
        public Boolean IsServiceGroupRequired
        {
            get;
            set;
        }
        public List<String> FilterColumns
        {
            get;
            set;
        }

        public List<String> FilterOperators
        {
            get;
            set;
        }

        public ArrayList FilterValues
        {
            get;
            set;
        }

        public List<String> LstStatusCode
        {
            get;
            set;
        }

        public Int32 DepartmentID
        {
            get;
            set;
        }



        public Int32? OrderID
        {
            get;
            set;
        }

        public Boolean? IsFlagged
        {
            get;
            set;
        }

        public Int32? OrganizationUserId
        {
            get;
            set;
        }

        public String ApplicantSSN
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
        public Int32? ServiceFormStatusID
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

        public Int32? DeptProgramMappingID
        {
            get;
            set;
        }
        public String DeptProgramMappingIDs
        {
            get;
            set;
        }

        public Int32? FilterUserGroupID
        {
            get;
            set;
        }

        public Int32? MatchUserGroupID
        {
            get;
            set;
        }


        public DateTime? FromDate
        {
            get;
            set;
        }

        public DateTime? ToDate
        {
            get;
            set;
        }

        public Int32? LoggedInUserId
        {
            get;
            set;
        }

        public Int32? LoggedInUserTenantId
        {
            get;
            set;
        }


        // Changes done for "Use SP for Compliance Item Data Search"                
        public String XMLStatusCodes
        {
            get;
            set;
        }
        public Boolean IsPaymentStatusChecked
        {
            get;
            set;

        }
        public Boolean IsClientStatusChecked
        {
            get;
            set;
        }
        public Boolean IsFlaggedChecked
        {
            get;
            set;
        }
        public Boolean IsArchiveChecked
        {
            get;
            set;
        }

        public Int32? UserGroupID
        {
            get;
            set;
        }

        public String SelectedArchieveStateId
        {
            get;
            set;
        }

        public String SelectedFlagged
        {
            get;
            set;
        }

        public String NodeIds
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
