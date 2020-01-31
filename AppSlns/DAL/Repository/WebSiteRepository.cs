using DAL.Interfaces;
using Entity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;


namespace DAL.Repository
{
    public class WebSiteRepository : BaseRepository, IWebSiteRepository
    {
        #region Private Variables

        private SysXAppDBEntities _dbNavigation;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebSiteRepository()
        {
            _dbNavigation = base.Context;
        }

        #endregion

        #region Web site
        /// <summary>
        /// Gets web site details by url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public WebSite GetWebSite(String url)
        {
            return _dbNavigation.WebSites.FirstOrDefault(webSite => webSite.URL.Equals(url));
        }

        /// <summary>
        /// Get the website of the default ADB Tenant, in case no website url matches the criteria
        /// </summary>
        /// <param name="defaultTenantId">Id of the default Tenant</param>
        /// <returns>Details of the website.</returns>
        public WebSite GetDefaultTenantWebsite(Int32 defaultTenantId)
        {
            TenantWebsiteMapping tenantWebsiteMapping = _dbNavigation.TenantWebsiteMappings.Include("WebSite").Where(twm => twm.TWM_TenantID == defaultTenantId).FirstOrDefault();
            return tenantWebsiteMapping.WebSite;
        }


        /// <summary>
        /// Gets web site login page Image 
        /// </summary>
        /// <param name="webSiteId"></param>
        /// <returns>Image of the Login page</returns>
        public String GetWebSiteLoginImage(Int32 webSiteId)
        {
            return _dbNavigation.WebSites.FirstOrDefault(webSite => webSite.WebSiteID == webSiteId).LoginPageImageURL;
        }

        public String GetWebSiteRightLogoImage(Int32 webSiteId)
        {
            return _dbNavigation.WebSites.FirstOrDefault(webSite => webSite.WebSiteID == webSiteId).TopRightImageUrl;
        }

        /// <summary>
        /// Gets web sites
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WebSite> GetWebSites()
        {
            return _dbNavigation.WebSites;
        }

        public TenantWebsiteMapping GetWebsiteTenantId(String websiteUrl)
        {
            WebSite website = _dbNavigation.WebSites.Where(web => web.URL.ToLower().Trim() == websiteUrl.ToLower().Trim()).FirstOrDefault();
            if (website.IsNotNull())
            {
                //TenantWebsiteMapping tenantWebsiteMapping = _dbNavigation.TenantWebsiteMappings.Where(twm => twm.TWM_WebSiteID == website.WebSiteID).FirstOrDefault();
                //if (tenantWebsiteMapping.IsNotNull())
                //    return tenantWebsiteMapping.TWM_TenantID;
                return _dbNavigation.TenantWebsiteMappings.Where(twm => twm.TWM_WebSiteID == website.WebSiteID).FirstOrDefault();
            }
            return null;
        }

        #endregion

        #region Web site web page

        /// <summary>
        /// Gets web site web page details by web site id and page name
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public WebSiteWebPage GetWebSiteWebPage(Int32 webSiteID, string pageName)
        {
            return _dbNavigation.WebSiteWebPages.FirstOrDefault(webSiteWebPage =>
                webSiteWebPage.WebSiteID == webSiteID &&
                webSiteWebPage.PageName.Equals(pageName));
        }

        /// <summary>
        /// Gets web site web pages
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <returns></returns>
        public IEnumerable<WebSiteWebPage> GetWebSiteWebPages(Int32 webSiteID)
        {
            return _dbNavigation.WebSiteWebPages.Where(webSiteWebPage => webSiteWebPage.WebSiteID == webSiteID);

        }


        /// <summary>
        /// Gets web site web page
        /// </summary>
        /// <param name="webSiteWebPageID"></param>
        /// <returns></returns>
        public WebSiteWebPage GetWebSiteWebPage(Int32 webSiteWebPageID)
        {
            return _dbNavigation.WebSiteWebPages.FirstOrDefault(webSiteWebPage => webSiteWebPage.WebSiteWebPageID == webSiteWebPageID);
        }

        /// <summary>
        /// Update WebSiteWebPage
        /// </summary>
        /// <param name="webSiteWebConfigID"></param>
        /// <returns></returns>
        public Boolean Update(WebSiteWebPage webpage)
        {
            _dbNavigation.SaveChanges();
            return true;
        }

        #endregion

        #region Web site web config

        /// <summary>
        /// Gets web site web config details by web site id
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <returns></returns>
        public WebSiteWebConfig GetWebSiteWebConfig(int webSiteID)
        {
            return _dbNavigation.WebSiteWebConfigs.Include("WebSite").FirstOrDefault(webSiteWebConfig => webSiteWebConfig.WebSiteID == webSiteID
               && !webSiteWebConfig.IsDeleted);
        }

        /// <summary>
        /// Gets web site web config
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <returns></returns>
        public IEnumerable<WebSiteWebConfig> GetWebSiteWebConfigs(Int32 webSiteID)
        {
            return _dbNavigation.WebSiteWebConfigs.Where(webSiteWebPage => webSiteWebPage.WebSiteID == webSiteID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webSiteWebConfigID"></param>
        /// <returns></returns>
        public WebSiteWebConfig GetWebSiteWebConfigByConfigID(Int32 webSiteWebConfigID)
        {
            return _dbNavigation.WebSiteWebConfigs.First(webSiteWebConfig => webSiteWebConfig.WebSiteWebConfigID == webSiteWebConfigID);
        }

        /// <summary>
        /// Update WebSiteWebConfig
        /// </summary>
        /// <param name="webSiteWebConfigID"></param>
        /// <returns></returns>
        public Boolean Update(WebSiteWebConfig webConfig)
        {
            _dbNavigation.SaveChanges();
            return true;
        }

        #endregion

        #region WebsiteSetup
        public WebSite GetWebSiteDetail(Int32 TenantID)
        {
            WebSite websiteEmpty = new WebSite();
            TenantWebsiteMapping tenantWebsiteMapping = _dbNavigation.TenantWebsiteMappings.Where(cond => cond.TWM_TenantID == TenantID && !cond.TWM_IsDeleted).FirstOrDefault();
            if (tenantWebsiteMapping != null)
            {
                return _dbNavigation.WebSites.Where(website => website.WebSiteID == tenantWebsiteMapping.TWM_WebSiteID).FirstOrDefault();
            }
            return websiteEmpty;
        }

        public Boolean SaveWebsiteDetail(WebSite webSite, TenantWebsiteMapping tenantWebsiteMapping)
        {
            _dbNavigation.WebSites.AddObject(webSite);
            _dbNavigation.SaveChanges();
            Int32 webSiteId = webSite.WebSiteID;
            tenantWebsiteMapping.TWM_WebSiteID = webSiteId;
            _dbNavigation.TenantWebsiteMappings.AddObject(tenantWebsiteMapping);
            if (_dbNavigation.SaveChanges() > 0)
            {
                return true;
            }
            else
                return false;
        }

        public TenantWebsiteMapping GetTenantWebsiteMapping(Int32 TenantID)
        {
            return _dbNavigation.TenantWebsiteMappings.Where(cond => cond.TWM_TenantID == TenantID && !cond.TWM_IsDeleted).FirstOrDefault();
        }

        public Boolean UpdateObject()
        {
            if (_dbNavigation.SaveChanges() > 0)
                return true;
            else
                return false;
            //return _dbNavigation.WebSites.Where(website => website.WebSiteID == webSiteID).FirstOrDefault();
        }

        public List<lkpWebsiteCustomPage> GetCustomPageList()
        {
            return _dbNavigation.lkpWebsiteCustomPages.Where(cond => cond.IsDeleted == false).ToList();
        }

        public Boolean SaveWebSitePage(WebSiteWebPage websitewebPage)
        {
            _dbNavigation.WebSiteWebPages.AddObject(websitewebPage);
            if (_dbNavigation.SaveChanges() > 0)
                return true;
            return false;
        }


        public Int32 GetOrganisationIDByURL(String websiteURL)
        {
            WebSite objWebSite = _dbNavigation.WebSites.FirstOrDefault(website => website.URL.ToLower().Equals(websiteURL));
            if (objWebSite.IsNotNull())
            {
                TenantWebsiteMapping objTWM = objWebSite.TenantWebsiteMappings.FirstOrDefault();
                if (objTWM.IsNotNull())
                {
                    Organization objORG = objTWM.Tenant.Organizations.FirstOrDefault(x => x.ParentOrganizationID == null);
                    return objORG == null ? 0 : objORG.OrganizationID;
                }
            }
            return 0;
        }

        //UAT-2792
        //public Int32 GetTenantIDByURL(String websiteURL)
        //{
        //    WebSite objWebSite = _dbNavigation.WebSites.FirstOrDefault(website => website.URL.ToLower().Equals(websiteURL));
        //    if (objWebSite.IsNotNull())
        //    {
        //        TenantWebsiteMapping objTWM = objWebSite.TenantWebsiteMappings.FirstOrDefault();
        //        return objTWM == null ? 0 : objTWM.TWM_TenantID;
        //    }
        //    return 0;
        //}

        #endregion

        public Boolean IsUrlExistForTenantType(String websiteURL, String tenantTypeCode)
        {
            var tempWebsite = _dbNavigation.WebSites.FirstOrDefault(website => website.URL.ToLower().Equals(websiteURL));
            if (tempWebsite.IsNotNull())
            {
                return tempWebsite.TenantWebsiteMappings.Where(x => x.TWM_IsDeleted == false).Select(x => x.Tenant)
                    .Any(x => x.lkpTenantType.TenantTypeCode == tenantTypeCode && x.IsDeleted == false);
            }
            return false;
        }

        #region WebsiteSetup
        //public WebSite GetWebSiteDetail(Int32 TenantID)
        //{
        //    Int32 webSiteID = _dbNavigation.TenantWebsiteMappings.Where(cond => cond.TWM_TenantID == TenantID).FirstOrDefault().TWM_WebSiteID;
        //    return _dbNavigation.WebSites.Where(website => website.WebSiteID == webSiteID).FirstOrDefault();
        //}

        public Boolean UpdateWebSiteDetail(WebSite websiteData)
        {
            WebSite webSite = _dbNavigation.WebSites.Where(cond => cond.WebSiteID == websiteData.WebSiteID).FirstOrDefault();
            return false;
            //return _dbNavigation.WebSites.Where(website => website.WebSiteID == webSiteID).FirstOrDefault();
        }


        #endregion

        #region Data Entry Help
        /// <summary>
        /// Gets Data entry help content by web site id and recordId
        /// </summary>
        /// <param name="websiteId"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public WebSiteWebPage GetDataEntryHelpContentByPackageId(Int32 webSiteID, Int32? recordId, String recordType)
        {
            String websiteWebPageType = WebsiteWebPageType.DataEntryHelp.GetStringValue();
            if (recordId.IsNotNull())
            {
                return _dbNavigation.WebSiteWebPages.Where(cond => cond.RecordId == recordId && cond.WebSiteID == webSiteID && cond.lkpWebsiteWebPageType.Code == websiteWebPageType && cond.lkpRecordType.Code == recordType && !cond.IsDeleted).FirstOrDefault();
            }
            else
            {
                return _dbNavigation.WebSiteWebPages.Where(cond => cond.RecordId == null && cond.lkpRecordType.Code == recordType && cond.WebSiteID == webSiteID && cond.lkpWebsiteWebPageType.Code == websiteWebPageType && !cond.IsDeleted).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get WebsiteWebPageType Id by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Int32 GetWebsiteWebPageTypeIdByCode(String code)
        {
            return _dbNavigation.lkpWebsiteWebPageTypes.Where(cond => cond.Code == code && !cond.IsDeleted).FirstOrDefault().WebsiteWebPageTypeID;
        }

        /// <summary>
        /// Get RecordType id by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Int16 GetRecordTypeIdByCode(String code)
        {
            return _dbNavigation.lkpRecordTypes.Where(cond => cond.Code == code && !cond.IsDeleted).FirstOrDefault().RecordTypeID;
        }

        /// <summary>
        /// Get existing package id list.
        /// </summary>
        /// <param name="websiteId"></param>
        /// <returns></returns>
        public List<Int32?> GetExistingPackageIdList(Int32 webSiteID, String recordType)
        {
            String websiteWebPageType = WebsiteWebPageType.DataEntryHelp.GetStringValue();
            return _dbNavigation.WebSiteWebPages.Where(cond => cond.WebSiteID == webSiteID && cond.lkpWebsiteWebPageType.Code == websiteWebPageType && !cond.IsDeleted && cond.lkpRecordType.Code == recordType).Select(x => x.RecordId).ToList();
        }
        #endregion


        /// <summary>
        /// Method is used to Get BusinesChannel Type based on TenantID
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public DataTable GetBusinessChannelTypeByTenantID(Int32 tenantId)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetBusinessChannelType", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantID", tenantId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BusinessChannelType";
                    return ds.Tables["BusinessChannelType"];
                }

            }
            return new DataTable("BusinessChannelType");

        }

        ///<summary>
        ///Method for getting Website header html
        ///</summary>
        public String GetWebSiteHeaderHtml(Int32 WebsiteId)
        {
            return _dbNavigation.WebSiteWebConfigs.Where(x => x.WebSiteID == WebsiteId && !x.IsDeleted).FirstOrDefault().HeaderHtml; 
        }
    }
}
