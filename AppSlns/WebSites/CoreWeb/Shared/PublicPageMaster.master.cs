using System;
using Microsoft.Practices.ObjectBuilder;
using Entity;
using System.Collections.Generic;
using System.Web.Configuration;
using INTSOF.Utils;
using System.Text;
using System.Threading;
using System.Configuration;
using CoreWeb.IntsofSecurityModel;


namespace CoreWeb.Shell.MasterPages
{
    public partial class PublicPageMaster : System.Web.UI.MasterPage, IPublicPageMasterView
    {
        private PublicPageMasterPresenter _presenter = new PublicPageMasterPresenter();
        //UAT-2439
        private Int32 _tenantid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();

                    CustomizePage(sender);
                }
                Presenter.OnViewLoaded();

                //if (IsLocationServiceTenant)
                //{
                //    imgRightLogo.Visible = true;
                //}

            }
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (Exception ex)
            {
                //ErrorMessage = ex.Message;
            }

        }


        public PublicPageMasterPresenter Presenter
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

        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// UAT-2439:
        /// </summary>
        Int32 IPublicPageMasterView.TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set
            {
                _tenantid = value;
            }
        }


        public Boolean IsLocationServiceTenant
        {
            get
            {
                if (ViewState["IsLocationServiceTenant"] == null)
                {
                    ViewState["IsLocationServiceTenant"] = false;
                }
                return Convert.ToBoolean(ViewState["IsLocationServiceTenant"].ToString());
            }
            set
            {
                ViewState["IsLocationServiceTenant"] = value;
            }
        }

        public Boolean IsCBIServiceTenant
        {
            get
            {
                if (ViewState["IsCBIServiceTenant"] == null)
                {
                    ViewState["IsCBIServiceTenant"] = false;
                }
                return Convert.ToBoolean(ViewState["IsCBIServiceTenant"].ToString());
            }
            set
            {
                ViewState["IsCBIServiceTenant"] = value;
            }
        }

        #region Methods

        private void CustomizePage(object sender)
        {
            String baseImagePath = WebConfigurationManager.AppSettings[AppConsts.CLIENT_WEBSITE_IMAGES];
            if (!IsSharedUserLogin(Page.Request.ServerVariables.Get("server_name")))
            {
                WebSiteId = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID));
            }
            else
            {
                WebSiteId = Presenter.GetDefaultTenantWebsite();
            }

            Presenter.IsLocationServiceTenant();

            if (IsLocationServiceTenant)
            {
                lblLocGrpName.Text = Presenter.GetLocationTenantCompanyName();
                CustomAddress.Visible = false;
            }
            else
            {
                IsLocAddress.Visible = false;
            }

            Presenter.GetWebsiteFooter();

            if (WebSiteId > 0)
                GenerateFooter();
            if (String.IsNullOrEmpty(ClientLogoUrl))
            {
                Presenter.GetClientLogo();

                //UAT-2439
                String TenantName = Presenter.GetTenantName();

                String imageURL = String.Empty;
                String rightImageURL = String.Empty;
                if (WebSiteId > 0)
                {
                    imageURL = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?WebsiteId={0}&DocumentType={1}", WebSiteId, "LoginImage");
                }


                //AD:Commenting following line, now image will be shown in the image control
                //divLogin.Style.Add("background-image", String.Format("url('{0}')", imageURL));

                imgLogo.ImageUrl = imageURL;

                Presenter.GetClientRightLogo();
                if (!string.IsNullOrWhiteSpace(ClientRightLogoUrl))
                {
                    rightImageURL = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?WebsiteId={0}&DocumentType={1}", WebSiteId, "RightLogoImage");
                    imgRightLogo.Visible = true;
                    imgRightLogo.ImageUrl = rightImageURL;
                }

                //UAT-2439
                if (!TenantName.IsNullOrEmpty())
                {
                    imgLogo.AlternateText = TenantName + " " + "Logo";
                    imgRightLogo.AlternateText = TenantName + " " + "Right Image";
                }
            }

            if (this.WebSiteId == AppConsts.NONE && IsCentralLogin(Page.Request.ServerVariables.Get("server_name")))
            {
                imgLogo.Visible = false;
                divClientLogo.Visible = false;
            }
            else if (IsSharedUserLogin(Page.Request.ServerVariables.Get("server_name")))
            {
                litFooter.Visible = false;
            }
        }

        private void GenerateFooter()
        {
            StringBuilder sbFooter = new StringBuilder();
            Int32 count = 0;
            foreach (var page in WebsitePages)
            {
                if (page.LinkPosition == Convert.ToInt32(CustomPageLinkPosition.Footer))
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String _viewType = String.Empty;
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    //{ "Child", @"UserControl/CustomPageContent.ascx"},
                                                                    {"PageId",Convert.ToString( page.WebSiteWebPageID)},
                                                                    {"PageTitle",page.LinkText}

                                                                 };
                    //String url = String.Format("Website/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    String url = String.Format("CustomContentPage.aspx?args={0}", queryString.ToEncryptedQueryString());
                    sbFooter = count == 0 ? sbFooter.Append("&nbsp;&nbsp;") : sbFooter.Append("|&nbsp;&nbsp;");
                    sbFooter.Append("<a href=" + url + ">" + page.LinkText + "</a>&nbsp;&nbsp;");
                    count++;
                }
            }
            //

            litFooter.Text = litFooter.Text + "&nbsp;&nbsp;" + Convert.ToString(sbFooter);
        }

        /// <summary>
        /// Change the UI settings, as per whether the screen is opened as a central url.
        /// </summary>
        private Boolean IsCentralLogin(String currentUrl)
        {
            var _centralLoginUrl = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_CENTRAL_LOGIN_URL]);

            String _centralHost = _centralLoginUrl;
            if (_centralLoginUrl.Contains("http"))
            {
                Uri _url = new Uri(_centralLoginUrl);
                _centralHost = _url.Host;
            }
            if (!_centralLoginUrl.IsNullOrEmpty() && currentUrl.ToLower().Trim().Contains(_centralHost.ToLower().Trim()))
                return true;

            return false;
        }

        private Boolean IsSharedUserLogin(String currentUrl)
        {
            var _sharedUserUrl = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);

            String _sharedUserHost = _sharedUserUrl;
            if (_sharedUserUrl.Contains("http"))
            {
                Uri _url = new Uri(_sharedUserUrl);
                _sharedUserHost = _url.Host;
            }
            if (!_sharedUserUrl.IsNullOrEmpty() && currentUrl.ToLower().Trim().Contains(_sharedUserHost.ToLower().Trim()))
                return true;

            return false;
        }

        #endregion

        #region View Properties Implementations

        public Int32 WebSiteId
        {
            get;
            set;
        }

        public List<WebSiteWebPage> WebsitePages
        {
            get;
            set;
        }

        public String FooterContent
        {
            set { litFooter.Text = String.Empty; }
        }


        public String ClientLogoUrl
        {
            get;
            set;
        }

        public String ClientRightLogoUrl
        {
            get;
            set;
        }
        #endregion

        #region public properties
        /// <summary>
        /// property to get the curent year for copyright.
        /// </summary>
        public String CopyRightYear
        {
            get { return DateTime.Now.Year.ToString(); }
        }
        #endregion
    }
}
