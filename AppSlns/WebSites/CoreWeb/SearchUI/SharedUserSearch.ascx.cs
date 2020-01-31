using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Telerik.Web.UI;

namespace CoreWeb.SearchUI.Views
{
    public partial class SharedUserSearch : BaseUserControl, ISharedUserSearch
    {
        #region VARIABLES

        #region PUBLIC VARIABLES

        #endregion

        #region PRIVATE VARIABLES
        private SharedUserSearchPresenter _presenter = new SharedUserSearchPresenter();
        private String _viewType;
        private CustomPagingArgsContract _gridCustomPaging = null;
        #endregion

        #endregion

        #region PROPERTIES

        #region PRIVATE PROPERTIES

        //Represents the Object of CurrentView Context
        ISharedUserSearch CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        //Represents the Object of Presenter
        SharedUserSearchPresenter Presenter
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

        #region PUBLIC PROPERTIES

        //Represents the OrganizationUserID of SharedUser
        Int32 ISharedUserSearch.SharedUserID
        {
            get
            {
                if (!txtUserId.Text.IsNullOrEmpty())
                {
                    return Convert.ToInt32(txtUserId.Text);
                }
                return AppConsts.NONE;
            }
            set
            {
                if (value > 0)
                {
                    ViewState["SharedUserID"] = txtUserId.Text = Convert.ToString(value);
                }
            }
        }

        //Represents the FirstName of SharedUser
        String ISharedUserSearch.FirstName
        {
            get
            {
                return txtFirstName.Text;
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        //Represents the LastName of SharedUser
        String ISharedUserSearch.LastName
        {
            get
            {
                return txtLastName.Text;
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        //Represents the UserName of SharedUser
        String ISharedUserSearch.UserName
        {
            get
            {
                return txtUserName.Text;
            }
            set
            {
                txtUserName.Text = value;
            }
        }

        //Represents the Email Address of SharedUser
        String ISharedUserSearch.EmailAddress
        {
            get
            {
                return txtEmail.Text;
            }
            set
            {
                txtEmail.Text = value;
            }
        }

        //Represents the SharedUserSearchContractList
        List<SharedUserSearchContract> ISharedUserSearch.SharedUserResultContract { get; set; }

        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 ISharedUserSearch.CurrentPageIndex
        {
            get
            {
                return grdSharedUserSearch.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdSharedUserSearch.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 ISharedUserSearch.PageSize
        {
            get
            {
                return grdSharedUserSearch.PageSize;
            }
            set
            {
                grdSharedUserSearch.PageSize = value;
            }
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 ISharedUserSearch.VirtualPageCount
        {
            get
            {
                return grdSharedUserSearch.VirtualItemCount;
            }
            set
            {
                grdSharedUserSearch.VirtualItemCount = value;
                grdSharedUserSearch.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract ISharedUserSearch.GridCustomPaging
        {
            get
            {
                if (_gridCustomPaging.IsNull())
                {
                    _gridCustomPaging = new CustomPagingArgsContract();
                }
                return _gridCustomPaging;
            }
            set
            {
                _gridCustomPaging = value;
                CurrentViewContext.VirtualPageCount = value.VirtualPageCount;
                CurrentViewContext.PageSize = value.PageSize;
                CurrentViewContext.CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        #endregion


        #endregion

        #endregion

        #region EVENTS

        #region PAGE EVENTS

        /// <summary>
        /// Page On Init Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Shared User Search";
                base.SetPageTitle("Shared User Search");
                fsucCmdBarButton.SubmitButton.CausesValidation = false;
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
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search Shared Users per the criteria entered above";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";

            if (!IsPostBack)
            {
                //Getting Data from Session if Back to Search button pressed from ClientProfilePage
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("BackToSharedUserSearch")
                        && args["BackToSharedUserSearch"].IsNotNull()
                        && Convert.ToBoolean(args["BackToSharedUserSearch"]) == true)
                    {
                        GetSessionData();
                    }
                    else
                        Session[AppConsts.SHARED_USER_SEARCH_CONTRACT] = null;
                }
                else
                {
                    grdSharedUserSearch.Visible = false;
                    Session[AppConsts.SHARED_USER_SEARCH_CONTRACT] = null;
                }
            }
        }
        #endregion

        #region GRID EVENTS

        /// <summary>
        /// NeedDataSource Event of grdSharedUserSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSharedUserSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                    grdSharedUserSearch.Visible = true;
                    CurrentViewContext.GridCustomPaging.CurrentPageIndex = CurrentViewContext.CurrentPageIndex;
                    CurrentViewContext.GridCustomPaging.PageSize = CurrentViewContext.PageSize;
                    Presenter.PerformSearch();
                    grdSharedUserSearch.DataSource = CurrentViewContext.SharedUserResultContract;
                    CurrentViewContext.GridCustomPaging.VirtualPageCount = CurrentViewContext.VirtualPageCount;
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
        /// ItemDataBound Event of grdSharedUserSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSharedUserSearch_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        /// <summary>
        /// SortCommand Event of grdSharedUserSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSharedUserSearch_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
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

        /// <summary>
        /// ItemCommand Event of grdSharedUserSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSharedUserSearch_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                #region DETAILS SCREEN NAVIGATION
                if (e.CommandName == "ViewDetail")
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String sharedUserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SharedUserID"].ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {"Child", ChildControls.SharedUserSearchDetails},
                                                                    {"SharedUserID", sharedUserID},
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    SetSessionData();
                    Response.Redirect(url, true);
                }

                #endregion

                #region For Sort command

                if (!ViewState["SortExpression"].IsNull())
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
                }

                #endregion
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

        #region BUTTON EVENTS

        /// <summary>
        /// Reset Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            txtUserId.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtUserName.Text = String.Empty;
            Session[AppConsts.CLIENT_SEARCH_SESSION_KEY] = null;
            //if (Presenter.IsDefaultTenant)
            //{
            //    ddlTenantName.ClearCheckedItems();
            //    ddlTenantName.EmptyMessage = "--Select--";
            //}
            grdSharedUserSearch.MasterTableView.SortExpressions.Clear();
            SetResetGrid();
        }

        /// <summary>
        /// Search Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            grdSharedUserSearch.Visible = true;
            SetResetGrid();
        }

        /// <summary>
        /// Cancel Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
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

        #endregion

        #endregion

        #region METHODS

        #region PRIVATE METHODS

        /// <summary>
        /// Method to Reset Grid
        /// </summary>
        private void SetResetGrid()
        {
            grdSharedUserSearch.MasterTableView.FilterExpression = null;
            grdSharedUserSearch.MasterTableView.SortExpressions.Clear();
            grdSharedUserSearch.CurrentPageIndex = 0;
            grdSharedUserSearch.MasterTableView.CurrentPageIndex = 0;
            grdSharedUserSearch.Rebind();
        }

        /// <summary>
        /// Method to Set Data in Session
        /// </summary>
        private void SetSessionData()
        {
            SharedUserSearchContract sharedUserSearchContract = new SharedUserSearchContract();

            sharedUserSearchContract.SharedUserID = CurrentViewContext.SharedUserID;
            sharedUserSearchContract.FirstName = CurrentViewContext.FirstName;
            sharedUserSearchContract.LastName = CurrentViewContext.LastName;
            sharedUserSearchContract.UserName = CurrentViewContext.UserName;
            sharedUserSearchContract.EmailAddress = CurrentViewContext.EmailAddress;
            var serializer = new XmlSerializer(typeof(SharedUserSearchContract));
            var strbuilder = new StringBuilder();

            using (TextWriter writer = new StringWriter(strbuilder))
            {
                serializer.Serialize(writer, sharedUserSearchContract);
            }
            //Session for maintaining control values
            Session[AppConsts.SHARED_USER_SEARCH_CONTRACT] = strbuilder.ToString();
        }

        private void GetSessionData()
        {
            var serializer = new XmlSerializer(typeof(SharedUserSearchContract));
            SharedUserSearchContract sharedUserSearchContract = new SharedUserSearchContract();
            if (Session[AppConsts.SHARED_USER_SEARCH_CONTRACT].IsNotNull())
            {
                TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.SHARED_USER_SEARCH_CONTRACT]));
                sharedUserSearchContract = (SharedUserSearchContract)serializer.Deserialize(reader);

                CurrentViewContext.SharedUserID = sharedUserSearchContract.SharedUserID;
                CurrentViewContext.FirstName = sharedUserSearchContract.FirstName;
                CurrentViewContext.LastName = sharedUserSearchContract.LastName;
                CurrentViewContext.UserName = sharedUserSearchContract.UserName;
                CurrentViewContext.EmailAddress = sharedUserSearchContract.EmailAddress;
                //Reset session
                Session[AppConsts.CLIENT_SEARCH_SESSION_KEY] = null;
            }
        }

        #endregion

        #region PUBLIC METHODS
        #endregion

        #endregion
    }
}