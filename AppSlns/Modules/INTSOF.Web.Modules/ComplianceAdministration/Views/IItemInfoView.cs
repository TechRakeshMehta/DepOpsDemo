using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IItemInfoView
    {
        IItemInfoView CurrentViewContext
        {
            get;
        }

        ComplianceItemsContract ViewContract
        {
            get;
            set;
        }

        //ComplianceItem complianceItem
        //{
        //    get;
        //    set;
        //}

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 DefaultTenantId { get; set; }

        Int32 CurrentItemId
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Int32 CurrentCategoryId { get; set; }

        Int32 CurrentPackageId { get; set; }

        //#region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //List<Entity.SharedDataEntity.UniversalItem> LstUniversalItem { get; set; }
        //Int32 SelectedUniversalCatItemID { get; set; }
        //Int32 UniversalItemMappingID
        //{
        //    get;
        //    set;
        //}
        //Int32 MappedUniversalCatItemID
        //{
        //    get;
        //    set;
        //}
        //Int32 SelectedUniversalCategoryID { get; set; }
        //Int32 MappedUniversalCategoryID
        //{
        //    get;
        //    set;
        //}
        //Int32 CategoryItemMappingID
        //{
        //    get;
        //    set;
        //}

        //#endregion


        /// <summary>
        /// UAT-2582 :- Get the List of Compliance Categories which are associated with the Item
        /// </summary>
       List<ComplianceCategory> lstComplianceCategory { get; set; }

        /// <summary>
        /// UAT-2582:- ',' seperated CategoryIDs to be passes in storeprocedure for disassociation
        /// </summary>
        String SelectedCategoryIDs
        {
            get;
            set;
        }

        int ComplianceCategoryItemID { get; set; }
    }
}




