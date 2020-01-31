using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class ManageAgencyContract
    {
        public Int32 AgencyID { get; set; }
        public Int32 TenantId { get; set; }
        public String Name { get; set; }
        public String LABEL { get; set; }
        public Int32? AgencyId { get; set; }
        public String Description { get; set; }
        public Int32? ZipCodeID { get; set; }
        public String FullAddress { get; set; }
        public String Address { get; set; }
        public String NpiNumber { get; set; }
        public String SharingStatusCode { get; set; }
        public String AttestationReportText { get; set; }
        public String AgencyHierarchyLabel { get; set; }
        public String AgencyHierarchyRootNodeLabel { get; set; }
        public CustomPagingArgsContract GridCustomPaging { get; set; }       
        public Int32 TotalRecordCount { get; set; }
    }
}
