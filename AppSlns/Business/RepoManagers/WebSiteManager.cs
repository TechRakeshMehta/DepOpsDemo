using Entity;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.RepoManagers
{
    public class WebSiteManager
    {
        /// <summary>
        /// Gets web site details by url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static WebSite GetWebSite(String url)
        {
            return LookupManager.GetLookUpData<WebSite>().Where(webSite => webSite.URL.ToLower().Equals(url.ToLower())).FirstOrDefault();            
        }

        /// <summary>
        /// Get the website of the default ADB Tenant, in case no website url matches the criteria
        /// </summary>
        /// <param name="defaultTenantId">Id of the default Tenant</param>
        /// <returns>Details of the website.</returns>
        public static WebSite GetDefaultTenantWebsite()
        {
            return BALUtils.GetWebSiteRepository().GetDefaultTenantWebsite(SecurityManager.DefaultTenantID);
        }

        /// <summary>
        /// Gets web site login page Image 
        /// </summary>
        /// <param name="webSiteId"></param>
        /// <returns>Image of the Login page</returns>
        public static String GetWebSiteLoginImage(Int32 webSiteId)
        {
            return BALUtils.GetWebSiteRepository().GetWebSiteLoginImage(webSiteId);
        }

        public static String GetWebSiteRightLogoImage(Int32 webSiteId)
        {
            return BALUtils.GetWebSiteRepository().GetWebSiteRightLogoImage(webSiteId);
        }

        public static Int32 GetWebsiteTenantId(String websiteUrl)
        {
            TenantWebsiteMapping tenantWebsiteMapping = BALUtils.GetWebSiteRepository().GetWebsiteTenantId(websiteUrl);
            if (tenantWebsiteMapping.IsNotNull())
                return tenantWebsiteMapping.TWM_TenantID;
            else
                return SecurityManager.DefaultTenantID;
            //return BALUtils.GetWebSiteRepository().GetWebsiteTenantId(websiteUrl);
        }

        /// <summary>
        /// Gets web site web config details by web site id
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <returns></returns>
        public static WebSiteWebConfig GetWebSiteWebConfig(Int32 webSiteID)
        {
            return BALUtils.GetWebSiteRepository().GetWebSiteWebConfig(webSiteID);
        }

       
        
        /// <summary>
        /// Gets web site web page details by web site id and page name
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public static WebSiteWebPage GetWebSiteWebPage(Int32 webSiteID, string pageName)
        {
            return BALUtils.GetWebSiteRepository().GetWebSiteWebPage(webSiteID, pageName);
        }

        /// <summary>
        /// Gets web site web pages
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <returns></returns>
        public static IEnumerable<WebSiteWebPage> GetWebSiteWebPages(Int32 webSiteID)
        {
            return BALUtils.GetWebSiteRepository().GetWebSiteWebPages(webSiteID);
        }

        /// <summary>
        /// Gets web site web page
        /// </summary>
        /// <param name="webSiteWebPageID"></param>
        /// <returns></returns>
        public static WebSiteWebPage GetWebSiteWebPage(Int32 webSiteWebPageID)
        {
            return BALUtils.GetWebSiteRepository().GetWebSiteWebPage(webSiteWebPageID);
        }

        /// <summary>
        /// Gets WebSite
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <returns></returns>
        public static IEnumerable<WebSite> GetWebSites()
        {
            return BALUtils.GetWebSiteRepository().GetWebSites();
        }

        /// <summary>
        /// Gets web site web config
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <returns></returns>
        public static IEnumerable<WebSiteWebConfig> GetWebSiteWebConfigs(Int32 webSiteID)
        {
            return BALUtils.GetWebSiteRepository().GetWebSiteWebConfigs(webSiteID);
        }

        public static WebSiteWebConfig GetWebSiteWebConfigByConfigID(Int32 webSiteWebConfigID)
        {
            return BALUtils.GetWebSiteRepository().GetWebSiteWebConfigByConfigID(webSiteWebConfigID);
        }

        /// <summary>
        ///Update the WebSiteWebPage table
        /// </summary>
        /// <param name="webSiteWebPageID"></param>
        /// <returns></returns>
        public static Boolean UpdateWebPageHtml(WebSiteWebPage webPage)
        {
            return BALUtils.GetWebSiteRepository().Update(webPage);
        }

        /// <summary>
        ///Update the WebSiteWebConfig table
        /// </summary>
        /// <param name="webSiteWebPageID"></param>
        /// <returns></returns>
        public static Boolean UpdateWebSiteWebConfig(WebSiteWebConfig webConfig)
        {
            return BALUtils.GetWebSiteRepository().Update(webConfig);
        }

        #region Website Setup
        public static WebSite GetWebSiteDetail(Int32 TenantID)
        {
            try
            {
                return BALUtils.GetWebSiteRepository().GetWebSiteDetail(TenantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String GetInstitutionUrl(int tenantId)
        {
            try
            {
                var webSite = BALUtils.GetWebSiteRepository().GetWebSiteDetail(tenantId);
                String applicationUrl = String.Empty;
                if (webSite.IsNotNull() && webSite.WebSiteID != SysXDBConsts.NONE)
                {
                    applicationUrl = webSite.URL;
                }
                else
                {
                    webSite = BALUtils.GetWebSiteRepository().GetWebSiteDetail(SecurityManager.DefaultTenantID);
                    applicationUrl = webSite.URL;
                }

                if (!(applicationUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || applicationUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
                {
                    if (HttpContext.Current != null)
                    {
                        applicationUrl = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", applicationUrl.Trim());
                    }
                    else
                    {
                        applicationUrl = string.Concat("http://", applicationUrl.Trim());
                    }
                }

                return applicationUrl;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //UAT-2792
        //public static Int32 GetTenantIDByURL(String host)
        //{
        //    try
        //    {
        //        return BALUtils.GetWebSiteRepository().GetOrganisationIDByURL(host);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        public static Boolean SaveWebsiteDetail(WebSite webSite, TenantWebsiteMapping tenantWebsiteMapping)
        {
            try
            {
                return BALUtils.GetWebSiteRepository().SaveWebsiteDetail(webSite, tenantWebsiteMapping);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static TenantWebsiteMapping GetTenantWebsiteMapping(Int32 TenantID)
        {
            try
            {
                return BALUtils.GetWebSiteRepository().GetTenantWebsiteMapping(TenantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateObject()
        {
            try
            {
                return BALUtils.GetWebSiteRepository().UpdateObject();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<lkpWebsiteCustomPage> GetCustomPageList()
        {
            try
            {
                return BALUtils.GetWebSiteRepository().GetCustomPageList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveWebSitePage(WebSiteWebPage websitewebPage)
        {
            try
            {
                return BALUtils.GetWebSiteRepository().SaveWebSitePage(websitewebPage);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 GetOrganisationIDByURL(String websiteURL)
        {
            try
            {
                return BALUtils.GetWebSiteRepository().GetOrganisationIDByURL(websiteURL.Trim().ToLower());
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

          public static Boolean IsUrlExistForTenantType(String websiteURL,String tenantTypeCode)
        {
            try
            {
                return BALUtils.GetWebSiteRepository().IsUrlExistForTenantType(websiteURL.Trim().ToLower(),tenantTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Data Entry Help
        /// <summary>
        /// Gets Data Entry Help Content.
        /// </summary>
        /// <param name="websiteId"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static WebSiteWebPage GetDataEntryHelpContentByPackageId(Int32 websiteId,Int32? recordId,String recordType)
        {
            return BALUtils.GetWebSiteRepository().GetDataEntryHelpContentByPackageId(websiteId, recordId,recordType);
        }


        /// <summary>
        /// Get WebsiteWebPageType Id by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Int32 GetWebsiteWebPageTypeIdByCode(String code)
        {
            return LookupManager.GetLookUpData<Entity.lkpWebsiteWebPageType>().Where(cond => cond.Code == code && !cond.IsDeleted).FirstOrDefault().WebsiteWebPageTypeID;
        }

        /// <summary>
        /// Get RecordType id by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Int16 GetRecordTypeIdByCode(String code)
        {
            return BALUtils.GetWebSiteRepository().GetRecordTypeIdByCode(code);
        }
        /// <summary>
        /// Gets Data entry help content by web site id, recordId, recordTypeCode and websiteWebPageTypeCode
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <param name="packageID"></param>
        /// <param name="recordTypeCode"></param>
        /// <param name="websiteWebPageTypeCode"></param>
        /// <returns></returns>
        //public static WebSiteWebPage GeDateHelpHtmlFromtWebSiteWebPage(Int32 webSiteID, Int32 packageID, String recordTypeCode, String websiteWebPageTypeCode)
        //{
        //    return BALUtils.GetWebSiteRepository().GeDateHelpHtmlFromtWebSiteWebPage(webSiteID, packageID, recordTypeCode, websiteWebPageTypeCode);
        //}

        /// <summary>
        /// Get existing package id list.
        /// </summary>
        /// <param name="websiteId"></param>
        /// <returns></returns>
        public static List<Int32> GetExistingPackageIdList(Int32 websiteId, String recordType)
        {
            try
            {
                List<Int32?> tempExistingPackageIdList = new List<Int32?>();
                tempExistingPackageIdList = BALUtils.GetWebSiteRepository().GetExistingPackageIdList(websiteId, recordType);
                //set -1 for null because null record Id is saved in db for default value.
                return tempExistingPackageIdList.Select(cond => cond.HasValue ? cond.Value : -1).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        /// <summary>
        /// Method for getting Website header html 
        ///</summary>        
        public static String GetWebSiteHeaderHtml(Int32 WebsiteId)
        {
            return BALUtils.GetWebSiteRepository().GetWebSiteHeaderHtml(WebsiteId);
        }
    }
}
