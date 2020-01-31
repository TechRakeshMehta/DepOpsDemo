using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.Reporting.WebForms;
using System.Net;
using System.Security.Principal;
using System.Configuration;

using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Business.RepoManagers;
using INTSOF.UI.Contract.Report;
using INTSOF.Utils;
using System.Reflection;


namespace CoreWeb.Reports.Views
{
    public partial class ReportViewer : BaseUserControl
    {
        SysXMembershipUser _user;


        /// <summary>
        /// set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {

            if (_setPageTitle)
            {
                if (this.Page is BasePage)
                {
                    Entity.Report reportDetial = ReportManager.GetReportByCode(ReportCode);
                    BasePage page = this.Page as BasePage;
                    page.Titles = reportDetial.RP_Name;

                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                IsSharedUserLogin(Page.Request.ServerVariables.Get("server_name"));
                if (ShowSearchParameterPanel)
                    dvSearchPanel.Visible = true;
                else
                    dvSearchPanel.Visible = false;
                //if (ReportViewer1.Controls.)
                //{

                //}
                _user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (_user != null)
                {
                    OrgUserId = _user.OrganizationUserId;
                    TenantID = _user.TenantId.Value;
                }
                if (!Page.IsPostBack)
                {
                    //ReportViewer1.RegisterPostBackControl(fsucCmdBarParameter);
                    Entity.Report reportDetial = ReportManager.GetReportByCode(ReportCode);
                    if (!reportDetial.RP_Description.IsNullOrEmpty())
                    {
                        if (reportDetial.RP_Code == "BSCR")
                        {
                            lblRepDescription.Text = "Disclaimer Note";
                        }
                        lblRepDescription.Visible = true;
                        repDescription.InnerHtml = reportDetial.RP_Description;
                    }
                    ReportViewer1.ProcessingMode = ProcessingMode.Remote;
                    //Start UAT-696 Rajeev jha 22 Aug 2014
                    ReportViewer1.SizeToReportContent = _sizeToReportContent;
                    ReportViewer1.ShowParameterPrompts = _showParameterPanel;
                    //End UAT-696 Rajeev jha 22 Aug 2014
                    ServerReport report = ReportViewer1.ServerReport;
                    report.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerUrl"]);
                    report.ReportPath = reportDetial.RP_Path;

                    report.ReportServerCredentials = new CustomReportServerCredentials();
                    // Set the report parameters for the report
                    ReportParameterInfoCollection parameters = ReportViewer1.ServerReport.GetParameters();

                    List<ReportParameter> repParams = new List<ReportParameter>();

                    if (!FromSavedReportFeature)
                    {
                        ReportParameter tenantID = new ReportParameter();
                        tenantID.Name = "TenantID";
                        if (_user.IsSharedUser && IsSharedUserLoginURL)
                        {
                            Session["BreadCrumb"] = null;
                            var profileSharingInvitations = ProfileSharingManager.GetInvitationsByInviteeOrgUserID(_user.OrganizationUserId);

                            if (profileSharingInvitations.IsNotNull())
                            {
                                var tenantIDs = profileSharingInvitations.Select(x => x.PSI_TenantID).Distinct().ToList();
                                String tenantIds = String.Join(",", tenantIDs.ToArray());
                                tenantID.Values.Add(tenantIds);
                            }
                            else
                            {
                                tenantID.Values.Add(String.Empty);
                            }

                            ReportParameter loggedInUserEmailId = new ReportParameter();
                            loggedInUserEmailId.Name = "LoggedInUserEmailId";
                            loggedInUserEmailId.Values.Add(_user.Email);
                            if (parameters["LoggedInUserEmailId"] != null)
                                repParams.Add(loggedInUserEmailId);
                        }
                        else
                        {
                            tenantID.Values.Add(TenantID.ToString());
                        }
                        ReportParameter userID = new ReportParameter();
                        userID.Name = "UserID";
                        userID.Values.Add(OrgUserId.ToString());

                        if (parameters["TenantID"] != null)
                            repParams.Add(tenantID);
                        if (parameters["UserID"] != null)
                            repParams.Add(userID);
                        //Start UAT-696 Rajeev jha 22 Aug 2014
                        if (dictParameters != null && dictParameters.Count > 0)
                        {
                            foreach (string key in dictParameters.Keys)
                                if (parameters[key] != null)
                                    repParams.Add(new ReportParameter(key, dictParameters[key]));
                        }
                    }
                    else
                    {
                        if (dictParameters != null && dictParameters.Count > 0)
                        {
                            foreach (string key in dictParameters.Keys)
                            {
                                if (parameters[key] != null)
                                {
                                    if (dictParameters[key].Contains(",") && !key.Equals("TenantID"))
                                    {
                                        ReportParameter param = new ReportParameter();
                                        param.Name = key;
                                        param.Values.AddRange(dictParameters[key].Split(',').Select(s => s.Replace("~", ",")).ToArray());  //UAT-3052                                    
                                        repParams.Add(param);
                                    }
                                    else
                                    {
                                        String value = dictParameters[key] == "null" ? null : dictParameters[key].Replace('~', ',');
                                        repParams.Add(new ReportParameter(key, value));
                                    }

                                }
                            }
                        }
                    }


                    //End UAT-696 Rajeev jha 22 Aug 2014
                    if (repParams.Count > 0)
                    {
                        ReportViewer1.ServerReport.SetParameters(repParams.ToArray());
                    }
                    ReportViewer1.SizeToReportContent = true;
                    //List<String> s = repParams.Select(x => x.Values).ToList();
                    //Code to set report name
                    if (ApplicantName.IsNotNull() && ApplicantName != "" && ApplicantName!= "  \"\"")
                    {
                        dvSpace.Visible = false;
                        divDescription.Visible = false;
                        lblError.Text = "A Passport Report cannot be pulled for " + ApplicantName + " due to not having a tracking package.";

                    }
                    if (_setPageTitle)
                    {
                        if (this.Page is BasePage)
                        {
                            BasePage page = this.Page as BasePage;
                            page.SetPageTitle(reportDetial.RP_Name);
                        }
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

        /// <summary>
        ///  UAT-2780: As an applicant, I should only be able to export my passport report in PDF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReportViewer1_Load(object sender, EventArgs e)
        {
            if (_user.IsApplicant && (ReportCode == "PRM01" || ReportCode == "PRDO" || ReportCode == "PRMAI01"))
            {
                DisableUnwantedExportFormat("PDF");
            }
        }

        /// <summary>
        /// Hide Unwanted Export options from report
        /// UAT-2780: As an applicant, I should only be able to export my passport report in PDF
        /// </summary>
        /// <param name="strFormatName"></param>
        public void DisableUnwantedExportFormat(string strFormatName)
        {
            FieldInfo info;
            foreach (RenderingExtension extension in ReportViewer1.ServerReport.ListRenderingExtensions())
            {
                if (extension.Name != strFormatName)
                {
                    info = extension.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                    info.SetValue(extension, false);
                }
            }
        }


        #region Attributes
        public string ReportCode { get; set; }
        public int? TenantID { get; set; }
        public int OrgUserId { get; set; }
        public String ApplicantName { get; set; }
        //Start UAT-696 Rajeev jha 22 Aug 2014
        private Dictionary<string, string> dictParameters;
        private string _parameters;
        private Boolean IsSharedUserLoginURL;

        public string Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {

                _parameters = value;
                dictParameters = new Dictionary<string, string>();
                string[] pairSeparater = new string[] { "$$$" };
                string[] keyValueSeparater = new string[] { "|||" };
                string[] pairs = _parameters.Split(pairSeparater, StringSplitOptions.RemoveEmptyEntries);
                foreach (string pair in pairs)
                {
                    string[] keyValue = pair.Split(keyValueSeparater, StringSplitOptions.None);
                    dictParameters.Add(keyValue[0], keyValue[1]);
                }
            }
        }

        private bool _showParameterPanel = true;
        public bool ShowParameterPanel
        {
            get { return _showParameterPanel; }
            set { _showParameterPanel = value; }
        }

        private bool _sizeToReportContent = false;
        public bool SizeToReportContent
        {
            get { return _sizeToReportContent; }
            set { _sizeToReportContent = value; }
        }

        private bool _setPageTitle = true;
        public bool SetPageTitle
        {
            get { return _setPageTitle; }
            set { _setPageTitle = value; }
        }
        //End UAT-696 Rajeev jha 22 Aug 2014
        private bool _showSearchParameterPanel = false;
        private bool _fromSavedReportFeature = false;
        public bool ShowSearchParameterPanel
        {
            get { return _showSearchParameterPanel; }
            set { _showSearchParameterPanel = value; }
        }

        public Boolean FromSavedReportFeature
        {
            get { return _fromSavedReportFeature; }
            set { _fromSavedReportFeature = value; }
        }

        #endregion

        protected void fsucCmdBarParameter_SaveClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), Guid.NewGuid().ToString(), "RefreshPage();", true);
        }

        protected void btnDoPostBack_Click(object sender, EventArgs e)
        {

            #region UAT-3052
            Int32 adminLoggedID = 0;
            if (!System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"].IsNullOrEmpty())
            {
                adminLoggedID = Convert.ToInt32(System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"]);
            }
            #endregion
            ReportParameterInfoCollection parameters = ReportViewer1.ServerReport.GetParameters();
            FavReportParamContract favReportParamContract = new FavReportParamContract()
            {
                UserID = OrgUserId, //UAT-3052
                CreatedByID = adminLoggedID == 0 ? OrgUserId : adminLoggedID,
                FavParamName = txtName.Text.Trim(),
                FavParamDescription = txtDescription.Text.Trim(),
                ReportCode = ReportCode
            };
            favReportParamContract.ParameterValues = new Dictionary<String, String>();
            foreach (ReportParameterInfo parameter in parameters)
            {
                String values = null;
                if (parameter.Values != null && ((parameter.Values.Count > AppConsts.ONE)
                                                        || (parameter.Values.Count == AppConsts.ONE && parameter.Values[0] != null)))
                {
                    values = String.Join(",", parameter.Values.Select(s => s.Replace(",", "~")).ToArray());//UAT-3052
                }
                favReportParamContract.ParameterValues.Add(parameter.Name, values);
            }
            Boolean isSavedSuccessfully = ReportManager.SaveReportFavouriteParameter(favReportParamContract);
            if (isSavedSuccessfully)
            {
                base.ShowSuccessMessage("Report Parameters saved successfully.");
            }
            else
            {
                base.ShowErrorMessage("Some error occured while saving Report Parameters. Please try again.");
            }
        }

        /// <summary>
        /// Method to check whether user logged-in with Shared User Login URL 
        /// </summary>
        /// <param name="currentUrl"></param>
        private void IsSharedUserLogin(String currentUrl)
        {
            var _sharedUserUrl = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);

            String _sharedUserHost = _sharedUserUrl;
            if (_sharedUserUrl.Contains("http"))
            {
                Uri _url = new Uri(_sharedUserUrl);
                _sharedUserHost = _url.Host;
            }
            if (!_sharedUserUrl.IsNullOrEmpty() && currentUrl.ToLower().Trim().Contains(_sharedUserHost.ToLower().Trim()))
            {
                IsSharedUserLoginURL = true;
                return;
            }
            IsSharedUserLoginURL = false;
            return;
        }
    }

    [Serializable]
    public sealed class CustomReportServerCredentials : IReportServerCredentials
    {
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                // Use the default Windows user.  Credentials will be
                // provided by the NetworkCredentials property.
                return null;
            }
        }

        public ICredentials NetworkCredentials
        {
            get
            {
                // Read the user information from the Web.config file.  
                // By reading the information on demand instead of 
                // storing it, the credentials will not be stored in 
                // session, reducing the vulnerable surface area to the
                // Web.config file, which can be secured with an ACL.

                //username
                string userName = ConfigurationManager.AppSettings["ReportViewerUser"];

                if (string.IsNullOrEmpty(userName))
                    throw new Exception("Missing user name from web.config file");

                // Password
                string password = ConfigurationManager.AppSettings["ReportViewerPassword"];

                if (string.IsNullOrEmpty(password))
                    throw new Exception("Missing password from web.config file");

                // Domain
                string domain = ConfigurationManager.AppSettings["ReportViewerDomain"];

                if (string.IsNullOrEmpty(domain))
                    throw new Exception("Missing domain from web.config file");

                return new NetworkCredential(userName, password, domain);

            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;

            // Not using form credentials
            return false;
        }


    }
}