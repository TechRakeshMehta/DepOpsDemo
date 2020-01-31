using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity;
using System.Linq;
using Business.RepoManagers;
using System.Web.Security;
using System.Text.RegularExpressions;
using INTSOF.Utils.Consts;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public class SelectBuisnessChannelPresenter : Presenter<ISelectBuisnessChannelView>
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
        /// Retrieves all the Line of Businesses based on current user's Id.
        /// </summary>
        /// <param name="currentUserId">value of current user's Id.</param>
        /// <returns></returns>
        public IQueryable<vw_UserAssignedBlocks> GetLineOfBusinessesByUser(String currentUserId)
        {
            return SecurityManager.GetLineOfBusinessesByUser(currentUserId);
        }

        public Int32 GetDefaultLineOfBusinesses(Int32 organizationUserId)
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(organizationUserId);
            Int32 selected=organizationUser.SysXBlockID.HasValue?organizationUser.SysXBlockID.Value:0;

            return selected;
        }


        /// <summary>
        /// Retrieves default Line of Businesses based on current user's Id.
        /// </summary>
        /// <param name="currentUserId">value of current user's Id.</param>
        /// <returns></returns>
        public String GetDefaultLineOfBusinessOfLoggedInUser(Int32 currentUserId)
        {
            return SecurityManager.GetOrganizationUser(currentUserId).SysXBlockID.GetValueOrDefault().ToString();
        }
        
        /// <summary>
        /// It validates the user, and then redirect to login page.
        /// </summary>
        public void ChangeBuisnessChannelAndRedirect(Int32 selecteditemID)
        {
           
           
        }

        public List<lkpBusinessChannelType> GetBusinessChannelTypes()
        {
          return  SecurityManager.GetBusinessChannelTypes();
        }
    }
}




