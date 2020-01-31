using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using Entity;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageDownloadPdfPresenter : Presenter<IManageDownloadPdfView>
    {

        public override void OnViewLoaded()
        {
          
        }

        public override void OnViewInitialized()
        {
            LoadContent();
        }

        public Int32 GetTenantId()
        {
            var tenant = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserID).Organization.Tenant;
            return tenant.TenantID;
        }
        private void LoadContent()
        {
            if (View.TenantID.IsNotNull() && View.PackageID.IsNotNull())
            {
                String recordType = RecordType.Package.GetStringValue();
                String websiteWebPageType = WebsiteWebPageType.DataEntryHelp.GetStringValue();
               Entity.ClientEntity.InstitutionWebPage institutionWebPage = ComplianceSetupManager.GeDateHelpHtmlFromtWebSiteWebPage(View.TenantID, View.PackageID, recordType, websiteWebPageType);
                if (institutionWebPage != null)
                {
                    View.PageHTML = institutionWebPage.HtmlMarkup;
                }
            }
        }
        public Boolean SavePageHtmlContentLocation(String filePath, Guid Id,String fileName)
        {
            TempFile tempFile = new TempFile();
            tempFile.TF_Path = filePath;
            tempFile.TF_Identifier = Id;
            tempFile.TF_IsDeleted = false;
            tempFile.TF_CreatedOn = DateTime.Now;
            tempFile.TF_CreatedByID = View.CurrentLoggedInUserID;
            tempFile.TF_FileName = fileName;
            return SecurityManager.SavePageHtmlContentLocation(tempFile);
        }
    }
}




