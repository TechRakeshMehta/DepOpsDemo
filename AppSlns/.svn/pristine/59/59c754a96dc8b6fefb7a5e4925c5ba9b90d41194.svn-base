#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;
#endregion

#region Application Specific
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Xml.Serialization;
using System.IO;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Text;
using System.Threading;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using CoreWeb.IntsofSecurityModel;


#endregion

#endregion


namespace CoreWeb.Search.Views
{
    public partial class ApplicantPortfolioSearchMaster : BaseUserControl, IApplicantPortfolioSearchMasterView
    {
        #region Variables

        private ApplicantPortfolioSearchMasterPresenter _presenter=new ApplicantPortfolioSearchMasterPresenter();
        private String _viewType;
        private Int32 _tenantId = 0;
        #endregion

        #region Properties

        
        public ApplicantPortfolioSearchMasterPresenter Presenter
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

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    //_tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        /// <summary>
        /// To set or get Search Instance Id
        /// </summary>
        public Int32 SearchInstanceId
        {
            get
            {
                if (!ViewState["SearchInstanceId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SearchInstanceId"]);
                }
                return 0;
            }
            set
            {
                ViewState["SearchInstanceId"] = value;
            }
        }

        /// <summary>
        /// To set or get error message
        /// </summary>
        public String ErrorMessage
        {
            get;
            set;
        }

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Applicant Portfolio Search Master";
                base.SetPageTitle("Applicant Portfolio Search Master");

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
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

            if (!this.IsPostBack)
            {
                ucAPFS_Online.SetuserContrOnLoad = true;
                //To check if cancel button is clicked on Edit Profile page
                //and get session values for controls
                GetQueryStringValues();
            }
        }
        protected void rptrSearchResults_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ShowResults")
            {
                HiddenField hdnSearchInstanceId = (HiddenField)e.Item.FindControl("hdnSearchInstanceId");
                Int32 searchInstanceId = Convert.ToInt32(hdnSearchInstanceId.Value);
                ucAPFS_Offline.SearchInstanceId = searchInstanceId;
                Label lblName = (Label)e.Item.FindControl("lblSearchName");
                lblSearchName.Text = lblName.Text;
                HiddenField hdnSearchParam = (HiddenField)e.Item.FindControl("hdnSearchParam");
                String SearchParam = hdnSearchParam.Value;
                ucAPFS_Offline.MasterPageTabIndex = AppConsts.OFFLINE_SEARCH_TABINDEX;
                if (SearchParam.IsNotNull())
                    SetSearchParameters(SearchParam);
                ucAPFS_Offline.RebindDatagrid = true;
            }
        }

        protected void rptrSearchResults_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label label = (Label)e.Item.FindControl("lblStatus");

            if (label.IsNotNull() && !(label.Text.Trim().Equals("Completed") || label.Text.Trim().Equals("Status")))
            {
                Button button = (Button)e.Item.FindControl("btnShowResult");
                button.Enabled = false;
            }
        }

        protected void RadTabStrip_TabClick(object sender, RadTabStripEventArgs e)
        {
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            ResetSearchParameters();
            if (e.Tab.Text.Equals("Offline Search Results"))
            {
                rptrSearchResults.DataSource = Presenter.GetOfflineSearchResultList();
                rptrSearchResults.DataBind();
            }
        }

        #endregion

        

        #region Private Methods

        private void SetSearchParameters(String SearchParam)
        {
            //ucAPFS_Offline.ResetPageControlsOffline = true;
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(SearchParam);
            XmlNode xNode = null;
            xNode = xDoc.SelectSingleNode("root/TenantID");
            ucAPFS_Offline.SelectedTenantId = xNode != null ? Convert.ToInt32(xNode.InnerText) : AppConsts.DEFAULT_SELECTED_TENANTID;

            ucAPFS_Offline.UserGroupListDataSource = Presenter.GetAllUserGroups(ucAPFS_Offline.SelectedTenantId);

            ucAPFS_Offline.SearchTypeDataSource = Presenter.GetSearchModeList();

            xNode = xDoc.SelectSingleNode("root/FirstName");
            ucAPFS_Offline.ApplicantFirstName = xNode != null ? xNode.InnerText : String.Empty;

            xNode = xDoc.SelectSingleNode("root/LastName");
            ucAPFS_Offline.ApplicantLastName = xNode != null ? xNode.InnerText : String.Empty;

            xNode = xDoc.SelectSingleNode("root/OrganizationUserID");
            if (xNode != null)
            {
                ucAPFS_Offline.OrganizationUserID = Convert.ToInt32(xNode.InnerText);
            }
            else
            {
                ucAPFS_Offline.OrganizationUserID = null;
            }
            xNode = xDoc.SelectSingleNode("root/EmailAddress");
            ucAPFS_Offline.EmailAddress = xNode != null ? xNode.InnerText : String.Empty;

            xNode = xDoc.SelectSingleNode("root/SSN");
            ucAPFS_Offline.SSN = xNode != null ? xNode.InnerText : String.Empty;

            xNode = xDoc.SelectSingleNode("root/DOB");
            if (xNode != null)
            {
                ucAPFS_Offline.DateOfBirth = Convert.ToDateTime(xNode.InnerText);
            }
            else
            {
                ucAPFS_Offline.DateOfBirth = null;
            }
            xNode = xDoc.SelectSingleNode("root/NodeID");
            ucAPFS_Offline.DPM_ID = xNode != null ? Convert.ToInt32(xNode.InnerText) : AppConsts.NONE;

            xNode = xDoc.SelectSingleNode("root/CustomFields");
            ucAPFS_Offline.CustomFields = xNode != null ? xNode.InnerText : String.Empty;

            xNode = xDoc.SelectSingleNode("root/MatchUserGroupID");
            ucAPFS_Offline.MatchUserGroupId = xNode != null ? Convert.ToInt32(xNode.InnerText) : AppConsts.NONE;

            xNode = xDoc.SelectSingleNode("root/FilterUserGroupID");
            ucAPFS_Offline.FilterUserGroupId = xNode != null ? Convert.ToInt32(xNode.InnerText) : AppConsts.NONE;

        }

        private void ResetSearchParameters()
        {
            if (RadTabStrip.SelectedIndex != AppConsts.NONE)
            {
                ucAPFS_Offline.TenantDropdownDataSource = Presenter.GetTenants();
                ucAPFS_Offline.SelectedTenantId = AppConsts.DEFAULT_SELECTED_TENANTID;
                ucAPFS_Offline.ResetPageControlsOffline = true;
            }
            else
            {
                ucAPFS_Online.TenantDropdownDataSource = Presenter.GetTenants();
                ucAPFS_Online.SelectedTenantId = Presenter.GetSelectedTenantId(TenantId);
                ucAPFS_Online.ResetPageControlsOffline = true;
            }
        }

        /// <summary>
        /// To get session values for controls
        /// </summary>
        private void GetQueryStringValues()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("CancelClicked") && args["CancelClicked"].IsNotNull())
                {
                    if (args.ContainsKey("SearchInstanceId"))
                    {
                        SearchInstanceId = Convert.ToInt32(args["SearchInstanceId"]);
                    }
                    var serializer = new XmlSerializer(typeof(SearchItemDataContract));
                    SearchItemDataContract searchDataContract = null;
                    if (Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY].IsNotNull())
                    {
                        searchDataContract = new SearchItemDataContract();
                        TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY]));
                        searchDataContract = (SearchItemDataContract)serializer.Deserialize(reader);
                    }
                    if (args.ContainsKey("MasterPageTabIndex") && Convert.ToInt16(args["MasterPageTabIndex"]) != AppConsts.ONLINE_SEARCH_TABINDEX)
                    {
                        Int16 tabIndex = Convert.ToInt16(args["MasterPageTabIndex"]);
                        ucAPFS_Offline.MasterPageTabIndex = tabIndex;
                        RadTabStrip.SelectedIndex = tabIndex;
                        RadMultiPage.SelectedIndex = tabIndex;
                        rptrSearchResults.DataSource = Presenter.GetOfflineSearchResultList();
                        rptrSearchResults.DataBind();
                        ucAPFS_Offline.SearchInstanceId = SearchInstanceId;
                        ucAPFS_Offline.TenantDropdownDataSource = Presenter.GetTenants();
                        if (searchDataContract.IsNotNull())
                            ucAPFS_Offline.GetSessionValues = searchDataContract;
                    }
                    else
                    {
                        ucAPFS_Online.SearchInstanceId = SearchInstanceId;
                        if (searchDataContract.IsNotNull())
                            ucAPFS_Online.GetSessionValues = searchDataContract;
                    }
                }
            }
        }

        #endregion
    }
}

