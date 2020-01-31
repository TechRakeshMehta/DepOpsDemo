#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Collections;

#endregion

#region Application Specific

using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.CommonControls.Views;
using INTSOF.Utils.Consts;
using INTERSOFT.WEB.UI.WebControls;
//using Microsoft.Practices.CompositeWeb.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Xml.Serialization;
using System.IO;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Text;
using System.Web.UI;
using System.Threading;
using CoreWeb.IntsofSecurityModel;
using INTSOF.UI.Contract.ComplianceOperation;


#endregion

#endregion


namespace CoreWeb.Search.Views
{
    public partial class ApplicantSearch : BaseUserControl, IApplicantSearchView
    {
        #region Variables

        private ApplicantSearchPresenter _presenter = new ApplicantSearchPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private SearchItemDataContract _gridSearchContract = null;

        #endregion

        #region Properties


        public ApplicantSearchPresenter Presenter
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

        public Int32 SelectedTenantId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlTenantName.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlTenantName.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlTenantName.SelectedValue = value.ToString();
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
                //indAdminProgramStudy();
                BindAllInstituteNodeProgram();
            }
        }

        public Int32 SelectedProgramStudyId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlProgram.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlProgram.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlProgram.SelectedValue = value.ToString();
                }
                else
                {
                    ddlProgram.SelectedIndex = value;
                }
            }
        }

        public Int32 TenantId
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

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IApplicantSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        //public List<Entity.AdminProgramStudy> lstAdminProgramStudy
        //{
        //    get;
        //    set;
        //}

        public List<InstitutionNode> lstInstituteNodePrgrams
        {
            get;
            set;
        }

        public String ApplicantFirstName
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

        public String ApplicantLastName
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

        public String EmailAddress
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

        public DateTime? DateOfBirth
        {
            get
            {
                return dpkrDOB.SelectedDate;
            }
            set
            {
                dpkrDOB.SelectedDate = value;
            }
        }

        //public List<Entity.ClientEntity.vwComplianceApplicantSearch> ApplicantSearchData
        //{
        //    get;
        //    set;
        //}

        public List<ApplicantSearchDataContract> ApplicantSearchData
        {
            get;
            set;
        }

        public Int32 ApplicantUserId
        {
            get
            {
                if (!txtUserId.Text.IsNullOrEmpty())
                {
                    return Convert.ToInt32(txtUserId.Text);
                }
                return 0;
            }
            set
            {
                if (value != AppConsts.NONE)
                {
                    txtUserId.Text = value.ToString();
                }
                else
                {
                    txtUserId.Text = String.Empty;
                }
            }
        }
        public String ApplicantSSN
        {
            //get
            //{
            //    return txtSSN.Text;
            //}
            //set
            //{
            //    txtSSN.Text = value;
            //}
            get;
            set;
        }

        Boolean IApplicantSearchView.IsSSNDisabled
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsSSNDisabled"] ?? false);
            }
            set
            {
                ViewState["IsSSNDisabled"] = value;
            }
        }


        #region Custom Paging


        /// <summary>
        /// Current Page Index</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdApplicantSearchData.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdApplicantSearchData.MasterTableView.CurrentPageIndex > 0)
                {
                    grdApplicantSearchData.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        /// <summary>
        /// Page Size</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdApplicantSearchData.PageSize > 100 ? 100 : grdApplicantSearchData.PageSize;
                return grdApplicantSearchData.PageSize;
            }
        }

        /// <summary>
        /// Virtual Page Count</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            set
            {
                grdApplicantSearchData.VirtualItemCount = value;
                grdApplicantSearchData.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
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

        #endregion

        /// <summary>
        /// To set shared class object of search contract
        /// </summary>
        public SearchItemDataContract SetSearchItemDataContract
        {
            set
            {
                var serializer = new XmlSerializer(typeof(SearchItemDataContract));
                var sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, value);
                }
                //Session for maintaning Grid Filter, Paging and Index
                Session[AppConsts.APPLICANT_SEARCH_GRID_SESSION_KEY] = sb.ToString();
            }
        }

        /// <summary>
        /// To get shared class object of search contract
        /// </summary>
        public SearchItemDataContract GetSearchItemDataContract
        {
            get
            {
                if (_gridSearchContract.IsNull())
                {
                    var serializer = new XmlSerializer(typeof(SearchItemDataContract));
                    TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.APPLICANT_SEARCH_GRID_SESSION_KEY]));
                    _gridSearchContract = (SearchItemDataContract)serializer.Deserialize(reader);
                }
                return _gridSearchContract;
            }
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public String SSNPermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["SSNPermissionCode"]);
            }
            set
            {
                ViewState["SSNPermissionCode"] = value;
            }
        }
        public Boolean IsDOBDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDOBDisabled"] ?? false);
            }
            set
            {
                ViewState["IsDOBDisabled"] = value;
            }
        }
        #endregion

        #region UAT-977- Archival Ability
        public List<lkpArchiveState> lstArchiveState
        {
            set
            {
                rbSubscriptionState.DataSource = value.OrderBy(x => x.AS_Code);
                rbSubscriptionState.DataBind();
                rbSubscriptionState.SelectedValue = ArchiveState.Active.GetStringValue();//ArchiveState.All.GetStringValue();
            }
        }

        public List<String> SelectedArchiveStateCode
        {
            get
            {
                if (!rbSubscriptionState.SelectedValue.IsNullOrEmpty())
                {
                    List<String> selectedCodes = new List<String>();
                    if (rbSubscriptionState.SelectedValue.Equals(ArchiveState.All.GetStringValue()))
                    {
                        return null;
                    }
                    else
                    {
                        selectedCodes.Add(rbSubscriptionState.SelectedValue);
                    }
                    return selectedCodes;
                }
                else
                    return null;
            }
            set
            {
                rbSubscriptionState.SelectedValue = value.FirstOrDefault();
            }
        }
        #endregion

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Applicant Search";
                base.SetPageTitle("Applicant Search");
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
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplicantSSN = txtSSN.TextWithPrompt;
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search orders per the criteria entered above";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
            if (!this.IsPostBack)
            {
                //Set MinDate and MaxDate for DOB
                dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
                dpkrDOB.MaxDate = DateTime.Now;

                Presenter.OnViewInitialized();
                BindControls();
                ApplySSNMask();
                //To check if cancel button is clicked on Edit Profile page
                //and get session values for controls
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("CancelClicked") && args["CancelClicked"].IsNotNull())
                    {
                        GetSessionValues();
                    }
                }
                else
                    grdApplicantSearchData.Visible = false;
                Presenter.GetSSNSetting();
            }
            Presenter.OnViewLoaded();
            if (CurrentViewContext.IsSSNDisabled)
            {
                txtSSN.Text = String.Empty;
                divSSN.Visible = false;
                grdApplicantSearchData.MasterTableView.GetColumn("ApplicantSSN").Visible = false;
                grdApplicantSearchData.MasterTableView.GetColumn("_ApplicantSSN").Visible = false;
            }
            else
            {
                divSSN.Visible = true;
                grdApplicantSearchData.MasterTableView.GetColumn("ApplicantSSN").Visible = true;
            }
            HideShowControlsForGranularPermission();
        }

        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            //Presenter.PerformSearch();
            //grdApplicantSearchData.DataSource = CurrentViewContext.ApplicantSearchData;
            //grdApplicantSearchData.DataBind();
            //To set controls values in session
            //SetSessionValues();

            //To reset grid filters 
            grdApplicantSearchData.Visible = true;
            ResetGridFilters();
        }

        /// <summary>
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            Presenter.GetTenants();
            BindControls();
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;
            txtEmail.Text = String.Empty;
            CurrentViewContext.SelectedProgramStudyId = 0;
            ApplicantUserId = 0;
            ApplicantSSN = null;
            txtSSN.Text = String.Empty;
            //rbSubscriptionState.Visible = false;
            if (Presenter.IsDefaultTenant)
            {
                rbSubscriptionState.Visible = false;
            }
            else
            {
                rbSubscriptionState.Visible = true;
            }
            //To reset grid filters 
            ResetGridFilters();
            //Reset session
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;

            //Presenter.PerformSearch();
            //grdApplicantSearchData.DataSource = CurrentViewContext.ApplicantSearchData;
            //grdApplicantSearchData.DataBind();
        }

        /// <summary>
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            //Reset session
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            //queryString = new Dictionary<String, String>
            //                                                     { 

            //                                                        { "Child", @"DashBoard.ascx"},

            //                                                     };
            //Response.Redirect(String.Format("~/Main/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString()));
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        }

        #region Grid Events

        /// <summary>
        /// Sets the list of filters to be displayed in Queue. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdApplicantSearchData.FilterMenu;
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

        /// <summary>
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                    grdApplicantSearchData.Visible = true;
                    GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                    GridCustomPaging.PageSize = PageSize;

                    Presenter.PerformSearch();
                    grdApplicantSearchData.DataSource = CurrentViewContext.ApplicantSearchData;
                    //To set controls values in session
                    SetSessionValues();
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
        /// Grid Item Command event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region DetailScreenNavigation

                if (e.CommandName.Equals("ViewDetail"))
                {
                    Int32 selectedTenantId = 0;

                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        selectedTenantId = SelectedTenantId;
                    }
                    else
                    {
                        selectedTenantId = TenantId;
                    }

                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String organizationUserId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(selectedTenantId) },
                                                                    { "Child", ChildControls.EditProfilePage},
                                                                    { "OrganizationUserId", organizationUserId},
                                                                    {"PageType", "ApplicantSearch"}
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);

                }
                #endregion

                #region For Filter command

                if (!ViewState["SortExpression"].IsNull())
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
                }

                CurrentViewContext.GridCustomPaging.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
                CurrentViewContext.GridCustomPaging.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
                CurrentViewContext.GridCustomPaging.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);

                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    Pair filter = (Pair)e.CommandArgument;

                    Int32 filterIndex = CurrentViewContext.GridCustomPaging.FilterColumns.IndexOf(filter.Second.ToString());
                    if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                    {
                        String filteringType = (e.Item as GridFilteringItem)[filter.Second.ToString()].Controls[0].GetType().Name;
                        String filterValue = grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                        if (filterIndex != -1)
                        {
                            CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = filter.First.ToString();
                            if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Decimal")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDecimal(filterValue);
                                }

                                //If filter Value Is Null Or Empty then set filter value to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = AppConsts.NONE;
                                }
                            }
                            else if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Int32")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToInt32(filterValue);
                                }

                                //If filter Value Is Null Or Empty then set filter value to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = AppConsts.NONE;
                                }
                            }
                            else if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.DateTime")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    try
                                    {
                                        //try to convert any value to date
                                        CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDateTime(filterValue);
                                    }
                                    catch
                                    {
                                        //date filter value could not be converted, set filter value to any default date
                                        CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDateTime("01/01/1901");
                                        //return;
                                    }
                                }

                                //To set IsNull filter to other Date format filter and set filter value to any default date in case of Null date
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDateTime("01/01/1901");
                                }
                            }
                            else
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = filterValue;
                            }
                        }
                        else
                        {
                            CurrentViewContext.GridCustomPaging.FilterColumns.Add(filter.Second.ToString());
                            CurrentViewContext.GridCustomPaging.FilterOperators.Add(filter.First.ToString());
                            if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Decimal")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDecimal(filterValue));
                                }

                                //If filter Value Is Null Or Empty then set filter value to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    Int32 index = CurrentViewContext.GridCustomPaging.FilterOperators.IndexOf("IsNull");
                                    CurrentViewContext.GridCustomPaging.FilterOperators[index] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(AppConsts.NONE);
                                }
                            }
                            else if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Int32")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToInt32(filterValue));
                                }

                                //If filter Value Is Null Or Empty then set filter value to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    Int32 index = CurrentViewContext.GridCustomPaging.FilterOperators.IndexOf("IsNull");
                                    CurrentViewContext.GridCustomPaging.FilterOperators[index] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(AppConsts.NONE);
                                }
                            }
                            else if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.DateTime")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    try
                                    {
                                        //try to convert any value to date
                                        CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDateTime(filterValue));
                                    }
                                    catch
                                    {
                                        //date filter value could not be converted, set filter value to any default date
                                        CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDateTime("01/01/1901"));
                                        //return;
                                    }
                                }

                                //To set IsNull filter to other Date format filter and set filter value to any default date in case of Null date
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    Int32 index = CurrentViewContext.GridCustomPaging.FilterOperators.IndexOf("IsNull");
                                    CurrentViewContext.GridCustomPaging.FilterOperators[index] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDateTime("01/01/1901"));
                                }
                            }
                            else
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues.Add(filterValue);
                            }
                        }
                    }
                    else
                    {
                        CurrentViewContext.GridCustomPaging.FilterOperators.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterValues.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterColumns.RemoveAt(filterIndex);
                    }

                    ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns;
                    ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators;
                    ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues;
                }

                #endregion
                // Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdApplicantSearchData);

                }

                #region EXPORT FUNCTIONALITY
                //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                // and displayed the masked column on Export instead of actual column.
                if (e.CommandName.IsNullOrEmpty())
                {
                    WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                    if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                    {
                        grdApplicantSearchData.MasterTableView.GetColumn("_ApplicantSSN").Display = true;
                    }
                }
                else if (e.CommandName == "Cancel")
                {
                    grdApplicantSearchData.MasterTableView.GetColumn("_ApplicantSSN").Display = false;
                }
                #endregion

            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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
        /// Grid Item Databound Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;

                //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                // and displayed the masked column on Export instead of actual column.
                dataItem["_ApplicantSSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["_ApplicantSSN"].Text));

                // UAT-806 Creation of granular permissions for Client Admin users
                if (!CurrentViewContext.IsSSNDisabled && CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                {
                    dataItem["ApplicantSSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["ApplicantSSN"].Text));
                }
                else if (!CurrentViewContext.IsSSNDisabled)
                {
                    dataItem["ApplicantSSN"].Text = Presenter.GetFormattedSSN(Convert.ToString(dataItem["ApplicantSSN"].Text));
                }

            }
        }

        /// <summary>
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_SortCommand(object sender, GridSortCommandEventArgs e)
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

        /// <summary>
        /// To bind Admin Program Study dropdown when Tenant Name changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindAllInstituteNodeProgram();
            if (ddlTenantName.SelectedIndex > 0)
            {
                rbSubscriptionState.Visible = true;
                Presenter.GetArchiveStateList();
            }
            else
            {
                rbSubscriptionState.Visible = false;
            }
            Presenter.GetSSNSetting();
            if (CurrentViewContext.IsSSNDisabled)
            {
                txtSSN.Text = String.Empty;
                ApplicantSSN = null;
                divSSN.Visible = false;
                grdApplicantSearchData.MasterTableView.GetColumn("ApplicantSSN").Visible = false;
            }
            else
            {
                divSSN.Visible = true;
                grdApplicantSearchData.MasterTableView.GetColumn("ApplicantSSN").Visible = true;
                HideShowControlsForGranularPermission();
            }
            if (ddlTenantName.SelectedIndex <= 0)
            {
                ResetGridFilters();
            }
        }

        /// <summary>
        /// Tenant Name DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        /// <summary>
        /// Program DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProgram_DataBound(object sender, EventArgs e)
        {
            ddlProgram.Items.Insert(0, new DropDownListItem("--Select--"));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To bind controls
        /// </summary>
        private void BindControls()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();

            if (Presenter.IsDefaultTenant)
            {
                //divTenant.Visible = true;
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantId = 0;
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
                rbSubscriptionState.Visible = true;
                Presenter.GetArchiveStateList();
            }

            //BindAdminProgramStudy();
        }

        /// <summary>
        /// To bind program dropdown
        /// </summary>
        //private void BindAdminProgramStudy()
        //{
        //    Presenter.GetAdminProgramStudy();
        //    ddlProgram.DataSource = CurrentViewContext.lstAdminProgramStudy;
        //    ddlProgram.DataBind();
        //    CurrentViewContext.SelectedProgramStudyId = 0;
        //}

        /// <summary>
        /// To bind program dropdown
        /// </summary>
        private void BindAllInstituteNodeProgram()
        {
            Presenter.GetAllInstituteNodePrograms();
            ddlProgram.DataSource = CurrentViewContext.lstInstituteNodePrgrams;
            ddlProgram.DataBind();
            CurrentViewContext.SelectedProgramStudyId = 0;
        }

        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues()
        {
            SearchItemDataContract searchDataContract = new SearchItemDataContract();

            searchDataContract.IsBackToSearch = true;

            searchDataContract.ClientID = SelectedTenantId;
            searchDataContract.ApplicantFirstName = ApplicantFirstName;
            searchDataContract.ApplicantLastName = ApplicantLastName;
            searchDataContract.EmailAddress = EmailAddress;
            searchDataContract.DateOfBirth = DateOfBirth;
            searchDataContract.ProgramID = SelectedProgramStudyId;
            searchDataContract.OrganizationUserId = ApplicantUserId;
            searchDataContract.ApplicantSSN = ApplicantSSN;
            searchDataContract.LstArchiveState = CurrentViewContext.SelectedArchiveStateCode;
            var serializer = new XmlSerializer(typeof(SearchItemDataContract));
            var strbuilder = new StringBuilder();

            using (TextWriter writer = new StringWriter(strbuilder))
            {
                serializer.Serialize(writer, searchDataContract);
            }
            //Session for maintaining control values
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = strbuilder.ToString();
        }

        /// <summary>
        /// To get session values for controls
        /// </summary>
        private void GetSessionValues()
        {
            var serializer = new XmlSerializer(typeof(SearchItemDataContract));
            SearchItemDataContract searchDataContract = new SearchItemDataContract();
            if (Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY].IsNotNull())
            {
                TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY]));
                searchDataContract = (SearchItemDataContract)serializer.Deserialize(reader);

                SelectedTenantId = searchDataContract.ClientID;
                ApplicantFirstName = searchDataContract.ApplicantFirstName;
                ApplicantLastName = searchDataContract.ApplicantLastName;
                EmailAddress = searchDataContract.EmailAddress;
                DateOfBirth = searchDataContract.DateOfBirth;
                SelectedProgramStudyId = searchDataContract.ProgramID;
                ApplicantUserId = searchDataContract.OrganizationUserId.Value;
                //ApplicantSSN = searchDataContract.ApplicantSSN;
                if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
                {
                    if (!searchDataContract.ApplicantSSN.IsNullOrEmpty())
                    {
                        CurrentViewContext.ApplicantSSN = searchDataContract.ApplicantSSN.Substring(searchDataContract.ApplicantSSN.Length - AppConsts.FOUR);
                        txtSSN.Text = CurrentViewContext.ApplicantSSN;
                    }
                }
                else
                {
                    CurrentViewContext.ApplicantSSN = searchDataContract.ApplicantSSN;
                    txtSSN.Text = CurrentViewContext.ApplicantSSN;
                }
                if (searchDataContract.ClientID != 0)
                {
                    Presenter.GetArchiveStateList();
                }
                if (searchDataContract.LstArchiveState.IsNotNull() && searchDataContract.LstArchiveState.Count > 0)
                {
                    rbSubscriptionState.SelectedValue = searchDataContract.LstArchiveState.FirstOrDefault();
                }
                else if (searchDataContract.LstArchiveState.IsNotNull() && searchDataContract.LstArchiveState.Count == 0)
                {
                    rbSubscriptionState.SelectedValue = ArchiveState.All.GetStringValue();
                }
                //Reset session
                Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdApplicantSearchData.MasterTableView.FilterExpression = null;
            grdApplicantSearchData.MasterTableView.SortExpressions.Clear();
            grdApplicantSearchData.CurrentPageIndex = 0;
            grdApplicantSearchData.MasterTableView.CurrentPageIndex = 0;
            foreach (GridColumn column in grdApplicantSearchData.MasterTableView.RenderColumns)
            {
                if (column.ColumnType == "GridBoundColumn")
                {
                    GridBoundColumn boundColumn = (GridBoundColumn)column;
                    String columnName = boundColumn.UniqueName.ToString();
                    grdApplicantSearchData.MasterTableView.GetColumnSafe(columnName).CurrentFilterFunction = GridKnownFunction.NoFilter;
                    grdApplicantSearchData.MasterTableView.GetColumnSafe(columnName).CurrentFilterValue = String.Empty;
                }
            }
            CurrentViewContext.GridCustomPaging.FilterColumns = new List<String>();
            CurrentViewContext.GridCustomPaging.FilterOperators = new List<String>();
            CurrentViewContext.GridCustomPaging.FilterValues = new ArrayList();

            ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns;
            ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators;
            ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues;
            Presenter.GetSSNSetting();
            if (CurrentViewContext.IsSSNDisabled)
            {
                txtSSN.Text = String.Empty;
                ApplicantSSN = null;
                divSSN.Visible = false;
                grdApplicantSearchData.MasterTableView.GetColumn("ApplicantSSN").Visible = false;
            }
            else
            {
                divSSN.Visible = true;
                grdApplicantSearchData.MasterTableView.GetColumn("ApplicantSSN").Visible = true;
                HideShowControlsForGranularPermission();
            }
            grdApplicantSearchData.Rebind();
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.IsDOBDisable)
            {
                divDOB.Visible = false;
                grdApplicantSearchData.MasterTableView.GetColumn("DateOfBirth").Visible = false;
            }
            if (!CurrentViewContext.IsSSNDisabled && CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                grdApplicantSearchData.MasterTableView.GetColumn("ApplicantSSN").Visible = false;
                grdApplicantSearchData.MasterTableView.GetColumn("_ApplicantSSN").Visible = false;
            }
            //else if (!this.IsPostBack && CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            //{
            //    //txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####"
            //    txtSSN.Mask = AppConsts.SSN_MASK_FORMAT_ALPHANUMERIC; 
            //}
        }
        #endregion
        private void ApplySSNMask()
        {
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            {
                //txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####"
                txtSSN.Mask = AppConsts.SSN_MASK_FORMAT_ALPHANUMERIC;
            }
        }
        #endregion
    }
}

