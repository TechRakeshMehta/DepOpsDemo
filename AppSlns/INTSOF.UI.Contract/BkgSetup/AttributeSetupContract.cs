using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract
{
    public class AttributeSetupContract
    {
        public String AttributeGroupName
        {
            get;
            set;
        }
        public String AttributeGroupDescription
        {
            get;
            set;
        }
        public String  IsSystemPreConfigured
        {
            get;
            set;
        }
        public Int32  AttributeGroupID
        {
            get;
            set;
        }
        public String IsEditable
        {
            get;
            set;
        }

        public Int32 BkgPackageSvcId
        {
            get;
            set;
        }
        public Int32 BkgPackageSvcAttributeMappingId
        {
            get;
            set;
        }

        public String AttributeName
        {
            get;
            set;
        }
        public String AttributeDescription
        {
            get;
            set;
        }
        public String AttributeLabel
        {
            get;
            set;
        }
        public String AttributeDataType
        {
            get;
            set;
        }
        public Boolean Active
        {
            get;
            set;
        }
        public Boolean Required
        {
            get;
            set;
        }

        public Boolean IsDisplay
        {
            get;
            set;
        }

        public Int32 AttributeID
        {
            get;
            set;
        }
        public Int32 BkgAttributeGroupMappingId
        {
            get;
            set;
        }
        public Int32 DisplayOrder
        {
            get;
            set;
        }
        public String  AttributeGroupCode
        {
            get;
            set;
        }
        public Boolean IsHiddenFromUI
        {
            get;
            set;
        }
    }

    public class AttributeDataSecurityClient 
    {
        public Int32 AttributeId { get; set; }
        public String AttributeName { get; set; }
    }
}
