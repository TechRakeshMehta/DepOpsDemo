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
    public partial class AgencyLocation : BaseUserControl, IAgencyLocationView
    {
        #region Variables
        private AgencyLocationPresenter _presenter = new AgencyLocationPresenter();
        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;
        #endregion

        #region Properties

        public AgencyLocationPresenter Presenter
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

        private IAgencyLocationView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public Int32 AgencyRootNodeID
        {
            get
            {
                if (!ViewState["AgencyRootNodeID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["AgencyRootNodeID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["AgencyRootNodeID"] = value;
            }
        }

        List<AgencyLocationDepartmentContract> IAgencyLocationView.lstAgencyLocations
        {
            get
            {
                if (!ViewState["lstAgencyLocations"].IsNullOrEmpty())
                    return (List<AgencyLocationDepartmentContract>)ViewState["lstAgencyLocations"];
                return new List<AgencyLocationDepartmentContract>();
            }
            set
            {
                ViewState["lstAgencyLocations"] = value;
            }
        }

        AgencyLocationDepartmentContract IAgencyLocationView.AgencyLocation
        {
            get;
            set;
        }

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        #endregion

        protected void grdAgencyLocations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetAgencyLocations();
                grdAgencyLocations.DataSource = CurrentViewContext.lstAgencyLocations;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdAgencyLocations_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    WclTextBox txtLocation = e.Item.FindControl("txtLocation") as WclTextBox;
                    WclNumericTextBox txtExperience = e.Item.FindControl("txtExperience") as WclNumericTextBox;
                    CurrentViewContext.AgencyLocation = new AgencyLocationDepartmentContract();
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        CurrentViewContext.AgencyLocation.AgencyLocationID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyLocationID"]);
                    }

                    if (!String.IsNullOrEmpty(txtLocation.Text))
                        CurrentViewContext.AgencyLocation.Location = txtLocation.Text.Trim();
                    if (!String.IsNullOrEmpty(txtExperience.Text))
                        CurrentViewContext.AgencyLocation.Experience = txtExperience.Text.Trim();
                    CurrentViewContext.AgencyLocation.AgencyHierarchyID = CurrentViewContext.AgencyRootNodeID;
                    if (!CurrentViewContext.AgencyLocation.IsNullOrEmpty())
                    {
                        if (Presenter.SaveAgencyLocation())
                        {
                            e.Canceled = false;
                            //base.ShowSuccessMessage("Location is saved successfully.");
                            eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Location saved successfully.");
                            grdAgencyLocations.Rebind();
                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefreshAgencyLocation();", true);
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
                    CurrentViewContext.AgencyLocation = new AgencyLocationDepartmentContract();
                    CurrentViewContext.AgencyLocation.AgencyLocationID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyLocationID"]);
                    if (!Presenter.IsDeptMappedWithLocation(CurrentViewContext.AgencyLocation.AgencyLocationID))
                    {
                        if (Presenter.DeleteAgencyLocation())
                        {
                            e.Canceled = false;
                            //base.ShowSuccessMessage("Location mapping is deleted successfully.");
                            eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Location mapping deleted successfully.");
                            grdAgencyLocations.Rebind();
                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefreshAgencyLocation();", true);
                        }
                        else
                        {
                            e.Canceled = true;
                            //base.ShowErrorMessage("Some error has occurred. Please try again.");
                            eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Some error has occurred. Please try again.");
                        }
                    }
                    else
                    {
                        e.Canceled = true;
                        //base.ShowInfoMessage("Location is currently in use. Please delete department(s) mapped.");
                        eventShowMessage(sender, StatusMessages.INFO_MESSAGE, "Location is currently in use. Please delete department(s) mapped.");
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        #region Grid Events

        #endregion

        #endregion
    }
}