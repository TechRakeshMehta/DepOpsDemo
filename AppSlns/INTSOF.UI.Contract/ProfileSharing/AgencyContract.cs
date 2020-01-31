using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Entity.ClientEntity;
using Entity.SharedDataEntity;

namespace INTSOF.UI.Contract.ProfileSharing
{
    /// <summary>
    /// Contract used to display the data in the Invitation grids for the Applicant and Shared user.
    /// </summary>
    [Serializable]
    public class AgencyContract
    {
        public Int32 AgencyID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String TenantName { get; set; }
        public String Address { get; set; }
        public String NpiNumber { get; set; }
        public Int32 AgencyAddressID { get; set; }
        public Int32 ZipCodeID { get; set; }
        public Int32 LoggedInUserID { get; set; }
        public Boolean IsAdmin { get; set; }
        public Int32 TenantID { get; set; }
        public String FullAddress { get; set; }

        public Int32 SearchStatusID { get; set; }
        public Int32 CreatedByTenantID { get; set; }
        public String SharingStatusCode { get; set; }
        //UAT 1522 WB: there should be a way to have a default attestation statement that would populate in the attestation box when the school is sharing with that agency, for that package
        public String AttestationReportText { get; set; }
        public List<AgencyHierarchyContract> LstAgencyHierarchy { get; set; }

        public List<AgencyPermission> lstAgencyPermission { get; set; }
        //UAT-2640:
        public String Label { get; set; }
        public AgencyHierarchyContract AgencyProfileSharePermission { get; set; }

        public String AgencyHierarchyLabel { get; set; }
        public String AgencyHierarchyRootNodeLabel { get; set; }
    }
}
