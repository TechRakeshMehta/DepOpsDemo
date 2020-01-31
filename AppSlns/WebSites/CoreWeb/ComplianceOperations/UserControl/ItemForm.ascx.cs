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
using Entity.ClientEntity;
using System.Web.Configuration;
using Telerik.Web.UI;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ItemForm : BaseUserControl, IItemFormView
    {
        #region Variables

        #region Private Variables
        private Int32 _itemId;
        private Int32 _attributesPerRow;
        private ItemFormPresenter _presenter = new ItemFormPresenter();
        private ApplicantComplianceAttributeDataContract _viewContract;
        private Int32 _tenantId;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public ItemFormPresenter Presenter
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

        public ComplianceItem ClientComplianceItem
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

        public List<ComplianceItemAttribute> ClientItemAttributes
        {
            get;
            set;
        }

        public Boolean IsItemSeries
        {
            get;
            set;
        }

        public IItemFormView CurrentViewContext
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

        public ApplicantComplianceAttributeDataContract ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ApplicantComplianceAttributeDataContract();
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

        public List<ApplicantComplianceAttributeDataContract> lstAttributesData
        {
            get;
            set;
        }

        public Int32 PackageId
        {
            get
            {
                if (ViewState["PackageId"].IsNotNull())
                    return Convert.ToInt32(ViewState["PackageId"]);
                return 0;
            }
            set
            {
                ViewState["PackageId"] = Convert.ToString(value);
            }
        }

        public ApplicantComplianceItemDataContract ItemDataContract
        {
            get;
            set;
        }

        public ApplicantComplianceCategoryDataContract CategoryDataContract
        {
            get;
            set;
        }

        public Int32 ComplianceCategoryId
        {
            get;
            set;
        }

        public Dictionary<Int32, Int32> AttributeDocuments
        {
            get;
            set;
        }

        public ApplicantComplianceItemData ApplicantItemData
        {
            get;
            set;
        }

        public ApplicantComplianceCategoryData ApplicantCategoryData
        {
            get;
            set;
        }

        public Int32 ApplicantComplianceCategoryId
        {
            get;
            set;
        }

        public Int32 ApplicantComplianceItemId
        {
            get;
            set;
        }
        //UAT-766 Document Upload file size limit should be 20 mb
        public Int32 MaxFileSize
        {
            get
            {
                return Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
            }
        }

        //UAT-3639
        public String DropzoneID
        {
            get { return this._dropZoneId; }
            set { _dropZoneId = value; }
        }
        private String _dropZoneId = "ItemFormDropZone" + DateTime.Now.Ticks.ToString();


        #region UAT-1607:Student Data Entry Screen changes
        #endregion

        //UAT-4067
        public String AllowedExtensions
        {
            get;
            set;
        }
        public Boolean IsAllowedFileExtensionEnable
        {
            get;
            set;
        }

        public String SelectedNodeIds { get; set; }

        #endregion

        #endregion

        #region Events


        protected override void OnPreRender(EventArgs e)
        {
            String[] dropzone = new String[] { "#" + this.DropzoneID };
            fupItemData.DropZones = dropzone;
            this.fupItemData.Localization.Select = "Hidden";

            //UAT-4067
            if (IsAllowedFileExtensionEnable)
            {
                String[] allowedExtensions = AllowedExtensions.Split(',');
                fupItemData.AllowedFileExtensions = allowedExtensions;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //UAt-2453 related changes.
            LoadControl();

        }

        #endregion

        #region Methods

        #region Private Methods

        private void GenerateNotesBox()
        {
            WclTextBox txt = new WclTextBox();
            txt.ID = "txtItemNotes";
            txt.TextMode = Telerik.Web.UI.InputMode.MultiLine;
            txt.Height = Unit.Pixel(75);
            if (CurrentViewContext.ApplicantItemData.IsNotNull())
            {
                txt.Text = CurrentViewContext.ApplicantItemData.Notes;

                ///Set the ApplicantComplianceItemId, to manage the case, in case client admin adds any attribute, AFTER applicant has added the notes for this item.
                hdfApplicantItemDataId.Value = Convert.ToString(CurrentViewContext.ApplicantItemData.ApplicantComplianceItemID);
            }

            var ctrl = pnlNotes.FindControl("txtItemNotes");
            if (ctrl.IsNullOrEmpty())
            {
                pnlNotes.Controls.Add(txt);
            }
        }

        private void GenerateAttributes()
        {
            _attributesPerRow = 2;
            if (CurrentViewContext.ClientComplianceItem.IsNull())
                return;

            ShowForm(true);
            CurrentViewContext.ClientItemAttributes = CurrentViewContext.ClientComplianceItem.ComplianceItemAttributes.ToList();

            if (CurrentViewContext.ClientItemAttributes.Count() == 0)
            {
                return;
            }

            //lblItemName.Text = CurrentViewContext.ClientComplianceItem.ScreenLabel;
            CurrentViewContext.ClientItemAttributes = ClientItemAttributes.Where(c => c.ComplianceAttribute.IsActive && !c.ComplianceAttribute.IsDeleted && c.CIA_IsActive && !c.CIA_IsDeleted)
                .OrderBy(att => att.CIA_DisplayOrder).ThenBy(x => x.CIA_AttributeID).ToList();

            #region MANAGE THE NUMBER OF ATTRIBUTES PER ROW, THAT SHOULD BE ADDED

            // List of current attributes to be added into the control
            List<ComplianceItemAttribute> lst = CurrentViewContext.ClientItemAttributes.Where(cond => cond.ComplianceAttribute.lkpComplianceAttributeDatatype.Code != "ADTSIGN").Take(_attributesPerRow).ToList();

            // Maintains list of attributes that are already added
            List<Int32> lstTemporary = new List<Int32>();

            int _attributesAdded = 0;
            #region [UAT-3077]

            #endregion
            for (int i = 1; i <= Math.Ceiling(Convert.ToDecimal(CurrentViewContext.ClientItemAttributes.Where(cond => cond.ComplianceAttribute.lkpComplianceAttributeDatatype.Code != "ADTSIGN").Count()) / _attributesPerRow); i++)
            {
                if (_attributesAdded == _attributesPerRow)
                {
                    _attributesAdded = 0;
                    lst = new List<ComplianceItemAttribute>();

                    foreach (var att in CurrentViewContext.ClientItemAttributes.Where(cond => cond.ComplianceAttribute.lkpComplianceAttributeDatatype.Code != "ADTSIGN"))
                    {
                        if (!lstTemporary.Contains(att.CIA_ID))
                        {
                            lst.Add(att);
                        }
                    }
                }
                lst = lst.Take(_attributesPerRow).ToList();
                lstTemporary.AddRange(lst.Select(att => att.CIA_ID));
                AddRow(lst);
                _attributesAdded += _attributesPerRow;
            }

            foreach (var item in CurrentViewContext.ClientItemAttributes.Where(cond => cond.ComplianceAttribute.lkpComplianceAttributeDatatype.Code == "ADTSIGN"))
            {
                var signatureAttr = new List<ComplianceItemAttribute>();
                signatureAttr.Add(item);
                AddRow(signatureAttr);
            }

            #endregion
        }

        /// <summary>
        ///  Add the row control for the selected attributes.
        /// </summary>
        /// <param name="lstAttributes">List of attributes for which Row control is to be added</param>
        private void AddRow(List<ComplianceItemAttribute> lstAttributes)
        {
            System.Web.UI.Control attributeRow = Page.LoadControl("~\\ComplianceOperations\\UserControl\\RowControl.ascx");
            (attributeRow as RowControl).ClientItemAttributes = lstAttributes;
            (attributeRow as RowControl).NoOfAttributesPerRow = _attributesPerRow;
            (attributeRow as RowControl).ItemId = CurrentViewContext.ItemId;
            (attributeRow as RowControl).IsItemSeries = CurrentViewContext.IsItemSeries;
            (attributeRow as RowControl).TenantId = CurrentViewContext.TenantId;
            //Implemented code for UAT-708
            (attributeRow as RowControl).CategoryId = Convert.ToInt32(hdfInstructionTextCategoryId.Value);
            (attributeRow as RowControl).PackageId = CurrentViewContext.PackageId;

            (attributeRow as RowControl).IsFileUploadApplicable -= new EventHandler(RowControl_IsFileUploadApplicable);
            (attributeRow as RowControl).IsFileUploadApplicable += new EventHandler(RowControl_IsFileUploadApplicable);


            #region SET THE APPLICANTCOMPLIANCEITEM-ID & PASS THE DATA OF THE ATTRIBUTES, WHICH ARE BEING LOADED, IN EDIT CASE

            if (CurrentViewContext.ApplicantItemData.IsNotNull())
            {
                (attributeRow as RowControl).ApplicantComplianceItemId = CurrentViewContext.ApplicantItemData.ApplicantComplianceItemID;
                (attributeRow as RowControl).ApplicantAttributeData = CurrentViewContext.ApplicantItemData.ApplicantComplianceAttributeDatas.ToList();
                ////hdfApplicantItemDataId.Value = Convert.ToString(CurrentViewContext.ApplicantItemData.ApplicantComplianceItemID);
            }

            #endregion

            //UAT-4067
            //if (IsAllowedFileExtensionEnable)
            //{
            //    (attributeRow as RowControl).lstAllowedExtensions = AllowedExtensions.Split(',').ToList();
            //}

            pnl.Controls.Add(attributeRow);
        }

        private void ShowForm(Boolean visibility)
        {
            pnlForm.Visible = visibility;
        }

        void RowControl_IsFileUploadApplicable(object sender, EventArgs e)
        {
            this.IsFileUploadRequired = true;
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

        #region UAT-1607:Student Data Entry Screen changes
        private void SetItemSeriesProperty(WclComboBox ddItems)
        {
            if (!ddItems.IsNullOrEmpty())
            {
                RadComboBoxItem item = ddItems.SelectedItem;
                if (!item.IsNullOrEmpty())
                {
                    CurrentViewContext.IsItemSeries = Convert.ToBoolean(item.Attributes["IsItemSeries"]);
                }
            }
        }
        private Int32 GetItemIDFromDropDown(String selectedItemId)
        {
            if (!selectedItemId.IsNullOrEmpty())
            {
                var itemId = selectedItemId.Split('_');
                if (!itemId.IsNullOrEmpty())
                {
                    return Convert.ToInt32(itemId[1]);
                }
            }
            return AppConsts.NONE;
        }
        #endregion

        #endregion

        #region Public Methods

        public void LoadControl()
        {
            ShowForm(false);

            #region GET THE SELECTED COMPLIANCE ITEM ID, TO GENERATE THE DYNAMIC FORM

            //ItemId is required to generate the dynamic controls, whether add or edit mode. 
            // This one, gets the ItemId for the Add Mode
            WclComboBox ddItems = GetCombo("cmbRequirement");
            //UAT-1607: Student Data Entry Screen changes
            //Set IsItemSeries property to get the current item is series or item.
            SetItemSeriesProperty(ddItems);
            if (!CurrentViewContext.IsItemSeries)
            {
                CurrentViewContext.ItemId = String.IsNullOrEmpty(ddItems.SelectedValue) ? AppConsts.NONE : Convert.ToInt32(ddItems.SelectedValue);
            }
            else
            {
                CurrentViewContext.ItemId = GetItemIDFromDropDown(ddItems.SelectedValue);
            }

            //Get the ItemId for the Edit Mode 
            if (CurrentViewContext.ItemId == 0)
            {
                HiddenField hdfComplianceItemId = GetHiddenField("hdfComplianceItemId");
                CurrentViewContext.ItemId = !String.IsNullOrEmpty(hdfComplianceItemId.Value) ? Convert.ToInt32(hdfComplianceItemId.Value) : AppConsts.NONE;

                HiddenField hdfPackageSubscriptionId = GetHiddenField("hdfPackageId");
                CurrentViewContext.PackageId = !String.IsNullOrEmpty(hdfPackageSubscriptionId.Value) ? Convert.ToInt32(hdfPackageSubscriptionId.Value) : AppConsts.NONE;

                HiddenField hdfTenantId = GetHiddenField("hdfTenantId");
                CurrentViewContext.TenantId = !String.IsNullOrEmpty(hdfTenantId.Value) ? Convert.ToInt32(hdfTenantId.Value) : AppConsts.NONE;
            }
            if (CurrentViewContext.ComplianceCategoryId == AppConsts.NONE)
            {
                HiddenField hdfComplianceCategoryId = GetHiddenField("hdfComplianceCategoryId");
                hdfInstructionTextCategoryId.Value = !String.IsNullOrEmpty(hdfComplianceCategoryId.Value) ? Convert.ToString(hdfComplianceCategoryId.Value) : AppConsts.ZERO;
                CurrentViewContext.ComplianceCategoryId = !String.IsNullOrEmpty(hdfComplianceCategoryId.Value) ? Convert.ToInt32(hdfComplianceCategoryId.Value) : AppConsts.NONE;
            }

            if (!CurrentViewContext.IsItemSeries)
            {
                Presenter.GetApplicantData();
                Presenter.GetComplianceItemForControls();
                var result = Presenter.CheckItemPayment();
                dvNotes.Visible = true;
                //  if (result.Item1 && result.Item2 > AppConsts.NONE)
                if (!CurrentViewContext.ClientComplianceItem.IsNullOrEmpty() && !CurrentViewContext.ClientComplianceItem.IsPaymentType.Value)
                {
                    dvItemPayment.Visible = false;
                }
                else
                {
                    if (!CurrentViewContext.ClientComplianceItem.IsNullOrEmpty())
                    {
                        if (CurrentViewContext.ClientComplianceItem.IsPaymentType.Value)
                        {
                            dvItemPayment.Visible = true;
                            litLabel.Text = String.Concat("$", Convert.ToString(Math.Round(CurrentViewContext.ClientComplianceItem.Amount.Value, 2, MidpointRounding.AwayFromZero)));
                            lblOrderStatus.Text = "";
                            var complianceAttribiute = CurrentViewContext.ClientComplianceItem.ComplianceItemAttributes.Where(cond => !cond.CIA_IsDeleted && cond.ComplianceAttribute.IsActive).ToList();
                            if (complianceAttribiute.Count <= AppConsts.NONE)
                                dvNotes.Visible = false;
                            BindItemPaymentData(result.Item2);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CompleteItemPaymentClick('" + true + "');", true);
                        }
                        else
                        {
                            dvItemPayment.Visible = false;
                        }
                    }
                    else
                    {
                        dvItemPayment.Visible = false;
                    }
                }
            }
            else
            {
                Presenter.GetItemSeriesDataForControls();
            }

            //UAT-4067
            //HiddenField hdnSelectedNodeIds = GetHiddenField("hdnSelectedNodeIds");
            //CurrentViewContext.SelectedNodeIds = !String.IsNullOrEmpty(hdnSelectedNodeIds.Value) ? hdnSelectedNodeIds.Value : String.Empty;
            //Presenter.GetAllowedFileExtensions();
            //END UAT-4067
            GenerateAttributes();

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            GenerateNotesBox();
            //UAT-766 Document Upload file size limit should be 20 mb
            fupItemData.MaxFileSize = MaxFileSize;
            #endregion
        }
        #endregion

        protected void lnkItemPayment_Click(object sender, EventArgs e)
        {
            try
            {
                #region Set Custom Data
                //hdfPackageSubscriptionID,hdnPackageName,hdnCategoryName,hdfPackageId

                if (!CurrentViewContext.ClientComplianceItem.IsNullOrEmpty())
                {
                    if (CurrentViewContext.ClientComplianceItem.IsPaymentType.Value)
                    {
                        BindItemPaymentData(null);
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                  //  { "Child", AppConsts.APPLICANT_PARKING_PAYMENT_CONTROL} 
                                                                    { "StageCode", "AAAA" } 
                                                                 };
                        String url = String.Format("~/ComplianceOperations/Pages/ItemPaymentPopup.aspx?args={0}", queryString.ToEncryptedQueryString());
                        //Response.Redirect(url, true);
                        // String url = String.Format("~/ComplianceOperations/Pages/ItemPaymentPopup.aspx?StageCode=AAAA");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenItemPaymentForm('" + url + "');", true);
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
                HiddenField hdfPackageID = GetHiddenField("hdfPackageId");
                HiddenField hdfPackageSubscriptionID = GetHiddenField("hdfPackageSubscriptionID");
                HiddenField hdnPackageName = GetHiddenField("hdnPackageName");
                HiddenField hdnCategoryName = GetHiddenField("hdnCategoryName");
                HiddenField hdfComplianceCategoryId = GetHiddenField("hdfComplianceCategoryId");
                HiddenField hdfComplianceItemId = GetHiddenField("hdfComplianceItemId");
                //HiddenField hdfInvoiceNumber = GetHiddenField("hdfInvoiceNumber");
                //HiddenField hdfOrgUserProfileID = GetHiddenField("hdfOrgUserProfileID");
                Session.Remove(ResourceConst.APPLICANT_PARKING_CART);
                INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract itemPaymentContract = new INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract();
                itemPaymentContract.PkgName = hdnPackageName.Value;
                itemPaymentContract.CategoryName = hdnCategoryName.Value;
                itemPaymentContract.ItemID = !CurrentViewContext.ClientComplianceItem.IsNullOrEmpty() ? Convert.ToInt32(CurrentViewContext.ClientComplianceItem.ComplianceItemID) : AppConsts.NONE;
                itemPaymentContract.CategoryID = !String.IsNullOrEmpty(hdfComplianceCategoryId.Value) ? Convert.ToInt32(hdfComplianceCategoryId.Value) : AppConsts.NONE;
                itemPaymentContract.ItemName = !String.IsNullOrEmpty(CurrentViewContext.ClientComplianceItem.ItemLabel) ? CurrentViewContext.ClientComplianceItem.ItemLabel : CurrentViewContext.ClientComplianceItem.Name;
                itemPaymentContract.PkgId = !String.IsNullOrEmpty(hdfPackageID.Value) ? Convert.ToInt32(hdfPackageID.Value) : AppConsts.NONE;
                itemPaymentContract.PkgSubscriptionId = Convert.ToInt32(hdfPackageSubscriptionID.Value);
                itemPaymentContract.TenantID = CurrentViewContext.TenantId;
                //if (!hdfOrgUserProfileID.Value.IsNullOrEmpty())
                //{
                //    itemPaymentContract.OrganizationUserProfileID = Convert.ToInt32(hdfOrgUserProfileID.Value);
                //}
                //if (!hdfInvoiceNumber.Value.IsNullOrEmpty())
                //{
                //    itemPaymentContract.invoiceNumber = hdfInvoiceNumber.Value;
                //}

                if (!CurrentViewContext.ClientComplianceItem.IsNullOrEmpty())
                {
                    if (CurrentViewContext.ClientComplianceItem.IsPaymentType.Value)
                    {
                        itemPaymentContract.TotalPrice = CurrentViewContext.ClientComplianceItem.Amount.Value;
                        itemPaymentContract.IsRequirementPackage = false;
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
            }
        }

        #endregion
    }
}

