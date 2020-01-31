using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System;
using System.Collections.Generic;

namespace CoreWeb.RotationPackages.Views
{
    public interface ICreateCategoryCopyView
    {
        Int32 RequirementCategoryID { get; set; }

        String CategoryName { get; set; }

        String CategoryLabel { get; set; }

        String ErrorMessage { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        String ExplanatoryNotes { get; set; }

        Boolean IsComplianceRequired { get; set; }

        DateTime? ComplianceReqStartDate { get; set; }

        DateTime? ComplianceReqEndDate { get; set; }

        //Commented code related to UAT-2985
        /*List<UniversalCategoryContract> lstUniversalCategory { set; }
       
        Int32 UniversalCategoryID { get; set; }
       
        UniversalCategoryContract UniversalCategoryData { get; set; }*/

        List<RequirementPackageContract> lstRequirementPackage { get; set; }

        String RequirementDocumentLink { get; set; }

        List<Int32> SelectMappedPackages { set; }

        Boolean AllowDatamovement { get; set; } //UAT-2603

        String RequirementDocumentLinkLabel { get; set; } //UAT-3161

        Boolean SendItemDoconApproval { get; set; }//UAT-3805
        //UAT-4165
        Dictionary<String, Boolean> lstEditableBy
        { get; set; }
        Boolean TriggerOtherCategoryRules { get; set; }//UAT-4259

        //UAT-4254//
        List<RequirementCategoryDocUrl> lstReqCatDocUrls { get; set; }
        Boolean IsEditModeOn { get; set; }
        Boolean IsLabelMode { get; set; }
        String SampleDocFormURL { get; set; }
        String NewSampleDocFormURL { get; }
        String NewSampleDocFormUrlDisplayLabel { get; }
        //END 
    }
}
