using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using CoreWeb.Shell;
using Telerik.Web.UI;
using INTSOF.Utils;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using CoreWeb.IntsofSecurityModel;
using System.Linq;
using System.Configuration;
using INTSOF.IMAGE.MANAGER;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ItemInfo : BaseWebPage, IItemInfoView
    {
        #region Variables

        private Int32 _tenantid;
        private ItemInfoPresenter _presenter = new ItemInfoPresenter();
        private ComplianceItemsContract _viewContract;

        #endregion

        #region Properties

        public ItemInfoPresenter Presenter
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

        public IItemInfoView CurrentViewContext
        {
            get { return this; }
        }

        public ComplianceItemsContract ViewContract
        {
            get
            {
                if (_viewContract == null)
                {
                    _viewContract = new ComplianceItemsContract();
                }
                return _viewContract;
            }
            set
            {
                _viewContract = value;
            }
        }

        public ComplianceItem complianceItem
        {
            get;
            set;

        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //tenantId = Presenter.GetTenantId();
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

        public Int32 DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        public Int32 CurrentItemId
        {
            get
            {
                return Convert.ToInt32(ViewState["currentItemId"]);
            }
            set
            {
                ViewState["currentItemId"] = value;
            }

        }

        public String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        public Int32 CurrentCategoryId
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentCategoryId"]);
            }
            set
            {
                ViewState["CurrentCategoryId"] = value;
            }
        }

        public Int32 CurrentPackageId
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentPackageId"]);
            }
            set
            {
                ViewState["CurrentPackageId"] = value;
            }
        }

        public String DissociateButtonStatus
        {
            get
            {
                return Convert.ToString(ViewState["DissociateButtonStatus"]);
            }
            set
            {
                ViewState["DissociateButtonStatus"] = value;
            }
        }

        //#region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //List<Entity.SharedDataEntity.UniversalItem> IItemInfoView.LstUniversalItem { get; set; }

        //Int32 IItemInfoView.SelectedUniversalCatItemID
        //{
        //    get
        //    {
        //        if (!cmbUniversalItem.SelectedValue.IsNullOrEmpty())
        //            return Convert.ToInt32(cmbUniversalItem.SelectedValue);
        //        return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        if (cmbUniversalItem.Items.Count > AppConsts.NONE)
        //        {
        //            cmbUniversalItem.SelectedValue = Convert.ToString(value);
        //        }
        //    }
        //}

        //Int32 IItemInfoView.UniversalItemMappingID
        //{
        //    get
        //    {
        //        if (!ViewState["UniversalItemMappingID"].IsNullOrEmpty())
        //            return Convert.ToInt32(ViewState["UniversalItemMappingID"]);
        //        else
        //            return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["UniversalItemMappingID"] = value;
        //    }
        //}

        //Int32 IItemInfoView.MappedUniversalCatItemID
        //{
        //    get
        //    {
        //        if (!ViewState["MappedUniversalCatItemID"].IsNullOrEmpty())
        //            return Convert.ToInt32(ViewState["MappedUniversalCatItemID"]);
        //        else
        //            return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["MappedUniversalCatItemID"] = value;
        //    }
        //}

        //Int32 IItemInfoView.SelectedUniversalCategoryID
        //{
        //    get
        //    {
        //        if (!ViewState["SelectedUniversalCategoryID"].IsNullOrEmpty())
        //            return Convert.ToInt32(ViewState["SelectedUniversalCategoryID"]);
        //        else
        //            return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["SelectedUniversalCategoryID"] = value;
        //    }
        //}
        //Int32 IItemInfoView.MappedUniversalCategoryID
        //{
        //    get
        //    {
        //        if (!ViewState["MappedUniversalCategoryID"].IsNullOrEmpty())
        //            return Convert.ToInt32(ViewState["MappedUniversalCategoryID"]);
        //        else
        //            return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["MappedUniversalCategoryID"] = value;
        //    }
        //}
        //Int32 IItemInfoView.CategoryItemMappingID
        //{
        //    get
        //    {
        //        if (!ViewState["CategoryItemMappingID"].IsNullOrEmpty())
        //            return Convert.ToInt32(ViewState["CategoryItemMappingID"]);
        //        else
        //            return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["CategoryItemMappingID"] = value;
        //    }
        //}
        //#endregion

        //UAT-2582
        List<ComplianceCategory> IItemInfoView.lstComplianceCategory
        {
            get;
            set;
        }

        //UAT-2582
        String IItemInfoView.SelectedCategoryIDs
        {
            get;
            set;
        }


        Int32 IItemInfoView.ComplianceCategoryItemID
        {
            get
            {
                if (!ViewState["ComplianceCategoryItemID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(ViewState["ComplianceCategoryItemID"]);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ComplianceCategoryItemID"] = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                CurrentViewContext.CurrentItemId = Convert.ToInt32(Request.QueryString["Id"]);
                CurrentViewContext.CurrentCategoryId = Convert.ToInt32(Request.QueryString["CategoryID"]);
                CurrentViewContext.CurrentPackageId = Convert.ToInt32(Request.QueryString["PackageId"]);
                SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);

                Presenter.GetCategoryItemMappingID();

                if (SelectedTenantId.IsNotNull())
                {
                    if (SelectedTenantId == DefaultTenantId)
                    {
                        divEffectiveDate.Visible = false;
                    }
                }
                BindItemInfo();
                DissociateButtonStatus = Presenter.GetItemDissociationStatus();
                ResetButtons(true);
                SetFormMode(false);
                //UAt-2305:
                //Presenter.MappedUniversalCategoryData();
                //Presenter.MappedUniversalItemData();
                //BindUniversalItems();

            }
            Presenter.OnViewLoaded();
            SetImageManagerDirectory();
            if (SelectedTenantId == DefaultTenantId)
                ((GridButtonColumn)grdAttributes.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Are you sure you want to delete this record?";
            else
                ((GridButtonColumn)grdAttributes.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Deleting this record will impact existing orders. Are you sure you want to delete?";
        }

        protected void fsucCmdBarItem_CancelClick(object sender, EventArgs e)
        {
            BindItemInfo();
            ResetButtons(true);
            SetFormMode(false);
            //UAT-2305:
            //BindUniversalItemData();
        }

        protected void fsucCmdBarItem_SubmitClick(object sender, EventArgs e)
        {
            ResetButtons(false);
            SetFormMode(true);
        }

        protected void fsucCmdBarItem_SaveClick(object sender, EventArgs e)
        {
            CurrentViewContext.ViewContract.Name = txtName.Text;
            CurrentViewContext.ViewContract.Description = rdEditorDescription.Content; //txtDescription.Text;
            CurrentViewContext.ViewContract.Details = rdEditorDetails.Content;
            CurrentViewContext.ViewContract.ItemLabel = txtLabel.Text;
            CurrentViewContext.ViewContract.ScreenLabel = txtScreenLabel.Text;
            CurrentViewContext.ViewContract.IsActive = chkActive.Checked;
            //CurrentViewContext.ViewContract.Description = txtDescription.Text;
            CurrentViewContext.ViewContract.EffectiveDate = dpkrEffectiveDate.SelectedDate;
            CurrentViewContext.ViewContract.ComplianceItemId = CurrentViewContext.CurrentItemId;
            CurrentViewContext.ViewContract.ExplanatoryNotes = rdEditorEcplanatoryNotes.Content; //txtNotes.Text;
            if (txtDisplayOrder.Text.Trim() != String.Empty)
            {
                CurrentViewContext.ViewContract.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
            }
            //UAT-3077
            CurrentViewContext.ViewContract.IsPaymentType = chkPaymentType.Checked;
            if (chkPaymentType.Checked)
            {
                CurrentViewContext.ViewContract.Amount = Convert.ToDecimal(txtAmount.Text);
            }
            Presenter.UpdateItem();

            Presenter.UpdateComplianceCategoryItemDisplayOrder();

            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("Compliance Item updated successfully.");
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                ResetButtons(true);
                SetFormMode(false);
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
                SetFormMode(true);
            }
        }

        protected void btnDissociateItem_Click(object sender, EventArgs e)
        {
            //UAT-2582 Getting selected cateogry ids and adding the current cateogryID by default
            if (cmbAssociatedCategories.CheckedItems.Count > AppConsts.NONE)
            {
                foreach (RadComboBoxItem item in cmbAssociatedCategories.CheckedItems)
                {
                    CurrentViewContext.SelectedCategoryIDs += item.Value + ',';
                }
            }
            CurrentViewContext.SelectedCategoryIDs += CurrentViewContext.CurrentCategoryId.ToString();

            Int32 dissociatedItemID = Presenter.DissociateItem();
            if (dissociatedItemID > 0)
            {
                CurrentViewContext.CurrentItemId = dissociatedItemID;
                DissociateButtonStatus = Presenter.GetItemDissociationStatus();
                String data = String.Empty;
                data = String.Format("{{\"DataId\":\"{0}\",\"ParentDataId\":\"{1}\",\"UICode\":\"{2}\",\"PackageId\":\"{3}\"}}", dissociatedItemID, CurrentCategoryId, RuleSetTreeNodeType.Item, CurrentPackageId);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTreeOnDissociate('" + data + "');", true);
                BindItemInfo();
                ResetButtons(true);
                SetFormMode(false);
                grdAttributes.Rebind();
                //UAT-2305:
                //BindUniversalItemData();
                base.ShowSuccessMessage("Item dissociated successfully.");
            }
            else
            {
                ResetButtons(true);
                SetFormMode(false);
                base.ShowInfoMessage("Item not dissociated successfully.");
            }

        }

        protected void grdAttributes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<ComplianceItemAttribute> lstItemAttributes = Presenter.GetComplianceItemAttributes(CurrentViewContext.CurrentItemId);
            if (lstItemAttributes.Count > 0)
            {
                grdAttributes.Visible = true;
                lblTitle.Visible = true;
                grdAttributes.DataSource = lstItemAttributes.OrderBy(col => col.CIA_DisplayOrder);
                grdAttributes.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(SelectedTenantId);
            }
            else
            {
                grdAttributes.Visible = false;
                lblTitle.Visible = false;
            }
        }

        protected void grdAttributes_DeleteCommand(object sender, GridCommandEventArgs e)
        {

            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            Int32 cia_ID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("CIA_ID"));
            HiddenField hdnfComplianceAttributeID = (e.Item as GridDataItem).FindControl("hdnfComplianceAttributeID") as HiddenField;
            if (Presenter.IfAttributeCanBeRemoved(Convert.ToInt32(hdnfComplianceAttributeID.Value), CurrentViewContext.CurrentItemId))
            {
                if (Presenter.DeleteComplianceItemAttribute(cia_ID, Convert.ToInt32(hdnfComplianceAttributeID.Value), CurrentViewContext.CurrentItemId, CurrentLoggedInUserId))
                {
                    grdAttributes.Rebind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    base.ShowSuccessMessage("Compliance item attribute mapping deleted successfully.");
                }
            }
            else
                base.ShowErrorInfoMessage(ErrorMessage);

            //if (Presenter.DeleteComplianceAttribute(complianceItemAttributeID, CurrentUserId))
            //  base.ShowSuccessMessage("Compliance item attribute deleted successfully.");
        }

        #region UAT-2402:Additional Tracking to Rotation Mapping development and testing
        protected void btnCopyItemData_ExtraClick(object sender, EventArgs e)
        {

            if (Presenter.IsUniversalAttributeMappingExist())
            {
                if (Presenter.AddApprovedItemsToCopyDataQueue())
                {
                    base.ShowSuccessMessage("Data synchronization has been initiated. Compliance item data will be copied to rotation item in few minutes.");
                }
            }
            else
            {
                base.ShowInfoMessage("Please complete universal attribute mapping and then proceed to copy item data to rotation. ");
            }

            ResetButtons(true);
            SetFormMode(false);

            //if (CurrentViewContext.UniversalItemMappingID > AppConsts.NONE)
            //{
            //    if (CurrentViewContext.SelectedUniversalCatItemID != CurrentViewContext.MappedUniversalCatItemID)
            //    {
            //        base.ShowInfoMessage("Please save universal mapping first and then proceed to copy item data to rotation.");
            //    }
            //    else
            //    {
            //        if (Presenter.IsUniversalAttributeMappingExist())
            //        {
            //            if (Presenter.AddApprovedItemsToCopyDataQueue())
            //            {
            //                base.ShowSuccessMessage("Data synchronization has been initiated. Compliance item data will be copied to rotation item in few minutes.");
            //            }
            //        }
            //        else
            //        {
            //            base.ShowInfoMessage("Please complete universal attribute mapping and then proceed to copy item data to rotation. ");
            //        }
            //    }
            //}
            //else
            //{
            //    base.ShowInfoMessage("Please complete universal Attribute/Item mapping and then proceed to copy item data to rotation. ");
            //}
            //ResetButtons(true);
            //SetFormMode(false);
        }

        protected void cmbUniversalItem_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            HideShowCopyItemDataButton(false);
        }
        #endregion

        #endregion

        #region Methods

        private void BindItemInfo()
        {
            Presenter.GetCurrentItemInfo();
            Presenter.GetLargeContent();
            txtName.Text = CurrentViewContext.ViewContract.Name;
            txtLabel.Text = CurrentViewContext.ViewContract.ItemLabel;
            txtScreenLabel.Text = CurrentViewContext.ViewContract.ScreenLabel;
            chkActive.Checked = CurrentViewContext.ViewContract.IsActive;
            dpkrEffectiveDate.SelectedDate = CurrentViewContext.ViewContract.EffectiveDate;
            //txtDescription.Text = CurrentViewContext.ViewContract.Description;
            rdEditorDescription.Content = CurrentViewContext.ViewContract.Description;
            rdEditorDetails.Content = CurrentViewContext.ViewContract.Details;
            //txtNotes.Text = CurrentViewContext.ViewContract.ExplanatoryNotes;
            rdEditorEcplanatoryNotes.Content = CurrentViewContext.ViewContract.ExplanatoryNotes;
            txtDisplayOrder.Text = Convert.ToString(CurrentViewContext.ViewContract.DisplayOrder);
            //UAT-3077
            chkPaymentType.Checked = CurrentViewContext.ViewContract.IsPaymentType;
            txtAmount.Text = CurrentViewContext.ViewContract.Amount.HasValue ? Convert.ToString(CurrentViewContext.ViewContract.Amount.Value) : String.Empty;
            if (Convert.ToInt32(SelectedTenantId) > DefaultTenantId)
            {
                if (!ucCategoriesItemsNodes.IsNullOrEmpty())
                {
                    dvMappingHierarchy.Visible = true;
                    ucCategoriesItemsNodes.SelectedTenantId = CurrentViewContext.SelectedTenantId;
                    ucCategoriesItemsNodes.ComplianceItemId = CurrentViewContext.CurrentItemId;              
                    ucCategoriesItemsNodes.BindListofNodes();
                }
            }
        }

        private void SetFormMode(Boolean isEnabled)
        {
            txtName.Enabled = isEnabled;
            txtLabel.Enabled = isEnabled;
            txtScreenLabel.Enabled = isEnabled;
            txtScreenLabel.Enabled = isEnabled;
            chkActive.IsActiveEnable = isEnabled;
            dpkrEffectiveDate.Enabled = isEnabled;
            //txtDescription.Enabled = isEnabled;
            if (isEnabled)
            {
                rdEditorDescription.EditModes = EditModes.All;
                rdEditorDetails.EditModes = EditModes.All;
                rdEditorEcplanatoryNotes.EditModes = EditModes.All;
            }
            else
            {
                rdEditorDescription.EditModes = EditModes.Preview;
                rdEditorDetails.EditModes = EditModes.Preview;
                rdEditorEcplanatoryNotes.EditModes = EditModes.Preview;
            }
            txtDisplayOrder.Enabled = isEnabled;
            //txtNotes.Enabled = isEnabled;
            //UAT-2305:
            //cmbUniversalItem.Enabled = isEnabled;
            //UAT-3077:
            chkPaymentType.Enabled = isEnabled;
            txtAmount.Enabled = isEnabled;
        }

        private void ResetButtons(Boolean isReset)
        {
            fsucCmdBarItem.SaveButton.Visible = !isReset;
            fsucCmdBarItem.CancelButton.Visible = !isReset;
            fsucCmdBarItem.SubmitButton.Visible = isReset;
            fsucCmdBarItem.SubmitButton.Text = "Edit";
            if (isReset && SelectedTenantId != DefaultTenantId)
            {
                if (DissociateButtonStatus.Equals(AppConsts.DISSOCIATION_BUTTON_VISIBLE))
                {
                    btnDissociateItem.Visible = true;
                    dvDisassociate.Visible = true; //UAT-2582
                    BindComplianceCategory(); //UAT-2582
                }
                else if (DissociateButtonStatus.Equals(AppConsts.DISSOCIATION_BUTTON_ALL))
                {
                    btnDissociateItem.Visible = true;
                    btnDissociateItem.Text = "Dissociate All";
                    dvDisassociate.Visible = true; //UAT-2582
                    BindComplianceCategory();
                }
                else
                {
                    btnDissociateItem.Visible = false;
                    dvDisassociate.Visible = false; //UAT-2582
                }
            }
            else
            {
                btnDissociateItem.Visible = false;
            }
            //UAT-2402
            HideShowCopyItemDataButton(isReset);
        }

        /// <summary>
        /// Method that set the Image Managere Directory in Content Editor.
        /// </summary>
        private void SetImageManagerDirectory()
        {
            String s3ImageManagerDirectory = ConfigurationManager.AppSettings["S3ImageManagerDirectory"];
            if (ConfigurationManager.AppSettings["FileManagerMode"] == "S3")
            {
                String[] viewImages = new String[] { s3ImageManagerDirectory };
                String[] uploadImages = new String[] { s3ImageManagerDirectory };
                String[] deleteImages = new String[] { s3ImageManagerDirectory };
                SetImageManagerSettingInEditor(rdEditorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdEditorDetails, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdEditorEcplanatoryNotes, viewImages, uploadImages, deleteImages);
                rdEditorDescription.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;
                rdEditorDetails.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;
                rdEditorEcplanatoryNotes.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;

            }
            else if (ConfigurationManager.AppSettings["FileManagerMode"] == "DB")
            {
                String[] viewImages = new String[] { "InstitutionImages/" };
                String[] uploadImages = new String[] { "InstitutionImages/" };
                String[] deleteImages = new String[] { "InstitutionImages/" };
                SetImageManagerSettingInEditor(rdEditorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdEditorDetails, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdEditorEcplanatoryNotes, viewImages, uploadImages, deleteImages);
                rdEditorDescription.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
                rdEditorDetails.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
                rdEditorEcplanatoryNotes.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
            }
            else
            {
                String[] viewImages = new String[] { "~/InstitutionImages" };
                String[] uploadImages = new String[] { "~/InstitutionImages" };
                String[] deleteImages = new String[] { "~/InstitutionImages" };
                SetImageManagerSettingInEditor(rdEditorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdEditorDetails, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdEditorEcplanatoryNotes, viewImages, uploadImages, deleteImages);
            }
        }
        private void SetImageManagerSettingInEditor(RadEditor editor, String[] viewImages, String[] uploadImages, String[] deleteImages)
        {
            editor.ImageManager.ViewPaths = viewImages;
            editor.ImageManager.UploadPaths = uploadImages;
            editor.ImageManager.DeletePaths = deleteImages;
            editor.ImageManager.MaxUploadFileSize = 71000000;

        }

        #region UAT-2305:

        //private void BindUniversalItemData()
        //{
        //    //UAt-2305:
        //    Presenter.MappedUniversalCategoryData();
        //    Presenter.MappedUniversalItemData();
        //    BindUniversalItems();
        //}
        //private void BindUniversalItems()
        //{
        //    Presenter.GetUniversalCategoryItems();
        //    cmbUniversalItem.DataSource = CurrentViewContext.LstUniversalItem;
        //    cmbUniversalItem.DataBind();

        //    if (CurrentViewContext.MappedUniversalCatItemID > AppConsts.NONE)
        //    {
        //        cmbUniversalItem.SelectedValue = Convert.ToString(CurrentViewContext.MappedUniversalCatItemID);
        //    }

        //}

        #region UAT-2402: Additional Tracking to Rotation Mapping development and testing
        private void HideShowCopyItemDataButton(Boolean isReset)
        {

            if (!isReset)
                btnCopyItemData.Visible = true;
            else
                btnCopyItemData.Visible = false;

            //if (!isReset && CurrentViewContext.SelectedUniversalCatItemID > AppConsts.NONE)
            //{
            //    btnCopyItemData.Visible = true;
            //}
            //else
            //{
            //    btnCopyItemData.Visible = false;
            //}
        }
        #endregion
        #endregion

        /// <summary>
        /// UAT-2582 :- Bind the Category drop down for disassociation selection
        /// </summary>
        private void BindComplianceCategory()
        {
            Presenter.GetComplianceCategoriesAssociatedToItem();
            cmbAssociatedCategories.DataSource = CurrentViewContext.lstComplianceCategory;
            cmbAssociatedCategories.DataBind();
        }
        #endregion

    }
}

