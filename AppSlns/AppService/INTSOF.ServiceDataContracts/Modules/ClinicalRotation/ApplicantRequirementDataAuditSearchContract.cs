using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    public class ApplicantRequirementDataAuditSearchContract
    {
        public String CategoryIDs
        {
            get;
            set;
        }

        public String PackageIDs
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

        public String AdminFirstName
        {
            get;
            set;
        }

        public String AdminLastName
        {
            get;
            set;
        }
        public Int32 ItemID
        {
            get;
            set;
        }

        public Int32? FilterUserGroupID
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

        //UAT-3117
        public String ComplioId
        {
            get;
            set;
        }

        public Int32 PackageTypeID
        {
            get;
            set;
        }
    }
}
