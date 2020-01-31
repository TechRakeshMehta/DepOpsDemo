using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class DAndRAttributeGroupMappingContract
    {
        public String FieldName { get; set; }
        public String AttributeGroup { get; set; }
        public String Attribute { get; set; }
        public Int32? ID { get; set; }
        public Int32? SvcAttGroupID { get; set; }
        public Int32? SvcAttrID { get; set; }
        public Int32? AttributeGroupMappingID { get; set; }
        public Int32? SpecialFieldTypeID { get; set; }
        public String SpecialFieldTypeName { get; set; }
        public Int32 TenantID { get; set; }
        public Int32 CustomAttributeID { get; set; }
        public Boolean IsApplicantAttribute { get; set; }
        public Boolean IsSpecialAttribute { get; set; } 
        public Boolean IsCustomAttribute { get; set; }
        public String TenantName { get; set; }
        public String CustomAttributeName { get; set; }
    }
}
