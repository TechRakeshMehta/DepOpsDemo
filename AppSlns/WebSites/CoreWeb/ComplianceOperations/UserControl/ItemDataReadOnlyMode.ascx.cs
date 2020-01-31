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
using Business.RepoManagers;
using CoreWeb.Shell;
namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ItemDataReadOnlyMode : BaseUserControl, IItemDataReadOnlyModeView
    {
        private ItemDataReadOnlyModePresenter _presenter = new ItemDataReadOnlyModePresenter();

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

        public IItemDataReadOnlyModeView CurrentViewContext
        {
            get { return this; }
        }

        public List<ApplicantItemVerificationData> VerificationData
        {
            get;
            set;
        }

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
        /// Get the Mapped documents from the Loader control and pass on to the Read Only Document control
        /// </summary>
        public List<ApplicantComplianceDocumentMap> lstApplicantComplianceDocumentMaps
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        public Boolean IsAdminLoggedIn { get; set; }
        //2528
        public Boolean IsUiRulesViolate { get; set; }

        public Boolean IsItmEditableByApplcnt { get; set; } //UAT-3599

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            //UAT 2528
            IsUiRulesViolate = false;
            ManagePanels();

            IsAdminLoggedIn = (Presenter.IsDefaultTenant);
            if (IsAdminLoggedIn)
            {
                if (CurrentViewContext.VerificationData[0].IsUiRulesViolate)
                {
                    pnlItem.Attributes.Add("Style", "background-color:#ff6666 !important");
                }
                else
                {
                    pnlItem.Attributes.Remove("Style");
                }
            }

            if (CurrentViewContext.VerificationData.IsNotNull() && CurrentViewContext.VerificationData.Count > 0)
            {
                string sampleDocFormURL = CurrentViewContext.VerificationData[0].SampleDocFormURL;
                string sampleDocFormDisplayLabelURL = CurrentViewContext.VerificationData[0].SampleDocFormDisplayURLLabel;//"..\\InstitutionImages\\Libertyuniversity\\Jellyfish.jpg";
                string SampleDocLink;
                //Only display hyperlink if sampleDocFromUrl available in VerificationData
                if (!sampleDocFormURL.IsNullOrEmpty())
                {
                    SampleDocLink = "<br /><a href=\"" + sampleDocFormURL + "\" onclick=\"\" target=\"_blank\");'>" +
                        (sampleDocFormDisplayLabelURL.IsNullOrEmpty()? "View Sample Document" : "View "+ sampleDocFormDisplayLabelURL) +"</a>";
                    //SampleDocLink = "<br /><a href='#' onclick='OpenSampleDocWindow(\"" + VerificationData[0].ItemName + "\");'>View Sample Doc</a>";
                    //ucExplanationDescription.SampleDocUrl = sampleDocFormURL;
                }
                else
                    SampleDocLink = String.Empty;

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

                #region UAT-3077:(1 of ?) Initial Analysis and begin Dev: Pay per submission item type (CC only) for Tracking and Rotation
                if (CurrentViewContext.VerificationData[0].ItemAmount.IsNotNull())
                {
                    decimal itemAmount = CurrentViewContext.VerificationData[0].ItemAmount.Value;
                    litItemAmount.Text = "$" + itemAmount.ToString("0.00");
                }
                litItemPaymentStatus.Text = CurrentViewContext.VerificationData[0].ItemPaymentStatus;


                if (!CurrentViewContext.VerificationData[0].IsNullOrEmpty()
                    && CurrentViewContext.VerificationData[0].IsItemPaymentPaid.HasValue
                    && CurrentViewContext.VerificationData[0].IsItemPaymentPaid.Value
                    && CurrentViewContext.VerificationData[0].PaidItemAmount.HasValue)
                {
                    litItemPaymentStatus.Text += string.Concat(" ($", CurrentViewContext.VerificationData[0].PaidItemAmount.Value.ToString("0.00"), ")"); ;
                }

                if (CurrentViewContext.VerificationData[0].IsPaymentTypeItem.Value)
                {
                    divItemPaymentPanel.Style["display"] = "block";
                }
                else
                {
                    divItemPaymentPanel.Style["display"] = "none";
                }
                #endregion
            }
        }

        public ItemDataReadOnlyModePresenter Presenter
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


            if (String.IsNullOrEmpty(FormMode)) // This fires only for the Third party
            {
                lnkItemName.Attributes.Add("UnifiedDocumentStartPageID", (UnifiedDocumentStartPageID != null ? Convert.ToString(UnifiedDocumentStartPageID) : "0"));
                litItemName.Text = VerificationData[0].ItemName.HtmlEncode() ;
                if (!IsItmEditableByApplcnt) //UAT-3599
                {
                    imageSDEdisabled.Visible = true;
                }
                pnlExceptionData.Visible = false;
                pnlNoItemData.Visible = true;
                pnlItemData.Visible = false;

                String _divType = ComplianceVerificationDetailsContract.GetItemDivType(VerificationData[0].ItemComplianceStatusCode);
                pnlNoItemData.Attributes.Add("divType", _divType);

                //pnlItemData.Attributes.Add("divType", "default");
                //pnlExceptionData.Attributes.Add("divType", "default");
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
                if (CurrentViewContext.VerificationData[0].SubmissionDate.HasValue && CurrentViewContext.VerificationData[0].SubmissionDate.Value != new DateTime(1900, 1, 1))
                {
                    lblItemDataSubmissionDate.Text = Convert.ToString(CurrentViewContext.VerificationData[0].SubmissionDate)+"(MT)";
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
                if (CurrentViewContext.VerificationData[0].SubmissionDate.HasValue && CurrentViewContext.VerificationData[0].SubmissionDate.Value != new DateTime(1900, 1, 1))
                {
                    lblExceptionDataSubmissionDate.Text = Convert.ToString(CurrentViewContext.VerificationData[0].SubmissionDate)+"(MT)";
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
                //pnlItemData.Attributes.Add("divType", "default");
                //pnlNoItemData.Attributes.Add("divType", "default");
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
                var _cmbData = CurrentViewContext.VerificationData.Where(attrOption => attrOption.ApplAttributeDataId == _attributeData.ApplAttributeDataId)
                  .ToList().
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
            else if (_dataTypeCode == ComplianceAttributeDatatypes.Screening_Document.GetStringValue().ToLower())
            {
                var _screeningDocTypeCode = DocumentType.SCREENING_DOCUMENT_ATTRIBUTE_TYPE_DOCUMENT.GetStringValue();
                HtmlGenericControl _divScreeningDocuments = (HtmlGenericControl)e.Item.FindControl("divScreeningDocuments");
                var _lstScreeningDocuments = CurrentViewContext.VerificationData.Where(vd => vd.ComplianceAttributeId == _attributeData.ComplianceAttributeId
                                             && vd.ScreeningDocTypeCode == _screeningDocTypeCode).ToList();

                StringBuilder _sbScreeningDocumentLinks = new StringBuilder();

                if (!_lstScreeningDocuments.IsNullOrEmpty())
                {
                    foreach (var document in _lstScreeningDocuments)
                    {
                        LinkButton lnkBtn = new LinkButton();
                        lnkBtn.ID = "lnkBtn_" + _attributeData.ComplianceAttributeId;
                        if (Presenter.IsDefaultTenant || Presenter.IsEDFormPreviouslyAccepted())
                        {
                            lnkBtn.OnClientClick = "ViewScreeningDocument(" + CurrentViewContext.SelectedTenantId_Global + ", " + document.ScreeningDocumentId + ");";
                        }
                        else
                        {
                            lnkBtn.OnClientClick = "OpenEmployerDisclosureDocument(" + CurrentViewContext.SelectedTenantId_Global + ", " + CurrentViewContext.CurrentLoggedInUserId + ", " + document.ScreeningDocumentId + ", '" + DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue() + "');";
                        }
                        lnkBtn.Attributes.Add("onclick", "return false;");

                        lnkBtn.Text = document.ScreeningDocumentName;
                        _divScreeningDocuments.Controls.Add(lnkBtn);
                        _divScreeningDocuments.Controls.Add(new LiteralControl("<br />"));
                    }
                    _divScreeningDocuments.Visible = true;
                }
                (e.Item.FindControl("litOptionsText") as Literal).Visible = false;
                (e.Item.FindControl("litAttributeValues") as Literal).Visible = false;
            }
        }
    }
}

