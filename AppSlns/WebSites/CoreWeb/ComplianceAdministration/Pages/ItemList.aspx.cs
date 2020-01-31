using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using Telerik.Web.UI;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using CoreWeb.Shell;
using System.Configuration;
using INTSOF.IMAGE.MANAGER;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ItemList : BaseWebPage, IItemListView
    {
        #region Variables

        private ItemListPresenter _presenter = new ItemListPresenter();
        private ComplianceItemsContract _viewContract;

        #endregion

        #region Properties


        public ItemListPresenter Presenter
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

        public List<ComplianceItem> lstMasterItems
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        public IItemListView CurrentViewContext
        {
            get { return this; }
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
       


        public ComplianceItemsContract ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ComplianceItemsContract();
                }
                return _viewContract;
            }
        }


        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        public Int32 CurrentItemId
        {
            get;
            set;
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

        #region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //List<Entity.SharedDataEntity.UniversalItem> IItemListView.LstUniversalItem { get; set; }

        //Int32 IItemListView.SelectedUniversalCatItemID
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

        //Int32 IItemListView.SelectedUniversalCategoryID
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
        //Int32 IItemListView.MappedUniversalCategoryID
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
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

                CurrentViewContext.CurrentCategoryId = Convert.ToInt32(Request.QueryString["Id"]);

                if (Request.QueryString["SelectedTenantId"] != null)
                {
                    SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    if (SelectedTenantId == DefaultTenantId)
                    {
                        divEffectiveDate.Visible = false;
                    }
                    else
                    {
                        divEffectiveDate.Visible = true;
                    }
                }
                //CurrentViewContext.CurrentCategoryId = 5;
                GetMasterComplianceItems();
            }
            Presenter.OnViewLoaded();
            ucItemsListing.ParentCategoryId = CurrentCategoryId;
            ucItemsListing.SelectedTenantId = SelectedTenantId;
            SetImageManagerDirectory();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
            txtDisplayOrder.Text = Convert.ToString(ucItemsListing.CountOfDisplayOrder == 0 ? 1 : ucItemsListing.CountOfDisplayOrder + 1);
            divCreate.Visible = divAddForm.Visible = true;
            GetMasterComplianceItems();
            //UAT-2305:
            //Presenter.MappedUniversalCategoryData();
            //Presenter.MappedUniversalCategoryData();
            //BindUniversalItems();
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "HideShowPanel(null,'true');", true);
        }

        protected void cmbMaster_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cmbMaster.SelectedValue == AppConsts.ZERO)
            {
                ResetForm();
                divCreate.Visible = true;
            }
            else
                divCreate.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ComplianceCategoryItemContract complianceCategoryItemContract = new ComplianceCategoryItemContract();
            complianceCategoryItemContract.CCI_CategoryId = CurrentViewContext.CurrentCategoryId;
            complianceCategoryItemContract.CCI_ItemId = Convert.ToInt32(cmbMaster.SelectedValue);
            if (txtDisplayOrder.Text.Trim() != String.Empty)
            {
                complianceCategoryItemContract.CCI_DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
            }
            else
            {
                complianceCategoryItemContract.CCI_DisplayOrder = 0;
            }
            if (cmbMaster.SelectedValue == AppConsts.ZERO)
            {
                CurrentViewContext.ViewContract.ItemLabel = txtLabel.Text;
                CurrentViewContext.ViewContract.Name = txtName.Text;
                CurrentViewContext.ViewContract.ScreenLabel = txtScreenLabel.Text;
                CurrentViewContext.ViewContract.Description = rdEditorDescription.Content; //txtDescription.Text;
                CurrentViewContext.ViewContract.Details = rdEditorDetails.Content;
                CurrentViewContext.ViewContract.EffectiveDate = dpkrEffectiveDate.SelectedDate;
                CurrentViewContext.ViewContract.IsActive = chkActive.Checked;
                CurrentViewContext.ViewContract.CreatedById = CurrentViewContext.CurrentLoggedInUserId;
                CurrentViewContext.ViewContract.CategoryItem = complianceCategoryItemContract;
                CurrentViewContext.ViewContract.ExplanatoryNotes = rdEditorEcplanatoryNotes.Content; //txtNotes.Text;
                //UAT-3077
                CurrentViewContext.ViewContract.IsPaymentType = chkPaymentType.Checked;
                if(chkPaymentType.Checked)
                {
                    CurrentViewContext.ViewContract.Amount = Convert.ToDecimal(txtAmount.Text);
                }

                Presenter.SaveComplianceItem();
            }
            else
            {
                CurrentViewContext.ViewContract.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                CurrentViewContext.CurrentItemId = Convert.ToInt32(cmbMaster.SelectedValue);
                Presenter.SaveComplianceCategoryItemMapping();
            }

            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("Compliance Item saved successfully.");
                divAddForm.Visible = false;
                ucItemsListing.RebindGrid();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                ResetForm();
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
                divAddForm.Visible = true;
            }
        }

        protected void fsucCmdBarItem_CancelClick(object sender, EventArgs e)
        {
            divAddForm.Visible = false;
        }

        #endregion

        #region Methods

        private void GetMasterComplianceItems()
        {
            Presenter.GetMasterItems();
            cmbMaster.DataSource = CurrentViewContext.lstMasterItems;
            cmbMaster.DataTextField = "Name";
            cmbMaster.DataValueField = "ComplianceItemID";
            CurrentViewContext.lstMasterItems.Insert(0, new ComplianceItem { ComplianceItemID = 0, Name = "Create New Item" });

            cmbMaster.DataBind();
        }

        private void ResetForm()
        {
            txtName.Text = String.Empty;
            //txtNotes.Text = String.Empty;
            rdEditorEcplanatoryNotes.Content = String.Empty;
            txtLabel.Text = String.Empty;
            txtScreenLabel.Text = string.Empty;
            chkActive.Checked = true;
            dpkrEffectiveDate.SelectedDate = null;
            //txtDescription.Text = String.Empty
            rdEditorDescription.Content = String.Empty;
            rdEditorDetails.Content = String.Empty; ;
            //UAT-3077
            chkPaymentType.Checked = false;
            txtAmount.Text = String.Empty;
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
        //private void BindUniversalItems()
        //{
        //    Presenter.GetUniversalCategoryItems();
        //    cmbUniversalItem.DataSource = CurrentViewContext.LstUniversalItem;
        //    cmbUniversalItem.DataBind();

        //}
        #endregion
        #endregion
    }
}

