using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Entity;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageRejectionReason : BaseUserControl, IManageRejectionReasonView
    {
        #region Variables

        #region Private
        private ManageRejectionReasonPresenter _presenter = new ManageRejectionReasonPresenter();
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
            }
        }
        /// <summary>
        /// set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.Title = "Manage Rejection Reason";
            base.SetPageTitle("Manage Rejection Reason");
            base.OnInit(e);
        }

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public ManageRejectionReasonPresenter Presenter
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
        IQueryable<RejectionReason> IManageRejectionReasonView.ListRejectionReason
        {
            get;
            set;
        }
        String IManageRejectionReasonView.ReasonText
        {
            get;
            set;
        }

        String IManageRejectionReasonView.ReasonName
        {
            get;
            set;
        }

        String IManageRejectionReasonView.ErrorMessage
        {
            get;
            set;
        }

        String IManageRejectionReasonView.SuccessMessage
        {
            get;
            set;
        }
        Int32 IManageRejectionReasonView.RejectionReasonCategoryId
        {
            get;
            set;
        }
        Int32 IManageRejectionReasonView.RejectionReasonID
        {
            get;
            set;
        }
        Int32 IManageRejectionReasonView.TenantId
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

        Int32 IManageRejectionReasonView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        List<lkpRejectionReasonCategory> IManageRejectionReasonView.ListRejectionCategory
        {
            get;
            set;
        }

        public IManageRejectionReasonView CurrentViewContext
        {
            get
            {
                return this;
            }

        }
        #endregion
        #endregion

        #region Events

        #region Grid Related Events

        protected void grdRejectionReason_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetRejectionReasons();
                grdRejectionReason.DataSource = CurrentViewContext.ListRejectionReason;
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

        protected void grdRejectionReason_ItemCommand(object sender, GridCommandEventArgs e)
        {
            grdRejectionReason.Visible = true;
            // Hide filter when exportig to pdf or word
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdRejectionReason);

            }
        }

        protected void grdRejectionReason_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    WclComboBox cmbReasonCategory = (WclComboBox)editform.FindControl("cmbReasonCategory");
                    Presenter.GetRejectionCategoryList();

                    if (!CurrentViewContext.ListRejectionCategory.IsNull())
                    {
                        cmbReasonCategory.DataSource = CurrentViewContext.ListRejectionCategory;
                        cmbReasonCategory.DataBind();

                        if (!(e.Item is GridEditFormInsertItem))
                        {
                            RejectionReason rejectionReason = (RejectionReason)e.Item.DataItem;
                            if (!rejectionReason.IsNull())
                            {
                                cmbReasonCategory.SelectedValue = Convert.ToString(rejectionReason.RR_RejectionReasonCategoryID);
                            }
                        }
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

        protected void grdRejectionReason_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ReasonText = (e.Item.FindControl("txtReasonText") as WclTextBox).Text.Trim();
                CurrentViewContext.ReasonName = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.RejectionReasonCategoryId = Convert.ToInt32((e.Item.FindControl("cmbReasonCategory") as WclComboBox).SelectedValue);
                
                if (!Presenter.SaveUpdateRejectionReason())
                {
                    e.Canceled = true;
                    base.ShowInfoMessage("Some Error has occurred.Please try again.");
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Rejection reason saved successfully.");
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
        protected void grdRejectionReason_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ReasonText = (e.Item.FindControl("txtReasonText") as WclTextBox).Text.Trim();
                CurrentViewContext.ReasonName = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.RejectionReasonCategoryId = Convert.ToInt32((e.Item.FindControl("cmbReasonCategory") as WclComboBox).SelectedValue);

                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.RejectionReasonID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("RR_ID"));

                if (!Presenter.SaveUpdateRejectionReason())
                {
                    e.Canceled = true;
                    base.ShowInfoMessage("Some Error has occurred.Please try again.");
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Rejection reason updated successfully.");
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
        protected void grdRejectionReason_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.RejectionReasonID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("RR_ID"));
               
                if (!Presenter.DeleteRejectionReason())
                {
                    e.Canceled = true;
                    base.ShowInfoMessage("Some Error has occurred.Please try again.");
                    grdRejectionReason.Rebind();
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Rejection reason deleted successfully.");
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