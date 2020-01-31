using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICategoryInfoView
    {
        ICategoryInfoView CurrentViewContext
        {
            get;
        }

        ComplianceCategoryContract ViewContract
        {
            get;
        }

        ComplianceCategory complianceCategories
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

        Int32 currentCategoryId
        {
            get;
        }

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

        Int32 DefaultTenantId
        {
            get;
            set;
        }

        // <summary>
        /// Gets and sets Package id
        /// </summary>
        Int32 PackageId { get; set; }


        /// Datetime for the start of the compliance category in client screen.
        /// </summary>
        DateTime? PreviousCmplncRqdStartDate { get; set; }

        /// <summary>
        /// Datetime for the end of the compliance category in client screen.
        /// </summary>
        DateTime? PreviousCmplncRqdEndDate { get; set; }

        Boolean PreviousComplianceRequired { get; set; }

        //#region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //List<Entity.SharedDataEntity.UniversalCategory> LstUniversalCategory { get; set; }
        //Int32 SelectedUniversalCategoryID { get; set; }
        //Int32 UniversalCategoryMappingID
        //{
        //    get;
        //    set;
        //}
        //Int32 MappedUniversalCategoryID
        //{
        //    get;
        //    set;
        //}
        //#endregion

        /// <summary>
        /// Get the List of Compliance Packages which are associated with the category
        /// </summary>
        List<CompliancePackage> lstCompliancePackage { get; set; }

        /// <summary>
        /// ',' seperated packageIDs to be passes in storeprocedure for disassociation
        /// </summary>
        String SelectedPackageIDs
        {
            get;
            set;
        }
    }
}




