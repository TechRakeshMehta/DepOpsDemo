using Entity.SharedDataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ClinicalRotation
{
    [Serializable]
    public class UniversalRotationMappingViewContract
    {
        public Int32 RequirementPackageID { get; set; }
        public String RequirementPackageName { get; set; }

        public Int32 RequirementCategoryID { get; set; }
        public String RequirementCategoryName { get; set; }

        public Int32 RequirementCategoryItemID { get; set; }
        public Int32 RequirementItemID { get; set; }
        public String RequirementItemName { get; set; }

        public Int32 RequirementItemFieldID { get; set; }
        public Int32 RequirementFieldID { get; set; }
        public String RequirementFieldName { get; set; }

        public Int32 UniversalCategoryID { get; set; }
        public String UniversalCategoryName { get; set; }

        public Int32 UniversalItemID { get; set; }
        public String UniversalItemName { get; set; }

        public Int32 UniversalFieldID { get; set; }
        public String UniversalFieldName { get; set; }
        public Int32 UniversalFieldMappingID { get; set; }

        public Int32 UniversalCatItemMappingID { get; set; }
        public Int32 UniversalItemAttrMappingID { get; set; }

        public Int32 UniversalReqCatMappingID { get; set; }
        public Int32 UniversalReqItemMappingID { get; set; }
        public Int32 UniversalReqAttrMappingID { get; set; }

        public Int32 RequirmentFieldOptionID { get; set; }
        public String RequirmentFieldOptionText { get; set; }

        public Int32 MappedUniversalAttrOptionID { get; set; }

        public List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> lstUniReqAttrInputMapping { get; set; }

        public List<Entity.SharedDataEntity.UniversalFieldOptionMapping> lstUniversalRequirementAttributeOptionMapping { get; set; }

        public Boolean IsPackageDisabled { get; set; }

        public String RequirmentFieldDataTypeCode { get; set; }

        public Int32 MappedUniversalFieldID { get; set; }
        public String MappedUniversalFieldName { get; set; }

        public DateTime? UniversalFieldMappingDate { get; set; }

        public List<UniversalRotationMappingOption> lstUniversalRotationMappingOption { get; set; }
    }
    [Serializable]
    public class UniversalRotationMappingOption
    {
        public Int32 MappedUniversalAttrOptionID { get; set; }

        public Int32 RequirmentFieldOptionID { get; set; }

        public String RequirmentFieldOptionText { get; set; }

        public Int32 UniversalFieldID { get; set; }

        public String UniversalFieldName { get; set; }

    }
}
