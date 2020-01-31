using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ApplicantDisclosurePresenter : Presenter<IApplicantDisclosureView>
    {
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
            LoadContent();
            // TODO: Implement code that will be executed the first time the view loads
        }

        private void LoadContent()
        {
            if (View.TenantID.IsNotNull() && View.TenantID > 0 && View.DPP_ID > 0)
            {
                DeptProgramPackage depProgramPackage = ComplianceDataManager.GetDeptProgramPackageById(View.DPP_ID, View.TenantID);
                if (depProgramPackage.IsNotNull())
                {
                    String recordType = RecordType.Package.GetStringValue();
                    String websiteWebPageType = WebsiteWebPageType.DisclosureForm.GetStringValue();
                    InstitutionWebPage institutionWebPage = ComplianceSetupManager.GeDateHelpHtmlFromtWebSiteWebPage(View.TenantID, depProgramPackage.DPP_CompliancePackageID, recordType, websiteWebPageType);
                    if (institutionWebPage.IsNotNull() && institutionWebPage.SystemDocument.IsNotNull())
                    {
                        //if (File.Exists(institutionWebPage.SystemDocument.DocumentPath))
                        //{
                        View.SystemDocumentID = institutionWebPage.SystemDocument.SystemDocumentID;
                        View.SystemDocumentIsDeleted = institutionWebPage.SystemDocument.IsDeleted;
                        //}
                    }
                }
            }
        }

        public void GetUploadedDisclosureDocument()
        {
            DeptProgramPackage depProgramPackage = ComplianceDataManager.GetDeptProgramPackageById(View.DPP_ID, View.TenantID);
            if (depProgramPackage.IsNotNull())
            {
                String recordType = RecordType.Package.GetStringValue();
                String websiteWebPageType = WebsiteWebPageType.DisclosureForm.GetStringValue();
                InstitutionWebPage institutionWebPage = ComplianceSetupManager.GeDateHelpHtmlFromtWebSiteWebPage(View.TenantID, depProgramPackage.DPP_CompliancePackageID, recordType, websiteWebPageType);
                if (institutionWebPage != null && institutionWebPage.SystemDocument.IsNotNull())
                {
                    View.DocumentPath = institutionWebPage.SystemDocument.DocumentPath;
                    View.SystemDocumentIsDeleted = institutionWebPage.SystemDocument.IsDeleted;
                }
            }
        }


        public ApplicantDocument SaveEsignedDisclosureDocument(Int32 tenantId, String pdfDocPath, String filename, Int32 fileSize, String documentTypeCode, Int32 currentLoggedInUserId, Int32 orgUserID)
        {
            return ComplianceSetupManager.SaveEsignedDocumentAsPdf(tenantId, pdfDocPath, filename, fileSize, documentTypeCode, currentLoggedInUserId, orgUserID);
        }
        public byte[] FillSignatureInDisclosurePDFDocument(byte[] pdfDocumentDataToBeFilledIn, byte[] imageToAddToDocument, List<SysDocumentFieldMappingContract> LstSpecialFields = null)
        {
            return ComplianceSetupManager.FillSignatureInDisClaimerPDFDocument(pdfDocumentDataToBeFilledIn, imageToAddToDocument, LstSpecialFields);
        }

        public void GetPackageName(Int32 DPP_ID, Int32 tenantId)
        {
            DeptProgramPackage depProgramPackage = ComplianceDataManager.GetDeptProgramPackageById(DPP_ID, tenantId);
            if (depProgramPackage.IsNotNull())
            {
                View.PackageName = depProgramPackage.CompliancePackage.PackageName;
            }
        }

        public Boolean IsDisclosureDocumentRequired()
        {
            Boolean IsDisclosureCheckRequired = true;
            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.DISCLOSURE_DOCUMENT_CHECK_REQUIRED);
            if (appConfiguration.IsNotNull())
            {
                Int32 keyValue = Convert.ToInt32(appConfiguration.AC_Value);
                if (keyValue.Equals(AppConsts.ONE))
                {
                    IsDisclosureCheckRequired = true;
                }
                else
                {
                    IsDisclosureCheckRequired = false;
                }
            }
            return IsDisclosureCheckRequired;
        }

        public String GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            return ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.Disclosure);
        }

        //public byte[] FillAttributesInPdf(List<SysDocumentFieldMappingContract> tempMappings)
        //{
        //    return BackgroundSetupManager.FillDataInPdfForm(tempMappings);
        //}

        public Boolean DeleteFileAmazonS3(String filePath, String fileType = "")
        {
            return CommonFileManager.DeleteDocument(filePath, fileType);
        }
    }
}
