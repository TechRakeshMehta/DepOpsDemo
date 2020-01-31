using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class AgencyReviewQueue : BaseUserControl, IAgencyReviewQueueView
    {
        #region VARIABLES

        #region PRIVATE VARIABLES

        private AgencyReviewQueuePresenter _presenter = new AgencyReviewQueuePresenter();
        private String _selectedTenantIdsCSV = String.Empty;
        private String _selectedSrchCodesCSV = String.Empty;

        #endregion

        #endregion

        #region PROPERTIES

        public AgencyReviewQueuePresenter Presenter
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

        /// <summary>
        /// List of Tenants
        /// </summary>
        List<TenantDetailContract> IAgencyReviewQueueView.lstTenants
        {
            set
            {
                cmbTenant.DataSource = value;
                cmbTenant.DataBind();
                //cmbTenant.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));
            }
        }

        /// <summary>
        /// List of lkpAgencySearchStatus
        /// </summary>
        List<AgencySearchStatusContract> IAgencyReviewQueueView.lstAgencySearchStatus
        {
            set
            {
                cmbSearchStatus.DataSource = value;
                cmbSearchStatus.DataBind();
                SelectDefaultSrchStatus();
            }
        }

        public IAgencyReviewQueueView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IAgencyReviewQueueView.lstSelectedTenantIds
        {
            get
            {
                _selectedTenantIdsCSV = String.Empty;
                foreach (var item in cmbTenant.Items.Where(itm => itm.Checked))
                {
                    _selectedTenantIdsCSV += item.Value + ",";
                }
                if (!String.IsNullOrEmpty(_selectedTenantIdsCSV))
                {
                    _selectedTenantIdsCSV = _selectedTenantIdsCSV.Substring(0, _selectedTenantIdsCSV.LastIndexOf(','));
                }
                return _selectedTenantIdsCSV;
            }
        }

        String IAgencyReviewQueueView.lstSelectedSrchCodes
        {
            get
            {
                foreach (var item in cmbSearchStatus.Items.Where(itm => itm.Checked))
                {
                    _selectedSrchCodesCSV += item.Value + ",";
                }

                if (!String.IsNullOrEmpty(_selectedSrchCodesCSV))
                {
                    _selectedSrchCodesCSV = _selectedSrchCodesCSV.Substring(0, _selectedSrchCodesCSV.LastIndexOf(','));
                }
                return _selectedSrchCodesCSV;
            }
        }

        List<AgencyReviewQueueContract> IAgencyReviewQueueView.lstAgencies
        {
            get;
            set;
        }

        #region Custom paging parameters

        Int32 IAgencyReviewQueueView.CurrentPageIndex
        {
            get
            {
                return grdAgencies.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdAgencies.MasterTableView.CurrentPageIndex > 0)
                {
                    grdAgencies.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        Int32 IAgencyReviewQueueView.PageSize
        {
            get
            {
                return grdAgencies.PageSize;
            }
            set
            {
                grdAgencies.PageSize = value;
            }
        }

        Int32 IAgencyReviewQueueView.VirtualRecordCount
        {
            get
            {
                if (ViewState["ItemCount"] != null)
                    return Convert.ToInt32(ViewState["ItemCount"]);
                else
                    return 0;
            }
            set
            {
                ViewState["ItemCount"] = value;
                grdAgencies.VirtualItemCount = value;
                grdAgencies.MasterTableView.VirtualItemCount = value;
            }
        }

        CustomPagingArgsContract IAgencyReviewQueueView.GridCustomPaging
        {
            get
            {
                if (ViewState["_gridCustomPaging"] == null)
                {
                    ViewState["_gridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["_gridCustomPaging"];
            }
            set
            {
                ViewState["_gridCustomPaging"] = value;
                CurrentViewContext.VirtualRecordCount = value.VirtualPageCount;
                CurrentViewContext.PageSize = value.PageSize;
                CurrentViewContext.CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        List<Int32> IAgencyReviewQueueView.lstSelectedAgencyIds
        {
            get;
            set;
        }

        String IAgencyReviewQueueView.StatusCode
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region EVENTS

        /// <summary>
        /// Page OnInit Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Agency Review Queue";
                base.SetPageTitle("Agency Review Queue");
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
            if (!IsPostBack)
            {
                Presenter.GetTenants();
                Presenter.GetAgencySearchStatus();
                ucAgencySrch.BindFilterCombobox();
                ucAgencySrch.AllowPostback = false;
            }
        }

        /// <summary>
        /// Perform Search and display the search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_SaveClick(object sender, EventArgs e)
        {
            grdAgencies.Rebind();
        }

        /// <summary>
        /// Redirect to Dashboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_CancelClick(object sender, EventArgs e)
        {
            try
            { 
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
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
        /// Reset the Search form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_SubmitClick(object sender, EventArgs e)
        {
            cmbTenant.SelectedValue = String.Empty;
            cmbTenant.Items.ForEach(itm => itm.Checked = false);
            cmbSearchStatus.Items.ForEach(itm => itm.Checked = false);
            SelectDefaultSrchStatus();
            CurrentViewContext.VirtualRecordCount = 0;

            grdAgencies.DataSource = new List<AgencyReviewQueueContract>();
            grdAgencies.DataBind();
            cmdStatusUpdate.Visible = false;
            ucAgencySrch.ResetCombobox();
        }

        /// <summary>
        /// Change status to Reviewed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdStatusUpdate_SaveClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.StatusCode = AgencySearchStausTypes.REVIEWED.GetStringValue();
                UpdateRebind();
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
        /// Change status to Available for Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdStatusUpdate_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.StatusCode = AgencySearchStausTypes.AVAILABLE.GetStringValue();
                UpdateRebind();
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

        protected void grdAgencies_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            CurrentViewContext.GridCustomPaging.CurrentPageIndex = CurrentViewContext.CurrentPageIndex;
            CurrentViewContext.GridCustomPaging.PageSize = CurrentViewContext.PageSize;
            CurrentViewContext.GridCustomPaging = CurrentViewContext.GridCustomPaging;

            Presenter.GetAgencies();
            grdAgencies.DataSource = CurrentViewContext.lstAgencies;
            grdAgencies.Visible = true;

            if (!CurrentViewContext.lstAgencies.IsNullOrEmpty())
            {
                cmdStatusUpdate.Visible = true;
            }
            else
            {
                cmdStatusUpdate.Visible = false;
            }
        }

        protected void grdAgencies_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
                    CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
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

        #region METHODS

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Update the Status in the database and rebind the grids.
        /// </summary>
        private void UpdateRebind()
        {
            SetSelectedAgencyIds();

            if (CurrentViewContext.lstSelectedAgencyIds.IsNullOrEmpty())
            {
                base.ShowInfoMessage("Please select atleast one agency.");
                return;
            }

            Presenter.SetAgencySearchStatus();
            grdAgencies.Rebind();
            base.ShowSuccessMessage("Status successfully updated for the selected Agencies.");
            ucAgencySrch.BindFilterCombobox();
        }

        /// <summary>
        /// Set the selected AgencyIDs for the status update.
        /// </summary>
        private void SetSelectedAgencyIds()
        {
            CurrentViewContext.lstSelectedAgencyIds = new List<Int32>();
            foreach (GridDataItem dataItem in grdAgencies.MasterTableView.Items)
            {
                var _chkBox = dataItem.FindControl("chk") as CheckBox;
                if (_chkBox.IsNotNull() && _chkBox.Checked)
                {
                    CurrentViewContext.lstSelectedAgencyIds.Add(Convert.ToInt32(dataItem.GetDataKeyValue("AgencyId")));
                }
            }
        }

        /// <summary>
        /// Set the default search option as checked.
        /// </summary>
        private void SelectDefaultSrchStatus()
        {
            var _notReviewedStatuCode = AgencySearchStausTypes.NOT_REVIEWED.GetStringValue();
            cmbSearchStatus.Items.Where(x => x.Value == _notReviewedStatuCode).First().Checked = true;
        }

        #endregion

        #endregion
    }
}