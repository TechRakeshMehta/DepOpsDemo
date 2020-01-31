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
    public partial class AnnouncementPopup : BaseWebPage, IAnnouncementPopupView
    {
        #region Private Variables

        private AnnouncementPopupPresenter _presenter = new AnnouncementPopupPresenter();

        #endregion

        #region PUBLIC PROPRTIES

        public AnnouncementPopupPresenter Presenter
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

        public IAnnouncementPopupView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IAnnouncementPopupView.LoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        AnnouncementContract IAnnouncementPopupView.ViewContract
        {
            get;
            set;
        }

        Int32 IAnnouncementPopupView.AnnouncementID
        {
            get
            {
                return Convert.ToInt32(ViewState["AnnouncementID"]);
            }
            set
            {
                ViewState["AnnouncementID"] = value;
            }
        }

        #endregion

        #region EVENTS

        #region PAGE EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Request["AnnouncementID"].IsNullOrEmpty())
                {
                    CurrentViewContext.AnnouncementID = Convert.ToInt32(Request["AnnouncementID"]);
                }
                //Insert Announcement Mapping
                Presenter.SaveAnnouncementMapping();
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
            Presenter.GetAnnouncementDetail();
            if (CurrentViewContext.ViewContract.IsNotNull())
            {
                //rdEditorNotes.Content = CurrentViewContext.ViewContract.AnnouncementText;
                litHTML.Text = CurrentViewContext.ViewContract.AnnouncementText.HtmlDecode();
            }
        }

        #endregion
    }
}