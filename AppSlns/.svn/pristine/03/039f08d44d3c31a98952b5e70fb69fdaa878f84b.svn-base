using CoreWeb.Security.Views;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class ManageRoleConfiguration : BaseUserControl, IManageRoleConfigurationView
    {
        #region Variables

        private ManageRoleConfigurationPresenter _presenter = new ManageRoleConfigurationPresenter();

        #endregion

        #region Properties


        private ManageRoleConfigurationPresenter Presenter
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

        private IManageRoleConfigurationView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IManageRoleConfigurationView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        List<Entity.RoleDetail> IManageRoleConfigurationView.lstRoles
        {
            get;
            set;
        }

        List<Entity.Tenant> IManageRoleConfigurationView.lstTenant
        {
            get;
            set;
        }

        List<RoleConfigurationContract> IManageRoleConfigurationView.lstRoleConfig
        {
            get;
            set;
        }

        RoleConfigurationContract IManageRoleConfigurationView.RolePreferredTenantSetting
        {
            get;
            set;
        }

        #endregion

        #region EVENTS

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Role Configuration";
                base.SetPageTitle("Manage Role Configuration");
                lblRoleConfig.Text = base.Title;
                grdRoleConfiguration.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
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

            if (!this.IsPostBack)
            {

            }
        }

        #endregion

        #region Grid Events

        protected void grdRoleConfiguration_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetRolesSetting();
                grdRoleConfiguration.DataSource = CurrentViewContext.lstRoleConfig;
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

        protected void grdRoleConfiguration_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                // RadComboBox cmbRoles = e.Item.FindControl("cmbRoles") as RadComboBox;
                if (!e.Item.IsNullOrEmpty())
                {
                    if (e.CommandName == "CheckChange" || e.CommandName == "DataEntryCheck" || e.CommandName == "ComplianceVerficationCheck" 
                        || e.CommandName == "RotationVerficationCheck" || e.CommandName == "LocationEnrollerCheck")
                    {
                        RadButton chkIsAllowPreferredTenant = e.Item.FindControl("chkIsAllowPreferredTenant") as RadButton;
                        RadButton chkIsAllowDataEntry = e.Item.FindControl("chkIsAllowDataEntry") as RadButton;
                        RadButton chkIsAllowComplianceVerfication = e.Item.FindControl("chkIsAllowComplianceVerfication") as RadButton;
                        RadButton chkIsAllowRotationVerfication = e.Item.FindControl("chkIsAllowRotationVerfication") as RadButton;
                        RadButton chkIsAllowLocationEnroller = e.Item.FindControl("chkIsAllowLocationEnroller") as RadButton;

                        CurrentViewContext.RolePreferredTenantSetting = new RoleConfigurationContract();
                        CurrentViewContext.RolePreferredTenantSetting.RPTS_IsAllowPreferredTenant = chkIsAllowPreferredTenant.Checked;
                        CurrentViewContext.RolePreferredTenantSetting.RPTS_IsAllowDataEntry = chkIsAllowDataEntry.Checked;
                        CurrentViewContext.RolePreferredTenantSetting.RPTS_IsAllowComplianceVerfication = chkIsAllowComplianceVerfication.Checked;
                        CurrentViewContext.RolePreferredTenantSetting.RPTS_IsAllowRotationVerfication = chkIsAllowRotationVerfication.Checked;
                        CurrentViewContext.RolePreferredTenantSetting.RPTS_IsAllowLocationEnroller = chkIsAllowLocationEnroller.Checked;

                        String roleId = Convert.ToString(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RPTS_RoleID"]);
                        CurrentViewContext.RolePreferredTenantSetting.RPTS_RoleID = roleId;

                        Int32 RPTS_ID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RPTS_ID"]);
                        if (RPTS_ID > AppConsts.NONE && !RPTS_ID.IsNullOrEmpty())
                        {
                            CurrentViewContext.RolePreferredTenantSetting.RPTS_ID = RPTS_ID;
                        }
                        if (Presenter.SaveRolePreferredTenantSetting())
                        {
                            e.Canceled = false;
                            base.ShowSuccessMessage("Setting saved successfully.");
                            grdRoleConfiguration.Rebind();
                        }
                        else
                        {
                            e.Canceled = true;
                            base.ShowInfoMessage("Some error has occurred. Please try again.");
                        }
                    }
                }

                //Export Functionality
                if (e.CommandName.IsNullOrEmpty())
                {
                    if (e.Item is GridCommandItem)
                    {
                        WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                        if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                        {
                            grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowPreferredTenantExp").Display = true;
                            grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowDataEntryExp").Display = true;
                            grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowComplianceVerficationExp").Display = true;
                            grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowRotationVerficationExp").Display = true;
                            grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowLocationEnrollerExp").Display = true;
                        }
                        else
                        {
                            grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowPreferredTenantExp").Display = false;
                            grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowDataEntryExp").Display = false;
                            grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowComplianceVerficationExp").Display = false;
                            grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowRotationVerficationExp").Display = false;
                            grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowLocationEnrollerExp").Display = false;
                        }
                    }
                }
                if (e.CommandName == "Cancel")
                {
                    grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowPreferredTenantExp").Display = false;
                    grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowDataEntryExp").Display = false;
                    grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowComplianceVerficationExp").Display = false;
                    grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowRotationVerficationExp").Display = false;
                    grdRoleConfiguration.MasterTableView.GetColumn("_IsAllowLocationEnrollerExp").Display = false;
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

        protected void grdRoleConfiguration_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                RadButton chkIsAllowPreferredTenant = e.Item.FindControl("chkIsAllowPreferredTenant") as RadButton;
                RadButton chkIsAllowDataEntry = e.Item.FindControl("chkIsAllowDataEntry") as RadButton;
                RadButton chkIsAllowComplianceVerfication = e.Item.FindControl("chkIsAllowComplianceVerfication") as RadButton;
                RadButton chkIsAllowRotationVerfication = e.Item.FindControl("chkIsAllowRotationVerfication") as RadButton;
                RadButton chkIsAllowLocationEnroller = e.Item.FindControl("chkIsAllowLocationEnroller") as RadButton;

                if (!chkIsAllowPreferredTenant.IsNullOrEmpty() || !chkIsAllowDataEntry.IsNullOrEmpty() || !chkIsAllowComplianceVerfication.IsNullOrEmpty()
                    || !chkIsAllowRotationVerfication.IsNullOrEmpty() || !chkIsAllowLocationEnroller.IsNullOrEmpty())
                {
                    String roleId = Convert.ToString(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RPTS_RoleID"]);
                    chkIsAllowPreferredTenant.Checked = CurrentViewContext.lstRoleConfig.Where(cond => cond.RPTS_RoleID == roleId).FirstOrDefault().RPTS_IsAllowPreferredTenant;
                    chkIsAllowDataEntry.Checked = CurrentViewContext.lstRoleConfig.Where(cond => cond.RPTS_RoleID == roleId).FirstOrDefault().RPTS_IsAllowDataEntry;
                    chkIsAllowComplianceVerfication.Checked = CurrentViewContext.lstRoleConfig.Where(cond => cond.RPTS_RoleID == roleId).FirstOrDefault().RPTS_IsAllowComplianceVerfication;
                    chkIsAllowRotationVerfication.Checked = CurrentViewContext.lstRoleConfig.Where(cond => cond.RPTS_RoleID == roleId).FirstOrDefault().RPTS_IsAllowRotationVerfication;
                    chkIsAllowLocationEnroller.Checked = CurrentViewContext.lstRoleConfig.Where(cond => cond.RPTS_RoleID == roleId).FirstOrDefault().RPTS_IsAllowLocationEnroller;
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

        #endregion
    }
}