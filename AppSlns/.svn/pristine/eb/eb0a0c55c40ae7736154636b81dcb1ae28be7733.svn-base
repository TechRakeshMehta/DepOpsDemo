using CoreWeb.ComplianceOperations.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Pages
{
    public partial class ComplianceSearchNotesPopUp : BaseWebPage, IComplianceSearchNotesView
    {
        #region Private variables
        private ComplianceSearchNotesPresenter _presenter = new ComplianceSearchNotesPresenter();
        int _tenantid = 0;
        #endregion
        #region   Public Properties

        ComplianceSearchNotesPresenter Presenter
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

        IComplianceSearchNotesView CurrentViewContext
        {
            get { return this; }
        }

        int IComplianceSearchNotesView.PackageSubscriptionID
        {
            get;
            set;
        }

        int IComplianceSearchNotesView.CompliancePackageID
        {
            get;
            set;
        }

        int IComplianceSearchNotesView.OrderID
        {
            get;
            set;
        }

        string IComplianceSearchNotesView.Notes
        {
            get;
            set;
        }

        int IComplianceSearchNotesView.CurrentLoggedInUserOrgId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }

        }

        int IComplianceSearchNotesView.OrganizationUserId
        {
            get;
            set;
        }
        int IComplianceSearchNotesView.SelectedTenantId
        {
            get;
            set;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    _presenter.OnViewInitialized();
                    if (!Request.QueryString["pkgSubscriptionId"].IsNull())
                    {
                        CurrentViewContext.PackageSubscriptionID = Convert.ToInt32(Request.QueryString["pkgSubscriptionId"]);
                    }

                    if (!Request.QueryString["selectedTenantId"].IsNull())
                    {
                        CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["selectedTenantId"]);
                    }


                    Presenter.GetComplianceSearchNote();
                    txtNotes.Text = CurrentViewContext.Notes;
                }
                Page.Title = "Compliance Search Note";
                _presenter.OnViewLoaded();
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

        #region Button Events
        /// <summary>
        /// Save Client Status 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucFeatureClientStatus_SaveClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.Notes = txtNotes.Text.Trim();
                if (!Request.QueryString["pkgSubscriptionId"].IsNull())
                {
                    CurrentViewContext.PackageSubscriptionID = Convert.ToInt32(Request.QueryString["pkgSubscriptionId"]);
                }
                else { CurrentViewContext.PackageSubscriptionID = 0; }
                if (!Request.QueryString["selectedTenantId"].IsNull())
                {
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["selectedTenantId"]);
                }
                if (Presenter.SaveComplianceSearchNote())
                {
                    this.ShowSuccessMessage("Note saved successfully");
                }
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

        #endregion
    }
}