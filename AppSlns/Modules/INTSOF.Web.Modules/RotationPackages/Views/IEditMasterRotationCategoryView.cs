using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface IEditMasterRotationCategoryView
    {
        Int32 RequirementCategoryID { get; set; }
        Int32 RequirementItemID { get; set; }
        String CategoryName { get; set; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Boolean IsEditMode { get; set; }
        List<RequirementCategoryContract> LstCategories { get; set; }
        List<RequirementItemContract> LstCategoryItems { get; set; }
        Boolean IsVersionRequired { get; set; }
        Int32 RequirementPackageId { get; set; }
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

        #region UAT-2305

        //List<UniversalCategoryContract> lstUniversalCategory { set; }

        //Int32 UniversalCategoryID { get; set; }

        //UniversalCategoryContract UniversalCategoryData { get; set; }
        //UAT-2213
        String CategoryLabel { get; set; }

        #endregion

        //UAT-2603
        Boolean IsAllowDataMovement { get; set; }
        //UAT-3161
        String RequirementDocumentLinkLabel { get; set; }
        Boolean SendItemDocOnApproval { get; set; }

        Dictionary<String, Boolean> lstEditableBy
        { get; set; }

        Boolean TriggerOtherCategoryRules { get; set; }

        //Added in UAT-4254 in release 181//
        List<RequirementCategoryDocUrl> lstReqCatDocUrls { get; set; }
        String NewSampleDocFormURL { get; }
        String NewSampleDocFormUrlDisplayLabel { get; }
        Boolean IsEditModeOn { get; set; }
        Boolean IsLabelMode { get; set; }
        String SampleDocFormURL { get; set; }

        //UAT-4657
        Dictionary<Int32, String> PackagesDataAssociatedWithCategory { get; set; }
        Boolean IsDetailsEditable { get; set; }

    }
}
