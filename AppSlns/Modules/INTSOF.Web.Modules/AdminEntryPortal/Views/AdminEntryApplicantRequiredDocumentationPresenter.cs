using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AdminEntryPortal.Views
{
    public class AdminEntryApplicantRequiredDocumentationPresenter : Presenter<IAdminEntryApplicantRequiredDocumentaionView>
    {
        public byte[] FillSignatureInDisclosurePDFDocument(byte[] pdfDocumentDataToBeFilledIn, byte[] imageToAddToDocument, List<SysDocumentFieldMappingContract> LstSpecialFields = null)
        {
            return ComplianceSetupManager.FillSignatureInDisClaimerPDFDocument(pdfDocumentDataToBeFilledIn, imageToAddToDocument, LstSpecialFields);
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
            return ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.RequiredDocumentation);
        }

        public Boolean DeleteFileAmazonS3(String filePath, String fileType = "")
        {
            return CommonFileManager.DeleteDocument(filePath, fileType);
        }
    }
}
