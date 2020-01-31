using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceOperations.Views;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System.Xml;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class DataEntry : BaseUserControl, IDateEntryView
    {
        #region Variables

        #region Private Variables

        private DataEntryPresenter _presenter = new DataEntryPresenter();
        private String _viewType;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        public DataEntryPresenter Presenter
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
        /// Id of the selected Tenant
        /// </summary>
        Int32 IDateEntryView.TenantId
        {
            get
            {
                return (Convert.ToInt32(ViewState["TenantId"]));
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        /// <summary>
        /// Id of the selected Document PK of the ApplicantDocument table. 
        /// </summary>
        public Int32 DocumentId
        {
            get
            {
                return (Convert.ToInt32(ViewState["DocumentId"]));
            }
            set
            {
                ViewState["DocumentId"] = value;
            }
        }

        /// <summary>
        /// Id of the selected Subscription
        /// </summary>
        Int32 IDateEntryView.PkgSubId
        {
            get
            {
                return (Convert.ToInt32(ViewState["PkgSubId"]));
            }
            set
            {
                ViewState["PkgSubId"] = value;
            }
        }


        /// <summary>
        /// OrganizationUserId of the Applicant to whom the current document belongs to
        /// </summary>
        Int32 IDateEntryView.ApplicantId
        {
            get
            {
                return (Convert.ToInt32(ViewState["ApplicantId"]));
            }
            set
            {
                ViewState["ApplicantId"] = value;
            }
        }

        /// <summary>
        ///  
        /// </summary>
        List<AdminDataEntryUIContract> IDateEntryView.UIContract
        {
            get;
            set;
        }

        /// <summary>
        /// Data to be stored for the Form
        /// </summary>
        AdminDataEntrySaveContract IDateEntryView.SaveContract
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the Context of the current view
        /// </summary>
        public IDateEntryView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Id of the Current logged-in used
        /// </summary>
        Int32 IDateEntryView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        ///  Document Status. 
        /// </summary>
        public String DocumentStatus
        {
            get
            {
                return (Convert.ToString(ViewState["DocumentStatus"]));
            }
            set
            {
                ViewState["DocumentStatus"] = value;
            }
        }

        public String QueueType
        {
            get
            {
                if (ViewState["QueueType"].IsNotNull())
                {
                    return Convert.ToString(ViewState["QueueType"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["QueueType"] = value;
            }
        }

        /// <summary>
        /// Id of the selected Document i.e.  PK of the ApplicantDocument table. 
        /// </summary>
        public Int32 NxtDocumentId
        {
            get
            {
                return (Convert.ToInt32(ViewState["DocumentId"]));
            }
            set
            {
                ViewState["DocumentId"] = value;
            }
        }

        /// <summary>
        /// Id of the selected Subscription
        /// </summary>
        public Int32 NxtPkgSubId
        {
            get
            {
                return (Convert.ToInt32(ViewState["PkgSubId"]));
            }
            set
            {
                ViewState["PkgSubId"] = value;
            }
        }

        public DataEntryQueueContract NextRecord
        {
            get
            {
                return ((ViewState["NextRecord"]) as DataEntryQueueContract);
            }
            set
            {
                ViewState["NextRecord"] = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (ViewState["gridCustomPaging"] == null)
                {
                    ViewState["gridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["gridCustomPaging"];
            }
        }

        /// <summary>
        /// Get FDEQ_ID (Flat data entry docId)
        /// </summary>
        public Int32 FDEQ_ID
        {
            get;
            set;

        }

        /// <summary>
        /// Get no of impacted item count
        /// </summary>
        public Int32 ImpactedItemCnt
        {
            get;
            set;
        }

        public List<Int32> lstImpactedIds { get; set; }

        #region UAT-1608:
        /// <summary>
        /// Data to be stored for itesmSeries
        /// </summary>
        AdminDataEntrySaveContract IDateEntryView.ItemSeriesSaveContract
        {
            get;
            set;
        }

        String IDateEntryView.ErrorMessage
        {
            set
            {
                base.ShowErrorMessage(value);
            }
        }
        String IDateEntryView.SuccessMessage
        {
            set
            {
                base.ShowSuccessMessage(value);
            }
        }
        #endregion

        #region UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
        /// <summary>
        /// Get FDEQ_ID (Flat data entry docId)
        /// </summary>
        public Int32 DiscardDocumentCount
        {
            get;
            set;
        }
        #endregion

        #region Production Issue: Data Entry[26/12/2016]
        Int32 IDateEntryView.SelectedDiscardReasonId
        {
            get
            {
                //if (!ddlDiscardReason.SelectedValue.IsNullOrEmpty())
                //{
                //hdnDocumentDiscardReasonId.Value = ddlDiscardReason.SelectedValue;
                //    return Convert.ToInt32(ddlDiscardReason.SelectedValue);
                //}
                if (!hdnDocumentDiscardReasonId.Value.IsNullOrEmpty())
                {
                    return Convert.ToInt32(hdnDocumentDiscardReasonId.Value);
                }
                return AppConsts.NONE;
            }

        }

        String IDateEntryView.DiscardReasonText
        {
            get
            {
                if (!ddlDiscardReason.SelectedValue.IsNullOrEmpty())
                {
                    return ddlDiscardReason.SelectedItem.Text;
                }
                return String.Empty;
            }

        }

        List<Entity.ClientEntity.lkpDocumentDiscardReason> IDateEntryView.LstDocumentDiscradReason
        {
            set
            {
                ddlDiscardReason.DataSource = value;
                ddlDiscardReason.DataBind();
            }
        }

        String IDateEntryView.AdditionalNotes
        {
            get { return txtAdditionalNotes.Text; }
        }

        /// <summary>
        /// Get Need to send email on document discard
        /// </summary>
        Boolean IDateEntryView.IsDiscardDocumentEmailNeedToSend
        {
            get;
            set;
        }

        #endregion
        //UAT 2695
        String IDateEntryView.DocumentName
        {
            get
            {
                return lblDocName.Text.Trim();
            }
            set
            {
                lblDocName.Text = value;
            }
        }
        Dictionary<Int32, Int32> IDateEntryView.lstRuleVoilatedItem
        {
            get
            {
                return ((ViewState["lstRuleVoilatedItem"]) as Dictionary<Int32, Int32>);
            }
            set
            {
                ViewState["lstRuleVoilatedItem"] = value;
            }
        }

        //UAT-2742
        List<Int32> IDateEntryView.lstPkgSubForDocDiscard
        {
            get
            {
                if (Session["lstPkgSubForDocDiscard"].IsNotNull())
                {
                    return Session["lstPkgSubForDocDiscard"] as List<Int32>;
                }
                return new List<Int32>();
            }
            set
            {
                Session["lstPkgSubForDocDiscard"] = value;
            }
        }
        //UAT-2742
        Boolean IDateEntryView.IsAnySubsForSaveAndDone
        {
            get
            {
                return Convert.ToBoolean(Session["IsAnySubsForSaveAndDone"]);
            }
            set
            {
                Session["IsAnySubsForSaveAndDone"] = value;
            }
        }
        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Pending Document Data Entry";
                lblAdminDataEntry.Text = base.Title;
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
                CaptureQueryStringData();
                CurrentViewContext.UIContract = Presenter.GetAdminDataEntrySubscription();
                GenerateForm();
                //Production issue changes [26/12/2016]
                //Presenter.GetDocumentDiscardReasonList();

                //Document Viewer Code
                Dictionary<String, String> requestDocViewerArgs = new Dictionary<String, String>();
                requestDocViewerArgs = new Dictionary<String, String>
                                                                 { 
                                                                    {"DocumentId", Convert.ToString(this.DocumentId) },
                                                                    {"TenantId", Convert.ToString(CurrentViewContext.TenantId)},                                                                    
                                                                 };
                string url = String.Format(@"/ComplianceOperations/AdminDataEntryDocViewer.aspx?args={0}", requestDocViewerArgs.ToEncryptedQueryString());
                hdnADEDocVwr.Value = Convert.ToString(url);
                hdnDocIDDataEntry.Value = Convert.ToString(this.DocumentId);

                if (!IsPostBack)
                {
                    hdnSelectedTenantID.Value = CurrentViewContext.TenantId.ToString();
                    DataEntryTrackingContract dataEntryTimeTracking = null;
                    dataEntryTimeTracking = (DataEntryTrackingContract)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.DATA_ENTRY_TRACKING);
                    if (dataEntryTimeTracking.IsNull())
                    {
                        DataEntryTrackingContract newTracking = new DataEntryTrackingContract();
                        newTracking.EntryTime = DateTime.Now;
                        newTracking.DocumentId = FDEQ_ID;
                        newTracking.QueueProcessUserId = CurrentUserId;
                        newTracking.StatusId = Presenter.GetDocumentStatusIdByCode(DataEntryDocumentStatus.IN_PROGRESS.GetStringValue());
                        SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.DATA_ENTRY_TRACKING, newTracking);
                    }
                    //else
                    //{
                    //    dataEntryTimeTracking.
                    //}
                    Presenter.GetDataEntryNextRecord(GetSessionValues());
                    //UAT 2695
                    Presenter.GetApplicantDocument();
                }
                base.SetPageTitle("Pending Document Data Entry");
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "handleRadComboUx", "handleRadComboUI();", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "handleRadPickerUx", "handleRadPickerUI();", true);
            }
            catch (Exception ex)
            {

            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "onDocReady();", true);
        }

        #endregion

        #region Button Events


        /// <summary>
        /// Temporary Saves the data and returns the user to Queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveTemp_Click(object sender, EventArgs e)
        {
            try
            {
                DocumentStatus = DataEntryDocumentStatus.IN_PROGRESS.GetStringValue();
                hdnButtonClicked.Value = "btnSaveTemp";
                if (SubmitData())
                {
                    Presenter.UpdateDocumentStatus();
                    DataEntryTrackingContract dataEntryTimeTracking = null;
                    dataEntryTimeTracking = (DataEntryTrackingContract)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.DATA_ENTRY_TRACKING);
                    if (dataEntryTimeTracking.IsNotNull())
                    {
                        dataEntryTimeTracking.AffectedItemIds = new List<Int32>();
                        dataEntryTimeTracking.AffectedItemIds.AddRange(lstImpactedIds);
                        dataEntryTimeTracking.ItemImpacted = ImpactedItemCnt;
                    }

                    base.LogDataEntry(String.Format("SAVE AND RETURN TO QUEUE: Data Saved and Moving Back to Queue, for TenantID: {0}, SubscriptionId: {1}, DocumentId: {2}",
                                       CurrentViewContext.TenantId,
                                       CurrentViewContext.PkgSubId,
                                       this.DocumentId)
                                       );

                    RoutePageBack(true);
                }
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
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Data is saved and Next document related information is laoded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //UAT-2742
                CurrentViewContext.lstPkgSubForDocDiscard = new List<Int32>();
                Session["lstPkgSubForDocDiscard"] = null;
                CurrentViewContext.IsAnySubsForSaveAndDone = true;

                hdnIsDiscardDocument.Value = "";

                hdnButtonClicked.Value = "btnSaveDone";
                if (SubmitData())
                {
                    MoveToNextRecord();
                }
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
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Back to Queue navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RoutePageBack(false);
        }

        protected void btnRedirect_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnDocChanged.Value == "1")
                {
                    //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
                    RedirectToNextDocument(hdnSelectedTenantID.Value, hdnPackageSubscriptionID.Value, Convert.ToString(NextRecord.ApplicantDocumentID)
                                           , Convert.ToString(NextRecord.FDEQ_ID), Convert.ToInt32(hdnApplicantUserID.Value), Convert.ToString(NextRecord.DiscardDocumentCount));

                }
                else
                {
                    //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
                    RedirectToNextDocument(hdnSelectedTenantID.Value, hdnPackageSubscriptionID.Value, hdnApplicantDocumentID.Value
                                           , hdnFDEQ_ID.Value, Convert.ToInt32(hdnApplicantUserID.Value), hdnDiscardDocumentCount.Value);
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
        /// Event to Discard the document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDiscard_Click(object sender, EventArgs e)
        {

            //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
            if (DiscardDocumentCount > AppConsts.NONE)
            {
                CurrentViewContext.DocumentStatus = DataEntryDocumentStatus.DOCUMENT_REJECTED.GetStringValue();
                Presenter.UpdateDocumentStatus();
                //Production Issue [26/12/2016]
                Presenter.IsDiscardDocumentEmailNeedToSend();
                if (CurrentViewContext.IsDiscardDocumentEmailNeedToSend)
                {
                    Presenter.SendDocumentRejectionEmail();
                }
                Presenter.UpdateDiscardDocumentCount(false);
            }
            else
            {
                Presenter.UpdateDiscardDocumentCount(true);
            }
            //UpdateDocumentTrackForDiscardDocument(CurrentViewContext.DocumentStatus);
            NavigateNextDocument(true);

        }

        protected void btnDiscard_Click1(object sender, EventArgs e)
        {
            // Get all the associated subscriptions for current document applicant
            var _lstCrntApplicantSubscriptions = Presenter.GetPackageSubscriptionOfApplicant(CurrentViewContext.TenantId, CurrentViewContext.ApplicantId);

            if (CurrentViewContext.lstPkgSubForDocDiscard.IsNullOrEmpty())
                CurrentViewContext.lstPkgSubForDocDiscard = new List<Int32>();

            CurrentViewContext.lstPkgSubForDocDiscard.Add(CurrentViewContext.PkgSubId);

            // Remove current subscription from the list.
            _lstCrntApplicantSubscriptions = _lstCrntApplicantSubscriptions.Where(con => !CurrentViewContext.lstPkgSubForDocDiscard.Contains(con.PackageSubscriptionID)).ToList();

            // Remove current subscription from the list.
            //  _lstCrntApplicantSubscriptions = _lstCrntApplicantSubscriptions.Where(sub => sub.PackageSubscriptionID != CurrentViewContext.PkgSubId).ToList();

            #region Update Document status and Enter Time Tracking

            CurrentViewContext.DocumentStatus = _lstCrntApplicantSubscriptions.IsNullOrEmpty()
                                                ? DataEntryDocumentStatus.DOCUMENT_REJECTED.GetStringValue()
                                                : DataEntryDocumentStatus.IN_PROGRESS.GetStringValue();

            //Presenter.UpdateDocumentStatus();

            //DataEntryTrackingContract dataEntryTimeTracking = null;
            //dataEntryTimeTracking = (DataEntryTrackingContract)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.DATA_ENTRY_TRACKING);
            //if (dataEntryTimeTracking.IsNotNull())
            //{
            //    dataEntryTimeTracking.ItemImpacted = ImpactedItemCnt;
            //    dataEntryTimeTracking.StatusId = Presenter.GetDocumentStatusIdByCode(DocumentStatus);

            //    dataEntryTimeTracking.AffectedItemIds = new List<Int32>();
            //    dataEntryTimeTracking.AffectedItemIds.AddRange(lstImpactedIds);
            //}
            #endregion

            if (CurrentViewContext.DocumentStatus == DataEntryDocumentStatus.DOCUMENT_REJECTED.GetStringValue())
            {
                CurrentViewContext.lstPkgSubForDocDiscard = new List<Int32>();
                Session["lstPkgSubForDocDiscard"] = null;
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenDocumentDiscardReasonPopup();", true);
                // NavigateNextDocument(false);
            }
            else
            {

                hdnCrntFdeqId.Value = Convert.ToString(CurrentViewContext.FDEQ_ID);
                hdnPackageSubscriptionID.Value = Convert.ToString(CurrentViewContext.PkgSubId);
                hdnDocumentId.Value = Convert.ToString(CurrentViewContext.DocumentId);
                if (CurrentViewContext.IsAnySubsForSaveAndDone)
                {
                    hdnIsDiscardDocument.Value = Convert.ToString(false);
                }
                else
                {
                    hdnIsDiscardDocument.Value = Convert.ToString(true);
                }
                SetParameters(CurrentViewContext.TenantId, CurrentViewContext.DocumentId, CurrentViewContext.ApplicantId, CurrentViewContext.FDEQ_ID, true, DiscardDocumentCount);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenMutlipleSubscriptionsPopup();", true);
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates the main table, header TR, Category name TD, Item name TD 
        /// and loads the Item level control
        /// </summary>
        private void GenerateForm()
        {

            var _disCat = CurrentViewContext.UIContract
                                   .GroupBy(grp => grp.CatId)
                                   .Select(sel => sel.First())
                                   .OrderBy(ord => ord.CatDisplayOrder).ThenBy(ord => ord.CatName).ThenBy(ord => ord.CatId)
                                   .ToList();

            foreach (var cat in _disCat)
            {
                var _items = CurrentViewContext.UIContract.Where(uic => uic.CatId == cat.CatId && !uic.IsItemSeries).ToList();
                var _itemsSeries = CurrentViewContext.UIContract.Where(uic => uic.CatId == cat.CatId && uic.IsItemSeries).ToList();

                //UAT-1642:WB: Admin Data Entry to only function for ADB Review and Joint Review
                Boolean ifAnyItemHavingAdminReview = _items.Any(cond => cond.IsReviewerTypeAdmin);
                Boolean ifAnySeriesHavingAdminReview = _itemsSeries.Any(cond => cond.IsReviewerTypeAdmin);
                if (!ifAnyItemHavingAdminReview && !ifAnySeriesHavingAdminReview)
                {
                    var recordsToBeRemoved = CurrentViewContext.UIContract.Where(cond => cond.CatId == cat.CatId);
                    CurrentViewContext.UIContract = CurrentViewContext.UIContract.Except(recordsToBeRemoved).ToList();
                }

                else
                {
                    if (ifAnySeriesHavingAdminReview)
                    {
                        foreach (var item in _itemsSeries)
                        {
                            item.IsReviewerTypeAdmin = true;
                        }
                    }

                    var itemsTobeRemoved = _items.Where(cond => !cond.IsReviewerTypeAdmin);
                    var itemsSeriesTobeRemoved = _itemsSeries.Where(cond => !cond.IsReviewerTypeAdmin);

                    foreach (AdminDataEntryUIContract contract in itemsTobeRemoved)
                    {
                        CurrentViewContext.UIContract.Remove(contract);
                    }
                    foreach (AdminDataEntryUIContract contract in itemsSeriesTobeRemoved)
                    {
                        CurrentViewContext.UIContract.Remove(contract);
                    }
                }

                //UAt-2128: Document association at time of upload for Chamberlain
                Boolean ifAnySeriesHavingDataEntryallowed = _itemsSeries.Any(cond => cond.ifDataEntryAllowed);
                if (ifAnySeriesHavingDataEntryallowed)
                {
                    foreach (var item in _itemsSeries)
                    {
                        item.ifDataEntryAllowed = true;
                    }
                }
            }

            // Add the Grouped distinct Attributes to the List
            var _distinctAttributes = CurrentViewContext.UIContract
                                        .Where(x => x.IsGrouped && x.AttrDataType != ComplianceAttributeDatatypes.FileUpload.GetStringValue()
                                                                && x.AttrDataType != ComplianceAttributeDatatypes.Screening_Document.GetStringValue())
                                        .GroupBy(grp => grp.AttrGroupId)
                                        .Select(sel => sel.First())
                                        .OrderByDescending(ord => ord.IsGrouped).ThenBy(ord => ord.AttrGroupId).ThenBy(ord => ord.AttrName).ThenBy(ord => ord.AttrId)
                                        .ToList();

            // Add the Non-Grouped distinct Attributes to the List
            _distinctAttributes.AddRange(CurrentViewContext.UIContract
                                        .Where(x => !x.IsGrouped && x.AttrDataType != ComplianceAttributeDatatypes.FileUpload.GetStringValue()
                                                                 && x.AttrDataType != ComplianceAttributeDatatypes.Screening_Document.GetStringValue())
                                        .GroupBy(grp => grp.AttrId)
                                        .Select(sel => sel.First())
                                        .OrderByDescending(ord => ord.AttrId).ThenBy(ord => ord.AttrName)
                                        .ToList());



            Dictionary<String, String> _dicAttributes = new Dictionary<String, String>();

            _dicAttributes.Add("width", "100%");
            _dicAttributes.Add("border", "0");
            _dicAttributes.Add("cellspacing", "0");
            _dicAttributes.Add("cellpadding", "0");
            _dicAttributes.Add("id", "tblForm");
            var _table = GenerateHTML("table", _dicAttributes, String.Empty);

            _dicAttributes = new Dictionary<String, String>();
            var _headerRow = GenerateHTML("tr", _dicAttributes, String.Empty);
            //new HtmlGenericControl("tr");


            _dicAttributes.Add("bgcolor", "#C6C5C5");
            _dicAttributes.Add("class", "borderB borderR");
            var _headerCat = GenerateHTML("td", _dicAttributes, "Category");
            var _headerChkBoxSwap = GenerateHTML("td", _dicAttributes, "Swap Items");
            var _headerChkBoxDocAssociation = GenerateHTML("td", _dicAttributes, "Update Item");
            var _headerItem = GenerateHTML("td", _dicAttributes, "Item");
            var _headerItemStatus = GenerateHTML("td", _dicAttributes, "Item Status");


            _headerRow.Controls.Add(_headerCat);
            _headerRow.Controls.Add(_headerChkBoxSwap);
            _headerRow.Controls.Add(_headerChkBoxDocAssociation);
            _headerRow.Controls.Add(_headerItem);
            _headerRow.Controls.Add(_headerItemStatus);

            foreach (var attHeader in _distinctAttributes)
            {
                _dicAttributes = new Dictionary<String, String>();
                var _headerCol = new HtmlGenericControl("td");
                _headerCol.Attributes.Add("bgcolor", "#C6C5C5");
                _headerCol.Attributes.Add("class", "borderB borderR");

                if (attHeader.IsGrouped)
                    _headerCol.InnerText = attHeader.AttrGroupName;
                else
                    _headerCol.InnerText = attHeader.AttrName;

                _headerRow.Controls.Add(_headerCol);
            }

            _table.Controls.Add(_headerRow);

            _disCat = CurrentViewContext.UIContract
                                 .GroupBy(grp => grp.CatId)
                                 .Select(sel => sel.First())
                                 .OrderBy(ord => ord.CatDisplayOrder).ThenBy(ord => ord.CatName).ThenBy(ord => ord.CatId)
                                 .ToList();

            Boolean ifDataEntryallowedCheckNeedToBeApplied = CurrentViewContext.UIContract.Any(cond => cond.ifDataEntryAllowed)
                                                            || CurrentViewContext.UIContract.Any(cond => cond.ifCatExceptionMapped);

            foreach (var cat in _disCat)
            {
                var _items = CurrentViewContext.UIContract.Where(uic => uic.CatId == cat.CatId && !uic.IsItemSeries)
                                                .GroupBy(uic => uic.ItemId)
                                                .Select(sel => sel.First())
                                                .OrderBy(ord => ord.ItemDisplayOrder).ThenBy(ord => ord.ItemName).ThenBy(ord => ord.ItemId)
                                                .ToList();
                var _itemsSeries = CurrentViewContext.UIContract.Where(uic => uic.CatId == cat.CatId && uic.IsItemSeries)
                                               .GroupBy(uic => uic.ItemId)
                                               .Select(sel => sel.First())
                                               .OrderBy(ord => ord.ItemDisplayOrder).ThenBy(ord => ord.ItemName).ThenBy(ord => ord.ItemId)
                                               .ToList();

                if (_items.IsNullOrEmpty())
                    _items = new List<AdminDataEntryUIContract>();
                if (!_itemsSeries.IsNullOrEmpty())
                    _items.AddRange(_itemsSeries);

                //Remove those items from the list which are having only client review.

                var _rowspnCount = _items.Where(cond => cond.IsReviewerTypeAdmin).Count();
                var _isCatAdded = false;
                if (_rowspnCount > AppConsts.NONE)
                {
                    foreach (var itemData in _items.Where(cond => cond.IsReviewerTypeAdmin))
                    {
                        _dicAttributes = new Dictionary<String, String>();

                        if (itemData.IsItemSeries)
                        {
                            _dicAttributes.Add("cattr", "cattr_" + cat.CatId + "_" + itemData.ItemId + "_" + itemData.ItemSeriesID);
                        }
                        else
                        {
                            _dicAttributes.Add("cattr", "cattr_" + cat.CatId + "_" + itemData.ItemId);
                        }

                        var _itemRow = GenerateHTML("tr", _dicAttributes, String.Empty);

                        if (!_isCatAdded)
                        {
                            if (!cat.CatExplanatoryNotes.IsNullOrEmpty())
                            {
                                _rowspnCount = _rowspnCount + 1;
                            }
                            _dicAttributes = new Dictionary<String, String>();
                            //UAT-1540:expose the compliance rules on the admin Data Entry Screen
                            var _itemRowCatNotes = GenerateHTML("tr", _dicAttributes, String.Empty);

                            _dicAttributes.Add("rowspan", Convert.ToString(_rowspnCount));
                            _dicAttributes.Add("valign", "middle");
                            _dicAttributes.Add("class", "borderB borderR");
                            var _catCol = GenerateHTML("td", _dicAttributes, cat.CatName);
                            //UAT 2591:Indicator for items that have expiration rules on the Admin Data Entry screen
                            if (cat.ifCatComplianceRule)
                            {
                                var span = new HtmlGenericControl("span");
                                span.InnerHtml = " (TC) ";
                                span.Attributes.Add("title", "This category has schedule compliance rule.");
                                span.Style.Add("color", "Blue");
                                span.Style.Add("font-weight", "bold");
                                _catCol.Controls.Add(span);
                            }
                            //UAT-1540:expose the compliance rules on the admin Data Entry Screen
                            if (!cat.CatExplanatoryNotes.IsNullOrEmpty())
                            {
                                var attrCount = _distinctAttributes.Count() + 4;
                                _dicAttributes = new Dictionary<String, String>();
                                _dicAttributes.Add("class", "borderB borderR colTextAlign bullet");
                                _dicAttributes.Add("colspan", Convert.ToString(attrCount));

                                var _catNotesCol = GenerateHTML("td", _dicAttributes, String.Empty);

                                LiteralControl lit = new LiteralControl();
                                lit.Text = cat.CatExplanatoryNotes;
                                _catNotesCol.Controls.Add(lit);
                                _catCol.Controls.Add(_catNotesCol);

                                _itemRowCatNotes.Controls.Add(_catCol);
                                _table.Controls.Add(_itemRowCatNotes);
                            }
                            else
                            {
                                _itemRow.Controls.Add(_catCol);
                            }
                            _isCatAdded = true;
                        }

                        System.Web.UI.Control _itemControl = Page.LoadControl("~/ComplianceOperations/UserControl/DataEntryItem.ascx");
                        //UAT-1608
                        (_itemControl as DataEntryItem).ItemUIContract = CurrentViewContext.UIContract.Where(uic => uic.CatId == cat.CatId && uic.IsItemSeries == itemData.IsItemSeries
                                                                                                   && uic.ItemId == itemData.ItemId).ToList();
                        (_itemControl as DataEntryItem).DistinctAttributes = _distinctAttributes;
                        (_itemControl as DataEntryItem).CatId = cat.CatId;
                        (_itemControl as DataEntryItem).ItemId = itemData.ItemId;
                        (_itemControl as DataEntryItem).ItemName = itemData.ItemName;
                        (_itemControl as DataEntryItem).IfItemExpiryRule = itemData.ifItemExpiryRule;
                        (_itemControl as DataEntryItem).ItemSeriesId = itemData.ItemSeriesID;
                        if (ifDataEntryallowedCheckNeedToBeApplied)
                            (_itemControl as DataEntryItem).ifDataEntryAllowed = itemData.ifDataEntryAllowed;
                        else
                            (_itemControl as DataEntryItem).ifDataEntryAllowed = true;
                        ////UAT-1608
                        if (itemData.IsItemSeries)
                        {
                            (_itemControl as DataEntryItem).ID = "ucDataEntryItem_" + cat.CatId + "_" + itemData.ItemId + "_" + itemData.ItemSeriesID;
                        }
                        else
                        {
                            (_itemControl as DataEntryItem).ID = "ucDataEntryItem_" + cat.CatId + "_" + itemData.ItemId;
                        }
                        _itemRow.Controls.Add(_itemControl);
                        _table.Controls.Add(_itemRow);
                    }
                }
            }

            pnl.Controls.Add(_table);
        }

        /// <summary>
        /// Generate HTML type control
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dicAttributes"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        private HtmlGenericControl GenerateHTML(String type, Dictionary<String, String> dicAttributes, String innerText)
        {
            var _htmlControl = new HtmlGenericControl(type);

            foreach (var attr in dicAttributes)
            {
                _htmlControl.Attributes.Add(attr.Key, attr.Value);
            }
            _htmlControl.InnerText = innerText;
            return _htmlControl;
        }

        /// <summary>
        /// Save/Update the Form data to Database
        /// </summary>
        private Boolean SubmitData()
        {
            lstImpactedIds = new List<int>();

            var _dataToSave = new AdminDataEntrySaveContract();
            _dataToSave.PackageSubscriptionId = CurrentViewContext.PkgSubId;
            _dataToSave.PackageId = CurrentViewContext.UIContract.First().PkgId;
            _dataToSave.DoccumentId = this.DocumentId;

            _dataToSave.ApplicantCmplncCategoryData = new List<ApplicantCmplncCategoryData>();

            var _controlCount = pnl.Controls.Count;

            var _lstCategories = CurrentViewContext.UIContract
                                            .Where(cond => !cond.IsItemSeries)
                                            .GroupBy(grp => grp.CatId)
                                            .Select(sel => sel.First())
                                            .ToList();

            foreach (var cat in _lstCategories)
            {
                _dataToSave.ApplicantCmplncCategoryData.Add(new ApplicantCmplncCategoryData
                {
                    AccdId = cat.CatDataId,
                    CatId = cat.CatId,
                    OldCategoryStatusCode = cat.OldCategoryStatusCode
                });
                //Added a check to ignore item series or series item
                //UAT-1608:Admin data entry screen [Shot Series]
                var _lstItems = CurrentViewContext.UIContract.Where(uic => uic.CatId == cat.CatId && !uic.IsReadOnly && !uic.IsItemSeries)
                                                    .GroupBy(grp => grp.ItemId)
                                                    .Select(sel => sel.First()).ToList();

                foreach (var item in _lstItems)
                {
                    var _currentControl = pnl.FindServerControlRecursively("ucDataEntryItem_" + cat.CatId + "_" + item.ItemId);

                    if (_currentControl.IsNotNull() && _currentControl is DataEntryItem)
                    {
                        var _itemControl = _currentControl as DataEntryItem;
                        var _catId = Convert.ToInt32((_itemControl.FindControl("hdfCatId") as HiddenField).Value);
                        SetCategoryData(_dataToSave, _catId, _itemControl);
                    }
                }
            }
            CurrentViewContext.SaveContract = _dataToSave;
            GetItemSeriesSaveContract();


            Boolean isSuccess = true;
            if (hdnOverRideUiRule.Value.IsNullOrEmpty())
            {
                isSuccess = EvaluateDataEntryUIRules();
            }

            if (isSuccess)
            {
                Boolean returnStatus = Presenter.SubmitAdminDataEntry();

                #region UAT-1608:

                //Method to save series data.
                SubmitSeriesData();

                #endregion

                Presenter.ExecuteAdjustItemSeriesRuleProcedure();

                hdnOverRideUiRule.Value = String.Empty;
                hdnButtonClicked.Value = String.Empty;
                CurrentViewContext.lstRuleVoilatedItem = null;
                return returnStatus;
            }
            else
            {
                return false;
            }


        }

        private Boolean EvaluateDataEntryUIRules()
        {
            AdminDataEntrySaveContract nonSeriesDataToSave = CurrentViewContext.SaveContract;
            Int32 packageSubscriptionID = nonSeriesDataToSave.PackageSubscriptionId;

            XmlDocument docNonSeriesData = new XmlDocument();
            XmlElement rootNode = (XmlElement)docNonSeriesData.AppendChild(docNonSeriesData.CreateElement("NonSeriesData"));
            foreach (ApplicantCmplncCategoryData categoryData in nonSeriesDataToSave.ApplicantCmplncCategoryData)
            {
                if (!categoryData.ApplicantCmplncItemData.IsNullOrEmpty())
                {
                    XmlNode categoryNode = rootNode.AppendChild(docNonSeriesData.CreateElement("Category"));
                    categoryNode.AppendChild(docNonSeriesData.CreateElement("ID")).InnerText = categoryData.CatId.ToString();
                    foreach (ApplicantCmplncItemData itemData in categoryData.ApplicantCmplncItemData)
                    {
                        XmlNode itemNode = categoryNode.AppendChild(docNonSeriesData.CreateElement("Item"));
                        itemNode.AppendChild(docNonSeriesData.CreateElement("ID")).InnerText = itemData.ItmId.ToString();
                        XmlNode attributesNode = itemNode.AppendChild(docNonSeriesData.CreateElement("Attributes"));
                        foreach (ApplicantCmplncAttrData attributeData in itemData.ApplicantCmplncAttrData)
                        {
                            XmlNode attributeNode = attributesNode.AppendChild(docNonSeriesData.CreateElement("Attribute"));
                            attributeNode.AppendChild(docNonSeriesData.CreateElement("ID")).InnerText = attributeData.AttrId.ToString();
                            attributeNode.AppendChild(docNonSeriesData.CreateElement("Value")).InnerText = attributeData.AttrValue;
                        }

                        List<AdminDataEntryUIContract> fileUploadDocs = CurrentViewContext.UIContract.Where(cond => cond.ItemId == itemData.ItmId
                                                                                                      && cond.CatId == categoryData.CatId
                                                                                                      && cond.AttrDataType.Trim().ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                                                                                                      .ToList();
                        foreach (AdminDataEntryUIContract attribute in fileUploadDocs)
                        {
                            XmlNode attributeNode = attributesNode.AppendChild(docNonSeriesData.CreateElement("Attribute"));
                            attributeNode.AppendChild(docNonSeriesData.CreateElement("ID")).InnerText = attribute.AttrId.ToString();
                            Int32 value = attribute.AttrValue.IsNullOrEmpty() ? 0 : Convert.ToInt32(attribute.AttrValue);
                            value = attribute.IsCurrentDocAssociated ? value : value + 1;
                            attributeNode.AppendChild(docNonSeriesData.CreateElement("Value")).InnerText = value.ToString();
                        }
                    }
                }
            }

            AdminDataEntrySaveContract seriesDataToSave = CurrentViewContext.ItemSeriesSaveContract;
            XmlDocument docSeriesData = new XmlDocument();
            XmlElement rootNodeSeries = (XmlElement)docSeriesData.AppendChild(docSeriesData.CreateElement("SeriesData"));
            if (!seriesDataToSave.IsNullOrEmpty())
            {
                foreach (ApplicantCmplncCategoryData cat in seriesDataToSave.ApplicantCmplncCategoryData)
                {
                    if (!cat.ApplicantCmplncItemData.IsNullOrEmpty())
                    {
                        List<Int32> lstSeriesIds = new List<Int32>();
                        lstSeriesIds = cat.ApplicantCmplncItemData.Select(col => col.ItemSeriesID).Distinct().ToList();
                        foreach (Int32 seriesID in lstSeriesIds)
                        {
                            XmlNode seriesNode = rootNodeSeries.AppendChild(docSeriesData.CreateElement("Series"));
                            seriesNode.AppendChild(docSeriesData.CreateElement("ID")).InnerText = seriesID.ToString();
                            List<ApplicantCmplncItemData> itemsInSeries = cat.ApplicantCmplncItemData.Where(col => col.ItemSeriesID == seriesID).ToList();
                            XmlNode itemsNode = null;
                            if (!itemsInSeries.IsNullOrEmpty())
                            {
                                itemsNode = seriesNode.AppendChild(docSeriesData.CreateElement("Rows"));
                            }
                            Int32 rowNum = AppConsts.ONE;
                            foreach (ApplicantCmplncItemData item in itemsInSeries)
                            {
                                XmlNode itemNode = itemsNode.AppendChild(docSeriesData.CreateElement("Row"));
                                itemNode.AppendChild(docSeriesData.CreateElement("RowNum")).InnerText = rowNum.ToString();
                                XmlNode attributesNode = null;
                                if (!item.ApplicantCmplncAttrData.IsNullOrEmpty())
                                {
                                    attributesNode = itemNode.AppendChild(docSeriesData.CreateElement("Attributes"));
                                }
                                foreach (ApplicantCmplncAttrData attrData in item.ApplicantCmplncAttrData)
                                {
                                    XmlNode attributeNode = attributesNode.AppendChild(docSeriesData.CreateElement("Attribute"));
                                    attributeNode.AppendChild(docSeriesData.CreateElement("ID")).InnerText = attrData.AttrId.ToString();
                                    attributeNode.AppendChild(docSeriesData.CreateElement("Value")).InnerText = attrData.AttrValue;
                                }

                                List<AdminDataEntryUIContract> fileUploadDocs = CurrentViewContext.UIContract.Where(cond => cond.ItemId == item.ItmId
                                                                                                     && cond.CatId == cat.CatId
                                                                                                     && cond.AttrDataType.Trim().ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                                                                                                     .ToList();
                                foreach (AdminDataEntryUIContract attribute in fileUploadDocs)
                                {
                                    XmlNode attributeNode = attributesNode.AppendChild(docSeriesData.CreateElement("Attribute"));
                                    attributeNode.AppendChild(docSeriesData.CreateElement("ID")).InnerText = attribute.AttrId.ToString();
                                    Int32 value = attribute.AttrValue.IsNullOrEmpty() ? 0 : Convert.ToInt32(attribute.AttrValue);
                                    value = attribute.IsCurrentDocAssociated ? value : value + 1;
                                    attributeNode.AppendChild(docSeriesData.CreateElement("Value")).InnerText = value.ToString();
                                }

                                rowNum++;
                            }
                        }
                    }
                }
            }

            Dictionary<Boolean, String> dicResponse = Presenter.EvaluateDataEntryUIRules(packageSubscriptionID, docNonSeriesData.OuterXml, docSeriesData.OuterXml);
            Boolean isSuccess = !dicResponse.Keys.FirstOrDefault();
            String message = dicResponse.Values.FirstOrDefault();
            if (!isSuccess)
            {
                ShowMessage(message, MessageType.Error);
            }
            return isSuccess;
        }

        private void ShowMessage(String strMessage, MessageType msgType)
        {
            String msgClass = "info";
            switch (msgType)
            {
                case MessageType.Error:
                    msgClass = "error";
                    break;
                case MessageType.Information:
                    msgClass = "info";
                    break;
                case MessageType.SuccessMessage:
                    msgClass = "sucs";
                    break;
                default:
                    msgClass = "info";
                    break;
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlertMessage"
                                                , "$page.showAlertMessageWithTitle('" + HttpUtility.JavaScriptStringEncode(strMessage.ToString(), false) + "','" + msgClass + "',true);", true);
        }

        /// <summary>
        /// Update the Item-data from the Form, into the selected Category record
        /// </summary>
        /// <param name="dataToSave"></param>
        /// <param name="catId"></param>
        /// <param name="itemControl"></param>
        private void SetCategoryData(AdminDataEntrySaveContract dataToSave, Int32 catId, DataEntryItem itemControl)
        {
            var _catData = dataToSave.ApplicantCmplncCategoryData.Where(dts => dts.CatId == catId).First();

            if (_catData.ApplicantCmplncItemData.IsNull())
                _catData.ApplicantCmplncItemData = new List<ApplicantCmplncItemData>();

            // UAT-1722 - Admin Data Entry Screen enable data entry only when confirming with checkbox	
            var _itemData = itemControl.GetItemDataToSave();

            if (_itemData.IsNotNull())
            {
                if (LogAffectedItems())
                {
                    lstImpactedIds.Add(_itemData.ItmId);
                }

                _itemData.IsUiRulesViolate = false;
                if (!hdnOverRideUiRule.Value.IsNullOrEmpty() && !CurrentViewContext.lstRuleVoilatedItem.IsNullOrEmpty())
                {
                    if (CurrentViewContext.lstRuleVoilatedItem.Where(cond => cond.Value == catId && cond.Key == _itemData.ItmId).Any())
                    {
                        _itemData.IsUiRulesViolate = true;
                    }
                }
                _catData.ApplicantCmplncItemData.Add(_itemData);
            }
        }

        /// <summary>
        /// Returns whether the Xml of the Affected Items should be logged in DataEntryTimeTracking or not.
        /// If no setting is found or setting is found and is 'Yes', then log the Xml.
        /// </summary>
        /// <returns></returns>
        private static bool LogAffectedItems()
        {
            return ConfigurationManager.AppSettings[AppConsts.APP_SETTING_LOG_DATA_ENTRY_XML].IsNull()
                 ||
            (ConfigurationManager.AppSettings[AppConsts.APP_SETTING_LOG_DATA_ENTRY_XML].IsNotNull()
            && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_LOG_DATA_ENTRY_XML]))
              ? true
              : false;
        }

        /// <summary>
        /// Gets the data from the Query string and assign to properties
        /// </summary>
        private void CaptureQueryStringData()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            }
            if (args.ContainsKey("TenantId"))
            {
                CurrentViewContext.TenantId = Convert.ToInt32(args["TenantId"]);
            }
            if (args.ContainsKey("PkgSubId"))
            {
                CurrentViewContext.PkgSubId = Convert.ToInt32(args["PkgSubId"]);
            }
            if (args.ContainsKey("AppDocId"))
            {
                this.DocumentId = Convert.ToInt32(args["AppDocId"]);
            }
            if (args.ContainsKey("QueueType"))
            {
                this.QueueType = Convert.ToString(args["QueueType"]);
            }
            if (args.ContainsKey("FDEQ_ID"))
            {
                this.FDEQ_ID = Convert.ToInt32(args["FDEQ_ID"]);
            }
            if (args.ContainsKey("ApplicantId"))
            {
                CurrentViewContext.ApplicantId = Convert.ToInt32(args["ApplicantId"]);
            }

            //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
            if (args.ContainsKey("DiscardDocumentCount"))
            {
                this.DiscardDocumentCount = Convert.ToInt32(args["DiscardDocumentCount"]);
                //Production Issue changes[26/12/2016]
                hdnDiscardDocumentCount.Value = args["DiscardDocumentCount"];
            }
            if (!IsPostBack)
            {
                base.LogDataEntry(String.Format("Data Entry Loaded for TenantID: {0}, SubscriptionId: {1}, DocumentId: {2}",
                                        CurrentViewContext.TenantId,
                                        CurrentViewContext.PkgSubId,
                                        this.DocumentId)
                                        );
            }
            else
            {
                base.LogDataEntry(String.Format(" Post Back for Data Entry Page, for TenantID: {0}, SubscriptionId: {1}, DocumentId: {2}",
                                   CurrentViewContext.TenantId,
                                   CurrentViewContext.PkgSubId,
                                   this.DocumentId)
                                   );
            }
        }

        /// <summary>
        /// To route Page Back
        /// </summary>
        /// <param name="showSuccessMessage"></param>
        private void RoutePageBack(bool showSuccessMessage, String successMessage = null)
        {
            String childControl = String.Empty;
            if (QueueType.ToLower() == DataEntryQueueType.DATA_ENTRY_ASSIGNMENT_QUEUE.GetStringValue().ToLower())
                childControl = ChildControls.DataEntryAssignmentQueue;
            else
                childControl = ChildControls.DataEntryUserWorkQueue;
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                     { "Child", childControl},
                                                                    { "QueueType", QueueType }
                                                                 };

            //if (showSuccessMessage)
            //{
            //    queryString.Add("UpdatedStatus", successMessage);
            //}
            String url = String.Empty;
            url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        public void MoveToNextRecord()
        {
            // Get all the associated subscriptions for current document applicant
            var _lstCrntApplicantSubscriptions = Presenter.GetPackageSubscriptionOfApplicant(CurrentViewContext.TenantId, CurrentViewContext.ApplicantId);

            // Remove current subscription from the list.
            _lstCrntApplicantSubscriptions = _lstCrntApplicantSubscriptions.Where(sub => sub.PackageSubscriptionID != CurrentViewContext.PkgSubId).ToList();

            #region Update Document status and Enter Time Tracking

            CurrentViewContext.DocumentStatus = _lstCrntApplicantSubscriptions.IsNullOrEmpty()
                                                ? DataEntryDocumentStatus.COMPLETE.GetStringValue()
                                                : DataEntryDocumentStatus.IN_PROGRESS.GetStringValue();

            Presenter.UpdateDocumentStatus();

            DataEntryTrackingContract dataEntryTimeTracking = null;
            dataEntryTimeTracking = (DataEntryTrackingContract)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.DATA_ENTRY_TRACKING);
            if (dataEntryTimeTracking.IsNotNull())
            {
                dataEntryTimeTracking.ItemImpacted = ImpactedItemCnt;
                dataEntryTimeTracking.StatusId = Presenter.GetDocumentStatusIdByCode(DocumentStatus);

                dataEntryTimeTracking.AffectedItemIds = new List<Int32>();
                dataEntryTimeTracking.AffectedItemIds.AddRange(lstImpactedIds);
            }

            #endregion

            if (CurrentViewContext.DocumentStatus == DataEntryDocumentStatus.COMPLETE.GetStringValue())
            {
                NavigateNextDocument(false);
            }
            else
            {
                // Update the document status to In-Progress and Open the window 
                // with all subscriptions related to current document, except the current one. 
                // CurrentFdeqId is maintained in 'hdnCrntFdeqId' hidden field
                // CurrentDocumentId is maintained in 
                hdnCrntFdeqId.Value = Convert.ToString(CurrentViewContext.FDEQ_ID);
                hdnPackageSubscriptionID.Value = Convert.ToString(CurrentViewContext.PkgSubId);
                hdnDocumentId.Value = Convert.ToString(CurrentViewContext.DocumentId);

                hdnIsDiscardDocument.Value = Convert.ToString(false);//UAT-2742

                // Handle the case when the last document for a Tenant is opened and multiple subscriptions are available for him
                // In that case, when the Admin selects to continue with any further subscription of same document, 
                // then use the same Current record FDEQ & DocumentID
                //if (NextRecord == null || NextRecord.FDEQ_ID == AppConsts.NONE)
                //{
                //    NextRecord.ApplicantDocumentID = CurrentViewContext.DocumentId;
                //    NextRecord.FDEQ_ID = CurrentViewContext.FDEQ_ID;
                //}

                //SetParameters(CurrentViewContext.TenantId, NextRecord.ApplicantDocumentID, CurrentViewContext.ApplicantId, NextRecord.FDEQ_ID, true);
                //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
                SetParameters(CurrentViewContext.TenantId, CurrentViewContext.DocumentId, CurrentViewContext.ApplicantId, CurrentViewContext.FDEQ_ID, true, DiscardDocumentCount);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenMutlipleSubscriptionsPopup();", true);
            }
        }

        private void NavigateNextDocument(Boolean isDiscardMode)
        {
            var _logMessage = "Document dsicarded ";
            if (NextRecord == null || NextRecord.FDEQ_ID == AppConsts.NONE)
            {
                if (!isDiscardMode)
                {
                    _logMessage = "SAVE & DONE: Data Saved and Moving Back to Queue as no subscription found to load";
                }

                base.LogDataEntry(String.Format(_logMessage + " for TenantID: {0}, SubscriptionId: {1}, DocumentId: {2}",
                                   CurrentViewContext.TenantId,
                                   CurrentViewContext.PkgSubId,
                                   this.DocumentId)
                                   );

                RoutePageBack(false);
            }
            else
            {
                if (!isDiscardMode)
                {
                    _logMessage = "SAVE & DONE: Data Saved and will load new subscription. ";
                }
                base.LogDataEntry(String.Format(_logMessage + "Finished working on TenantID: {0}, SubscriptionId: {1}, DocumentId: {2}",
                                    CurrentViewContext.TenantId,
                                    CurrentViewContext.PkgSubId,
                                    this.DocumentId)
                                    );

                //Get list of package subscription to check the count of subscriptions, If applicant have only one subscription then directly redirect to the data entry 
                //detail page else open the popup window to select package subscription.
                List<PackageSubscriptionForDataEntry> lstNextUserSubscriptions = new List<PackageSubscriptionForDataEntry>();
                lstNextUserSubscriptions = Presenter.GetPackageSubscriptionOfApplicant(NextRecord.TenantID, NextRecord.ApplicantOrganizationUserID);
                //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
                SetParameters(NextRecord.TenantID, NextRecord.ApplicantDocumentID, NextRecord.ApplicantOrganizationUserID, NextRecord.FDEQ_ID, false, NextRecord.DiscardDocumentCount);

                if (!lstNextUserSubscriptions.IsNullOrEmpty() && lstNextUserSubscriptions.Count > AppConsts.NONE)
                {
                    if (lstNextUserSubscriptions.Count == AppConsts.ONE)
                    {
                        Int32 packageSubscriptionID = lstNextUserSubscriptions.First().PackageSubscriptionID;
                        RedirectToNextDocument(NextRecord.TenantID.ToString(), packageSubscriptionID.ToString(), NextRecord.ApplicantDocumentID.ToString(),
                            NextRecord.FDEQ_ID.ToString(), NextRecord.ApplicantOrganizationUserID, NextRecord.DiscardDocumentCount.ToString());
                    }
                    else
                    {
                        hdnIsDiscardDocument.Value = Convert.ToString(false);//UAT-2742
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenMutlipleSubscriptionsPopup();", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowConfimDialog();", true);
                }
            }
        }

        /// <summary>
        /// Method to redirect on data entry detail page 
        /// </summary>
        /// <param name="selectedTenantID">Selected document tenantID</param>
        /// <param name="packageSubscriptionID"> selected package subscription id</param>
        /// <param name="applicantDocumentID">selected applicant document id</param>
        private void RedirectToNextDocument(String selectedTenantID, String packageSubscriptionID, String applicantDocumentID, String FDEQ_Id, Int32 applicantId, String discardDocumentCount)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", selectedTenantID },
                                                                    { "Child", ChildControls.DataEntryViewDetail},
                                                                    { "PkgSubId", packageSubscriptionID },
                                                                    { "AppDocId", applicantDocumentID },
                                                                    { "QueueType", QueueType },
                                                                    { "FDEQ_ID", FDEQ_Id },
                                                                    {"ApplicantId" , Convert.ToString(applicantId)},
                                                                     //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
                                                                    {"DiscardDocumentCount" ,discardDocumentCount},
                                                                 };
            string url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Get filter values from session.
        /// </summary>
        private DataEntryQueueFilterContract GetSessionValues()
        {
            DataEntryQueueFilterContract dataEntryQueueFilters = (DataEntryQueueFilterContract)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_KEY_DATA_ENTRY_QUEUE);
            if (!dataEntryQueueFilters.IsNullOrEmpty())
            {
                if (!dataEntryQueueFilters.QueueType.IsNullOrEmpty())
                {
                    QueueType = dataEntryQueueFilters.QueueType;
                }
                GridCustomPaging.CurrentPageIndex = 1;
                GridCustomPaging.PageSize = dataEntryQueueFilters.VirtualPageCount;
                GridCustomPaging.DefaultSortExpression = dataEntryQueueFilters.DefaultSortExpression;
                GridCustomPaging.SecondarySortExpression = dataEntryQueueFilters.SecondarySortExpression;
                GridCustomPaging.FilterColumns = dataEntryQueueFilters.FilterColumns;
                GridCustomPaging.FilterOperators = dataEntryQueueFilters.FilterOperators;
                GridCustomPaging.FilterValues = dataEntryQueueFilters.FilterValues;
                GridCustomPaging.FilterTypes = dataEntryQueueFilters.FilterTypes;

            }
            return dataEntryQueueFilters;
        }

        /// <summary>
        /// Set hidden fields, based on scenarios
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="applicantDocumentId"></param>
        /// <param name="applicantId"></param>
        /// <param name="fdeqId"></param>
        private void SetParameters(Int32 tenantId, Int32 applicantDocumentId, Int32 applicantId, Int32 fdeqId, Boolean isSameDocument, Int32 discardDocumentCount)
        {
            hdnSelectedTenantID.Value = Convert.ToString(tenantId);
            hdnApplicantDocumentID.Value = Convert.ToString(applicantDocumentId);

            hdnApplicantUserID.Value = Convert.ToString(applicantId);
            hdnFDEQ_ID.Value = Convert.ToString(fdeqId);
            //UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice
            hdnDiscardDocumentCount.Value = Convert.ToString(discardDocumentCount);
            hdnIsSameDocument.Value = Convert.ToString(isSameDocument);

            hdnCurrentUserId.Value = Convert.ToString(CurrentViewContext.CurrentUserId);

            if (isSameDocument)
            {
                hdnNextTenantId.Value = Convert.ToString(NextRecord.TenantID);
                hdnNextApplicantId.Value = Convert.ToString(NextRecord.ApplicantOrganizationUserID);
            }
            else
            {
                hdnNextTenantId.Value = AppConsts.ZERO;
                hdnNextApplicantId.Value = AppConsts.ZERO;
            }
        }

        #region UAT-1608:Admin data entry screen [Shot Series]
        /// <summary>
        /// Save/Update the Form data to Database
        /// </summary>
        private Boolean SubmitSeriesData()
        {
            if (!CurrentViewContext.ItemSeriesSaveContract.IsNullOrEmpty())
            {
                return Presenter.SubmitAdminSeriesDataEntry();
            }
            return true;
        }

        private void GetItemSeriesSaveContract()
        {
            Boolean isSeriesRequiredToSave = false;
            var dataToSave = new AdminDataEntrySaveContract();
            dataToSave.PackageSubscriptionId = CurrentViewContext.PkgSubId;
            dataToSave.PackageId = CurrentViewContext.UIContract.First().PkgId;
            dataToSave.DoccumentId = this.DocumentId;

            dataToSave.ApplicantCmplncCategoryData = new List<ApplicantCmplncCategoryData>();

            var controlCount = pnl.Controls.Count;

            var lstCategories = CurrentViewContext.UIContract
                                            .GroupBy(grp => grp.CatId)
                                            .Select(sel => sel.First())
                                            .ToList();

            foreach (var cat in lstCategories)
            {
                dataToSave.ApplicantCmplncCategoryData.Add(new ApplicantCmplncCategoryData
                {
                    AccdId = cat.CatDataId,
                    CatId = cat.CatId
                });
                //Get Series Items
                var lstItemSeries = CurrentViewContext.UIContract.Where(uic => uic.CatId == cat.CatId && uic.IsItemSeries)
                                                    .GroupBy(grp => grp.ItemId)
                                                    .Select(sel => sel.First()).ToList();
                if (!lstItemSeries.IsNullOrEmpty() && lstItemSeries.Count > AppConsts.NONE)
                {
                    isSeriesRequiredToSave = true;
                }

                foreach (var item in lstItemSeries)
                {
                    var currentControl = pnl.FindServerControlRecursively("ucDataEntryItem_" + cat.CatId + "_" + item.ItemId + "_" + item.ItemSeriesID);

                    if (currentControl.IsNotNull() && currentControl is DataEntryItem)
                    {
                        var itemControl = currentControl as DataEntryItem;
                        var catId = Convert.ToInt32((itemControl.FindControl("hdfCatId") as HiddenField).Value);
                        SetCategoryData(dataToSave, catId, itemControl);
                        SetFileUploadTypeAttributeData(cat.CatId, item, dataToSave);
                    }
                }
            }
            if (isSeriesRequiredToSave)
            {
                CurrentViewContext.ItemSeriesSaveContract = dataToSave;
            }
        }

        private void SetFileUploadTypeAttributeData(Int32 catId, AdminDataEntryUIContract itemUIContractData, AdminDataEntrySaveContract dataToSave)
        {
            String fileUploadAttributeTypeCode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
            var fileUploadAttribute = CurrentViewContext.UIContract.FirstOrDefault(uic => uic.CatId == catId && uic.IsItemSeries
                                                                               && uic.ItemId == itemUIContractData.ItemId
                                                                               && uic.AttrDataType == fileUploadAttributeTypeCode);
            if (!fileUploadAttribute.IsNullOrEmpty())
            {
                var catData = dataToSave.ApplicantCmplncCategoryData.Where(dts => dts.CatId == catId).First();
                if (!catData.IsNullOrEmpty() && !catData.ApplicantCmplncItemData.IsNullOrEmpty())
                {
                    var itemData = catData.ApplicantCmplncItemData.FirstOrDefault(x => x.ItmId == itemUIContractData.ItemId);
                    if (!itemData.IsNullOrEmpty() && (itemData.IsDocAssociationReq || itemData.IsDataChanged))
                    {
                        var attributeData = new ApplicantCmplncAttrData();
                        attributeData.AttrId = Convert.ToInt32(fileUploadAttribute.AttrId);
                        attributeData.AttrTypeCode = fileUploadAttribute.AttrDataType;
                        attributeData.AttrValue = Convert.ToString(AppConsts.ONE);
                        itemData.ApplicantCmplncAttrData.Add(attributeData);
                    }
                }
            }

        }
        private void SetCategoryDataForSeries(AdminDataEntrySaveContract dataToSave, List<AdminDataEntryUIContract> lstItemSeries, AdminDataEntryUIContract cat, Boolean isSeriesData)
        {
            foreach (var item in lstItemSeries)
            {
                var currentControl = isSeriesData ? pnl.FindServerControlRecursively("ucDataEntryItem_" + cat.CatId + "_" + item.ItemId + "_" + item.ItemSeriesID)
                                                   : pnl.FindServerControlRecursively("ucDataEntryItem_" + cat.CatId + "_" + item.ItemId);

                if (currentControl.IsNotNull() && currentControl is DataEntryItem)
                {
                    var itemControl = currentControl as DataEntryItem;
                    var catId = Convert.ToInt32((itemControl.FindControl("hdfCatId") as HiddenField).Value);
                    SetCategoryData(dataToSave, catId, itemControl);
                    if (isSeriesData)
                    {
                        SetFileUploadTypeAttributeData(cat.CatId, item, dataToSave);
                    }
                }
            }
        }
        #endregion

        #region UAT-2456:Admin Data Entry: "Discard Documents" should have to go through the process twice

        private void UpdateDocumentTrackForDiscardDocument(String documentStatus)
        {
            DataEntryTrackingContract dataEntryTimeTracking = null;
            dataEntryTimeTracking = (DataEntryTrackingContract)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.DATA_ENTRY_TRACKING);
            if (dataEntryTimeTracking.IsNotNull())
            {
                dataEntryTimeTracking.ItemImpacted = AppConsts.NONE;
                if (!documentStatus.IsNullOrEmpty())
                {
                    dataEntryTimeTracking.StatusId = Presenter.GetDocumentStatusIdByCode(documentStatus);
                }
                dataEntryTimeTracking.AffectedItemIds = new List<Int32>();
                //dataEntryTimeTracking.DiscardReasonId = CurrentViewContext.SelectedDiscardReasonId;
                //dataEntryTimeTracking.StatusNotes = CurrentViewContext.AdditionalNotes;
                //dataEntryTimeTracking.DiscardReason = CurrentViewContext.DiscardReasonText;
            }
        }
        #endregion
        #endregion

        #endregion
    }
}