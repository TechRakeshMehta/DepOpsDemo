using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICategoryListView
    {
        ICategoryListView CurrentViewContext
        {
            get;
        }

        ComplianceCategoryContract ViewContract
        {
            get;
        }

        List<ComplianceCategory> complianceCategories
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

        Int32 selectedCategoryId
        {
            get;
            set;
        }

        Int32 PackageId { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
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
        /// <summary>
        /// Gets and sets DisplayOrder 
        /// </summary>
        Int32 DisplayOrder
        {
            get;
            set;
        }

        //#region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //List<Entity.SharedDataEntity.UniversalCategory> LstUniversalCategory { get; set; }
        //Int32 SelectedUniversalCategoryID { get;  }
        //#endregion
    }
}




