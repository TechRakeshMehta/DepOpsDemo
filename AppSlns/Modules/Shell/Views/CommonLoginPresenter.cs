#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  CommonLoginPresenter.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using System.Text.RegularExpressions;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using Business.RepoManagers;
using INTSOF.Utils.Consts;
using CoreWeb.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.Shell.Views
{
    public class CommonLoginPresenter : Presenter<ICommonLoginView>
    {
        public Boolean ResendVerificationLink()
        {
            SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(View.UserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;
            if (!user.IsNullOrEmpty())
            {

                Entity.OrganizationUser organizationUser = null;
                IQueryable<OrganizationUser> lstOrgUsers = SecurityManager.GetOrganizationUserInfoByUserId(user.UserId.ToString());
                if (!lstOrgUsers.IsNullOrEmpty())
                {
                    organizationUser = lstOrgUsers.FirstOrDefault(obj => obj.OrganizationID == user.OrganizationId);
                    if (!organizationUser.IsNullOrEmpty())
                    {
                        //Get Website Url
                        Entity.WebSite webSite = WebSiteManager.GetWebSiteDetail(organizationUser.Organization.Tenant.TenantID);
                        String applicationUrl = String.Empty;
                        if (webSite.IsNotNull() && webSite.WebSiteID != SysXDBConsts.NONE)
                        {
                            applicationUrl = webSite.URL;
                        }
                        else
                        {
                            webSite = WebSiteManager.GetWebSiteDetail(SecurityManager.DefaultTenantID);
                            applicationUrl = webSite.URL;
                        }
                        applicationUrl = applicationUrl + "/Login.aspx?UsrVerCode=" + organizationUser.UserVerificationCode;
                        Dictionary<String, String> args = new Dictionary<String, String>();
                        args.ToDecryptedQueryString(View.EncPValue);
                        String password = String.Empty;
                        if (args.ContainsKey("EncPValue"))
                        {
                            password = Convert.ToString(args["EncPValue"]);
                        }
                        return SecurityManager.SendEmailForNewApplicant(organizationUser, applicationUrl, password);
                    }
                }
            }

            return false;
        }

        public void GetWebsiteFooter()
        {
            if (View.WebSiteId > 0)
            {
                WebSiteWebConfig webSiteWebConfig = WebSiteManager.GetWebSiteWebConfig(View.WebSiteId);
                if (webSiteWebConfig != null)
                {
                    View.lstWebsitePages = webSiteWebConfig.WebSite.WebSiteWebPages.Where(webPage => !webPage.IsDeleted && webPage.IsActive).OrderBy(linkOrder => linkOrder.LinkOrder).ToList();
                    View.FooterHtml = webSiteWebConfig.FooterText;
                }
            }
        }

        public void GetImageUrl()
        {
            if (View.WebSiteId > 0)
                View.LoginPageImageUrl = WebSiteManager.GetWebSiteLoginImage(View.WebSiteId);
        }
    }
}
