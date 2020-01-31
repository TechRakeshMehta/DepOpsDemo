using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.RotationPackages
{
    public class RequirementRuleData
    {
        public Int32 Rps_Id { get; set; }
        public Int32 PackageId { get; set; }
        public Int32 CategoryId { get; set; }
        public Int32 ItemId { get; set; }
        public Int32 ApplicantUserID { get; set; }
        public Int32 SystemUserId { get; set; }
        public Boolean IsNewPackage { get; set; }
        public Int32 FieldId { get; set; }
        public Int32 ApplicantRequirementItemDataID { get; set; }
    }

    public class RequirementRuleObjectTree
    {
        public String RuleObjectTypeId { get; set; }
        public String RuleObjectTypeCode { get; set; }
        public String RuleObjectId { get; set; }
        public String RuleObjectParentId { get; set; }
    }

    public class RequirementExpressionData
    {
        public String CateogryName { get; set; }
        public Int32 CategoryID { get; set; }
        public Int32 RequirementObjectCategoryID { get; set; }

        public String ItemName { get; set; }
        public Int32 ItemID { get; set; }
        public Int32 RequirementObjectItemID { get; set; }

        public String AttributeName { get; set; }
        public Int32 AttributeID { get; set; }
        public Int32 RequirementObjectAttributeID { get; set; }
        public String AttributeDataTypeName { get; set; }
    }
}
