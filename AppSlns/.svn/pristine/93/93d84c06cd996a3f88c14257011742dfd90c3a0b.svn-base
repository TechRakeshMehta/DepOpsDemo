using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using Telerik.Web.UI;
using System.Configuration;
using INTSOF.IMAGE.MANAGER;
using System.Globalization;
using System.Collections.Generic;
using CoreWeb.Shell.Views;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class CategoryInfo : BaseWebPage, ICategoryInfoView
    {
        #region Variables

        private CategoryInfoPresenter _presenter = new CategoryInfoPresenter();
        private ComplianceCategoryContract _viewContract;

        #endregion

        #region Properties


        public CategoryInfoPresenter Presenter
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

        private Int32 _tenantid;

        /// <summary>
        /// Get or Set the Tenant ID
        /// </summary>
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

        public ICategoryInfoView CurrentViewContext
        {
            get { return this; }
        }

        public ComplianceCategoryContract ViewContract
        {
            get
            {
                if (_viewContract == null)
                {
                    _viewContract = new ComplianceCategoryContract();
                }
                return _viewContract;
            }
        }

        public ComplianceCategory complianceCategories
        {
            get;
            set;

        }

        public int CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public int currentCategoryId
        {
            get
            {
                return Convert.ToInt32(ViewState["currentCategoryId"]);
            }
            set
            {
                ViewState["currentCategoryId"] = value;
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

        // <summary>
        /// Gets and sets Package id
        /// </summary>
        public Int32 PackageId
        {
            get
            {
                return Convert.ToInt32(ViewState["PackageId"]);
            }
            set
            {
                ViewState["PackageId"] = value;
            }
        }

        /// Datetime for the start of the compliance category in client screen.
        /// </summary>
        DateTime? ICategoryInfoView.PreviousCmplncRqdStartDate
        {
            get
            {
                if (!ViewState["PreviousCmplncRqdStartDate"].IsNullOrEmpty())
                    return Convert.ToDateTime(ViewState["PreviousCmplncRqdStartDate"]);
                else
                    return null;
            }
            set
            {
                ViewState["PreviousCmplncRqdStartDate"] = value;
            }
        }

        /// <summary>
        /// Datetime for the end of the compliance category in client screen.
        /// </summary>
        DateTime? ICategoryInfoView.PreviousCmplncRqdEndDate
        {
            get
            {
                if (!ViewState["PreviousCmplncRqdEndDate"].IsNullOrEmpty())
                    return Convert.ToDateTime(ViewState["PreviousCmplncRqdEndDate"]);
                else
                    return null;
            }
            set
            {
                ViewState["PreviousCmplncRqdEndDate"] = value;
            }
        }

        Boolean ICategoryInfoView.PreviousComplianceRequired
        {
            get
            {
                if (!ViewState["PreviousComplianceRequired"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["PreviousComplianceRequired"]);
                else
                    return true;
            }
            set
            {
                ViewState["PreviousComplianceRequired"] = value;
            }
        }

        #region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //List<Entity.SharedDataEntity.UniversalCategory> ICategoryInfoView.LstUniversalCategory { get; set; }

        //Int32 ICategoryInfoView.SelectedUniversalCategoryID
        //{
        //    get
        //    {
        //        if (!cmbUniversalCategory.SelectedValue.IsNullOrEmpty())
        //            return Convert.ToInt32(cmbUniversalCategory.SelectedValue);
        //        return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        if (cmbUniversalCategory.Items.Count > AppConsts.NONE)
        //        {
        //            cmbUniversalCategory.SelectedValue = Convert.ToString(value);
        //        }
        //    }
        //}

        //Int32 ICategoryInfoView.UniversalCategoryMappingID
        //{
        //    get
        //    {
        //        if (!ViewState["UniversalCategoryMappingID"].IsNullOrEmpty())
        //            return Convert.ToInt32(ViewState["UniversalCategoryMappingID"]);
        //        else
        //            return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["UniversalCategoryMappingID"] = value;
        //    }
        //}

        //Int32 ICategoryInfoView.MappedUniversalCategoryID
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
        #endregion

        List<CompliancePackage> ICategoryInfoView.lstCompliancePackage
        {
            get;
            set;
        }

        String ICategoryInfoView.SelectedPackageIDs
        {
            get;
            set;
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            fsucCmdBarCat.DisplayButtons = CommandBarButtons.Save;
            fsucCmdBarCat.DisplayButtons = CommandBarButtons.Cancel;
            fsucCmdBarCat.DisplayButtons = CommandBarButtons.Submit;

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                currentCategoryId = Convert.ToInt32(Request.QueryString["Id"]);
                SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                PackageId = Convert.ToInt32(Request.QueryString["PackageId"]);
                BindCategoryInfo();
                DissociateButtonStatus = Presenter.GetCategoryDissociationStatus();
                ResetButtons(true);
                SetFormMode(false);
                //UAT-2305:
                //BindUniversalCategories();
                //Presenter.MappedUniversalCategoryData();
            }
            Presenter.OnViewLoaded();
            ItemsListing.ParentCategoryId = currentCategoryId;
            ItemsListing.SelectedTenantId = SelectedTenantId;
            SetImageManagerDirectory();
            //CultureInfo cultureInfo = new CultureInfo("en-US");
            //DateTimeFormatInfo dateInfo = new DateTimeFormatInfo();
            //dateInfo.ShortDatePattern = "MM/yyyy";
            //cultureInfo.DateTimeFormat = dateInfo;
            //dpStartFrm.Culture = cultureInfo;
            //dpEndTo.Culture = cultureInfo;
        }

        protected void fsucCmdBarCat_CancelClick(object sender, EventArgs e)
        {
            BindCategoryInfo();
            ResetButtons(true);
            SetFormMode(false);
            //UAT-2305:
            //BindUniversalCategoryData();
        }

        protected void fsucCmdBarCat_SubmitClick(object sender, EventArgs e)
        {
            ResetButtons(false);
            SetFormMode(true);
            fsucCmdBarCat.SaveButton.ValidationGroup = "grpFormSubmit";
        }

        protected void fsucCmdBarCat_SaveClick(object sender, EventArgs e)
        {
            CurrentViewContext.ViewContract.CategoryName = txtCategoryName.Text;
            CurrentViewContext.ViewContract.Description = rdEditorDescription.Content;// txtCatDescription.Text;
            CurrentViewContext.ViewContract.CategoryLabel = txtCategoryLabel.Text;
            CurrentViewContext.ViewContract.ScreenLabel = txtScreenLabel.Text;
            CurrentViewContext.ViewContract.Active = chkActive.Checked;
            CurrentViewContext.ViewContract.ComplianceCategoryId = CurrentViewContext.currentCategoryId;
            // CurrentViewContext.ViewContract.ExplanatoryNotes = txtCatNotes.Text;
            CurrentViewContext.ViewContract.ExplanatoryNotes = rdEditorEcplanatoryNotes.Content;
            CurrentViewContext.ViewContract.SendItemDoconApproval = chkSendItemDocApp.Checked; //UAT-3805
            if (txtDisplayOrder.Text.Trim() != String.Empty)
            {
                CurrentViewContext.ViewContract.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
            }
            if (CurrentViewContext.SelectedTenantId == CurrentViewContext.DefaultTenantId)
            {
                //Always save Compliance Required true for master db
                CurrentViewContext.ViewContract.ComplianceRequired = true;
            }
            else
            {
                //save Compliance Required field value
                if (rblComplianceRequired.SelectedIndex != -1)
                {
                    CurrentViewContext.ViewContract.ComplianceRequired = Convert.ToBoolean(rblComplianceRequired.SelectedValue);
                }
                CurrentViewContext.ViewContract.CmplncRqdStartDate = dpStartFrm.SelectedDate.HasValue ? dpStartFrm.SelectedDate : null;
                CurrentViewContext.ViewContract.CmplncRqdEndDate = dpEndTo.SelectedDate.HasValue ? dpEndTo.SelectedDate : null;
                
                //UAT-2725
                CurrentViewContext.ViewContract.TriggerOtherCategoryRules = Convert.ToBoolean(rblTriggerComplianceCheck.SelectedValue);
            }
            Presenter.saveCurrentCategoryInfo();
            Presenter.UpdateCompliancePackageCategoryDisplayOrder();

            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("Compliance Category updated successfully.");
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
            ResetButtons(true);
            SetFormMode(false);
        }

        protected void btnDissociateCategory_Click(object sender, EventArgs e)
        {
            //UAT-2582 Getting selected package ids and adding the current PackageID by default
            if (cmbAssociatedPackages.CheckedItems.Count > AppConsts.NONE)
            {
                foreach (RadComboBoxItem item in cmbAssociatedPackages.CheckedItems)
                {
                    CurrentViewContext.SelectedPackageIDs += item.Value + ',';
                }
            }
            CurrentViewContext.SelectedPackageIDs += CurrentViewContext.PackageId.ToString();

            Int32 dissociatedCategoryID = Presenter.DissociateCategory();
            if (dissociatedCategoryID > 0)
            {
                currentCategoryId = dissociatedCategoryID;
                ItemsListing.ParentCategoryId = dissociatedCategoryID;
                DissociateButtonStatus = Presenter.GetCategoryDissociationStatus();
                String data = String.Empty;
                data = String.Format("{{\"DataId\":\"{0}\",\"ParentDataId\":\"{1}\",\"UICode\":\"{2}\"}}", dissociatedCategoryID, PackageId, RuleSetTreeNodeType.Category);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTreeOnDissociate('" + data + "');", true);
                BindCategoryInfo();
                ResetButtons(true);
                SetFormMode(false);
                ItemsListing.RebindGrid();
                //UAT-2305:
                //BindUniversalCategoryData();
                base.ShowSuccessMessage("Category dissociated successfully.");
            }
            else
            {
                ResetButtons(true);
                SetFormMode(false);
                base.ShowInfoMessage("Category not dissociated successfully.");
            }
        }

        #endregion

        #region Methods

        public void BindCategoryInfo()
        {
            Presenter.getCurrentCategoryInfo();
            txtCategoryName.Text = CurrentViewContext.complianceCategories.CategoryName;
            //txtCatDescription.Text = CurrentViewContext.complianceCategories.Description;
            rdEditorDescription.Content = CurrentViewContext.complianceCategories.Description;
            txtCategoryLabel.Text = CurrentViewContext.complianceCategories.CategoryLabel;
            txtScreenLabel.Text = CurrentViewContext.complianceCategories.ScreenLabel;
            chkActive.Checked = CurrentViewContext.complianceCategories.IsActive;
            CurrentViewContext.ViewContract.ComplianceCategoryId = CurrentViewContext.complianceCategories.ComplianceCategoryID;
            Presenter.GetLargeContent();
            //txtCatNotes.Text = CurrentViewContext.ViewContract.ExplanatoryNotes;
            rdEditorEcplanatoryNotes.Content = CurrentViewContext.ViewContract.ExplanatoryNotes;
            Presenter.GetCompliancePackageCategory();
            txtDisplayOrder.Text = Convert.ToString(CurrentViewContext.ViewContract.DisplayOrder);
            chkSendItemDocApp.Checked = Convert.ToBoolean(CurrentViewContext.complianceCategories.SendItemDocOnApproval); //UAT-3805
            if (CurrentViewContext.SelectedTenantId != CurrentViewContext.DefaultTenantId)
            {
                divComplianceRequired.Visible = true; 
                rblComplianceRequired.SelectedValue = Convert.ToString(CurrentViewContext.ViewContract.ComplianceRequired);
                CurrentViewContext.PreviousComplianceRequired = CurrentViewContext.ViewContract.ComplianceRequired;
                //if (CurrentViewContext.ViewContract.CmplncRqdStartDate.HasValue)
                //{
                dpStartFrm.SelectedDate = CurrentViewContext.ViewContract.CmplncRqdStartDate;
                CurrentViewContext.PreviousCmplncRqdStartDate = CurrentViewContext.ViewContract.CmplncRqdStartDate;
                //}
                //if (CurrentViewContext.ViewContract.CmplncRqdEndDate.HasValue)
                //{
                dpEndTo.SelectedDate = CurrentViewContext.ViewContract.CmplncRqdEndDate;
                CurrentViewContext.PreviousCmplncRqdEndDate = CurrentViewContext.ViewContract.CmplncRqdEndDate;
                //}

                dvTriggerComplianceCheck.Visible = true;
                rblTriggerComplianceCheck.SelectedValue = Convert.ToString(CurrentViewContext.complianceCategories.TriggerOtherCategoryRules);
              
                if (Convert.ToInt32(SelectedTenantId) > DefaultTenantId)
                {
                    if (!ucCategoriesItemsNodes.IsNullOrEmpty())
                    {
                        dvMappingHierarchy.Visible = true;
                        ucCategoriesItemsNodes.SelectedTenantId = CurrentViewContext.SelectedTenantId;
                        ucCategoriesItemsNodes.ComplianceCategoryId = CurrentViewContext.ViewContract.ComplianceCategoryId;
                        ucCategoriesItemsNodes.ComplianceItemId = null;
                        ucCategoriesItemsNodes.BindListofNodes();
                    }
                }
            }
        }

        private void ResetButtons(Boolean isReset)
        {
            fsucCmdBarCat.SaveButton.Visible = !isReset;
            fsucCmdBarCat.CancelButton.Visible = !isReset;
            fsucCmdBarCat.SubmitButton.Visible = isReset;
            fsucCmdBarCat.SubmitButton.Text = "Edit";
            if (SelectedTenantId != DefaultTenantId && isReset && DissociateButtonStatus.Equals(AppConsts.DISSOCIATION_BUTTON_VISIBLE))
            {
                btnDissociateCategory.Visible = isReset;
                dvDisassociate.Visible = isReset; //UAT-2582
                BindCompliancePackages();
            }
            else
            {
                btnDissociateCategory.Visible = false;
                dvDisassociate.Visible = false; //UAT-2582
            }

        }

        private void SetFormMode(Boolean isEnabled)
        {
            txtCategoryName.Enabled = isEnabled;
            //txtCatDescription.Enabled = isEnabled;
            if (isEnabled)
            {
                rdEditorDescription.EditModes = EditModes.All;
                rdEditorEcplanatoryNotes.EditModes = EditModes.All;
            }
            else
            {
                rdEditorDescription.EditModes = EditModes.Preview;
                rdEditorEcplanatoryNotes.EditModes = EditModes.Preview;
            }
            txtCategoryLabel.Enabled = isEnabled;
            txtScreenLabel.Enabled = isEnabled;
            chkActive.IsActiveEnable = isEnabled;
            chkSendItemDocApp.IsActiveEnable = isEnabled;
            //txtCatNotes.Enabled = isEnabled;

            txtDisplayOrder.Enabled = isEnabled;
            rblComplianceRequired.Enabled = isEnabled;
            dpStartFrm.Enabled = isEnabled;
            dpEndTo.Enabled = isEnabled;
            //UAT-2305:
            //cmbUniversalCategory.Enabled = isEnabled;
            //UAT-2725
            rblTriggerComplianceCheck.Enabled = isEnabled;
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
                SetImageManagerSettingInEditor(rdEditorEcplanatoryNotes, viewImages, uploadImages, deleteImages);
                rdEditorDescription.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;
                rdEditorEcplanatoryNotes.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;

            }
            else if (ConfigurationManager.AppSettings["FileManagerMode"] == "DB")
            {
                String[] viewImages = new String[] { "InstitutionImages/" };
                String[] uploadImages = new String[] { "InstitutionImages/" };
                String[] deleteImages = new String[] { "InstitutionImages/" };
                SetImageManagerSettingInEditor(rdEditorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdEditorEcplanatoryNotes, viewImages, uploadImages, deleteImages);
                rdEditorDescription.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
                rdEditorEcplanatoryNotes.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
            }
            else
            {
                String[] viewImages = new String[] { "~/InstitutionImages" };
                String[] uploadImages = new String[] { "~/InstitutionImages" };
                String[] deleteImages = new String[] { "~/InstitutionImages" };
                SetImageManagerSettingInEditor(rdEditorDescription, viewImages, uploadImages, deleteImages);
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
        #endregion

        #region UAT-2305:
        //private void BindUniversalCategoryData()
        //{
        //    BindUniversalCategories();
        //    Presenter.MappedUniversalCategoryData();
        //}

        //private void BindUniversalCategories()
        //{
        //    Presenter.GetUniversalCategories();
        //    cmbUniversalCategory.DataSource = CurrentViewContext.LstUniversalCategory;
        //    cmbUniversalCategory.DataBind();

        //}

        /// <summary>
        /// UAT-2582 :- Bind the packages drop down for disassociation selection
        /// </summary>
        private void BindCompliancePackages()
        {
            Presenter.GetCompliancePackagesAssociatedtoCat();
            cmbAssociatedPackages.DataSource = CurrentViewContext.lstCompliancePackage;
            cmbAssociatedPackages.DataBind();
        }
        #endregion

    }
}

