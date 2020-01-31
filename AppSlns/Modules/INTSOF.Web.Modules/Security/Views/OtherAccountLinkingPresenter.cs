using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace CoreWeb.IntsofSecurityModel.Views
{
    public class OtherAccountLinkingPresenter : Presenter<IOtherAccountLinking>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public Boolean ValidateEmail()
        {
            //List<Entity.OrganizationUser> lstUser = SecurityManager.GetOrganizationUsersByEmail(View.EmailID);
            //if (lstUser.IsNullOrEmpty() || lstUser.Count == AppConsts.NONE)
            //    return false;
            //else
            //    return true;
            return SecurityManager.IsOrganizationUserExistsForEmail(View.EmailID);
        }

        public void ValidateEmailandGetUsers()
        {
            View.ExistingUser = SecurityManager.GetExistingUserBasedOnEmailId(View.EmailID, new Guid(View.SourceUserId), View.SourceOrgUserId);
        }


        /// <summary>
        /// It validates the user, and then redirect to login page.
        /// </summary>
        public Boolean ValidateUserNameAndPassword()
        {
            SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(View.UserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;

            if (System.Web.Security.Membership.ValidateUser(Regex.Replace(View.UserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE), Regex.Replace(View.Password, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)))
            {
                // Checks if the user is locked.
                if (!user.IsNull() && user.IsLockedOut)
                {
                    return false;
                }
                Entity.OrganizationUser orgUser = SecurityManager.GetOrganizationUser(user.OrganizationUserId);
                if (orgUser.UserID.ToString() == View.TargetUserID)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Boolean LinkAccount(String SelectedUserId, String otherAccountUserId)
        {
            return SecurityManager.LinkOtherAccount(new Guid(SelectedUserId), new Guid(otherAccountUserId), View.CurrentLoggedinOrgUserID);
        }

        public void GetSourceTargetDetails()
        {
            View.lstUserContract = new List<ManageUsersContract>();
            Entity.OrganizationUser orgUser = SecurityManager.GetOrganizationUser(View.SourceOrgUserId);
            ManageUsersContract sourceDetails = new ManageUsersContract();
            sourceDetails.UserName = orgUser.aspnet_Users.UserName;
            sourceDetails.EmailAddress = orgUser.aspnet_Users.aspnet_Membership.Email;
            sourceDetails.UserId = Convert.ToString(orgUser.UserID);
            View.lstUserContract.Add(sourceDetails);

            ManageUsersContract targetDetails = new ManageUsersContract();
            targetDetails.UserName = View.UserName;
            targetDetails.EmailAddress = View.EmailID;
            targetDetails.UserId = View.TargetUserID.ToString();
            View.lstUserContract.Add(targetDetails);
        }
    }
}
