using System;
using Microsoft.Practices.ObjectBuilder;
using CoreWeb.Shell;
using System.Collections.Generic;
using Entity;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

using System.Linq;
using System.Collections;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI;

namespace CoreWeb.Messaging.Views
{

    public partial class SubscriptionSettingByAdmin : BaseUserControl, ISubscriptionSettingByAdminView
    {
        private SubscriptionSettingByAdminPresenter _presenter=new SubscriptionSettingByAdminPresenter();
        private CustomPagingArgsContract _gridCustomPaging = null;

        public ISubscriptionSettingByAdminView CurrentViewContext
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
                return grdUsers.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdUsers.MasterTableView.CurrentPageIndex > 0)
                {
                    grdUsers.MasterTableView.CurrentPageIndex = value - 1;
                }
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
                //return grdUsers.PageSize > 100 ? 100 : grdUsers.PageSize;
                return grdUsers.PageSize;
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
                grdUsers.VirtualItemCount = value;
                grdUsers.MasterTableView.VirtualItemCount = value;

            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (_gridCustomPaging.IsNull())
                {
                    _gridCustomPaging = new CustomPagingArgsContract();
                }
                return _gridCustomPaging;
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

        public List<String> FilterTypes
        {
            get;
            set;
        }

        public ArrayList FilterValues
        {
            get;
            set;
        }
        #endregion
        //private SearchItemDataContract _gridSearchContract = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            lblSuccess.Visible = false;
            lblSuccess.Text = String.Empty;
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Email Settings";
                base.SetPageTitle("Email Settings");

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

        
        public SubscriptionSettingByAdminPresenter Presenter
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


        public int OrganizationUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IQueryable<vw_ApplicantUser> ApplicantUsers
        {
            get;
            set;
        }

        IEnumerable<lkpCommunicationEvent> notificationCommunicationEvents = null;
        public IEnumerable<lkpCommunicationEvent> NotificationCommunicationEvents
        {
            get { return notificationCommunicationEvents; }
            set { notificationCommunicationEvents = value; }
        }

        IEnumerable<lkpCommunicationEvent> reminderCommunicationEvents = null;
        public IEnumerable<lkpCommunicationEvent> ReminderCommunicationEvents
        {
            get { return reminderCommunicationEvents; }
            set { reminderCommunicationEvents = value; }
        }

        List<UserCommunicationSubscriptionSetting> selectedUserCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();
        public List<UserCommunicationSubscriptionSetting> SelectedUserCommunicationSubscriptionSettings
        {
            get { return selectedUserCommunicationSubscriptionSettings; }
            set { selectedUserCommunicationSubscriptionSettings = value; }
        }

        List<UserCommunicationSubscriptionSetting> unSelectedUserCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();
        public List<UserCommunicationSubscriptionSetting> UnSelectedUserCommunicationSubscriptionSettings
        {
            get { return unSelectedUserCommunicationSubscriptionSettings; }
            set { unSelectedUserCommunicationSubscriptionSettings = value; }
        }

        protected void grdUsers_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdUsers.FilterMenu;

            if (grdUsers.clearFilterMethod == null)
                grdUsers.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);
            int i = 0;
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

        protected void grdUsers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
            GridCustomPaging.PageSize = PageSize;
            GridCustomPaging.FilterColumns = FilterColumns;
            GridCustomPaging.FilterOperators = FilterOperators;
            GridCustomPaging.FilterValues = FilterValues;
            GridCustomPaging.FilterTypes = FilterTypes;
            Presenter.GetApplicantUsers();
            grdUsers.DataSource = CurrentViewContext.ApplicantUsers;

        }

        protected void grdUsers_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                Int32 organizationUserId = (Int32)gridDataItem.GetDataKeyValue("OrganizationUserID");

                CheckBoxList checkBoxList = (CheckBoxList)e.Item.FindControl("cblNotification");
                IEnumerable<UserCommunicationSubscriptionSetting> selectedUserCommunicationSubscriptionSetting = Presenter.GetNotificationUserCommunicationSubscriptionSetting(organizationUserId);
                BindAndSelectSubscription(checkBoxList, NotificationCommunicationEvents, selectedUserCommunicationSubscriptionSetting);

                checkBoxList = (CheckBoxList)e.Item.FindControl("cblReminder");
                selectedUserCommunicationSubscriptionSetting = Presenter.GetReminderUserCommunicationSubscriptionSetting(organizationUserId);
                BindAndSelectSubscription(checkBoxList, ReminderCommunicationEvents, selectedUserCommunicationSubscriptionSetting);
            }
        }


        protected void grdUsers_ItemCommand(object sender, GridCommandEventArgs e)
        {
            #region For Filter command

            if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filter = (Pair)e.CommandArgument;
                ViewState["FilterPair"] = filter;
            }
            FilterGridColumn(grdUsers, CurrentViewContext.GridCustomPaging);

            #endregion

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SetSelectedAndUnSelectedSubscriptionSettings();
            Presenter.AddUserCommunicationSubscriptionSettings();
            lblSuccess.Visible = true;
            lblSuccess.ShowMessage("Email Settings saved successfully.", MessageType.SuccessMessage);
        }

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkBoxList"></param>
        /// <param name="communicationEvents"></param>
        /// <param name="selectedUserCommunicationSubscriptionSettings"></param>
        private void BindAndSelectSubscription(CheckBoxList checkBoxList, IEnumerable<lkpCommunicationEvent> communicationEvents, IEnumerable<UserCommunicationSubscriptionSetting> selectedUserCommunicationSubscriptionSettings)
        {
            checkBoxList.RepeatColumns = 2;
            checkBoxList.DataSource = communicationEvents;
            checkBoxList.DataBind();
            foreach (ListItem listItem in checkBoxList.Items)
            {
                if (selectedUserCommunicationSubscriptionSettings.Any(x => x.CommunicationEventID.Equals(Int32.Parse(listItem.Value))))
                    listItem.Selected = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="communicationTypeId"></param>
        /// <param name="checkBoxList"></param>
        /// <returns></returns>
        private void GetSelectedAndUnSelectedSubscriptionSettings(
            Int32 organizationUserId,
            Int32 communicationTypeId,
            CheckBoxList checkBoxList,
            out List<UserCommunicationSubscriptionSetting> selectedUserCommunicationSubscriptionSettings,
            out List<UserCommunicationSubscriptionSetting> unSelectedUserCommunicationSubscriptionSettings)
        {
            selectedUserCommunicationSubscriptionSettings = null;
            unSelectedUserCommunicationSubscriptionSettings = null;

            foreach (ListItem listItem in checkBoxList.Items)
            {
                if (listItem.Selected)
                {
                    if (selectedUserCommunicationSubscriptionSettings == null)
                        selectedUserCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();

                    selectedUserCommunicationSubscriptionSettings.Add(
                         new UserCommunicationSubscriptionSetting()
                         {
                             OrganizationUserID = organizationUserId,
                             CommunicationTypeID = communicationTypeId,
                             CommunicationEventID = Int32.Parse(listItem.Value)
                         });
                }
                else
                {
                    if (unSelectedUserCommunicationSubscriptionSettings == null)
                        unSelectedUserCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();

                    unSelectedUserCommunicationSubscriptionSettings.Add(
                         new UserCommunicationSubscriptionSetting()
                         {
                             OrganizationUserID = organizationUserId,
                             CommunicationTypeID = communicationTypeId,
                             CommunicationEventID = Int32.Parse(listItem.Value)
                         });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetSelectedAndUnSelectedSubscriptionSettings()
        {
            SelectedUserCommunicationSubscriptionSettings.Clear();
            UnSelectedUserCommunicationSubscriptionSettings.Clear();

            List<UserCommunicationSubscriptionSetting> result = new List<UserCommunicationSubscriptionSetting>();
            foreach (GridDataItem gridDataItem in grdUsers.Items)
            {
                Int32 organizationUserId = (Int32)gridDataItem.GetDataKeyValue("OrganizationUserID");
                CheckBoxList cblNotification = (CheckBoxList)gridDataItem.FindControl("cblNotification");
                CheckBoxList cblReminder = (CheckBoxList)gridDataItem.FindControl("cblReminder");

                List<UserCommunicationSubscriptionSetting> selectedUserCommunicationSubscriptionSettings = null;
                List<UserCommunicationSubscriptionSetting> unSelectedUserCommunicationSubscriptionSettings = null;


                if (cblNotification != null)
                {
                    selectedUserCommunicationSubscriptionSettings = null;
                    unSelectedUserCommunicationSubscriptionSettings = null;

                    GetSelectedAndUnSelectedSubscriptionSettings(
                        organizationUserId,
                        Presenter.NotificationCommunicationTypeId,
                        cblNotification,
                        out selectedUserCommunicationSubscriptionSettings,
                        out unSelectedUserCommunicationSubscriptionSettings);

                    if (selectedUserCommunicationSubscriptionSettings != null)
                        SelectedUserCommunicationSubscriptionSettings.AddRange(selectedUserCommunicationSubscriptionSettings);

                    if (unSelectedUserCommunicationSubscriptionSettings != null)
                        UnSelectedUserCommunicationSubscriptionSettings.AddRange(unSelectedUserCommunicationSubscriptionSettings);
                }

                if (cblReminder != null)
                {
                    selectedUserCommunicationSubscriptionSettings = null;
                    unSelectedUserCommunicationSubscriptionSettings = null;

                    GetSelectedAndUnSelectedSubscriptionSettings(
                        organizationUserId,
                        Presenter.ReminderCommunicationTypeId,
                        cblReminder,
                        out selectedUserCommunicationSubscriptionSettings,
                        out unSelectedUserCommunicationSubscriptionSettings);

                    if (selectedUserCommunicationSubscriptionSettings != null)
                        SelectedUserCommunicationSubscriptionSettings.AddRange(selectedUserCommunicationSubscriptionSettings);

                    if (unSelectedUserCommunicationSubscriptionSettings != null)
                        UnSelectedUserCommunicationSubscriptionSettings.AddRange(unSelectedUserCommunicationSubscriptionSettings);
                }
            }
        }


        #endregion


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
            CurrentViewContext.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)ViewState["FilterTypes"];
            if (ViewState["FilterPair"] != null)
            {
                Pair filter = (Pair)ViewState["FilterPair"];
                Int32 filterIndex = CurrentViewContext.FilterColumns.IndexOf(filter.Second.ToString());

                if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                {
                    String filterValue = gridControl.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;
                    String filterType=gridControl.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                    if (filterIndex != -1)
                    {
                        CurrentViewContext.FilterOperators[filterIndex] = filter.First.ToString();
                        CurrentViewContext.FilterTypes[filterIndex] = filterType;
                        if (filterType == "System.Decimal")
                        {

                            if (CurrentViewContext.FilterOperators.Contains("IsNull"))
                            {
                                CurrentViewContext.FilterOperators[filterIndex] = "NullOtherThanString";
                                CurrentViewContext.FilterValues[filterIndex] = AppConsts.NONE;
                            }
                            else
                            {
                                Decimal decFilterValue = 0;
                                Decimal.TryParse(filterValue, out decFilterValue);
                                CurrentViewContext.FilterValues[filterIndex] = decFilterValue;
                            }

                        }
                        else if (filterType == "System.Int32")
                        {


                            if (CurrentViewContext.FilterOperators.Contains("IsNull"))
                            {
                                CurrentViewContext.FilterOperators[filterIndex] = "NullOtherThanString";
                                CurrentViewContext.FilterValues[filterIndex] = AppConsts.NONE;
                            }
                            else
                            {
                                Int32 intFilterVal = 0;
                                Int32.TryParse(filterValue, out intFilterVal);
                                CurrentViewContext.FilterValues[filterIndex] = intFilterVal;
                            }

                        }
                        else if (filterType == "System.DateTime")
                        {

                            if (CurrentViewContext.FilterOperators.Contains("IsNull"))
                            {
                                CurrentViewContext.FilterOperators[filterIndex] = "NullOtherThanString";
                                CurrentViewContext.FilterValues[filterIndex] = AppConsts.NONE;
                            }
                            else
                            {
                                DateTime dtFilterValue = Convert.ToDateTime("01/01/0001");
                                DateTime.TryParse(filterValue, out dtFilterValue);
                                CurrentViewContext.FilterValues[filterIndex] = dtFilterValue;
                            }

                        }
                        else
                        {
                            CurrentViewContext.FilterValues[filterIndex] = filterValue;
                        }


                    }
                    else
                    {
                        CurrentViewContext.FilterColumns.Add(filter.Second.ToString());
                        CurrentViewContext.FilterOperators.Add(filter.First.ToString());
                        CurrentViewContext.FilterTypes.Add(filterType);
                        if (filterType == "System.Decimal")
                        {

                            if (CurrentViewContext.FilterOperators.Contains("IsNull"))
                            {
                                Int32 index = CurrentViewContext.FilterOperators.IndexOf("IsNull");
                                CurrentViewContext.FilterOperators[index] = "NullOtherThanString";
                                CurrentViewContext.FilterValues.Add(AppConsts.NONE);
                            }
                            else
                            {
                                Decimal decFilterValue = 0;
                                Decimal.TryParse(filterValue, out decFilterValue);
                                CurrentViewContext.FilterValues.Add(decFilterValue);
                            }
                        }
                        else if (filterType == "System.Int32")
                        {

                            if (CurrentViewContext.FilterOperators.Contains("IsNull"))
                            {
                                Int32 index = CurrentViewContext.FilterOperators.IndexOf("IsNull");
                                CurrentViewContext.FilterOperators[index] = "NullOtherThanString";
                                CurrentViewContext.FilterValues.Add(AppConsts.NONE);
                            }
                            else
                            {
                                Int32 intfilterVal = 0;
                                Int32.TryParse(filterValue, out intfilterVal);
                                CurrentViewContext.FilterValues.Add(intfilterVal);
                            }
                        }
                        else if (filterType == "System.DateTime")
                        {

                            if (CurrentViewContext.FilterOperators.Contains("IsNull"))
                            {
                                Int32 index = CurrentViewContext.FilterOperators.IndexOf("IsNull");
                                CurrentViewContext.FilterOperators[index] = "NullOtherThanString";
                                CurrentViewContext.FilterValues.Add(AppConsts.NONE);
                            }
                            else
                            {
                                DateTime dtFilterValue = Convert.ToDateTime("01/01/0001");
                                DateTime.TryParse(filterValue, out dtFilterValue);
                                CurrentViewContext.FilterValues.Add(dtFilterValue);
                            }
                        }
                        else
                        {
                            CurrentViewContext.FilterValues.Add(filterValue);
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


        public void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = null;
            ViewState["FilterOperators"] = null;
            ViewState["FilterValues"] = null;
            ViewState["FilterPair"] = null;
            ViewState["FilterTypes"] = null;
        }
    }
}

