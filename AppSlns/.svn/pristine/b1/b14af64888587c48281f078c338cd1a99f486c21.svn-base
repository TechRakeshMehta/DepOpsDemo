using CoreWeb.ComplianceOperations.Views;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Pages
{
    public partial class RejectedItemListSubmissionPopup : BaseWebPage, IRejectedItemListSubmissionPopup
    {

        #region Properties

        #region Private Properties

        private RejectedItemListSubmissionPopupPresenter _presenter = new RejectedItemListSubmissionPopupPresenter();

        #endregion

        #region Public Properties

        public RejectedItemListSubmissionPopupPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }
        public IRejectedItemListSubmissionPopup CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IRejectedItemListSubmissionPopup.OrgUserId
        {
            get
            {
                if (ViewState["OrgUserId"].IsNotNull())
                    return Convert.ToInt32(ViewState["OrgUserId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["OrgUserId"] = value;
            }
        }

        Int32 IRejectedItemListSubmissionPopup.TenantId
        {
            get
            {
                if (ViewState["TenantId"].IsNotNull())
                    return Convert.ToInt32(ViewState["TenantId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IRejectedItemListSubmissionPopup.CurrenLoggedInUserId
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return CurrentViewContext.OrgUserId;
            }
        }

        List<RejectedItemListContract> IRejectedItemListSubmissionPopup.lstRejectedItemListContract
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region  EVENTS

        #region PAGE EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
                CurrentViewContext.OrgUserId = Convert.ToInt32(Request.QueryString["orgUserId"]);

                if (CurrentViewContext.TenantId > AppConsts.NONE && CurrentViewContext.OrgUserId > AppConsts.NONE)
                    Presenter.GetRejectedItemListForReSubmission();
                BindRepeater();

            }
        }

        #endregion

        #region BUTTON CLICK
        protected void fsucCommandBar_SaveClick(object sender, EventArgs e)
        {
            List<Int32> lstSelectedApplicantComplItemId = new List<Int32>();
            foreach (RepeaterItem item in rptrRejectedItemList.Items)
            {
                HiddenField hdnApplicantComplianceItemID = item.FindControl("hdnApplicantComplianceItemId") as HiddenField;
                Int32 ApplicantComplianceItemID = Convert.ToInt32(hdnApplicantComplianceItemID.Value);
                CheckBox chkbox = item.FindControl("chkItem") as CheckBox;
                if (chkbox.Checked)
                {
                    lstSelectedApplicantComplItemId.Add(ApplicantComplianceItemID);
                }
            }
            if (!lstSelectedApplicantComplItemId.IsNullOrEmpty())
            {
                if (Presenter.ResubmitApplicantComplianceItemData(lstSelectedApplicantComplItemId))
                {
                    lblErrorMessage.Visible = false;
                    lblSuccessMessage.Visible = true;
                    lblSuccessMessage.Text = "Selected item(s) successfully requeued for review.";
                    return;
                }
                else
                {
                    //show error message
                    lblSuccessMessage.Visible = false;
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Selected item(s) does not successfully requeued for review.";
                    return;
                }
            }
            else
            {
                //show error message
                lblSuccessMessage.Visible = false;
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Please select item(s) that you want to requeue for review.";
                return;
            }
        }
        #endregion

        #endregion

        #region PRIVATE METHODS

        private void BindRepeater()
        {
            rptrRejectedItemList.DataSource = CurrentViewContext.lstRejectedItemListContract;
            rptrRejectedItemList.DataBind();
        }

        #endregion
    }
}