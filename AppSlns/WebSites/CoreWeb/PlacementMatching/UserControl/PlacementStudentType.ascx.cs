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
    public partial class PlacementStudentType : BaseUserControl, IStudentTypeView
    {
        StudentTypeViewPresenter _presenter = new StudentTypeViewPresenter();
        public StudentTypeViewPresenter Presenter
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
      List<StudentTypeContract> IStudentTypeView.lstStudentTypes
        {
            get;
            set;
        }

        public IStudentTypeView CurrentViewContext
        {
            get
            {
                return this;
            }

        }

        StudentTypeContract IStudentTypeView.studentTypeContract { get; set; }
        Int32 IStudentTypeView.CurrentUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }
        String IStudentTypeView.SuccessMsg { get; set; }
        String IStudentTypeView.ErrorMsg { get; set; }
        String IStudentTypeView.InfoMsg { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Grid Events


        protected void grStudentType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetStudentTypes();
                grStudentType.DataSource = CurrentViewContext.lstStudentTypes;
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

        protected void grStudentType_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
           try
            {
                CurrentViewContext.studentTypeContract = new StudentTypeContract();
                CurrentViewContext.studentTypeContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.studentTypeContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                Presenter.InsertStudentType();
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

        protected void grStudentType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.studentTypeContract = new StudentTypeContract();
                CurrentViewContext.studentTypeContract.StudentTypeId = Convert.ToInt32((e.Item.FindControl("StudentTypeId") as WclTextBox).Text.Trim());
                CurrentViewContext.studentTypeContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.studentTypeContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                if (Presenter.UpdateStudentType())
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Student Type updated successfully.");
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

        protected void grStudentType_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 studentTypeID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("StudentTypeId")); ;
                if (Presenter.DeleteStudentType(studentTypeID))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("StudentType deleted successfully.");
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

        protected void grStudentType_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    StudentTypeContract studentType = (StudentTypeContract)e.Item.DataItem;


                    //Restricts the user to edit and delete Rule Template if RuleTemplate is associated with any Rule.
                    if (studentType.IsMappingExists)
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