using System;
using Microsoft.Practices.ObjectBuilder;
using Telerik.Web.UI;
using INTSOF.Utils;
using System.Linq;
using Entity;
using CoreWeb.Shell;
using System.Collections.Generic;
using Business.RepoManagers;
using System.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;

namespace CoreWeb.Messaging.Views
{
    public partial class AddressLookup : System.Web.UI.Page, IAddressLookupView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private AddressLookupPresenter _presenter = new AddressLookupPresenter();
        private CustomPagingArgsContract _gridUsersCustomPaging = null;
        private CustomPagingArgsContract _gridMessagingGroupCustomPaging = null;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public IQueryable<OrganizationUser> OrganizationUsers
        {
            set
            {
                grdUsers.DataSource = value.ToList();
            }
        }

        /// <summary>
        /// Gets Communication Type
        /// </summary>
        public lkpCommunicationTypeContext CommunicationType
        {
            get
            {
                lkpCommunicationTypeContext communicationType = lkpCommunicationTypeContext.MESSAGE;
                if (!String.IsNullOrEmpty(Request[AppConsts.COMMUNICATION_TYPE_QUERY_STRING]))
                    communicationType = Request[AppConsts.COMMUNICATION_TYPE_QUERY_STRING].ParseEnumbyCode<lkpCommunicationTypeContext>();
                return communicationType;
            }
        }

        public IQueryable<vw_ListOfUsers> MessagingGroups
        {
            set
            {
                grdMessagingGroups.DataSource = value;
            }
        }

        public Boolean IsApplicant
        {
            get
            {
                return Presenter.GetOrganizationUser(SysXWebSiteUtils.SessionService.OrganizationUserId).IsApplicant.GetValueOrDefault(false);
            }
        }


        public AddressLookupPresenter Presenter
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

        public Int32 SelectedProgramId
        {
            get;
            set;
        }

        public Int32 SelectedTenantId
        {
            get;
            set;
        }

        public IAddressLookupView CurrentViewContext
        {
            get { return this; }
        }

        #region Custom Paging Properties

        /// <summary>
        /// CurrentPageIndex
        /// </summary>
        /// <value> Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                if (grdUsers.Visible == true)
                {
                    return grdUsers.MasterTableView.CurrentPageIndex + 1;
                }
                return grdMessagingGroups.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdUsers.Visible == true)
                {
                    grdUsers.MasterTableView.CurrentPageIndex = value == 0 ? 0 : value - 1;
                }
                grdMessagingGroups.MasterTableView.CurrentPageIndex = value == 0 ? 0 : value - 1;
            }
        }

        /// <summary>
        /// PageSize
        /// </summary>
        /// <value> Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                if (grdUsers.Visible == true)
                {
                    //return grdUsers.PageSize > 100 ? 100 : grdUsers.PageSize;
                    return grdUsers.PageSize;
                }
                //return grdMessagingGroups.PageSize > 100 ? 100 : grdUsers.PageSize;
                return grdMessagingGroups.PageSize;
            }
        }

        /// <summary>
        /// VirtualPageCount
        /// </summary>
        /// <value> Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            set
            {
                if (grdUsers.Visible == true)
                {
                    grdUsers.VirtualItemCount = value;
                    grdUsers.MasterTableView.VirtualItemCount = value;
                }
                grdMessagingGroups.VirtualItemCount = value;
                grdMessagingGroups.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridUsersCustomPaging
        {
            get
            {
                if (_gridUsersCustomPaging.IsNull())
                {
                    _gridUsersCustomPaging = new CustomPagingArgsContract();
                }
                return _gridUsersCustomPaging;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridMessagingGroupCustomPaging
        {
            get
            {
                if (_gridMessagingGroupCustomPaging.IsNull())
                {
                    _gridMessagingGroupCustomPaging = new CustomPagingArgsContract();
                }
                return _gridMessagingGroupCustomPaging;
            }
        }

        public List<String> FilterColumns
        {
            get;
            set;
        }

        public List<String> FilterOperators
        {
            get;
            set;
        }

        public ArrayList FilterValues
        {
            get;
            set;
        }

        public List<String> FilterTypes
        {
            get;
            set;
        }

        public List<Int32> SelectedOrganizationUserIds
        {
            get;
            set;
        }

        #endregion


        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();



                hdnParentScreen.Value = Request["parentScreen"].ToString();
                if (Request["parentScreen"].ToString() == "transferRules")
                {
                    btnToUsers.Text = "From -->";
                    btnCcUsers.Visible = false;
                    acbCcList.Visible = false;


                }
                else
                {
                    btnToUsers.Text = "To -->";
                    btnCcUsers.Visible = true;
                    acbCcList.Visible = true;
                }
                String setUserTo = String.Format("BindUsers('{0}'); return false;", btnToUsers.ClientKey);
                String setUserCc = String.Format("BindUsers('{0}'); return false;", btnCcUsers.ClientKey);
                btnToUsers.Attributes.Add("onClick", setUserTo);
                btnCcUsers.Attributes.Add("onClick", setUserCc);
                if (IsApplicant)
                {
                    dvBccList.Visible = false;
                }
                else
                {
                    dvBccList.Visible = true;
                    String setUserBcc = String.Format("BindUsers('{0}'); return false;", btnBccUsers.ClientKey);
                    btnBccUsers.Attributes.Add("onClick", setUserBcc);
                }
                SetGridVisibility();
            }
            Presenter.OnViewLoaded();

            CurrentViewContext.SelectedTenantId = !String.IsNullOrEmpty(Convert.ToString(Request["tenantId"])) ? Convert.ToInt32(Request["tenantId"]) : AppConsts.NONE;
            CurrentViewContext.SelectedProgramId = !String.IsNullOrEmpty(Convert.ToString(Request["programId"])) ? Convert.ToInt32(Request["programId"]) : AppConsts.NONE;
        }

        #endregion

        #region Button Events



        #endregion

        #region Grid Events

        /// <summary>
        /// Retrieves list of active Users.
        /// </summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data</param>
        protected void grdUsers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!Session["OrgUsersToList"].IsNullOrEmpty())
            {
                var orgUsersToList = Session["OrgUsersToList"] as Dictionary<Int32, String>;
                SelectedOrganizationUserIds = new List<int>();
                SelectedOrganizationUserIds = orgUsersToList.Select(cond => cond.Key).ToList();
            }

            SetGridUsersCustomPaging(CurrentViewContext.GridUsersCustomPaging);
            Presenter.RetrieveUsers(SysXWebSiteUtils.SessionService.OrganizationUserId, CommunicationType);
        }

        protected void grdUsers_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdUsers.FilterMenu;
            RemoveExtraFilterOperator(menu);
            if (grdUsers.clearFilterMethod == null)
                grdUsers.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);
        }

        /// <summary>
        /// Shows email bound column, if communication type context is e-mail   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUsers_PreRender(object sender, EventArgs e)
        {
            if (CommunicationType == lkpCommunicationTypeContext.EMAIL)
                (grdUsers.MasterTableView.GetColumn("aspnet_Users.aspnet_Membership.Email") as GridBoundColumn).Visible = true;
        }

        protected void grdUsers_ItemCommand(object sender, GridCommandEventArgs e)
        {
            #region For Filter command

            if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filter = (Pair)e.CommandArgument;
                ViewState["FilterPair"] = filter;
            }
            FilterGridColumn(grdUsers, CurrentViewContext.GridUsersCustomPaging);

            #endregion

        }

        /// <summary>
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUsers_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            SetSortingFields(e, CurrentViewContext.GridUsersCustomPaging);
        }

        protected void grdMessagingGroups_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            SetGridUsersCustomPaging(CurrentViewContext.GridMessagingGroupCustomPaging);
            Presenter.GetMessagingGroups(SysXWebSiteUtils.SessionService.OrganizationUserId, this.IsApplicant);
        }

        protected void grdMessagingGroups_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdUsers.FilterMenu;
            RemoveExtraFilterOperator(menu);
            if (grdMessagingGroups.clearFilterMethod == null)
                grdMessagingGroups.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);
        }

        protected void grdMessagingGroups_ItemCommand(object sender, GridCommandEventArgs e)
        {
            #region For Filter command

            if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filter = (Pair)e.CommandArgument;
                ViewState["FilterPair"] = filter;
            }
            FilterGridColumn(grdMessagingGroups, CurrentViewContext.GridMessagingGroupCustomPaging);

            #endregion

        }

        /// <summary>
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMessagingGroups_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            SetSortingFields(e, CurrentViewContext.GridMessagingGroupCustomPaging);
        }

        #endregion

        #region DropDown Events



        #endregion

        #endregion

        #region Methods

        #region Private Methods

        private void SetGridVisibility()
        {
            if (this.IsApplicant)
            {
                grdUsers.Visible = false;
                divUsers.Visible = false;
            }
            else
            {
                grdMessagingGroups.Visible = false;
                divMsgGroups.Visible = false;
            }
        }

        private void SetGridUsersCustomPaging(CustomPagingArgsContract customPagingArgsContract)
        {
            customPagingArgsContract.CurrentPageIndex = CurrentPageIndex;
            customPagingArgsContract.PageSize = PageSize;
            customPagingArgsContract.FilterColumns = FilterColumns;
            customPagingArgsContract.FilterOperators = FilterOperators;
            customPagingArgsContract.FilterValues = FilterValues;
            customPagingArgsContract.FilterTypes = FilterTypes;
        }

        private static void RemoveExtraFilterOperator(GridFilterMenu menu)
        {
            Int32 i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString())
                {
                    menu.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        private void SetSortingFields(GridSortCommandEventArgs e, CustomPagingArgsContract customPagingArgsContract)
        {
            if (e.NewSortOrder != GridSortOrder.None)
            {
                ViewState["SortExpression"] = e.SortExpression;
                ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                customPagingArgsContract.SortExpression = e.SortExpression;
                customPagingArgsContract.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
            }
            else
            {
                ViewState["SortExpression"] = String.Empty;
                ViewState["SortDirection"] = false;
                customPagingArgsContract.SortExpression = String.Empty;
                customPagingArgsContract.SortDirectionDescending = false;
            }
        }

        private void FilterGridColumn(WclGrid gridControl, CustomPagingArgsContract customPagingArgsContract)
        {
            if (!ViewState["SortExpression"].IsNull())
            {
                customPagingArgsContract.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                customPagingArgsContract.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
            }
            CurrentViewContext.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
            CurrentViewContext.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
            CurrentViewContext.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);
            CurrentViewContext.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)(ViewState["FilterTypes"]);

            if (ViewState["FilterPair"] != null)
            {
                Pair filter = (Pair)ViewState["FilterPair"];
                Int32 filterIndex = CurrentViewContext.FilterColumns.IndexOf(filter.Second.ToString());
                String filterType = gridControl.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                String filterValue = gridControl.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString() && !filterValue.Equals(String.Empty))
                {

                    if (filterIndex != -1)
                    {
                        CurrentViewContext.FilterOperators[filterIndex] = filter.First.ToString();
                        CurrentViewContext.FilterTypes[filterIndex] = filterType;
                        if (filterType == "System.Decimal")
                        {
                            CurrentViewContext.FilterValues[filterIndex] = Convert.ToDecimal(filterValue);
                        }
                        else if (filterType == "System.Int32")
                        {
                            CurrentViewContext.FilterValues[filterIndex] = Convert.ToInt32(filterValue);
                        }
                        else if (filterType == "System.DateTime")
                        {
                            CurrentViewContext.FilterValues[filterIndex] = Convert.ToDateTime(filterValue);
                        }
                        else
                        {
                            CurrentViewContext.FilterValues[filterIndex] = filterValue.ToLower();
                        }
                    }
                    else
                    {
                        CurrentViewContext.FilterColumns.Add(filter.Second.ToString());
                        CurrentViewContext.FilterOperators.Add(filter.First.ToString());
                        CurrentViewContext.FilterTypes.Add(filterType);
                        if (gridControl.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Decimal")
                        {
                            CurrentViewContext.FilterValues.Add(Convert.ToDecimal(filterValue));
                        }
                        else if (gridControl.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Int32")
                        {
                            CurrentViewContext.FilterValues.Add(Convert.ToInt32(filterValue));
                        }
                        else if (gridControl.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.DateTime")
                        {
                            CurrentViewContext.FilterValues.Add(Convert.ToDateTime(filterValue));
                        }
                        else
                        {
                            CurrentViewContext.FilterValues.Add(filterValue.ToLower());
                        }
                    }
                }
                else if (filterIndex != -1)
                {
                    CurrentViewContext.FilterOperators.RemoveAt(filterIndex);
                    CurrentViewContext.FilterValues.RemoveAt(filterIndex);
                    CurrentViewContext.FilterColumns.RemoveAt(filterIndex);
                    CurrentViewContext.FilterTypes.RemoveAt(filterIndex);
                }
                ViewState["FilterColumns"] = CurrentViewContext.FilterColumns;
                ViewState["FilterOperators"] = CurrentViewContext.FilterOperators;
                ViewState["FilterValues"] = CurrentViewContext.FilterValues;
                ViewState["FilterTypes"] = CurrentViewContext.FilterTypes;
            }
        }

        #endregion

        #region Public Methods

        public void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = null;
            ViewState["FilterOperators"] = null;
            ViewState["FilterValues"] = null;
            ViewState["FilterTypes"] = null;
            ViewState["FilterPair"] = null;
        }


        #endregion

        #endregion
    }
}

