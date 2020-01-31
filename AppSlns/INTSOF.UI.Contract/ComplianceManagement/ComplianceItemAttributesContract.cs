using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ComplianceItemAttributesContract
    {
        public Int32 ComplianceItemAttributeId { get; set; }
        public Int32 ComplianceItemId { get; set; }
        public Int32 AttributeTypeId { get; set; }
        public Int32 DataTypeId { get; set; }
        public String Name { get; set; }
        public String AttributeLabel { get; set; }
        public Int32 DisplayOrder { get; set; }
        public Boolean Visible { get; set; }
        public Boolean Required { get; set; }
        public String Description { get; set; }
        public String AttributeType { get; set; }
        public String DataType { get; set; }
        public String AttributeOptions { get; set; }


        public String ExplanatoryNotes { get; set; }
        public String CriteriaMetvalue { get; set; }
        public String DefaultSelectedValue { get; set; }
        public Int32? MaximumCharacters { get; set; }
        public DateTime? MaximumDate { get; set; }
        public Int32? MaximumValue { get; set; }
        public DateTime? MinimumDate { get; set; }
        public Int32? MinimumValue { get; set; }
        public Boolean RequiredValue { get; set; }
        public Boolean VisibleValue { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public String CalculationExpression { get; set; }

        public Int32 ClientComplianceItemAttributeID { get; set; }        
        

    }
}

