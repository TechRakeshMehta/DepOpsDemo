using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class AdminEntryNodeSpecificTemplates : BaseUserControl, IAdminEntryNodeSpecificTemplatesView
    {
        #region Variables

        AdminEntryNodeSpecificTemplatesPresenter _presenter = new AdminEntryNodeSpecificTemplatesPresenter();

        #endregion

        #region Properties

        #region Public Properties
        public AdminEntryNodeSpecificTemplatesPresenter Presenter
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

        public IAdminEntryNodeSpecificTemplatesView CurrentViewsContext
        {
            get { return this; }
        }

        #endregion

        #region Private Properties
        Int32 IAdminEntryNodeSpecificTemplatesView.TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNullOrEmpty())
                    return (Int32)(ViewState["TenantId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IAdminEntryNodeSpecificTemplatesView.DeptProgramMappingID
        {
            get
            {
                if (!ViewState["DeptProgramMappingID"].IsNullOrEmpty())
                    return (Int32)(ViewState["DeptProgramMappingID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["DeptProgramMappingID"] = value;
            }
        }

        Int32 IAdminEntryNodeSpecificTemplatesView.CurrentLoggedInUserID
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        List<Entity.CommunicationTemplatePlaceHolder> IAdminEntryNodeSpecificTemplatesView.lstTemplatePlaceHolders
        {
            get
            {
                if (!ViewState["lstTemplatePlaceHolders"].IsNullOrEmpty())
                    return ViewState["lstTemplatePlaceHolders"] as List<Entity.CommunicationTemplatePlaceHolder>;
                return new List<Entity.CommunicationTemplatePlaceHolder>();
            }
            set
            {
                ViewState["lstTemplatePlaceHolders"] = value;
            }
        }

        AdminEntryNodeTemplate IAdminEntryNodeSpecificTemplatesView.AdminEntryNodeTemplate
        {
            get
            {
                if (!ViewState["AdminEntryNodeTemplate"].IsNullOrEmpty())
                    return ViewState["AdminEntryNodeTemplate"] as AdminEntryNodeTemplate;
                return new AdminEntryNodeTemplate();
            }
            set
            {
                ViewState["AdminEntryNodeTemplate"] = value;
            }
        }

        #endregion



        #endregion


        #region Events

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    CaptureQueryString();
                    AddPlaceHolders();
                    GetTemplate();
                    BindControls();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Button Events
        protected void fsucCmdBar_SaveClick(object sender, EventArgs e)
        {
            try
            {

                CurrentViewsContext.AdminEntryNodeTemplate.TemplateName = !String.IsNullOrEmpty(txtTemplateName.Text) ? txtTemplateName.Text : String.Empty;
                CurrentViewsContext.AdminEntryNodeTemplate.Subject = !String.IsNullOrEmpty(txtTemplateSubject.Text) ? txtTemplateSubject.Text : String.Empty;
                CurrentViewsContext.AdminEntryNodeTemplate.Content = !String.IsNullOrEmpty(editorContent.Content) && !String.IsNullOrEmpty(editorContent.Text) ? editorContent.Content : String.Empty;

                if (Presenter.SaveUpdateAdminEntryNodeTemplate())
                {
                    GetTemplate();
                    BindControls();
                    (this.Page as BaseWebPage).ShowSuccessMessage("Template saved successfully");
                }
                else
                {
                    (this.Page as BaseWebPage).ShowErrorMessage("Some error occurred. Please try again.");
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void fsucCmdBar_CancelClick(object sender, EventArgs e)
        {
            try
            {
                //GetTemplate();
                BindControls();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        private void CaptureQueryString()
        {
            if (!Request.IsNullOrEmpty() && !Request.QueryString.IsNullOrEmpty())
            {
                if (!Request.QueryString["SelectedTenantId"].IsNullOrEmpty())
                    CurrentViewsContext.TenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                if (!Request.QueryString["Id"].IsNullOrEmpty())
                    CurrentViewsContext.DeptProgramMappingID = Convert.ToInt32(Request.QueryString["Id"]);
            }
        }
        private void BindControls()
        {
            if (!CurrentViewsContext.AdminEntryNodeTemplate.IsNullOrEmpty())
            {
                txtTemplateName.Text = !String.IsNullOrEmpty(CurrentViewsContext.AdminEntryNodeTemplate.TemplateName) ? CurrentViewsContext.AdminEntryNodeTemplate.TemplateName : String.Empty;
                txtTemplateSubject.Text = !String.IsNullOrEmpty(CurrentViewsContext.AdminEntryNodeTemplate.Subject) ? CurrentViewsContext.AdminEntryNodeTemplate.Subject : String.Empty;
                editorContent.Content = !String.IsNullOrEmpty(CurrentViewsContext.AdminEntryNodeTemplate.Content) ? CurrentViewsContext.AdminEntryNodeTemplate.Content : String.Empty;
            }
        }

        private void GetTemplate()
        {
            //Get template on the basis of dPM_ID , if there is no template mapped with the ID then get default template.
            Presenter.GetTemplate();
        }

        private void AddPlaceHolders()
        {
            Presenter.BindTemplatePlaceHolders();
            EditorTool editorDropDownTool = editorContent.FindTool("ddPlaceHolders");
            EditorDropDown ddPlaceholders;
            if (editorDropDownTool.IsNull())
            {
                EditorToolGroup dynamicToolbar = new EditorToolGroup();
                editorContent.Tools.Add(dynamicToolbar);

                ddPlaceholders = new EditorDropDown("ddPlaceHolders");
                ddPlaceholders.Text = AppConsts.COMBOBOX_ITEM_SELECT;

                ddPlaceholders.Attributes["width"] = "110px";
                ddPlaceholders.Attributes["popupwidth"] = "150px";
                ddPlaceholders.Attributes["popupheight"] = "100px";

                dynamicToolbar.Tools.Add(ddPlaceholders);
                //ddn.Attributes.Add("OnClientCommandExecuting", "OnClientCommandExecuting");
            }
            else
            {
                ddPlaceholders = editorDropDownTool as EditorDropDown;
            }

            ddPlaceholders.Items.Clear();
            foreach (var placeHolder in CurrentViewsContext.lstTemplatePlaceHolders)
            {
                ddPlaceholders.Items.Add(placeHolder.Name, placeHolder.PlaceHolder);
            }
        }

        #endregion

        #endregion


    }
}