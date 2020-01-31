using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Text;
using CoreWeb.ComplianceOperations.Views;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.Configuration;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;


namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public partial class RequirementItemForm : BaseUserControl, IRequirementItemFormView
    {
        #region Variables

        #region Private Variables
        private Int32 _FieldsPerRow;
        private RequirementItemFormPresenter _presenter = new RequirementItemFormPresenter();
        private ApplicantRequirementFieldDataContract _viewContract;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public RequirementItemFormPresenter Presenter
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
        /// Manage the display of upload documents control on main form. value is set through the delegate calls
        /// </summary>
        public Boolean IsFileUploadRequired { get; set; }

        // <summary>
        /// Manage the display of save button on main form. value is set through the delegate calls
        /// </summary>
        public Boolean IsViewDocumentRequired { get; set; }

        // <summary>
        /// Manage the display of save button on main form. value is set through the delegate calls
        /// </summary>
        public Boolean IsViewVideoRequired { get; set; }


        public RequirementItemContract RequirementItem
        {
            get;
            set;
        }

        public Boolean ReadOnly
        {
            get;
            set;
        }

        public String ItemName
        {
            get;
            set;
        }

        public List<RequirementFieldContract> RequirementItemFields
        {
            get;
            set;
        }

        public IRequirementItemFormView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Id of the Master compliance Item (Client database), present in the ApplicantComplianceItem Entity. 
        /// </summary>
        public Int32 ItemId
        {
            get;
            set;
        }

        public ApplicantRequirementFieldDataContract ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ApplicantRequirementFieldDataContract();
                }
                return _viewContract;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public Int32 TenantId
        {
            get
            {
                if (ViewState["TenantId"].IsNotNull())
                    return Convert.ToInt32(ViewState["TenantId"]);
                return 0;
            }
            set
            {
                ViewState["TenantId"] = Convert.ToString(value);
            }
        }

        public Int32 RequirementPkgSubscriptionId
        {
            get
            {
                if (ViewState["RequirementPkgSubscriptionId"].IsNotNull())
                    return Convert.ToInt32(ViewState["RequirementPkgSubscriptionId"]);
                return 0;
            }
            set
            {
                ViewState["RequirementPkgSubscriptionId"] = Convert.ToString(value);
            }
        }

        public Int32 RequirementCategoryId
        {
            get;
            set;
        }

        public Dictionary<Int32, Int32> AttributeDocuments
        {
            get;
            set;
        }

        public ApplicantRequirementItemDataContract RequirementItemData
        {
            get;
            set;
        }

        public Int32 ApplicantRequirementItemId
        {
            get;
            set;
        }
        public Int32 MaxFileSize
        {
            get
            {
                return Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
            }
        }

        public Int32 RequirementPackageId { get; set; }

        public List<RequirementObjectTreeContract> LstRequirementObjTreeProperty { get; set; }

        //UAT-3639
        public String DropzoneID
        {
            get { return this._dropZoneId; }
            set { _dropZoneId = value; }
        }
        private String _dropZoneId = "RequirementItemFormDropZone" + DateTime.Now.Ticks.ToString();

        #endregion

        #endregion

        #region Events

        //UAT-3639
        protected override void OnPreRender(EventArgs e)
        {
            String[] dropzone = new String[] { "#" + this.DropzoneID };
            fupItemData.DropZones = dropzone;
            this.fupItemData.Localization.Select = "Hidden";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //UAT-2449
            LoadControl();
        }

        #endregion

        #region Methods

        #region Private Methods

        //Comented For clinical rotation
        //private void GenerateNotesBox()
        //{
        //    WclTextBox txt = new WclTextBox();
        //    txt.ID = "txtItemNotes";
        //    txt.TextMode = Telerik.Web.UI.InputMode.MultiLine;
        //    txt.Height = Unit.Pixel(75);
        //    if (CurrentViewContext.ApplicantItemData.IsNotNull())
        //    {
        //        txt.Text = CurrentViewContext.ApplicantItemData.Notes;

        //        ///Set the ApplicantComplianceItemId, to manage the case, in case client admin adds any attribute, AFTER applicant has added the notes for this item.
        //        hdfApplicantItemDataId.Value = Convert.ToString(CurrentViewContext.ApplicantItemData.ApplicantComplianceItemID);
        //    }
        //    pnlNotes.Controls.Add(txt);
        //}

        private void GenerateFields()
        {
            _FieldsPerRow = 2;
            if (CurrentViewContext.RequirementItem.IsNull() ||
               (CurrentViewContext.RequirementItem.IsNotNull() && CurrentViewContext.RequirementItem.LstRequirementField.IsNull()))
            {
                return;
            }

            ShowForm(true);
            CurrentViewContext.RequirementItemFields = CurrentViewContext.RequirementItem.LstRequirementField.ToList();

            if (CurrentViewContext.RequirementItemFields.IsNull() && CurrentViewContext.RequirementItemFields.Count() == 0)
            {
                return;
            }

            //lblItemName.Text = CurrentViewContext.ClientComplianceItem.ScreenLabel;
            //Comented For clinical rotation
            //CurrentViewContext.ClientItemAttributes = ClientItemAttributes.Where(c => c.ComplianceAttribute.IsActive && !c.ComplianceAttribute.IsDeleted && c.CIA_IsActive && !c.CIA_IsDeleted)
            //    .OrderBy(att => att.CIA_DisplayOrder).ThenBy(x => x.CIA_AttributeID).ToList();

            #region MANAGE THE NUMBER OF Fields PER ROW, THAT SHOULD BE ADDED

            // List of current attributes to be added into the control
            List<RequirementFieldContract> lst = CurrentViewContext.RequirementItemFields.Where(cond => cond.RequirementFieldData.RequirementFieldDataTypeCode != "AAAG").Take(_FieldsPerRow).ToList();

            // Maintains list of attributes that are already added
            List<Int32> lstTemporary = new List<Int32>();

            int _fieldsAdded = 0;
            for (int i = 1; i <= Math.Ceiling(Convert.ToDecimal(CurrentViewContext.RequirementItemFields.Where(cond => cond.RequirementFieldData.RequirementFieldDataTypeCode != "AAAG").Count()) / _FieldsPerRow); i++)
            {
                if (_fieldsAdded == _FieldsPerRow)
                {
                    _fieldsAdded = 0;
                    lst = new List<RequirementFieldContract>();

                    foreach (var field in CurrentViewContext.RequirementItemFields.Where(cond => cond.RequirementFieldData.RequirementFieldDataTypeCode != "AAAG"))
                    {
                        if (!lstTemporary.Contains(field.RequirementItemFieldID))
                        {
                            lst.Add(field);
                        }
                    }
                }
                lst = lst.Take(_FieldsPerRow).ToList();
                lstTemporary.AddRange(lst.Select(att => att.RequirementItemFieldID));
                AddRow(lst);
                _fieldsAdded += _FieldsPerRow;
            }

            foreach (var item in CurrentViewContext.RequirementItemFields.Where(cond => cond.RequirementFieldData.RequirementFieldDataTypeCode == "AAAG").ToList())
            {
                var signatureAttr = new List<RequirementFieldContract>();
                signatureAttr.Add(item);
                AddRow(signatureAttr);
            }

            #endregion
        }

        /// <summary>
        ///  Add the row control for the selected attributes.
        /// </summary>
        /// <param name="lstAttributes">List of attributes for which Row control is to be added</param>
        private void AddRow(List<RequirementFieldContract> lstAttributes)
        {
            System.Web.UI.Control fieldRow = Page.LoadControl("~\\ApplicantRotationRequirement\\UserControl\\RequirementRowControl.ascx");
            (fieldRow as RequirementRowControl).RequirementItemFields = lstAttributes;
            (fieldRow as RequirementRowControl).NoOfFieldsPerRow = _FieldsPerRow;
            (fieldRow as RequirementRowControl).ItemId = CurrentViewContext.ItemId;
            (fieldRow as RequirementRowControl).TenantId = CurrentViewContext.TenantId;
            //Implemented code for UAT-708
            (fieldRow as RequirementRowControl).CategoryId = CurrentViewContext.RequirementCategoryId;
            //(fieldRow as RequirementRowControl).PackageId = CurrentViewContext.Re;

            (fieldRow as RequirementRowControl).IsFileUploadApplicable -= new EventHandler(RowControl_IsFileUploadApplicable);
            (fieldRow as RequirementRowControl).IsFileUploadApplicable += new EventHandler(RowControl_IsFileUploadApplicable);

            (fieldRow as RequirementRowControl).IsViewDocumnetApplicable -= new EventHandler(RowControl_IsViewDocumnetApplicable);
            (fieldRow as RequirementRowControl).IsViewDocumnetApplicable += new EventHandler(RowControl_IsViewDocumnetApplicable);

            (fieldRow as RequirementRowControl).IsViewVideoApplicable -= new EventHandler(RowControl_IsViewVideoApplicable);
            (fieldRow as RequirementRowControl).IsViewVideoApplicable += new EventHandler(RowControl_IsViewVideoApplicable);
            (fieldRow as RequirementRowControl).LstRequirementObjTreeProperty = LstRequirementObjTreeProperty;


            #region SET THE APPLICANTCOMPLIANCEITEM-ID & PASS THE DATA OF THE ATTRIBUTES, WHICH ARE BEING LOADED, IN EDIT CASE

            if (CurrentViewContext.RequirementItemData.IsNotNull())
            {
                (fieldRow as RequirementRowControl).ApplicantRequirementItemId = CurrentViewContext.RequirementItemData.RequirementItemDataID;
                if (CurrentViewContext.RequirementItemData.ApplicantRequirementFieldData.IsNotNull())
                {
                    (fieldRow as RequirementRowControl).ApplicantRequirementFieldData = CurrentViewContext.RequirementItemData.ApplicantRequirementFieldData.ToList();
                }

                hdfApplicantReqItemDataId.Value = Convert.ToString(CurrentViewContext.RequirementItemData.RequirementItemDataID);
                //Changes related to bud ID:15048
                hdnItemStatusCode.Value = Convert.ToString(CurrentViewContext.RequirementItemData.RequirementItemStatusCode);
            }

            #endregion

            pnl.Controls.Add(fieldRow);
        }

        private void ShowForm(Boolean visibility)
        {
            pnlForm.Visible = visibility;
            pnlMessage.Visible = visibility;
        }

        void RowControl_IsFileUploadApplicable(object sender, EventArgs e)
        {
            this.IsFileUploadRequired = true;
        }

        void RowControl_IsViewDocumnetApplicable(object sender, EventArgs e)
        {
            this.IsViewDocumentRequired = true;
        }

        void RowControl_IsViewVideoApplicable(object sender, EventArgs e)
        {
            this.IsViewVideoRequired = true;
        }

        private WclComboBox GetCombo(String cmbName)
        {
            WclComboBox cmb = null;
            Control ctl = this.Parent;
            while (true)
            {
                cmb = (WclComboBox)ctl.FindControl(cmbName);
                if (cmb.IsNull())
                {
                    if (ctl.Parent == null)
                        return cmb;
                    ctl = ctl.Parent;
                    continue;
                }
                return cmb;
            }
        }

        private HiddenField GetHiddenField(String hdfName)
        {
            HiddenField hdf = null;
            Control ctl = this.Parent;
            while (true)
            {
                hdf = (HiddenField)ctl.FindControl(hdfName);
                if (hdf.IsNull())
                {
                    if (ctl.Parent == null)
                        return hdf;
                    ctl = ctl.Parent;
                    continue;
                }
                return hdf;
            }
        }

        #endregion

        #region Public Methods
        //UAT-2449 
        public void LoadControl()
        {
            ShowForm(false);

            #region GET THE SELECTED COMPLIANCE ITEM ID, TO GENERATE THE DYNAMIC FORM

            //ItemId is required to generate the dynamic controls, whether add or edit mode. 
            // This one, gets the ItemId for the Add Mode
            WclComboBox ddItems = GetCombo("cmbRequirement");
            CurrentViewContext.ItemId = String.IsNullOrEmpty(ddItems.SelectedValue) ? AppConsts.NONE : Convert.ToInt32(ddItems.SelectedValue);

            //Get the ItemId for the Edit Mode 
            if (CurrentViewContext.ItemId == 0)
            {
                HiddenField hdfRequirementItemId = GetHiddenField("hdfRequirementItemId");
                CurrentViewContext.ItemId = !String.IsNullOrEmpty(hdfRequirementItemId.Value) ? Convert.ToInt32(hdfRequirementItemId.Value) : AppConsts.NONE;

                HiddenField hdfRequirementPkgSubscriptionID = GetHiddenField("hdfRequirementPkgSubscriptionID");
                CurrentViewContext.RequirementPkgSubscriptionId = !String.IsNullOrEmpty(hdfRequirementPkgSubscriptionID.Value) ?
                                                                   Convert.ToInt32(hdfRequirementPkgSubscriptionID.Value) : AppConsts.NONE;

            }
            if (CurrentViewContext.RequirementCategoryId == AppConsts.NONE)
            {
                HiddenField hdRequirementCategoryId = GetHiddenField("hdRequirementCategoryId");
                CurrentViewContext.RequirementCategoryId = !String.IsNullOrEmpty(hdRequirementCategoryId.Value) ? Convert.ToInt32(hdRequirementCategoryId.Value) : AppConsts.NONE;
            }
            HiddenField hdRequirementPackageId = GetHiddenField("hdRequirementPackageId");
            CurrentViewContext.RequirementPackageId = !String.IsNullOrEmpty(hdRequirementPackageId.Value) ? Convert.ToInt32(hdRequirementPackageId.Value) : AppConsts.NONE;

            HiddenField hdfTenantId = GetHiddenField("hdfTenantId");
            CurrentViewContext.TenantId = !String.IsNullOrEmpty(hdfTenantId.Value) ? Convert.ToInt32(hdfTenantId.Value) : AppConsts.NONE;

            Presenter.GetApplicantRequirementItemData();
            Presenter.GetRequirementItemForControls();
            Presenter.GetAttributeObjectTreeProperties();


            #region UAT-3077
            var result = Presenter.CheckItemPayment(); 
             if (!CurrentViewContext.RequirementItem.IsNullOrEmpty() && !CurrentViewContext.RequirementItem.IsPaymentType)
            {
                dvItemPayment.Visible = false;
            }
            else
            {
                if (!CurrentViewContext.RequirementItem.IsNullOrEmpty())
                {
                    if (CurrentViewContext.RequirementItem.IsPaymentType)
                    {
                        dvItemPayment.Visible = true;
                        //dvItemPayment.Attributes.Add("style", "display:block");
                        litLabel.Text = String.Concat("$", Convert.ToString(Math.Round(CurrentViewContext.RequirementItem.Amount.Value, 2, MidpointRounding.AwayFromZero)));
                        BindItemPaymentData(result.Item2);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CompleteItemPaymentClick('" + true + "');", true);
                    }
                    else
                    {
                        dvItemPayment.Visible = false;
                        //dvItemPayment.Attributes.Add("style", "display:none");
                    }
                }
                else
                {
                    dvItemPayment.Visible = false;
                    //dvItemPayment.Attributes.Add("style", "display:none");
                }
            }
            #endregion

            GenerateFields();

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            //Comented For clinical rotation
            //GenerateNotesBox();
            //UAT-766 Document Upload file size limit should be 20 mb
            fupItemData.MaxFileSize = MaxFileSize;
            #endregion
        }
        #endregion

        #endregion

        #region UAT-3077
        protected void lnkItemPayment_Click(object sender, EventArgs e)
        {
            try
            {
                #region Set Custom Data

                if (!CurrentViewContext.RequirementItem.IsNullOrEmpty())
                {
                    if (CurrentViewContext.RequirementItem.IsPaymentType)
                    {
                        BindItemPaymentData(null);
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "IsOrderCreated" , AppConsts.ZERO} 
                                                                 };

                        String url = String.Format("~/ComplianceOperations/Pages/ItemPaymentPopup.aspx");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenItemPaymentForm('" + url + "');", true);
                        //String url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", AppConsts.APPLICANT_PARKING_PAYMENT_CONTROL, queryString.ToEncryptedQueryString());
                        //Response.Redirect(url, true);
                    }
                }
                #endregion

            }
            catch (Exception)
            {

            }
        }
        private void BindItemPaymentData(Int32? orderID)
        {
            try
            {
                //hdfPackageSubscriptionID,hdnPackageName,hdnCategoryName,hdfPackageId
                HiddenField hdfPackageID = GetHiddenField("hdRequirementPackageId");
                HiddenField hdfPackageSubscriptionID = GetHiddenField("hdfRequirementPkgSubscriptionID");
                //    HiddenField hdnPackageName = GetHiddenField("hdnPackageName");
                // HiddenField hdnCategoryName = GetHiddenField("hdnCategoryName");
                HiddenField hdfComplianceCategoryId = GetHiddenField("hdRequirementCategoryId");
                HiddenField hdnClinicalRotationID = GetHiddenField("hdnClinicalRotationID");
                INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract oldData = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract;

                Session.Remove(ResourceConst.APPLICANT_PARKING_CART);
                INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract itemPaymentContract = new INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract();
                itemPaymentContract.PkgName = String.Empty;
                itemPaymentContract.CategoryName = String.Empty;
                itemPaymentContract.ItemID = !CurrentViewContext.RequirementItem.IsNullOrEmpty() ? Convert.ToInt32(CurrentViewContext.RequirementItem.RequirementItemID) : AppConsts.NONE;
                itemPaymentContract.CategoryID = !String.IsNullOrEmpty(hdfComplianceCategoryId.Value) ? Convert.ToInt32(hdfComplianceCategoryId.Value) : AppConsts.NONE;
                itemPaymentContract.ItemName = !String.IsNullOrEmpty(CurrentViewContext.RequirementItem.RequirementItemLabel) ? CurrentViewContext.RequirementItem.RequirementItemLabel : CurrentViewContext.RequirementItem.RequirementItemName;
                itemPaymentContract.PkgId = !String.IsNullOrEmpty(hdfPackageID.Value) ? Convert.ToInt32(hdfPackageID.Value) : AppConsts.NONE;
                itemPaymentContract.PkgSubscriptionId = Convert.ToInt32(hdfPackageSubscriptionID.Value);
                itemPaymentContract.ClinicalRotationID = !String.IsNullOrEmpty(hdnClinicalRotationID.Value) ? Convert.ToInt32(hdnClinicalRotationID.Value) : AppConsts.NONE;
                itemPaymentContract.TenantID = CurrentViewContext.TenantId;
                if (!oldData.IsNullOrEmpty())
                {
                    if (oldData.orderID.IsNotNull() && oldData.orderID > AppConsts.NONE && oldData.ItemID == itemPaymentContract.ItemID && oldData.PkgSubscriptionId == itemPaymentContract.PkgSubscriptionId && oldData.CategoryID == itemPaymentContract.CategoryID)
                    {
                        itemPaymentContract.orderID = oldData.orderID;
                    }
                }
                if (!CurrentViewContext.RequirementItem.IsNullOrEmpty())
                {
                    if (CurrentViewContext.RequirementItem.IsPaymentType)
                    {
                        itemPaymentContract.TotalPrice = CurrentViewContext.RequirementItem.Amount.Value;
                        itemPaymentContract.IsRequirementPackage = true;
                    }
                }
                if (orderID.HasValue)
                {
                    itemPaymentContract.orderID = orderID.Value;
                }
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_PARKING_CART, itemPaymentContract);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}

