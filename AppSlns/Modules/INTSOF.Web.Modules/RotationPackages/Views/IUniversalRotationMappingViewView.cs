using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.UI.Contract.ClinicalRotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IUniversalRotationMappingViewView
    {
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        List<UniversalRotationMappingViewContract> lstUniversalRotationMappingViewContract
        {
            get;
            set;
        }

        List<UniversalCategory> lstUniversalCategory
        {
            get;
            set;
        }

        List<UniversalItem> lstUniversalItem
        {
            get;
            set;
        }

        List<UniversalField> lstUniversalAttribute
        {
            get;
            set;
        }

        Int32 RequirementPackageID
        {
            get;
            set;
        }

        Int32 RequirementFieldID
        {
            get;
            set;
        }

        Int32 UniversalCategoryID
        {
            get;
            set;
        }

        Int32 UniversalItemID
        {
            get;
            set;
        }

        UniversalRotationMappingViewContract UpdateContract
        {
            get;
            set;
        }

        Boolean Status
        {
            get;
            set;
        }

        List<RequirementPackage> lstRequirementPackages
        {
            get;
            set;
        }

        Int32 UniversalReqAttrMappingID
        {
            get;
            set;
        }

        List<InputTypeRotationAttributeContract> lstInputTypeRotationAttributeContract
        {
            get;
            set;
        }

        Boolean IsPackageDisabled
        {
            get;
            set;
        }

        String RequirmentFieldDataTypeCode
        {
            get;
            set;
        }

        List<UniversalFieldOption> lstUniversalAttributeOptions
        {
            get;
            set;
        }

        Int32 UniversalItemAttrMappingID
        {
            get;
            set;
        }

        List<UniversalRotationMappingViewContract> lstRequirmentFieldOptions
        {
            get;
            set;
        }
        Int32 UniversalFieldID
        {
            get;
            set;
        }

        Int32 UniversalFieldMappingID
        {
            get;
            set;
        }
    }
}
