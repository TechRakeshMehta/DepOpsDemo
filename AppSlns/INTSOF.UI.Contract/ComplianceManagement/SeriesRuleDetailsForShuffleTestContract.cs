using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class SeriesRuleDetailsForShuffleTestContract
    {
        public Int32 RuleMappingID { get; set; }
        public Int32 RuleMappingDetailID { get; set; }
        public Int32 ConstantTpeID { get; set; }
        public String ConstantTypeCode { get; set; }
        public String ConstantValue { get; set; }
        public Int32 ObjectTypeID { get; set; }
        public String ObjectTypeCode { get; set; }
        public Int32 ObjectID { get; set; }
        public Int32 ObjectMappingTypeID { get; set; }
        public String ObjectMappingTypeCode { get; set; }
        public Int32 ComplianceCategoryID { get; set; }
        public String CategoryName { get; set; }
        public Int32 ComplianceItemID { get; set; }
        public String ItemName { get; set; }
        public Int32 ComplianceAttributeID { get; set; }
        public String AttributeName { get; set; }
        public String ComplianceAttributeDataTypeCode { get; set; }
        public String OptionText { get; set; }
        public String OptionValue { get; set; }
    }
}
