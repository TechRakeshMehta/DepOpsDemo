using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using Telerik.Web.UI;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System.Web.UI.HtmlControls;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ApplicantDataEntryHelp : BaseUserControl, IApplicantDataEntryHelpView
    {
        #region Variables

        #region Private Variables

        private ApplicantDataEntryHelpPresenter _presenter = new ApplicantDataEntryHelpPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties


        public ApplicantDataEntryHelpPresenter Presenter
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

        public IApplicantDataEntryHelpView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 TenantID
        {
            get;
            set;
        }

        public String PageHTML
        {
            get
            {
                return litHTML.Text;
            }
            set
            {
                litHTML.Text = value;
            }
        }
        public Int32 PackageID
        {
            get;
            set;
        }

        public String PackageName
        {
            get;
            set;
        }

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }

        public String ControlUseType
        {
            get
            {
                if (ViewState["ControlUseType"] != null)
                    return (Convert.ToString(ViewState["ControlUseType"]));
                return String.Empty;
            }
            set
            {
                ViewState["ControlUseType"] = value;
            }
        }
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
                base.OnInit(e);
                //base.Title = "Start Here";
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

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Dictionary<String, String> args = new Dictionary<String, String>();
            //if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            //{
            //    args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            //}

            //CurrentViewContext.TenantID = args.ContainsKey("TenantId") ? Convert.ToInt32(args["TenantId"]) : AppConsts.NONE;
            //change done for applicant dashboard redesign.

            if (Request.QueryString["TenantId"] != null)
            {
                CurrentViewContext.TenantID = Convert.ToInt32(Request.QueryString["TenantId"].ToString());
            }
            if (Request.QueryString["PackageId"] != null)
            {
                CurrentViewContext.PackageID = Convert.ToInt32(Request.QueryString["PackageId"].ToString());
            }
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            if (ControlUseType == AppConsts.DASHBOARD)
            {
                manageDownloadPdf.TenantID = CurrentViewContext.TenantID;
                manageDownloadPdf.PackageID = CurrentViewContext.PackageID;
                manageDownloadPdf.PackageName = PackageName;

            }
            Presenter.OnViewLoaded();
            //base.SetPageTitle("Start Here");
        }

        #endregion

        #endregion
    }
}

