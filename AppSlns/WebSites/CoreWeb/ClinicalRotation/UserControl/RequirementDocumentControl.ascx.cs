using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ClinicalRotation.Views;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.UserControl
{
    public partial class RequirementDocumentControl : BaseUserControl, IRequirementDocumentControlView
    {
        #region VARIABLES
        private String _viewType;
        private RequirementDocumentControlPresenter _presenter = new RequirementDocumentControlPresenter();

        #endregion

        #region PRESENTER OBJECT

        public RequirementDocumentControlPresenter Presenter
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
        #endregion

        #region PAGE EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID] == null ? String.Empty : Request.QueryString[AppConsts.UCID];

            if (!this.IsPostBack)
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    CaptureQuerystringParameters(args);
                }
                Presenter.OnViewInitialized();
                HiddenField hdnFirstDocumentID = this.Parent.FindControl("hdnFirstDocumentID") as HiddenField;
                String firstDocumentID = String.Empty;
                if (hdnFirstDocumentID.IsNotNull())
                {
                    firstDocumentID = hdnFirstDocumentID.Value;
                }
                Dictionary<String, String> requestDocViewerArgs = new Dictionary<String, String>();
                requestDocViewerArgs = new Dictionary<String, String>
                                                                 { 
                                                                    {"DocumentID", firstDocumentID },
                                                                    {"SelectedTenantID", Convert.ToString(CurrentViewContext.SelectedTenantId)},                                                                    
                                                                 };
                string url = String.Format(@"/ClinicalRotation/Pages/RequirementVerificationDocViewer.aspx?args={0}", requestDocViewerArgs.ToEncryptedQueryString());
                hdnADEDocVwr.Value = Convert.ToString(url);
            }

            Presenter.OnViewLoaded();

        }

        #endregion

        #region METHODS

        /// <summary>
        /// This Methods set the different property on the page extracting from the query string.
        /// </summary>
        /// <param name="args">Query string parameter thsat contains the value.</param>
        private void CaptureQuerystringParameters(Dictionary<string, string> args)
        {
            if (args.IsNotNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey(ProfileSharingQryString.SelectedTenantId))
                {
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(args[ProfileSharingQryString.SelectedTenantId]);
                }
                if (args.ContainsKey(ProfileSharingQryString.RotationId))
                {
                    CurrentViewContext.ClinicalRotationId = Convert.ToInt32(args[ProfileSharingQryString.RotationId]);
                }
                if (args.ContainsKey(ProfileSharingQryString.ApplicantId))
                {
                    CurrentViewContext.CurrentApplicantId = Convert.ToInt32(args[ProfileSharingQryString.ApplicantId]);
                }
            }

        }

        /// <summary>
        /// Binds the document to the rotator specific to the category on page load
        /// and all documents related to the client.
        /// </summary>
        private void BindDocuments()
        {

        }

        #endregion

        #region PROPERTIES
        public IRequirementDocumentControlView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IRequirementDocumentControlView.SelectedTenantId
        {
            get
            {
                return (Int32)(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        Int32 IRequirementDocumentControlView.OrganizationUserId
        {
            get
            {
                return (Int32)(ViewState["OrganizationUserId_User"]);
            }
            set
            {
                ViewState["OrganizationUserId_User"] = value;
            }
        }

        Int32 IRequirementDocumentControlView.CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        Int32 IRequirementDocumentControlView.ItemDataId
        {
            get
            {
                return (Int32)(ViewState["ItemDataId"]);
            }
            set
            {
                ViewState["ItemDataId"] = value;
            }
        }

        List<ApplicantDocuments> IRequirementDocumentControlView.lstApplicantDocument
        {
            get;
            set;
        }

        Int32 IRequirementDocumentControlView.CurrentApplicantId
        {
            get
            {
                return (Int32)(ViewState["CurrentApplicantId"]);
            }
            set
            {
                ViewState["CurrentApplicantId"] = value;
            }
        }

        Int32 IRequirementDocumentControlView.ClinicalRotationId
        {
            get
            {
                return (Int32)(ViewState["ClinicalRotationId"]);
            }
            set
            {
                ViewState["ClinicalRotationId"] = value;
            }
        }
        #endregion

    }
}