using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class SupplementAutomationConfigurationQueue : BaseUserControl, ISupplementAutomationConfigurationQueueView
    {
        #region Variables

        #region Private Variables

        private SupplementAutomationConfigurationQueuePresenter _presenter = new SupplementAutomationConfigurationQueuePresenter();
        private String _viewType;

        #endregion

        #endregion

        #region Properties

        public SupplementAutomationConfigurationQueuePresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
        }

        public ISupplementAutomationConfigurationQueueView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Tenant> lstTenant
        {
            get;
            set;
        }

        public String Description
        {
            get
            {
                return txtDescription.Text.Trim();
            }
            set
            {
                txtDescription.Text = value;
            }
        }

        public List<Int32> MappedTenantIds
        {
            get
            {
                if (ddlTenantName.CheckedItems.IsNullOrEmpty())
                {
                    return new List<int>();
                }
                return ddlTenantName.CheckedItems.Select(cond => Convert.ToInt32(cond.Value)).ToList();
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    foreach (Int32 item in value)
                    {
                        ddlTenantName.FindItemByValue(item.ToString()).Checked = true;
                    }
                }
            }
        }

        public Decimal Percentage
        {
            get
            {
                if (!String.IsNullOrEmpty(txtPercentage.Text))
                    return Convert.ToDecimal(txtPercentage.Text);
                return 0;
            }
            set
            {
                txtPercentage.Text = value.ToString();
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public Int32 SupplementAutomationConfigurationId
        {
            get
            {
                if (ViewState["SupplementAutomationConfigurationId"] != null)
                    return Convert.ToInt32(ViewState["SupplementAutomationConfigurationId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SupplementAutomationConfigurationId"] = value;
            }
        }


        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Supplement Automation Configuration";
                base.SetPageTitle("Supplement Automation Configuration");
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
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                if (!this.IsPostBack)
                {
                    this.Presenter.OnViewInitialized();
                    BindControls();
                }
                this._presenter.OnViewLoaded();
                if (this.IsPostBack)
                {
                    Boolean isQueueConfigInEditMode = Convert.ToBoolean(hdnIsQueueConfigInEditMode.Value);
                    SetFormMode(isQueueConfigInEditMode);
                    ResetButtons(isQueueConfigInEditMode);
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

        #region Button Events

        /// <summary>
        /// Button "Add Mapping" Click Event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fscuSupplementAutomation_SaveClick(object sender, EventArgs e)
        {
            try
            {
                String successMessage = String.Empty;
                if (CurrentViewContext.SupplementAutomationConfigurationId > AppConsts.NONE)
                {
                    successMessage = "Supplement Automation Configuration updated successfully.";
                }
                else
                {
                    successMessage = "Supplement Automation Configuration saved successfully.";
                }
                Presenter.SaveSupplementAutomationConfiguration();
                hdnIsQueueConfigInEditMode.Value = false.ToString();
                SetFormMode(false);
                ResetButtons(false);
  
                base.ShowSuccessMessage(successMessage);
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
        /// Button "Add Mapping" Click Event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fscuSupplementAutomation_CancelClick(object sender, EventArgs e)
        {
            try
            {
                hdnIsQueueConfigInEditMode.Value = false.ToString();
                if (CurrentViewContext.SupplementAutomationConfigurationId != AppConsts.NONE)
                {
                    SetFormMode(false);
                    ResetButtons(false);
                    Presenter.GetCurrentSupplementAutomationConfiguration();
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

        protected void fscuSupplementAutomation_SubmitClick(object sender, EventArgs e)
        {
            hdnIsQueueConfigInEditMode.Value = true.ToString();
            SetFormMode(true);
            ResetButtons(true);
        }

        #endregion

        #region Private Methods
        private void BindControls()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();
            Presenter.GetCurrentSupplementAutomationConfiguration();
            if (CurrentViewContext.SupplementAutomationConfigurationId != AppConsts.NONE)
            {

                hdnIsQueueConfigInEditMode.Value = false.ToString();
                SetFormMode(false);
                ResetButtons(false);
            }
            else
            {
                hdnIsQueueConfigInEditMode.Value = true.ToString();
                SetFormMode(true);
                ResetButtons(true);
            }

        }

        private void ResetButtons(Boolean isQueueConfigInEditMode)
        {
            fscuSupplementAutomation.SaveButton.Visible = isQueueConfigInEditMode;
            fscuSupplementAutomation.CancelButton.Visible = isQueueConfigInEditMode;
            fscuSupplementAutomation.SubmitButton.Visible = !isQueueConfigInEditMode;
            if (CurrentViewContext.SupplementAutomationConfigurationId > AppConsts.NONE)
            {
                //Changes Save Button Text from "Update Configuration" to save.
                fscuSupplementAutomation.SaveButtonText = "Save";
            }
            else
            {
                //Changes Save Button Text from "Save Configuration" to save.
                fscuSupplementAutomation.SaveButtonText = "Save";
            }
            fscuSupplementAutomation.SaveButton.ValidationGroup = "grpQueueConfiguration";
        }

        private void SetFormMode(Boolean isQueueConfigInEditMode)
        {
            txtDescription.Enabled = isQueueConfigInEditMode;
            txtPercentage.Enabled = isQueueConfigInEditMode;
            ddlTenantName.Items.ForEach(cond =>
            {
                cond.Enabled = isQueueConfigInEditMode;
            });
            ddlTenantName.EnableCheckAllItemsCheckBox = isQueueConfigInEditMode;
        }

        #endregion

        #endregion
    }
}