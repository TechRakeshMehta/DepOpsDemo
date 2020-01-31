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
namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageInstitutionNodeType : BaseUserControl, IManageInstitutionNodeTypeView
    {
        #region Variables

        #region Private
        private ManageInstitutionNodeTypePresenter _presenter = new ManageInstitutionNodeTypePresenter();
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
                    ddlTenant.DataSource = ListTenants;
                    ddlTenant.DataBind();
                    /*UAT-3032*/
                    GetPreferredSelectedTenant();
                    /*END UAT-3032*/
                }
            }
            if (Presenter.IsAdminLoggedIn())
            {
                if (ddlTenant.SelectedValue == String.Empty || Convert.ToInt32(ddlTenant.SelectedValue) == AppConsts.NONE)
                {
                    dvInstitutionNodeType.Visible = false;
                }
            }
            else
            {
                //SelectedTenantID = Presenter.GetTenantId();
                SelectedTenantID = TenantId;
                paneTenant.Visible = false;
            }
        }
        /// <summary>
        /// set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.Title = "Manage Institution Node Type";
            base.SetPageTitle("Manage Institution Node Type");
            base.OnInit(e);
        }

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public ManageInstitutionNodeTypePresenter Presenter
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
        public IQueryable<InstitutionNodeType> ListInstitutionNodeType
        {
            get;
            set;
        }
        public List<Tenant> ListTenants
        {
            set;
            get;
        }
        public String InstitutionNodeTypeName
        {
            get;
            set;
        }

        public String InstitutionNodeTypeDescription
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
        public Int32 InstitutionNodeTypeId
        {
            get;
            set;
        }
        public String LastCode
        {
            get;
            set;
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

        public IManageInstitutionNodeTypeView CurrentViewContext
        {
            get
            {
                return this;
            }

        }

        /*UAT - 3032*/

        Int32 IManageInstitutionNodeTypeView.PreferredSelectedTenantID
        {
            get
            {
                if (!ViewState["PreferredSelectedTenantID"].IsNull())
                {
                    return (Int32)ViewState["PreferredSelectedTenantID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PreferredSelectedTenantID"] = value;
            }
        }
        /* END UAT - 3032*/
        #endregion
        #endregion

        #region Events

        #region Drop Down Selection
        protected void ddlTenant_SelectedIndexChange(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                    dvInstitutionNodeType.Visible = true;
                    grdInstitutionNodeType.Rebind();
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

        #region Grid Related Events

        protected void grdInstitutionNodeType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetInstitutionNodeType();
                grdInstitutionNodeType.DataSource = CurrentViewContext.ListInstitutionNodeType;
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

        protected void grdInstitutionNodeType_ItemCommand(object sender, GridCommandEventArgs e)
        {
            dvInstitutionNodeType.Visible = true;
            // Hide filter when exportig to pdf or word
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdInstitutionNodeType);

            }
        }
        protected void grdInstitutionNodeType_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.InstitutionNodeTypeName = (e.Item.FindControl("txtInstitutionNodeTypeName") as WclTextBox).Text.Trim();
                CurrentViewContext.InstitutionNodeTypeDescription = (e.Item.FindControl("txtInstitutionNodeTypeDescription") as WclTextBox).Text.Trim();
                Presenter.SaveInsitutionNodeType();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    //base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
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
        protected void grdInstitutionNodeType_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.InstitutionNodeTypeName = (e.Item.FindControl("txtInstitutionNodeTypeName") as WclTextBox).Text.Trim();
                CurrentViewContext.InstitutionNodeTypeDescription = (e.Item.FindControl("txtInstitutionNodeTypeDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.InstitutionNodeTypeId = Convert.ToInt32((e.Item.FindControl("txtInstitutionNodeTypeId") as WclTextBox).Text.Trim());
                Presenter.UpdateInstitutionNodeType();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    //base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
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
        protected void grdInstitutionNodeType_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.InstitutionNodeTypeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("INT_ID"));
                Presenter.DeleteInstitutionNodeType();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    //base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    grdInstitutionNodeType.Rebind();
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                    //grdPriceAdjustment.Rebind();
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

        protected void grdInstitutionNodeType_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    InstitutionNodeType institutionNodeType = (InstitutionNodeType)e.Item.DataItem;

                    if (institutionNodeType.INT_IsSystem)
                    {
                        (e.Item as GridEditableItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
                        (e.Item as GridEditableItem)["DeleteColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                        (e.Item as GridEditableItem)["EditCommandColumn"].Controls[AppConsts.NONE].Visible = false;
                        (e.Item as GridEditableItem)["EditCommandColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                    }

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

        #region UAT-3032:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (CurrentViewContext.SelectedTenantID.IsNullOrEmpty() || CurrentViewContext.SelectedTenantID == AppConsts.NONE)
            {
               // Presenter.GetPreferredSelectedTenant();
                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                }
            }
        }
        #endregion
    }
}

