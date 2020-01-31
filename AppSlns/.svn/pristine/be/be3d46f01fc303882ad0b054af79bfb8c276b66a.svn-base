#region Namespaces

#region System Specified
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#region Application Specified
using INTSOF.Utils;
using Entity;
using Business.RepoManagers;
using CoreWeb.Shell;

#endregion

#endregion

public partial class SelectBuisnessChannel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

               // GenerateFooter();
            }
        }
        catch (Exception ex)
        {

        }
    }

    #region Methods
    #region Private Methods
    /// <summary>
    /// Method to generate the dynamic footer
    /// </summary>
    private void GenerateFooter()
    {
        StringBuilder sbFooter = new StringBuilder();
        Int32 WebSiteId = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID));
        if (WebSiteId > 0)
        {
            WebSiteWebConfig webSiteWebConfig = WebSiteManager.GetWebSiteWebConfig(WebSiteId);
            if (webSiteWebConfig != null)
            {
                List<WebSiteWebPage> lstWebsitePages = webSiteWebConfig.WebSite.WebSiteWebPages.Where(webPage => !webPage.IsDeleted && webPage.IsActive).OrderBy(linkOrder => linkOrder.LinkOrder).ToList();
               // litFooter.Text = webSiteWebConfig.FooterText;
                Int32 count = 0;
                foreach (var page in lstWebsitePages)
                {
                    if (page.LinkPosition == Convert.ToInt32(CustomPageLinkPosition.Footer))
                    {
                        Dictionary<String, String> queryString = new Dictionary<String, String>();
                        String _viewType = String.Empty;
                        queryString = new Dictionary<String, String>
                                                                          { 
                                                                             //{ "Child", @"UserControl/CustomPageContent.ascx"},
                                                                             {"PageId",Convert.ToString( page.WebSiteWebPageID)},
                                                                             {"PageTitle",page.LinkText}
                                                                          };
                        String url = String.Format("CustomContentPage.aspx?args={0}",queryString.ToEncryptedQueryString());
                        sbFooter = count == 0 ? sbFooter.Append("&nbsp;&nbsp;") : sbFooter.Append("|&nbsp;&nbsp;");
                        sbFooter.Append("<a href=" + url + ">" + page.LinkText + "</a>&nbsp;&nbsp;");
                        count++;
                    }
                }
                litFooter.Text = litFooter.Text + "&nbsp;&nbsp;"
                 + Convert.ToString(sbFooter);
            }
        }
    }
    #endregion
    #endregion
}