#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ChanngePassword.aspx.cs
// Purpose:   
//

#endregion

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
using System.Configuration;
using CoreWeb;

#endregion

#endregion

/// <summary>
/// This class handles the operations change password section of the application.
/// </summary>
public partial class ChangePassword : System.Web.UI.Page
{
    #region Variables

    #endregion

    #region Properties

    public Boolean IsLocationServiceTenant
    {
        get
        {
            if (!ViewState["IsLocationServiceTenant"].IsNullOrEmpty())
                return Convert.ToBoolean(ViewState["IsLocationServiceTenant"]);
            return false;
        }
        set
        {
            ViewState["IsLocationServiceTenant"] = value;
        }
    }

    #endregion

    #region Events

    /// <summary>
    /// Page load event for initialized event in presenter.
    /// </summary>
    /// <param name="sender">The object firing the event.</param>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //GenerateFooter();                
            }
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    #region Methods
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
                //litFooter.Text = webSiteWebConfig.FooterText;
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
                        String url = String.Format("CustomContentPage.aspx?args={0}", queryString.ToEncryptedQueryString());
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

    #region Globalization for Multi-Language

    protected override void InitializeCulture()
    {
        //If is location service tenant and key added in config is true.
        Boolean isLanguageTransaltionEnable = ConfigurationManager.AppSettings["IsLanguageTranslation"].IsNullOrEmpty() ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["IsLanguageTranslation"]);

        var websiteUrl = Page.Request.ServerVariables.Get("server_name");  //  "CBI.complio.com"; //
        Int32 tenantId = WebSiteManager.GetWebsiteTenantId(websiteUrl);
        if (tenantId > AppConsts.NONE)
        {
            IsLocationServiceTenant = true; //forgotPasswordPresenterContext.IsLocationServiceTenant(tenantId);
            if (Session["IsLocationTenant"].IsNullOrEmpty())
                SysXWebSiteUtils.SessionService.SetCustomData("IsLocationTenant", IsLocationServiceTenant);
        }

        if (isLanguageTransaltionEnable && IsLocationServiceTenant)
        {
            LanguageTranslateUtils.LanguageTranslateInit();
            base.InitializeCulture();
        }
    }
    
    #endregion
}