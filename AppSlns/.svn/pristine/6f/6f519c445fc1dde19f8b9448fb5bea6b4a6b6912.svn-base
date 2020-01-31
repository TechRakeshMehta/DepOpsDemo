using CoreWeb.ComplianceOperations.Views;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageCompliancePriorityObject : BaseUserControl, IManageCompliancePriorityObjectView
    {

        #region Variables
        private ManageCompliancePriorityObjectPresenter _presenter = new ManageCompliancePriorityObjectPresenter();
        #endregion

        #region Properties

        public ManageCompliancePriorityObjectPresenter Presenter
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

        public IManageCompliancePriorityObjectView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<CompliancePriorityObjectContract> lstCompPriorityObject
        {
            get
            {
                if (!ViewState["lstCompPriorityObject"].IsNullOrEmpty())
                {
                    return ViewState["lstCompPriorityObject"] as List<CompliancePriorityObjectContract>;
                }
                return new List<CompliancePriorityObjectContract>();
            }
            set
            {
                ViewState["lstCompPriorityObject"] = value;
            }
        }

        Int32 IManageCompliancePriorityObjectView.CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
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
                base.Title = "Manage Compliance Priority Object";
                base.SetPageTitle("Manage Compliance Priority Object");
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

                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region Grid Events

        protected void grdCompPriorityObject_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetCompliancePriorityObjects();
                grdCompPriorityObject.DataSource = CurrentViewContext.lstCompPriorityObject;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdCompPriorityObject_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {

                    WclTextBox txtName = e.Item.FindControl("txtName") as WclTextBox;
                    WclTextBox txtDescription = e.Item.FindControl("txtDescription") as WclTextBox;

                    CompliancePriorityObjectContract compPriorityObject = new CompliancePriorityObjectContract();
                    if (e.CommandName == RadGrid.UpdateCommandName)
                        compPriorityObject.CPO_ID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CPO_ID"]);
                    compPriorityObject.CPO_Name = txtName.Text.Trim();
                    compPriorityObject.CPO_Description = txtDescription.Text.Trim();
                    if (Presenter.SaveComplPriorityObject(compPriorityObject))
                    {
                        //  e.Canceled = false;
                        if (e.CommandName == RadGrid.UpdateCommandName)
                            base.ShowSuccessMessage("Compliance priority object updated successfully.");
                        else
                            base.ShowSuccessMessage("Compliance priority object saved successfully.");
                        e.Canceled = false;
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowErrorMessage("Some error has occurred. Please try again.");
                    }
                }

                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    Int32 compPriorityObjectID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CPO_ID"]);
                    if (!compPriorityObjectID.IsNullOrEmpty() && compPriorityObjectID > AppConsts.NONE)
                    {
                        if (Presenter.DeleteComplPriorityObject(compPriorityObjectID))
                        {
                            base.ShowSuccessMessage("Compliance priority object is deleted successfully.");
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion


        #endregion

        #region Methods

        #endregion

    }
}