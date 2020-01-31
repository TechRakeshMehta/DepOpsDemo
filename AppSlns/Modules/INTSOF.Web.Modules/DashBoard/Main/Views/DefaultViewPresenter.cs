using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
using Entity.SharedDataEntity;

namespace CoreWeb.Main.Views
{
    public class DefaultViewPresenter : Presenter<IDefaultView>
    {

        public DefaultViewPresenter()
        {
        }

        public Entity.OrganizationUser LoginUser(Int32 currentLoggedInUserId)
        {
            Entity.OrganizationUser objOrg = SecurityManager.GetOrganizationUser(currentLoggedInUserId);
            return objOrg;
        }

        public Boolean? CheckIfUserIsApplicant(Int32 currentLoggedInUserId)
        {
            Boolean? isapplicant = SecurityManager.GetOrganizationUser(currentLoggedInUserId).IsApplicant;
            return isapplicant;
        }

        public Boolean CheckIfApplicantHasPlacedOrder(Int32 currentLoggedInUserId)
        {
            return ComplianceDataManager.CheckIfApplicantHasPlacedOrder(View.TenantId, currentLoggedInUserId);
        }

        // Check If Applicant Has any Order with Payment Due.
        public Boolean CheckIfApplicantHasPaymentDue(Int32 currentLoggedInUserId)
        {
            return ComplianceDataManager.CheckIfApplicantHasPaymentDue(View.TenantId, currentLoggedInUserId);
        }

        /// <summary>
        /// Get the applicant orders to check if applicant has placed any order and 
        /// user further to check if any payment is due.
        /// </summary>
        public List<Entity.ClientEntity.Order> GetApplicantOrders(Int32 currentLoggedInUserId)
        {
            return ComplianceDataManager.GetApplicantOrders(View.TenantId, currentLoggedInUserId);
        }

        /// <summary>
        /// Check If Applicant Has any Order with Payment Due.
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public Boolean CheckApplicantPaymentDue(Int32 currentLoggedInUserId, List<Entity.ClientEntity.Order> lstOrders)
        {
            return ComplianceDataManager.CheckApplicantPaymentDue(currentLoggedInUserId, lstOrders, View.TenantId);
        }

        public UserLoginHistory AddUserLoginActivity(Int32 currentLoggedInUserId, String sessionID)
        {
           return SecurityManager.AddUserLoginActivity(currentLoggedInUserId, sessionID);
        }


        public ProfileSharingInvitation GetInvitationDataByToken(Guid inviteToken)
        {
            return ProfileSharingManager.GetInvitationDataByToken(inviteToken);
        }

        //UAT-2519
        public String GetApplicantNameByID(Int32 AppOrgUserID)
        {
            OrganizationUser OrgUsers = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(AppOrgUserID);
            if (OrgUsers != null)
                return OrgUsers.FirstName + " " + OrgUsers.LastName;
            return String.Empty;
        }
    }
}
