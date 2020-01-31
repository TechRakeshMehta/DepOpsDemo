#region Namespaces
#region System Defined
using System;
using System.Collections.Generic;
using System.Text;
#endregion
#region Project Specific
using Entity;
#endregion
#endregion

namespace CoreWeb.WebSite.Views
{
    public interface IWebsitePageListView
    {
        /// <summary>
        /// Get and set the List of websitewebpages.
        /// </summary>
        List<WebSiteWebPage> GetWebsSiteWebPageList { get; set; }

        /// <summary>
        /// Get or Set the TenantID
        /// </summary>
        Int32 TenantID { get; set; }

        /// <summary>
        /// Get or Set the WebsiteId
        /// </summary>
        Int32 WebSiteId { get; set; }

        String SiteUrl
        {
            get;
            set;
        }

        String TenantName
        {
            get;
            set;
        }

    }
}




