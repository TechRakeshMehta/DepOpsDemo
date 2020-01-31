using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.ClinicalRotation.Views;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using Entity.ClientEntity;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RequirementVerificationItemControl : BaseUserControl, IRequirementVerificationItemControl
    {
        #region Variables

        //UAT-1470 :As a student, there should be a way to close out of the video once you open it.
        private RequirementVerificationItemControlPresenter _presenter = new RequirementVerificationItemControlPresenter();

        #endregion;

        #region Properties

        /// <summary>
        /// RequirementItemID
        /// </summary>
        Int32 IRequirementVerificationItemControl.ItemId { get; set; }

        /// <summary>
        /// ApplicantRequirementItemDataID
        /// </summary>
        Int32 IRequirementVerificationItemControl.ApplReqItemDataId { get; set; }

        /// <summary>
        /// Represents the Item level data
        /// </summary>
        List<RequirementVerificationDetailContract> IRequirementVerificationItemControl.lstItemLevelData
        {
            get;
            set;
        }

        /// <summary>
        /// List for 'lkpRequirementItemStatus' entity
        /// </summary>
        List<RequirementItemStatusContract> IRequirementVerificationItemControl.lstReqItemStatusTypes
        {
            get;
            set;
        }

        /// <summary>
        /// SelectedTenantId
        /// </summary>
        Int32 IRequirementVerificationItemControl.SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// ID Prefix for the Field Control
        /// </summary>
        String IRequirementVerificationItemControl.FieldControlIdPrefix
        {
            get
            {
                return "ucFieldControl_" + CurrentViewContext.lstItemLevelData[0].CatId + "_" + CurrentViewContext.ItemId + "_";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        String IRequirementVerificationItemControl.FieldControlIdGenerator
        {
            get
            {
                return "_" + CurrentViewContext.lstItemLevelData.First().CatId + "_" + CurrentViewContext.ItemId + "_";
            }
        }

        /// <summary>
        /// Represents the screen from which the screen was opened
        /// </summary>
        String IRequirementVerificationItemControl.ControlUseType
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the current Context
        /// </summary>
        public IRequirementVerificationItemControl CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        Boolean IRequirementVerificationItemControl.IsViewDocFieldViewed //UAT-4543
        {
            get;
            set;
        }

        #region UAT-1470 :As a student, there should be a way to close out of the video once you open it.
        /// <summary>
        /// PackageID
        /// </summary>
        Int32 IRequirementVerificationItemControl.PackageID { get; set; }

        /// <summary>
        /// CategoryID
        /// </summary>
        Int32 IRequirementVerificationItemControl.CategoryID { get; set; }

        List<RequirementObjectTreeContract> IRequirementVerificationItemControl.LstRequirementObjTreeProperty { get; set; }
        public RequirementVerificationItemControlPresenter Presenter
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
        #endregion

        #region UAT-2224: Admin access to upload/associate documents on rotation package items.

        public List<ApplicantFieldDocumentMappingContract> lstApplicantRequirementDocumentMaps { get; set; }

        public List<ApplicantDocumentContract> lstApplicantDocument
        {
            get
            {
                List<ApplicantDocumentContract> lstApplicantDocumentObj = new List<ApplicantDocumentContract>();
                if (ViewState["lstApplicantDocument"].IsNotNull())
                {
                    lstApplicantDocumentObj = (List<ApplicantDocumentContract>)ViewState["lstApplicantDocument"];
                }
                return lstApplicantDocumentObj;
            }
            set
            {
                ViewState["lstApplicantDocument"] = ucRequirementVerificationDetailsDocumentConrol.lstApplicantDocument = value;
            }
        }

        public Int32? FileUpoladFieldId
        {
            get
            {
                if (!String.IsNullOrEmpty(hdfFileUploadFieldId.Value))
                    return Convert.ToInt32(hdfFileUploadFieldId.Value);
                return AppConsts.NONE;
            }
            set
            {
                hdfFileUploadFieldId.Value = Convert.ToString(value);
            }
        }

        public Boolean IsFileUpoloadApplicable
        {
            get
            {
                if (!String.IsNullOrEmpty(hdfFileUploadExists.Value))
                    return Convert.ToBoolean(hdfFileUploadExists.Value);

                return false;
            }
            set
            {
                hdfFileUploadExists.Value = Convert.ToString(value);
            }
        }

        public Int32 SelectedApplicantId_Global
        {
            get
            {
                if (!ViewState["SelectedApplicantIdEM"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedApplicantIdEM"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedApplicantIdEM"].IsNullOrEmpty())
                    ViewState["SelectedApplicantIdEM"] = value;
            }
        }

        public Int32 CurrentRequirementPackageSubscriptionID_Global
        {
            get
            {
                if (!ViewState["CurrentRequirementPackageSubscriptionIDEM"].IsNullOrEmpty())
                    return (Int32)(ViewState["CurrentRequirementPackageSubscriptionIDEM"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["CurrentRequirementPackageSubscriptionIDEM"].IsNullOrEmpty())
                    ViewState["CurrentRequirementPackageSubscriptionIDEM"] = value;
            }
        }

        public String EntityPermissionName
        {
            get
            {
                if (!ViewState["EntityPermissionName"].IsNullOrEmpty())
                    return (String)(ViewState["EntityPermissionName"]);
                else return "";
            }
            set
            {
                if (ViewState["EntityPermissionName"].IsNullOrEmpty())
                    ViewState["EntityPermissionName"] = value;
            }
        }

        public Boolean IsItemEditable { get; set; }
        public Int32 CurrentTenantId_Global
        {
            get
            {
                if (!ViewState["CurrentTenantId"].IsNullOrEmpty())
                    return (Int32)(ViewState["CurrentTenantId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["CurrentTenantId"].IsNullOrEmpty())
                    ViewState["CurrentTenantId"] = value;
            }
        }
        #endregion

        /// <summary>
        /// RequirementItemSampleDocURL
        /// </summary>
        String IRequirementVerificationItemControl.RequirementItemSampleDocURL //UAT-3309
        {
            get;
            set;
        }

        #region UAT-4368
        public Boolean IsClientAdminLoggedIn
        {
            get;
            set;
        }
        public Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentViewContext.lstItemLevelData.IsNullOrEmpty())
            {
                RequirementVerificationDetailContract _currenItemData = null;
                int AssignToUserID = CurrentViewContext.lstItemLevelData.Distinct().Where(x => x.AssignToUserID > AppConsts.NONE).Select(x => x.AssignToUserID).FirstOrDefault();
                if (AssignToUserID>AppConsts.NONE)
                {
                    _currenItemData = CurrentViewContext.lstItemLevelData.Where(x => x.AssignToUserID > AppConsts.NONE).OrderBy(ord => ord.RequirementItemDisplayOrder).First();
                }
                else
                {
                    _currenItemData = CurrentViewContext.lstItemLevelData.OrderBy(ord => ord.RequirementItemDisplayOrder).First();
                }
                litItemName.Text = _currenItemData.ItemName.HtmlEncode();
                //Start UAt-4253
                if ((_currenItemData.IsCategoryDataMovementAllowed && !_currenItemData.IsItemDataMovementAllowed) || !_currenItemData.IsCategoryDataMovementAllowed)
                    imageADEdisabled.Visible = true; 
                //END UAT-4253
                txtRejectionreason.Text = _currenItemData.RejectionReason?.Replace("###", System.Environment.NewLine);
                CurrentViewContext.ApplReqItemDataId = _currenItemData.ApplReqItemDataId;
                //imgItemStatus.ImageUrl = GetImagePathItemStatus(_currenItemData.ItemStatusCode);
                //imgItemStatus.ToolTip = _currenItemData.ItemStatusName;
                //UAT-4543
                if (!CurrentViewContext.lstItemLevelData.IsNullOrEmpty())
                    CurrentViewContext.IsViewDocFieldViewed = CurrentViewContext.lstItemLevelData.Where(cond => cond.FieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue()).IsNullOrEmpty() || CurrentViewContext.lstItemLevelData.Where(cond => cond.FieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue() && cond.FieldDataValue != "0" && cond.FieldDataValue != "").Count() > 0 ? true : false;
                //END UAT-4543
                litStatus.Text = _currenItemData.ItemStatusName;
                lblSubmissionDate.Text = (_currenItemData.ItemSubmissionDate.HasValue && _currenItemData.ItemSubmissionDate != new DateTime(1900, 1, 1)) ? _currenItemData.ItemSubmissionDate.Value.ToString("M/d/yyyy hh:mm:ss tt") + "(MT)" : string.Empty;

                if (CurrentViewContext.ApplReqItemDataId == AppConsts.NONE)
                    chkDeleteItem.Visible = false;
                else
                    chkDeleteItem.Visible = true;

                txtRejectionreason.Attributes.Add("noteItemId", Convert.ToString(_currenItemData.ItemId));
                rbtnListStatus.Attributes.Add("actionItemId", Convert.ToString(_currenItemData.ItemId));

                rbtnListStatus.Attributes.Add("catId", Convert.ToString(_currenItemData.CatId));
                rbtnListStatus.Attributes.Add("itmId", Convert.ToString(_currenItemData.ItemId));

                //if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE.ToLower().Trim())
                //{
                //    txtRejectionreason.Enabled = false;
                //}


                #region UAT-1470 :As a student, there should be a way to close out of the video once you open it.
                CurrentViewContext.PackageID = _currenItemData.PkgId;
                CurrentViewContext.CategoryID = _currenItemData.CatId;
                Presenter.GetAttributeObjectTreeProperties();
                #endregion

                #region UAT-2224: Admin access to upload/associate documents on rotation package items.

                RequirementVerificationDetailContract _itemLevelDataFileUpload = CurrentViewContext.lstItemLevelData.FirstOrDefault(x => x.FieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue());
                RequirementVerificationDetailContract _itemLevelDataViewDoc = CurrentViewContext.lstItemLevelData.FirstOrDefault(x => x.FieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue());

                Boolean isIncompleteItem = false;
                if (_currenItemData.ItemStatusCode == RequirementItemStatus.INCOMPLETE.GetStringValue())
                    isIncompleteItem = true;

                if (_itemLevelDataFileUpload.IsNotNull()) // If File upload type attribute exists
                {
                    this.FileUpoladFieldId = _itemLevelDataFileUpload.FieldId;

                    // Check if editable ?
                    //ucRequirementVerificationDetailsDocumentConrol.IsReadOnly = !IsAttributeEditable(Convert.ToInt32(_itemLevelDataFileUpload.FieldId));

                    if (_itemLevelDataFileUpload.ApplReqItemDataId.IsNotNull()) // If the data was already added for the Document attributes' item
                        ucRequirementVerificationDetailsDocumentConrol.RequirementItemDataId = _itemLevelDataFileUpload.ApplReqItemDataId;
                    ucRequirementVerificationDetailsDocumentConrol.RequirementItemId = _currenItemData.ItemId;
                    ucRequirementVerificationDetailsDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId;
                    ucRequirementVerificationDetailsDocumentConrol.ApplicantId = this.SelectedApplicantId_Global;
                    ucRequirementVerificationDetailsDocumentConrol.RequirementPackageSubscriptionId = this.CurrentRequirementPackageSubscriptionID_Global;
                    ucRequirementVerificationDetailsDocumentConrol.RequirementCategoryId = Convert.ToInt32(_itemLevelDataFileUpload.CatId);
                    ucRequirementVerificationDetailsDocumentConrol.RequirementFieldId = Convert.ToInt32(_itemLevelDataFileUpload.FieldId);
                    ucRequirementVerificationDetailsDocumentConrol.lstApplicantRequirementDocumentMaps = this.lstApplicantRequirementDocumentMaps;
                    ucRequirementVerificationDetailsDocumentConrol.lstApplicantDocument = this.lstApplicantDocument;
                    ucRequirementVerificationDetailsDocumentConrol.IsFileUploadApplicable = this.IsFileUpoloadApplicable = true;
                    ucRequirementVerificationDetailsDocumentConrol.IsFieldRequired = _itemLevelDataFileUpload.IsFieldRequired;
                    ucRequirementVerificationDetailsDocumentConrol.IsIncompleteItem = isIncompleteItem;
                    ucRequirementVerificationDetailsDocumentConrol.IsFileUploadControlExist = true;
                    ucRequirementVerificationDetailsDocumentConrol.IsItemEditable = CurrentViewContext.IsItemEditable;
                    //UAT 4380
                    ucRequirementVerificationDetailsDocumentConrol.IsAdminLoggedIn = CurrentViewContext.IsAdminLoggedIn;
                    ucRequirementVerificationDetailsDocumentConrol.IsClientAdminLoggedIn = CurrentViewContext.IsClientAdminLoggedIn;
                    ucRequirementVerificationDetailsDocumentConrol.IsFieldEditableByAdmin = _itemLevelDataFileUpload.IsFieldEditableByAdmin;
                    ucRequirementVerificationDetailsDocumentConrol.IsFieldEditableByClientAdmin = _itemLevelDataFileUpload.IsFieldEditableByClientAdmin;

                    //UAT 2371
                    ucRequirementVerificationDetailsDocumentConrol.EntityPermissionName = CurrentViewContext.EntityPermissionName;

                    if (_itemLevelDataViewDoc.IsNotNull())
                    {
                        ucRequirementVerificationDetailsDocumentConrol.ViewApplDocId = _itemLevelDataViewDoc.ApplDocId;
                    }
                }
                //else if (_itemLevelDataViewDoc.IsNotNull()) //if only view doc attribute present
                //{
                //    if (_itemLevelDataViewDoc.ApplReqItemDataId.IsNotNull()) // If the data was already added for the Document attributes' item
                //        ucRequirementVerificationDetailsDocumentConrol.RequirementItemDataId = _itemLevelDataViewDoc.ApplReqItemDataId;
                //    ucRequirementVerificationDetailsDocumentConrol.RequirementItemId = _currenItemData.ItemId;
                //    ucRequirementVerificationDetailsDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId;
                //    ucRequirementVerificationDetailsDocumentConrol.ApplicantId = this.SelectedApplicantId_Global;
                //    ucRequirementVerificationDetailsDocumentConrol.RequirementPackageSubscriptionId = this.CurrentRequirementPackageSubscriptionID_Global;
                //    ucRequirementVerificationDetailsDocumentConrol.RequirementCategoryId = Convert.ToInt32(_itemLevelDataViewDoc.CatId);
                //    ucRequirementVerificationDetailsDocumentConrol.RequirementFieldId = Convert.ToInt32(_itemLevelDataViewDoc.FieldId);
                //    ucRequirementVerificationDetailsDocumentConrol.lstApplicantRequirementDocumentMaps = this.lstApplicantRequirementDocumentMaps;
                //    ucRequirementVerificationDetailsDocumentConrol.lstApplicantDocument = this.lstApplicantDocument;
                //    ucRequirementVerificationDetailsDocumentConrol.IsFileUploadApplicable = false;
                //    ucRequirementVerificationDetailsDocumentConrol.IsViewDocApplicable = true;
                //    ucRequirementVerificationDetailsDocumentConrol.IsFieldRequired = _itemLevelDataViewDoc.IsFieldRequired;
                //    ucRequirementVerificationDetailsDocumentConrol.IsIncompleteItem = isIncompleteItem;
                //}
                else
                {
                    ucRequirementVerificationDetailsDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId;
                    ucRequirementVerificationDetailsDocumentConrol.IsReadOnly = false;
                    ucRequirementVerificationDetailsDocumentConrol.IsFileUploadApplicable = false;
                    ucRequirementVerificationDetailsDocumentConrol.EntityPermissionName = CurrentViewContext.EntityPermissionName;
                    ucRequirementVerificationDetailsDocumentConrol.IsFileUploadControlExist = false;
                    ucRequirementVerificationDetailsDocumentConrol.ApplicantId = this.SelectedApplicantId_Global;
                }

                #endregion

                #region UAT-3309
                string sampleDocFormURL = _currenItemData.ReqItemSampleDocFormURL;
                string SampleDocLink;
                //Only display hyperlink if sampleDocFromUrl available in VerificationData


                if (_currenItemData.ListRequirementItemURLContract != null && _currenItemData.ListRequirementItemURLContract.Count > 0)
                {
                    bool IsAddDescription = false;
                    foreach (var item in _currenItemData.ListRequirementItemURLContract)
                    {
                        Literal ObjLiteral = new Literal();
                        ObjLiteral.ID = "ID_" + item.RItemURLID;
                        sampleDocFormURL = item.RItemURLSampleDocURL;
                        string ViewSampleDoc = "View Sample Doc";
                        if (!item.RItemURLLabel.IsNullOrEmpty())
                        { ViewSampleDoc = item.RItemURLLabel; }

                        SampleDocLink = "<br /><a href=\"" + sampleDocFormURL + "\" onclick=\"\" target=\"_blank\");'>" + ViewSampleDoc + "</a>";
                        AdminExplanation.Controls.Add(ObjLiteral);
                        if (!CurrentViewContext.lstItemLevelData.First().ItemDescription.IsNullOrEmpty() && !IsAddDescription)
                        {
                            if (!IsAddDescription)
                            {
                                ObjLiteral.Text = String.Format("<span class='expl-title'></span><span class='expl-dur'></span>{0}{1}", CurrentViewContext.lstItemLevelData.First().ItemDescription, SampleDocLink);
                                IsAddDescription = true;
                            }
                        }
                        else
                            ObjLiteral.Text = SampleDocLink;
                    }
                }
                else
                {
                    Literal ObjLiteral = new Literal();
                    ObjLiteral.ID = "ID_" + "1";
                    if (!CurrentViewContext.lstItemLevelData.First().ItemDescription.IsNullOrEmpty())
                    {
                        ObjLiteral.Text = String.Format("<span class='expl-title'></span><span class='expl-dur'></span>{0}{1}", CurrentViewContext.lstItemLevelData.First().ItemDescription, "");
                    }
                    AdminExplanation.Controls.Add(ObjLiteral);
                }

                //if (!sampleDocFormURL.IsNullOrEmpty())
                //{
                //    SampleDocLink = "<br /><a href=\"" + sampleDocFormURL + "\" onclick=\"\" target=\"_blank\");'>View Sample Doc</a>";
                //}
                //else
                //    SampleDocLink = String.Empty;


                #endregion


                if (!CurrentViewContext.lstItemLevelData.First().ItemExplanatoryNotes.IsNullOrEmpty())
                    litExplanatoryNotes.Text = String.Format("<span class='expl-title'></span><span class='expl-dur'></span>{0}", CurrentViewContext.lstItemLevelData.First().ItemExplanatoryNotes);
                else
                    litExplanatoryNotes.Text = String.Empty;

                #region UAT-3077:(1 of ?) Initial Analysis and begin Dev: Pay per submission item type (CC only) for Tracking and Rotation
                if (_currenItemData.ItemAmount.IsNotNull())
                {
                    decimal itemAmount = _currenItemData.ItemAmount.Value;


                    litItemAmount.Text = "$" + itemAmount.ToString("0.00");
                }
                litItemPaymentStatus.Text = _currenItemData.ItemPaymentStatus;

                if (_currenItemData.IsItemPaymentPaid && _currenItemData.PaidItemAmount.HasValue)
                    litItemPaymentStatus.Text += string.Concat(" ($", _currenItemData.PaidItemAmount.Value.ToString("0.00"), ")");

                if (_currenItemData.IsPaymentTypeItem)
                {
                    divItemPaymentPanel.Style["display"] = "block";
                }
                else
                {
                    divItemPaymentPanel.Style["display"] = "none";
                }
                #endregion
                if (_currenItemData.AssignToUserID == CoreWeb.Shell.SysXWebSiteUtils.SessionService.OrganizationUserId)
                    HighlightAssignedItems(true);
            }

            hdfNotApproved.Value = RequirementItemStatus.NOT_APPROVED.GetStringValue();
            hdfIncomplete.Value = RequirementItemStatus.INCOMPLETE.GetStringValue();

            GenerateFields();
            BindItemStatus(CurrentViewContext.lstReqItemStatusTypes);
            //Check Item Assign to UserID
            if (!CurrentViewContext.IsItemEditable)
            {
                chkDeleteItem.Enabled = false;
            }
        }

        #endregion

        #region Methods

        #region Public Methods
        #region UAT-3345
        /// <summary>
        /// 
        ///UAT-3345 Saves all document mappings of the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public Int32 saveDocumentMappings()
        {
            var ReqCatDataId = this.ucRequirementVerificationDetailsDocumentConrol.SaveItemDocumentMappings();
            SetItemAndFieldDataIdInContext();

            return ReqCatDataId;
        }

        private void SetItemAndFieldDataIdInContext()
        {

            if (CurrentViewContext.ApplReqItemDataId <= 0 && this.ucRequirementVerificationDetailsDocumentConrol.CurrentViewContext.RequirementItemDataId > 0)
            {
                CurrentViewContext.ApplReqItemDataId = this.ucRequirementVerificationDetailsDocumentConrol.CurrentViewContext.RequirementItemDataId;
                if (this.ucRequirementVerificationDetailsDocumentConrol.RequirementFieldDataId > 0)
                {
                    var DocfieldData = CurrentViewContext.lstItemLevelData.FirstOrDefault(vdd => vdd.FieldId == this.ucRequirementVerificationDetailsDocumentConrol.RequirementFieldId);
                    if (DocfieldData.ApplReqFieldDataId <= 0)
                        DocfieldData.ApplReqFieldDataId = this.ucRequirementVerificationDetailsDocumentConrol.RequirementFieldDataId;

                }
            }
        }
        #endregion


        /// <summary>
        /// Get the Item and it's Field related Data 
        /// </summary>
        /// <returns></returns>
        public RequirementVerificationItemData GetItemFieldData()
        {
            if (rbtnListStatus.SelectedValue == RequirementItemStatus.INCOMPLETE.GetStringValue() && !chkDeleteItem.Checked)
            {
                return null;
            }

            var _reqVerificationItemData = new RequirementVerificationItemData();
            _reqVerificationItemData.lstFieldData = new List<RequirementVerificationFieldData>();
            _reqVerificationItemData.ItemId = CurrentViewContext.ItemId;
            _reqVerificationItemData.ApplicantItemDataId = CurrentViewContext.ApplReqItemDataId;
            _reqVerificationItemData.ItemStatusCode = rbtnListStatus.SelectedValue;
            _reqVerificationItemData.RejectionReason = txtRejectionreason.Text;
            _reqVerificationItemData.IsItemMarkedAsDeleted = chkDeleteItem.Checked;

            _reqVerificationItemData.IsFileUploadApplicable = this.IsFileUpoloadApplicable;
            _reqVerificationItemData.FileUploadAttributeId = this.FileUpoladFieldId;

            var _distinctFields = CurrentViewContext.lstItemLevelData.OrderBy(vdd => vdd.FieldId).Select(vdd => vdd.FieldId).Distinct().ToList();

            foreach (var fieldId in _distinctFields)
            {
                var _ctrl = divFieldContainer.FindServerControlRecursively(CurrentViewContext.FieldControlIdPrefix + fieldId);

                //UAT-2679
                if (chkDeleteItem.Checked && _ctrl.IsNull())
                {
                    var field = CurrentViewContext.lstItemLevelData.Where(cond => cond.FieldId == fieldId).FirstOrDefault();

                    if (!field.IsNullOrEmpty() && field.FieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue())
                    {
                        _reqVerificationItemData.lstFieldData.Add(new RequirementVerificationFieldData
                        {
                            FieldTypeCode = field.FieldDataTypeCode,
                            FieldId = fieldId,
                            ApplicantFieldDataId = field.ApplReqFieldDataId,
                        });
                    }
                }


                if (_ctrl.IsNotNull() && _ctrl is RequirementVerificationFieldControl)
                {
                    var _fieldControl = _ctrl as RequirementVerificationFieldControl;
                   var _fieldData = ((CoreWeb.ClinicalRotation.Views.IRequirementVerificationFieldControl)_fieldControl).FieldData;

                    var _hdfFieldTypeCode = (_fieldControl.FindControl("hdfFieldTypeCode") as HiddenField);
                    var _hdfApplicantFieldDataId = (_fieldControl.FindControl("hdfApplicantFieldDataId") as HiddenField);
                    var _hdfFieldId = (_fieldControl.FindControl("hdfFieldId") as HiddenField);

                    if (_hdfFieldTypeCode.Value == RequirementFieldDataType.DATE.GetStringValue())
                    {
                        var _dateCtrl = _fieldControl.FindControl("dp" + CurrentViewContext.FieldControlIdGenerator + _hdfFieldId.Value) as WclDatePicker;
                        _reqVerificationItemData.lstFieldData.Add(new RequirementVerificationFieldData
                        {
                            FieldTypeCode = _hdfFieldTypeCode.Value,
                            FieldId = Convert.ToInt32(_hdfFieldId.Value),
                            ApplicantFieldDataId = Convert.ToInt32(_hdfApplicantFieldDataId.Value),
                            ApplicantFieldDataValue = Convert.ToString(_dateCtrl.SelectedDate)
                        });
                        //Code Commented for UAT 4380
                        //if (!CurrentViewContext.IsItemEditable)
                        //{
                        //    _dateCtrl.Enabled = false;
                        //}
                        //UAT 4380
                        if (_fieldControl.IsAdminLoggedIn)
                        {
                            if (!_fieldData.IsFieldEditableByAdmin)
                                _dateCtrl.Enabled = false;
                        }
                        if (_fieldControl.IsClientAdminLoggedIn)
                        {
                            if (!_fieldData.IsFieldEditableByAdmin)
                                _dateCtrl.Enabled = false;
                        }
                    }
                    else if (_hdfFieldTypeCode.Value == RequirementFieldDataType.OPTIONS.GetStringValue())
                    {
                        var _comboCtrl = _fieldControl.FindControl("combo" + CurrentViewContext.FieldControlIdGenerator + _hdfFieldId.Value) as WclComboBox;
                        _reqVerificationItemData.lstFieldData.Add(new RequirementVerificationFieldData
                        {
                            FieldTypeCode = _hdfFieldTypeCode.Value,
                            FieldId = Convert.ToInt32(_hdfFieldId.Value),
                            ApplicantFieldDataId = Convert.ToInt32(_hdfApplicantFieldDataId.Value),
                            ApplicantFieldDataValue = Convert.ToString(_comboCtrl.SelectedValue)
                        });
                        //Code Commented for UAT 4380
                        //if (!CurrentViewContext.IsItemEditable)
                        //{
                        //    _comboCtrl.Enabled = false;
                        //}
                        //UAT 4380
                        if (_fieldControl.IsAdminLoggedIn)
                        {
                            if (!_fieldData.IsFieldEditableByAdmin)
                                _comboCtrl.Enabled = false;
                        }
                        if (_fieldControl.IsClientAdminLoggedIn)
                        {
                            if (!_fieldData.IsFieldEditableByAdmin)
                                _comboCtrl.Enabled = false;
                        }
                    }
                    else if (_hdfFieldTypeCode.Value == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue()
                           || _hdfFieldTypeCode.Value == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue()
                           || _hdfFieldTypeCode.Value == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                    {
                        _reqVerificationItemData.lstFieldData.Add(new RequirementVerificationFieldData
                        {
                            FieldTypeCode = _hdfFieldTypeCode.Value,
                            FieldId = Convert.ToInt32(_hdfFieldId.Value),
                            ApplicantFieldDataId = Convert.ToInt32(_hdfApplicantFieldDataId.Value),
                            ApplicantFieldDataValue = AppConsts.ZERO
                        });
                    }
                    else if (_hdfFieldTypeCode.Value == RequirementFieldDataType.TEXT.GetStringValue())
                    {
                        var _textCtrl = _fieldControl.FindControl("txt" + CurrentViewContext.FieldControlIdGenerator + _hdfFieldId.Value) as WclTextBox;
                        _reqVerificationItemData.lstFieldData.Add(new RequirementVerificationFieldData
                        {
                            FieldTypeCode = _hdfFieldTypeCode.Value,
                            FieldId = Convert.ToInt32(_hdfFieldId.Value),
                            ApplicantFieldDataId = Convert.ToInt32(_hdfApplicantFieldDataId.Value),
                            ApplicantFieldDataValue = Convert.ToString(_textCtrl.Text)
                        });
                        //Code Commented for UAT 4380
                        //if (!CurrentViewContext.IsItemEditable)
                        //{
                        //    _textCtrl.Enabled = false;
                        //}
                        //UAT 4380
                        if (_fieldControl.IsAdminLoggedIn)
                        {
                            if (!_fieldData.IsFieldEditableByAdmin)
                                _textCtrl.Enabled = false;
                        }
                        if (_fieldControl.IsClientAdminLoggedIn)
                        {
                            if (!_fieldData.IsFieldEditableByAdmin)
                                _textCtrl.Enabled = false;
                        }
                    }
                }
            }

            if (!chkDeleteItem.Checked)
            {
                //UAT-2224: Admin access to upload/associate documents on rotation package items.
                if (this.IsFileUpoloadApplicable && ucRequirementVerificationDetailsDocumentConrol.lstApplicantRequirementDocumentMaps.IsNullOrEmpty()
                    && ucRequirementVerificationDetailsDocumentConrol.IsFieldRequired)
                {
                    _reqVerificationItemData.IsDocFieldValidationFailed = true;
                    ucRequirementVerificationDetailsDocumentConrol.ShowHideValidationMessage("Document is required.", true);
                }
            }

            return _reqVerificationItemData;
        }

        #region UAT-4260
        public void SetValidationMessage(String validationMessage)
        {
            lblMessage.Text = validationMessage;
            lblMessage.CssClass = "error";
        }
        #endregion

        #endregion

        #region Private Methods

        private void HighlightAssignedItems(Boolean highlightItemBackground)
        {
            if (highlightItemBackground)
            {
                dvlnkItemName.Attributes.Add("class", "highlightAssignedItem");
                dvDetailPanel.Attributes.Add("class", "highlightDetailBorder sxform auto");
            }
        }
        private void GenerateFields()
        {
            Int32 _attributesPerRow = 3;
            String uploadDocTypeCode = RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue();

            // List of current Fields to be added into the control
            List<RequirementVerificationDetailContract> lstDstctFields = CurrentViewContext.lstItemLevelData.Where(cond => cond.FieldDataTypeCode != uploadDocTypeCode).OrderBy(vdd => vdd.RequirementItemFieldDisplayOrder).DistinctBy(vdd => vdd.FieldId).ToList();
            List<RequirementVerificationDetailContract> lstDocTypeField = CurrentViewContext.lstItemLevelData.Where(cond => cond.FieldDataTypeCode == uploadDocTypeCode).OrderBy(vdd => vdd.RequirementItemFieldDisplayOrder).DistinctBy(vdd => vdd.FieldId).ToList();

            foreach (RequirementVerificationDetailContract docTypeField in lstDocTypeField)
            {
                lstDstctFields.Add(docTypeField);
            }

            List<RequirementVerificationDetailContract> lstFieldsToAdd = lstDstctFields;

            // Maintains list of attributes that are already added
            List<Int32> lstAddedFieldIds = new List<Int32>();

            int _attributesAdded = 0;
            for (int i = 1; i <= Math.Ceiling(Convert.ToDecimal(lstDstctFields.Count()) / _attributesPerRow); i++)
            {
                if (_attributesAdded == _attributesPerRow)
                {
                    _attributesAdded = 0;
                    lstFieldsToAdd = new List<RequirementVerificationDetailContract>();

                    foreach (var att in lstDstctFields)
                    {
                        if (!lstAddedFieldIds.Contains(att.FieldId))
                        {
                            lstFieldsToAdd.Add(att);
                        }
                    }
                }
                lstFieldsToAdd = lstFieldsToAdd.Take(_attributesPerRow).ToList();
                lstAddedFieldIds.AddRange(lstFieldsToAdd.Select(att => att.FieldId));
                GenerateFieldRow(lstFieldsToAdd.Select(sel => sel.FieldId).ToList());
                _attributesAdded += _attributesPerRow;
            }
        }

        /// <summary>
        /// Generate the Field controls for the current item.
        /// </summary>
        private void GenerateFieldRow(List<Int32> _distinctFields)
        {
            var _comboTypeCode = RequirementFieldDataType.OPTIONS.GetStringValue();
            var _uploadDocTypeCode = RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue();

            System.Web.UI.HtmlControls.HtmlGenericControl dicRow = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            //dicRow.Attributes.Add("class", "col-md-12");

            System.Web.UI.HtmlControls.HtmlGenericControl dicRow1 = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            //dicRow1.Attributes.Add("class", "row");
            dicRow.Controls.Add(dicRow1);

            _distinctFields = _distinctFields.Where(cond => cond > 0).ToList();

            foreach (var fieldId in _distinctFields)
            {
                var _lstcurrentFieldData = CurrentViewContext.lstItemLevelData.Where(vdd => vdd.FieldId == fieldId).ToList();
                var _lstcurrentFieldDataTypeCode = _lstcurrentFieldData.FirstOrDefault().FieldDataTypeCode;

                //UAT-2224: Admin access to upload/associate documents on rotation package items.
                if (!(_lstcurrentFieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue()))
                {
                    System.Web.UI.Control _fieldControl = Page.LoadControl("~/ClinicalRotation/UserControl/RequirementVerificationFieldControl.ascx");
                    (_fieldControl as IRequirementVerificationFieldControl).FieldData = CurrentViewContext.lstItemLevelData.Where(vdd => vdd.FieldId == fieldId).OrderBy(o => o.RequirementItemFieldDisplayOrder).First();
                    (_fieldControl as IRequirementVerificationFieldControl).SelectedTenantId = CurrentViewContext.SelectedTenantId;
                    (_fieldControl as RequirementVerificationFieldControl).ID = CurrentViewContext.FieldControlIdPrefix + fieldId;
                    (_fieldControl as IRequirementVerificationFieldControl).ControlUseType = CurrentViewContext.ControlUseType;
                    (_fieldControl as IRequirementVerificationFieldControl).IsItemEditable = CurrentViewContext.IsItemEditable;
                    //UAT 4380
                    (_fieldControl as IRequirementVerificationFieldControl).IsAdminLoggedIn = CurrentViewContext.IsAdminLoggedIn;
                    (_fieldControl as IRequirementVerificationFieldControl).IsClientAdminLoggedIn = CurrentViewContext.IsClientAdminLoggedIn;
                    if (_lstcurrentFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
                    {

                        #region Get Combobox Options

                        var _dic = new Dictionary<String, String>();

                        foreach (var comboItem in _lstcurrentFieldData.Distinct())
                        {
                            //UAT-4244
                            if (!_dic.ContainsKey(comboItem.OptionValue))
                            {
                                _dic.Add(comboItem.OptionValue, comboItem.OptionText);
                            }
                        }
                        (_fieldControl as IRequirementVerificationFieldControl).dicComboData = _dic;

                        #endregion
                    }
                    //UAT-2224: Admin access to upload/associate documents on rotation package items.
                    //else if (_lstcurrentFieldData.First().FieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue())
                    //{
                    //    #region Get documents for the DocumentUploadType Field

                    //    var lstDocuments = new List<Tuple<Int32, String, String>>();

                    //    if (_lstcurrentFieldData.Any(dt => dt.ApplDocId > AppConsts.NONE))
                    //    {
                    //        foreach (var doc in _lstcurrentFieldData)
                    //        {
                    //            var _tplDocument = new Tuple<Int32, String, String>(doc.ApplDocId, doc.FieldDocName, doc.FieldDocPath);
                    //            lstDocuments.Add(_tplDocument);
                    //        }
                    //    }
                    //    (_fieldControl as IRequirementVerificationFieldControl).lstDocuments = lstDocuments;

                    //    #endregion
                    //}
                    else if (_lstcurrentFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                    {
                        #region UAT-1470 :As a student, there should be a way to close out of the video once you open it.
                        Int32 reqObjectTreeId = AppConsts.NONE;
                        String attObjectTypeCode = LCObjectType.ComplianceATR.GetStringValue();
                        String objectAttributeTypeCode = ObjectAttribute.REQUIRED.GetStringValue();
                        String boxOpentime = String.Empty;
                        if (CurrentViewContext.LstRequirementObjTreeProperty.IsNotNull())
                        {
                            var fieldProperty = CurrentViewContext.LstRequirementObjTreeProperty.FirstOrDefault(cond => cond.ObjectID == fieldId
                                                                                            && cond.ObjectTypeCode == attObjectTypeCode
                                                                                            && cond.ObjectAttributeTypeCode == objectAttributeTypeCode);

                            if (fieldProperty.IsNotNull())
                            {
                                reqObjectTreeId = fieldProperty.RequirementObjectTreeID;
                                boxOpentime = Presenter.GetObjectAttrProperties(reqObjectTreeId);
                            }
                        }

                        (_fieldControl as IRequirementVerificationFieldControl).VideoRequiredOpenTime = boxOpentime;

                        #endregion

                    }
                    else if (_lstcurrentFieldDataTypeCode == RequirementFieldDataType.TEXT.GetStringValue())
                    {

                    }
                    dicRow.Controls.Add(_fieldControl);
                }
            }

            //System.Web.UI.HtmlControls.HtmlGenericControl dicRowEnd = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            //dicRowEnd.Attributes.Add("class", "sxroend");
            //dicRow.Controls.Add(dicRowEnd);
            divFieldContainer.Controls.Add(dicRow);
        }

        /// <summary>
        /// Bind the ItemStatus radio-buttons and enable/disable them based on condition
        /// </summary>
        /// <param name="value"></param>
        private void BindItemStatus(List<RequirementItemStatusContract> value)
        {
            var _lkpItemStatus = value.Where(ris => ris.ReqItemstatusCode != RequirementItemStatus.SUBMITTED.GetStringValue()).ToList();

            foreach (var itemStatus in _lkpItemStatus)
            {
                var _isEnabled = true;
                var _itemStsText = String.Empty;
                if (itemStatus.ReqItemstatusCode == RequirementItemStatus.EXPIRED.GetStringValue() ||
                    (CurrentViewContext.ApplReqItemDataId != AppConsts.NONE && itemStatus.ReqItemstatusCode == RequirementItemStatus.INCOMPLETE.GetStringValue())
                    //|| (CurrentViewContext.ControlUseType == AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE.ToLower().Trim())
                    )
                {
                    _isEnabled = false;
                }

                if (itemStatus.ReqItemstatusCode == RequirementItemStatus.NOT_APPROVED.GetStringValue())
                {
                    //UAT-1860-Change "Approved" and "Rejected" status text for items to "Meets Requirements" and "Does Not Meet Requirements"
                    _itemStsText = "Does Not Meet Requirements";
                }
                //UAT-4543
                if (!CurrentViewContext.IsAdminLoggedIn && itemStatus.ReqItemstatusCode == RequirementItemStatus.APPROVED.GetStringValue())
                {
                    _isEnabled = CurrentViewContext.IsViewDocFieldViewed;
                    _itemStsText = itemStatus.ReqItemstatusName;
                }
                //END UAT
                else
                {
                    _itemStsText = itemStatus.ReqItemstatusName;
                }
                rbtnListStatus.Items.Add(new ListItem
                {
                    Text = _itemStsText,
                    Value = itemStatus.ReqItemstatusCode,
                    Enabled = _isEnabled
                });
            }
            rbtnListStatus.SelectedValue = CurrentViewContext.lstItemLevelData.First().ItemStatusCode;
        }

        /// <summary>
        /// Get the ImageUrl for the ItemStatus icone
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private string GetImagePathItemStatus(string status)
        {
            if (status == RequirementItemStatus.INCOMPLETE.GetStringValue()
             || status == RequirementItemStatus.NOT_APPROVED.GetStringValue()
             || status == RequirementItemStatus.EXPIRED.GetStringValue())
            {
                return ResolveUrl("~/Resources/Mod/Compliance/icons/no16.png");
            }
            else if (status == RequirementItemStatus.APPROVED.GetStringValue())
            {
                return ResolveUrl("~/Resources/Mod/Compliance/icons/yes16.png");
            }
            else if (status == RequirementItemStatus.PENDING_REVIEW.GetStringValue())
            {
                return ResolveUrl("~/Resources/Mod/Compliance/icons/attn16.png");
            }
            return ResolveUrl("~/Resources/Mod/Compliance/icons/no16.png");
        }
        #endregion

        #endregion

    }
}
