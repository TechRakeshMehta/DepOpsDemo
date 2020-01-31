using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class ClientContactProfile : BaseUserControl, IClientContactProfileView
    {

        #region VARIABLES

        #region PRIVATE VARIABLES

        private ClientContactProfilePresenter _presenter = new ClientContactProfilePresenter();
        private Int32 tenantId = 0;
        private Boolean _editFlag = false;
        private Int32 _uploadedDocumentFileSize;
        private String _uploadedDocumentFilePath;
        private Boolean _isFalsePostBack = false;
        private String _viewType;
        #endregion

        #endregion

        #region PROPERTIES

        public ClientContactProfilePresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public IClientContactProfileView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        List<TenantDetailContract> IClientContactProfileView.LstTenant
        {
            get;
            set;
        }

        Int32 IClientContactProfileView.TenantID
        {
            get
            {
                if (tenantId == 0)
                {
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

        Boolean IClientContactProfileView.IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 IClientContactProfileView.SelectedTenantID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbTenant.SelectedValue))
                    return 0;
                return Convert.ToInt32(cmbTenant.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    cmbTenant.SelectedValue = Convert.ToString(value);
                }
                else
                {
                    cmbTenant.SelectedIndex = value;
                }
            }
        }

        Int32 IClientContactProfileView.ClientContactID
        {
            get
            {
                if (!ViewState["ClientContactID"].IsNull())
                {
                    return (Int32)(ViewState["ClientContactID"]);
                }

                return AppConsts.NONE;
            }
            set
            {
                ViewState["ClientContactID"] = value;
            }
        }

        Int32 IClientContactProfileView.CurrentLoggedInUserID
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        String IClientContactProfileView.ClientContactEmailID
        {
            get;
            set;
        }

        Boolean IClientContactProfileView.SuccessMsg
        {
            get;
            set;
        }

        OrganizationUserContract IClientContactProfileView.OrganizationUser
        {
            get;
            set;
        }

        List<Int32> DocumentUploadedHistory
        {
            get
            {
                if (!ViewState["DocumentUploadedHistory"].IsNull())
                {
                    return (List<Int32>)(ViewState["DocumentUploadedHistory"]);
                }
                return new List<Int32>();
            }
            set
            {
                ViewState["DocumentUploadedHistory"] = value;
            }
        }

        List<ClientContactSyllabusDocumentContract> IClientContactProfileView.RotationDocumentList
        {
            get;
            set;
        }

        //List<SharedSystemDocTypeContract> DocumentTypeListTemp
        //{
        //    get
        //    {
        //        if (!ViewState["DocumentTypeListTemp"].IsNull())
        //        {
        //            return (List<SharedSystemDocTypeContract>)(ViewState["DocumentTypeListTemp"]);
        //        }

        //        return new List<SharedSystemDocTypeContract>();
        //    }
        //    set
        //    {
        //        ViewState["DocumentTypeListTemp"] = value;
        //    }
        //}

        public Boolean IsFalsePostBack
        {
            get
            {
                return _isFalsePostBack;
            }
            set
            {
                _isFalsePostBack = value;
            }
        }

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }

        public String PageType
        {
            get
            {
                if (ViewState["PageType"] != null)
                    return (Convert.ToString(ViewState["PageType"]));
                return (Convert.ToString(ViewState["PageType"]));
            }
            set
            {
                ViewState["PageType"] = value;
            }
        }
        #endregion

        #region PAGE EVENTS

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Instructor/Preceptor Profile";
                base.SetPageTitle("Instructor/Preceptor Profile");
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
                //change done for Instructor/Preceptor dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    if (!this.IsPostBack || IsFalsePostBack)
                    {
                        Presenter.GetUserData();
                        BindControls();
                        HandleSyllabusGridSettings();
                    }
                    //ucUploadDocument.ParentPage = "editprofile";
                    //ucUploadDocument.IsFalsePostBack = IsFalsePostBack;
                }
                //ucUploadDocument.Visiblity = Visiblity;

                Presenter.GetUserData();
                if (SysXWebSiteUtils.SessionService.OrganizationUserId == CurrentViewContext.OrganizationUser.OrganizationUserID)
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = RedirectToChangePassword(queryString);
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

        #region CONTROL EVENTS

        protected void cmdbar_ClientContactProfile_SubmitClick(object sender, EventArgs e)
        {

        }

        #region DropDown Events

        protected void cmbTenant_DataBound(object sender, EventArgs e)
        {
            cmbTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //1.Get the ClientContact ID baClientContactEmailIDsed on TenantID and Email ID.
            //2. Bind the Document Grid.
            try
            {
                CurrentViewContext.ClientContactEmailID = base.SysXMembershipUser.Email;
                //ucUploadDocument.SelectedTenantID = String.IsNullOrEmpty(cmbTenant.SelectedValue) ? 0 : Convert.ToInt32(cmbTenant.SelectedValue);
                //ucUploadDocument.HandleDocumentUploadGridSettings();
                HandleSyllabusGridSettings();
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

        #region RotationDocumentGrid

        protected void grdRotationDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                //change done for Instructor/Preceptor dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    Presenter.GetClientContactRotationDocuments();
                    // grdRotationDocuments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                    grdRotationDocuments.DataSource = CurrentViewContext.RotationDocumentList;
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

        protected void grdRotationDocuments_ItemCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void grdRotationDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                LinkButton lnkRotationDoc = (LinkButton)e.Item.FindControl("lnkRotationDoc");
                Int32 doucmentID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DocumentID"]);
                string url = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?clientSystemDocumentId={0}&tenantId={1}", doucmentID, CurrentViewContext.SelectedTenantID);
                lnkRotationDoc.OnClientClick = "DownloadForm('" + url + "')";
                // anchor.HRef = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}", applicantDoc.ApplicantDocumentID, CurrentUserTenantId);
            }
        }
        #endregion

        #endregion

        #region PRIVATE METHODS

        private void BindControls()
        {

            Presenter.GetUserData();
            txtFirstName.Text = CurrentViewContext.OrganizationUser.FirstName;
            txtMiddleName.Text = CurrentViewContext.OrganizationUser.MiddleName;
            txtLastName.Text = CurrentViewContext.OrganizationUser.LastName;
            txtEmail.Text = CurrentViewContext.OrganizationUser.Email;
            txtSSN.Text = CurrentViewContext.OrganizationUser.SSN; //UAT-4355

            Presenter.IsAdminLoggedIn();
            CurrentViewContext.ClientContactEmailID = txtEmail.Text;
            Presenter.GetTenants();
            cmbTenant.DataSource = CurrentViewContext.LstTenant;
            cmbTenant.DataBind();

            //DocumentTypeListTemp = new List<SharedSystemDocTypeContract>(CurrentViewContext.DocumentTypeList);
            //Enable or disable tenant dropdown for admin or client admin 
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                cmbTenant.Enabled = true;
                CurrentViewContext.SelectedTenantID = 0;
            }
            else
            {
                cmbTenant.Enabled = false;
                CurrentViewContext.SelectedTenantID = CurrentViewContext.TenantID;
            }
        }

        //UAT 1426 WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
        //private void ShowHideUploadDocumentButton(WclGrid grdUploadDocuments)
        //{
        //    //hide add button if document count is 4(means all type of document is uploaded.)
        //    if (CurrentViewContext.UploadedDocumentList.Count == AppConsts.FOUR)
        //    {
        //        grdUploadDocuments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
        //    }
        //    else
        //    {
        //        grdUploadDocuments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;
        //    }
        //}

        //UAT 1426 WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
        //private Boolean IsDocumentIsAllowedToUpload(SharedSystemDocumentContract uploadedDocument)
        //{
        //    if (CurrentViewContext.UploadedDocumentList.Select(sel => sel.DocumentTypeID).Contains(uploadedDocument.DocumentTypeID))
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// Handle the number of document types available to upload document.
        ///// </summary>
        //private void HandleDocumentTypeDropdown()
        //{
        //    CurrentViewContext.DocumentTypeList = new List<SharedSystemDocTypeContract>(DocumentTypeListTemp);
        //    List<Int32> alreadyUploadDocTypes = CurrentViewContext.UploadedDocumentList.Select(x => x.DocumentTypeID).ToList();
        //    if (!alreadyUploadDocTypes.IsNullOrEmpty())
        //    {
        //        CurrentViewContext.DocumentTypeList.RemoveAll(x => alreadyUploadDocTypes.Contains(x.SharedSystemDocTypeID));
        //    }

        //}

        private void HandleSyllabusGridSettings()
        {
            if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
            {
                //Get Client Contact ID by Email 
                Presenter.GetClientContactByEmail();
                //Bind Upload Document Grid

                //cmdbar_ClientContactProfile.Visible = true;
                hfTenantId.Value = Convert.ToString(CurrentViewContext.SelectedTenantID);
            }
            else
            {
                //grdUploadDocuments.Visible = false;
                //grdRotationDocuments.Visible = false;
                //cmdbar_ClientContactProfile.Visible = false;
                CurrentViewContext.ClientContactID = AppConsts.NONE; //Reset client contactID
                hfTenantId.Value = String.Empty;
            }

            //Bind Rotation Document Grid
            Presenter.GetClientContactRotationDocuments();
            grdRotationDocuments.DataSource = CurrentViewContext.RotationDocumentList;
            grdRotationDocuments.DataBind();
        }

        ///UAT-4183
        /// <summary>
        /// Used for open the change password page.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private Dictionary<string, string> RedirectToChangePassword(Dictionary<String, String> queryString)
        {
            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            }
            if (queryString.ContainsKey("MenuID"))
            {
                if (queryString["MenuID"] == "1")
                {
                    this.PageType = "InstructorDashboard";
                }
                else
                {
                    this.PageType = string.Empty;
                }
            }            
           
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", @"~/IntsofSecurityModel/ChangePassword.ascx"},
                                                                    {"PageType",PageType}
                                                                 };
            _viewType = "Instructor";
            lnkChangePassword.NavigateUrl = String.Format("~/IntsofSecurityModel/default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

            //Adding tooltip for the user profile link only if link is enabled
            if (!string.IsNullOrWhiteSpace(lnkChangePassword.NavigateUrl))
            {
                lnkChangePassword.ToolTip = Resources.Language.CLICKTOCHANGEPASSWORD;
            }

            //Setting iframe as a target 
            lnkChangePassword.Target = "pageFrame";
            return queryString;
        }
        #endregion

        protected void fsucCmdBar_SaveClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.OrganizationUser = new OrganizationUserContract();
                CurrentViewContext.OrganizationUser.FirstName = txtFirstName.Text;
                CurrentViewContext.OrganizationUser.MiddleName = txtMiddleName.Text;
                CurrentViewContext.OrganizationUser.LastName = txtLastName.Text;
                CurrentViewContext.OrganizationUser.OrganizationUserID = CurrentUserId;
                CurrentViewContext.OrganizationUser.SSN =txtSSN.Text;  //UAT-4355
                Presenter.UpdateClientContactOrganisationUser();
                if (CurrentViewContext.SuccessMsg)
                {
                    base.ShowSuccessMessage("Profile updated successfully.");
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

        protected void fsucCmdBar_CancelClick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                     { "SelectedTenantID", Convert.ToString(CurrentViewContext.SelectedTenantID) },
                                                                    { "Child", AppConsts.INSTRUCTOR_PRESEPTOR_DASHBOARD}
                                                                 };
                Response.Redirect(String.Format("~/ProfileSharing/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
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

        protected void fsucCmdBar_ClearClick(object sender, EventArgs e)
        {
            try
            {
                _viewType = "Instructor";
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",ChildControls.OtherAccountLinkingNew},
                                                                    {"PageType",this.PageType}
                                                                 };
                string url = String.Format("~/IntsofSecurityModel/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
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
    }
}