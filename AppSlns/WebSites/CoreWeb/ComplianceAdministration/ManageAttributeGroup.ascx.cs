#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Web.Services;
using System.Linq;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using CoreWeb.IntsofSecurityModel;


#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageAttributeGroup : BaseUserControl, IManageAttributeGroupView
    {

        #region Variables

        #region Private
        private ManageAttributeGroupPresenter _presenter=new ManageAttributeGroupPresenter();
        private Int32 _tenantId;
        #endregion

        #region public

        #endregion
        #endregion

        #region Page Events
        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                if (Presenter.IsAdminLoggedIn())
                {
                    Presenter.OnViewLoaded();
                    dvComplianceAttributeGroup.Visible = false;
                    ddlTenant.DataSource = ListTenants;
                    ddlTenant.DataBind();
                }
                else
                {
                    SelectedTenantID = TenantId;
                    paneTenant.Visible = false;
                }
            }
            //this._presenter.OnViewLoaded();
        }
        /// <summary>
        /// set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.Title = "Manage Attribute Group";
            base.SetPageTitle("Manage Attribute Group");
            base.OnInit(e);
        }

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties
        
        public ManageAttributeGroupPresenter Presenter
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

        public IQueryable<ComplianceAttributeGroup> ListComplianceAttributeGroup
        {
            get;
            set;
        }
        public List<Tenant> ListTenants
        {
            set;
            get;
        }
        public String Name
        {
            get;
            set;
        }

        public String Label
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public String SuccessMessage
        {
            get;
            set;
        }
        public Int32 ComplianceAttributeGroupId
        {
            get;
            set;
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (ViewState["ClientTenantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ClientTenantID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ClientTenantID"] = value;
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IManageAttributeGroupView CurrentViewContext
        {
            get
            {
                return this;
            }

        }
        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        public Int32 DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
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
        #endregion
        #endregion

        #region Events

        #region Drop Down Selection
        protected void ddlTenant_SelectedIndexChange(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                dvComplianceAttributeGroup.Visible = true;
                grdComplianceAttributeGroup.Rebind();

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

        #region
        protected void grdComplianceAttributeGroup_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetAllComplianceAttributeGroup();
                grdComplianceAttributeGroup.DataSource = CurrentViewContext.ListComplianceAttributeGroup;
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

        protected void grdComplianceAttributeGroup_ItemCommand(object sender, GridCommandEventArgs e)
        {
            dvComplianceAttributeGroup.Visible = true;
            // Hide filter when exportig to pdf or word
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdComplianceAttributeGroup);

            }
        }
        protected void grdComplianceAttributeGroup_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.Name = (e.Item.FindControl("txtComplianceAttributeGroupName") as WclTextBox).Text.Trim();
                CurrentViewContext.Label = (e.Item.FindControl("txtComplianceAttributeGroupLabel") as WclTextBox).Text.Trim();
                Presenter.SaveAttributeGroup();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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
        protected void grdComplianceAttributeGroup_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.Name = (e.Item.FindControl("txtComplianceAttributeGroupName") as WclTextBox).Text.Trim();
                CurrentViewContext.Label = (e.Item.FindControl("txtComplianceAttributeGroupLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.ComplianceAttributeGroupId = Convert.ToInt32((e.Item.FindControl("txtComplianceAttributeGroupId") as WclTextBox).Text.Trim());
                Presenter.UpdateAttributeGroup();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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
        protected void grdComplianceAttributeGroup_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.ComplianceAttributeGroupId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("CAG_ID"));
                Presenter.DeleteAttributeGroup();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;

                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    grdComplianceAttributeGroup.Rebind();
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

