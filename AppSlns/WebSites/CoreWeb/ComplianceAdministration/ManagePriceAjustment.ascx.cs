#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Web.Services;
using System.Linq;
using System.Web.UI.WebControls;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;


#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManagePriceAjustment : BaseUserControl, IManagePriceAjustmentView
    {
        #region Private Variables
        private ManagePriceAjustmentPresenter _presenter=new ManagePriceAjustmentPresenter();
        #endregion

        #region Properties

        #region Presenter

        
        public ManagePriceAjustmentPresenter Presenter
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

        #region public Properties
        public List<Tenant> ListTenants
        {
            set;
            get;
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if ( ViewState["ClientTenantID"].IsNotNull())
                {
                    return  Convert.ToInt32(ViewState["ClientTenantID"]);
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

        //public Int32 CurrentUserTenantId
        //{
        //    get;
        //    set;
        //}

        public IQueryable<PriceAdjustment> ListPriceAdjustment
        {
            get;
            set;
        }

        public Int32 PriceAdjustmentId
        {
            get;
            set;
        }

       public String Label
        { 
           get;
           set;
       }

        public String Description
        { 
            get; 
            set;
        }

        public String ErrorMessage { get; set; }

        public String SuccessMessage { get; set; }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public  IManagePriceAjustmentView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion
        #endregion

        #region Events

        #region Grid Events

        protected void grdPriceAdjustment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetPriceAdjustmentList();
                grdPriceAdjustment.DataSource = CurrentViewContext.ListPriceAdjustment;
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
        /// Called when data is bound in grid.
        /// </summary>
        /// <param name="sender">Current control i.e. Grid</param>
        /// <param name="e">Contains related data</param>
        protected void grdPriceAdjustment_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
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

        protected void grdPriceAdjustment_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvPriceAdjustment.Visible = true;
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdPriceAdjustment);

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

        protected void grdPriceAdjustment_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.Label = (e.Item.FindControl("txtLabel") as WclTextBox).Text.Trim();
                Presenter.SavePriceAdjustment();
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

        protected void grdPriceAdjustment_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.Label = (e.Item.FindControl("txtLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.PriceAdjustmentId = Convert.ToInt32((e.Item.FindControl("txtPriceAdjustmentId") as WclTextBox).Text);
                Presenter.UpdatePriceAdjustment();
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

        protected void grdPriceAdjustment_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.PriceAdjustmentId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("PriceAdjustmentID"));
                Presenter.DeletePriceAdjustment();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    //base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    grdPriceAdjustment.Rebind();
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

        #endregion

        #region DropDown Events

        /// <summary>
        /// Binds the subscription options as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                    dvPriceAdjustment.Visible = true;
                    grdPriceAdjustment.Rebind();
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

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Price Adjustment";
                base.SetPageTitle("Price Adjustment");
                base.OnInit(e);
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
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    if (Presenter.IsAdminLoggedIn())
                    {
                        Presenter.OnViewLoaded();
                        ddlTenant.DataSource = ListTenants;
                        ddlTenant.DataBind();
                    }
                }
                if (Presenter.IsAdminLoggedIn())
                {                      
                    if (ddlTenant.SelectedValue == String.Empty || Convert.ToInt32(ddlTenant.SelectedValue) == AppConsts.NONE)
                    {
                        dvPriceAdjustment.Visible = false;
                    }
                }
                else
                {
                    pnlTenant.Visible = false;
                    //SelectedTenantID = Presenter.GetTenantId();
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

