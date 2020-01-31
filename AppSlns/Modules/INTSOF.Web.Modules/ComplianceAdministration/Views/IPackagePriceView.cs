using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IPackagePriceView
    {
        List<Entity.ClientEntity.PriceAdjustment> ListPriceAdjustment { get; set; }

        List<PriceContract> ListDeptProgramPackagePriceAdjustment { get; set; }

        //List<DeptProgramPackagePriceAdjustment> ListDeptProgramPackagePriceAdjustment { get; set; }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        PriceContract ViewContract
        {
            get;
        }

        /// <summary>
        /// Gets the current UserId
        /// </summary>
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// Gets the TenantId
        /// </summary>
        Int32 TenantId { get; set; }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId { get; set; }

        /// <summary>
        /// Gets the Error Message
        /// </summary>
        String ErrorMessage { get; set; }
        
        String TreeNodeType { get; set; }
        String TreeNodeValue { get; set; }
        Decimal Price { get; set; }
        Decimal TotalPrice { get; set; }
        Decimal? RushOrderAdditionalPrice { get; set; }
        Decimal PriceAdjustmentValue { get; set; }
        Int32 SelectedPriceAdjustmentID { get; set; }

        //Int32 DeptProgramPackageSubscriptionID { get; set; }
        //Int32 DeptProgramPackageID { get; set; }

        Int32 ID { get; set; }
        Int32 ParentID { get; set; }
        Int32 MappingID { get; set; }
        Int32 ParentSubscriptionID { get; set; }
        Int32 ComplianceCategoryID { get; set; }
        Boolean IsPriceDisabled { get; set; }
        Boolean IsShowMessage { get; set; }
        String PermissionCode { get; set; }

        int ItemID { get; set; }

        int ItmCatID { get; set; }

        int ItmSubsID { get; set; }
    }
}




