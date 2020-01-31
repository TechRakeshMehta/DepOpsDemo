using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class ExternalLogin : System.Web.UI.Page, IExternalLoginView
    {

        #region Properties

        #region Private Properties
        private ExternalLoginPresenter _presenter = new ExternalLoginPresenter();
        private const string _externalViewDocumentPath = "~/ExternalViewDocument.aspx";

        #endregion

        #region Public Properties

        public ExternalLoginPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this; ;
            }
        }

        public IExternalLoginView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IExternalLoginView.FirstName
        {
            get
            {
                return Convert.ToString(ViewState["FirstName"]);
            }
            set
            {
                ViewState["FirstName"] = value;
            }
        }

        String IExternalLoginView.LastName
        {
            get
            {
                return Convert.ToString(ViewState["LastName"]);
            }
            set
            {
                ViewState["LastName"] = value;
            }
        }

        DateTime? IExternalLoginView.DOB
        {
            get
            {
                return ViewState["DOB"].IsNotNull() ? Convert.ToDateTime(ViewState["DOB"]) : (DateTime?)null;
            }
            set
            {
                ViewState["DOB"] = value;
            }
        }
        String IExternalLoginView.SSN
        {
            get
            {
                return Convert.ToString(ViewState["SSN"]);
            }
            set
            {
                ViewState["SSN"] = value;
            }
        }

        String IExternalLoginView.UserName
        {
            get
            {
                return Convert.ToString(ViewState["UserName"]);
            }
            set
            {
                ViewState["UserName"] = value;
            }
        }

        String IExternalLoginView.Token
        {
            get;
            set;
        }

        String IExternalLoginView.SchoolName
        {
            get;
            set;
        }

        String IExternalLoginView.ExternalID
        {
            get
            {
                return Convert.ToString(ViewState["ExternalID"]);
            }
            set
            {
                ViewState["ExternalID"] = value;
            }
        }

        String IExternalLoginView.mappingCode
        {        
           get
            {
                return Convert.ToString(ViewState["MappingCode"]);
            }
            set
            {
                ViewState["MappingCode"] = value;
            }
        }

        String IExternalLoginView.Email1
        {
            get
            {
                return Convert.ToString(ViewState["Email1"]);
            }
            set
            {
                ViewState["Email1"] = value;
            }
        }

        String IExternalLoginView.Email2
        {
            get
            {
                return Convert.ToString(ViewState["Email2"]);
            }
            set
            {
                ViewState["Email2"] = value;
            }
        }

        String IExternalLoginView.Phone
        {
            get
            {
                return Convert.ToString(ViewState["Phone"]);
            }
            set
            {
                ViewState["Phone"] = value;
            }
        }

        Int32 IExternalLoginView.ExternalUserTenantId
        {
            get;
            set;
        }

        Int32 IExternalLoginView.TenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IExternalLoginView.IntegrationClientId
        {
            get
            {
                return Convert.ToInt32(ViewState["IntegrationClientId"]);
            }
            set
            {
                ViewState["IntegrationClientId"] = value;
            }
        }

        //public List<INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract> matchingUserList
        //{
        //    get;
        //    set;
        //}

        String IExternalLoginView.WebsiteLoginUrl
        {
            get;
            set;
        }
        List<INTSOF.UI.Contract.SysXSecurityModel.ExternalDataFromTokenDataContract> IExternalLoginView.ExternalDataList
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region Get External Userdata
                  if (!Request.QueryString["TokenId"].IsNull())
                  {
                      CurrentViewContext.Token = Request.QueryString["TokenId"];
                  }
                //Load the student data based upon the token received from the API
                Presenter.GetDataFromSecurityToken();

                if (CurrentViewContext.ExternalDataList.IsNotNull() && CurrentViewContext.ExternalDataList.Count > AppConsts.NONE)
                {
                    foreach (INTSOF.UI.Contract.SysXSecurityModel.ExternalDataFromTokenDataContract item in CurrentViewContext.ExternalDataList)
                    {
                        if (item.IsNotNull())
                        {
                            switch (item.Name.ToLower())
                            {
                                case "school":
                                    CurrentViewContext.SchoolName = item.Value;
                                    break;
                                case "externalid":
                                    CurrentViewContext.ExternalID = item.Value;
                                    break;
                                case "tenantid":
                                    CurrentViewContext.TenantId = Convert.ToInt32(item.Value);
                                    break;
                                case "firstname":
                                    CurrentViewContext.FirstName = item.Value;
                                    break;
                                case "lastname":
                                    CurrentViewContext.LastName = item.Value;
                                    break;
                                case "dob":
                                    CheckValidDate(item.Value);
                                    break;
                                case "ssn":
                                    CurrentViewContext.SSN = item.Value;
                                    break;
                                case "email1":
                                    CurrentViewContext.Email1 = item.Value;
                                    break;
                                case "email2":
                                    CurrentViewContext.Email2 = item.Value;
                                    break;
                                case "phone":
                                    CurrentViewContext.Phone = item.Value;
                                    break;
                                case "username":
                                    CurrentViewContext.UserName = item.Value;
                                    break;
                                case "mappingcode":
                                    CurrentViewContext.mappingCode = item.Value;
                                    break;
                            }
                        }
                    }
                }
                #endregion

                if (CurrentViewContext.ExternalDataList.IsNotNull() && CurrentViewContext.ExternalDataList.Count > AppConsts.NONE)
                {
                    #region Check the required fields
                    Boolean RequiredFieldCheckResult = true;
                    List<String> controls = new List<String>();
                    if (String.IsNullOrEmpty(CurrentViewContext.Token))
                    {
                        controls.Add("Token is required");
                        RequiredFieldCheckResult = false;
                    }
                    if (String.IsNullOrEmpty(CurrentViewContext.ExternalID))
                    {
                        controls.Add("External ID is required");
                        RequiredFieldCheckResult = false;
                    }
                    if (String.IsNullOrEmpty(CurrentViewContext.SchoolName))
                    {
                        controls.Add("School name is required");
                        RequiredFieldCheckResult = false;
                    }


                    blValidationSummary.DataSource = controls;
                    blValidationSummary.DataBind();
                    #endregion

                    if (RequiredFieldCheckResult)
                    {
                        //Validate Token
                        if (ValidateToken(CurrentViewContext.Token))
                        {
                            string tokenTypeCode = string.Empty;
                            Boolean isTokenTypeDocList = false;

                            if (CurrentViewContext.SchoolName.IsNotNull())
                            {
                                //get tenant id based on school name if not existed then check if external id not exist  then Show Error message "School is not configured" 
                                Presenter.GetTenantIdBySchoolName();
                                //if existed then get tenant id and proceed to login url
                            }

                            //Check if external ID exist in api.IntegrationClientOrganizationUserMap in security db
                            Presenter.ExternalUserTenantId();

                            //Getting token type & Creating session if token type is Doc List Token to redirect to external document page
                            if (!CurrentViewContext.ExternalDataList.IsNullOrEmpty() &&
                                    CurrentViewContext.ExternalDataList.Count > 0)
                            {
                                string documentList = string.Empty;

                                var docObj = CurrentViewContext.ExternalDataList.Find(cond => cond.Name.ToLower() == "docs");
                                var tokenTypeObj = CurrentViewContext.ExternalDataList.Find(cond => cond.Name.ToLower() == "tokentypecode");

                                if (!docObj.IsNullOrEmpty())
                                    documentList = docObj.Value;

                                if (!tokenTypeObj.IsNullOrEmpty())
                                    tokenTypeCode = tokenTypeObj.Value;

                                if (string.Compare(tokenTypeCode.ToUpper(), SecurityTokenType.DocListToken.GetStringValue()) == 0)
                                {
                                    Dictionary<int, string> dicDataToViewDocs = new Dictionary<int, string>();
                                    dicDataToViewDocs.Add(CurrentViewContext.TenantId, documentList);
                                    Session["DataToViewDocs"] = dicDataToViewDocs;
                                    isTokenTypeDocList = true;
                                }
                            }
                         
                            if (CurrentViewContext.ExternalUserTenantId > AppConsts.NONE)
                            {
                                //Check external tenantID match with school name
                                if (!string.IsNullOrEmpty(CurrentViewContext.SchoolName))
                                {
                                    if (CurrentViewContext.ExternalUserTenantId == CurrentViewContext.TenantId)
                                    {
                                        if (!isTokenTypeDocList)
                                        {
                                            if (!String.IsNullOrEmpty(CurrentViewContext.UserName))
                                            {
                                                String externalUserName = Presenter.GetUserNameByExternald();

                                                //Redirect to login page for Auto-Login                                       
                                                SysXMembershipUser user = System.Web.Security.Membership.GetUser(System.Text.RegularExpressions.Regex.Replace(externalUserName, @"(?<=^\s*)\s|\s(?=\s*$)", INTSOF.Utils.Consts.SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;


                                                Dictionary<String, INTSOF.UI.Contract.SysXSecurityModel.ApplicantInsituteDataContract> applicantData = new Dictionary<String, INTSOF.UI.Contract.SysXSecurityModel.ApplicantInsituteDataContract>();
                                                INTSOF.UI.Contract.SysXSecurityModel.ApplicantInsituteDataContract appInstData = new INTSOF.UI.Contract.SysXSecurityModel.ApplicantInsituteDataContract();
                                                appInstData.UserID = Convert.ToString(user.UserId);
                                                appInstData.TagetInstURL = Presenter.GetInstitutionUrl();
                                                appInstData.TokenCreatedTime = DateTime.Now;
                                                appInstData.TenantID = CurrentViewContext.TenantId;
                                                appInstData.IsIncorrectLogin = user.IncorrectLoginUrlUsed;
                                                String key = Guid.NewGuid().ToString();

                                                Dictionary<String, INTSOF.UI.Contract.SysXSecurityModel.ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
                                                if (applicationData != null)
                                                {
                                                    applicantData = applicationData;
                                                    applicantData.Add(key, appInstData);
                                                    Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
                                                }
                                                else
                                                {
                                                    applicantData.Add(key, appInstData);
                                                    Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
                                                }

                                                //Log out from application then redirect to selected tenant url, append key in querystring.
                                                // On login page get data from Application Variable.

                                                // Presenter.DoLogOff(!user.IsNull(), user.UserLoginHistoryID);
                                                //Redirect to login page
                                                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };
                                                Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
                                                // Check if the user is assigned to any of the LOB.
                                                //Entity.lkpSysXBlock sysxblock = Presenter.GetDefaultLineOfBusinessByUserName(CurrentViewContext.UserName);

                                                //if (!sysxblock.IsNull())
                                                //{
                                                //    CoreWeb.Shell.SysXWebSiteUtils.SessionService.SetSysXBlockId(sysxblock.SysXBlockId);
                                                //    CoreWeb.Shell.SysXWebSiteUtils.SessionService.SetSysXBlockName(sysxblock.Name);
                                                //CoreWeb.Shell.SysXWebSiteUtils.SessionService.SetSysXMembershipUser(user);
                                                //// Presenter.ResetPasswordAttempCount(CurrentViewContext.UserName);
                                                //System.Web.Security.FormsAuthentication.RedirectFromLoginPage(externalUserName, false);
                                                //}

                                            }
                                            else
                                            {
                                                Presenter.GetInstitutionUrlByExternalUserTenantID();
                                                Response.Redirect(CurrentViewContext.WebsiteLoginUrl);
                                            }
                                        }
                                        else
                                        {
                                            Response.Redirect(_externalViewDocumentPath);
                                        }
                                    }
                                    else
                                    {
                                        lblError.Text = "School is not configured";
                                    }
                                }
                                else
                                {
                                    if (!isTokenTypeDocList)
                                    {
                                        //If school name is empty then get tenant ID from externalID then get data
                                        Presenter.GetInstitutionUrlByExternalUserTenantID();
                                        Response.Redirect(CurrentViewContext.WebsiteLoginUrl);
                                    }
                                    else
                                    {
                                        Response.Redirect(_externalViewDocumentPath);
                                    }
                                }

                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(CurrentViewContext.SchoolName)
                                       && CurrentViewContext.TenantId > AppConsts.NONE)
                                {
                                    if (!isTokenTypeDocList)
                                    {
                                        //Call the SP will bind all organizationuserids which are matching
                                        List<INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract> matchingCount = new List<INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract>();
                                        if (!String.IsNullOrEmpty(CurrentViewContext.mappingCode) && CurrentViewContext.mappingCode == AppConsts.CORE_ACCOUNT_LINKING_MAPPING_GROUP_CODE)
                                        {
                                            matchingCount = new List<INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract>();
                                            matchingCount = Presenter.GetMatchingOrganisationUserListForCoreLinking();
                                        }
                                        else
                                        {
                                            matchingCount = Presenter.GetMatchingOrganizationUserDetails();
                                        }
                                        if (matchingCount.Count > AppConsts.NONE)
                                        {
                                            Presenter.GetInstitutionUrlBySchoolNameTenantID();
                                            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "IsExternalUserRegistration", true.ToString()},
                                                                    { "FirstName", CurrentViewContext.FirstName},
                                                                    { "LastName", CurrentViewContext.LastName},
                                                                    { "DOB", CurrentViewContext.DOB.IsNotNull() ? CurrentViewContext.DOB.Value.ToShortDateString() : String.Empty},
                                                                    { "SSN", CurrentViewContext.SSN},
                                                                    { "UserName",  CurrentViewContext.UserName.IsNotNull()? CurrentViewContext.UserName:String.Empty},
                                                                    { "PrimaryEmail", CurrentViewContext.Email1},
                                                                    { "SecondaryEmail", CurrentViewContext.Email2},
                                                                    { "PrimaryPhone", CurrentViewContext.Phone},
                                                                    { "ExternalID", CurrentViewContext.ExternalID.IsNotNull() ? CurrentViewContext.ExternalID.ToString() : string.Empty},
                                                                    { "IntegrationClientId", CurrentViewContext.IntegrationClientId.ToString()},
                                                                    { "MappingCode", CurrentViewContext.mappingCode.ToString()}
                                                                 };

                                            string url = String.Format("/UserRegistration.aspx?args={0}", queryString.ToEncryptedQueryString());
                                            url = CurrentViewContext.WebsiteLoginUrl + url;
                                            Response.Redirect(url);
                                        }
                                        else
                                        {
                                            displaySection.Visible = true;
                                        }
                                    }
                                    else
                                    {
                                        Response.Redirect(_externalViewDocumentPath);
                                    }
                                }

                                else
                                {
                                    lblError.Text = "School is not configured";
                                }
                            }
                        }
                        else
                        {
                            //Show Error message 
                            lblError.Text = "Invalid token";
                        }
                    }
                }
                else
                {
                    //Show Error message 
                    lblError.Text = "Invalid token";
                }
            }
        }
        #endregion

        #region Methods
        #region Private Methods
        private void CheckValidDate(String inputDate)
        {
            try
            {
                CurrentViewContext.DOB = Convert.ToDateTime(inputDate);
            }
            catch (Exception)
            {
                CurrentViewContext.DOB = null;
            }
        }

        private Boolean ValidateToken(String token)
        {
            try
            {
                if (token.IsNotNull())
                    return Presenter.ValidateToken();
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #endregion

        protected void cbConfirmation_SubmitClick(object sender, EventArgs e)
        {
            Presenter.GetInstitutionUrlBySchoolNameTenantID();

            Dictionary<String, String> queryString = new Dictionary<String, String>
            {
                                                                    { "ExternalID", CurrentViewContext.ExternalID.IsNotNull() ? CurrentViewContext.ExternalID.ToString() : string.Empty},
                                                                    { "IntegrationClientId", CurrentViewContext.IntegrationClientId.ToString()},
                                                                    { "IslinkingExternalUser", true.ToString()},
                                                                    { "IsRequestExternalPage", true.ToString()},
                                                                    {"ExternalTenantId",CurrentViewContext.TenantId.IsNotNull()?CurrentViewContext.TenantId.ToString():"0"}

                                                                 };
            string url = String.Format("/Login.aspx?args={0}", queryString.ToEncryptedQueryString());

            url = CurrentViewContext.WebsiteLoginUrl + url;
            Response.Redirect(url);
        }

        protected void cbConfirmation_CancelClick(object sender, EventArgs e)
        {
            Presenter.GetInstitutionUrlBySchoolNameTenantID();
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                     {
                                        { "IsExternalUserRegistration", true.ToString()},
                                        { "FirstName", CurrentViewContext.FirstName},
                                        { "LastName", CurrentViewContext.LastName},
                                        { "DOB", CurrentViewContext.DOB.IsNotNull() ? CurrentViewContext.DOB.Value.ToShortDateString() : String.Empty},
                                        { "SSN", CurrentViewContext.SSN},
                                        { "UserName",  CurrentViewContext.UserName.IsNotNull()? CurrentViewContext.UserName:String.Empty},
                                        { "PrimaryEmail", CurrentViewContext.Email1},
                                        { "SecondaryEmail", CurrentViewContext.Email2},
                                        { "PrimaryPhone", CurrentViewContext.Phone},
                                        { "ExternalID", CurrentViewContext.ExternalID.IsNotNull() ? CurrentViewContext.ExternalID.ToString() : string.Empty},
                                        { "IntegrationClientId", CurrentViewContext.IntegrationClientId.ToString()},
                                        { "MappingCode", CurrentViewContext.mappingCode.ToString()}
                                     };

            string url = String.Format("/UserRegistration.aspx?args={0}", queryString.ToEncryptedQueryString());
            url = CurrentViewContext.WebsiteLoginUrl + url;
            Response.Redirect(url);
        }
    }
}