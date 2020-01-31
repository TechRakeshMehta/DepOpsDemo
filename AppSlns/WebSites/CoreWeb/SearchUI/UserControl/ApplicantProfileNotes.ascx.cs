#region Namespaces

#region Syatem Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion
#region Project Specific
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
#endregion
#endregion

namespace CoreWeb.SearchUI.Views
{
    public partial class ApplicantProfileNotes : BaseUserControl, IApplicantProfileNotesView
    {
        #region Private Variables
        private String _viewType;
        private ApplicantProfileNotesPresenter _presenter = new ApplicantProfileNotesPresenter();
        private Boolean _isReloadData = false;
        //Start UAT-5052
        private Int32 _loggedInUserTenantId;
        //End UAT-5052
        #endregion

        #region Properties

        #region public Properties
        public IApplicantProfileNotesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public ApplicantProfileNotesPresenter Presenter
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

        public Int32 SelectedTenantId
        {
            get;
            set;
        }

        Int32 IApplicantProfileNotesView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        public Int32 ApplicantUserID
        {
            get;
            set;
        }

        public Boolean IsReadOnly
        {
            get;
            set;
        }

        public Entity.OrganizationUser OrganizationUser { get; set; }

        /// <summary>
        /// Gets and sets the Address1
        /// </summary>
        //String IApplicantProfileNotesView.NewNote
        //{
        //    get
        //    {
        //        return txtNewNote.Text.Trim();
        //    }
        //    set
        //    {
        //        txtNewNote.Text = value;
        //    }
        //}

        public List<ApplicantProfileNotesContract> ApplicantProfileNoteList
        {
            get
            {
                if (!(ViewState["ApplicantProfileNoteList"] is List<ApplicantProfileNotesContract>))
                {
                    ViewState["ApplicantProfileNoteList"] = new List<ApplicantProfileNotesContract>();
                }
                return (List<ApplicantProfileNotesContract>)ViewState["ApplicantProfileNoteList"];
            }
            set
            {
                ViewState["ApplicantProfileNoteList"] = value;
            }
        }

        public String CurrentLoggedInUserName
        {
            get
            {
                if (ViewState["CurrentLoggedInUserName"].IsNotNull())
                {
                    return Convert.ToString(ViewState["CurrentLoggedInUserName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["CurrentLoggedInUserName"] = value;
            }
        }

        //Start UAT-5052
        public Int32 LoggedInUserTenantId
        {
            get
            {
                if (_loggedInUserTenantId == 0)
                {
                    IntsofSecurityModel.SysXMembershipUser user = (IntsofSecurityModel.SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _loggedInUserTenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _loggedInUserTenantId;
            }
            set { _loggedInUserTenantId = value; }
        }

        Boolean IApplicantProfileNotesView.IsClientAdmin
        {
            get
            {
                if (ViewState["IsClientAdmin"].IsNotNull())
                {
                    return Convert.ToBoolean(ViewState["IsClientAdmin"]);
                }
                return false;
            }
            set
            {
                ViewState["IsClientAdmin"] = value;
            }
        }
        //End UAT-5052

        public Boolean IsAdminNotes { get; set; } //UAT-3326
        #endregion

        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
                    Presenter.GetCurrentLoggedInUserName();
                    //Start UAT-5052
                    Presenter.IsClientAdmin();
                    //End UAT-5052
                    //ApplyActionLevelPermission(ActionCollection, "Applicant profile Notes");
                }
                //UAT-1154
                //if (IsReadOnly)
                //{
                //    //divNewNotes.Visible = false;
                //    //hdrNotes.Visible = false;
                //    grdNotes.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                //    grdNotes.MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                //    grdNotes.MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = false;
                //}
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
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Notes";
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
        #endregion

        #region Grid Events
        protected void grdNotes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                List<ApplicantProfileNotesContract> tempApplicantNotesList = new List<ApplicantProfileNotesContract>();
                if (CurrentViewContext.ApplicantProfileNoteList.IsNull() || CurrentViewContext.ApplicantProfileNoteList.Count == AppConsts.NONE || _isReloadData == true || IsReadOnly == true)
                {
                    Presenter.GetApplicantProfileNotesList();
                    tempApplicantNotesList = CurrentViewContext.ApplicantProfileNoteList;
                    _isReloadData = false;
                }
                else
                {
                    tempApplicantNotesList = CurrentViewContext.ApplicantProfileNoteList.Where(cond => cond.APN_IsDeleted == false).ToList();
                }
                grdNotes.DataSource = tempApplicantNotesList;
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

        protected void grdNotes_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                ApplicantProfileNotesContract applicantprofilenoteContract = null;
                String updatedNote = (e.Item.FindControl("txtProfileNote") as WclTextBox).Text.Trim();
                Int32 applicantProfileNoteID = Convert.ToInt32((e.Item.FindControl("txtAPN_ID") as WclTextBox).Text);
                if (IsReadOnly)
                {
                    if (!Presenter.UpdateApplicantProfileNote(applicantProfileNoteID, updatedNote))
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                        grdNotes.Rebind();
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Note updated successfully.");
                    }
                }
                else
                {
                    if (applicantProfileNoteID > 0)
                    {
                        applicantprofilenoteContract = CurrentViewContext.ApplicantProfileNoteList.FirstOrDefault(x => x.APN_ID.Equals(applicantProfileNoteID));
                    }
                    else
                    {
                        String tempId = (e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempId"].ToString().Trim();
                        applicantprofilenoteContract = CurrentViewContext.ApplicantProfileNoteList.FirstOrDefault(x => x.TempId == tempId);
                    }
                    if (applicantprofilenoteContract.IsNotNull())
                    {
                        applicantprofilenoteContract.APN_ProfileNote = updatedNote;
                        if (!applicantprofilenoteContract.IsNew)
                        {
                            applicantprofilenoteContract.IsUpdated = true;
                            applicantprofilenoteContract.APN_ModifiedOn = DateTime.Now;
                            applicantprofilenoteContract.APN_ModifiedBy = CurrentViewContext.CurrentLoggedInUserId;
                        }
                        else
                        {
                            applicantprofilenoteContract.APN_CreatedOn = DateTime.Now;
                            applicantprofilenoteContract.APN_CreatedBy = CurrentViewContext.CurrentLoggedInUserId;
                            //Start UAT-5052
                            applicantprofilenoteContract.APN_IsVisibleToClientAdmin = CurrentViewContext.IsClientAdmin ? true : false;
                            //End UAT-5052
                        }
                    }
                }
                //Presenter.UpdateApplicantProfileNote(applicantProfilrNoteID, updatedNote);
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

        protected void grdNotes_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                ApplicantProfileNotesContract applicantprofilenoteContract = null;
                Int32 applicantProfileNoteID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("APN_ID"));
                if (IsReadOnly)
                {
                    if (!Presenter.DeleteApplicantProfileNote(applicantProfileNoteID))
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                        grdNotes.Rebind();
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Note deleted successfully.");
                    }
                }
                else
                {

                    if (applicantProfileNoteID > 0)
                    {
                        applicantprofilenoteContract = CurrentViewContext.ApplicantProfileNoteList.FirstOrDefault(x => x.APN_ID.Equals(applicantProfileNoteID));
                        if (applicantprofilenoteContract.IsNotNull())
                        {
                            applicantprofilenoteContract.APN_IsDeleted = true;
                            applicantprofilenoteContract.APN_ModifiedOn = DateTime.Now;
                            applicantprofilenoteContract.APN_ModifiedBy = CurrentViewContext.CurrentLoggedInUserId;
                        }
                    }
                    else
                    {
                        String tempId = (e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TempId"].ToString().Trim();
                        applicantprofilenoteContract = CurrentViewContext.ApplicantProfileNoteList.FirstOrDefault(x => x.TempId == tempId);
                        if (applicantprofilenoteContract.IsNotNull() && applicantprofilenoteContract.IsNew == true)
                        {
                            CurrentViewContext.ApplicantProfileNoteList.Remove(applicantprofilenoteContract);
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

        protected void grdNotes_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                ApplicantProfileNotesContract applicantprofilenoteContract = null;
                String updatedNote = (e.Item.FindControl("txtProfileNote") as WclTextBox).Text.Trim();
                if (IsReadOnly)
                {
                    if (!Presenter.SaveApplicantProfileNotes(updatedNote))
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                        grdNotes.Rebind();
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Note saved successfully.");
                    }
                }
                else
                {
                    if (CurrentViewContext.ApplicantProfileNoteList.IsNull() && CurrentViewContext.ApplicantProfileNoteList.Count == AppConsts.NONE)
                        CurrentViewContext.ApplicantProfileNoteList = new List<ApplicantProfileNotesContract>();

                    applicantprofilenoteContract = new ApplicantProfileNotesContract();
                    applicantprofilenoteContract.TempId = Guid.NewGuid().ToString();
                    applicantprofilenoteContract.APN_ProfileNote = updatedNote;
                    applicantprofilenoteContract.IsNew = true;
                    applicantprofilenoteContract.APN_OrganizationUserID = CurrentViewContext.ApplicantUserID;
                    applicantprofilenoteContract.APN_IsDeleted = false;
                    applicantprofilenoteContract.APN_CreatedBy = CurrentViewContext.CurrentLoggedInUserId;
                    applicantprofilenoteContract.APN_CreatedOn = DateTime.Now;
                    applicantprofilenoteContract.CreatedBy = CurrentLoggedInUserName;
                    //Start UAT-5052
                    applicantprofilenoteContract.APN_IsVisibleToClientAdmin = CurrentViewContext.IsClientAdmin ? true : false;
                    //End UAT-5052
                    CurrentViewContext.ApplicantProfileNoteList.Add(applicantprofilenoteContract);
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

        //Start UAT-5052
        protected void grdNotes_ItemDatabound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                Boolean IsVisibleToClientAdmin = false;
                IsVisibleToClientAdmin= ((ApplicantProfileNotesContract)e.Item.DataItem).APN_IsVisibleToClientAdmin;
                if (!CurrentViewContext.IsClientAdmin && IsVisibleToClientAdmin)
                {
                    dataItem["DeleteColumn"].Controls[0].Visible = false;
                    dataItem["EditCommandColumn"].Controls[0].Visible = false;
                }
            }
        }
        //End UAT-5052

        protected void grdNotes_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    HtmlControl spnMessage = (HtmlControl)editform.FindControl("spnMessage");
                    if (IsReadOnly && spnMessage.IsNotNull())
                    {
                        spnMessage.Visible = false;
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

        //protected void CmdBarCancel_Click(Object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Boolean status = Presenter.SaveApplicantProfileNotes();
        //        grdNotes.Rebind();
        //        if (status)
        //        {
        //            lblSuccess.Visible = true;
        //            lblSuccess.ShowMessage("Note Added successfully.", MessageType.SuccessMessage);
        //        }
        //        else
        //        {
        //            lblSuccess.Visible = true;
        //            lblSuccess.ShowMessage("Some error has occured while adding the note.", MessageType.Error);
        //        }
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //}

        //public override List<ClsFeatureAction> ActionCollection
        //{
        //    get
        //    {
        //        List<ClsFeatureAction> actionCollection = new List<ClsFeatureAction>();
        //        actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
        //        {
        //            ControlActionId = "Add",
        //            ControlActionLabel = "Add Note",
        //            SystemControl = btnAdd,
        //            ScreenName = "Bkg Notes"
        //        });
        //        return actionCollection;
        //    }
        //}

        //protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        //{
        //    base.ApplyActionLevelPermission(ctrlCollection, screenName);
        //}

        #region Methods

        #region Public Methods
        public Boolean SaveUpdateProfileNote()
        {
            try
            {
                Boolean status = false;
                if (Presenter.SaveUpdateProfileNote())
                {
                    status = true;
                    _isReloadData = true;
                    grdNotes.Rebind();
                }
                return status;
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
            return false;
        }
        #endregion

        #endregion


    }
}