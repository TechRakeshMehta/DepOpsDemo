#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  PostRegistration.aspx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using System.Text;
using System.Linq;

#endregion

#region Application Specific

using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using CoreWeb.Shell;
using CoreWeb.Shell.Views;
using System.Collections.Generic;
using Entity;
using Business.RepoManagers;

#endregion

#endregion

public partial class PostRegistration : System.Web.UI.Page
{
    public String LoginPageImageUrl
    {
        get;
        set;
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Dictionary<String, String> encryptedQueryString = null;
            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                encryptedQueryString = new Dictionary<String, String>();
                encryptedQueryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            }
            if (!encryptedQueryString.IsNull())
            {
               if (encryptedQueryString.ContainsKey("IsNewAccount") && Convert.ToBoolean(encryptedQueryString["IsNewAccount"]))
                {
                    if (encryptedQueryString.ContainsKey(AppConsts.CHILD))
                    {
                        lblEmail.Text = encryptedQueryString[AppConsts.CHILD].ToString();
                        dvNewApplicantAccount.Visible = true;
                    }
                }
                else if (encryptedQueryString.ContainsKey("IsSharedUserAccount") && Convert.ToBoolean(encryptedQueryString["IsSharedUserAccount"]))
                {
                    if (encryptedQueryString.ContainsKey(AppConsts.CHILD))
                    {
                        lblEmail.Text = encryptedQueryString[AppConsts.CHILD].ToString();
                        dvNewApplicantAccount.Visible = true;
                        dvNewApplicant.Visible = false;
                        dvSharedUser.Visible = true;
                    }
                }
                else if (encryptedQueryString.ContainsKey("IsSharedUserLinkedAccount") && Convert.ToBoolean(encryptedQueryString["IsSharedUserLinkedAccount"]))
                {
                    //if (encryptedQueryString.ContainsKey(AppConsts.CHILD))
                    //{
                    // lblEmail.Text = encryptedQueryString[AppConsts.CHILD].ToString();
                    dvNewApplicantAccount.Visible = false;
                    dvNewApplicant.Visible = false;
                    dvSharedUser.Visible = false;
                    dvSharedUserLinkedAccount.Visible = true;
                    //}
                }
                else
                {
                    if (encryptedQueryString.ContainsKey(AppConsts.CHILD))
                    {
                        lblLinkEmail.Text = encryptedQueryString[AppConsts.CHILD].ToString();
                        dvLinkedApplicantAccount.Visible = true;
                    }
                }
            }
            //GenerateFooter();

            #region SET LOGIN PAGE IMAGE BASED ON THE CLIENT URL

            //ManageLoginPageImage(sender);

            #endregion
        }
    }

    ///// <summary>
    ///// Load the image of the login page based on the client site.
    ///// </summary>
    ///// <param name="sender">Current page</param>
    //private void ManageLoginPageImage(object sender)
    //{
    //    String baseImagePath = WebConfigurationManager.AppSettings[AppConsts.CLIENT_WEBSITE_IMAGES];
    //    Int32 WebSiteId = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID));

    //    if (String.IsNullOrEmpty(LoginPageImageUrl))
    //    {
    //        Page page = (Page)sender;
    //        String SiteUrl = page.Request.ServerVariables.Get("server_name");


    //        String imageURL = String.Empty;
    //        if (WebSiteId > 0)
    //            imageURL = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?WebsiteId={0}&DocumentType={1}", WebSiteId, "LoginImage");

    //        divLogin.Style.Add("background-image", String.Format("url('{0}')", imageURL));
    //    }
    //}

    #region Methods
    /// <summary>
    /// Method to generate the dynamic footer
    /// </summary>
    //private void GenerateFooter()
    //{

    //    StringBuilder sbFooter = new StringBuilder();
    //    Int32 WebSiteId = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID));
    //    if (WebSiteId > 0)
    //    {
    //        WebSiteWebConfig webSiteWebConfig = WebSiteManager.GetWebSiteWebConfig(WebSiteId);
    //        Int32 count = 0;
    //        if (webSiteWebConfig != null)
    //        {
    //            List<WebSiteWebPage> lstWebsitePages = webSiteWebConfig.WebSite.WebSiteWebPages.Where(webPage => !webPage.IsDeleted && webPage.IsActive).OrderBy(linkOrder => linkOrder.LinkOrder).ToList();
    //            //litFooter.Text = webSiteWebConfig.FooterText;
    //            foreach (var page in lstWebsitePages)
    //            {
    //                if (page.LinkPosition == Convert.ToInt32(CustomPageLinkPosition.Footer))
    //                {
    //                    Dictionary<String, String> queryString = new Dictionary<String, String>();
    //                    String _viewType = String.Empty;
    //                    queryString = new Dictionary<String, String>
    //                                                                      { 
    //                                                                         //{ "Child", @"UserControl/CustomPageContent.ascx"},
    //                                                                         {"PageId",Convert.ToString( page.WebSiteWebPageID)},
    //                                                                         {"PageTitle",page.LinkText}
    //                                                                      };
    //                    String url = String.Format("CustomContentPage.aspx?args={0}", queryString.ToEncryptedQueryString());
    //                    sbFooter = count == 0 ? sbFooter.Append("&nbsp;&nbsp;") : sbFooter.Append("|&nbsp;&nbsp;");
    //                    sbFooter.Append("<a href=" + url + ">" + page.LinkText + "</a>&nbsp;&nbsp;");
    //                    count++;
    //                }
    //            }
    //            litFooter.Text = litFooter.Text + "&nbsp;&nbsp;"
    //             + Convert.ToString(sbFooter);
    //        }
    //    }
    //}
    #endregion
}
