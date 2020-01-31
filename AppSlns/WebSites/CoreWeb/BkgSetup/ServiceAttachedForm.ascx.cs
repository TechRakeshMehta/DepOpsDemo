using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using CoreWeb.BkgSetup.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ServiceAttachedForm : BaseUserControl, IServiceAttachedFormView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ServiceAttachedFormPresenter _presenter = new ServiceAttachedFormPresenter();
        private String _viewType;
        private Int32 _tenantid;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        private ServiceAttachedFormPresenter Presenter
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
        /// Gets and sets Logged In User TenantId
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    _tenantid = Presenter.GetTenantId();
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
        /// Gets and Sets list of tenants.
        /// </summary>
        public List<Entity.ClientEntity.Tenant> ListTenants
        {
            set;
            get;
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

        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.HasValue ? _isAdminLoggedIn.Value : true;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        /// <summary>
        /// Returns the current logged-in user ID.
        /// </summary>
        Int32 IServiceAttachedFormView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        //Int32 IManageServiceGroupView.TenantId
        //{
        //    get;
        //    set;

        //}

        public IServiceAttachedFormView CurrentViewContext
        {
            get { return this; }
        }

        List<ServiceAttachedFormContract> IServiceAttachedFormView.ListServiceAttachedForm
        {
            set;
            get;
        }

        List<Entity.ServiceAttachedForm> IServiceAttachedFormView.LstParentServiceAttachedForm
        {
            set;
            get;
        }

        Entity.SystemDocument IServiceAttachedFormView.SystemDocumentToSaveUpdate
        {
            get;
            set;
        }

        Int32 IServiceAttachedFormView.ParentFormId
        {
            get;
            set;
        }

        String IServiceAttachedFormView.FormName
        {
            get;
            set;
        }

        Int32 IServiceAttachedFormView.SF_ID
        {
            get;
            set;
        }

        String IServiceAttachedFormView.ErrorMessage
        {
            get;
            set;
        }

        String IServiceAttachedFormView.ServiceFormDispatchType
        {
            get;
            set;
        }

        Int16 IServiceAttachedFormView.ServiceFormType
        {
            get;
            set;
        }

        String IServiceAttachedFormView.TemplateName
        {
            get;
            set;
        }

        String IServiceAttachedFormView.TemplateSubject
        {
            get;
            set;
        }

        String IServiceAttachedFormView.TemplateContent
        {
            get;
            set;
        }

        String IServiceAttachedFormView.ReminderTemplateName
        {
            get;
            set;
        }

        String IServiceAttachedFormView.ReminderTemplateSubject
        {
            get;
            set;
        }

        String IServiceAttachedFormView.ReminderTemplateContent
        {
            get;
            set;
        }

        List<Entity.CommunicationTemplatePlaceHolder> IServiceAttachedFormView.TemplatePlaceHolders
        {
            get;
            set;
        }

        String IServiceAttachedFormView.SuccessMessage
        {
            get;
            set;
        }

        List<Entity.CommunicationTemplateEntity> IServiceAttachedFormView.ServiceFormCommunicationTemplateData
        {
            get;
            set;
        }

        Boolean IServiceAttachedFormView.IsUpdate
        {
            get;
            set;
        }

        Int32 IServiceAttachedFormView.SvcFormCommunicationTemplateID
        {
            get;
            set;
        }

        Int32 IServiceAttachedFormView.ReminderCommunicationTemplateID
        {
            get;
            set;
        }

        Boolean IServiceAttachedFormView.IsCorruptedDocument
        {
            get;
            set;
        }

        Boolean IServiceAttachedFormView.IsBkgServiceAttachedFormMappingExists
        {
            get;
            set;
        }

        //UAT-2480
        Boolean IServiceAttachedFormView.IsActiveVersionsPresent
        {
            get;
            set;
        }

        public List<Entity.lkpLanguage> Languages
        {
            get
            {
                if (ViewState["Languages"] == null)
                {
                    ViewState["Languages"] = new List<Entity.lkpLanguage>();
                }
                return (List<Entity.lkpLanguage>)ViewState["Languages"];
            }
            set
            {
                ViewState["Languages"] = value;
            }
        }

        public Int32 DefaultLanguageId
        {
            get
            {
                if (Languages.Any())
                {
                    return Languages.First(cl => cl.LAN_Code == INTSOF.Utils.CommunicationLanguages.DEFAULT.GetStringValue()).LAN_ID;
                }
                return 0;
            }
        }

        public Int32 SelectedLanguageId
        {
            get;
            set;
        }

       

        #endregion

        #region Public Properties

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Manage Service Attached Form";
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
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Presenter.GetLanguages();
                }
                base.SetPageTitle("Manage Service Attached Form");
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

        #region Grid Related Events

        protected void grdServiceAttachedForm_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetServiceAttachedForm();
                grdServiceAttachedForm.DataSource = CurrentViewContext.ListServiceAttachedForm;
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

        protected void grdServiceAttachedForm_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.IsUpdate = false;
                CurrentViewContext.FormName = (e.Item.FindControl("txtFormName") as WclTextBox).Text.Trim();
                if (Presenter.CheckIfServiceAttachedFormNameAlreadyExist())
                {
                    e.Canceled = true;
                    base.ShowInfoMessage("Service Form Name cannot be duplicate.");
                    return;
                }

                /*Service Form Type
                 * Form Version-0
                 * New Form-1
                */
                var rbtnFormVer = (e.Item.FindControl("rbtnFormVer") as RadioButton).Checked;
                var rbtnNewForm = (e.Item.FindControl("rbtnNewForm") as RadioButton).Checked;

                CurrentViewContext.ServiceFormType = (rbtnFormVer) ? Convert.ToInt16(AppConsts.NONE) : Convert.ToInt16(AppConsts.ONE);

                //If Service Form Type is 'Form Version', then Parent Form and Service Form Name is required and displayed.
                if (rbtnFormVer)
                {
                    if (Convert.ToInt32((e.Item.FindControl("ddlParentForm") as WclComboBox).SelectedValue) == AppConsts.NONE)
                    {
                        e.Canceled = true;
                        base.ShowErrorMessage("Please Select Parent Service Form");
                        return;
                    }

                    CurrentViewContext.ParentFormId = Convert.ToInt32((e.Item.FindControl("ddlParentForm") as WclComboBox).SelectedValue);
                    if (!ManageUploadServiceForm(e.Item, false))
                    {
                        e.Canceled = true;
                        return;
                    }
                }
                else
                {
                    RadioButtonList rblDispatchType = (e.Item.FindControl("rblDispatchType") as RadioButtonList);
                    DispatchType rblDispatchTypeVal = (DispatchType)Enum.Parse(typeof(DispatchType), rblDispatchType.SelectedValue);

                    CurrentViewContext.ServiceFormDispatchType = Convert.ToString(rblDispatchTypeVal);

                    //If Dispatch Type is Manual, then only Service Form Name
                    //If Dispatch Type is Automatic, then Uplaod Form, Templat information is displayed.
                    if (rblDispatchTypeVal == DispatchType.Automatic)
                    {
                        if (!ManageUploadServiceForm(e.Item, false))
                        {
                            e.Canceled = true;
                            return;
                        }
                        #region UAT-3965 Get the First Value of dropdown box. 
                        WclComboBox cmbTemplateLanguage = (e.Item.FindControl("cmbTemplateLanguage") as WclComboBox);
                        if (cmbTemplateLanguage.Items.Count > 0)
                        {
                            
                            CurrentViewContext.SelectedLanguageId =  Convert.ToInt32(cmbTemplateLanguage.Items[0].Value);
                        } 
                        #endregion
                        //CurrentViewContext.SelectedLanguageId = Convert.ToInt32((e.Item.FindControl("cmbTemplateLanguage") as WclComboBox).Items.Select(x => x.Value).FirstOrDefault());
                        CurrentViewContext.TemplateName = (e.Item.FindControl("txtTemplateName") as WclTextBox).Text;
                        CurrentViewContext.TemplateSubject = (e.Item.FindControl("txtTemplateSubject") as WclTextBox).Text;
                        CurrentViewContext.TemplateContent = (e.Item.FindControl("ecTemplateContent") as WclEditor).Content;

                        CurrentViewContext.ReminderTemplateName = (e.Item.FindControl("txtReminderTemplateName") as WclTextBox).Text;
                        CurrentViewContext.ReminderTemplateSubject = (e.Item.FindControl("txtReminderSubject") as WclTextBox).Text;
                        CurrentViewContext.ReminderTemplateContent = (e.Item.FindControl("ecReminderContent") as WclEditor).Content;
                    }
                }

                Presenter.SaveServiceAttachedForm();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }

                e.Canceled = false;
                base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        protected void grdServiceAttachedForm_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.IsNotNull())
                {
                    CurrentViewContext.SF_ID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SF_ID"]);

                    CurrentViewContext.FormName = (e.Item.FindControl("txtFormName") as WclTextBox).Text.Trim();
                    CurrentViewContext.IsUpdate = true;
                    if (Presenter.CheckIfServiceAttachedFormNameAlreadyExist())
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage("Form Name can not be duplicate.");
                        return;
                    }

                    /*Service Form Type
                        * Form Version-0
                        * New Form-1
                    */
                    var rbtnFormVer = (e.Item.FindControl("rbtnFormVer") as RadioButton).Checked;
                    var rbtnNewForm = (e.Item.FindControl("rbtnNewForm") as RadioButton).Checked;

                    CurrentViewContext.ServiceFormType = (rbtnFormVer) ? Convert.ToInt16(AppConsts.NONE) : Convert.ToInt16(AppConsts.ONE); ;

                    //If Service Form Type is 'Form Version', then Parent Form and Service Form Name is required and displayed.
                    if (rbtnFormVer)
                    {
                        if (Convert.ToInt32((e.Item.FindControl("ddlParentForm") as WclComboBox).SelectedValue) == AppConsts.NONE)
                        {
                            e.Canceled = true;
                            base.ShowErrorMessage("Please Select Parent Service Form");
                            return;
                        }
                        CurrentViewContext.ParentFormId = Convert.ToInt32((e.Item.FindControl("ddlParentForm") as WclComboBox).SelectedValue);
                        if (!ManageUploadServiceForm(e.Item, true))
                        {
                            e.Canceled = true;
                            return;
                        }
                    }
                    else
                    {
                        RadioButtonList rblDispatchType = (e.Item.FindControl("rblDispatchType") as RadioButtonList);
                        DispatchType rblDispatchTypeVal = (DispatchType)Enum.Parse(typeof(DispatchType), rblDispatchType.SelectedValue);

                        CurrentViewContext.ServiceFormDispatchType = Convert.ToString(rblDispatchTypeVal);

                        //If Dispatch Type is Manual, then only Service Form Name
                        //If Dispatch Type is Automatic, then Uplaod Form, Templat information is displayed.
                        if (rblDispatchTypeVal == DispatchType.Automatic)
                        {
                            if (!ManageUploadServiceForm(e.Item, true))
                            {
                                e.Canceled = true;
                                return;
                            }

                            #region UAT-3965 Get the First Value of dropdown box.
                            WclComboBox cmbTemplateLanguage = (e.Item.FindControl("cmbTemplateLanguage") as WclComboBox);
                            if (cmbTemplateLanguage.Items.Count > 0)
                            {

                                CurrentViewContext.SelectedLanguageId = Convert.ToInt32(cmbTemplateLanguage.Items[0].Value);
                            }
                            #endregion
                            CurrentViewContext.TemplateName = (e.Item.FindControl("txtTemplateName") as WclTextBox).Text;
                            CurrentViewContext.TemplateSubject = (e.Item.FindControl("txtTemplateSubject") as WclTextBox).Text;
                            CurrentViewContext.TemplateContent = (e.Item.FindControl("ecTemplateContent") as WclEditor).Content;

                            CurrentViewContext.ReminderTemplateName = (e.Item.FindControl("txtReminderTemplateName") as WclTextBox).Text;
                            CurrentViewContext.ReminderTemplateSubject = (e.Item.FindControl("txtReminderSubject") as WclTextBox).Text;
                            CurrentViewContext.ReminderTemplateContent = (e.Item.FindControl("ecReminderContent") as WclEditor).Content;

                            CurrentViewContext.SvcFormCommunicationTemplateID = Convert.ToInt32(((HiddenField)e.Item.FindControl("hdnSvcFormCommunicationTemplateID")).Value);
                            CurrentViewContext.ReminderCommunicationTemplateID = Convert.ToInt32(((HiddenField)e.Item.FindControl("hdnReminderCommunicationTemplateID")).Value);
                        }
                    }

                    Presenter.UpdateServiceAttachedForm();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                    }

                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
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

        protected void grdServiceAttachedForm_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Int32 serviceAttachedFormID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SF_ID"]);
                CurrentViewContext.SF_ID = serviceAttachedFormID;
                Presenter.GetBkgServiceAttachedFormMappingByServiceFormID();
                if (CurrentViewContext.IsBkgServiceAttachedFormMappingExists)
                {
                    base.ShowErrorInfoMessage("This Service Form cannot be 'Deleted' as Service Form is already mapped with Background Service. " +
                                            "Please remove Background Service Attached Form mapping and try again.");
                    return;
                }

                Presenter.DeleteServiceAttachedForm();
                if (CurrentViewContext.IsActiveVersionsPresent)
                {
                    base.ShowInfoMessage("You cannot delete this service form because it has active child version(s).");
                    return;
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                    return;
                }
                base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        protected void grdServiceAttachedForm_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                  || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdServiceAttachedForm);
                }
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

        protected void grdServiceAttachedForm_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                //insert operation
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);

                    RadioButton rbtnFormVer = editform.FindControl("rbtnFormVer") as RadioButton;
                    RadioButton rbtnNewForm = editform.FindControl("rbtnNewForm") as RadioButton;
                    RadioButtonList rblDispatchType = editform.FindControl("rblDispatchType") as RadioButtonList;
                    WclComboBox ddlParentForm = editform.FindControl("ddlParentForm") as WclComboBox;
                    WclEditor wclEditor = editform.FindControl("ecTemplateContent") as WclEditor;
                    WclEditor wclReminderContent = editform.FindControl("ecReminderContent") as WclEditor;
                    WclComboBox cmbTemplateLanguage = editform.FindControl("cmbTemplateLanguage") as WclComboBox;

                    cmbTemplateLanguage.DataSource = CurrentViewContext.Languages;
                    cmbTemplateLanguage.DataBind();
                    cmbTemplateLanguage.SelectedValue = CurrentViewContext.DefaultLanguageId.ToString();
                    cmbTemplateLanguage.Enabled = false;

                    Array lstValues = Enum.GetValues(typeof(DispatchType));
                    foreach (var value in lstValues)
                    {
                        ListItem item = new ListItem(Enum.GetName(typeof(DispatchType), value), Convert.ToInt32(value).ToString());
                        rblDispatchType.Items.Add(item);
                    }

                    if (ddlParentForm.IsNotNull())
                    {
                        //Bind Service type dropdown
                        Presenter.GetParentServiceAttachedForm();

                        if (CurrentViewContext.LstParentServiceAttachedForm.IsNotNull())
                        {
                            ddlParentForm.DataSource = CurrentViewContext.LstParentServiceAttachedForm;
                            ddlParentForm.DataBind();
                        }
                    }

                    Presenter.BindTemplatePlaceHolders();
                    AddPlaceHolders(wclEditor);
                    AddPlaceHolders(wclReminderContent);

                    if (!(e.Item.DataItem is GridInsertionObject) && e.Item.DataItem.IsNotNull())
                    {

                        //ServiceAttachedFormContract
                        ServiceAttachedFormContract svcFormContract = (ServiceAttachedFormContract)e.Item.DataItem;

                        CurrentViewContext.SF_ID = svcFormContract.SF_ID;

                        rbtnFormVer.Checked = svcFormContract.ParentFormName.IsNullOrEmpty() ? false : true;
                        rbtnNewForm.Checked = svcFormContract.ParentFormName.IsNullOrEmpty() ? true : false;
                        rblDispatchType.Items.FindByText(Convert.ToString(DispatchType.Automatic)).Selected = svcFormContract.ServiceFormDispatchMode ? true : false;
                        rblDispatchType.Items.FindByText(Convert.ToString(DispatchType.Manual)).Selected = svcFormContract.ServiceFormDispatchMode ? false : true;
                        (editform.FindControl("txtFormName") as WclTextBox).Text = svcFormContract.FormName;

                        rbtnFormVer.Enabled = false;
                        rbtnNewForm.Enabled = false;
                        rblDispatchType.Enabled = false;

                        //Form Version
                        if (rbtnFormVer.Checked)
                        {
                            HtmlGenericControl dvParentForm = (HtmlGenericControl)editform.FindControl("dvParentForm");
                            dvParentForm.Attributes.Add("style", "display:inline");
                            ddlParentForm.SelectedValue = Convert.ToString(svcFormContract.ParentSvcFormID);
                            //UAT-2480
                            if (!svcFormContract.ParentSvcFormID.IsNullOrEmpty() && svcFormContract.ParentSvcFormID > AppConsts.NONE)
                            {
                                HtmlGenericControl dvSrvcFormTemplate = (HtmlGenericControl)editform.FindControl("dvSrvcFormTemplate");
                                dvSrvcFormTemplate.Visible = false;
                                HtmlGenericControl dvSrvcFormReminderTemplate = (HtmlGenericControl)editform.FindControl("dvSrvcFormReminderTemplate");
                                dvSrvcFormReminderTemplate.Visible = false;
                            }

                            Presenter.GetBkgServiceAttachedFormMappingByServiceFormID();
                            if (CurrentViewContext.IsBkgServiceAttachedFormMappingExists)
                            {
                                ddlParentForm.Enabled = false;
                            }

                            //to do upload form
                            Label lblUploadFormName = (Label)editform.FindControl("lblUploadFormName");
                            lblUploadFormName.Text = svcFormContract.SystemDocumentFileName;
                            lblUploadFormName.Visible = true;

                            CustomValidator rfvParentForm = (editform.FindControl("rfvParentForm") as CustomValidator);
                            rfvParentForm.Enabled = true;
                        }

                        //New Form and Automatic
                        if (rbtnNewForm.Checked && svcFormContract.ServiceFormDispatchMode)
                        {
                            //to do upload form
                            Label lblUploadFormName = (Label)editform.FindControl("lblUploadFormName");
                            lblUploadFormName.Text = svcFormContract.SystemDocumentFileName;
                            lblUploadFormName.Visible = true;

                            HtmlGenericControl dvParentForm = (HtmlGenericControl)editform.FindControl("dvParentForm");
                            HtmlGenericControl dvSrvcFormTemplate = (HtmlGenericControl)editform.FindControl("dvSrvcFormTemplate");
                            HtmlGenericControl dvSrvcFormReminderTemplate = (HtmlGenericControl)editform.FindControl("dvSrvcFormReminderTemplate");
                            HtmlGenericControl dvDispatchType = (HtmlGenericControl)editform.FindControl("dvDispatchType");

                            CustomValidator rfvParentForm = (editform.FindControl("rfvParentForm") as CustomValidator);
                            rfvParentForm.Enabled = false;

                            dvParentForm.Attributes.Add("style", "display: none");
                            dvSrvcFormTemplate.Attributes.Add("style", "display: inline");
                            dvSrvcFormReminderTemplate.Attributes.Add("style", "display: inline");
                            dvDispatchType.Attributes.Add("style", "display: inline");

                            Presenter.GetServiceFormCommunicationTemplateData();
                            List<Entity.CommunicationTemplateEntity> serviceFormCommunicationTemplateData = CurrentViewContext.ServiceFormCommunicationTemplateData;

                            var serviceFormTemplateData = serviceFormCommunicationTemplateData
                                                          .Where(cond => cond.lkpCommunicationEntityType.CET_Code
                                                                                    == CommunicationEntityType.BACKGROUND_SERVICE_FORM.GetStringValue())
                                                          .FirstOrDefault();

                            var serviceFormReminderTemplateData = serviceFormCommunicationTemplateData
                                                          .Where(cond => cond.lkpCommunicationEntityType.CET_Code
                                                                            == CommunicationEntityType.SERVICE_FORM_NOTIFICATION_REMINDER.GetStringValue())
                                                          .FirstOrDefault();

                            (editform.FindControl("txtTemplateName") as WclTextBox).Text = serviceFormTemplateData.CommunicationTemplate.Name;
                            (editform.FindControl("txtTemplateSubject") as WclTextBox).Text = serviceFormTemplateData.CommunicationTemplate.Subject;
                            (editform.FindControl("ecTemplateContent") as WclEditor).Content = serviceFormTemplateData.CommunicationTemplate.Content;

                            (editform.FindControl("txtReminderTemplateName") as WclTextBox).Text = serviceFormReminderTemplateData.CommunicationTemplate.Name;
                            (editform.FindControl("txtReminderSubject") as WclTextBox).Text = serviceFormReminderTemplateData.CommunicationTemplate.Subject;
                            (editform.FindControl("ecReminderContent") as WclEditor).Content = serviceFormReminderTemplateData.CommunicationTemplate.Content;

                            (editform.FindControl("hdnSvcFormCommunicationTemplateID") as HiddenField).Value = Convert.ToString(serviceFormTemplateData.CommunicationTemplate.CommunicationTemplateID);
                            (editform.FindControl("hdnReminderCommunicationTemplateID") as HiddenField).Value = Convert.ToString(serviceFormReminderTemplateData.CommunicationTemplate.CommunicationTemplateID);
                        }
                        ////New Form and Manual
                        else if (rbtnNewForm.Checked && !svcFormContract.ServiceFormDispatchMode)
                        {
                            HtmlGenericControl dvParentForm = (HtmlGenericControl)editform.FindControl("dvParentForm");
                            dvParentForm.Attributes.Add("style", "display:none");
                            HtmlGenericControl dvDispatchType = (HtmlGenericControl)editform.FindControl("dvDispatchType");
                            dvDispatchType.Attributes.Add("style", "display: inline");
                            HtmlGenericControl dvUploadSvcForm = (HtmlGenericControl)editform.FindControl("dvUploadSvcForm");
                            dvUploadSvcForm.Attributes.Add("style", "display: none");
                            CustomValidator rfvParentForm = (editform.FindControl("rfvParentForm") as CustomValidator);
                            rfvParentForm.Enabled = false;
                        }
                    }

                }
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

        #region DropDown Events

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        private Boolean ManageUploadServiceForm(GridItem gridItem, Boolean isUpdate)
        {
            WclAsyncUpload uploadControl = gridItem.FindControl("uploadControl") as WclAsyncUpload;
            Label lblUploadFormMsg = (gridItem.FindControl("lblUploadFormMsg") as Label);
            lblUploadFormMsg.Visible = false;
            if (uploadControl.UploadedFiles.Count > 0)
            {
                UploadDocument(uploadControl);
                if (CurrentViewContext.IsCorruptedDocument)
                {
                    base.ShowErrorMessage("Some error occurred while uploading Service Form. Please try again or contact System Administrator.");
                    return false;
                }
            }
            else if (!isUpdate)
            {
                lblUploadFormMsg.Visible = true;
                return false;
            }
            return true;
        }

        private void UploadDocument(WclAsyncUpload uploadControl)
        {
            String filePath = String.Empty;
            Boolean aWSUseS3 = false;
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
            filePath = WebConfigurationManager.AppSettings[AppConsts.SYSTEM_DOCUMENT_LOCATION];
            if (!WebConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(WebConfigurationManager.AppSettings["AWSUseS3"]);
            }
            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + TenantId.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                if (filePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for " + AppConsts.SYSTEM_DOCUMENT_LOCATION + " in config.", null);
                    return;
                }
                if (!filePath.EndsWith("\\"))
                {
                    filePath += "\\";
                }

                filePath += "Tenant(" + TenantId.ToString() + @")\";

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }

            foreach (UploadedFile item in uploadControl.UploadedFiles)
            {
                Entity.SystemDocument systemDocument = new Entity.SystemDocument();
                String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                //Save file
                String newTempFilePath = Path.Combine(tempFilePath, fileName);
                item.SaveAs(newTempFilePath);

                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    //Move file to other location
                    String destFilePath = Path.Combine(filePath, fileName);
                    File.Copy(newTempFilePath, destFilePath);
                    systemDocument.DocumentPath = destFilePath;
                }
                else
                {
                    if (filePath.IsNullOrEmpty())
                    {
                        base.LogError("Please provide path for " + AppConsts.SYSTEM_DOCUMENT_LOCATION + " in config.", null);
                        return;
                    }
                    if (!filePath.EndsWith("//"))
                    {
                        filePath += "//";
                    }

                    //AWS code to save document to S3 location
                    AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                    String destFolder = filePath + "Tenant(" + TenantId.ToString() + @")/";
                    String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                    CurrentViewContext.IsCorruptedDocument = false;
                    //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                    if (returnFilePath.IsNullOrEmpty())
                    {
                        CurrentViewContext.IsCorruptedDocument = true;
                        continue;
                    }
                    systemDocument.DocumentPath = returnFilePath; //Path.Combine(destFolder, fileName);
                }
                try
                {
                    if (!String.IsNullOrEmpty(newTempFilePath))
                        File.Delete(newTempFilePath);
                }
                catch (Exception) { }


                systemDocument.FileName = item.FileName;
                systemDocument.Size = item.ContentLength;
                systemDocument.CreatedBy = CurrentUserId;
                systemDocument.CreatedOn = DateTime.Now;
                systemDocument.IsDeleted = false;
                CurrentViewContext.SystemDocumentToSaveUpdate = systemDocument;
            }
        }

        private void AddPlaceHolders(WclEditor wclEditor)
        {
            EditorTool editorDropDownTool = wclEditor.FindTool("ddPlaceHolders");
            EditorDropDown ddPlaceholders;
            if (editorDropDownTool.IsNull())
            {
                EditorToolGroup dynamicToolbar = new EditorToolGroup();
                wclEditor.Tools.Add(dynamicToolbar);

                ddPlaceholders = new EditorDropDown("ddPlaceHolders");
                ddPlaceholders.Text = AppConsts.COMBOBOX_ITEM_SELECT;

                ddPlaceholders.Attributes["width"] = "110px";
                ddPlaceholders.Attributes["popupwidth"] = "150px";
                ddPlaceholders.Attributes["popupheight"] = "100px";

                dynamicToolbar.Tools.Add(ddPlaceholders);
            }
            else
            {
                ddPlaceholders = editorDropDownTool as EditorDropDown;
            }

            ddPlaceholders.Items.Clear();
            foreach (var placeHolder in CurrentViewContext.TemplatePlaceHolders)
            {
                ddPlaceholders.Items.Add(placeHolder.Name, placeHolder.PlaceHolder);
            }
        }

        private void EnableDisableCommunicationTemplateReqFiedlValidator(Boolean enabled, GridEditFormItem editform)
        {

            RequiredFieldValidator rfvTemplateName = (editform.FindControl("rfvTemplateName") as RequiredFieldValidator);
            rfvTemplateName.Enabled = enabled;
            RequiredFieldValidator rfvContent = (editform.FindControl("rfvContent") as RequiredFieldValidator);
            rfvContent.Enabled = enabled;
            RequiredFieldValidator rfvSubject = (editform.FindControl("rfvSubject") as RequiredFieldValidator);
            rfvSubject.Enabled = enabled;
            RequiredFieldValidator rgvTemplateName = (editform.FindControl("rgvTemplateName") as RequiredFieldValidator);
            rgvTemplateName.Enabled = enabled;

            RequiredFieldValidator rfvReminderTemplateName = (editform.FindControl("rfvReminderTemplateName") as RequiredFieldValidator);
            rfvReminderTemplateName.Enabled = enabled;
            RequiredFieldValidator rgvReminderTemplateName = (editform.FindControl("rgvReminderTemplateName") as RequiredFieldValidator);
            rgvReminderTemplateName.Enabled = enabled;
            RequiredFieldValidator rfvReminderSubject = (editform.FindControl("rfvReminderSubject") as RequiredFieldValidator);
            rfvReminderSubject.Enabled = enabled;
            RequiredFieldValidator rfvReminderContent = (editform.FindControl("rfvReminderContent") as RequiredFieldValidator);
            rfvReminderContent.Enabled = enabled;
        }

        #endregion

        #endregion
    }
}