#region NAMESPACES

#region SYSTEM_DEFINED
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Microsoft.Practices.ObjectBuilder;
#endregion

#region APPLICATION_SPECIFIC
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Telerik.Web.UI;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.SearchUI.Views;
#endregion

#endregion

namespace CoreWeb.SearchUI
{
    public partial class ClientLoginSearch : BaseUserControl, IClientLoginSearchView
    {

        #region VARIABLES

        #region PUBLIC VARIABLES
        #endregion

        #region PRIVATE VARIABLES
        private ClientLoginSearchPresenter _presenter = new ClientLoginSearchPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private List<Int32> _selectedTenantIds;
        private List<int> _selectedAgencyIds;
        #endregion

        #endregion

        #region PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion


        #region PUBLIC PROPERTIES



        public ClientLoginSearchPresenter Presenter
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

        public Int32 TenantID
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;
            }
            set { tenantId = value; }
        }

        public List<int> SelectedTenantIDs
        {
            get
            {
                _selectedTenantIds = new List<int>();
                foreach (RadComboBoxItem item in ddlTenantName.Items)
                {
                    if (item.Checked == true)
                        _selectedTenantIds.Add(Convert.ToInt32(item.Value));
                }
                return _selectedTenantIds;
            }
            set
            {
                _selectedTenantIds = value;
                foreach (RadComboBoxItem item in ddlTenantName.Items)
                {
                    if (_selectedTenantIds.Contains(Convert.ToInt32(item.Value)))
                        item.Checked = true;
                }
            }
        }



        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        public IClientLoginSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public int CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public int OrganizationUserID
        {
            get;
            set;
        }

        public string ClientFirstName
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

        public string ClientLastName
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

        public string ClientUserName
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

        public string EmailAddress
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

        public List<ClientLoginSearchContract> ClientSearchData { get; set; }

        public int CurrentPageIndex
        {
            get
            {
                return grdClientSearchData.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdClientSearchData.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        public int PageSize
        {
            get
            {
                return grdClientSearchData.PageSize;
            }
            set
            {
                grdClientSearchData.PageSize = value;
            }
        }

        public int VirtualPageCount
        {
            get
            {
                return grdClientSearchData.VirtualItemCount;
            }
            set
            {
                grdClientSearchData.VirtualItemCount = value;
                grdClientSearchData.MasterTableView.VirtualItemCount = value;
            }

        }

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
            set
            {
                _gridCustomPaging = value;
                VirtualPageCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        #endregion

        #endregion

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Client Login Search";
                base.SetPageTitle("Client Login Search");
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
        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search Client Users per the criteria entered above";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";

            if (!IsPostBack)
            {
                grdClientSearchData.Visible = false;
                Presenter.OnViewInitialized();
                BindControls();
            }
            Presenter.OnViewLoaded();
        }

        protected void grdClientSearchData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                    GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                    GridCustomPaging.PageSize = PageSize;
                    Presenter.PerformSearch();
                    grdClientSearchData.DataSource = CurrentViewContext.ClientSearchData;
                    GridCustomPaging.VirtualPageCount = VirtualPageCount;
                    //To-do 
                    //SetSessionValues();
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

        protected void grdClientSearchData_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    RadButton ClientView = dataItem["ClientView"].Controls[1] as RadButton;
                    if (Convert.ToBoolean(dataItem.GetDataKeyValue("IsActive").ToString()))
                        ClientView.Visible = true;
                    else
                        ClientView.Visible = false;

                    dataItem["Phone"].Text = Presenter.GetFormattedPhoneNumber(Convert.ToString(dataItem["Phone"].Text));
                    if (dataItem["OrganizationUserId"].Text == AppConsts.ZERO)
                    {
                        dataItem["OrganizationUserId"].Text = String.Empty;
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

        protected void grdClientSearchData_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                #region For Sort command
                if (!ViewState["SortExpression"].IsNull())
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
                }
                #endregion

                if (e.CommandName.Equals("ClientView"))
                {
                    String organizationUserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    String UserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"].ToString();
                    Int32 ClientTenantId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ClientTenantID"].ToString());
                    #region Switch to Client Admin View
                    SwitchToClient(UserID, ClientTenantId, Convert.ToInt32(organizationUserID));
                    #endregion

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

        protected void grdClientSearchData_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    GridCustomPaging.SortExpression = e.SortExpression;
                    ViewState["SortExpression"] = e.SortExpression;
                    GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    GridCustomPaging.SortExpression = String.Empty;
                    ViewState["SortExpression"] = String.Empty;
                    GridCustomPaging.SortDirectionDescending = false;
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

        #region COMMANDBAR EVENTS
        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            txtEmail.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtUserName.Text = String.Empty;
            if (Presenter.IsDefaultTenant)
            {
                ddlTenantName.ClearCheckedItems();
                ddlTenantName.EmptyMessage = "--Select--";

                ViewState["PreviousSelectedTenants"] = null;
            }
            grdClientSearchData.MasterTableView.SortExpressions.Clear();
            SetResetGrid();
        }

        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            grdClientSearchData.Visible = true;
            SetResetGrid();
        }

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


        #region PRIVATE METHODS

        public void BindControls()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();

            //For admins
            if (Presenter.IsDefaultTenant)
            {
                ddlTenantName.Enabled = true;
                ddlTenantName.EmptyMessage = "--Select--";
                CurrentViewContext.SelectedTenantIDs.Clear();
            }
            //for client admins
            else
            {
                ddlTenantName.CheckBoxes = false;
                List<Int32> selectedTenantIDForClient = new List<Int32>();
                selectedTenantIDForClient.Add(CurrentViewContext.TenantID);
                CurrentViewContext.SelectedTenantIDs = selectedTenantIDForClient;

            }

        }

        private void SetResetGrid()
        {
            grdClientSearchData.MasterTableView.FilterExpression = null;
            grdClientSearchData.MasterTableView.SortExpressions.Clear();
            grdClientSearchData.CurrentPageIndex = 0;
            grdClientSearchData.MasterTableView.CurrentPageIndex = 0;
            grdClientSearchData.Rebind();
        }

        /// <summary>
        /// Method used to check command format of ExportFormat Dropdown.
        /// </summary>
        /// <param name="cmbExportFormat"></param>
        /// <returns>true if command selected is matched</returns>
        private Boolean IsExportCommand(WclComboBox cmbExportFormat)
        {
            return cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel");
        }

        /// <summary>
        /// Method to switch to Agency View
        /// </summary>
        private void SwitchToClient(String userID, Int32 clientTenantId, Int32 organisationUserId)
        {
            String switchingTargetURL = Presenter.GetSwitchingTargetUrl(clientTenantId);
            RedirectToTargetSwitchingView(userID, switchingTargetURL, clientTenantId, organisationUserId);
        }

        /// <summary>
        /// Method To create/update WebApplicationData, Redirect to Target applicant View.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="switchingTargetURL"></param>
        private void RedirectToTargetSwitchingView(String clientAdminUserID, String switchingTargetURL, Int32 clientTenantId, Int32 clientAdminOrgUserId)
        {
            Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
            ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
            appInstData.UserID = clientAdminUserID;
            appInstData.TagetInstURL = switchingTargetURL;
            appInstData.TokenCreatedTime = DateTime.Now;
            appInstData.UserTypeSwitchViewCode = UserTypeSwitchView.ClientAdmin.GetStringValue();
            appInstData.AdminOrgUserID = CurrentLoggedInUserID;
            appInstData.TenantID = clientTenantId;
            String key = Guid.NewGuid().ToString();

            Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
            if (applicationData != null)
            {
                applicantData = applicationData;
                applicantData.Add(key, appInstData);
                Presenter.UpdateWebAgencyUserData("ApplicantInstData", applicantData);
            }
            else
            {
                applicantData.Add(key, appInstData);
                Presenter.AddWebAgencyUserData("ApplicantInstData", applicantData);
            }

            //Log out from application then redirect to selected tenant url, append key in querystring.
            // On login page get data from Application Variable.
            //Presenter.DoLogOff(true);

            Presenter.AddImpersonationHistory(clientAdminOrgUserId, CurrentLoggedInUserID);

            //Redirect to login page
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };

            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenClientAdminUserView('" + String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}&DeletePrevUsrState=true", key) + "');", true);
        }

        #endregion

        #region PUBLIC METHODS
        #endregion
    }
}