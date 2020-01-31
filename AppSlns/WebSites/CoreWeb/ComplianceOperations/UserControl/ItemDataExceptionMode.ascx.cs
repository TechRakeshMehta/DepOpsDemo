using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Collections.Generic;
using System.Web.Configuration;
using System.IO;
using Telerik.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Linq;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ItemDataExceptionMode : BaseUserControl, IItemDataExceptionModeView
    {
        private ItemDataExceptionModePresenter _presenter = new ItemDataExceptionModePresenter();

        #region Properties

        public int CurrentStatusId { get; set; }
        public int NewItemStatusId { get { return _presenter.GetNewStatusId(rbtnActions.SelectedValue); } }


        public List<ApplicantItemVerificationData> VerificationData
        {
            get;
            set;
        }

        public IItemDataExceptionModeView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }
        public Int32 CurrentPackageSubscriptionId
        {
            get { return (Int32)(ViewState["CurrentPackageSubscriptionIdEx"]); }
            set { ViewState["CurrentPackageSubscriptionIdEx"] = value; }
        }
        public Int32 CurrentTenantId_Global
        {
            get
            {
                if (!ViewState["CurrentTenantIdEx"].IsNullOrEmpty())
                    return (Int32)(ViewState["CurrentTenantIdEx"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["CurrentTenantIdEx"].IsNullOrEmpty())
                    ViewState["CurrentTenantIdEx"] = value;
            }
        }
        public Int32 SelectedTenantId_Global
        {
            get
            {
                if (!ViewState["SelectedTenantIdEx"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedTenantIdEx"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedTenantIdEx"].IsNullOrEmpty())
                    ViewState["SelectedTenantIdEx"] = value;
            }
        }

        public List<ApplicantDocuments> lstApplicantDocument
        {
            get
            {
                List<ApplicantDocuments> lstApplicantDocumentObj = new List<ApplicantDocuments>();
                if (ViewState["lstApplicantDocument"].IsNotNull())
                {
                    lstApplicantDocumentObj = (List<INTSOF.UI.Contract.ComplianceOperation.ApplicantDocuments>)ViewState["lstApplicantDocument"];
                }
                return lstApplicantDocumentObj;
            }
            set
            {
                ViewState["lstApplicantDocument"] = ucVerificationDetailsDocumentConrol.lstApplicantDocument = value;
            }
        }

        public Int32 SelectedCompliancePackageId_Global
        {
            get
            {
                if (!ViewState["SelectedCompliancePackageIdEx"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedCompliancePackageIdEx"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedCompliancePackageIdEx"].IsNullOrEmpty())
                    ViewState["SelectedCompliancePackageIdEx"] = value;
            }
        }
        public Int32 SelectedComplianceCategoryId_Global
        {
            get
            {
                if (!ViewState["SelectedComplianceCategoryIdEx"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedComplianceCategoryIdEx"]);
                else
                    return 0;
            }
            set
            {
                ViewState["SelectedComplianceCategoryIdEx"] = value;
            }
        }
        public Int32 SelectedApplicantId_Global
        {
            get
            {
                if (!ViewState["SelectedApplicantIdEx"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedApplicantIdEx"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedApplicantIdEx"].IsNullOrEmpty())
                    ViewState["SelectedApplicantIdEx"] = value;
            }
        }

        public Int32 ApplicantItemDataId
        {
            get;
            set;
        }

        public String StatusCode
        {
            get;
            set;
        }

        public String AttemptedStatusCode
        {
            get { return rbtnActions.SelectedValue; }
        }

        public Int32 AttemptedItemStatusId
        {
            get { return _presenter.GetNewStatusId(rbtnActions.SelectedValue); }
        }

        public String ExceptionComments
        {
            get { return txtVerificationComments.Text; }
        }

        public String AdminComments
        {
            get { return txtAdminNotes.Text; }
        }

        public String Comments
        {
            get;
            set;
        }

        public Int32 MappingId
        {
            get;
            set;
        }

        public Int32 CurrentItemId
        {
            get;
            set;
        }

        public Int32 ExceptionItemIdUpdated
        {
            get;
            set;
        }

        /// <summary>
        /// Contains Assignment Properties of all Items of a Category of a package
        /// </summary>
        public List<ListItemAssignmentProperties> lstAssignmentProperties
        {
            get;
            set;
        }

        public Int32? UnifiedDocumentStartPageID
        {
            get;
            set;
        }

        /// <summary>
        /// Get the Mapped documents from the Loader control and pass on to the Document control
        /// </summary>
        public List<ExceptionDocumentMapping> lstExceptionDocumentDocumentMaps { get; set; }

        /// <summary>
        /// Status of Item when page loads
        /// </summary>
        public String CurrentItemStatusText
        {
            get { return litStatus.Text; }
        }

        public String PackageName
        {
            get;
            set;
        }
        public String ApplicantName
        {
            get
            {
                if (!CurrentViewContext.VerificationData.IsNullOrEmpty())
                    return CurrentViewContext.VerificationData[0].ApplicantName;

                return String.Empty;
            }
        }
        public DateTime SubmissionDate
        {
            get;
            set;
        }
        public Int32 HierarchyNodeId
        {
            get;
            set;
        }
        public String RushOrderStatusText
        {
            get;
            set;
        }
        public String RushOrderStatusCode
        {
            get;
            set;
        }

        /// <summary>
        /// Status of Item selected for Save
        /// </summary>
        public String NewItemStatusText
        {
            get { return rbtnActions.SelectedItem.Text; }
        }

        public String EscalatedCode
        {
            get
            {
                return CurrentViewContext.VerificationData[0].EscalationCode;
            }
        }

        public String CurrentTenantTypeCode { get; set; }

        public Boolean IsDeleteChecked
        {
            get { return chkDeleteItem.Checked; }
        }

        public ItemDataExceptionModePresenter Presenter
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

        public DateTime? ItemExpirationDate { get; set; }
        public String ApprovedWithExcepStatusCode
        {
            get
            {
                return ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
            }
        }

        public String MinExpirationDate { get { return (DateTime.Now.AddDays(1)).ToShortDateString(); } }

        //UAT-1056
        public Boolean IsItemAsisgnedToCurrentUser
        {
            get
            {
                if (!ViewState["IsItemAsisgnedToCurrentUser"].IsNullOrEmpty())
                {
                    return (Boolean)ViewState["IsItemAsisgnedToCurrentUser"];
                }
                return false;
            }
            set
            {
                if (ViewState["IsItemAsisgnedToCurrentUser"].IsNullOrEmpty())
                    ViewState["IsItemAsisgnedToCurrentUser"] = value;
            }
        } 

        public bool IsAdminLoggedIn { get; set; }  //UAT-2807

        public Boolean IsItmEditableByApplcnt { get; set; } //UAT-3599


        #region UAT-3951:Rejection Reason
        public String NotApprovedStatusCode
        {
            get
            {
                return ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue();
            }
        }

        public List<Entity.RejectionReason> ListRejectionReasons { get; set; }

        List<Int32> IItemDataExceptionModeView.SelectedRejectionReasonIds
        {
            get
            {
                if (!hdnSelectedRejectionReasonIDsExcep.Value.IsNullOrEmpty())
                {
                    return hdnSelectedRejectionReasonIDsExcep.Value.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                }
                return null;
            }
        }

        #endregion
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                CurrentViewContext.VerificationData = VerificationData;
            }
            Presenter.OnViewLoaded();

            LoadItemDetails();
            List<ComplianceItem> _lstItems = Presenter.GetAvailableItemsForException();
            _lstItems.ForEach(item => item.ItemLabel = item.ItemLabel.IsNullOrEmpty() ? item.Name : item.ItemLabel);
            cmbItems.DataSource = _lstItems;
            cmbItems.DataValueField = "ComplianceItemID";
            cmbItems.DataTextField = "ItemLabel";
            cmbItems.DataBind();

            cmbItems.SelectedValue = Convert.ToString(CurrentViewContext.CurrentItemId);
            ucVerificationDetailsDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
            ucVerificationDetailsDocumentConrol.ApplicantId = CurrentViewContext.SelectedApplicantId_Global;
            ucVerificationDetailsDocumentConrol.ComplianceItemId = CurrentViewContext.CurrentItemId;
            ucVerificationDetailsDocumentConrol.IsReadOnly = false;
            ucVerificationDetailsDocumentConrol.lstExceptionDocumentDocumentMaps = this.lstExceptionDocumentDocumentMaps;

            //ApplicantItemVerificationData _verificationData = CurrentViewContext.VerificationData
            //    .Where(x => x.ComplianceAttributeId != null).FirstOrDefault();

            //ucVerificationDetailsDocumentConrol.ComplianceAttributeId = Convert.ToInt32(_verificationData.ComplianceAttributeId);
            ucVerificationDetailsDocumentConrol.PackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionId;
            hdfRejectionCodeException.Value = ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue();

            this.CurrentStatusId = !CurrentViewContext.VerificationData[0].ItemComplianceStatusId.IsNullOrEmpty()
                ? Convert.ToInt32(CurrentViewContext.VerificationData[0].ItemComplianceStatusId)
                : AppConsts.NONE;

            String _divType = ComplianceVerificationDetailsContract.GetItemDivType(CurrentViewContext.StatusCode);
            divExceptionMode.Attributes.Add("divType", _divType);

            //UAT- 505 - Is chkDeleteItem visible to delete items
            IsDeleteApplicable();

            //UAT-1056 - Ability to enter data for all categories from user work queue. need visual indicator of item assigned/not assigned
            HighlightAssignedItems();

            //UAT-3951:
            BindRejectionReasons();
            ShowHideRejectionReasonControl();
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            BindItems();
        }

        #endregion

        #region Methods

        private void BindItems()
        {
            rbtnActions.Items.Add(new ListItem { Text = VerificationDataActions.PENDING_REVIEW_EXCEPTION.GetStringValue(), Value = ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue() });
            rbtnActions.Items.Add(new ListItem { Text = VerificationDataActions.APPROVED_EXCEPTION.GetStringValue(), Value = ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue() });
            rbtnActions.Items.Add(new ListItem { Text = VerificationDataActions.DECLINED_EXCEPTION.GetStringValue(), Value = ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue() });
            rbtnActions.Items.Add(new ListItem { Text = VerificationDataActions.EXPIRED.GetStringValue(), Value = ApplicantItemComplianceStatus.Expired.GetStringValue() });//UAT-505 
            rbtnActions.Items.FindByValue(ApplicantItemComplianceStatus.Expired.GetStringValue()).Enabled = false;//.Attributes.Add("Style", "Display:none;")//Disabled the expired status 
        }

        public void RebindItemDataExceptionModeControlsData(List<ApplicantItemVerificationData> applicantItmVerificationData, Boolean isReadOnlyControlsAfterSave)
        {
            txtAdminNotes.Text = String.Empty;
            CurrentViewContext.VerificationData = applicantItmVerificationData;
            LoadItemDetails();
            //UAT- 505 - Is chkDeleteItem visible to delete items
            IsDeleteApplicable();

            if (isReadOnlyControlsAfterSave)
            {
                ucVerificationDetailsDocumentConrol.RebindAfterSave(isReadOnlyControlsAfterSave);
                spnComments.InnerText = "Comments:";
                dvAdminActions.Visible = false;
                dvItemsList.Visible = false;
                //UAT- 505 - Delete Item in Exception mode
                chkDeleteItem.Visible = false;
            }
            
            //UAT-3951
            ShowHideRejectionReasonControl();
        }

        void LoadItemDetails()
        {
            if (VerificationData.IsNotNull())
            {
                IsAdminLoggedIn = (Presenter.IsDefaultTenant); //UAT 2807

                if (CurrentViewContext.VerificationData[0].ItemExpirationDate.IsNullOrEmpty())
                {
                    dpExpirationDate.MinDate = DateTime.Now.AddDays(1); ;//Set min date of Expiration Date.
                }
                else if (Convert.ToDateTime(Convert.ToDateTime(CurrentViewContext.VerificationData[0].ItemExpirationDate).ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                {
                    dpExpirationDate.MinDate = Convert.ToDateTime(CurrentViewContext.VerificationData[0].ItemExpirationDate);
                }
                else
                {
                    dpExpirationDate.MinDate = DateTime.Now.AddDays(1);
                }

                #region UAT:719 Check Exceptions turned off for a Category/Item
                Boolean isExceptionAllowed = Presenter.IsAllowExceptionOnCategory();
                if (isExceptionAllowed)
                {
                    imageExceptionOff.Visible = true;
                }
                #endregion

                #region UAT:3614 Icon on Three Panel Screen if Item is set to approval required: No
                ListItemAssignmentProperties assignmentProperty = this.lstAssignmentProperties.FirstOrDefault();
                imageAutoApprove.Visible = assignmentProperty.ApprovalRequired.HasValue ? assignmentProperty.ApprovalRequired.Value : false;
                #endregion

                #region UAT 725: Add submission time and date to verification details screen for each item
                if (CurrentViewContext.VerificationData[0].SubmissionDate.HasValue && CurrentViewContext.VerificationData[0].SubmissionDate.Value != new DateTime(1900, 1, 1))
                {
                    lblSubmissionDate.Text = (Convert.ToString(CurrentViewContext.VerificationData[0].SubmissionDate)+"(MT)");
                }
                #endregion

                lnkItemName.Attributes.Add("UnifiedDocumentStartPageID", (UnifiedDocumentStartPageID != null ? Convert.ToString(UnifiedDocumentStartPageID) : "0"));

                string sampleDocFormURL = CurrentViewContext.VerificationData[0].SampleDocFormURL;
                string sampleDocFormDisplayUrlLabel = CurrentViewContext.VerificationData[0].SampleDocFormDisplayURLLabel;//"..\\InstitutionImages\\Libertyuniversity\\Jellyfish.jpg";
                string SampleDocLink;
                //Only display hyperlink if sampleDocFromUrl available in VerificationData
                if (!sampleDocFormURL.IsNullOrEmpty())
                {
                    SampleDocLink = "<br /><a href=\"" + sampleDocFormURL + "\" onclick=\"\" target=\"_blank\");'>" +
                        (sampleDocFormDisplayUrlLabel.IsNullOrEmpty()? "View Sample Document" : "View " + sampleDocFormDisplayUrlLabel )+
                        "</a>";
                    //SampleDocLink = "<br /><a href='#' onclick='OpenSampleDocWindow(\"" + VerificationData[0].ItemName + "\");'>View Sample Doc</a>";
                    //ucExplanationDescription.SampleDocUrl = sampleDocFormURL;
                }
                else
                    SampleDocLink = String.Empty;

                CurrentViewContext.StatusCode = CurrentViewContext.VerificationData[0].ItemComplianceStatusCode;
                litItemName.Text = CurrentViewContext.VerificationData[0].ItemName.HtmlEncode();
                if (!IsItmEditableByApplcnt) //UAT-3599
                {
                    imageSDEdisabled.Visible = true;
                }
                litStatus.Text = CurrentViewContext.VerificationData[0].ItemComplianceStatus;
                litExceptionReason.Text = CurrentViewContext.VerificationData[0].ExceptionReason;
                ucExplanationDescription.ApplicantExplanation = CurrentViewContext.VerificationData[0].ItemExpNotes;
                ucExplanationDescription.AdminExplanation = VerificationData[0].ItemDescription + @SampleDocLink;
                hdfApplicantItemDataId.Value = Convert.ToString(CurrentViewContext.VerificationData[0].ApplicantCompItemId);
                hdfOrganizationUserId.Value = Convert.ToString(CurrentViewContext.SelectedApplicantId_Global);
                //UAT 2807
                if (IsAdminLoggedIn)
                {
                    txtVerificationComments.Text = CurrentViewContext.VerificationData[0].VerificationComments;
                }
                else
                {
                    txtVerificationComments.Text = CurrentViewContext.VerificationData[0].VerificationCommentsWithInitials;
                }
                CurrentViewContext.ApplicantItemDataId = Convert.ToInt32(CurrentViewContext.VerificationData[0].ApplicantCompItemId);
                CurrentViewContext.CurrentItemId = CurrentViewContext.VerificationData[0].ComplianceItemId;
                ucVerificationDetailsDocumentConrol.ItemDataId = CurrentViewContext.ApplicantItemDataId;
                rbtnActions.Attributes.Add("exActionItemId", Convert.ToString(CurrentViewContext.VerificationData[0].ComplianceItemId));
                txtAdminNotes.Attributes.Add("exNoteItemId", Convert.ToString(CurrentViewContext.VerificationData[0].ComplianceItemId));
                txtAdminNotes.Attributes.Add("exCurrentStatus", Convert.ToString(CurrentViewContext.VerificationData[0].ItemComplianceStatusCode));
                dpExpirationDate.SelectedDate = CurrentViewContext.VerificationData[0].ItemExpirationDate;//UAT-519 Added expiration date control and set expiration date of item.
                dpExpiDateReadOnly.SelectedDate = CurrentViewContext.VerificationData[0].ItemExpirationDate;
                //UAT-519 Added expiration date control and set expiration date of item.
                hdnExpirationDateDB.Value = CurrentViewContext.VerificationData[0].ItemExpirationDate.IsNullOrEmpty() ? String.Empty : Convert.ToString(CurrentViewContext.VerificationData[0].ItemExpirationDate);
                rbtnActions.SelectedValue = hdfItemExceptionStatus.Value = CurrentViewContext.VerificationData[0].ItemComplianceStatusCode;
                if (rbtnActions.SelectedValue == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue())
                {
                    divExpirationDate.Style["Display"] = "block";
                    divExpiDateReadOnly.Style["Display"] = "none";
                }
                else if (rbtnActions.SelectedValue == ApplicantItemComplianceStatus.Expired.GetStringValue())
                {
                    divExpiDateReadOnly.Style["Display"] = "block";
                    divExpirationDate.Style["Display"] = "none";
                }
                else
                {
                    dpExpirationDate.SelectedDate = null;
                    divExpirationDate.Style["Display"] = "none";
                    divExpiDateReadOnly.Style["Display"] = "none";
                }
                divExpirationDate.ID = "divExpirationDate";
                dpExpirationDate.ID = "dpExpirationDate";
                hdnExpirationDateDB.ID = "hdnExpirationDateDB";
                divExpiDateReadOnly.ID = "divExpiDateReadOnly";
                divExpirationDate.ID += Convert.ToString(CurrentViewContext.VerificationData[0].ComplianceItemId);
                dpExpirationDate.ID += Convert.ToString(CurrentViewContext.VerificationData[0].ComplianceItemId);
                hdnExpirationDateDB.ID += Convert.ToString(CurrentViewContext.VerificationData[0].ComplianceItemId);
                divExpiDateReadOnly.ID += Convert.ToString(CurrentViewContext.VerificationData[0].ComplianceItemId);

                #region UAT-722
                String unifiedDocPageMapping = String.Empty;
                String appDocumentIds = String.Empty;
                var currentItemUnifiedDocList = lstApplicantDocument.Where(cond => cond.ComplianceItemID == CurrentViewContext.VerificationData[0].ComplianceItemId).ToList();
                if (currentItemUnifiedDocList.IsNotNull() && currentItemUnifiedDocList.Count() > 0)
                {
                    Int16 counter = 0;
                    foreach (var record in currentItemUnifiedDocList)
                    {
                        if (counter == AppConsts.NONE)
                        {
                            unifiedDocPageMapping = record.UnifiedDocumentStartPageID + "-" + record.UnifiedDocumentEndPageID;
                        }
                        else
                        {
                            unifiedDocPageMapping = unifiedDocPageMapping + "," + record.UnifiedDocumentStartPageID + "-" + record.UnifiedDocumentEndPageID;
                        }
                        appDocumentIds = appDocumentIds + "," + record.ApplicantDocumentId;
                        counter++;
                    }
                }
                lnkItemName.Attributes.Add("UnifiedDocPageMapping", Convert.ToString(unifiedDocPageMapping));
                //UAT-1538:
                lnkItemName.Attributes.Add("appDocumentIds", Convert.ToString(appDocumentIds));
                #endregion
            }
        }

        public String Save(String recordActionType)
        {
            try
            {
                if (!String.IsNullOrEmpty(rbtnActions.SelectedValue))
                {
                    if (chkDeleteItem.Checked == true)
                    {
                        _presenter.DeleteApplicantItemAttrData();
                        ucVerificationDetailsDocumentConrol.UpdateNewDocumentList();
                        HttpContext.Current.Items["RefreshPage"] = 1;
                        return String.Empty;
                    }
                    else
                    {
                        CurrentViewContext.StatusCode = rbtnActions.SelectedValue;

                        if (CurrentViewContext.StatusCode.IsNotNull() && CurrentViewContext.StatusCode != ApplicantItemComplianceStatus.Pending_Review.GetStringValue())
                        {
                            CurrentViewContext.Comments = txtAdminNotes.Text;
                            CurrentViewContext.ExceptionItemIdUpdated = Convert.ToInt32(cmbItems.SelectedValue);
                            if (!dpExpirationDate.SelectedDate.IsNullOrEmpty())
                            {
                                CurrentViewContext.ItemExpirationDate = dpExpirationDate.SelectedDate;
                            }

                            if (
                                (hdfItemExceptionStatus.Value != ApplicantItemComplianceStatus.Expired.GetStringValue() && rbtnActions.SelectedValue == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue() && CurrentViewContext.ItemExpirationDate.IsNotNull() && !hdnExpirationDateDB.Value.IsNullOrEmpty()
                                 && Convert.ToDateTime(CurrentViewContext.ItemExpirationDate).ToShortDateString() != Convert.ToDateTime(hdnExpirationDateDB.Value).ToShortDateString()
                                 && Convert.ToDateTime(Convert.ToDateTime(CurrentViewContext.ItemExpirationDate).ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString())
                                )
                                || (hdfItemExceptionStatus.Value == ApplicantItemComplianceStatus.Expired.GetStringValue() && rbtnActions.SelectedValue == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue() && CurrentViewContext.ItemExpirationDate.IsNotNull() && !hdnExpirationDateDB.Value.IsNullOrEmpty()
                                    && Convert.ToDateTime(Convert.ToDateTime(CurrentViewContext.ItemExpirationDate).ToShortDateString()) < Convert.ToDateTime(DateTime.Now.ToShortDateString())
                                   )
                              )
                            {
                                lblMessage.Text = "Expiration Date should be a future date.";
                                lblMessage.CssClass = "error";
                                return "Expiration Date should be a future date.";
                            }
                            else
                            {
                                _presenter.UpdateAndSaveExceptionItemData(recordActionType);
                            }
                        }

                        return String.Empty;
                    }
                }
                else
                    return String.Empty;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                return litItemName.Text + " could not be saved";
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                return litItemName.Text + " could not be saved";
            }
        }

        protected void rpDocuments_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Trim() == "viewdocument")
            {
            }
            else if (e.CommandName.ToLower().Trim() == "removedocument")
            {
                try
                {
                    CurrentViewContext.MappingId = Convert.ToInt32((e.Item.FindControl("hdf") as HiddenField).Value);
                    Presenter.RemoveExceptionDocumentMapping();
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }
            }
        }

        public void SetSuccessMessage(String sucessMessage)
        {
            lblMessage.Text = sucessMessage;
            lblMessage.CssClass = "sucs";
        }
        public void SetValidationMessage(String sucessMessage)
        {
            lblMessage.Text = sucessMessage;
            lblMessage.CssClass = "error";
        }
        /// <summary>
        /// Is chkDeleteItem visible to delete items
        /// </summary>
        /// <param name="isDeleteApplicable"></param>
        /// <param name="complianceItemId"></param>
        private void IsDeleteApplicable(Boolean isDeleteApplicable = true, Int32 complianceItemId = 0)
        {
            if (isDeleteApplicable)
            {
                //if TenantType is Compliance_Reviewer i.e. Third Party
                if (CurrentTenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue())
                {
                    chkDeleteItem.Visible = false;
                    return;
                }
                chkDeleteItem.Visible = true;
            }
            else
            {
                if (complianceItemId == this.CurrentItemId)
                    chkDeleteItem.Visible = true;
            }
        }

        /// <summary>
        /// Method to Hightlight Assigned items
        /// </summary>
        private void HighlightAssignedItems()
        {
            if (CurrentViewContext.IsItemAsisgnedToCurrentUser)
            {
                dvlnkItemName.Attributes.Add("class", "highlightAssignedItem");
                dvDetailPanel.Attributes.Add("class", "highlightDetailBorder sxform auto");
            }
        }

        #region UAT-2768:
        public Dictionary<String,String> AssignDocument(Boolean isUpdateSessionList,Boolean needToFireUIRules)
        {
            return ucVerificationDetailsDocumentConrol.AssignDocument(isUpdateSessionList, needToFireUIRules,false);
        }
        #endregion

        #endregion 

        #region UAT-3951: Addition of option to use preset ADB Admin rejection notes
        private void BindRejectionReasons()
        {
            cmbRejectionReason.DataSource = CurrentViewContext.ListRejectionReasons;
            cmbRejectionReason.DataBind();
        }

        protected void cmbRejectionReason_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            try
            {
                Entity.RejectionReason rejectionReasonObj = (Entity.RejectionReason)e.Item.DataItem;
                if (!rejectionReasonObj.IsNullOrEmpty())
                {
                    e.Item.Attributes["RR_ReasonText"] = rejectionReasonObj.RR_ReasonText;
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

        private void ShowHideRejectionReasonControl()
        {
            if (IsAdminLoggedIn)
            {
                if (String.Compare(rbtnActions.SelectedValue, NotApprovedStatusCode, true) == AppConsts.NONE)
                {
                    dvRejectionReason.Style["display"] = "block";
                }
                else
                {
                    dvRejectionReason.Style["display"] = "none";
                }
            }
        }
        #endregion

        public Boolean IsRestrictedFileExist()
        {
            HiddenField hdnIsRestrictedFileTypeChecked = ucVerificationDetailsDocumentConrol.FindControl("hdnIsRestrictedFileTypeChecked") as HiddenField;
            if (!hdnIsRestrictedFileTypeChecked.Value.IsNullOrEmpty())
                return Convert.ToBoolean(hdnIsRestrictedFileTypeChecked.Value);
            else
                return false;
        }
        public List<Tuple<int, string, Boolean>> IsRestrictionDocChecked(Boolean isDeleted, Int32 complianceItemId)
        {
            Repeater rptrAllDocuments = ucVerificationDetailsDocumentConrol.FindControl("rptrAllDocuments") as Repeater;

            var tupleList = new List<Tuple<int, string, Boolean>>();

            foreach (RepeaterItem item in rptrAllDocuments.Items)
            {
                CheckBox chkIsMapped = (CheckBox)item.FindControl("chkIsMapped");
                var hdnDocumentId = (HiddenField)item.FindControl("hdnDocumentId");

                if (chkIsMapped.Checked)
                {
                    Label lblErrorMessage = (Label)item.FindControl("lblErrorMessage");
                    if (!lblErrorMessage.Text.IsNullOrEmpty() && !isDeleted)
                    {
                        tupleList.Add(Tuple.Create(complianceItemId, lblErrorMessage.Text, isDeleted));
                    }
                }
            }

            return tupleList;
        }
    }

}


