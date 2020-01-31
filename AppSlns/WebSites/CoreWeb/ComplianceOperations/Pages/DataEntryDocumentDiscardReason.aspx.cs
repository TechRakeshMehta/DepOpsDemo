using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class DataEntryDocumentDiscardReason : BaseWebPage, IDataEntryDocumentDiscardReasonView
    {
        #region Private Variables

        private DataEntryDocumentDiscardReasonPresenter _presenter = new DataEntryDocumentDiscardReasonPresenter();

        #endregion

        #region Properties
        public DataEntryDocumentDiscardReasonPresenter Presenter
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

        public IDataEntryDocumentDiscardReasonView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IDataEntryDocumentDiscardReasonView.CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IDataEntryDocumentDiscardReasonView.TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IDataEntryDocumentDiscardReasonView.FDEQ_ID
        {
            get
            {
                if (!ViewState["FDEQ_ID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["FDEQ_ID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["FDEQ_ID"] = value;
            }
        }

        Int32 IDataEntryDocumentDiscardReasonView.DocumentId
        {
            get
            {
                if (!ViewState["DocumentId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["DocumentId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["DocumentId"] = value;
            }
        }

        Int32 IDataEntryDocumentDiscardReasonView.SelectedDiscardReasonId
        {
            get
            {
                if (!ddlDiscardReason.SelectedValue.IsNullOrEmpty())
                {
                    hdnDocumentDiscardReasonId.Value = ddlDiscardReason.SelectedValue;
                    return Convert.ToInt32(ddlDiscardReason.SelectedValue);
                }
                return AppConsts.NONE;
            }

        }

        String IDataEntryDocumentDiscardReasonView.DiscardReasonText
        {
            get
            {
                if (!ddlDiscardReason.SelectedValue.IsNullOrEmpty())
                {
                    return ddlDiscardReason.SelectedItem.Text;
                }
                return String.Empty;
            }

        }

        List<lkpDocumentDiscardReason> IDataEntryDocumentDiscardReasonView.LstDocumentDiscradReason
        {
            set
            {
                ddlDiscardReason.DataSource = value;
                ddlDiscardReason.DataBind();
            }
        }

        String IDataEntryDocumentDiscardReasonView.AdditionalNotes
        {
            get { return txtAdditionalNotes.Text; }
        }

        public Int32 DiscardDocumentCount
        {
            get
            {
                if (!Request["DiscardDocumentCount"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(Request["DiscardDocumentCount"]);
                }
                return AppConsts.NONE;
            }
        }
        #endregion

        #region Events

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CaptureQueryStringData();
                    Presenter.GetDocumentDiscardReasonList();
                }

                cmdBarDiscardReason.SubmitButton.ValidationGroup = "vgDiscardReason";
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

        #region Button Events
        protected void Continue_Click(Object sender, EventArgs e)
        {
            try
            {
                String documentStatus = DataEntryDocumentStatus.DOCUMENT_REJECTED.GetStringValue();

                DataEntryTrackingContract dataEntryTimeTracking = null;
                dataEntryTimeTracking = (DataEntryTrackingContract)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.DATA_ENTRY_TRACKING);
                if (dataEntryTimeTracking.IsNotNull())
                {
                    dataEntryTimeTracking.ItemImpacted = AppConsts.NONE;
                    dataEntryTimeTracking.StatusId = Presenter.GetDocumentStatusIdByCode(documentStatus);
                    dataEntryTimeTracking.AffectedItemIds = new List<Int32>();
                    if (CurrentViewContext.SelectedDiscardReasonId > AppConsts.NONE)
                    {
                        dataEntryTimeTracking.DiscardReasonId = CurrentViewContext.SelectedDiscardReasonId;
                    }
                    dataEntryTimeTracking.StatusNotes = CurrentViewContext.AdditionalNotes;
                    dataEntryTimeTracking.DiscardReason = CurrentViewContext.DiscardReasonText;
                }

                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
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
        #endregion

        #region Methods

        #region Private Methods
        private void CaptureQueryStringData()
        {
            if (!Request["TenantID"].IsNullOrEmpty())
            {
                CurrentViewContext.TenantId = Convert.ToInt32(Request["TenantID"]);
            }

            if (!Request["DocId"].IsNullOrEmpty())
            {
                CurrentViewContext.DocumentId = Convert.ToInt32(Request["DocId"]);
            }

            if (!Request["FdeqId"].IsNullOrEmpty())
            {
                CurrentViewContext.FDEQ_ID = Convert.ToInt32(Request["FdeqId"]);
            }

        }
        #endregion
        #endregion
    }
}
