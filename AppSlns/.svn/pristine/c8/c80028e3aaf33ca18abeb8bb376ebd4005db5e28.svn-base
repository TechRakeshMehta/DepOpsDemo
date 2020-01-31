using CoreWeb.Security.Views;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb
{
    public partial class LocationImages : System.Web.UI.Page
    {
        #region Properties

        #region Public properties
        public LocationImagesPresenter locationImagesPresenterContext
        {
            get
            {
                return new LocationImagesPresenter();
            }
        }
        public FingerPrintApplicantLocationImageContract lstLocationImagesData
        {
            get
            {
                if (ViewState["lstLocationImagesData"].IsNotNull())
                {
                    return (FingerPrintApplicantLocationImageContract)(ViewState["lstLocationImagesData"]);
                }
                return new FingerPrintApplicantLocationImageContract();
            }
            set
            {
                ViewState["lstLocationImagesData"] = value;
            }
        }
        public Int32 LocationId
        {
            get
            {
                if (ViewState["LocationId"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["LocationId"]);
                }
                return 0;
            }
            set
            {
                ViewState["LocationId"] = value;
            }
        }
        public Int32 UserId
        {
            get
            {
                if (ViewState["UserId"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["UserId"]);
                }
                return 0;
            }
            set
            {
                ViewState["UserId"] = value;
            }
        }
        public Int32 TenantId
        {
            get
            {
                if (ViewState["TenantId"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                return 0;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }
        public String LanguageCode
        {
            get
            {
                if (ViewState["LanguageCode"].IsNotNull())
                {
                    return Convert.ToString(ViewState["LanguageCode"]);
                }
                return Languages.ENGLISH.GetStringValue();
            }
            set
            {
                ViewState["LanguageCode"] = value;
            }
        }
        #endregion

        #endregion

        #region Events

        #region PageEvents
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {

                    if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                    {
                        Dictionary<String, String> queryString = new Dictionary<String, String>();
                        if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                        {
                            queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                        }
                        if (queryString.ContainsKey("LocationId"))
                        {
                            LocationId = Convert.ToInt32(queryString["LocationId"]);
                        }
                        if (queryString.ContainsKey("uId"))
                        {
                            UserId = Convert.ToInt32(queryString["uId"]);
                        }
                        if (queryString.ContainsKey("tId"))
                        {
                            TenantId = Convert.ToInt32(queryString["tId"]);
                        }
                    }
                    if (LocationId > AppConsts.NONE)
                    {
                        if (UserId > AppConsts.NONE && TenantId > AppConsts.NONE)
                            LanguageCode = locationImagesPresenterContext.GetUserPreferedLangCode(UserId, TenantId);
                        InitializeCulture();
                        lstLocationImagesData = locationImagesPresenterContext.GetImageDataList(LocationId);
                        lblLocationNameTxt.Text = lstLocationImagesData.LocationName;
                        rptrImgView.DataSource = lstLocationImagesData.imageDataList;
                        rptrImgIndicator.DataSource = lstLocationImagesData.imageDataList;
                        rptrImgView.DataBind();
                        rptrImgIndicator.DataBind();

                        if (lstLocationImagesData.IsNotNull() && lstLocationImagesData.imageDataList.Count > AppConsts.NONE)
                        {

                            divLocationImages.Visible = true;
                            divNoImg.Visible = false;
                        }
                        else
                        {
                            divLocationImages.Visible = false;
                            divNoImg.Visible = true;
                        }
                    }
                    else
                    {
                        divLocationImages.Visible = false;
                        divNoImg.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        #region Multi Language
        protected override void InitializeCulture()
        {
            string languageCode = String.IsNullOrEmpty(LanguageCode) ? Languages.ENGLISH.GetStringValue() : LanguageCode;
            LanguageTranslateUtils.SetLanguageInSession(languageCode);
            LanguageTranslateUtils.LanguageTranslateInit();
            base.InitializeCulture();
        }
        #endregion
        #endregion


    }
}