using System;
using System.Linq;
using INTSOF.Utils;
using CoreWeb.Shell;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Text;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class AttributeControl : BaseUserControl, IAttributeControlView
    {
        #region Variables

        public event EventHandler IsFileUploadApplicable;

        #region Private Variables

        private AttributeControlPresenter _presenter = new AttributeControlPresenter();
        private IAttributeControlView _currentViewContext;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public AttributeControlPresenter Presenter
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

        public ComplianceItemAttribute ClientItemAttributes
        {
            get;
            set;
        }

        public IAttributeControlView CurrentViewContext
        {
            get
            {
                if (_currentViewContext.IsNull())
                    _currentViewContext = new AttributeControl();
                return _currentViewContext;
            }
        }

        public Int32 ItemId
        {
            get;
            set;
        }

        //UAT-3806
        public List<ListItemEditableBies> lstIsEditableBy
        {
            get;
            set;
        }
        public Boolean IsItemSeries
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public List<ApplicantDocument> ApplicantDocuments
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        public Int32 ApplicantComplianceAttributeID
        {
            get;
            set;
        }

        public ApplicantComplianceAttributeData ApplicantAttributeData
        {
            get;
            set;
        }

        public String hdfApplicantAttributeDataControlId
        {
            get;
            set;
        }

        public List<String> lstAllowedExtensions
        {
            get
            {
                if (!ViewState["lstAllowedExtensions"].IsNullOrEmpty())
                    return (List<String>)(ViewState["lstAllowedExtensions"]);
                return new List<String>();
            }
            set
            {
                ViewState["lstAllowedExtensions"] = value;
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
            }
            Presenter.OnViewLoaded();
            pnlControls.Controls.Clear();

            if (CurrentViewContext.ClientItemAttributes.IsNull())
            {
                CurrentViewContext.ClientItemAttributes = ClientItemAttributes;
                CurrentViewContext.ApplicantAttributeData = ApplicantAttributeData;
                CurrentViewContext.IsItemSeries = IsItemSeries;
                //Fixed wrong attrivute values saved issue.
                hdfApplicantAttributeDataControlId = "hdfApplicantAttributeDataId_" + CurrentViewContext.ClientItemAttributes.CIA_ID;
                hdfApplicantAttributeDataId.ID = hdfApplicantAttributeDataControlId;
                CreateDynamicControl(CurrentViewContext.ClientItemAttributes);
                litLabel.Text = (!String.IsNullOrEmpty(CurrentViewContext.ClientItemAttributes.ComplianceAttribute.AttributeLabel) ? CurrentViewContext.ClientItemAttributes.ComplianceAttribute.AttributeLabel : CurrentViewContext.ClientItemAttributes.ComplianceAttribute.Name).HtmlEncode();
                litLabel.Text = "<span class='cptn'>" + litLabel.Text + "</span>";
                WclToolTip tltpMain = new WclToolTip();
                dvMain.ID = dvMain.ID + CurrentViewContext.ClientItemAttributes.CIA_ID;
                tltpMain.ID = "tltp_" + CurrentViewContext.ClientItemAttributes.CIA_ID;
                tltpMain.Text = CurrentViewContext.ClientItemAttributes.ComplianceAttribute.Description.HtmlEncode();
                tltpMain.Visible = tltpMain.Text.Trim().IsNullOrEmpty() ? false : true;
                tltpMain.TargetControlID = dvMain.ID;
                tltpMain.RelativeTo = Telerik.Web.UI.ToolTipRelativeDisplay.Mouse;
                tltpMain.Position = Telerik.Web.UI.ToolTipPosition.TopRight;
                tltpMain.Width = Unit.Pixel(300);
                pnlControls.Controls.Add(tltpMain);
            }
        }

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Generate the dynamic attribute
        /// </summary>
        /// <param name="clientItemAttribute">Attribute for which control is to be generated</param>
        private void CreateDynamicControl(ComplianceItemAttribute clientItemAttribute)
        {
            String dataTypeCode = clientItemAttribute.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower().Trim();
            Boolean _isCalculatedAttribute = clientItemAttribute.ComplianceAttribute.lkpComplianceAttributeType.Code.ToLower() == ComplianceAttributeType.Calculated.GetStringValue().ToLower()
                                            ? true : false;
            //UAT-3888
            HiddenField hdnPreviousValue = new HiddenField();
            hdnPreviousValue.ID = String.Format("hdnPreviousValue", clientItemAttribute.CIA_ID);
            hdnPreviousValue.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            pnlControls.Controls.Add(hdnPreviousValue);
            //UAT-3806
            Boolean _isAttributeEditable = clientItemAttribute.ComplianceAttribute.lkpComplianceAttributeType.Code.ToLower() == ComplianceAttributeType.Manual.GetStringValue().ToLower()
                                       && IsAttributeEditable(Convert.ToInt32(clientItemAttribute.ComplianceAttribute.ComplianceAttributeID));
            if (dataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
            {
                WclDatePicker dPicker = new WclDatePicker();
                dPicker.ToolTip = clientItemAttribute.ComplianceAttribute.Description;
                dPicker.DateInput.DateFormat = "MM-dd-yyyy";
                dPicker.ID = "dp_" + clientItemAttribute.CIA_ID;
                dPicker.Attributes.Add("onclick", "HideInstructionMessageDiv('" + clientItemAttribute.CIA_ID + "');");
                dPicker.ClientEvents.OnPopupClosing = "HideInstructionMessageDiv";//string.Format("HideInstructionMessageDiv('{0}');", clientItemAttribute.CIA_ID);

                dPicker.MinDate = Convert.ToDateTime("01-01-1900");
                if ((_isCalculatedAttribute || !_isAttributeEditable) && !CurrentViewContext.IsItemSeries)
                {
                    dPicker.Enabled = false;
                    dPicker.DateInput.EmptyMessage = "";
                }
                else
                {
                    dPicker.Enabled = true;
                    dPicker.DateInput.EmptyMessage = "Select a date";
                }
                pnlControls.Controls.Add(dPicker);
                HiddenField hdnCIA_ID = new HiddenField();
                hdnCIA_ID.ID = "hdnCIA_ID";
                hdnCIA_ID.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                hdnCIA_ID.Value = Convert.ToString(clientItemAttribute.CIA_ID);
                pnlControls.Controls.Add(hdnCIA_ID);
                HiddenField hdnAlreadyEnteredDate = new HiddenField();
                hdnAlreadyEnteredDate.ID = String.Format("hdnAlreadyEnteredDate_{0}", clientItemAttribute.CIA_ID);
                hdnAlreadyEnteredDate.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                if (CurrentViewContext.ApplicantAttributeData.IsNotNull() && !CurrentViewContext.ApplicantAttributeData.AttributeValue.IsNullOrEmpty())
                    hdnAlreadyEnteredDate.Value = CurrentViewContext.ApplicantAttributeData.AttributeValue;


                pnlControls.Controls.Add(hdnAlreadyEnteredDate);





                #region BIND DATA IF IN EDIT MODE

                try
                {
                    if (CurrentViewContext.ApplicantAttributeData.IsNotNull() && !CurrentViewContext.ApplicantAttributeData.AttributeValue.IsNullOrEmpty())
                    {
                        dPicker.SelectedDate = Convert.ToDateTime(CurrentViewContext.ApplicantAttributeData.AttributeValue);
                        hdnPreviousValue.Value = dPicker.SelectedDate.HasValue
                            ? dPicker.SelectedDate.Value.ToShortDateString()
                            : "";
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                #endregion
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Text.GetStringValue().ToLower().Trim())
            {
                WclTextBox txtTextType = new WclTextBox();
                txtTextType.ID = "txtTextType_" + clientItemAttribute.CIA_ID;
                if (clientItemAttribute.ComplianceAttribute.MaximumCharacters.IsNull() || clientItemAttribute.ComplianceAttribute.MaximumCharacters <= 0)
                    txtTextType.MaxLength = 50;
                else
                    txtTextType.MaxLength = Convert.ToInt32(clientItemAttribute.ComplianceAttribute.MaximumCharacters);

                txtTextType.Enabled = (_isCalculatedAttribute || !_isAttributeEditable) &&  !CurrentViewContext.IsItemSeries ? false : true;
                txtTextType.ToolTip = clientItemAttribute.ComplianceAttribute.Description;
                pnlControls.Controls.Add(txtTextType);

                #region BIND DATA IF IN EDIT MODE

                try
                {
                    if (CurrentViewContext.ApplicantAttributeData.IsNotNull())
                    {
                        txtTextType.Text = CurrentViewContext.ApplicantAttributeData.AttributeValue;
                        hdnPreviousValue.Value = txtTextType.Text;
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                #endregion
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Numeric.GetStringValue().ToLower().Trim())
            {
                WclNumericTextBox txtNumeric = new WclNumericTextBox();
                txtNumeric.ID = "txtNumericType_" + clientItemAttribute.CIA_ID;
                txtNumeric.Enabled = (_isCalculatedAttribute || !_isAttributeEditable) && !CurrentViewContext.IsItemSeries ? false : true;
                txtNumeric.ToolTip = clientItemAttribute.ComplianceAttribute.Description;
                pnlControls.Controls.Add(txtNumeric);

                #region BIND DATA IF IN EDIT MODE

                try
                {
                    if (CurrentViewContext.ApplicantAttributeData.IsNotNull())
                    {
                        txtNumeric.Text = CurrentViewContext.ApplicantAttributeData.AttributeValue;
                        hdnPreviousValue.Value = txtNumeric.Text;
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                #endregion
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower().Trim())
            {
                WclComboBox cmbOptions = new WclComboBox();
                cmbOptions.ID = "cmbOptions_" + clientItemAttribute.CIA_ID;
                if ((_isCalculatedAttribute || !_isAttributeEditable) && !CurrentViewContext.IsItemSeries)
                {
                    cmbOptions.Enabled = false;
                    cmbOptions.Items.Insert(0, new RadComboBoxItem("", AppConsts.ZERO));
                }
                else
                {
                    cmbOptions.Enabled = true;
                    cmbOptions.Items.Insert(0, new RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
                }

                List<ComplianceAttributeOption> lst = clientItemAttribute.ComplianceAttribute.ComplianceAttributeOptions.Where(opt => !opt.IsDeleted && opt.IsActive).ToList();

                foreach (var attributeOption in lst)
                {
                    cmbOptions.Items.Add(new RadComboBoxItem(attributeOption.OptionText, attributeOption.OptionValue));
                }
                cmbOptions.ToolTip = clientItemAttribute.ComplianceAttribute.Description;
                pnlControls.Controls.Add(cmbOptions);

                #region BIND DATA IF IN EDIT MODE

                try
                {
                    if (CurrentViewContext.ApplicantAttributeData.IsNotNull())
                    {
                        cmbOptions.SelectedValue = CurrentViewContext.ApplicantAttributeData.AttributeValue;
                        hdnPreviousValue.Value = cmbOptions.SelectedValue;
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                #endregion
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower().Trim())
            {
                WclComboBox cmbFileUpload = new WclComboBox();
                cmbFileUpload.ID = "cmbFileUpload_" + clientItemAttribute.CIA_ID;
                cmbFileUpload.CssClass = "cmbFileUpload";
                cmbFileUpload.ToolTip = clientItemAttribute.ComplianceAttribute.Description;
                cmbFileUpload.CheckBoxes = true;
                cmbFileUpload.EmptyMessage = "-- SELECT --";
                //UAT-1864
                cmbFileUpload.OnClientDropDownClosed = "BindDocumentForPreview";
                if ((_isCalculatedAttribute || !_isAttributeEditable) && !CurrentViewContext.IsItemSeries)
                {
                    cmbFileUpload.Enabled = false;
                    cmbFileUpload.EmptyMessage = "";
                }
                else
                {
                    cmbFileUpload.Enabled = true;
                    cmbFileUpload.EmptyMessage = "-- SELECT --";
                }
                Presenter.GetDocuments();

                List<Int32> lstMappedDocuments = null;
                Int32 disableViewDocumentID = AppConsts.NONE;
                Boolean isDocuementToDisabled = false;
                #region BIND DATA IF IN EDIT MODE
                try
                {
                    if (CurrentViewContext.ApplicantAttributeData.IsNotNull())
                    {
                        lstMappedDocuments = new List<Int32>();
                        String viewDocCode = ComplianceAttributeDatatypes.View_Document.GetStringValue();
                        List<ApplicantDocument> lstdocs = CurrentViewContext.ApplicantAttributeData.ApplicantComplianceDocumentMaps.Where(cond => !cond.IsDeleted).Select(cond => cond.ApplicantDocument).ToList();
                        lstMappedDocuments = lstdocs.Select(doc => doc.ApplicantDocumentID).ToList();
                        foreach (ApplicantDocument doc in lstdocs)
                        {
                            if (!doc.ApplicantComplianceDocumentMaps.IsNullOrEmpty() && doc.ApplicantComplianceDocumentMaps
                                  .Any(cond => !cond.ApplicantComplianceAttributeData.ComplianceAttribute.IsDeleted
                                && cond.ApplicantComplianceAttributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code == viewDocCode))
                            {
                                ApplicantComplianceAttributeData attrData = doc.ApplicantComplianceDocumentMaps
                                       .Where(cond => !cond.ApplicantComplianceAttributeData.ComplianceAttribute.IsDeleted
                                     && cond.ApplicantComplianceAttributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code == viewDocCode).FirstOrDefault().ApplicantComplianceAttributeData;

                                if (attrData.ApplicantComplianceItemID == ApplicantAttributeData.ApplicantComplianceItemID)
                                {
                                    disableViewDocumentID = doc.ApplicantDocumentID;
                                    isDocuementToDisabled = true;
                                    break;
                                }

                            }
                        }

                    }
                    foreach (var document in CurrentViewContext.ApplicantDocuments)
                    {
                        Boolean _isMapped = false;
                        Boolean isEnabled = true;
                        if (CurrentViewContext.ApplicantAttributeData.IsNotNull())
                            if (lstMappedDocuments.Count > 0)
                            {
                                _isMapped = lstMappedDocuments.Where(docId => docId == document.ApplicantDocumentID).Any();
                            }
                        if (document.ApplicantDocumentID == disableViewDocumentID && disableViewDocumentID > AppConsts.NONE && isDocuementToDisabled)
                        {
                            isEnabled = false;
                        }
                        RadComboBoxItem item = new RadComboBoxItem { Text = document.FileName, Value = Convert.ToString(document.ApplicantDocumentID), Checked = _isMapped }; //, Enabled = isEnabled                        
                        item.Attributes["desc"] = document.Description;
                        if (!isEnabled)
                        {
                            //item.Style.Add("disabled", "true");
                            item.Enabled = false;
                        }
                        //item.Attributes["Enabled"] = isEnabled.ToString();
                        cmbFileUpload.Items.Add(item);
                    }

                    hdnPreviousValue.Value = string.Join(",", cmbFileUpload.Items.Where(it => it.Checked).Select(it => it.Value).ToList());

                    #region UAT-1864 : As an applicant, I should be able to preview documents in the document selection dropdown on the submit item screen.
                    List<RadComboBoxItem> cmb = cmbFileUpload.CheckedItems.ToList();
                    Panel pnlDocumentPreview = new Panel();
                    if (cmb.Count > 0)
                    {
                        (this.Parent.Parent.Parent.FindControl("dvDocumentPreview") as HtmlGenericControl).Style["display"] = "block";
                        pnlDocumentPreview = (this.Parent.Parent.Parent.FindControl("pnlDocumentPreview") as Panel);
                    }
                    else
                    {
                        (this.Parent.Parent.Parent.FindControl("dvDocumentPreview") as HtmlGenericControl).Style["display"] = "none";
                        pnlDocumentPreview = (this.Parent.Parent.Parent.FindControl("pnlDocumentPreview") as Panel);
                    }

                    ////UAT-4367
                    //if (cmb.Count > 0)
                    //{
                    //    HiddenField hdnSelectedNodeIds = this.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.FindControl("hdnSelectedNodeIds") as HiddenField;
                    //    String selectedNodeIds = hdnSelectedNodeIds.Value;
                    //    Presenter.GetAllowedFileExtensions(selectedNodeIds);
                    //}
                    //END

                    foreach (var doc in cmb)
                    {
                        LinkButton lnkBtn = new LinkButton();
                        Label nextLine = new Label();
                        nextLine.Text = "<br/>";
                        lnkBtn.ID = doc.Value;
                        lnkBtn.Text = doc.Text;
                        lnkBtn.OnClientClick = "OpenAddAnotherItemPopup(" + doc.Value + ")";
                        lnkBtn.Attributes.Add("onclick", "return false;");
                        pnlDocumentPreview.Controls.Add(lnkBtn);

                        //UAT-4067//
                        //Int32 docId = Convert.ToInt32(doc.Value);
                        //String docPath = CurrentViewContext.ApplicantDocuments.Where(con => con.ApplicantDocumentID == docId).Select(Sel => Sel.DocumentPath).FirstOrDefault();
                        //var docExtension = System.IO.Path.GetExtension(docPath);
                        //docExtension = docExtension.Remove(".");

                        //if (!lstAllowedExtensions.IsNullOrEmpty() && !lstAllowedExtensions.Contains(docExtension.ToLower()) && !lstAllowedExtensions.Contains(docExtension.ToUpper()))
                        //{
                        //    Label lblFileErrorMsg = new Label();
                        //    lblFileErrorMsg.ID = "lblFileErrorMsg_" + doc.Value;
                        //    lblFileErrorMsg.Text = " Unsupported file format.";
                        //    lblFileErrorMsg.ForeColor = System.Drawing.Color.Red;
                        //    pnlDocumentPreview.Controls.Add(lblFileErrorMsg);

                        //    HiddenField hdnIsAnyRestrictedFileUploaded = this.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.FindControl("hdnIsAnyRestrictedFileUploaded") as HiddenField;
                        //    hdnIsAnyRestrictedFileUploaded.Value = true.ToString().ToLower();
                        //}
                        //END UAT-4067

                        pnlDocumentPreview.Controls.Add(nextLine);
                    }
                    #endregion

                    //if (_isCalculatedAttribute)
                    //    IsFileUploadApplicable(this, null);
                    //else
                    IsFileUploadApplicable(this, new EventArgs());
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                #endregion


                //Attaching client side event handler for combobox upload event
                //This handler should be decalard in global scope of the script
                cmbFileUpload.OnClientLoad = "efn_atrFileUpdOnLoad";

                pnlControls.Controls.Add(cmbFileUpload);



                ////if (Convert.ToBoolean(clientItemAttribute.Required))
                ////{
                ////    ApplyRequiredField(clientItemAttribute.ClientComplianceItemAttributeID, cmbFileUpload.ID, clientItemAttribute.AttributeLabel + " is required.");
                ////}
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower().Trim())
            {

                LinkButton linkbutton = new LinkButton();
                linkbutton.ID = "lnkBtnViewDocType_" + clientItemAttribute.CIA_ID;
                linkbutton.ToolTip = clientItemAttribute.ComplianceAttribute.Description;
                linkbutton.Attributes.Add("onclick", "return false;");
                linkbutton.Attributes.Add("DataTypeCode", ComplianceAttributeDatatypes.View_Document.GetStringValue());
                pnlControls.Controls.Add(linkbutton);
                if (clientItemAttribute.ComplianceAttribute.ComplianceAttributeDocuments.IsNullOrEmpty() ||
                    !clientItemAttribute.ComplianceAttribute.ComplianceAttributeDocuments.Any(cond => !cond.CAD_IsDeleted))
                {
                    linkbutton.Text = "N/A";
                    hdnfSystemDocumentId.Value = "0";
                }
                else
                {

                    linkbutton.Text = Convert.ToString(clientItemAttribute.ComplianceAttribute.ComplianceAttributeDocuments
                        .FirstOrDefault(cond => !cond.CAD_IsDeleted).ClientSystemDocument.CSD_FileName);
                    String orgUserID = CurrentLoggedInUserId.ToString();
                    String applicantDocID = "0";
                    String clientDocID = Convert.ToString(clientItemAttribute.ComplianceAttribute.ComplianceAttributeDocuments
                        .FirstOrDefault(cond => !cond.CAD_IsDeleted).CAD_DocumentID);
                    linkbutton.OnClientClick = "openPdfPopUp('" + orgUserID + "','" + clientDocID + "','" + applicantDocID + "');";

                }
                if ((_isCalculatedAttribute || !_isAttributeEditable) && !CurrentViewContext.IsItemSeries)
                {
                    linkbutton.Enabled = false;
                    linkbutton.OnClientClick = "";
                }
                else
                {
                    linkbutton.Enabled = true;
                    String orgUserID = CurrentLoggedInUserId.ToString();
                    String applicantDocID = "0";
                    String clientDocID = Convert.ToString(clientItemAttribute.ComplianceAttribute.ComplianceAttributeDocuments
                         .FirstOrDefault(cond => !cond.CAD_IsDeleted).CAD_DocumentID);
                    linkbutton.OnClientClick = "openPdfPopUp('" + orgUserID + "','" + clientDocID + "','" + applicantDocID + "');";
                }

                #region BIND DATA IF IN EDIT MODE

                try
                {
                    if (CurrentViewContext.ApplicantAttributeData.IsNotNull() && CurrentViewContext.ApplicantAttributeData.AttributeValue != AppConsts.ZERO)
                    {
                        String applicantDocID = "0";
                        ApplicantDocument appDoc = Presenter.GetApplicantDocumentByApplAttrDataID(CurrentViewContext.ApplicantAttributeData.ApplicantComplianceAttributeID);
                        if (!appDoc.IsNullOrEmpty())
                        {
                            hdnPreviousValue.Value = appDoc.FileName;
                            linkbutton.Text = appDoc.FileName.HtmlEncode();
                            applicantDocID = appDoc.ApplicantDocumentID.ToString();
                            String orgUserID = CurrentLoggedInUserId.ToString();
                            //UAT-3279
                            String clientDocID = String.Empty;
                            if (!clientItemAttribute.ComplianceAttribute.ComplianceAttributeDocuments.IsNullOrEmpty()
                                || clientItemAttribute.ComplianceAttribute.ComplianceAttributeDocuments
                                                        .Any(cond => !cond.CAD_IsDeleted))
                            {
                                clientDocID = Convert.ToString(clientItemAttribute.ComplianceAttribute.ComplianceAttributeDocuments
                                    .FirstOrDefault(cond => !cond.CAD_IsDeleted).CAD_DocumentID);
                            }
                            linkbutton.OnClientClick = "openPdfPopUp('" + orgUserID + "','" + clientDocID + "','" + applicantDocID + "');";

                            hdnOrgUserID.Value = CurrentLoggedInUserId.ToString();
                            linkbutton.Attributes.Add("AppDocID", applicantDocID);
                        }

                        //if (CurrentViewContext.ApplicantAttributeData.ApplicantComplianceDocumentMaps.Any(cond => !cond.IsDeleted))
                        //{
                        //    applicantDocID = CurrentViewContext.ApplicantAttributeData.ApplicantComplianceDocumentMaps
                        //                            .FirstOrDefault(cond => !cond.IsDeleted).ApplicantDocumentID.ToString();
                        //}

                    }
                    else
                    {
                        linkbutton.Attributes.Add("AppDocID", "0");
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                #endregion
            }
            //UAT-1738
            else if (String.Compare(dataTypeCode, ComplianceAttributeDatatypes.Screening_Document.GetStringValue(), true) == AppConsts.NONE)
            {

                LinkButton linkbutton = new LinkButton();
                linkbutton.ID = "lnkBtnScreeningDoc_" + clientItemAttribute.CIA_ID;
                linkbutton.Enabled = (_isCalculatedAttribute || !_isAttributeEditable) && !CurrentViewContext.IsItemSeries ? false : true;
                linkbutton.ToolTip = clientItemAttribute.ComplianceAttribute.Description;
                linkbutton.Attributes.Add("onclick", "return false;");
                linkbutton.Attributes.Add("DataTypeCode", ComplianceAttributeDatatypes.Screening_Document.GetStringValue());
                linkbutton.Text = "";
                pnlControls.Controls.Add(linkbutton);

                #region BIND DATA IF IN EDIT MODE

                try
                {
                    if (CurrentViewContext.ApplicantAttributeData.IsNotNull() && CurrentViewContext.ApplicantAttributeData.AttributeValue != AppConsts.ZERO)
                    {
                        String applicantDocID = "0";
                        ApplicantDocument appDoc = Presenter.GetApplicantDocumentByApplAttrDataID(CurrentViewContext.ApplicantAttributeData.ApplicantComplianceAttributeID);
                        if (!appDoc.IsNullOrEmpty())
                        {
                            hdnPreviousValue.Value = appDoc.FileName;
                            linkbutton.Text = appDoc.FileName.HtmlEncode();
                            applicantDocID = appDoc.ApplicantDocumentID.ToString();
                            linkbutton.OnClientClick = "ViewScreeningDoc('" + applicantDocID + "');";
                        }
                        linkbutton.Attributes.Add("AttributeValue", CurrentViewContext.ApplicantAttributeData.AttributeValue);

                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                #endregion
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Signature.GetStringValue().ToLower().Trim())
            {
                dvAttributeControl.Attributes.Add("style", "border-right-width: 0px;width:415px !important;");
                HtmlGenericControl mainDiv = new HtmlGenericControl("DIV");
                mainDiv.ID = "dvSignature_" + clientItemAttribute.CIA_ID;
                mainDiv.Attributes.Add("class", "sigPad");
                mainDiv.Attributes.Add("FieldType", dataTypeCode);
                mainDiv.Attributes.Add("ItemFieldId", clientItemAttribute.CIA_ID.ToString());

                //if (!isEnabled)
                //    mainDiv.Style.Add("Disabled", "true");

                HtmlGenericControl canvas = new HtmlGenericControl("canvas");
                canvas.ID = "signature";
                canvas.Attributes.Add("class", "pad borderTextArea");
                canvas.Attributes.Add("style", "border: 2px solid green;");
                HtmlInputHidden hdnOutput = new HtmlInputHidden();
                hdnOutput.ID = "hiddenOutput_" + clientItemAttribute.CIA_ID.ToString();
                hdnOutput.Attributes.Add("class", "output");

                HtmlGenericControl lnkClear = new HtmlGenericControl("a");
                lnkClear.Attributes.Add("href", "#clear");
                lnkClear.InnerText = "Clear Signature";
                lnkClear.Attributes.Add("class", "clearButton");
                lnkClear.Attributes.Add("style", "font-size: 13px;color: blue;float:right");

                mainDiv.Controls.Add(hdnOutput);
                mainDiv.Controls.Add(canvas);
                mainDiv.Controls.Add(lnkClear);

                pnlControls.Controls.Add(mainDiv);

                #region Add Min Length Validation
                Label lblMinLengthSignatureCheck = new Label();
                lblMinLengthSignatureCheck.ID = "lblMinLengthSignatureCheck_" + clientItemAttribute.CIA_ID;
                lblMinLengthSignatureCheck.CssClass = "checkMinLengthSignature errmsg";
                lblMinLengthSignatureCheck.Text = "";
                pnlControls.Controls.Add(lblMinLengthSignatureCheck);
                #endregion
                CustomValidator cstValidator = null;

                //if (isFieldRequired)
                //{
                //    cstValidator = ApplyCustomFieldValidator(requirementField.RequirementItemFieldID, hdnOutput.ID, validationMsg, dataTypeCode);
                //}

                #region BIND DATA IF IN EDIT MODE

                try
                {
                    if (CurrentViewContext.ApplicantAttributeData.IsNotNull())
                    {
                        if (!CurrentViewContext.ApplicantAttributeData.ComplianceAttributeDataLargeContents.IsNullOrEmpty())
                        {
                            HtmlImage img = new HtmlImage();
                            var base64Data = Convert.ToBase64String(CurrentViewContext.ApplicantAttributeData.ComplianceAttributeDataLargeContents.FirstOrDefault().CADLC_Signature.ToArray());
                            img.Src = "data:image/gif;base64," + base64Data;
                            img.Width = 300;
                            img.Height = 150;
                            mainDiv.Controls.Add(img);

                            canvas.Visible = false;
                            lnkClear.Visible = false;

                            if (!cstValidator.IsNullOrEmpty())
                                cstValidator.Enabled = false;
                        }
                        else
                        {
                            canvas.Visible = true;
                            lnkClear.Visible = true;
                        }
                    }
                    else
                    {
                        canvas.Visible = true;
                        lnkClear.Visible = true;
                    }
                    if (!_isAttributeEditable && !CurrentViewContext.IsItemSeries)
                    {
                        canvas.Visible = false;
                        lnkClear.Visible = false;
                    }
                    else
                    {
                        canvas.Visible = true;
                        lnkClear.Visible = true;
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                #endregion
            }

            if (CurrentViewContext.ApplicantAttributeData.IsNotNull())
            {
                hdfApplicantAttributeDataId.Value = Convert.ToString(CurrentViewContext.ApplicantAttributeData.ApplicantComplianceAttributeID);
            }
        }

        private void ApplyRequiredField(Int32 attributeId, String controlId, String errorMessage)
        {
            RequiredFieldValidator rfValidator = new RequiredFieldValidator();
            rfValidator.ID = "rf_" + attributeId.ToString();
            rfValidator.Display = ValidatorDisplay.Dynamic;
            rfValidator.ControlToValidate = controlId;
            rfValidator.ErrorMessage = errorMessage;
            rfValidator.CssClass = "errmsg";
            rfValidator.ValidationGroup = "vGroup_" + ItemId.ToString();
            pnlControls.Controls.Add(rfValidator);
        }

        //Start UAT-3806
        private List<ListItemEditableBies> GetItemEditableByProperties(Int32 itemId)
        {
            return lstIsEditableBy
                       .Where(ap => ap.ComplianceItemId == itemId)
                       .ToList();
        }
        private Boolean IsAttributeEditable(Int32 attributeId)
        {
            String _attributeTypeCode = LCObjectType.ComplianceATR.GetStringValue();
            String _itemTypeCode = LCObjectType.ComplianceItem.GetStringValue();
            List<ListItemEditableBies> lstItemEditableBy = GetItemEditableByProperties(ClientItemAttributes.CIA_ItemID);
            Boolean _isEditable = false;

            List<ListItemEditableBies> _attributeEditableByList = lstItemEditableBy.Where
                      (data => data.ComplianceAttributeId == attributeId).ToList();

            //  Code can be null in case Assignement Property is defined at the 
            // Package Level or not defined at all in the hierarchy
            if (_attributeEditableByList.IsNullOrEmpty() || _attributeEditableByList.Any(eb => String.IsNullOrEmpty(eb.EditableByCode)))
                _isEditable = false;

            else
            {
                // Check for Applicant
                if (lstItemEditableBy.Where(editableBy => editableBy.EditableByCode == LkpEditableBy.Applicant
                                                      && editableBy.ComplianceAttributeId == attributeId).Any())
                {
                    _isEditable = true;
                }
            }
            return _isEditable;
        }
        //END UAT

        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}

