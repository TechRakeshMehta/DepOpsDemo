using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Collections.Generic;
using Entity.ClientEntity;
using System.Linq;
using Telerik.Web.UI;
using CoreWeb.Shell;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils;
using System.Configuration;
using INTSOF.IMAGE.MANAGER;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class CategoryList : BaseWebPage, ICategoryListView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private CategoryListPresenter _presenter = new CategoryListPresenter();
        private String _viewType;
        private ComplianceCategoryContract _viewContract;
        private Int32 _tenantid;
        //string logInfo = String.Empty;

        #endregion
        #endregion

        #region properties


        public CategoryListPresenter Presenter
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

        Int32 ICategoryListView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

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

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
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

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        public Int32 CountOfDisplayOrder
        {
            get
            {
                return Convert.ToInt32(ViewState["CountOfDisplayOrder"]);
            }
            set
            {
                ViewState["CountOfDisplayOrder"] = value;
            }
        }

        public ICategoryListView CurrentViewContext
        {
            get { return this; }
        }

        ComplianceCategoryContract ICategoryListView.ViewContract
        {
            get
            {

                if (_viewContract == null)
                    _viewContract = new ComplianceCategoryContract();
                return _viewContract;
            }


        }

        List<ComplianceCategory> ICategoryListView.complianceCategories
        {
            get;
            set;

        }

        Int32 ICategoryListView.selectedCategoryId
        {
            get;
            set;
        }

        Int32 ICategoryListView.PackageId
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
        /// <summary>
        /// Gets and sets DisplayOrder 
        /// </summary>
        public Int32 DisplayOrder
        {
            get;
            set;
        }

        #region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //List<Entity.SharedDataEntity.UniversalCategory> ICategoryListView.LstUniversalCategory { get; set; }

        //Int32 ICategoryListView.SelectedUniversalCategoryID
        //{
        //    get
        //    {
        //        if (!cmbUniversalCategory.SelectedValue.IsNullOrEmpty())
        //            return Convert.ToInt32(cmbUniversalCategory.SelectedValue);
        //        return AppConsts.NONE;
        //    }
        //}
        #endregion
        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                grdCategory.WclGridDataObject = new GridObjectDataContainer();
                ((GridObjectDataContainer)(grdCategory.WclGridDataObject)).ColumnsToSkipEncoding.Add("Description");
                base.OnInit(e);
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
        protected void Page_Load(object sender, EventArgs e)
        {
            //logInfo += "**Category List Page** **Entered into Page_Load event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;


          

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                CurrentViewContext.PackageId = Convert.ToInt32(Request.QueryString["Id"]);

                if (Request.QueryString["SelectedTenantId"] != null)
                {
                    SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                }
                //BindCategories();
                //BindCategory();
            }
            Presenter.OnViewLoaded();
            CurrentViewContext.ViewContract.AssignToPackageId = CurrentViewContext.PackageId;
            //logInfo += "**Exit from Page_Load event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;
            //Call a method that set the ImageManagerDirectory.
            SetImageManagerDirectory();
            if (SelectedTenantId == DefaultTenantId)
                ((GridButtonColumn)grdCategory.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Are you sure you want to delete this record?";
            else
                ((GridButtonColumn)grdCategory.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Deleting this record will impact existing orders. Are you sure you want to delete?";

            //UAT-2725
            if (SelectedTenantId == DefaultTenantId)
                dvTriggerComplianceCheck.Visible = false;
            else
                dvTriggerComplianceCheck.Visible = true;
        }

        //protected void Page_Unload(object sender, EventArgs e)
        //{
        //    // enter loginfo

        //    SystemException ex = new SystemException();
        //    base.LogError(logInfo, ex);
        //}

        //public void BindCategories()
        //{
        //    Presenter.GetMasterComplianceCategories();
        //    cmbMaster.DataSource = CurrentViewContext.complianceCategories;
        //    cmbMaster.DataBind();
        //}

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //logInfo += "**Entered into btnAdd_Click event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;

            BindCategory();
            divCreate.Visible = divAddForm.Visible = true;
           Presenter.GetComplianceCategoriesByPackage();
           txtDisplayOrder.Text =Convert.ToString( CountOfDisplayOrder == 0 ? 1 : CountOfDisplayOrder+1);

            //UAT-2305: 
            //BindUniversalCategories();

            //logInfo += "**Exit from btnAdd_Click event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;
            //ResetControls();
        }

        protected void grdCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //logInfo += "**Entered into grdCategory_NeedDataSource event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;
            List<CompliancePackageCategory> lstCategories = Presenter.GetComplianceCategoriesByPackage();
            if (lstCategories.Count > Convert.ToInt32( AppConsts.ZERO))
                CountOfDisplayOrder = lstCategories.OrderByDescending(x => x.CPC_DisplayOrder).Select(x => x.CPC_DisplayOrder).First();
         
            if (lstCategories.Count > 0)
            {
                grdCategory.Visible = true;
                lblTitle.Visible = true;
                grdCategory.DataSource = lstCategories.OrderBy(ordr => ordr.CPC_DisplayOrder);
                grdCategory.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(SelectedTenantId);
            }
            else
            {
                grdCategory.Visible = false;
                lblTitle.Visible = false;
            }
            //logInfo += "**Exit from grdCategory_NeedDataSource event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;
        }

        protected void grdCategory_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            //logInfo += "**Entered into grdCategory_DeleteCommand event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;
            HiddenField hdnfComplianceCategoryID = (e.Item as GridDataItem).FindControl("hdnfComplianceCategoryID") as HiddenField;
            CurrentViewContext.ViewContract.ComplianceCategoryId = Convert.ToInt32(hdnfComplianceCategoryID.Value);
            if (Presenter.ifPackageCanBeDelted())
            {
                if (Presenter.DeletePackageCategoryMapping())
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    base.ShowSuccessMessage("Compliance package category mapping deleted successfully.");
                }
                else
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
            }
            else
                base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);

            //logInfo += "**Exit from grdCategory_DeleteCommand event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //logInfo += "**Entered into btnSave_Click event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;

            if (cmbMaster.SelectedValue == String.Empty)
            {
                CurrentViewContext.ViewContract.CategoryName = txtCategoryName.Text.Trim();
                CurrentViewContext.ViewContract.Description = rdEditorDescription.Content; //txtCatDescription.Text.Trim();
                CurrentViewContext.ViewContract.CategoryLabel = txtCategoryLabel.Text.Trim();
                CurrentViewContext.ViewContract.ScreenLabel = txtScreenLabel.Text.Trim();
                CurrentViewContext.ViewContract.Active = chkActive.Checked;
                CurrentViewContext.ViewContract.TenantID = SelectedTenantId;
                CurrentViewContext.ViewContract.DisplayOrder = Convert.ToInt32( txtDisplayOrder.Text.Trim());
                CurrentViewContext.ViewContract.ExplanatoryNotes = rdEditorEcplanatoryNotes.Content; //txtCatNotes.Text.Trim();
                CurrentViewContext.ViewContract.SendItemDoconApproval = chkSendItemDocApp.Checked; //UAT-3805
                if (txtDisplayOrder.Text.Trim() != String.Empty)
                {
                    CurrentViewContext.ViewContract.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                }
                CurrentViewContext.ViewContract.TriggerOtherCategoryRules = Convert.ToBoolean(rblTriggerComplianceCheck.SelectedValue);
            }
            else
            {
                CurrentViewContext.selectedCategoryId = Convert.ToInt32(cmbMaster.SelectedValue);
                CurrentViewContext.ViewContract.DisplayOrder = CountOfDisplayOrder + 1;

            }
            //Set/get ComplianceRequired field
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
                    CurrentViewContext.ViewContract.CmplncRqdStartDate = dpStartFrm.SelectedDate.HasValue ? dpStartFrm.SelectedDate : null;
                    CurrentViewContext.ViewContract.CmplncRqdEndDate = dpEndTo.SelectedDate.HasValue ? dpEndTo.SelectedDate : null;
                }
            }
            Presenter.SaveNewCategory();

            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("Compliance Category saved successfully.");

                //13-02-2014  Changes Done for -"Category listing screen save performance improvement. (remove rebinding)"
                //BindCategory();

                ResetControls();
                grdCategory.Rebind();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                divAddForm.Visible = false;
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
                divAddForm.Visible = true;
            }
            //logInfo += "**Exit from btnSave_Click event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;
        }

        protected void btnCancel_click(object sender, EventArgs e)
        {
            //logInfo += "**Entered into btnCancel_click event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;

            divAddForm.Visible = false;
            ResetControls();
            //logInfo += "**Exit from btnCancel_click event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;
        }

        protected void cmbMaster_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //logInfo += "**Entered into cmbMaster_SelectedIndexChanged event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;

            divCreate.Visible = String.IsNullOrEmpty(cmbMaster.SelectedValue);
            ResetControls();
            //logInfo += "**Exit from cmbMaster_SelectedIndexChanged event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;
        }

        protected void cmbMaster_DataBound(object sender, EventArgs e)
        {
            cmbMaster.Items.Insert(0, new RadComboBoxItem("Create New", String.Empty));
        }

        #endregion

        #region Methods

        private void ResetControls()
        {
            //logInfo += "**Entered into ResetControls event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;

            txtCategoryName.Text = String.Empty;
            //txtCatDescription.Text = String.Empty;
            rdEditorDescription.Content = String.Empty;
            txtCategoryLabel.Text = String.Empty;
            txtScreenLabel.Text = String.Empty;
            chkActive.Checked = true;
            // txtCatNotes.Text = String.Empty;
            rdEditorEcplanatoryNotes.Content = String.Empty;
            txtDisplayOrder.Text =Convert.ToString( CountOfDisplayOrder + 1);
            if (SelectedTenantId != DefaultTenantId)
            {
                divComplianceRequired.Visible = true;
                rblComplianceRequired.SelectedValue = Convert.ToString(true);
                dpEndTo.SelectedDate = null;
                dpStartFrm.SelectedDate = null;
                dvTriggerComplianceCheck.Visible = true;
            }
            chkSendItemDocApp.Checked = false; //UAT-3805
            //logInfo += "**Exit from ResetControls event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;
        }

        private void BindCategory()
        {
            //logInfo += "**Entered into BindCategory event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;

            ResetControls();
            cmbMaster.DataSource = Presenter.GetNotMappedComplianceCategories();
            cmbMaster.DataBind();
            //logInfo += "**Exit from BindCategory event**" + DateTime.Now.ToString("hh.mm.ss.ff") + Environment.NewLine;
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
        private void BindUniversalCategories()
        {
            //Presenter.GetUniversalCategories();
            //cmbUniversalCategory.DataSource = CurrentViewContext.LstUniversalCategory;
            //cmbUniversalCategory.DataBind();
        }
        #endregion
    }
}

