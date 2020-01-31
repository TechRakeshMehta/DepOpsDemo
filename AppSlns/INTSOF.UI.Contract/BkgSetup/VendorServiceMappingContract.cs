using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class VendorServiceMappingContract
    {
        //Vendor Service Mapping Properties
        public Int32 BSESM_ID { get; set; }
        public Guid BSESM_Code { get; set; }
        public Int32 BSESM_CreatedBy { get; set; }
        public DateTime BSESM_CreatedOn { get; set; }
        public Int32? BSESM_ModifiedBy { get; set; }
        public DateTime? BSESM_ModifiedOn { get; set; }
        public Boolean IsEditable { get; set; }

        //Background Service Properties
        public Int32 BSE_ID { get; set; }
        public String BSE_Name { get; set; }
        public String BSE_Description { get; set; }

        //External Service Properties
        public Int32 EBS_ID { get; set; }
        public String EBS_Name { get; set; }
        public String EBS_Description { get; set; }
        public String EBS_ExternalCode { get; set; }
        public Guid EBS_Code { get; set; }

        //External Vendor properties
        public Int32 EVE_ID { get; set; }
        public String EVE_Name { get; set; }
        public String EVE_Description { get; set; }
        public String EVE_Code { get; set; }

        //Vendor Service Attribute Mapping
        //public List<ExternalSvcAtributeMapping> lst
    }

    public class VendorServiceAttributeMappingContract
    {
        public Int32 ESAM_ID { get; set; }
        public Int32 ESAM_BkgSvcAttributeGroupMappingID { get; set; }
        public Int32 ESAM_ExternalBkgSvcAttributeID { get; set; }
        public Int32 ESAM_ServiceMappingId { get; set; }
        public Int32 ESAM_CreatedBy { get; set; }
        public DateTime ESAM_CreatedOn { get; set; }
        public String ESAM_FieldDelimiter { get; set; }
        public Int32 EBSA_FieldID { get; set; }
        public String EBSA_Label { get; set; }
        public String EBSA_LocationField { get; set; }
        public String EBSA_DefaultValue { get; set; }
        public String BSA_Name { get; set; }
        public String BSA_Description { get; set; }
        public String BSAD_Name { get; set; }
        public Int32? ESAM_ModifiedBy { get; set; }
        public DateTime? ESAM_ModifiedOn { get; set; }
        public Boolean IsComplex { get; set; }
        public List<Int32> BkgSvcAttMappingIDs { get; set; }
        public List<ExternalServiceAttribute> ExtSvcAttList { get; set; }
        public Boolean IsRequired { get; set; }
        public Boolean ExternalIsRequired { get; set; }
    }

    public class ServiceAttributesContract
    {
        public List<ExternalServiceAttribute> ExternalServiceAttributeList {get;set;}
        public List<InternalServiceAttribute> InternalServiceAttributeList { get; set; }
    }

    public class ExternalServiceAttribute
    {
        public Int32 ExtSvcAttributeID { get; set; }
        public String ExtSvcAttributeName { get; set; }
        public Int32? ExtSvcAttributeMappingID { get; set; }
        public String BkgSvcAttributeName { get; set; }
        public Int32? BkgSvcAttributeGroupMappingID { get; set; }
        public Int32? FieldSequence { get; set; }
        public Int32? FormatTypeID { get; set; }
    }

    public class InternalServiceAttribute
    {
        public Int32 BSAGM_ID { get; set; }
        public String BSA_Name { get; set; }
    }
}
