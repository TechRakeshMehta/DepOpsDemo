using CoreWeb.Shell;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.AdminEntryPortal.Views
{
    public partial class AdminEntryOrderConfirmation : BaseUserControl, IAdminEntryOrderConfirmationView
    {
        #region Variables

        #region Private Variables

        private AdminEntryOrderConfirmationPresenter _presenter = new AdminEntryOrderConfirmationPresenter();

        #endregion

        #endregion

        #region Properties

        #region public Properties

        public AdminEntryOrderConfirmationPresenter Presenter
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

        public IAdminEntryOrderConfirmationView CurrentViewContext
        {
            get { return this; }
        }
        #endregion
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                lblAdminEntryConfirmationMessage.Text = "Your order has been placed successfully.";
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }

        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            try
            {
                SysXWebSiteUtils.SessionService.ClearSession(true);
                FormsAuthentication.RedirectToLoginPage();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
    }
}