using CoreWeb.IntsofSecurityModel.Views;
using CoreWeb.Shell;
using INTSOF.UI.Contract.CommonControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.CommonOperations.Pages
{
    public partial class AccountLinkingPage : BaseWebPage, IAccountLinkingView
    {
        private AccountLinkingPresenter _presenter = new AccountLinkingPresenter();
        private const String NONE_OF_THESE = "None of the above, Create new account.";

        #region Public Properties
        public AccountLinkingPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this; ;
            }
        }
        AccountLinkingProfileContract IAccountLinkingView.accountLinkingProfileContract
        {
            get
            {
                if (!ViewState["accountLinkingProfileContract"].IsNull())
                {
                    return (ViewState["accountLinkingProfileContract"]) as AccountLinkingProfileContract;
                }
                return new AccountLinkingProfileContract();
            }
            set
            {
                ViewState["accountLinkingProfileContract"] = value;
            }
        }

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

        public IAccountLinkingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        public List<LookupContract> ExistingUsersList
        {
            set
            {
                //LookupContract lookupContract = new LookupContract();
                //lookupContract.Name = NONE_OF_THESE;
                //lookupContract.Code = NONE_OF_THESE;
                //value.Add(lookupContract);
                rblExistingProfiles.DataSource = value;
                rblExistingProfiles.DataBind();
            }
        }
        #endregion

        #region [Events]
        #region [Page Events]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var res = (AccountLinkingProfileContract)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ACCOUNT_LINKING_SESSION_KEY);
                if (!res.IsNullOrEmpty())
                {
                    CurrentViewContext.accountLinkingProfileContract = res;
                    SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ACCOUNT_LINKING_SESSION_KEY, null);
                    Presenter.IsExistingUser();
                }
            }
        }
        #endregion

        #region [Button Events]
        protected void cbExistingProfiles_CancelClick(object sender, EventArgs e)
        {
            SetSessionData(true, false, String.Empty, String.Empty);
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "closePopUp();", true);
        }

        protected void cbExistingProfiles_SaveClick(object sender, EventArgs e)
        {
            String selectedProfile = rblExistingProfiles.SelectedValue;
            SetSessionData(false, true, "Success", selectedProfile);
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "closePopUp();", true);
        }
        #endregion


        #region [Methods]
        private void SetSessionData(Boolean isCancelButtonClicked, Boolean isProfileLinked, String response, String selectedLinkingProfileOrgUsername)
        {
            AccountLinkingProfileContract accountLinkingProfileContract = new AccountLinkingProfileContract();
            accountLinkingProfileContract = CurrentViewContext.accountLinkingProfileContract;
            accountLinkingProfileContract.SelectedLinkingProfileOrgUsername = selectedLinkingProfileOrgUsername;
            accountLinkingProfileContract.IsCancelButtonClicked = isCancelButtonClicked;
            accountLinkingProfileContract.IsProfileLinked = isProfileLinked;
            accountLinkingProfileContract.ProfileLinkingResponse = response;
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ACCOUNT_LINKING_SESSION_KEY, accountLinkingProfileContract);
        }
        #endregion
        #endregion
    }
}