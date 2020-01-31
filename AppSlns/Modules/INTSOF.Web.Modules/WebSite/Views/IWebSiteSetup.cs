#region NameSpaces

#region System Defined
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
#endregion

#region Project Specific
using Entity;
using INTSOF.Utils;
#endregion
#endregion

namespace CoreWeb.WebSite.Views
{
   public interface IWebSiteSetup
    {
        String SiteUrl { get; set; }
        String Notes { get; set; }
        String SiteTitle { get; set; }
        //String MasterPage { get; set; }
        String Theme { get; set; }
        Boolean IsSiteMarkup { get; set; }
        Int32 TenantID { get; }
       String LoginImageUrl{get;set;}
       Int32 WebsiteId { get; set; }
        string RegistrationImageUrl { get; set; }
    }

   public class WebSiteContract
   {
       public Boolean IsEditableMode { get; set; }
   }
}
