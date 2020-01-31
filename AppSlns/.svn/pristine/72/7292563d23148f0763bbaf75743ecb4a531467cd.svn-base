using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using CoreWeb.ApplicantRotationRequirement.Views;
using CoreWeb.Shell;
using INTSOF.Utils;
using System.Text.RegularExpressions;
using System.Configuration;


namespace CoreWeb.ApplicantRotationRequirement.Pages
{
    public partial class SharedUserViewVideoPopup : BaseWebPage, ISharedUserViewVideoPopupView
    {

        #region Variables

        #region Private Variables
        private SharedUserViewVideoPopupPresenter _presenter = new SharedUserViewVideoPopupPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        RequirementPackageContract ISharedUserViewVideoPopupView.RequirementPackageContractSessionData
        {
            get
            {
                RequirementPackageContract requirementPackageContract = (RequirementPackageContract)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.REQUIREMENT_PACKAGE_SESSION_CONTRACT);
                if (requirementPackageContract.IsNullOrEmpty())
                {
                    requirementPackageContract = new RequirementPackageContract();
                }
                return requirementPackageContract;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.REQUIREMENT_PACKAGE_SESSION_CONTRACT, value);
            }
        }

        String ISharedUserViewVideoPopupView.VideoPreviewURL
        {
            get;
            set;
        }

        Int32 ISharedUserViewVideoPopupView.RequrmntFieldVideoID
        {
            get;
            set;
        }

        Int32 ISharedUserViewVideoPopupView.TenantID
        {
            get;
            set;
        }

        Int32 ISharedUserViewVideoPopupView.RequrmntObjTreeID
        {
            get;
            set;
        }

        RequirementFieldVideoData ISharedUserViewVideoPopupView.RequrmntVideoData
        {
            get;
            set;
        }

        ObjectAttributeContract ISharedUserViewVideoPopupView.ObjectAttrContract
        {
            get;
            set;
        }

        RequirementObjectTreeContract ISharedUserViewVideoPopupView.RequrmntObjTreePropertyContract
        {
            get;
            set;
        }

        String ISharedUserViewVideoPopupView.IsFromAdmin
        {
            get;
            set;
        }

        Boolean ISharedUserViewVideoPopupView.IsEditMode
        {
            get;
            set;
        }

        #endregion

        #region Public Properties

        public SharedUserViewVideoPopupPresenter Presenter
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

        public ISharedUserViewVideoPopupView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        #endregion

        #endregion

        #region Page Event

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    //CurrentViewContext.RotId = _rotId;

                    Page.Title = "View Video";
                    if (!Request.QueryString["RequrmntFieldVideoID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.RequrmntFieldVideoID = Convert.ToInt32(Request.QueryString["RequrmntFieldVideoID"]);
                    }
                    if (!Request.QueryString["TenantID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.TenantID = Convert.ToInt32(Request.QueryString["TenantID"]);
                    }
                    if (!Request.QueryString["RequrmntObjTreeID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.RequrmntObjTreeID = Convert.ToInt32(Request.QueryString["RequrmntObjTreeID"]);
                    }

                    if (!Request.QueryString["IsFromAdmin"].IsNullOrEmpty())
                    {
                        CurrentViewContext.IsFromAdmin = Request.QueryString["IsFromAdmin"].ToString();
                    }

                    if (!Request.QueryString["IsEditMode"].IsNullOrEmpty())
                    {
                        CurrentViewContext.IsEditMode = Convert.ToBoolean(Request.QueryString["IsEditMode"]);
                    }

                    if (!CurrentViewContext.IsFromAdmin.IsNullOrEmpty() && CurrentViewContext.IsFromAdmin == "True")
                    {
                        LoadVideoForPreview();
                    }
                    else
                    {
                        LoadVideo();
                    }
                }
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Video Url from database.
        /// </summary>
        private void LoadVideo()
        {

            Presenter.GetRfVideoData();
            Presenter.GetObjectAttrProperties();
            SetVideoWndControls();
            hdnIsAdminRequested.Value = "False";
        }

        /// <summary>
        /// Set Video information to hidden fields
        /// </summary>
        private void SetVideoWndControls()
        {
            //hdnVideoUrl.Value = CurrentViewContext.requirementVideoData.VideoURL;
            if (CurrentViewContext.IsEditMode)
            {
                hdnBoxStayOpenTime.Value = "0";
                hdnIsEditMode.Value = "true";
            }
            else
            {
                hdnBoxStayOpenTime.Value = CurrentViewContext.ObjectAttrContract.BoxOpenTime.IsNullOrEmpty()
                                            ? "0" : CurrentViewContext.ObjectAttrContract.BoxOpenTime;
            }
            hdnIsReqToOpen.Value = CurrentViewContext.ObjectAttrContract.IsRequiredToOpen ? "True" : "False";

            //if (hdnBoxStayOpenTime.Value == "0")
            //{
            //    btnCloseViewVideo.Visible = true;
            //}
            //else
            //{
            //    btnCloseViewVideo.Visible = false;
            //}
            String videoUrl = GetFormattedVideoURL(CurrentViewContext.RequrmntVideoData.VideoURL);
            iframeViewVideo.Src = videoUrl;
            //UAT-1470: As a student, there should be a way to close out of the video once you open it.
            hdnVideoRequiredOpenTime.Value = CurrentViewContext.ObjectAttrContract.BoxOpenTime.IsNullOrEmpty()
                                            ? "0" : CurrentViewContext.ObjectAttrContract.BoxOpenTime;
            if (!hdnVideoRequiredOpenTime.Value.IsNullOrEmpty() && Convert.ToBoolean(hdnIsReqToOpen.Value) == true)
            {
                TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(hdnVideoRequiredOpenTime.Value));
                String requiredTime = String.Empty;
                if (t.Hours > AppConsts.NONE)
                {
                    requiredTime = t.Hours + " hr(s) ";
                }
                if (t.Minutes > AppConsts.NONE)
                {
                    requiredTime = requiredTime + t.Minutes + " min(s) ";
                }
                if (t.Seconds > AppConsts.NONE)
                {
                    requiredTime = requiredTime + t.Seconds + " Sec(s) ";
                }

                lblMessage.Text = "In order to complete this item, you must watch the entire video with a duration of " + requiredTime;
                lblMessage.Visible = true;
            }
        }

        private void LoadVideoForPreview()
        {
            String videoUrl = GetFormattedVideoURL(CurrentViewContext.RequirementPackageContractSessionData.PreviewVideoURL);
            iframeViewVideo.Src = videoUrl;
            hdnIsAdminRequested.Value = "True";
        }

        private String GetFormattedVideoURL(String inputURL)
        {
            String outputUrl = inputURL;
            Regex vimeoRegex = new Regex(@"vimeo\.com/(?:.*#|.*/videos/)?([0-9]+)");
            Regex youTubeRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
            if (youTubeRegex.IsMatch(outputUrl))
            {
                String replacement = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.YOUTUBE_URL_REPLACEMENT_KEY]);
                outputUrl = youTubeRegex.Replace(outputUrl, replacement);
            }
            else if (vimeoRegex.IsMatch(outputUrl))
            {
                String replacement = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.VIMEO_URL_REPLACEMENT_KEY]);
                outputUrl = vimeoRegex.Replace(outputUrl, replacement);
            }
            return outputUrl;
        }

        #endregion

    }
}