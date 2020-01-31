using System;
using INTSOF.Utils;
using Entity.ClientEntity;
using CoreWeb.Shell;

namespace CoreWeb.BkgSetup.Views
{
    public partial class EditServiceDetail : BaseUserControl, IEditServiceDetailView
    {
        #region Variables

        #region Private variables

        private EditServiceDetailPresenter _presenter = new EditServiceDetailPresenter();

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        private IEditServiceDetailView CurrentViewContext
        {
            get { return this; }
        }

        public EditServiceDetailPresenter Presenter
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

        String IEditServiceDetailView.ServiceName
        {
            get
            {
                return txtServiceName.Text;
            }
            set
            {
                txtServiceName.Text = value;
            }

        }

        String IEditServiceDetailView.DisplayName
        {
            get
            {
                return txtSvcDisplayName.Text;
            }
            set
            {
                txtSvcDisplayName.Text = value;
            }

        }

        String IEditServiceDetailView.Notes
        {
            get
            {
                return txtSvcNotes.Text;
            }
            set
            {
                txtSvcNotes.Text = value;
            }

        }


        Int32? IEditServiceDetailView.ResidenceDuration
        {
            get
            {
                return txtResidenceDuration.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtResidenceDuration.Text);
            }
            set
            {
                txtResidenceDuration.Text = value.HasValue ? value.ToString() : null;
            }
        }

        Boolean IEditServiceDetailView.SendDocsToStudent
        {
            get
            {
                return chkSendDocsToStudent.Checked;
            }
            set
            {
                chkSendDocsToStudent.Checked = value;
            }
        }

        Boolean IEditServiceDetailView.IsSupplemental
        {
            get
            {
                return ChkIsSupplemental.Checked;
            }
            set
            {
                ChkIsSupplemental.Checked = value;
            }
        }

        Boolean IEditServiceDetailView.IgnoreRHOnSupplement
        {
            get
            {
                return ChkIgnoreRHSuppl.Checked;
            }
            set
            {
                ChkIgnoreRHSuppl.Checked = value;
            }
        }

        public int TenantId
        {
            get
            {
                return (Int32)(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        public Int32 BkgPackageSrvcId
        {
            get
            {
                return (Int32)(ViewState["BkgPackageSrvcId"]);
            }
            set
            {
                ViewState["BkgPackageSrvcId"] = value;
            }
        }

        Int32 IEditServiceDetailView.BackgroundServiceId
        {
            get
            {
                return (Int32)(ViewState["BackgroundServiceId"]);
            }
            set
            {
                ViewState["BackgroundServiceId"] = value;
            }
        }

        BkgPackageSvc IEditServiceDetailView.CurrentBkgPackageSvc
        {
            get;
            set;
        }

        Int32 IEditServiceDetailView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        String IEditServiceDetailView.ErrorMessage { get; set; }

        String IEditServiceDetailView.SuccessMessage { get; set; }

        String IEditServiceDetailView.InfoMessage { get; set; }

        //UAT-3109
        String IEditServiceDetailView.AMERNumber
        {
            get
            {
                return txtSvcAMERNumber.Text;
            }
            set
            {
                txtSvcAMERNumber.Text = value;
            }

        }


        #endregion

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetServiceDetails();
                    ResetButtons(true);
                    ResetControls(true);
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.UpdateBkgPackageSvc();
                ResetButtons(true);
                ResetControls(true);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                (this.Page as BaseWebPage).ShowSuccessMessage("Service Updated successfully.");
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SetServiceDetails();
                ResetButtons(true);
                ResetControls(true);
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ResetButtons(false);
                ResetControls(false);
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

        #region Private Methods
        private void SetServiceDetails()
        {
            Boolean? showdivPkgCount, showdivYears, showdivMinOcc, showdivMaxOcc, showdivSendDocToStud, showdivIsSupplemental, showdivIgnoreRHSuppl;
            showdivPkgCount = showdivYears = showdivMinOcc = showdivMaxOcc = showdivSendDocToStud = showdivIsSupplemental = showdivIgnoreRHSuppl = false;
            Presenter.GetCurrentBkgPackageSvc();
            Presenter.GetExternalCode();
            BkgPackageSvc currentBkgPackageSvc = CurrentViewContext.CurrentBkgPackageSvc;
            if (currentBkgPackageSvc.IsNotNull())
            {
                txtServiceName.Text = currentBkgPackageSvc.BackgroundService.BSE_Name;
                txtSvcDisplayName.Text = currentBkgPackageSvc.BPS_DisplayName;
                txtSvcNotes.Text = currentBkgPackageSvc.BPS_Notes;
                txtResidenceDuration.Text = currentBkgPackageSvc.BPS_NumberOfYearsOfResidence.ToString();
                chkSendDocsToStudent.Checked = currentBkgPackageSvc.BPS_SendDocumentsToStudent.Value;
                ChkIsSupplemental.Checked = currentBkgPackageSvc.BPS_IsSupplemental.Value;
                ChkIgnoreRHSuppl.Checked = currentBkgPackageSvc.BPS_IgnoreResidentialHistoryOnSupplement.Value;
                CurrentViewContext.BackgroundServiceId = currentBkgPackageSvc.BackgroundService.BSE_ID;
                Entity.ApplicableServiceSetting serviceSettings = Presenter.GetServiceSettings();
                if (serviceSettings.IsNotNull())
                {
                    showdivYears = serviceSettings.ASSE_ShowResidenceYears;
                    showdivSendDocToStud = serviceSettings.ASSE_ShowSendDocument;
                    showdivIsSupplemental = serviceSettings.ASSE_ShowIsSupplemental;
                    showdivIgnoreRHSuppl = serviceSettings.ASSE_ShowIgnoreResidentialHistory;
                }
            }

            if ((showdivYears.HasValue && !showdivYears.Value) && (showdivSendDocToStud.HasValue && !showdivSendDocToStud.Value))
            {
                divSettings.Style.Add("display", "none");
            }
            else
                divSettings.Style.Add("display", "block");

            if ((showdivIsSupplemental.HasValue && !showdivIsSupplemental.Value) && (showdivIgnoreRHSuppl.HasValue && !showdivIgnoreRHSuppl.Value))
            {
                divSettings3.Style.Add("display", "none");
            }
            else
                divSettings3.Style.Add("display", "block");

            divYears.Visible = (showdivYears.HasValue) ? showdivYears.Value : false;
            divDocToStud.Visible = (showdivSendDocToStud.HasValue) ? showdivSendDocToStud.Value : false;
            divIsSupplemental.Visible = (showdivIsSupplemental.HasValue) ? showdivIsSupplemental.Value : false;
            divIgnoreRHSuppl.Visible = (showdivIgnoreRHSuppl.HasValue) ? showdivIgnoreRHSuppl.Value : false;
        }

        private void ResetButtons(Boolean isReadOnly)
        {
            btnEdit.Visible = isReadOnly;
            btnSave.Visible = !isReadOnly;
            btnCancel.Visible = !isReadOnly;
        }

        private void ResetControls(Boolean isReadOnly)
        {
            txtServiceName.Enabled = false;
            txtSvcDisplayName.Enabled = !isReadOnly;
            txtSvcNotes.Enabled = !isReadOnly;
            txtResidenceDuration.Enabled = !isReadOnly;
            chkSendDocsToStudent.Enabled = !isReadOnly;
            ChkIsSupplemental.Enabled = !isReadOnly;
            ChkIgnoreRHSuppl.Enabled = !isReadOnly;
        }
        #endregion
    }
}