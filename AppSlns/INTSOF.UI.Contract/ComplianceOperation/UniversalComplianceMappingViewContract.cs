using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ClinicalRotation
{
    [Serializable]
    public class UniversalComplianceMappingViewContract
    {
        public Int32 CompliancePackageID { get; set; }
        public String CompliancePackageName { get; set; }

        public Int32 ComplianceCategoryID { get; set; }
        public String ComplianceCategoryName { get; set; }

        public Int32 ComplianceCategoryItemID { get; set; }
        public Int32 ComplianceItemID { get; set; }
        public String ComplianceItemName { get; set; }

        public Int32 ComplianceItemAttributeID { get; set; }
        public Int32 ComplianceAttributeID { get; set; }
        public String ComplianceAttributeName { get; set; }

        public Int32 UniversalCategoryID { get; set; }
        public String UniversalCategoryName { get; set; }

        public Int32 UniversalItemID { get; set; }
        public String UniversalItemName { get; set; }

        public Int32 UniversalFieldID { get; set; }
        public String UniversalFieldName { get; set; }


        public Int32 UniversalCatMappingID { get; set; }
        public Int32 UniversalItemMappingID { get; set; }
        public Int32 UniversalAttrMappingID { get; set; }

        public Int32 UniversalCatItemMappingID { get; set; }
        public Int32 UniversalItemAttrMappingID { get; set; }

        //Compliance Mapping
        public Int32 MappedUniversalCategoryID { get; set; }

        //Item Mapping
        public Int32 MappedUniversalCatItemID { get; set; }

        public List<Entity.ClientEntity.UniversalFieldInputTypeMapping> lstUniAttrInputMapping { get; set; }

        public List<Entity.ClientEntity.UniversalFieldOptionMapping> lstUniversalAttributeOptionMapping { get; set; }

        public String ComplianceAttrDataTypeCode { get; set; }

        public Int32 MappedUniversalAttrOptionID { get; set; }

        public Int32 ComplianceAttributeOptionID { get; set; }

        public String ComplianceAttributeOptionText { get; set; }

        public Int32 UniversalFieldMappingID { get; set; }

        public DateTime? UniversalFieldMappingDate { get; set; }
    }
}
