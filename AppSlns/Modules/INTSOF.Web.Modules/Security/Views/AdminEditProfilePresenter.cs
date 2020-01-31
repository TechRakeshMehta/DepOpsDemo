#region Header Comment Block
// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  AdminEditProfilePresenter.cs
// Purpose:   
//

#endregion

#region Namespace

#region System Defined

using System;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Business.RepoManagers;
using System.Collections.Generic;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion


namespace CoreWeb.IntsofSecurityModel.Views
{
    public class AdminEditProfilePresenter : Presenter<IAdminEditProfileView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// To get Org User Data
        /// </summary>
        public void GetOrgUserData()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);
            View.UserName = organizationUser.aspnet_Users.UserName;
            View.FirstName = organizationUser.FirstName;
            View.LastName = organizationUser.LastName;
            View.PrimaryEmail = organizationUser.aspnet_Users.aspnet_Membership.Email;
            //View.PrimaryEmail = organizationUser.PrimaryEmailAddress;
        }

        /// <summary>
        /// To check if User exists
        /// </summary>
        /// <returns></returns>
        public Boolean IsExistingUser()
        {
            List<LookupContract> lstExistingUser = SecurityManager.GetExistingUserLists(View.UserName, DateTime.Now, null, View.FirstName, View.LastName,false);
            if (lstExistingUser.IsNotNull() && lstExistingUser.Any())
            {
                var _tempUserList = lstExistingUser.Where(x => x.UserID != View.CurrentLoggedInUserId).ToList();
                if (_tempUserList.Any(x => x.Code.Trim().ToLower() == View.UserName.Trim().ToLower()))
                    return true;
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        /// To save user profile data
        /// </summary>
        /// <returns></returns>
        public Boolean SaveUserData()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);

            organizationUser.aspnet_Users.UserName = View.UserName;
            organizationUser.aspnet_Users.LoweredUserName = View.UserName.ToLower();
            organizationUser.ModifiedByID = View.CurrentLoggedInUserId;
            organizationUser.ModifiedOn = DateTime.Now;

            if (SecurityManager.UpdateOrganizationUser(organizationUser))
            {
                //SecurityManager.SynchoniseUserProfile(organizationUser.OrganizationUserID, View.SelectedTenantId);
                //CommunicationManager.SendMailOnProfileChange(organizationUser);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// UAT-2930
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean ShowhideTwoFactorAuthentication(String userId)
        {
            return SecurityManager.ShowhideTwoFactorAuthentication(userId);
        }
    }
}




