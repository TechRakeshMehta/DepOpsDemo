using INTSOF.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class ManualServiceFormContract
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

        public Int32 SFStatusId
        {
            get;
            set;
        }

        public Int32 NotificationId
        {
            get;
            set;
        }

        public Int32 OrderServiceFormId
        {
            get;
            set;
        }

        public Int32 TotalCount
        {
            get;
            set;
        }

        public String ServiceFormStatus
        {
            get;
            set;
        }

        public String ApplicantAddress
        {
            get;
            set;
        }

        public Int32 ServiceFormId
        {
            get;
            set;
        }

        public String ServiceName
        {
            get;
            set;
        }

        public String SFName
        {
            get;
            set;
        }

        public String SFStatus
        {
            get;
            set;
        }

        public String ApplicantFirstName
        {
            get;
            set;
        }

        public String HierarchyLabel
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

        public Int32? ServiceID
        {
            get;
            set;
        }

        public Int32 DepartmentID
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

        public String ApplicantEmailAddress
        {
            get;
            set;
        }

        public Int32 HierarchyNodeID
        {
            get;
            set;
        }

        public String OrderNumber
        {
            get;
            set;
        }
        //UAT-2165
        public String PackageName
        {
            get;
            set;
        }
        //UAT-2671
        public String ServiceGroupName
        {
            get;
            set;
        }
    }
}

