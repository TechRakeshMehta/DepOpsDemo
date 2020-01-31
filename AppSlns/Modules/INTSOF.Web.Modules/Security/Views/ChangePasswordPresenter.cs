#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ChangePasswordPresenter.cs
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
    /// This class handles all the CRUD(Create/ Read/ Update/ Delete) operations for change password
    /// section of security module.
    /// </summary>
    public class ChangePasswordPresenter : Presenter<IChangePasswordView>
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

        /// <summary>
        /// This method is invoked by the view every time it loads.
        /// </summary>
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed the first time the view loads.
        }

        /// <summary>
        /// This method is invoked by the view the first time it loads.
        /// </summary>
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads.
        }

        /// <summary>
        /// Updates the password.
        /// </summary>
        public void UpdatePassword(string email = null)
        {
            //OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.ViewContract.OrganizationUserId);
            List<OrganizationUser> lstOrganizationUser = null;

            String oldPassword = String.Empty;//UAT-4097
            if (email.IsNullOrEmpty())
            {
                lstOrganizationUser = SecurityManager.GetOrganizationUsersByEmail(View.ViewContract.Email);

                oldPassword = SysXMembershipUtil.HashPasswordIWithSalt(View.OldPassword, lstOrganizationUser.FirstOrDefault().aspnet_Users.aspnet_Membership.PasswordSalt);

                //if (!organizationUser.aspnet_Users.aspnet_Membership.Password.Equals(oldPassword))
                if (!lstOrganizationUser.Any(x => x.aspnet_Users.aspnet_Membership.Password.Equals(oldPassword)))
                {
                    //View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_OLD_PASSWORD_MATCHING);
                    View.ErrorMessage = View.OLDPASWRDNOTMATCH;
                    View.ViewContract.OperationStatus = false;
                    return;
                }
            }
            else
            {
                lstOrganizationUser = SecurityManager.GetOrganizationUsersByEmail(email);

                if (!lstOrganizationUser.IsNullOrEmpty())
                    oldPassword = SysXMembershipUtil.HashPasswordIWithSalt(View.OldPassword, lstOrganizationUser.FirstOrDefault().aspnet_Users.aspnet_Membership.PasswordSalt);
            }
            if (!lstOrganizationUser.IsNullOrEmpty())
                SecurityManager.UpdatePasswordDetails(lstOrganizationUser.FirstOrDefault(), lstOrganizationUser.FirstOrDefault().OrganizationUserID, oldPassword); // This will update the PasswordHistory table too.

            foreach (OrganizationUser organizationUser in lstOrganizationUser)
            {
                organizationUser.aspnet_Users.aspnet_Membership.Password = SysXMembershipUtil.HashPasswordIWithSalt(View.NewPassword, organizationUser.aspnet_Users.aspnet_Membership.PasswordSalt);
                organizationUser.aspnet_Users.aspnet_Membership.LastPasswordChangedDate = DateTime.Now;
                organizationUser.IsNewPassword = false;
                if (View.DefaultLineOfBusiness.IsNotNull() && View.DefaultLineOfBusiness.Count() > 0)
                {
                    organizationUser.SysXBlockID = Convert.ToInt32(View.DefaultLineOfBusiness);
                }
                SecurityManager.UpdateOrganizationUser(organizationUser);
            }
            //if (View.OrgUsrID > AppConsts.NONE)
            // SecurityManager.UpdatePasswordDetails(lstOrganizationUser.FirstOrDefault(), lstOrganizationUser.FirstOrDefault().OrganizationUserID, oldPassword); // This will update the PasswordHistory table too.
            //else
            //    SecurityManager.UpdatePasswordDetails(lstOrganizationUser.FirstOrDefault(), lstOrganizationUser.FirstOrDefault().OrganizationUserID, oldPassword); // This will update the PasswordHistory table too.

            View.ViewContract.OperationStatus = true;
            View.ErrorMessage = View.PSWRDCHNGESUCSESFLY;
            //SysXUtils.GetMessage(ResourceConst.SECURITY_CHANGE_PASSWORD_SUCCESSFULLY);
        }

        public Boolean IsPasswordExistsInHistory(Guid userId, Boolean isUserExixtInLocationTenants)
        {
            return SecurityManager.IsPasswordExistsInHistory(userId, View.NewPassword, isUserExixtInLocationTenants);
        }
        public Boolean IsUserExixtInLocationTenants(Guid userId)
        {
            return SecurityManager.IsUserExixtInLocationTenants(userId);
        }

        /// <summary>
        /// Retrieves all the Line of Businesses based on current user's Id.
        /// </summary>
        /// <param name="currentUserId">value of current user's Id.</param>
        /// <returns></returns>
        public IQueryable<vw_UserAssignedBlocks> GetLineOfBusinessesByUser(String currentUserId)
        {
            return SecurityManager.GetLineOfBusinessesByUser(currentUserId);
        }

        public Boolean? CheckIfUserIsApplicantOrSharedUser(Int32 userId)
        {

            Boolean? isapplicant = (SecurityManager.GetOrganizationUser(userId).IsApplicant ?? false) || (SecurityManager.GetOrganizationUser(userId).IsSharedUser ?? false);
            return isapplicant;
        }
        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}