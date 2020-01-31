using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class ServiceAttributeGroupContract
    {

        public Int32 ServiceAttributeGroupID { get; set; }
        public String ServiceAttributeGroupName { get; set; }
        public String ServiceAttibuteGroupDesc { get; set; }
        public Guid? CopiedFromCode { get; set; }
        public Guid Code { get; set; }
        public Boolean IsEditable { get; set; }
        public Boolean IsRequired { get; set; }
        //public Boolean IsDisplay { get; set; }
        // public Int32? DisplaySequence { get; set; }
        public Boolean IsSystemPreConfigured { get; set; }
        public Boolean IsDeleted { get; set; }
        public Boolean IsActive { get; set; }
        public Int32 CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int32? ModifiedByID { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class MapServiceAttributeToGroupContract
    {
        public Int32 AttributeGroupMappingID { get; set; }
        public Int32 AttributeGroupID { get; set; }
        public Boolean? IsEditable { get; set; }
        public Boolean? IsDisplay { get; set; }
        public Boolean? IsRequired { get; set; }
        public Int32 AttributeID { get; set; }
        public String AttributeName { get; set; }
        public Int32? DisplaySequence { get; set; }
        public Boolean IsDeleted { get; set; }
        public String AttributeDataTypeCode { get; set; }
        public int? SourceAttributeID { get; set; }
        public Boolean? IsHiddenFromUI { get; set; }
    }
}
