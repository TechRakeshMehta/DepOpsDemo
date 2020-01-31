using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IItemListView
    {
        List<ComplianceItem> lstMasterItems { get; set; }

        ComplianceItemsContract ViewContract { get; }
        /// <summary>
        /// Id of the category to which the item belongs to
        /// </summary>
        Int32 CurrentCategoryId { get; set; }

        /// <summary>
        /// Id of the Item which is to be mapped to a Category
        /// </summary>
        Int32 CurrentItemId { get; set; }

        /// <summary>
        /// OrganizationUserID of the currently logged in user
        /// </summary>
        Int32 CurrentLoggedInUserId { get; }

        Int32 TenantId { get; set; }

        IItemListView CurrentViewContext { get; }
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
       Int32 DefaultTenantId
       {
           get;
           set;
       }

       //#region UAT-2305:Tracking to Rotation category/item/attribute mapping
       //List<Entity.SharedDataEntity.UniversalItem> LstUniversalItem { get; set; }
       //Int32 SelectedUniversalCatItemID { get; set; }
       //Int32 SelectedUniversalCategoryID { get; set; }
       //Int32 MappedUniversalCategoryID
       //{
       //    get;
       //    set;
       //}
       //#endregion
    }
}




