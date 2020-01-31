using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.SharedObjects;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RequirementItemDataLoaderPresenter : Presenter<IRequirementItemDataLoader>
    {
        public void GetReqItemStatusTypes()
        {
            View.lstReqItemStatusTypes = RequirementVerificationManager.GetRequirementItemStatusTypes(View.TenantId);
        }

        #region UAT-4165
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.CurrentTenantId_Global == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.CurrentTenantId_Global;
            }
        }

        public Boolean IsThirdPartyTenant
        {
            get
            {
                return SecurityManager.IsTenantThirdPartyType(View.CurrentTenantId_Global, TenantType.Compliance_Reviewer.GetStringValue());
            }
        }

        public void IsClientAdmin()
        {
            if (!IsDefaultTenant && !IsThirdPartyTenant)
            {
                View.IsClientAdminLoggedIn = true;
            }
            else
            {
                View.IsClientAdminLoggedIn = false;
            }
        }
        
        #endregion

        #region UAT-2224: Admin access to upload/associate documents on rotation package items.

        /// <summary>
        /// Get applicant documents
        /// </summary>
        public void GetDocuments()
        {
            if (View.SelectedApplicantId_Global > 0 && View.TenantId > 0)
            {
                View.lstApplicantDocument = ApplicantRequirementManager.GetApplicantDocument(View.SelectedApplicantId_Global, View.TenantId);
            }
            else
            {
                View.lstApplicantDocument = new List<ApplicantDocumentContract>();
            }
        }

        /// <summary>
        /// Get Applicant Requirement Item Data
        /// </summary>
        /// <param name="itemId"></param>
        public void GetApplicantRequirementItemData(Int32 itemId)
        {
            if (View.CurrentRequirementPackageSubscriptionID_Global > AppConsts.NONE && itemId > AppConsts.NONE
                && View.CategoryId > AppConsts.NONE && View.TenantId > AppConsts.NONE)
            {
                ApplicantRequirementParameterContract appParameterContract = new ApplicantRequirementParameterContract();
                appParameterContract.RequirementPkgSubscriptionId = View.CurrentRequirementPackageSubscriptionID_Global;
                appParameterContract.RequirementItemId = itemId;
                appParameterContract.RequirementCategoryId = View.CategoryId;
                appParameterContract.TenantId = View.TenantId;

                View.RequirementItemData = ApplicantRequirementManager.GetApplicantRequirementItemData(appParameterContract, View.SelectedApplicantId_Global);
            }
            else
            {
                View.RequirementItemData = new ApplicantRequirementItemDataContract();
            }

        }

        #endregion
    }
}
