using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class ManageServiceContract
    {
        public Int32 ServiceID { get; set; }
        public String ServiceName { get; set; }
        public String ServiceDesc { get; set; }
        //public Boolean Active { get; set; }
        public Int32 TenantID { get; set; }
        public Int32 ServiceTypeID { get; set; }
        public Int32? ParentServiceID { get; set; }
        public Boolean ShowPackageCount { get; set; }
        public Boolean ShowResidenceYears { get; set; }
        public Boolean ShowMaxOcuurence { get; set; }
        public Boolean ShowMinOcuurence { get; set; }
        public Boolean ShowSendDocument { get; set; }
        public Boolean ShowIsSupplemental { get; set; }
        public Boolean ShowIgnoreResidentialHistory { get; set; }

        //UAT-1728: Create ability to add cofigurable text to the result report (and flagged only and service group reports) by service. 
        public String ConfigurableServiceText { get; set; }
    }


    public class ManageServiceAttributeGrpContract
    {
        public Int32 ServiceattGrpID { get; set; }
        public String ServiceAttGrpName { get; set; }
        public String ServiceAttGrpDesc { get; set; }

        public Boolean? IsEditable { get; set; }
        public Int32 AttributeID { get; set; }
        public String AttributeName { get; set; }
    }

    public class ManageServiceCustomFormContract
    {
        public Int32 CustomFormID { get; set; }
        public String CustomFormName { get; set; }
        public String CustomFormDesc { get; set; }
        public Boolean IsEditable { get; set; }
        public Boolean? IsSystemPreConfigured { get; set; }
        public Int32 SequenceOrder { get; set; }
        public Int32 SvcFormMappingID { get; set; }

    }
}
