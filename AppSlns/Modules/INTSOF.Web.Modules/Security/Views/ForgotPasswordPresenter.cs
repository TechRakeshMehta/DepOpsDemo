#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ForgotPasswordPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using INTSOF.Utils;
using Business.RepoManagers;
using Entity;
using System.Collections.Generic;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles all the CRUD(Create/ Read/ Update/ Delete) operations for forgot password.
    /// </summary>
    public class ForgotPasswordPresenter : Presenter<IForgotPasswordView>
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        public int GetWebsiteTenantId(string websiteUrl)
        {
            return WebSiteManager.GetWebsiteTenantId(websiteUrl);
        }

        public bool IsLocationServiceTenant(int tenantId)
        {
            return SecurityManager.IsLocationServiceTenant(tenantId);
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///  Use to generate the code.
        /// </summary>
        public void HandleGenerateCode(String regEmailReq)
        {
            List<OrganizationUser> lstOrganizationUser = SecurityManager.GetOrganizationUsersByEmail(View.ViewContract.Email);

            if (lstOrganizationUser.IsNotNull() && lstOrganizationUser.Count > 0)
            {
                foreach (OrganizationUser organizationUser in lstOrganizationUser)
                {
                    organizationUser.VerificationCode = View.ViewContract.VerificationCode;
                    SecurityManager.UpdateOrganizationUser(organizationUser);
                }
                OrganizationUser tempOrgUser = lstOrganizationUser.FirstOrDefault();
                View.ViewContract.UserName = tempOrgUser.FirstName + " " + tempOrgUser.LastName;
                // View.ViewContract.TenantName =tempOrgUser.Organization.Tenant.TenantName;
                View.ViewContract.OperationStatus = true;
                View.ViewContract.OrganizationUserId = tempOrgUser.OrganizationUserID;
            }
            else
            {
                View.ViewContract.OperationStatus = false;
                //View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_REGISTERED_EMAIL_REQUIRED);  REGISTEREDEMAILREQUIRED
                View.ErrorMessage = regEmailReq;
            }
        }

        /// <summary>
        ///  Use to save.
        /// </summary>
        public void HandleSave()
        {
            List<OrganizationUser> lstOrganizationUser = SecurityManager.GetOrganizationUsersByEmail(View.ViewContract.Email);

            if (lstOrganizationUser.IsNotNull() && lstOrganizationUser.Count > 0 && !lstOrganizationUser.Any(x => x.VerificationCode != View.ViewContract.VerificationCode))
            {
                OrganizationUser tempOrgUser = lstOrganizationUser.FirstOrDefault();
                if (View.IsUserNameReset)
                {
                    View.ViewContract.LoginUserName = tempOrgUser.aspnet_Users.UserName;
                }

                // 28/08/2014 UAT-700 Implementated
                //else
                //{
                //foreach (OrganizationUser organizationUser in lstOrganizationUser)
                //{
                //    organizationUser.aspnet_Users.aspnet_Membership.Password = SysXMembershipUtil.HashPasswordIWithSalt(View.ViewContract.ResetPassword, organizationUser.aspnet_Users.aspnet_Membership.PasswordSalt);
                //    organizationUser.IsNewPassword = true;
                //    SecurityManager.UpdateOrganizationUser(organizationUser);
                //}
                //}
                View.ViewContract.OrganizationUserId = tempOrgUser.OrganizationUserID;
                View.ViewContract.UserName = tempOrgUser.FirstName + " " + tempOrgUser.LastName;
                //  View.ViewContract.TenantName = tempOrgUser.Organization.Tenant.TenantName;
                View.ViewContract.OperationStatus = true;
            }
            else
            {
                View.ViewContract.OperationStatus = false;
                //View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_CORRECT_VERIFICATION_CODE);
                View.ErrorMessage = View.IncorrectVerificationCode;
            }
        }

        #endregion

        #endregion
    }
}