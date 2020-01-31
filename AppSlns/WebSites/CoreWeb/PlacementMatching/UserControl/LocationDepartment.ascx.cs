using CoreWeb.Shell;
using Entity.SharedDataEntity;
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
    public partial class LocationDepartment : BaseUserControl, ILocationDepartmentView
    {
        #region Variables
        private LocationDepartmentPresenter _presenter = new LocationDepartmentPresenter();
        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;
        #endregion

        #region Properties

        public LocationDepartmentPresenter Presenter
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

        public ILocationDepartmentView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        List<Department> ILocationDepartmentView.lstDepartment
        {
            get
            {
                if (!ViewState["lstDepartment"].IsNullOrEmpty())
                {
                    return ViewState["lstDepartment"] as List<Department>;
                }
                return new List<Department>();
            }
            set
            {
                ViewState["lstDepartment"] = value;
            }
        }

        List<StudentType> ILocationDepartmentView.lstStudentType
        {
            get
            {
                if (!ViewState["lstStudentType"].IsNullOrEmpty())
                {
                    return ViewState["lstStudentType"] as List<StudentType>;
                }
                return new List<StudentType>();
            }
            set
            {
                ViewState["lstStudentType"] = value;
            }
        }

        List<AgencyLocationDepartmentContract> ILocationDepartmentView.lstAgencyLocationDepartment
        {
            get
            {
                if (!ViewState["lstAgencyLocationDepartment"].IsNullOrEmpty())
                {
                    return ViewState["lstAgencyLocationDepartment"] as List<AgencyLocationDepartmentContract>;
                }
                return new List<AgencyLocationDepartmentContract>();
            }
            set
            {
                ViewState["lstAgencyLocationDepartment"] = value;
            }
        }

        public Int32 AgencyLocationID
        {
            get
            {
                if (!ViewState["AgencyLocationID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["AgencyLocationID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["AgencyLocationID"] = value;
            }
        }
        AgencyLocationDepartmentContract ILocationDepartmentView.LocationDepartment
        {
            get
            {
                if (!ViewState["LocationDepartment"].IsNullOrEmpty())
                {
                    return ViewState["LocationDepartment"] as AgencyLocationDepartmentContract;
                }
                return new AgencyLocationDepartmentContract();
            }
            set
            {
                ViewState["LocationDepartment"] = value;
            }
        }
        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                }
                GetDepartments();
                GetStudentTypes();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        #endregion

        #region Grid Events

        protected void grdLocationDepartment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetAgencyLocationDepartment();  // need to change to sp
                grdLocationDepartment.DataSource = CurrentViewContext.lstAgencyLocationDepartment;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdLocationDepartment_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    WclComboBox ddlDepartment = e.Item.FindControl("ddlDepartment") as WclComboBox;
                    WclComboBox cmbStudentType = e.Item.FindControl("cmbStudentType") as WclComboBox;

                    CurrentViewContext.LocationDepartment = new AgencyLocationDepartmentContract();
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        CurrentViewContext.LocationDepartment.AgencyLocationDepartmentID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyLocationDepartmentID"]);
                    }

                    CurrentViewContext.LocationDepartment.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);
                    CurrentViewContext.LocationDepartment.lstStudentTypeID = new List<Int32>();

                    foreach (RadComboBoxItem item in cmbStudentType.CheckedItems)
                    {
                        CurrentViewContext.LocationDepartment.lstStudentTypeID.Add(Convert.ToInt32(item.Value));
                    }

                    CurrentViewContext.LocationDepartment.AgencyLocationID = CurrentViewContext.AgencyLocationID;

                    if (!CurrentViewContext.LocationDepartment.IsNullOrEmpty())
                    {
                        if (Presenter.SaveAgencyLocationDepartment())
                        {
                            e.Canceled = false;
                            //base.ShowSuccessMessage("Department mapping is saved successfully.");
                            eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Department mapping saved successfully.");
                            grdLocationDepartment.Rebind();
                        }
                        else
                        {
                            e.Canceled = true;
                            //base.ShowErrorMessage("Some error has occurred. Please try again.");
                            eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Some error has occurred. Please try again.");
                        }
                    }
                }
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.LocationDepartment = new AgencyLocationDepartmentContract();
                    CurrentViewContext.LocationDepartment.AgencyLocationDepartmentID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyLocationDepartmentID"]);
                    if (Presenter.DeleteAgencyLocationDepartment())
                    {
                        e.Canceled = false;
                        //base.ShowSuccessMessage("Department mapping is deleted successfully.");
                        eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Department mapping deleted successfully.");
                        grdLocationDepartment.Rebind();
                    }
                    else
                    {
                        e.Canceled = true;
                        //base.ShowErrorMessage("Some error has occurred. Please try again.");
                        eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Some error has occurred. Please try again.");
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdLocationDepartment_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlDepartment = e.Item.FindControl("ddlDepartment") as WclComboBox;
                    WclComboBox cmbStudentType = e.Item.FindControl("cmbStudentType") as WclComboBox;
                    List<Int32> lstAlreadyMappedDeptIds = CurrentViewContext.lstAgencyLocationDepartment.Select(Sel => Sel.DepartmentID).ToList();
                    CurrentViewContext.lstDepartment = CurrentViewContext.lstDepartment.Where(cond => !lstAlreadyMappedDeptIds.Contains(cond.DP_ID)).ToList(); // Remove already mapped departments.
                    ddlDepartment.DataSource = CurrentViewContext.lstDepartment;
                    ddlDepartment.DataBind();
                    ddlDepartment.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
                    cmbStudentType.DataSource = CurrentViewContext.lstStudentType;
                    cmbStudentType.DataBind();
                    //cmbStudentType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT All--"));

                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        AgencyLocationDepartmentContract agencyLocationDepartmentContract = (AgencyLocationDepartmentContract)e.Item.DataItem;
                        if (!agencyLocationDepartmentContract.IsNullOrEmpty())
                        {
                            Int32 lastIndex = ddlDepartment.Items.Count - 1;
                            ddlDepartment.Items.Insert(lastIndex+1, new Telerik.Web.UI.RadComboBoxItem(agencyLocationDepartmentContract.Department,agencyLocationDepartmentContract.DepartmentID.ToString()));
                            ddlDepartment.SelectedValue = agencyLocationDepartmentContract.DepartmentID.ToString();
                            foreach (RadComboBoxItem item in cmbStudentType.Items)
                            {
                                item.Checked = agencyLocationDepartmentContract.lstStudentTypeID.Contains(Convert.ToInt32(item.Value));
                            }
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        #endregion

        #region Dropdown Events

        //protected void ddlDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    try
        //    {
        //        GridEditFormInsertItem insertItem = (sender as WclComboBox).NamingContainer as GridEditFormInsertItem;
        //        String selectedValue = (sender as WclComboBox).SelectedValue;
        //        WclComboBox cmbStudentType = insertItem.FindControl("cmbStudentType") as WclComboBox;
        //        if (!selectedValue.IsNullOrEmpty())
        //        {
        //            cmbStudentType.DataSource = CurrentViewContext.lstStudentType;
        //            cmbStudentType.DataBind();
        //        }
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        //base.ShowErrorMessage(ex.Message);
        //        eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        base.LogError(ex);
        //        //base.ShowErrorMessage(ex.Message);
        //        eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
        //    }
        //}

        #endregion

        #endregion

        #region Methods

        private void GetDepartments()
        {
            Presenter.GetDepartments();
        }

        private void GetStudentTypes()
        {
            Presenter.GetStudentTypes();
        }

        #endregion


    }
}