#region Namespaces

#region SystemDefined

using System;
using System.Linq;
using System.Collections.Generic;
using INTSOF.SharedObjects;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Entity.ClientEntity;


#endregion

#endregion
namespace CoreWeb.ComplianceOperations.Views
{
    public class ApplicantDisclaimerPresenter : Presenter<IApplicantDisclaimerView>
    {
        public override void OnViewLoaded()
        {
            
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
           
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId(Int32 currentLoggedInUserId)
        {
            return SecurityManager.GetOrganizationUser(currentLoggedInUserId).Organization.TenantID.Value;
        }
        public ApplicantDocument SaveDisclaimerAsPdf(Int32 tenantId, String pdfDocPath, String filename, Int32 fileSize,String documentTypeCode, Int32 currentLoggedInUserId, Int32 orgUserID)
        {
            return ComplianceSetupManager.SaveEsignedDocumentAsPdf(tenantId, pdfDocPath, filename, fileSize, documentTypeCode, currentLoggedInUserId, orgUserID);
        }
        public byte[] FillSignatureInDisClaimerPDFDocument(byte[] pdfDocumentDataToBeFilledIn, byte[] imageToAddToDocument)
        {
            return ComplianceSetupManager.FillSignatureInDisClaimerPDFDocument(pdfDocumentDataToBeFilledIn, imageToAddToDocument);     
        }
        public void GetPackageName(Int32 DPP_ID, Int32 tenantId)
        {
            DeptProgramPackage depProgramPackage = ComplianceDataManager.GetDeptProgramPackageById(DPP_ID, tenantId);
            if (depProgramPackage.IsNotNull())
            {
                View.PackageName = depProgramPackage.CompliancePackage.PackageName;
            }
        }

        #region UAT-4592
        public bool IsOverrideDisclaimerDocument()
        {
            String disclaimerDocumentOverride = AppConsts.DISCLAIMER_DOCUMENT_OVERRIDE;
            Entity.ClientEntity.AppConfiguration appConfiguration = ComplianceDataManager.GetAppConfiguration(disclaimerDocumentOverride, View.TenantId);

            if (appConfiguration.IsNotNull() && !appConfiguration.AC_Value.IsNullOrEmpty())
            {
                View.DisclaimerDocumentSystemDocumentID = Convert.ToInt32(appConfiguration.AC_Value);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Method to get the Disclosure document
        /// </summary>
        /// <returns></returns>
        public void GetDisclaimerDocumentData()
        {
            if (View.DisclaimerDocumentSystemDocumentID > 0)
            {
                Entity.SystemDocument systemDocument = SecurityManager.GetSystemDocumentByID(View.DisclaimerDocumentSystemDocumentID);
                View.DocumentPath = systemDocument.DocumentPath;
                View.SystemDocumentIsDeleted = systemDocument.IsDeleted;
                //return systemDocument;
            }
            //return null;
        }
        #endregion

    }
}
