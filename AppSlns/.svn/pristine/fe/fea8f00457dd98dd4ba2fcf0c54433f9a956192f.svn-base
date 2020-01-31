using CoreWeb.AgencyHierarchy.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;

namespace CoreWeb.AgencyHierarchy.UserControls
{
    public partial class AgencyHierarchyProfileSharePermission : BaseUserControl, IAgencyHierarchyProfileSharePermissionView
    {
        #region Handlers

        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;

        #endregion

        #region [Private Variables]

        private AgencyHierarchyProfileSharePermissionPresenter _presenter = new AgencyHierarchyProfileSharePermissionPresenter();
        private Int32 _tenantId;

        #endregion

        #region [Public Variables]

        public IAgencyHierarchyProfileSharePermissionView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public AgencyHierarchyProfileSharePermissionPresenter Presenter
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


        Int32 IAgencyHierarchyProfileSharePermissionView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        List<AgencyHierarchyProfileSharePermissionDataContract> IAgencyHierarchyProfileSharePermissionView.lstAgencyHierarchyProfileSharePermission
        {
            get;
            set;
        }

        public Int32 AgencyHierarchyID
        {
            get
            {
                return Convert.ToInt32(ViewState["AgencyHierarchyId"]);
            }
            set
            {
                ViewState["AgencyHierarchyId"] = value;
            }
        }

        Int32 IAgencyHierarchyProfileSharePermissionView.SelectedTenantID
        {
            get;
            set;
        }

        List<TenantDetailContract> IAgencyHierarchyProfileSharePermissionView.lstTenant
        {
            get
            {
                if (ViewState["lstTenant"].IsNullOrEmpty())
                {
                    Presenter.GetTenants();
                }
                return (List<TenantDetailContract>)ViewState["lstTenant"];
            }
            set
            {
                ViewState["lstTenant"] = value;
            }
        }

        AgencyHierarchyProfileSharePermissionDataContract IAgencyHierarchyProfileSharePermissionView.AgencyHierarchyProfileSharePermissionDataContract
        {
            get;
            set;
        }
        #endregion

        #region PAGE EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.AgencyHierarchyID == AppConsts.NONE)
                {
                    grdAgencyHierarProfileSharePerm.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }
        #endregion

        #region GRID EVENTS

        protected void grdAgencyHierarProfileSharePerm_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.AgencyHierarchyProfileSharePermissionDataContract = new AgencyHierarchyProfileSharePermissionDataContract();
                CurrentViewContext.AgencyHierarchyProfileSharePermissionDataContract.AgencyHierarchyAgencyProfileSharePermissionsID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyHierarchyAgencyProfileSharePermissionsID"]);
                CurrentViewContext.AgencyHierarchyProfileSharePermissionDataContract.CurrentLoggedInUserID = CurrentViewContext.CurrentUserId;
                CurrentViewContext.AgencyHierarchyProfileSharePermissionDataContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                Presenter.RemoveProfileSharePermission();
                eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency hierarchy profile share permission deleted sucessfully!");
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }

        }

        protected void grdAgencyHierarProfileSharePerm_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetProfileSharePermissionByAgencyHierarchyID();
                grdAgencyHierarProfileSharePerm.DataSource = CurrentViewContext.lstAgencyHierarchyProfileSharePermission;

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }

        }

        protected void grdAgencyHierarProfileSharePerm_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    CurrentViewContext.AgencyHierarchyProfileSharePermissionDataContract = new AgencyHierarchyProfileSharePermissionDataContract();
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        CurrentViewContext.AgencyHierarchyProfileSharePermissionDataContract.AgencyHierarchyAgencyProfileSharePermissionsID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyHierarchyAgencyProfileSharePermissionsID"]);
                    }
                    CurrentViewContext.AgencyHierarchyProfileSharePermissionDataContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                    WclComboBox ddlTenant = e.Item.FindControl("ddlTenant") as WclComboBox;
                    CurrentViewContext.AgencyHierarchyProfileSharePermissionDataContract.TenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                    CurrentViewContext.AgencyHierarchyProfileSharePermissionDataContract.CurrentLoggedInUserID = CurrentViewContext.CurrentUserId;
                    CheckBox chkStudentProfileSharingPermission = e.Item.FindControl("chkStudentProfileSharingPermission") as CheckBox;
                    CurrentViewContext.AgencyHierarchyProfileSharePermissionDataContract.IsStudentShare = chkStudentProfileSharingPermission.Checked;
                    CheckBox chkAdminProfileSharingPermission = e.Item.FindControl("chkAdminProfileSharingPermission") as CheckBox;
                    CurrentViewContext.AgencyHierarchyProfileSharePermissionDataContract.IsAdminShare = chkAdminProfileSharingPermission.Checked;

                    Presenter.SaveUpdateProfileSharePermission();

                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency hierarchy profile share permission updated sucessfully!");
                        lblFocus.Focus();
                    }
                    else
                    {
                        eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency hierarchy profile share permission added sucessfully!");
                        lblFocus.Focus();
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdAgencyHierarProfileSharePerm_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlTenant = editform.FindControl("ddlTenant") as WclComboBox;

                    List<Int32> AlreadyMappedTenants = CurrentViewContext.lstAgencyHierarchyProfileSharePermission.Where(cond => cond.AgencyHierarchyID == CurrentViewContext.AgencyHierarchyID).Select(sel => sel.TenantID).ToList();

                    ddlTenant.DataSource = CurrentViewContext.lstTenant.Where(cmd => !AlreadyMappedTenants.Contains(cmd.TenantID));
                    ddlTenant.DataBind();
                    ddlTenant.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));

                    ddlTenant.Focus();
                    if (e.Item is GridEditFormItem && !(e.Item is GridEditFormInsertItem))
                    {
                        Int32 tenantID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"]);


                        ddlTenant.SelectedValue = tenantID.ToString();
                        ddlTenant.Enabled = false;
                    }
                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }
        #endregion
    }
}