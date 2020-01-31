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
    public class ApplicantRequiredDocumentationPresenter : Presenter<IApplicantRequiredDocumentationView>
    {
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
        }

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
