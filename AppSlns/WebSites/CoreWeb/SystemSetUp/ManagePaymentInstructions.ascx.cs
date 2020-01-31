using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.SystemSetUp.Views;
using Entity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.IMAGE.MANAGER;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.SystemSetUp.Views
{
    public partial class ManagePaymentInstructions : BaseUserControl, IManagePaymentInstructionView
    {

        #region Private Variables

        private ManagePaymentInstructionPresenter _presenter = new ManagePaymentInstructionPresenter();

        #endregion

        #region Properties


        public ManagePaymentInstructionPresenter Presenter
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
        /// Gets the default TenantId
        /// </summary>
        Int32 IManagePaymentInstructionView.DefaultTenantId
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
        /// Returns the current logged-in user ID.
        /// </summary>
        Int32 IManagePaymentInstructionView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        String IManagePaymentInstructionView.ErrorMessage
        {
            get;
            set;
        }

        String IManagePaymentInstructionView.SuccessMessage
        {
            get;
            set;
        } 

        public IManagePaymentInstructionView CurrentViewContext
        {
            get { return this; }
        }


        public Int32 SelectedPaymentOption
        {
            get
            {
                if (!cmbPaymentOption.IsNullOrEmpty())
                {
                    return Convert.ToInt32(cmbPaymentOption.SelectedValue);
                }
                return 0;
            }
            set
            {
                cmbPaymentOption.SelectedValue = value.ToString();
            }
        }

        public List<lkpPaymentOption> lstPaymentOption
        {
            get;
            set;
        }


        public String InstructionText
        {
            get
            {
                return radHTMLEditor.Content;
            }
            set
            {
                radHTMLEditor.Content = value;
            }
        }

        #endregion

        #region  Events

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Payment Option Instructions";
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
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                ApplyActionLevelPermission(ActionCollection, "Payment Option Instructions");                
                BindcmbPaymentOption();
            }
            base.SetPageTitle("Payment Option Instructions");
            String s3ImageManagerDirectory = ConfigurationManager.AppSettings["S3ImageManagerDirectory"];
            if (ConfigurationManager.AppSettings["FileManagerMode"] == "S3")
            {
                String[] viewImages = new String[] { s3ImageManagerDirectory };
                String[] uploadImages = new String[] { s3ImageManagerDirectory };
                String[] deleteImages = new String[] { s3ImageManagerDirectory };
                radHTMLEditor.ImageManager.ViewPaths = viewImages;
                radHTMLEditor.ImageManager.UploadPaths = uploadImages;
                radHTMLEditor.ImageManager.DeletePaths = deleteImages;
                radHTMLEditor.ImageManager.MaxUploadFileSize = 71000000;
                radHTMLEditor.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;

            }
            else if (ConfigurationManager.AppSettings["FileManagerMode"] == "DB")
            {
                String[] viewImages = new String[] { "InstitutionImages/" };
                String[] uploadImages = new String[] { "InstitutionImages/" };
                String[] deleteImages = new String[] { "InstitutionImages/" };
                radHTMLEditor.ImageManager.ViewPaths = viewImages;
                radHTMLEditor.ImageManager.UploadPaths = uploadImages;
                radHTMLEditor.ImageManager.DeletePaths = deleteImages;
                radHTMLEditor.ImageManager.MaxUploadFileSize = 71000000;
                radHTMLEditor.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
            }
            else
            {
                String[] viewImages = new String[] { "~/InstitutionImages" };
                String[] uploadImages = new String[] { "~/InstitutionImages" };
                String[] deleteImages = new String[] { "~/InstitutionImages" };
                radHTMLEditor.ImageManager.ViewPaths = viewImages;
                radHTMLEditor.ImageManager.UploadPaths = uploadImages;
                radHTMLEditor.ImageManager.DeletePaths = deleteImages;
                radHTMLEditor.ImageManager.MaxUploadFileSize = 71000000;
            }
        }       

        #endregion

        #region Dropdown Events

        protected void cmbPaymentOption_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            SetControls(true);
        }       

        #endregion

        #region Button Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.UpdatePaymentOptionInstructionText();
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    base.ShowSuccessMessage(CurrentViewContext.ErrorMessage);
                }               
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    SetControls(true);
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SetControls(true);
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
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                SetControls(false);
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
        #endregion

        #endregion         

        #region Methods

        private void BindcmbPaymentOption()
        {
            Presenter.GetlkpPaymentOptions();
            BindCombo(cmbPaymentOption, lstPaymentOption);            
        }

        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            cmbBox.Items.Clear();
            cmbBox.ClearSelection();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
            cmbBox.Items.Insert(AppConsts.NONE, new RadComboBoxItem { Selected = true, Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.MINUS_ONE.ToString() });
        }

        private void SetControls(Boolean IsEnable)
        {
            if (IsEnable)
            {
                radHTMLEditor.EditModes = EditModes.Preview;
            }
            else
            {
                radHTMLEditor.EditModes = EditModes.All;
            }
            btnCancel.Visible = !IsEnable;
            btnSave.Visible = !IsEnable;
            btnEdit.Visible = IsEnable;
            Presenter.GetCurrentPaymentOptionInstruction();
        }

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Update Instruction Text",
                    CustomActionLabel = "Save",//Chnages Control Action Label From "Update Instruction Text" to "Save"
                    ScreenName = "Payment Option Instructions"
                });                
                return actionCollection;
            }
        }

        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Update Instruction Text")
                                {
                                    btnSave.Enabled = false;
                                }                                
                                break;
                            }
                    }

                });
            }
        }

        #endregion

       
    }
}