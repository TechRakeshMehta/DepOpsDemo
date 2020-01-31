using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.Templates;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity;
using System.Data;
using System.Web.Configuration;
using System.IO;

namespace CoreWeb.SystemSetUp.Views
{
    public partial class CreateAccountInvitation : BaseUserControl, ICreateAccountInvitationView
    {
        #region Variables

        #region Private Variables

        private Int32 tenantId = 0;
        private object _dataItem = null;
        private CreateAccountInvitationPresenter _presenter = new CreateAccountInvitationPresenter();
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public CreateAccountInvitationPresenter Presenter
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

        public Boolean IsEditModeOn
        {
            get
            {
                if (ViewState["IsEditModeOn"] != null)
                {
                    return Convert.ToBoolean(ViewState["IsEditModeOn"]);
                }
                return false;
            }
            set
            {
                txtNewEmail.Enabled = !value;
                txtNewFirstName.Enabled = !value;
                txtNewLastName.Enabled = !value;
                btnAddNewRecord.Enabled = !value;
                ViewState["IsEditModeOn"] = value;
            }
        }

        public Boolean IsLabelMode
        {
            get;
            set;
        }

        public object DataItem
        {
            get
            {
                if (!ViewState["DataItemViewState"].IsNullOrEmpty())
                {
                    return (Object)ViewState["DataItemViewState"];
                }
                else
                {
                    return this._dataItem;
                }
            }
            set
            {
                this._dataItem = value;
            }
        }

        #endregion


        Int32 ICreateAccountInvitationView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 ICreateAccountInvitationView.SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    Int32.TryParse(ddlTenantName.SelectedValue, out _selectedTenantId);
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
            }
        }

        Int32 ICreateAccountInvitationView.TenantID
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;
            }
            set { tenantId = value; }
        }

        ICreateAccountInvitationView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Boolean ICreateAccountInvitationView.HasDuplicateNames
        {
            get
            {
                List<String> nameList = new List<String>();

                if (!String.IsNullOrWhiteSpace(txtNewFirstName.Text))
                {
                    nameList.Add(txtNewFirstName.Text.ToLower().Trim() + "#" + txtNewLastName.Text.ToLower().Trim());
                }
                nameList.AddRange(CurrentViewContext.AccountInvitationTempList.Select(x => x.FirstName.ToLower().Trim() + "#" + x.LastName.ToLower().Trim()).ToList());

                nameList.Add(CurrentViewContext.UserFirstName.ToLower().Trim() + "#" + CurrentViewContext.UserLastName.ToLower().Trim());
                return !(nameList.Count() == nameList.Distinct().Count());

            }
        }

        String ICreateAccountInvitationView.ErrorMessage
        {
            get;
            set;
        }

        String ICreateAccountInvitationView.SuccessMessage
        {
            get;
            set;
        }

        //List<AccountInvitationContract> ICreateAccountInvitationView.AccountInvitationList
        //{
        //    get
        //    {
        //        if (IsEditModeOn)
        //        {
        //            IsEditModeOn = false;
        //            rptrRecpientName.DataSource = CurrentViewContext.AccountInvitationTempList;
        //            rptrRecpientName.DataBind();
        //        }
        //        if (divErrorMessage.Visible)
        //            divErrorMessage.Visible = false;
        //        if (CurrentViewContext.AccountInvitationTempList.IsNotNull())
        //        {
        //            //  No point in adding anything if empty
        //            if (!String.IsNullOrWhiteSpace(txtNewFirstName.Text))
        //            {
        //                AccountInvitationContract invtationDetail = new AccountInvitationContract() { FirstName = txtNewFirstName.Text.Trim(), LastName = txtNewLastName.Text.Trim() };
        //                if (!CurrentViewContext.HasDuplicateNames)
        //                {
        //                    // Add a new Alias name
        //                    CurrentViewContext.AccountInvitationTempList.Add(invtationDetail);

        //                    rptrRecpientName.DataSource = CurrentViewContext.AccountInvitationTempList;
        //                    rptrRecpientName.DataBind();
        //                }
        //                txtNewFirstName.Text = String.Empty;
        //                txtNewLastName.Text = string.Empty;
        //                divErrorMessage.Visible = false;
        //            }
        //            return CurrentViewContext.AccountInvitationTempList;
        //        }
        //        return new List<AccountInvitationContract>();
        //    }
        //    set
        //    {
        //        CurrentViewContext.AccountInvitationTempList = value;
        //    }
        //}

        List<AccountInvitationContract> ICreateAccountInvitationView.AccountInvitationTempList
        {
            get
            {
                if (ViewState["AccountInvitationTempList"] != null)
                {
                    return ViewState["AccountInvitationTempList"] as List<AccountInvitationContract>;
                }
                //return null;
                return new List<AccountInvitationContract>();
            }
            set
            {
                ViewState["AccountInvitationTempList"] = value;
            }
        }

        String ICreateAccountInvitationView.UserFirstName
        {
            get
            {
                if (ViewState["UserFirstName"] != null)
                {
                    return Convert.ToString(ViewState["UserFirstName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["UserFirstName"] = value;
            }
        }

        String ICreateAccountInvitationView.UserLastName
        {
            get
            {
                if (ViewState["UserLastName"] != null)
                {
                    return Convert.ToString(ViewState["UserLastName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["UserLastName"] = value;
            }
        }

        String ICreateAccountInvitationView.UserEmail
        {
            get
            {
                if (ViewState["UserLastName"] != null)
                {
                    return Convert.ToString(ViewState["UserLastName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["UserLastName"] = value;
            }
        }

        List<Entity.ClientEntity.Tenant> ICreateAccountInvitationView.LstTenant
        {
            get;
            set;
        }

        List<CommunicationTemplatePlaceHolder> ICreateAccountInvitationView.TemplatePlaceHolders
        {
            get
            {
                if (ViewState["TemplatePlaceHolders"] != null)
                {
                    return (List<CommunicationTemplatePlaceHolder>)(ViewState["TemplatePlaceHolders"]);
                }
                return new List<CommunicationTemplatePlaceHolder>();
            }
            set
            {
                ViewState["TemplatePlaceHolders"] = value;
            }
        }

        SystemEventTemplatesContract ICreateAccountInvitationView.SystemEventTemplate
        {
            get
            {
                if (ViewState["SystemEventTemplate"] != null)
                {
                    return (SystemEventTemplatesContract)(ViewState["SystemEventTemplate"]);
                }
                return new SystemEventTemplatesContract();
            }
            set
            {
                ViewState["SystemEventTemplate"] = value;
            }
        }

        Int32 ICreateAccountInvitationView.TemplateId
        {
            get
            {
                return ViewState["TemplateId"].IsNull() ? AppConsts.NONE : (Int32)ViewState["TemplateId"];
            }
            set
            {
                ViewState["TemplateId"] = value;
            }
        }

        Int32 ICreateAccountInvitationView.SubEventID
        {
            get
            {
                return ViewState["SubEventID"].IsNull() ? AppConsts.NONE : (Int32)ViewState["SubEventID"];
            }
            set
            {
                ViewState["SubEventID"] = value;
            }
        }

        #endregion

        #region  Events

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Create Account Invitation";
                lblCreateAccountInvitation.Text = base.Title;
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
                IsLabelMode = true;
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindTenant();
                    if (CurrentViewContext.AccountInvitationTempList.IsNull())
                        CurrentViewContext.AccountInvitationTempList = new List<AccountInvitationContract>();

                    rptrRecpientName.DataSource = CurrentViewContext.AccountInvitationTempList;
                    rptrRecpientName.DataBind();
                    BindEditFormData();
                }
                Presenter.OnViewLoaded();
                base.SetPageTitle("Create Account Invitation");
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

        #region DropdownEvent
        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
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

        #region Repeater Events
        protected void rptrRecpientName_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                AccountInvitationContract accountInvitation = null;

                if (e.CommandName == "delete")
                {
                    CurrentViewContext.AccountInvitationTempList.Remove(CurrentViewContext.AccountInvitationTempList[e.Item.ItemIndex]);
                    divErrorMessage.Visible = false;
                    IsEditModeOn = false;

                    rptrRecpientName.DataSource = CurrentViewContext.AccountInvitationTempList;
                    rptrRecpientName.DataBind();

                }
                if (e.CommandName == "cancel")
                {
                    WclTextBox txtFirstName = e.Item.FindControl("txtFirstName") as WclTextBox;
                    if (txtFirstName.IsNotNull())
                        txtFirstName.Visible = false;
                    WclTextBox txtLastName = e.Item.FindControl("txtLastName") as WclTextBox;
                    if (txtLastName.IsNotNull())
                        txtLastName.Visible = false;
                    WclTextBox txtEmail = e.Item.FindControl("txtEmail") as WclTextBox;
                    if (txtEmail.IsNotNull())
                        txtEmail.Visible = true;

                    if (IsLabelMode)
                    {
                        Label lblfirstName = e.Item.FindControl("lblfirstName") as Label;
                        Label lblLastName = e.Item.FindControl("lblLastName") as Label;
                        if (lblfirstName.IsNotNull())
                            lblfirstName.Visible = true;
                        if (lblLastName.IsNotNull())
                            lblLastName.Visible = true;

                        Label lblEmail = e.Item.FindControl("lblEmail") as Label;
                        if (lblEmail.IsNotNull())
                            lblEmail.Visible = true;
                    }
                    else
                    {
                        WclTextBox txtFirstName1 = e.Item.FindControl("txtFirstName1") as WclTextBox;
                        WclTextBox txtLastName1 = e.Item.FindControl("txtLastName1") as WclTextBox;
                        if (txtFirstName1.IsNotNull())
                            txtFirstName1.Visible = true;
                        if (txtLastName1.IsNotNull())
                            txtLastName1.Visible = true;
                        WclTextBox txtEmail1 = e.Item.FindControl("txtEmail1") as WclTextBox;
                        if (txtEmail1.IsNotNull())
                            txtEmail1.Visible = true;
                    }


                    LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                    if (btnEdit.IsNotNull())
                    {
                        btnEdit.Text = "Edit";
                        btnEdit.CommandName = "edit";
                    }
                    LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                    if (btnDelete.IsNotNull())
                    {
                        btnDelete.Text = "Delete";
                        btnDelete.CommandName = "delete";
                        btnDelete.OnClientClick = "return confirm('Are you sure you want to delete the record ?')";
                    }
                    divErrorMessage.Visible = false;
                    IsEditModeOn = false;
                    rptrRecpientName.DataSource = CurrentViewContext.AccountInvitationTempList;
                    rptrRecpientName.DataBind();
                }
                else if (e.CommandName == "edit")
                {
                    if (IsEditModeOn)
                    {
                        divErrorMessage.Visible = true;
                        lblErrorMsg.Text = "Only one record can be updated at a time.";
                    }
                    else
                    {
                        WclTextBox txtFirstName = e.Item.FindControl("txtFirstName") as WclTextBox;
                        if (txtFirstName.IsNotNull())
                            txtFirstName.Visible = true;
                        WclTextBox txtLastName = e.Item.FindControl("txtLastName") as WclTextBox;
                        if (txtLastName.IsNotNull())
                            txtLastName.Visible = true;
                        WclTextBox txtEmail = e.Item.FindControl("txtEmail") as WclTextBox;
                        if (txtEmail.IsNotNull())
                            txtEmail.Visible = true;
                        if (IsLabelMode)
                        {

                            Label lblfirstName = e.Item.FindControl("lblfirstName") as Label;
                            Label lblLastName = e.Item.FindControl("lblLastName") as Label;
                            if (lblfirstName.IsNotNull())
                                lblfirstName.Visible = false;
                            if (lblLastName.IsNotNull())
                                lblLastName.Visible = false;
                            Label lblEmail = e.Item.FindControl("lblEmail") as Label;
                            if (lblEmail.IsNotNull())
                                lblEmail.Visible = false;
                        }
                        else
                        {
                            WclTextBox txtFirstName1 = e.Item.FindControl("txtFirstName1") as WclTextBox;
                            WclTextBox txtLastName1 = e.Item.FindControl("txtLastName1") as WclTextBox;
                            if (txtFirstName1.IsNotNull())
                                txtFirstName1.Visible = true;
                            if (txtLastName1.IsNotNull())
                                txtLastName1.Visible = true;

                            WclTextBox txtEmail1 = e.Item.FindControl("txtEmail1") as WclTextBox;
                            if (txtEmail1.IsNotNull())
                                txtEmail1.Visible = true;
                        }



                        LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                        if (btnEdit.IsNotNull())
                        {
                            btnEdit.Text = "Save";
                            btnEdit.CommandName = "save";
                        }
                        LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                        if (btnDelete.IsNotNull())
                        {
                            btnDelete.Text = "Cancel";
                            btnDelete.CommandName = "cancel";
                            btnDelete.OnClientClick = "";
                        }
                        divErrorMessage.Visible = false;
                        IsEditModeOn = true;
                    }
                }
                else if (e.CommandName == "save")
                {
                    accountInvitation = CurrentViewContext.AccountInvitationTempList[e.Item.ItemIndex];
                    WclTextBox txtFirstName = e.Item.FindControl("txtFirstName") as WclTextBox;
                    WclTextBox txtLastName = e.Item.FindControl("txtLastName") as WclTextBox;
                    WclTextBox txtEmail = e.Item.FindControl("txtEmail") as WclTextBox;
                    if (txtFirstName.IsNotNull() && txtLastName.IsNotNull() && txtEmail.IsNotNull())
                    {
                        if (Presenter.IsEmailAlreadyExist(txtEmail.Text.Trim()))
                        {
                            IsEditModeOn = true;
                            base.ShowInfoMessage("Email address is already in use.");
                            return;
                        }

                        if (CurrentViewContext.UserEmail.ToLower().Equals(txtEmail.Text.ToLower()))
                        {
                            IsEditModeOn = true;
                            //divErrorMessage.Visible = true;
                            base.ShowInfoMessage("Duplicate records cannot be added.");
                            return;
                        }

                        if (!(accountInvitation.FirstName.ToLower() == txtFirstName.Text.ToLower().Trim() && accountInvitation.LastName.ToLower() == txtLastName.Text.ToLower().Trim() && accountInvitation.Email.ToLower() == txtEmail.Text.ToLower().Trim()))
                        {
                            if (!CurrentViewContext.AccountInvitationTempList.Any(cond => cond.Email.ToLower() == txtEmail.Text.ToLower().Trim()) || txtEmail.Text == accountInvitation.Email)
                            {
                                accountInvitation.FirstName = txtFirstName.Text.Trim();
                                accountInvitation.LastName = txtLastName.Text.Trim();
                                accountInvitation.Email = txtEmail.Text.Trim();
                                txtFirstName.Visible = false;
                                txtLastName.Visible = false;
                                txtEmail.Visible = false;


                                if (IsLabelMode)
                                {
                                    Label lblfirstName = e.Item.FindControl("lblfirstName") as Label;
                                    if (lblfirstName.IsNotNull())
                                        lblfirstName.Visible = true;
                                    Label lblLastName = e.Item.FindControl("lblLastName") as Label;
                                    if (lblLastName.IsNotNull())
                                        lblLastName.Visible = true;
                                    Label lblEmail = e.Item.FindControl("lblEmail") as Label;
                                    if (lblEmail.IsNotNull())
                                        lblEmail.Visible = true;
                                }
                                else
                                {

                                    WclTextBox txtFirstName1 = e.Item.FindControl("txtFirstName1") as WclTextBox;
                                    if (txtFirstName1.IsNotNull())
                                        txtFirstName1.Visible = true;
                                    WclTextBox txtLastName1 = e.Item.FindControl("txtLastName1") as WclTextBox;
                                    if (txtLastName1.IsNotNull())
                                        txtLastName1.Visible = true;
                                    WclTextBox txtEmail1 = e.Item.FindControl("txtEmail1") as WclTextBox;
                                    if (txtEmail1.IsNotNull())
                                        txtEmail1.Visible = true;
                                }

                                LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                                if (btnEdit.IsNotNull())
                                {
                                    btnEdit.Text = "Edit";
                                    btnEdit.CommandName = "edit";
                                }
                                LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                                if (btnDelete.IsNotNull())
                                {
                                    btnDelete.Text = "Delete";
                                    btnDelete.CommandName = "delete";
                                    btnDelete.OnClientClick = "return confirm('Are you sure you want to delete the record ?')";
                                }
                                divErrorMessage.Visible = false;
                                IsEditModeOn = false;
                                rptrRecpientName.DataSource = CurrentViewContext.AccountInvitationTempList;
                                rptrRecpientName.DataBind();
                            }
                            else
                            {
                                IsEditModeOn = true;
                                base.ShowInfoMessage("Duplicate records cannot be added.");
                            }
                        }
                        else
                        {
                            txtFirstName.Visible = false;
                            txtLastName.Visible = false;
                            txtEmail.Visible = false;

                            if (IsLabelMode)
                            {
                                Label lblfirstName = e.Item.FindControl("lblfirstName") as Label;
                                if (lblfirstName.IsNotNull())
                                    lblfirstName.Visible = true;
                                Label lblLastName = e.Item.FindControl("lblLastName") as Label;
                                if (lblLastName.IsNotNull())
                                    lblLastName.Visible = true;
                                Label lblEmail = e.Item.FindControl("lblEmail") as Label;
                                if (lblEmail.IsNotNull())
                                    lblEmail.Visible = true;
                            }
                            else
                            {
                                WclTextBox txtFirstName1 = e.Item.FindControl("txtFirstName1") as WclTextBox;
                                if (txtFirstName1.IsNotNull())
                                    txtFirstName1.Visible = true;
                                WclTextBox txtLastName1 = e.Item.FindControl("txtLastName1") as WclTextBox;
                                if (txtLastName1.IsNotNull())
                                    txtLastName1.Visible = true;
                                WclTextBox txtEmail1 = e.Item.FindControl("txtEmail1") as WclTextBox;
                                if (txtEmail1.IsNotNull())
                                    txtEmail1.Visible = true;
                            }

                            LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                            if (btnEdit.IsNotNull())
                            {
                                btnEdit.Text = "Edit";
                                btnEdit.CommandName = "edit";
                            }
                            LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                            if (btnDelete.IsNotNull())
                            {
                                btnDelete.Text = "Delete";
                                btnDelete.CommandName = "delete";
                                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete the record ?')";
                            }
                            divErrorMessage.Visible = false;
                            IsEditModeOn = false;
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

        protected void rptrRecpientName_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //To Do...
        }
        #endregion

        #region Button Events
        protected void AddMore_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantId > AppConsts.NONE)
                {

                    //  No point in adding anything if empty
                    if (!String.IsNullOrWhiteSpace(txtNewEmail.Text))
                    {
                        if (Presenter.IsEmailAlreadyExist(txtNewEmail.Text.Trim()))
                        {
                            base.ShowInfoMessage("Email address is already in use.");
                            return;
                        }

                        AccountInvitationContract accountInvitation = new AccountInvitationContract() { FirstName = txtNewFirstName.Text.Trim(), LastName = txtNewLastName.Text.Trim(), Email = txtNewEmail.Text.Trim() };
                        if ((!CurrentViewContext.AccountInvitationTempList.Any(cond => cond.Email.ToLower() == accountInvitation.Email.ToLower())))
                        {
                            // Add a new Record
                            var previousAccountInvitationTempList = CurrentViewContext.AccountInvitationTempList;
                            previousAccountInvitationTempList.Add(accountInvitation);
                            CurrentViewContext.AccountInvitationTempList = previousAccountInvitationTempList;
                            txtNewEmail.Text = String.Empty;
                            txtNewFirstName.Text = String.Empty;
                            txtNewLastName.Text = String.Empty;

                            divErrorMessage.Visible = false;
                            rptrRecpientName.DataSource = CurrentViewContext.AccountInvitationTempList;
                            rptrRecpientName.DataBind();

                            foreach (RepeaterItem ri in rptrRecpientName.Items)
                            {
                                Label lblfirstName = ri.FindControl("lblfirstName") as Label;
                                WclTextBox txtFirstName1 = ri.FindControl("txtFirstName1") as WclTextBox;
                                Label lblLastName = ri.FindControl("lblLastName") as Label;
                                WclTextBox txtLastName1 = ri.FindControl("txtLastName1") as WclTextBox;
                                if (IsLabelMode)
                                {
                                    if (lblfirstName.IsNotNull())
                                        lblfirstName.Visible = true;
                                    if (txtFirstName1.IsNotNull())
                                        txtFirstName1.Visible = false;
                                    if (lblLastName.IsNotNull())
                                        lblLastName.Visible = true;
                                    if (txtLastName1.IsNotNull())
                                        txtLastName1.Visible = false;
                                }
                                else
                                {
                                    if (lblfirstName.IsNotNull())
                                        lblfirstName.Visible = false;
                                    if (txtFirstName1.IsNotNull())
                                        txtFirstName1.Visible = true;
                                    if (lblLastName.IsNotNull())
                                        lblLastName.Visible = false;
                                    if (txtLastName1.IsNotNull())
                                        txtLastName1.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            base.ShowInfoMessage("Duplicate records cannot be added.");

                        }
                        IsEditModeOn = false;
                    }
                }
                else
                {
                    base.ShowInfoMessage("Please select institution.");
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

        protected void CmdBarSubmit_SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.AccountInvitationTempList.IsNullOrEmpty())
                {
                    ShowInfoMessage("Please add atleast one recipient.");
                    return;
                }
                else
                {
                    //UAT-1729: Bulk upload of the Admin Invitation to Complio and download of upload template.
                    var orgUsers = Presenter.IsEmailsAlreadyExist(CurrentViewContext.AccountInvitationTempList.Select(x => x.Email.ToLower()).ToList());
                    if (!orgUsers.IsNullOrEmpty())
                    {
                        var emails = String.Join(", ", orgUsers);
                        emails += " email address(s) are already in use.";
                        base.ShowInfoMessage(emails);
                        return;
                    }

                    //Maintain History : AccountCreationContacts
                    List<AccountCreationContact> accountCreationContactList = new List<AccountCreationContact>();
                    foreach (AccountInvitationContract accInvitationContract in CurrentViewContext.AccountInvitationTempList)
                    {
                        AccountCreationContact accountCreationContact = new AccountCreationContact();
                        accountCreationContact.ACC_FirstName = accInvitationContract.FirstName;
                        accountCreationContact.ACC_LastName = accInvitationContract.LastName;
                        accountCreationContact.ACC_Email = accInvitationContract.Email;
                        accountCreationContact.ACC_TenantID = Convert.ToInt32(ddlTenantName.SelectedValue);
                        accountCreationContact.ACC_AdminOrgUserID = CurrentViewContext.CurrentLoggedInUserId;
                        accountCreationContact.ACC_IsDeleted = false;
                        accountCreationContact.ACC_CreatedByID = CurrentViewContext.CurrentLoggedInUserId;
                        accountCreationContact.ACC_CreatedOn = DateTime.Now;
                        //Adding in list
                        accountCreationContactList.Add(accountCreationContact);
                    }

                    //Update the editor template
                    CurrentViewContext.SystemEventTemplate.TemplateContent = editorContent.Content;
                    CurrentViewContext.SystemEventTemplate.Subject = txtSubject.Text;

                    //2.Save Contacts and Send Email
                    if (Presenter.SaveAccountCreationContact(accountCreationContactList))
                    {
                        base.ShowSuccessMessage("Invitation(s) sent successfully.");
                        ResetForm(false);
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

        protected void CmdBarSubmit_CancelClick(object sender, EventArgs e)
        {
            try
            {
                //Redirect to dashboard.
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME));
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

        protected void CmdBarSubmit_ResetClick(object sender, EventArgs e)
        {
            try
            {
                //Reset form
                ResetForm(true);
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

        //UAT-1729: Bulk upload of the Admin Invitation to Complio and download of upload template.
        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                CreateSpreadsheet();
            }
            catch (System.Threading.ThreadAbortException ex)
            {

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

        protected void btnUploadTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploadControl.UploadedFiles.Count > AppConsts.NONE)
                {
                    UploadDocument();
                }
                else
                {
                    base.ShowInfoMessage("Please upload document.");
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

        #endregion

        #region Private Methods

        private void BindTenant()
        {
            ddlTenantName.DataSource = CurrentViewContext.LstTenant;
            ddlTenantName.DataBind();
            CurrentViewContext.SelectedTenantId = AppConsts.NONE;
            if (!Presenter.IsDefaultTenant)
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantID;
                ddlTenantName.Enabled = false;
            }
            ddlTenantName.SelectedValue = CurrentViewContext.SelectedTenantId.ToString();
        }

        /// <summary>
        /// Create the dropdown of the Place holders for selected sub-event
        /// </summary>
        private void BindPlaceHolders()
        {
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
            Presenter.GetPlaceHolderList(); //Fill SubEventID
            foreach (var placeHolder in CurrentViewContext.TemplatePlaceHolders)
            {
                ddPlaceholders.Items.Add(placeHolder.Name, placeHolder.PlaceHolder);
            }
        }

        /// <summary>
        /// Bind the data of the Edit Form
        /// </summary>
        private void BindEditFormData()
        {
            BindPlaceHolders();
            Presenter.GetTemplateDetails();  // Fill SystemEventTemplate and TemplateId
            if (CurrentViewContext.SystemEventTemplate.IsNotNull())
            {
                editorContent.Content = CurrentViewContext.SystemEventTemplate.TemplateContent;
                txtSubject.Text = CurrentViewContext.SystemEventTemplate.Subject;
            }
        }

        private void ResetForm(bool resetTenant = false)
        {
            CurrentViewContext.AccountInvitationTempList = new List<AccountInvitationContract>();
            rptrRecpientName.DataSource = CurrentViewContext.AccountInvitationTempList;
            rptrRecpientName.DataBind();
            txtNewFirstName.Text = String.Empty;
            txtNewLastName.Text = String.Empty;
            txtNewEmail.Text = String.Empty;
            IsEditModeOn = false;

            if (resetTenant)
            {
                if (Presenter.IsDefaultTenant)
                {
                    ddlTenantName.SelectedValue = AppConsts.ZERO;
                }
            }
            BindEditFormData();
        }

        //UAT-1729: Bulk upload of the Admin Invitation to Complio and download of upload template.
        /// <summary>
        /// Create Spreadsheet
        /// </summary>
        private void CreateSpreadsheet()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add(new DataColumn("FirstName", typeof(string)));
            dt.Columns.Add(new DataColumn("LastName", typeof(string)));
            dt.Columns.Add(new DataColumn("Email", typeof(string)));

            //calling create Excel File Method and passing dataTable   
            CreateExcelFile(dt);
        }

        /// <summary>
        /// Create Excel File from DataTable
        /// </summary>
        /// <param name="dt"></param>
        private void CreateExcelFile(DataTable dt)
        {
            var _fileName = "Exported_CreateAccountTemplate";
            Byte[] fileBytes = ExcelReader.GetCreateAccountInvitationBytes(dt, _fileName);

            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.xls", _fileName));
            response.Clear();
            response.BinaryWrite(fileBytes);
            response.End();
        }

        /// <summary>
        /// To save the uploaded files.
        /// </summary>
        private void UploadDocument()
        {
            foreach (UploadedFile item in uploadControl.UploadedFiles)
            {

                String tempFilePath = String.Empty;
                String fileName = String.Empty;
                String fileExtension = String.Empty;
                fileExtension = Path.GetExtension(item.FileName);
                try
                {
                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                        if (tempFilePath.IsNullOrEmpty())
                        {
                            base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                            return;
                        }

                        if (!tempFilePath.EndsWith(@"\"))
                        {
                            tempFilePath += @"\";
                        }

                        tempFilePath += "Tenant_" + CurrentViewContext.SelectedTenantId.ToString() + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";

                        String tempFileName = item.FileName;
                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);

                        fileName = Guid.NewGuid().ToString() + fileExtension;

                        string newPath = Path.Combine(tempFilePath, fileName);
                        item.SaveAs(tempFilePath + fileName);

                        //Read Excel Data
                        List<ApplicantDetailContract> applicantDetails = ExcelReader.GetApplicantListFromFile(newPath, true);
                        List<AccountInvitationContract> accountInvitationDetails = new List<AccountInvitationContract>();

                        applicantDetails.ForEach(x =>
                            {
                                AccountInvitationContract accountInvitationContract = new AccountInvitationContract();
                                accountInvitationContract.FirstName = x.FirstName;
                                accountInvitationContract.LastName = x.LastName;
                                accountInvitationContract.Email = x.Email.ToLower();
                                accountInvitationDetails.Add(accountInvitationContract);
                            });

                        var accountInvDetails = accountInvitationDetails.DistinctBy(x => x.Email).ToList();

                        if (!CurrentViewContext.AccountInvitationTempList.IsNullOrEmpty())
                        {
                            var previousAccountInvEmailList = CurrentViewContext.AccountInvitationTempList.Select(x => x.Email.ToLower()).ToList();
                            List<AccountInvitationContract> tempAccountInvDetails = new List<AccountInvitationContract>();
                            tempAccountInvDetails.AddRange(accountInvDetails);
                            foreach (var accountInvDetail in tempAccountInvDetails)
                            {
                                if (previousAccountInvEmailList.Contains(accountInvDetail.Email))
                                    accountInvDetails.Remove(accountInvDetail);
                            }
                            accountInvDetails.AddRange(CurrentViewContext.AccountInvitationTempList);
                        }
                        CurrentViewContext.AccountInvitationTempList = accountInvDetails;
                        rptrRecpientName.DataSource = CurrentViewContext.AccountInvitationTempList;
                        rptrRecpientName.DataBind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Please upload xls/xlsx file only.");
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
                finally
                {
                    //Delete directory after read excel sheet.
                    if (Directory.Exists(tempFilePath))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);
                        dirInfo.Delete(true);
                    }
                }
            }
        }

        #endregion

    }
}