using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface ISetupRequirementFieldView
    {
        /// <summary>
        /// Requirement FieldId
        /// </summary>
        Int32 RequirementFieldID { get; set; }
        /// <summary>
        /// Requirement Item ID
        /// </summary>
        Int32 RequirementItemID { get; set; }
        //UAT-4380
        Boolean IsCalculatedAttribute { get; }
        String ItemHId { get; set; }
        Boolean IsCustomSettings { get; set; }
        Dictionary<String, Boolean> lstEditableBy { get; set; }

        String ErrorMessage { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        Boolean IsEditMode { get; set; }
        List<RequirementFieldContract> lstItemFields { get; set; }
        Boolean IsVersionRequired { get; set; }

        /// <summary>
        /// Set or get Field Name.
        /// </summary>
        String FieldName { get; set; }
        List<RotationFieldDataTypeContract> LstRotationFieldDataType { get; set; }

        Boolean IsDocumentSaved { get; }

        List<String> LstDocumentFieldCodes { get; }

        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        RequirementFieldContract FieldData { get; set; }

       
        List<RequirementFieldType> lstFieldType { get; set; }

        List<RequirementAttributeGroups> lstAttributeGroups { get; set; }
        /// <summary>
        /// Selected Package ID
        /// </summary>
        Int32 SelectedPackageID { get; set; }

        #region UAT-2305

        Int32 UniFieldID { get; set; }
        Int32 UniFieldMappingID { get; set; }
        List<UniversalAttributeContract> lstUniversalAttributes { get; set; }

        UniversalAttributeContract UniversalAttributeData { get; set; }

        List<InputTypeComplianceAttributeServiceContract> lstSelectedInputAttribute { get; set; }

        #endregion

        #region UAT-2402
        Dictionary<Int32, String> lstAttributeOptionValue { get; set; }
        Dictionary<Int32, String> lstRequirementFieldOptionValue { get; set; }
        #endregion

        #region UAT-2213
        Boolean IsNewPackage { get; set; }
        Int32 RequirementCategoryId { get; set; }
        #endregion

        #region Rules
        List<RequirementRuleContract> lstItemRules
        {
            get;
            set;
        }
        Int32 RequirementObjectTreeID { get; set; }
        #endregion

        //UAT-4657
        Boolean IsDetailsEditable { get; set; }
    }
}
