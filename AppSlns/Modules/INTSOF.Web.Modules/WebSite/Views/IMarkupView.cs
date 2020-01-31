using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Entity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
namespace CoreWeb.WebSite.Views
{
    public interface IMarkupView
    {
        #region Properties

        /// <summary>
        /// Selected content type
        /// </summary>
        ContentType SelectedContentType { get; set; }

        /// <summary>
        /// Page id
        /// </summary>
        Int32 PageID { get; }

        /// <summary>
        /// Content
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// Web site id
        /// </summary>
        Int32 WebSiteId { get; set; }

        /// <summary>
        /// Get or set TenantId
        /// </summary>
        Int32 TenantID { get; set; }

        /// <summary>
        /// get or set Tenant Name
        /// </summary>
        String TenantName { get; set; }


        /// <summary>
        /// LinkText
        /// </summary>
        string LinkText { get; set; }

        /// <summary>
        /// Link Order
        /// </summary>
        Int32? LinkOrder { get; set; }

        /// <summary>
        /// Link Position
        /// </summary>
        Int32? LinkPosition { get; set; }

        /// <summary>
        /// Page Title
        /// </summary>
        String PageTitle { get; set; }

        /// <summary>
        /// Is Active
        /// </summary>
        Boolean IsActive { get; set; }

        /// <summary>
        /// Get or set Footer Text
        /// </summary>
        String FooterText { get; set; }

        /// <summary>
        /// Get current loggedIn UserId
        /// </summary>
        Int32 CurrentUserId { get; }

        /// <summary>
        /// Get or set WebsiteWebPageId
        /// </summary>
        Int32 WebSiteWebPageId { get; set; }

        /// <summary>
        ///get or set WebsiteWebconfigid
        /// </summary>
        Int32 WebSiteWebConfigId { get; set; }

        /// <summary>
        /// Get or Set Master Page Value.
        /// </summary>
        String MasterPage { get; set; }

        /// <summary>
        /// Get or Set ErrorMessage Value.
        /// </summary>
        String ErrorMessage { get; set; }

        /// <summary>
        /// Get or SuccessMessage Value.
        /// </summary>
        String SuccessMessage { get; set; }

        /// <summary>
        /// Get or InfoMessage Value.
        /// </summary>
        String InfoMessage { get; set; }

        String SiteUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Populates the dropdown with the list of Compliance Package.
        /// </summary>
        List<Entity.ClientEntity.CompliancePackage> lstCompliancePackage
        {
            set;
        }

        /// <summary>
        /// Get or Set Selected Package id.
        /// </summary>
        Int32 SelectedPackageId
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Selected Package Name.
        /// </summary>
        String SelectedPackageName
        {
            get;
        }

        /// <summary>
        /// SelectedContentOperation
        /// </summary>
        Int32 SelectedContentOperation
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set Package List
        /// </summary>
        List<Entity.ClientEntity.CompliancePackage> AllPackageList
        {
            get;
            set;
        }

        Entity.ClientEntity.SystemDocument SystemDocumentToSaveUpdate
        {
            get;
            set;
        }

        #region DISCLOSURE FORM
        List<DisclosureDocument> ListAttachedDocument { get; set; }
        Int32 InstitutionWebPageID
        {
            get;
            set;
        }

        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualRecordCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        #endregion
        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// Loads web sites
        /// </summary>
        /// <param name="webSites"></param>
        void LoadWebsites(IEnumerable<Entity.WebSite> webSites);

        /// <summary>
        /// Loads content type
        /// </summary>
        /// <param name="contentTypes"></param>
        void LoadContentType(IEnumerable<KeyValuePair<ContentType, string>> contentTypes);

        /// <summary>
        /// Loads web site web page
        /// </summary>
        /// <param name="webSiteWebPages"></param>
        void LoadWebSiteWebPage(IEnumerable<WebSiteWebPage> webSiteWebPages);

        /// <summary>
        /// Loads web site web config
        /// </summary>
        /// <param name="webSiteWebConfigs"></param>
        void LoadWebSiteWebConfig(IEnumerable<WebSiteWebConfig> webSiteWebConfigs);

        #endregion

    }
}




