using CoreWeb.AgencyJobBoard.Views;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CoreWeb.Shell;

namespace CoreWeb.AgencyJobBoard.UserControls.Views
{
    public partial class ViewAgencyJobPost : BaseUserControl, IViewAgencyJobPostView
    {
        #region Variables

        private ViewAgencyJobPostPresenter _presenter = new ViewAgencyJobPostPresenter();
        private String _viewType;

        #endregion

        #region Properties

        public ViewAgencyJobPostPresenter Presenter
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

        public IViewAgencyJobPostView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IViewAgencyJobPostView.CurrentAgencyJobID { get; set; }

        public AgencyJobContract AgencyJobDetail
        {
            get;
            set;
        }

        public int loggedInUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                return base.CurrentUserId;
            }
        }

        Boolean IViewAgencyJobPostView.IsApplicant
        {
            get
            {
                if (!ViewState["IsAppliacnt"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsAppliacnt"]);
                }
                return false;
            }
            set
            {
                ViewState["IsAppliacnt"] = value;
            }
        }

        Boolean IViewAgencyJobPostView.IsPreviewMode
        {
            get
            {
                if (!ViewState["IsPreviewMode"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsPreviewMode"]);
                }
                return false;
            }
            set
            {
                ViewState["IsPreviewMode"] = value;
            }
        }

        Int32 IViewAgencyJobPostView.OrganisationUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }

        public String ControlUseType
        {
            get
            {
                if (ViewState["ControlUseType"] != null)
                    return (Convert.ToString(ViewState["ControlUseType"]));
                return String.Empty;
            }
            set
            {
                ViewState["ControlUseType"] = value;
            }
        }

        #endregion

        #region  Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                Presenter.CheckIfUserIsApplicant();
                if (!CurrentViewContext.IsApplicant)
                {
                    base.Title = "Details";
                   // base.SetPageTitle("Details");
                    base.OnInit(e);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    //base.Title = "Job Details";
                    //base.OnInit(e);

                    if (!this.IsPostBack || this.ControlUseType == AppConsts.DASHBOARD)
                    {
                        if (!CurrentViewContext.IsPreviewMode)
                        {
                            Presenter.OnViewInitialized();
                            CaptureQueryStringParameters();

                            if (!CurrentViewContext.IsApplicant)
                                fsucCmdBarAgencyDetails.SaveButton.Enabled = false;
                            else
                                fsucCmdBarAgencyDetails.SaveButton.Enabled = true;
                        }
                        else
                        {
                            fsucCmdBarAgencyDetails.Visible = false;
                            //cmdClosePopup.Visible = true;
                        }
                    }
                    BindControls();
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

        #region [Private Methods]

        private void CaptureQueryStringParameters()
        {
            var args = new Dictionary<String, String>();

            if (!Request.QueryString["argss"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["argss"]);
                if (args.ContainsKey("AgencyJobID"))
                {
                    CurrentViewContext.CurrentAgencyJobID = Convert.ToInt32(args["AgencyJobID"]);
                }

                if (args.ContainsKey("IsApplicant"))
                {
                    CurrentViewContext.IsApplicant = Convert.ToBoolean(args["IsApplicant"]);
                }
            }

            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("AgencyJobID"))
                {
                    CurrentViewContext.CurrentAgencyJobID = Convert.ToInt32(args["AgencyJobID"]);
                }

                if (args.ContainsKey("IsApplicant"))
                {
                    CurrentViewContext.IsApplicant = Convert.ToBoolean(args["IsApplicant"]);
                }
            }
        }

        private void BindControls()
        {
            if (CurrentViewContext.IsPreviewMode)
                Presenter.GetAgencyLogo();
            else
                Presenter.GetSelectedJobPostDetails();
            if (!CurrentViewContext.AgencyJobDetail.IsNullOrEmpty())
            {
                imgCntrl.ImageUrl = RenderLogo(CurrentViewContext.AgencyJobDetail.LogoPath);
                if (!CurrentViewContext.AgencyJobDetail.JobTitle.IsNullOrEmpty())
                {
                    dvJobTitle.Visible = true;
                    lblJobTitle.Text = CurrentViewContext.AgencyJobDetail.JobTitle.HtmlEncode();
                }
                if (!CurrentViewContext.AgencyJobDetail.Company.IsNullOrEmpty())
                {
                    dvCompany.Visible = true;
                    lblCompany.Text = CurrentViewContext.AgencyJobDetail.Company.HtmlEncode();
                }
                if (!CurrentViewContext.AgencyJobDetail.Location.IsNullOrEmpty())
                {
                    lblLocation.Visible = true;
                    lblLocation.Text = CurrentViewContext.AgencyJobDetail.Location.HtmlEncode();
                }
                if (!CurrentViewContext.AgencyJobDetail.JobDescription.IsNullOrEmpty())
                {
                    dvDescription.Visible = true;
                    spnDescription.InnerHtml = CurrentViewContext.AgencyJobDetail.JobDescription;
                }
                if (!CurrentViewContext.AgencyJobDetail.Instructions.IsNullOrEmpty())
                {
                    dvInstruction.Visible = true;
                    spnInstruction.InnerHtml = CurrentViewContext.AgencyJobDetail.Instructions.Replace("\n", "<br/>").HtmlEncode(); ;
                }
                if (!CurrentViewContext.AgencyJobDetail.HowToApply.IsNullOrEmpty())
                {
                    spnHowToApply.InnerHtml = CurrentViewContext.AgencyJobDetail.HowToApply.Replace("\n", "<br/>");
                }
                if (!CurrentViewContext.AgencyJobDetail.Company.IsNullOrEmpty() && !CurrentViewContext.AgencyJobDetail.Location.IsNullOrEmpty())
                {
                    lblHyphen.Visible = true;
                }
                if (CurrentViewContext.IsApplicant && CurrentViewContext.AgencyJobDetail.HowToApply.IsNullOrEmpty())
                {
                    fsucCmdBarAgencyDetails.SaveButton.Style.Add("display", "none");
                }
            }
        }

        private string RenderLogo(String documentPath)
        {
            if (File.Exists(documentPath))
            {
                String extension = Path.GetExtension(documentPath);
                FileStream myFileStream = new FileStream(documentPath, FileMode.Open);
                long FileSize = myFileStream.Length;
                byte[] buffer = new byte[(int)FileSize];
                myFileStream.Read(buffer, 0, (int)FileSize);
                myFileStream.Close();
                myFileStream.Dispose();
                string base64 = Convert.ToBase64String(buffer);
                return string.Format("data:{0};base64,{1}", GetContentType(extension), base64);
            }
            return string.Empty;
        }

        private String GetContentType(String fileExtension)
        {
            switch (fileExtension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".swf":
                    return "application/x-shockwave-flash";
                case ".gif":
                    return "image/gif";
                case ".jpeg":
                case ".jpg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".txt":
                    return "text/plain";
                case ".xls":
                case ".xlsx":
                case ".csv":
                    return "application/vnd.ms-excel";
                default:
                    return "application/octet-stream";
            }
        }

        #endregion

        #region [Button Events]

        protected void fsucCmdBarAgencyDetails_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                String url = String.Empty;
                if (CurrentViewContext.IsApplicant)
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                {
                                    { "CancelClick",  "True"}
                                };

                    Response.Redirect(AppConsts.DASHBOARD_URL + "?MenuId=11&argss=" + queryString.ToEncryptedQueryString(), true);
                }
                else
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                {
                                    { "Child",  AppConsts.VIEW_AGENCY_JOB_POST},
                                    { "CancelClick",  "True"}
                                };

                    url = String.Format("~/AgencyJobBoard/Default.aspx?ucid={0}&args={1}", "", queryString.ToEncryptedQueryString());
                }
                Response.Redirect(url, true);
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
    }
}