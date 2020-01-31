using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IMasterRotationCategoryView
    {
        /// <summary>
        /// Requirement CategoryId
        /// </summary>
        Int32 RequirementCategoryID { get; set; }
        /// <summary>
        /// Requirement Item ID
        /// </summary>
        String RequirementCategoryName { get; set; }

        String ErrorMessage { get; set; }

        Int32 CurrentLoggedInUserId { get; set; }

        Boolean IsEditMode { get; set; }
        List<RequirementCategoryContract> LstCategories { get; set; }
        List<RequirementItemContract> LstCategoryItems { get; set; }


        String RequirementDocumentLink
        {
            get;
            set;
        }

        String ExplanatoryNotes
        {
            get;
            set;
        }


        bool IsViewOnly { get; set; }

        #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        Boolean IsComplianceRequired { get; set; }
        DateTime? ComplianceReqStartDate { get; set; }
        DateTime? ComplianceReqEndDate { get; set; }
        #endregion

        //UAT  2213
        String LstSelectedAgencyIDs
        {
            get;
            set;
        }

        //UAT-3161
        String RequirementDocumentLinkLabel
        {
            get;
            set;
        }

        #region Custom paging parameters

        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        Int32 PageSize
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set;
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        String RequirementCategoryLabel { get; set; }
        List<AgencyDetailContract> lstAgency { get; set; }
        Int32 TenantId { get; set; }

        //List<UniversalCategoryContract> lstUniversalCategory { get; set; }
        //Int32 UniversalCategoryID { get; set; }
        //UniversalCategoryContract UniversalCategoryData { get; set; }
        RequirementCategoryContract ViewContract
        {
            get;
        }
        RequirementCategoryContract SingleCategory { get; set; }
        //UAT-2603/
        Boolean IsAllowDataMovement { get; set; }

        //UAT-4254
        List<RequirementCategoryDocUrl> lstReqCatDocUrls { get; set; }

        //String NewSampleDocFormURL { get; }
        // String NewSampleDocFormUrlDisplayLabel { get; }
        Boolean IsEditModeOn { get; set; }
        Boolean IsLabelMode { get; set; }
        String SampleDocFormURL { get; set; }

        #endregion
    }
}
