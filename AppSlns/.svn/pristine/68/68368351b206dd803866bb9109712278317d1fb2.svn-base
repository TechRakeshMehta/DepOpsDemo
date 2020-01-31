using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using Telerik.Web.UI;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.IO;
using System.Threading;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManagePersonalDocument : BaseUserControl, IManagePersonalDocumentView
    {
        #region Variables

        #region Private Variables
        private String _viewType;
        private ManagePersonalDocumentPresenter _presenter = new ManagePersonalDocumentPresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        public ManagePersonalDocumentPresenter Presenter
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

        public IManagePersonalDocumentView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<ApplicantDocument> ToSaveApplicantUploadedDocuments
        {
            get;
            set;
        }
        //Applicantid Set when screen opens from Admin(verification Details And Portfolio Search)
        Int32 IManagePersonalDocumentView.FromAdminApplicantID
        {
            get;
            set;
        }
        String IManagePersonalDocumentView.ErrorMessage
        {
            get;
            set;
        }

        Int32 IManagePersonalDocumentView.CurrentUserID
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        List<ApplicantDocumentDetails> IManagePersonalDocumentView.ApplicantUploadedDocuments
        {
            get;
            set;
        }

        Int32 IManagePersonalDocumentView.TenantID
        {
            get;
            set;
        }

        Int32 IManagePersonalDocumentView.ApplicantUploadedDocumentID
        {
            get;
            set;
        }

        ApplicantDocument toUpdateUploadedDocument = new ApplicantDocument();

        ApplicantDocument IManagePersonalDocumentView.ToUpdateUploadedDocument
        {
            get { return toUpdateUploadedDocument; }
            set { toUpdateUploadedDocument = value; }
        }

        Int32 IManagePersonalDocumentView.OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                }
                else
                {
                    return CurrentViewContext.CurrentUserID;
                }
            }
        }

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
                _viewType = Request.QueryString[AppConsts.UCID] == null ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Manage Professional Documents";
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
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

            }
            Presenter.OnViewLoaded();

            SetParameters();
        }

        public void SetParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                CurrentViewContext.TenantID = args.ContainsKey("TenantId") ? Convert.ToInt32(args["TenantId"]) : AppConsts.NONE;
                CurrentViewContext.FromAdminApplicantID = CurrentViewContext.CurrentUserID;
                ucUploadDocuments.TenantID = CurrentViewContext.TenantID;
                ucUploadDocuments.IsAdminScreen = false;
            }
            //base.SetPageTitle("Manage Personal Documents");
            ucUploadDocuments.OnCompletedUpload -= new UploadDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            ucUploadDocuments.OnCompletedUpload += new UploadDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            ucUploadDocuments.IsPersonalDocumentScreen = true;
        }

        public void ReloadGrid()
        {
            grdPersonalDocs.Rebind();
        }

        #endregion

        #region Grid Related Events
        void ucUploadDocuments_OnCompletedUpload()
        {
            grdPersonalDocs.Rebind();
        }

        /// <summary>
        /// Retrieves a list of all uploaded documents.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdPersonalDocs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetApplicantPersonalDocumentDetails();
            grdPersonalDocs.DataSource = CurrentViewContext.ApplicantUploadedDocuments;
        }

        /// <summary>
        /// Performs an delete operation for uploaded documents.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdPersonalDocs_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            CurrentViewContext.ApplicantUploadedDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ApplicantDocumentID"));
            if (Presenter.DeleteApplicantUploadedDocument())
            {
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    base.LogDebug(CurrentViewContext.ErrorMessage);
                }
                base.ShowSuccessMessage("Document deleted successfully.");
            }
            else
            {
                base.ShowInfoMessage("Some error occured. Please try again.");
            }
        }

        /// <summary>
        /// Performs an update operation for uploaded documents.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdPersonalDocs_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            CurrentViewContext.ApplicantUploadedDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ApplicantDocumentID"));

            String description = (e.Item.FindControl("txtDescription") as WclTextBox).Text;

            CurrentViewContext.ToUpdateUploadedDocument.ApplicantDocumentID = CurrentViewContext.ApplicantUploadedDocumentID;
            CurrentViewContext.ToUpdateUploadedDocument.Description = description;
            CurrentViewContext.ToUpdateUploadedDocument.ModifiedByID = CurrentViewContext.OrgUsrID;
            CurrentViewContext.ToUpdateUploadedDocument.ModifiedOn = DateTime.Now;

            if (Presenter.UpdateApplicantUploadedDocument())
            {
                base.ShowSuccessMessage("Document updated successfully.");
            }
            else
            {
                base.ShowErrorMessage("Document cannot be updated.");
            }

        }

        protected void grdPersonalDocs_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                ApplicantDocumentDetails docs = e.Item.DataItem as ApplicantDocumentDetails;
                if (!docs.DocumentPath.IsNullOrEmpty())
                {
                    String fileName = docs.DocumentPath.Substring(docs.DocumentPath.LastIndexOf("/") >= 0 ? docs.DocumentPath.LastIndexOf("/") : 0).Remove("/");
                    HtmlAnchor anchor = (HtmlAnchor)e.Item.FindControl("ancManageDocument");
                    anchor.HRef = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}&IsPDFFileDownload={2}", docs.ApplicantDocumentID, CurrentViewContext.TenantID, true);
                }
            }
        }


        protected void grdPersonalDocs_ItemCommand(object sender, GridCommandEventArgs e)
        {
            //Hide filter when exportig to pdf or word
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdPersonalDocs);

            }
        }

        #endregion

        #endregion
    }
}

