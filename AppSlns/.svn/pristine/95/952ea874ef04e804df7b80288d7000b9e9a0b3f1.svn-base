using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Collections.Generic;
using System.Linq;

using System.Web.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.UI;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Business.RepoManagers;
namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ReconciliationItemDataReadOnlyMode : BaseUserControl, IReconciliationItemDataReadOnlyModeView
    {
        private ReconciliationItemDataReadOnlyModePrsenter _presenter = new ReconciliationItemDataReadOnlyModePrsenter();
         private int _tenantid;
        public Int32 SelectedTenantId_Global
        {
            get
            {
                if (!ViewState["SelectedTenantIdRO"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedTenantIdRO"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedTenantIdRO"].IsNullOrEmpty())
                    ViewState["SelectedTenantIdRO"] = value;
            }
        }

        public IReconciliationItemDataReadOnlyModeView CurrentViewContext
        {
            get { return this; }
        }

        public List<ApplicantItemVerificationData> VerificationData
        {
            get;
            set;
        }
        public List<ReconciliationDetailsDataContract> lstReconciliationDetailsData { get; set; }
        public String FormMode
        {
            get;
            set;
        }

        public List<ApplicantDocuments> lstApplicantDocument
        {
            get;
            set;
        }

        public Int32? UnifiedDocumentStartPageID
        {
            get;
            set;
        }

        public Int32 CurrentPackageSubscriptionId
        {
            get { return (Int32)(ViewState["CurrentPackageSubscriptionIdRO"]); }
            set { ViewState["CurrentPackageSubscriptionIdRO"] = value; }
        }

        /// <summary>
        /// Get the Mapped documents from the Loader control and pass on to the Document control
        /// </summary>
        public List<ApplicantComplianceDocumentMap> lstApplicantComplianceDocumentMaps
        {
            get;
            set;
        }

        public Int32 TenantId
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
            set { _tenantid = value; }
        }
        public Boolean IsAdminLoggedIn { get; set; }
        //2528
        public Boolean IsUiRulesViolate { get; set; }

        /// <summary>
        /// Contains the Assignment Properties of the current item
        /// </summary>
        public List<ListItemAssignmentProperties> lstAssignmentProperties
        {
            get;
            set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            IsUiRulesViolate = false;

            Presenter.OnViewLoaded();
            
            ManagePanels();
            if (CurrentViewContext.VerificationData.IsNotNull() && CurrentViewContext.VerificationData.Count > 0)
            {
                #region UAT:3614 Icon on Three Panel Screen if Item is set to approval required: No
                IsAdminLoggedIn = (SecurityManager.DefaultTenantID == TenantId);
                if (IsAdminLoggedIn)
                {
                    ListItemAssignmentProperties assignmentProperty = this.lstAssignmentProperties.FirstOrDefault();
                    imageAutoApprove.Visible = assignmentProperty.ApprovalRequired.HasValue ? !(assignmentProperty.ApprovalRequired.Value) : false;
                    //if (CurrentViewContext.VerificationData[0].IsUiRulesViolate)
                    //{
                    //    pnlItem.Attributes.Add("Style", "background-color:#ff6666 !important");
                    //}
                    //else
                    //{
                    //    pnlItem.Attributes.Remove("Style");
                    //}
                }
                #endregion
                string[] sampleDocFormURLs = CurrentViewContext.VerificationData[0].SampleDocFormURL.Split(',');
                string[] sampleDocFormDisplayURLLabels = CurrentViewContext.VerificationData[0].SampleDocFormDisplayURLLabel.Split(',');               
                StringBuilder sb = new StringBuilder();
                string SampleDocLink = string.Empty;
                for (int i = 0; i < sampleDocFormURLs.Count(); i++)
                {
                    if (!sampleDocFormURLs[i].IsNullOrEmpty())
                    {
                        sb.Append("<br /><a href=\"" + sampleDocFormURLs[i] + "\" onclick=\"\" target=\"_blank\");'>" +
                            (sampleDocFormDisplayURLLabels[i].IsNullOrEmpty()? "View Sample Document" : "View "+ sampleDocFormDisplayURLLabels[i]) +"</a>");
                    }
                }
                //foreach (var sampleDocFormURL in sampleDocFormURLs)
                //{
                //    //Only display hyperlink if sampleDocFromUrl available
                //    if (!sampleDocFormURL.IsNullOrEmpty())
                //    {
                //        sb.Append("<br /><a href=\"" + sampleDocFormURL + "\" onclick=\"\" target=\"_blank\");'>View Sample Doc</a>");
                //    }

                //}
                SampleDocLink = sb.ToString();
                ucExplanationDescription.ApplicantExplanation = CurrentViewContext.VerificationData[0].ItemExpNotes;
                ucExplanationDescription.AdminExplanation = CurrentViewContext.VerificationData[0].ItemDescription + @SampleDocLink; ;
                ucReadOnlyMode.PackageSubscriptionId = ucReadOnlyModeException.PackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionId;
                ucReadOnlyMode.lstApplicantDocument = ucReadOnlyModeException.lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                ucReadOnlyMode.SelectedTenantId_Global = ucReadOnlyModeException.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                ucReadOnlyMode.lstApplicantComplianceDocumentMaps = this.lstApplicantComplianceDocumentMaps;
                ItemDescExplanation1.ApplicantExplanation = CurrentViewContext.VerificationData[0].ItemExpNotes;
                ItemDescExplanation1.AdminExplanation = CurrentViewContext.VerificationData[0].ItemDescription + @SampleDocLink;
                ItemDescExplanation2.ApplicantExplanation = CurrentViewContext.VerificationData[0].ItemExpNotes;
                ItemDescExplanation2.AdminExplanation = CurrentViewContext.VerificationData[0].ItemDescription + @SampleDocLink;
            }
        }

        public ReconciliationItemDataReadOnlyModePrsenter Presenter
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

        public void RebindItemDataReadOnlyModeControlsData(List<ApplicantItemVerificationData> applicantItmVerificationData, Boolean isReadOnlyControlsAfterSave)
        {
            txtAdminNotes.Text = String.Empty;
            CurrentViewContext.VerificationData = applicantItmVerificationData;
            ManagePanels();
        }


        private void ManagePanels()
        {

            if (String.IsNullOrEmpty(FormMode))
            {
                

                lnkItemName.Attributes.Add("UnifiedDocumentStartPageID", (UnifiedDocumentStartPageID != null ? Convert.ToString(UnifiedDocumentStartPageID) : "0"));
                litItemName.Text = VerificationData[0].ItemName;
                #region UAT 725: Add submission time and date to verification details screen for each item
                lblItemDataSubmissionDate.Text = Convert.ToString(CurrentViewContext.VerificationData[0].SubmissionDate);

                if (lblItemDataSubmissionDate.Text != "") //UAT-3371
                {
                    lblItemDataSubmissionDate.Text += "(MT)";
                }
                #endregion
                litStatusDataEntered.Text = "Incomplete";
                liteItemNameItemData.Text = VerificationData[0].ItemName;
                txtCommentsDataEntered.Text = CurrentViewContext.VerificationData[0].VerificationComments;
                pnlExceptionData.Visible = false;
                pnlNoItemData.Visible = false;
                pnlItemData.Visible = true;
                List<ApplicantItemVerificationData> lstAttributes = VerificationData
                 .GroupBy(attr => attr.ComplianceAttributeId)
                 .Select(attr => attr.First())
                 .Where(att => att.AttributeTypeCode.IsNotNull() && att.AttributeTypeCode.ToLower() != ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                 .ToList<ApplicantItemVerificationData>();
                rpAttributes.DataSource = lstAttributes;
                rpAttributes.DataBind();
                String _divType = ComplianceVerificationDetailsContract.GetItemDivType(VerificationData[0].ItemComplianceStatusCode);
                pnlNoItemData.Attributes.Add("divType", _divType);
            }
            else if (IsItemDataEntered())
            {
                if (VerificationData[0].ItemComplianceStatusCode.ToLower() == ApplicantItemComplianceStatus.Pending_Review.GetStringValue().ToLower()
               || VerificationData[0].ItemComplianceStatusCode.ToLower() == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue().ToLower()
               || VerificationData[0].ItemComplianceStatusCode.ToLower() == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue().ToLower())
                    litStatusDataEntered.Text = CurrentViewContext.VerificationData[0].ItemComplianceStatusDescription;
                else
                    litStatusDataEntered.Text = CurrentViewContext.VerificationData[0].ItemComplianceStatus;

                #region UAT 725: Add submission time and date to verification details screen for each item
                lblItemDataSubmissionDate.Text = Convert.ToString(CurrentViewContext.VerificationData[0].SubmissionDate);

                if (lblItemDataSubmissionDate.Text != "") //UAT-3371
                {
                    lblItemDataSubmissionDate.Text += "(MT)";
                }
                            
                #endregion

                liteItemNameItemData.Text = VerificationData[0].ItemName;
                txtCommentsDataEntered.Text = CurrentViewContext.VerificationData[0].VerificationComments;
                pnlNoItemData.Visible = false;
                pnlExceptionData.Visible = false;
                pnlItemData.Visible = true;

                List<ApplicantItemVerificationData> lstAttributes = VerificationData.GroupBy(s => s.ApplAttributeDataId)
                  .Select(attr => attr.First())
                  .Where(att => att.ApplAttributeDataId.IsNotNull() && att.AttributeTypeCode.ToLower() != ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower() && att.ApplAttributeDataId.IsNotNull())
                  .ToList<ApplicantItemVerificationData>();

                rpAttributes.DataSource = lstAttributes;
                rpAttributes.DataBind();
                if (CurrentViewContext.VerificationData[0].ApplicantCompItemId.IsNotNull())
                {
                    ucReadOnlyMode.ItemDataId = Convert.ToInt32(CurrentViewContext.VerificationData[0].ApplicantCompItemId);
                    ucReadOnlyMode.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                    ucReadOnlyMode.lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                }
                //data enterer
                String _divType = ComplianceVerificationDetailsContract.GetItemDivType(VerificationData[0].ItemComplianceStatusCode);
                pnlItemData.Attributes.Add("divType", _divType);


                //pnlNoItemData.Attributes.Add("divType", "default");
                //pnlExceptionData.Attributes.Add("divType", "default");
            }
            else if (IsExceptionEntered())
            {
                litItemNameExceptionData.Text = VerificationData[0].ItemName.HtmlEncode();

                #region UAT 725: Add submission time and date to verification details screen for each item
                lblExceptionDataSubmissionDate.Text = Convert.ToString(CurrentViewContext.VerificationData[0].SubmissionDate);

                if (lblExceptionDataSubmissionDate.Text != "") //UAT-3371
                {
                    lblExceptionDataSubmissionDate.Text += "(MT)";
                }
                #endregion

                litStatusExceptionApproved.Text = CurrentViewContext.VerificationData[0].ItemComplianceStatus;
                litExceptionReason.Text = CurrentViewContext.VerificationData[0].ExceptionReason;
                dpExpirationDate.SelectedDate = CurrentViewContext.VerificationData[0].ItemExpirationDate; //UAT-519 Added expiration date control and set expiration date of item.
                txtAdminNotes.Text = CurrentViewContext.VerificationData[0].VerificationComments;
                pnlNoItemData.Visible = false;
                pnlExceptionData.Visible = true;
                pnlItemData.Visible = false;
                if (CurrentViewContext.VerificationData[0].ApplicantCompItemId.IsNotNull())
                {
                    ucReadOnlyModeException.ItemDataId = Convert.ToInt32(CurrentViewContext.VerificationData[0].ApplicantCompItemId);
                    ucReadOnlyModeException.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                    ucReadOnlyModeException.lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                }
                String _divType = ComplianceVerificationDetailsContract.GetItemDivType(VerificationData[0].ItemComplianceStatusCode);
                pnlExceptionData.Attributes.Add("divType", _divType);
                if (CurrentViewContext.VerificationData[0].ItemComplianceStatusCode == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue())
                {
                    divExpirationDate.Style["Display"] = "block";
                }
                else
                {
                    divExpirationDate.Style["Display"] = "none";
                }
            }
        }

        private bool IsExceptionEntered()
        {
            return FormMode.ToLower().Trim() == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue().ToLower()
                            || FormMode.ToLower().Trim() == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue().ToLower()
                            || FormMode.ToLower().Trim() == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue().ToLower();
        }

        private bool IsItemDataEntered()
        {
            return FormMode.ToLower().Trim() == ApplicantItemComplianceStatus.Approved.GetStringValue().ToLower()
                            || FormMode.ToLower().Trim() == ApplicantItemComplianceStatus.Not_Approved.GetStringValue().ToLower()
                            || FormMode.ToLower().Trim() == ApplicantItemComplianceStatus.Expired.GetStringValue().ToLower()
                            || FormMode.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue().ToLower()
                            || FormMode.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue().ToLower()
                            || FormMode.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review.GetStringValue().ToLower();
        }

        public String optionText { get; set; }

        protected void rpAttributes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ApplicantItemVerificationData _attributeData = (ApplicantItemVerificationData)e.Item.DataItem;
            optionText = String.Empty;
            String _dataTypeCode = _attributeData.AttributeTypeCode.ToLower().Trim();
            if (_dataTypeCode == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower().Trim())
            {
                String _optionsCode = ComplianceAttributeDatatypes.Options.GetStringValue();
                var _cmbData = CurrentViewContext.VerificationData
                       .Where(attrOption => attrOption.ComplianceAttributeId == _attributeData.ComplianceAttributeId
                        && attrOption.AttributeTypeCode == _optionsCode).
                  Select(optn => new { optn.OptionText, optn.OptionValue });

                if (_cmbData.Where(v => v.OptionValue == _attributeData.AttributeValue).FirstOrDefault().IsNotNull())
                {
                    optionText = _cmbData.Where(v => v.OptionValue == _attributeData.AttributeValue).FirstOrDefault().OptionText;
                }
                (e.Item.FindControl("litOptionsText") as Literal).Text = optionText.HtmlEncode();
                (e.Item.FindControl("litOptionsText") as Literal).Visible = true;
                (e.Item.FindControl("litAttributeValues") as Literal).Visible = false;
            }
            else
            {
                (e.Item.FindControl("litOptionsText") as Literal).Visible = false;
                (e.Item.FindControl("litAttributeValues") as Literal).Visible = true;
            }

            if (_dataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
            {
                if (!String.IsNullOrEmpty(_attributeData.AttributeValue))
                {
                    (e.Item.FindControl("litAttributeValues") as Literal).Text = Convert.ToDateTime(_attributeData.AttributeValue).ToShortDateString();
                }
            }
            else if (_dataTypeCode == ComplianceAttributeDatatypes.Screening_Document.GetStringValue().ToLower().Trim())
            {
                var _screeningDocTypeCode = DocumentType.SCREENING_DOCUMENT_ATTRIBUTE_TYPE_DOCUMENT.GetStringValue();
                HtmlGenericControl _divScreeningDocuments = (HtmlGenericControl)e.Item.FindControl("divScreeningDocuments");
                var _lstScreeningDocuments = CurrentViewContext.VerificationData.Where(vd => vd.ComplianceAttributeId == _attributeData.ComplianceAttributeId
                                             && vd.ScreeningDocTypeCode == _screeningDocTypeCode).ToList();

                StringBuilder _sbScreeningDocumentLinks = new StringBuilder();
                foreach (var document in _lstScreeningDocuments)
                {
                    LinkButton lnkBtn = new LinkButton();
                    lnkBtn.ID = "lnkBtn_" + _attributeData.ComplianceAttributeId;
                    lnkBtn.OnClientClick = "ViewScreeningDocument(" + CurrentViewContext.SelectedTenantId_Global + ", " + document.ScreeningDocumentId + ");";
                    lnkBtn.Attributes.Add("onclick", "return false;");

                    lnkBtn.Text = document.ScreeningDocumentName;
                    _divScreeningDocuments.Controls.Add(lnkBtn);
                    _divScreeningDocuments.Controls.Add(new LiteralControl("<br />"));
                }
                (e.Item.FindControl("litAttributeValues") as Literal).Visible = false;
            }
        }
    }
}

