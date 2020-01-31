using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class ManageServiceAttributeData
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Boolean Active { get; set; }
        public String Lable { get; set; }
        public Int32? MaxLength { get; set; }
        public Int32? MinLength { get; set; }
        public Int32? MaxIntValue { get; set; }
        public Int32? MinIntValue { get; set; }
        public String MaxDateValue { get; set; }
        public String MinDateValue { get; set; }
        public String OptionValues { get; set; }
        public Boolean IsSystmComfigered { get; set; }
        public String AttributeDataType { get; set; }
        public Boolean ShowAllDataInEditForm { get; set; }
        public Boolean IsDisplay { get; set; }
        public Boolean IsHiddenFromUI { get; set; }
        public Boolean IsRequired { get; set; }
        public Int32? ServiceIdToBeUpdated { get; set; }
        public Boolean IsEditable { get; set; }
        public Int32? DataTypeID { get; set; }
        public String AttributeGroupCode { get; set; }
        public String ValidationExpression { get; set; }
        public String ValidationMessage { get; set; }
    }

    public class ServiceAttributeParameter
    {
        public Int32 PackageId { get; set; }
        public Int32 ServiceGroupId { get; set; }
        public Int32 ServiceId { get; set; }
        public Int32 AttributeGroupId { get; set; }
        public Int32 AttributeId { get; set; }
    
    }
}
