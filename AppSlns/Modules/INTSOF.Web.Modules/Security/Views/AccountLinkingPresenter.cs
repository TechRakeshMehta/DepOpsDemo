using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
using INTSOF.Utils;
using System.Text.RegularExpressions;
using INTSOF.Utils.Consts;
using System.Security.Cryptography;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Configuration;
using INTSOF.Utils.SsoHandlers;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public class AccountLinkingPresenter : Presenter<IAccountLinkingView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }
        public Boolean IsExistingUser()
        {
            if (!View.accountLinkingProfileContract.IsNullOrEmpty())
            {
                List<LookupContract> lstExistingUser = SecurityManager.GetExistingUserProfileLists(View.accountLinkingProfileContract.Username, View.accountLinkingProfileContract.UserEmail);
                if (lstExistingUser.Count > 0)
                {
                    View.ExistingUsersList = lstExistingUser;
                    return true;
                }
            }
            return false;
        } 
    }
}




