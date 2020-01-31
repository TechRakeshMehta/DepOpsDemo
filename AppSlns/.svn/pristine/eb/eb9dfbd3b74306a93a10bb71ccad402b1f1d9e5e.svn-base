using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.PackageBundleManagement;

namespace CoreWeb.CommonOperations.Views
{
    public partial class BulletinPopup : BaseWebPage, IBulletinPopupView
    {
        #region Private Variables

        private BulletinPopupPresenter _presenter = new BulletinPopupPresenter();

        #endregion

        #region PUBLIC PROPRTIES

        public BulletinPopupPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public IBulletinPopupView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IBulletinPopupView.LoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        BulletinContract IBulletinPopupView.ViewContract
        {
            get;
            set;
        }

        Int32 IBulletinPopupView.BulletinID
        {
            get
            {
                return Convert.ToInt32(ViewState["BulletinID"]);
            }
            set
            {
                ViewState["BulletinID"] = value;
            }
        }

        #endregion

        #region EVENTS

        #region PAGE EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Request["BulletinID"].IsNullOrEmpty())
                {
                    CurrentViewContext.BulletinID = Convert.ToInt32(Request["BulletinID"]);
                }
                //Insert Bulletin Mapping
                Presenter.SaveBulletinMapping();
                //Bind controls
                BindControls();
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Bind controls
        /// </summary>
        private void BindControls()
        {
            Presenter.GetBulletinDetail();
            if (CurrentViewContext.ViewContract.IsNotNull())
            {
                litHTML.Text = CurrentViewContext.ViewContract.BulletinContent;
            }
        }

        #endregion
    }
}