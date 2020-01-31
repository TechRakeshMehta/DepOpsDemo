using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IUniversalComplianceMappingViewView
    {
        Int32 DefaultTenantId
        {
            get;
            set;
        }

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

        List<UniversalComplianceMappingViewContract> lstUniversalComplianceMappingViewContract
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

        List<Entity.SharedDataEntity.UniversalField> lstUniversalAttribute
        {
            get;
            set;
        }

        Int32 CompliancePackageID
        {
            get;
            set;
        }

        Int32 ComplianceFieldID
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

        UniversalComplianceMappingViewContract UpdateContract
        {
            get;
            set;
        }

        Boolean Status
        {
            get;
            set;
        }

        List<Tenant> ListTenants
        {
            get;
            set;
        }

        List<CompliancePackage> ListCompliancePackages
        {
            get;
            set;
        }


        Int32 UniversalAttrMappingID
        {
            get;
            set;
        }
        Int32 UniversalFieldMappingID
        {
            get;
            set;
        }
        Int32 UniversalFieldID
        {
            get;
            set;
        }
        List<InputTypeComplianceAttributeContract> lstInputTypeComplianceAttributeContract
        {
            get;
            set;
        }

        Dictionary<Int32, String> lstUniversalAttributeOptions
        {
            get;
            set;
        }

        Int32 UniversalItemAttrMappingID
        {
            get;
            set;
        }

        List<UniversalComplianceMappingViewContract> lstComplianceAttributeOptions
        {
            get;
            set;
        }
        Int32 ComplianceAttributeID
        {
            get;
            set;
        }
    }
}
