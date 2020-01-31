using INTSOF.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class ManualServiceFormsSearchContract
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
        public Int32? ServiceID
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

        public Int32? OrganizationUserId
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
        
        public Int32? DeptProgramMappingID
        {
            get;
            set;
        }

        public String SelectedDeptProgramMappingID
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

    }
}
