#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using CoreWeb.Shell;

#endregion

#region UserDefined

using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils;
using System.Web.UI.HtmlControls;
using CoreWeb.ComplianceOperations.Views;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations
{
    public partial class ManageApplicantDocument : BaseUserControl, IManageApplicantDocumentView
    {
        #region Variables

        #region Private

        private ManageApplicantDocumentPresenter _presenter = new ManageApplicantDocumentPresenter();
        private Int32 _tenantId;
        private String _viewType;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public ManageApplicantDocumentPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IManageApplicantDocumentView CurrentViewContext
        {
            get
            {
                return this;
            }

        }

        public Boolean IsControlLoaded { get; set; }

        #endregion

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled
        /// to fire the DI engine.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                ucManageUploadDocument.Visible = true;
                ucManagePersonalDocument.Visible = false;
                liHome.Attributes.Add("class", "active");
                hdnSelectedTab.Value = "1";

                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("TAB"))
                    {
                        if ((Convert.ToString(args["TAB"]) == "REQUIREMENT"))
                        {
                            hdnSelectedTab.Value = "2";
                            HandleTabs(ucManagePersonalDocument.ClientID, false);
                        }
                    }
                    //UAT-2729
                    if (args.ContainsKey("isUploadOrViewDocument"))
                    {
                        if (Convert.ToString(args["isUploadOrViewDocument"]) == "View")
                        {
                            lnkBacKToComplianceTracking.Text = "Back to Dashboard";
                        }
                    }
                }
            }
            SetReturnTabFocus();
            BasePage basePage = base.Page as BasePage;

            if (ucManageUploadDocument.Visible && hdnSelectedTab.Value == "1")
            {
                ucManageUploadDocument.Visible = true;
            }
            else
            {
                ucManageUploadDocument.Visible = false;
            }
            ucManageUploadDocument.IsAllowedFileExtensionEnable = true;
        }
        protected void tabHome_Click(object sender, EventArgs e)
        {
            HandleTabs(ucManageUploadDocument.ClientID, true);
        }

        protected void tabRequirement_ServerClick(object sender, EventArgs e)
        {
            HandleTabs(ucManagePersonalDocument.ClientID, false);
        }
        #endregion

        private void HandleTabs(String controlID, Boolean isRotationRebindRequired)
        {
            ucManageUploadDocument.Visible = false;
            ucManagePersonalDocument.Visible = false;
            liHome.Attributes.Remove("class");
            liRequirement.Attributes.Remove("class");
            if (ucManageUploadDocument.ClientID == controlID)
            {
                ucManageUploadDocument.Visible = true;
                ucManagePersonalDocument.Visible = false;
                liHome.Attributes.Add("class", "active");
                ucManageUploadDocument.ReloadGrid();
            }
            else if (ucManagePersonalDocument.ClientID == controlID)
            {
                ucManagePersonalDocument.Visible = true;
                ucManageUploadDocument.Visible = false;
                ucManagePersonalDocument.ReloadGrid();
                IsControlLoaded = true;
                liRequirement.Attributes.Add("class", "active");
            }

        }

        /// <summary>
        /// Set the tab focus when user retruns from the Details screen.
        /// </summary>
        private void SetReturnTabFocus()
        {
            var _srcTab = Convert.ToString(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SHARED_USER_GRID_SOURCE));

            Session.Remove(AppConsts.SESSION_SHARED_USER_GRID_SOURCE);

            // Handle Normal Page Load after login
            if (_srcTab.IsNullOrEmpty())
            {
                return;
            }

            if (_srcTab.IsNullOrEmpty() || _srcTab == SharedUserGridSource.ROTATION_WIDGET_TAB.GetStringValue())
            {
                hdnSelectedTab.Value = "1";
                HandleTabs(ucManageUploadDocument.ClientID, false);
            }
            else if (_srcTab == SharedUserGridSource.REQUIREMENT_SHARES.GetStringValue())
            {
                hdnSelectedTab.Value = "2";
                HandleTabs(ucManagePersonalDocument.ClientID, false);
            }
        }

        #endregion

        //UAT-2729 : Add link on manage documents screen to return applicant to their compliance tracking. 
        protected void lnkBacKToComplianceTracking_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(AppConsts.APPLICANT_MAIN_PAGE_NAME);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
    }
}