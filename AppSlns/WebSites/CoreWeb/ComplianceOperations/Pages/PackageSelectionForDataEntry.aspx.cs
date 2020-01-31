using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using CoreWeb.ComplianceOperations.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations.Pages.Views
{
    public partial class PackageSelectionForDataEntry : BaseWebPage, IPackageSelectionForDataEntryView
    {
        #region Private Variables

        private PackageSelectionForDataEntryPresenter _presenter = new PackageSelectionForDataEntryPresenter();
        private List<PackageSubscriptionForDataEntry> _lstSubscriptions = new List<PackageSubscriptionForDataEntry>();

        #endregion

        #region Properties

        public PackageSelectionForDataEntryPresenter Presenter
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

        public IPackageSelectionForDataEntryView CurrentViewContext
        {
            get { return this; }
        }

        /// <summary>
        /// This will contain Aplicant TenantID of the type, based on the scenarios
        /// 1. If the current document had only one subscription and next record had multiple subscriptions, this will contain the TenantID of the 'Next Record'
        /// 2. If the current document had multiple subscriptions, then it will have the TenantID of the 'same document record'. 
        /// In that case, NextRecordTenantId and NextRecordApplicantId are used to get the data of the Next record in queue. 
        /// </summary>
        public Int32 SelectedTenantID
        {
            get
            {
                if (ViewState["SelectedTenantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedTenantID"] = value;
            }
        }

        /// <summary>
        /// This will contain Aplicant OrganizationID of the type, based on the scenarios
        /// 1. If the current document had only one subscription and next record had multiple subscriptions , this will contain the ApplicantID of the 'Next Record'
        /// 2. If the current document had multiple subscriptions, then it will have the ApplicantID of the 'same document record'. 
        /// In that case, NextRecordTenantId and NextRecordApplicantId are used to get the data of the Next record in queue. 
        /// </summary>
        public Int32 ApplicantOrganizationUserID
        {
            get
            {
                if (ViewState["ApplicantOrganizationUserID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ApplicantOrganizationUserID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ApplicantOrganizationUserID"] = value;
            }
        }

        /// <summary>
        /// Will be available only from the Details screen and that too if the document had multiple subscriptions
        /// and user selects 'Save and Done'
        /// </summary>
        Int32 IPackageSelectionForDataEntryView.NextRecordTenantId
        {
            get
            {
                if (ViewState["NextRecordTenantId"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["NextRecordTenantId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["NextRecordTenantId"] = value;
            }
        }

        /// <summary>
        /// Will be available only from the Details screen and that too if the document had multiple subscriptions
        /// and user selects 'Save and Done'
        /// </summary>
        Int32 IPackageSelectionForDataEntryView.NextRecordApplicantId
        {
            get
            {
                if (ViewState["NextRecordApplicantId"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["NextRecordApplicantId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["NextRecordApplicantId"] = value;
            }
        }

        /// <summary>
        /// Represents the property to decide whether the Same document subscriptions are 
        /// being displayed when the user has Selected 'Save & Done' from dEtails screen.
        /// This will be false 
        /// 1. When the screen is opened from the Queue 
        /// 2. There was only one susbcription for the Document which was opened 
        /// 3. There were multiple susbcriptions for the Document which was opened but user selected 'Go to Next Document'
        /// </summary>
        Boolean IPackageSelectionForDataEntryView.IsSameDocument
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsSameDocument"]);
            }
            set
            {
                ViewState["IsSameDocument"] = value;
            }
        }

        /// <summary>
        ///  Represents the SubscriptionId of the applicant for whom multiple subscriptions exist.
        ///  Will be available, only - When the 'Save and Done' is clicked from the Details screen and 
        ///  Applicant has more subscriptions apart from current one.
        /// </summary>
        Int32 IPackageSelectionForDataEntryView.CurrentSubscriptionId
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentSubscriptionId"]);
            }
            set
            {
                ViewState["CurrentSubscriptionId"] = value;
            }
        }

        List<PackageSubscriptionForDataEntry> IPackageSelectionForDataEntryView.lstPackageSubscription
        {
            set
            {
                rptPackageSubscription.DataSource = _lstSubscriptions = value;
                rptPackageSubscription.DataBind();
            }
            get
            {
                return _lstSubscriptions;
            }
        }

        /// <summary>
        /// Represents the status of the document to be updated in the database
        /// </summary>
        String IPackageSelectionForDataEntryView.DocumentStatus
        {
            get;
            set;
        }

        /// <summary>
        /// ÒrganizationUserID of the CurrentLogged in user
        /// </summary>
        Int32 IPackageSelectionForDataEntryView.CurrentUserId
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentUserId"]);
            }
            set
            {
                ViewState["CurrentUserId"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 IPackageSelectionForDataEntryView.DocumentId
        {
            get
            {
                return Convert.ToInt32(ViewState["DocumentId"]);
            }
            set
            {
                ViewState["DocumentId"] = value;
            }
        }

        /// <summary>
        /// Represents the 'FlatDataEntryQueue' PK for the Document which was opened.
        /// 1. If the current document had only one subscription and next record had multiple subscriptions , this will contain the FDEQId of the 'Next Record'
        /// 2. If the current document had multiple subscriptions, then it will have the FDEQId of the 'same document record'. 
        /// </summary>
        Int32 IPackageSelectionForDataEntryView.FDEQId
        {
            get
            {
                return Convert.ToInt32(ViewState["FDEQId"]);
            }
            set
            {
                ViewState["FDEQId"] = value;
            }
        }


        /// <summary>
        /// Data to track the Data entry Productivity
        /// </summary>
        DataEntryTrackingContract IPackageSelectionForDataEntryView.DataEntryTimeTracking
        {
            get;
            set;
        }

        /// <summary>
        /// DocumentStatusID to be used for time tracking
        /// </summary>
        short IPackageSelectionForDataEntryView.DocumentStatusId
        {
            get;
            set;
        }

        //UAT-2742
        Boolean IPackageSelectionForDataEntryView.IsDiscardDocument
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDiscardDocument"]);
            }
            set
            {
                ViewState["IsDiscardDocument"] = value;
            }
        }

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CaptureQueryStringData();

                    Presenter.GetPackageSubscriptionOfApplicant();

                    // This will also cover the scenarion when the Screen is opened from the Queue. 
                    if (CurrentViewContext.IsSameDocument)
                    {
                        CurrentViewContext.lstPackageSubscription = CurrentViewContext.lstPackageSubscription
                                                                                      .Where(ps => ps.PackageSubscriptionID != CurrentViewContext.CurrentSubscriptionId)
                                                                                      .ToList();
                        //UAT-2742
                        if (CurrentViewContext.IsDiscardDocument)
                        {
                            ManageControls(true, false, "Following subscription(s) are related to the current document.");
                        }
                        else
                        {
                            ManageControls(true, false, "Following subscription(s) are related to the current document. You can select from these or load next document.");
                        }
                    }
                    else
                    {
                        ManageControls(false, true, String.Empty);
                    }
                    //UAT-2742
                    if (CurrentViewContext.IsDiscardDocument)
                    {
                        cmdSameDocument.SubmitButton.Style["display"] = "none";
                    }
                    else
                    {
                        cmdSameDocument.SubmitButton.Style["display"] = "inline-block";
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

        #region Repeater Events
        #endregion

        #region Button Events

        protected void btnOk_Click(Object sender, EventArgs e)
        {
            LoadSelectedSubscription();
        }

        /// <summary>
        /// Event to Load the Subscription for the document already selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSameDocument_SaveClick(Object sender, EventArgs e)
        {
            hdnIsDocChange.Value = "0";
            hdnApplicantId.Value = Convert.ToString(CurrentViewContext.ApplicantOrganizationUserID);
            LoadSelectedSubscription();
        }

        /// <summary>
        /// Event to Open Display the Panel for the Next record subscriptions. 
        /// Fetch the next record related subscriptions to be displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSameDocument_SubmitClick(Object sender, EventArgs e)
        {
            CurrentViewContext.DocumentStatus = DataEntryDocumentStatus.COMPLETE.GetStringValue();
            Presenter.UpdateDocumentStatus();

            CurrentViewContext.DataEntryTimeTracking = (DataEntryTrackingContract)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.DATA_ENTRY_TRACKING);

            if (CurrentViewContext.DataEntryTimeTracking.IsNotNull())
            {
                Presenter.GetDocumentStatusIdByCode();
                CurrentViewContext.DataEntryTimeTracking.ExitTime = DateTime.Now;
                CurrentViewContext.DataEntryTimeTracking.StatusId = CurrentViewContext.DocumentStatusId;
                Presenter.UpdateTimeTracking();
                Session.Remove(ResourceConst.DATA_ENTRY_TRACKING);
                //return "Success";
            }

            if (CurrentViewContext.NextRecordApplicantId == AppConsts.NONE || CurrentViewContext.NextRecordApplicantId == AppConsts.NONE)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ClosePopup();", true);
                return;
            }

            hdnApplicantId.Value = Convert.ToString(CurrentViewContext.NextRecordApplicantId);
            hdnTenantId.Value = Convert.ToString(CurrentViewContext.NextRecordTenantId);
            hdnIsDocChange.Value = "1";

            CurrentViewContext.SelectedTenantID = CurrentViewContext.NextRecordTenantId;
            CurrentViewContext.ApplicantOrganizationUserID = CurrentViewContext.NextRecordApplicantId;

            ManageControls(false, true, String.Empty);
            Presenter.GetPackageSubscriptionOfApplicant();

            if (CurrentViewContext.lstPackageSubscription.IsNullOrEmpty())
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ClosePopup();", true);
            }
            else if (!CurrentViewContext.lstPackageSubscription.IsNullOrEmpty() && CurrentViewContext.lstPackageSubscription.Count == AppConsts.ONE)
            {
                // Set the SubscriptionId for the Only record available and close the window
                hdnSubscriptionID.Value = Convert.ToString(CurrentViewContext.lstPackageSubscription.First().PackageSubscriptionID);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
            }
        }

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Loads the subscription selected from the RadioButton List
        /// </summary>
        private void LoadSelectedSubscription()
        {
            try
            {
                Int32 count = 0;
                foreach (var rptItem in rptPackageSubscription.Items)
                {
                    RadioButton rbtnSubscription = ((rptItem as RepeaterItem).FindControl("rbtnSubscription") as RadioButton);
                    if (rbtnSubscription.IsNotNull() && rbtnSubscription.Checked)
                    {
                        Int32 _packageSubscriptionId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnPackageSubscriptionID") as HiddenField).Value);
                        hdnSubscriptionID.Value = _packageSubscriptionId.ToString();
                        count++;
                        break;
                    }
                }
                if (count > AppConsts.NONE)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
                }
                else
                {
                    base.ShowInfoMessage("Please select package subscription.");
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Read the data from Querystrings and set in properties
        /// </summary>
        private void CaptureQueryStringData()
        {
            if (!Request["TenantID"].IsNullOrEmpty())
            {
                CurrentViewContext.SelectedTenantID = Convert.ToInt32(Request["TenantID"]);
                // Must be Updated in detail screen hidden field  through call to 'returnToParent' function 
                hdnTenantId.Value = Convert.ToString(Request["TenantID"]);
            }
            if (!Request["ApplicantID"].IsNullOrEmpty())
            {
                CurrentViewContext.ApplicantOrganizationUserID = Convert.ToInt32(Request["ApplicantID"]);
                // Must be Updated in detail screen hidden field  through call to 'returnToParent' function 
                hdnApplicantId.Value = Convert.ToString(Request["ApplicantID"]);
            }
            if (!Request["IsSameDoc"].IsNullOrEmpty())
            {
                CurrentViewContext.IsSameDocument = Convert.ToBoolean(Request["IsSameDoc"]);
            }
            if (!Request["CurrentSubId"].IsNullOrEmpty())
            {
                CurrentViewContext.CurrentSubscriptionId = Convert.ToInt32(Request["CurrentSubId"]);
            }

            if (!Request["NextTenantId"].IsNullOrEmpty())
            {
                CurrentViewContext.NextRecordTenantId = Convert.ToInt32(Request["NextTenantId"]);
            }
            if (!Request["NextAppId"].IsNullOrEmpty())
            {
                CurrentViewContext.NextRecordApplicantId = Convert.ToInt32(Request["NextAppId"]);
            }

            if (!Request["DocId"].IsNullOrEmpty())
            {
                CurrentViewContext.DocumentId = Convert.ToInt32(Request["DocId"]);
            }

            if (!Request["CrntUserId"].IsNullOrEmpty())
            {
                CurrentViewContext.CurrentUserId = Convert.ToInt32(Request["CrntUserId"]);
            }

            if (!Request["FdeqId"].IsNullOrEmpty())
            {
                CurrentViewContext.FDEQId = Convert.ToInt32(Request["FdeqId"]);
            }
            //UAT-2742
            if (!Request["IsDiscardDocument"].IsNullOrEmpty())
            {
                CurrentViewContext.IsDiscardDocument = Convert.ToBoolean(Request["IsDiscardDocument"]);
            }
        }

        /// <summary>
        /// Manage the controls dependening on the whether the current document has multiple subscriptions related to it or not
        /// </summary>
        /// <param name="currentRecordCtrlsVisibility"></param>
        /// <param name="nextRecordCtrlsVisibilit"></param>
        /// <param name="headerText"></param>
        private void ManageControls(Boolean currentRecordCtrlsVisibility, Boolean nextRecordCtrlsVisibilit, String headerText)
        {
            divNextDocumentButtons.Visible = nextRecordCtrlsVisibilit;
            divSameDocumentButtons.Visible = currentRecordCtrlsVisibility;
            litMsg.Text = headerText;
        }

        #endregion

        #endregion

        #endregion
    }
}