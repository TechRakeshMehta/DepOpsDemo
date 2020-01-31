using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class PlacementDepartment : BaseUserControl, IDepartmentView
    {
        DepartmentViewPresenter _presenter = new DepartmentViewPresenter();
        public DepartmentViewPresenter Presenter
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
        List<DepartmentContract> IDepartmentView.lstDepartments
        {
            get;
            set;
        }
        DepartmentContract IDepartmentView.departmentContract
        {
            get;
            set;
        }
        public IDepartmentView CurrentViewContext
        {
            get
            {
                return this;
            }

        }
        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        String IDepartmentView.SuccessMsg { get; set; }
        String IDepartmentView.ErrorMsg { get; set; }
        String IDepartmentView.InfoMsg { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region Grid Event
        protected void grDepartment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetDepartments();
            grDepartment.DataSource = CurrentViewContext.lstDepartments;
        }

        protected void grDepartment_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.departmentContract = new DepartmentContract();
                CurrentViewContext.departmentContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.departmentContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                Presenter.InsertDepartment();
                if (!CurrentViewContext.SuccessMsg.IsNullOrEmpty())
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMsg);
                }
                else if (!CurrentViewContext.ErrorMsg.IsNullOrEmpty())
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMsg);
                }
                else
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.InfoMsg);
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

        protected void grDepartment_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.departmentContract = new DepartmentContract();
                CurrentViewContext.departmentContract.DepartmentID = Convert.ToInt32((e.Item.FindControl("txtDepartmentID") as WclTextBox).Text.Trim());
                CurrentViewContext.departmentContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.departmentContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                if (Presenter.UpdateDepartment())
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Department updated successfully.");
                }
                else
                {
                    e.Canceled = true;
                    base.ShowErrorMessage("Some error occurred.Please try again.");
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

        protected void grDepartment_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 departmentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("DepartmentID")); ;
                if (Presenter.DeleteDepartment(departmentID))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Department deleted successfully.");
                }
                else
                {
                    e.Canceled = true;
                    base.ShowErrorMessage("Some error occurred.Please try again.");
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

        protected void grDepartment_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    DepartmentContract department = (DepartmentContract)e.Item.DataItem;
                 

                    //Restricts the user to edit and delete Rule Template if RuleTemplate is associated with any Rule.
                    if (department.IsMappingExists)
                    {
                        //RadButton btnEdit = item["EditCommandColumn"].FindControl("btnEdit") as RadButton;
                        //btnEdit.Text = "View";
                        //btnEdit.Icon.PrimaryIconCssClass = null;
                        //btnEdit.Icon.PrimaryIconUrl = "~/App_Themes/Default/images/View.png";

                        ImageButton deleteColumn = item["DeleteColumn"].Controls[0] as ImageButton;
                        deleteColumn.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}