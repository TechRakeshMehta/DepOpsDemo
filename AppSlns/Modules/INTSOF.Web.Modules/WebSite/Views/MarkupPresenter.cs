#region NameSpaces

#region System Defined
using INTSOF.SharedObjects;
using System.Linq;
using System;
#endregion

#region Project Specific
using Business.RepoManagers;
using INTSOF.Utils;
using Entity;
using System.Collections.Generic;
using INTSOF.UI.Contract.ComplianceOperation;
#endregion

#endregion


namespace CoreWeb.WebSite.Views
{
    public class MarkupPresenter : Presenter<IMarkupView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IWebSiteController _controller;
        // public MarkupPresenter([CreateNew] IWebSiteController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            GetTanantName();
            GetWebsiteUrl();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads

            View.LoadWebsites(WebSiteManager.GetWebSites());
            View.LoadContentType(EnumList.Of<ContentType>());
            //LoadWebPages();
            LoadContent();
        }

        /// <summary>
        /// Loads web pages
        /// </summary>
        public void LoadWebPages()
        {
            switch (View.SelectedContentType)
            {
                case ContentType.WEBPAGE:
                    View.LoadWebSiteWebPage(WebSiteManager.GetWebSiteWebPages(View.WebSiteId));
                    break;

                case ContentType.HEADER:
                case ContentType.FOOTER:
                    View.LoadWebSiteWebConfig(WebSiteManager.GetWebSiteWebConfigs(View.WebSiteId));
                    View.WebSiteWebConfigId = WebSiteManager.GetWebSiteWebConfigs(View.WebSiteId).FirstOrDefault().WebSiteWebConfigID;
                    break;
            }
            LoadContent();
        }

        /// <summary>
        /// Load content
        /// </summary>
        public void LoadContent()
        {
            View.WebSiteWebConfigId = WebSiteManager.GetWebSiteWebConfigs(View.WebSiteId).FirstOrDefault().WebSiteWebConfigID;
            switch (View.SelectedContentType)
            {
                case ContentType.WEBPAGE:
                    {

                        WebSiteWebPage webSiteWebPage = WebSiteManager.GetWebSiteWebPage(View.WebSiteWebPageId);
                        if (webSiteWebPage.IsNotNull())
                        {
                            View.LinkText = webSiteWebPage.LinkText;
                            View.LinkOrder = webSiteWebPage.LinkOrder;
                            View.PageTitle = webSiteWebPage.PageName;
                            View.LinkPosition = webSiteWebPage.LinkPosition;
                            View.IsActive = webSiteWebPage.IsActive;
                            View.Content = webSiteWebPage.HtmlMarkup;
                            View.MasterPage = webSiteWebPage.MasterPage;
                        }
                        else
                        {
                            View.MasterPage = WebSiteManager.GetWebSiteWebConfigByConfigID(View.WebSiteWebConfigId).DefaultMasterPage;
                        }
                    }
                    break;
                case ContentType.HEADER:
                    WebSiteWebConfig webSiteWebConfig = WebSiteManager.GetWebSiteWebConfigByConfigID(View.WebSiteWebConfigId);
                    View.Content = webSiteWebConfig.HeaderHtml; //WebSiteManager.GetWebSiteWebConfigByConfigID(View.WebSiteWebConfigId).HeaderHtml;
                    View.MasterPage = webSiteWebConfig.DefaultMasterPage;
                    break;
                case ContentType.FOOTER:
                    //View.FooterText = WebSiteManager.GetWebSiteWebConfigByConfigID(View.WebSiteWebConfigId).FooterText;
                    WebSiteWebConfig webSiteWebConfigTemp = WebSiteManager.GetWebSiteWebConfigByConfigID(View.WebSiteWebConfigId);
                    View.FooterText = webSiteWebConfigTemp.FooterText; //WebSiteManager.GetWebSiteWebConfigByConfigID(View.WebSiteWebConfigId).HeaderHtml;
                    View.MasterPage = webSiteWebConfigTemp.DefaultMasterPage;
                    break;
                case ContentType.APPLICANTDATAENTRYHELP:
                    View.Content = null;
                    break;
                case ContentType.DISCLOSUREFORM:
                    View.Content = null;
                    break;
            }
        }

        /// <summary>
        /// Save WebsiteContent
        /// </summary>
        public void SaveWebSiteContent()
        {
            WebSiteWebConfig webSiteWebConfig;

            switch (View.SelectedContentType)
            {
                case ContentType.WEBPAGE:
                    if (View.WebSiteWebPageId == 0)
                    {
                        //String defaultMasterPage = WebSiteManager.GetWebSiteWebConfigs(View.WebSiteId).FirstOrDefault().DefaultMasterPage;
                        WebSiteWebPage webSiteWebPage = new WebSiteWebPage();
                        Int32 websiteWebPageTypeId = WebSiteManager.GetWebsiteWebPageTypeIdByCode(WebsiteWebPageType.Other.GetStringValue());
                        webSiteWebPage.HtmlMarkup = View.Content;
                        webSiteWebPage.LinkOrder = View.LinkOrder;
                        webSiteWebPage.WebSiteID = View.WebSiteId;
                        webSiteWebPage.LinkText = View.LinkText;
                        webSiteWebPage.LinkPosition = View.LinkPosition;
                        webSiteWebPage.PageName = View.PageTitle;
                        // webSiteWebPage.MasterPage = defaultMasterPage;
                        webSiteWebPage.MasterPage = View.MasterPage;
                        webSiteWebPage.IsActive = View.IsActive;
                        webSiteWebPage.WebSiteWebPageTypeID = websiteWebPageTypeId;
                        webSiteWebPage.CreatedByID = View.CurrentUserId;
                        webSiteWebPage.CreatedOn = DateTime.Now;
                        if (WebSiteManager.SaveWebSitePage(webSiteWebPage))
                        {
                            View.WebSiteWebPageId = webSiteWebPage.WebSiteWebPageID;
                            View.SuccessMessage = "Page content saved successfully.";
                        }
                        else
                        {
                            View.ErrorMessage = "Some error occured.Please try again.";
                        }
                    }
                    else
                    {
                        WebSiteWebPage webSiteWebPage = WebSiteManager.GetWebSiteWebPage(View.WebSiteWebPageId);
                        webSiteWebPage.HtmlMarkup = View.Content;
                        webSiteWebPage.LinkOrder = View.LinkOrder;
                        webSiteWebPage.LinkText = View.LinkText;
                        webSiteWebPage.LinkPosition = View.LinkPosition;
                        webSiteWebPage.PageName = View.PageTitle;
                        webSiteWebPage.IsActive = View.IsActive;
                        //
                        webSiteWebPage.ModifiedByID = View.CurrentUserId;
                        webSiteWebPage.ModifiedOn = DateTime.Now;

                        if (WebSiteManager.UpdateWebPageHtml(webSiteWebPage))
                        {
                            View.SuccessMessage = "Page content updated successfully.";
                        }
                        else
                        {
                            View.ErrorMessage = "Some error occured.Please try again.";
                        }
                    }
                    break;
                case ContentType.HEADER:
                    webSiteWebConfig = WebSiteManager.GetWebSiteWebConfigByConfigID(View.WebSiteWebConfigId);
                    webSiteWebConfig.HeaderHtml = View.Content;
                    //
                    webSiteWebConfig.ModifiedByID = View.CurrentUserId;
                    webSiteWebConfig.ModifiedOn = DateTime.Now;
                    if (WebSiteManager.UpdateWebSiteWebConfig(webSiteWebConfig))
                    {
                        View.SuccessMessage = "Header content updated successfully.";
                    }
                    else
                    {
                        View.ErrorMessage = "Some error occured.Please try again.";
                    }
                    break;
                case ContentType.FOOTER:
                    webSiteWebConfig = WebSiteManager.GetWebSiteWebConfigByConfigID(View.WebSiteWebConfigId);
                    //if(!View.FooterText.IsNullOrEmpty())
                    webSiteWebConfig.FooterText = View.FooterText;
                    //
                    webSiteWebConfig.ModifiedByID = View.CurrentUserId;
                    webSiteWebConfig.ModifiedOn = DateTime.Now;
                    if (WebSiteManager.UpdateWebSiteWebConfig(webSiteWebConfig))
                    {
                        View.SuccessMessage = "Footer content updated successfully.";
                    }
                    else
                    {
                        View.ErrorMessage = "Some error occured.Please try again.";
                    }
                    break;
                case ContentType.APPLICANTDATAENTRYHELP:
                    SaveUpdateDataEntryHelpAndDisclosureFormContent(RecordType.Package.GetStringValue());
                    break;
                case ContentType.DISCLOSUREFORM:
                    SaveUpdateDataEntryHelpAndDisclosureFormContent(RecordType.Package.GetStringValue());
                    break;
            }
        }

        /// <summary>
        /// Method to Get and Set the Tenant Name in View Property.
        /// </summary>
        public void GetTanantName()
        {
            View.TenantName = SecurityManager.GetTenant(View.TenantID).TenantName;
        }

        /// <summary>
        /// get website Url.
        /// </summary>
        public void GetWebsiteUrl()
        {
            View.SiteUrl = WebSiteManager.GetWebSiteDetail(View.TenantID).URL;
        }

        /// <summary>
        /// Get CompliancePackage list
        /// </summary>
        public void GetCompliancePackage()
        {
            List<Entity.ClientEntity.CompliancePackage> tempPackageList = new List<Entity.ClientEntity.CompliancePackage>();
            if (View.TenantID != AppConsts.NONE)
            {
                String webSiteWebPageType = String.Empty;
                tempPackageList = View.AllPackageList;
                if (View.SelectedContentType == ContentType.APPLICANTDATAENTRYHELP)
                {
                    webSiteWebPageType = WebsiteWebPageType.DataEntryHelp.GetStringValue();
                }
                else if (View.SelectedContentType == ContentType.DISCLOSUREFORM)
                {
                    webSiteWebPageType = WebsiteWebPageType.DisclosureForm.GetStringValue();
                }
                List<Int32> tempExistingPackageIdList = new List<Int32>();
                //tempPackageList.Insert(0, new Entity.ClientEntity.CompliancePackage { PackageName = "Default", CompliancePackageID = -1 });
                tempExistingPackageIdList = ComplianceSetupManager.GetExistingPackageIdList(View.TenantID, RecordType.Package.GetStringValue(), webSiteWebPageType);
                if (View.SelectedContentOperation == AppConsts.ONE)
                {
                    tempPackageList = tempPackageList.Where(x => !tempExistingPackageIdList.Contains(x.CompliancePackageID)).ToList();
                }
                else
                {
                    tempPackageList = tempPackageList.Where(x => tempExistingPackageIdList.Contains(x.CompliancePackageID)).ToList();
                }

                tempPackageList.Insert(0, new Entity.ClientEntity.CompliancePackage { PackageName = "--Select--", CompliancePackageID = 0 });
                View.lstCompliancePackage = tempPackageList;
            }
            else
            {
                tempPackageList.Insert(0, new Entity.ClientEntity.CompliancePackage { PackageName = "--Select--", CompliancePackageID = 0 });
                View.lstCompliancePackage = tempPackageList;
            }
        }

        /// <summary>
        /// Get Content on the baisi of package Id.
        /// </summary>
        public void GetDataEntryHelpContentByPackage()
        {
            if (View.SelectedPackageId != AppConsts.NONE)
            {
                String webSiteWebPageType = String.Empty;
                Int32? selectedPackageId = null;
                String recordType = RecordType.Package.GetStringValue();
                if (View.SelectedContentType == ContentType.APPLICANTDATAENTRYHELP)
                {
                    webSiteWebPageType = WebsiteWebPageType.DataEntryHelp.GetStringValue();
                }
                else if (View.SelectedContentType == ContentType.DISCLOSUREFORM)
                {
                    webSiteWebPageType = WebsiteWebPageType.DisclosureForm.GetStringValue();
                }
                if (View.SelectedPackageId != -1)
                {
                    selectedPackageId = View.SelectedPackageId;
                }
                Entity.ClientEntity.InstitutionWebPage institutionWebPage = ComplianceSetupManager.GetDataEntryHelpContentByPackageId(View.TenantID, selectedPackageId, recordType, webSiteWebPageType);
                if (institutionWebPage.IsNotNull())
                {
                    View.Content = institutionWebPage.HtmlMarkup;
                    View.WebSiteWebPageId = institutionWebPage.InstitutionWebPageID;
                }
                else
                {
                    View.Content = null;
                    View.WebSiteWebPageId = AppConsts.NONE;
                }
            }
            else
            {
                View.Content = null;
                View.WebSiteWebPageId = AppConsts.NONE;
            }
        }

        /// <summary>
        /// Get all the packages.
        /// </summary>
        public void GetAllPackageList()
        {
            if (View.TenantID != AppConsts.NONE)
            {
                List<Entity.ClientEntity.CompliancePackage> tempAllPackageList = ComplianceSetupManager.GetCompliancePackage(View.TenantID, false).ToList();
                tempAllPackageList.Insert(0, new Entity.ClientEntity.CompliancePackage { PackageName = "--For all undefined--", CompliancePackageID = -1 });
                View.AllPackageList = tempAllPackageList;
            }
        }

        private void SaveUpdateDataEntryHelpAndDisclosureFormContent(String recordType)
        {
            String successMessage = String.Empty;
            //Int32? documentId = null;
            if (View.WebSiteWebPageId == 0)
            {
                String webSiteWebPageType = String.Empty;
                String pageName = String.Empty;

                if (View.SelectedContentType == ContentType.APPLICANTDATAENTRYHELP)
                {
                    webSiteWebPageType = WebsiteWebPageType.DataEntryHelp.GetStringValue();
                    pageName = "Data Entry Help - " + View.SelectedPackageName;
                    successMessage = "Applicant Data Entry Help content saved successfully for ";
                }
                else if (View.SelectedContentType == ContentType.DISCLOSUREFORM)
                {
                    webSiteWebPageType = WebsiteWebPageType.DisclosureForm.GetStringValue();
                    pageName = "Disclosure Form - " + View.SelectedPackageName;
                    View.Content = null;
                    //DocumentId
                    successMessage = "Disclosure document saved successfully for ";
                }
                Int32 websiteWebPageTypeId = ComplianceSetupManager.GetWebsiteWebPageTypeIdByCode(webSiteWebPageType, View.TenantID);
                Int16 recordTypeId = ComplianceSetupManager.GetRecordTypeIdByCode(recordType, View.TenantID);
                Entity.ClientEntity.InstitutionWebPage institutionWebPage = new Entity.ClientEntity.InstitutionWebPage();
                institutionWebPage.TenantID = View.TenantID;
                institutionWebPage.PageName = pageName;
                institutionWebPage.HtmlMarkup = View.Content;
                institutionWebPage.IsActive = true;
                institutionWebPage.WebSiteWebPageTypeID = websiteWebPageTypeId;
                institutionWebPage.IsDeleted = false;
                institutionWebPage.CreatedByID = View.CurrentUserId;
                institutionWebPage.CreatedOn = DateTime.Now;
                institutionWebPage.RecordTypeID = recordTypeId;
                if (!View.SelectedPackageId.Equals(AppConsts.NONE) && View.SelectedPackageId != -1)
                {
                    institutionWebPage.RecordID = View.SelectedPackageId;
                }
                if ((View.SelectedContentType == ContentType.DISCLOSUREFORM) && View.SystemDocumentToSaveUpdate.IsNotNull())
                {
                    institutionWebPage.SystemDocument = View.SystemDocumentToSaveUpdate;
                }
                if (ComplianceSetupManager.SaveWebSitePage(institutionWebPage, View.TenantID))
                {
                    //View.WebSiteWebPageId = webSiteWebPage.WebSiteWebPageID;
                    View.SuccessMessage = successMessage + View.SelectedPackageName + ".";
                }
                else
                {
                    View.ErrorMessage = "Some error occured.Please try again.";
                }
            }
            else
            {
                if (View.SelectedContentType == ContentType.APPLICANTDATAENTRYHELP)
                {
                    successMessage = "Applicant Data Entry Help content updated successfully for ";
                    //}
                    //else if (View.SelectedContentType == ContentType.DISCLOSUREFORM)
                    //{
                    //    successMessage = "Disclosure document updated successfully for ";
                    //}
                    Entity.ClientEntity.InstitutionWebPage institutionWebPage = ComplianceSetupManager.GetInstitutionWebPage(View.WebSiteWebPageId, View.TenantID);
                    institutionWebPage.HtmlMarkup = View.Content;
                    institutionWebPage.ModifiedByID = View.CurrentUserId;
                    institutionWebPage.ModifiedOn = DateTime.Now;
                    //if ((View.SelectedContentType == ContentType.DISCLOSUREFORM) && View.SystemDocumentToSaveUpdate.IsNotNull())
                    //{
                    //    institutionWebPage.SystemDocument.FileName = View.SystemDocumentToSaveUpdate.FileName;
                    //    institutionWebPage.SystemDocument.DocumentPath = View.SystemDocumentToSaveUpdate.DocumentPath;
                    //    institutionWebPage.SystemDocument.Size = View.SystemDocumentToSaveUpdate.Size;
                    //    institutionWebPage.SystemDocument.ModifiedBy = View.SystemDocumentToSaveUpdate.ModifiedBy;
                    //    institutionWebPage.SystemDocument.ModifiedOn = View.SystemDocumentToSaveUpdate.ModifiedOn;
                    //}
                    if (ComplianceSetupManager.UpdateWebPageHtml(institutionWebPage, View.TenantID))
                    {
                        View.SuccessMessage = successMessage + View.SelectedPackageName + ".";
                    }
                    else
                    {
                        View.ErrorMessage = "Some error occured.Please try again.";
                    }
                }
            }

        }

        #region DISCLOSURE FORM
        public void GetAttachedDisclosureFormList()
        {

            if (View.SelectedContentType == ContentType.DISCLOSUREFORM)
            {
                if (View.TenantID > 0)
                {
                    String webSiteWebPageType = WebsiteWebPageType.DisclosureForm.GetStringValue();
                    String recordType = RecordType.Package.GetStringValue();
                    View.ListAttachedDocument = ComplianceSetupManager.GetAttachedDisclosureFormList(View.TenantID, recordType, webSiteWebPageType, View.GridCustomPaging);
                    if (View.ListAttachedDocument.IsNotNull() && View.ListAttachedDocument.Count > 0)
                    {
                        if (View.ListAttachedDocument[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ListAttachedDocument[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }

                }
                else
                {
                    View.ListAttachedDocument = new List<DisclosureDocument>();
                }
            }
            else
            {
                View.ListAttachedDocument = new List<DisclosureDocument>();
            }
        }

        public Boolean DeleteAttachedForm()
        {
            if (View.InstitutionWebPageID > 0)
            {
                Entity.ClientEntity.InstitutionWebPage institutionWebPage = ComplianceSetupManager.GetInstitutionWebPage(View.InstitutionWebPageID, View.TenantID);
                institutionWebPage.IsDeleted = true;
                institutionWebPage.ModifiedByID = View.CurrentUserId;
                institutionWebPage.ModifiedOn = DateTime.Now;
                institutionWebPage.SystemDocument.IsDeleted = true;
                return ComplianceSetupManager.UpdateWebPageHtml(institutionWebPage, View.TenantID);
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}




