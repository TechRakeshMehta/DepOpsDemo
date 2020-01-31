using Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Interfaces
{
    public interface IWebSiteRepository
    {
        #region Web site

        /// <summary>
        /// Gets web site details by url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        WebSite GetWebSite(string url);

        /// <summary>
        /// Gets web site login page Image 
        /// </summary>
        /// <param name="webSiteId"></param>
        /// <returns>Image of the Login page</returns>
        String GetWebSiteLoginImage(Int32 webSiteId);
        /// <summary>
        /// Gets web site right logo Image If uploaded
        /// </summary>
        /// <param name="webSiteId"></param>
        /// <returns></returns>
        String GetWebSiteRightLogoImage(Int32 webSiteId);

        /// <summary>
        /// Gets web sites
        /// </summary>
        /// <returns></returns>
        IEnumerable<WebSite> GetWebSites();

        TenantWebsiteMapping GetWebsiteTenantId(String websiteUrl);

        /// <summary>
        /// Get the website of the default ADB Tenant, in case no website url matches the criteria
        /// </summary>
        /// <param name="defaultTenantId">Id of the default Tenant</param>
        /// <returns>Details of the website.</returns>
        WebSite GetDefaultTenantWebsite(Int32 defaultTenantId);

        #endregion

        #region Web site web page

        /// <summary>
        /// Gets WebSiteWebPage details by webSiteId and pageName
        /// </summary>
        /// <param name="webSiteId"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        WebSiteWebPage GetWebSiteWebPage(Int32 webSiteId, string pageName);

        /// <summary>
        /// Gets WebSiteWebPages
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <returns></returns>
        IEnumerable<WebSiteWebPage> GetWebSiteWebPages(Int32 webSiteID);

        /// <summary>
        /// Gets WebSiteWebPage
        /// </summary>
        /// <param name="webSiteWebPageID"></param>
        /// <returns></returns>
        WebSiteWebPage GetWebSiteWebPage(Int32 webSiteWebPageID);

        /// <summary>
        /// Update WebSiteWebConfig
        /// </summary>
        /// <returns></returns>
        Boolean Update(WebSiteWebPage webPage);
    
        #endregion

        #region Web site web config

        /// <summary>
        /// Gets web site web config details by web site id
        /// </summary>
        /// <param name="webSiteId"></param>
        /// <returns></returns>
        WebSiteWebConfig GetWebSiteWebConfig(Int32 webSiteId);

        /// <summary>
        /// Gets web site web config
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <returns></returns>
        IEnumerable<WebSiteWebConfig> GetWebSiteWebConfigs(Int32 webSiteID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        WebSiteWebConfig GetWebSiteWebConfigByConfigID(Int32 webSiteWebConfigID);


        /// <summary>
        /// Update WebSiteWebConfig
        /// </summary>
        /// <returns></returns>
        Boolean Update(WebSiteWebConfig webConfig);

        #endregion

        #region Website Setup
        WebSite GetWebSiteDetail(Int32 TenantID);
        Boolean SaveWebsiteDetail(WebSite webSite, TenantWebsiteMapping tenantWebsiteMApping);
        TenantWebsiteMapping GetTenantWebsiteMapping(Int32 TenantID);
        Boolean UpdateObject();
        List<lkpWebsiteCustomPage> GetCustomPageList();
        Boolean SaveWebSitePage(WebSiteWebPage websitewebPage);
        Int32 GetOrganisationIDByURL(String websiteURL);
        Boolean IsUrlExistForTenantType(String websiteURL, String tenantTypeCode);
     //   Int32 GetTenantIDByURL(String host);    //UAT-2792
        #endregion

        #region Data Entry Help
        /// <summary>
        /// Gets Data entry help content by web site id and recordId
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        WebSiteWebPage GetDataEntryHelpContentByPackageId(Int32 webSiteID, Int32? recordId,String recordType);

        /// <summary>
        /// Get WebsiteWebPageType Id by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Int32 GetWebsiteWebPageTypeIdByCode(String code);

        /// <summary>
        /// Get RecordType id by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Int16 GetRecordTypeIdByCode(String code);
        /// <summary>
        /// Get Date help html content based on package Id, recordTypeCode, websiteWebPageTypeCode and website Id from WebSiteWebPage
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <param name="packageID"></param>
        /// <returns></returns>
        //WebSiteWebPage GeDateHelpHtmlFromtWebSiteWebPage(Int32 webSiteID, Int32 packageID, String recordTypeCode, String websiteWebPageTypeCode);
        /// <summary>
        /// Get existing package id list.
        /// </summary>
        /// <param name="websiteId"></param>
        /// <returns></returns>
        List<Int32?> GetExistingPackageIdList(Int32 webSiteID, String recordType);
        #endregion

        /// <summary>
        /// Method is used to Get BusinesChannel Type based on TenantID
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        DataTable GetBusinessChannelTypeByTenantID(Int32 tenantID);

        String GetWebSiteHeaderHtml(Int32 WebsiteId);

    }
}
