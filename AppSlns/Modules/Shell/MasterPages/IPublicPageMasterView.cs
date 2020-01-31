using Entity;
using System;
using System.Collections.Generic;

namespace CoreWeb.Shell.MasterPages
{
    public interface IPublicPageMasterView
    {
        Int32 WebSiteId
        {
            get;
            set;
        }

        List<WebSiteWebPage> WebsitePages
        {
            get;
            set;
        }

        String FooterContent
        {
            set;
        }

        String ClientLogoUrl
        {
            get;
            set;
        }

        String ClientRightLogoUrl
        {
            get;
            set;
        }
        //UAT-2439
        Int32 TenantId
        {
            get;
            set;
        }

        Boolean IsLocationServiceTenant { get; set; } //UAT 3600
    }
}




