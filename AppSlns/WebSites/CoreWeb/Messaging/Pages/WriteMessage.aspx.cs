using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using Telerik.Web.UI;
using CuteWebUI;
using System.Collections.Generic;
using System.Linq;
using Entity;
using System.Web.Services;
using Business.RepoManagers;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.Configuration;
using System.IO;

using System.Web.UI;
using System.Text;
using CoreWeb.Shell;
using CoreWeb.IntsofExceptionModel.Interface;
using System.Configuration;
using System.Threading;


namespace CoreWeb.Messaging.Views
{
    public partial class WriteMessage : Page, IWriteMessageView
    {
        String fileName = String.Empty;
        private WriteMessagePresenter _presenter = new WriteMessagePresenter();
        private ISysXExceptionService _exceptionService = SysXWebSiteUtils.ExceptionService;
        private bool _isNeedToEnableToCcButtonsAndEmailFunctionality;

        public WriteMessagePresenter Presenter
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

        private MessagingContract _viewContract = null;

        /// <summary>
        /// Get current context
        /// </summary>
        private IWriteMessageView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Get the email subject.
        /// </summary>
        String IWriteMessageView.Subject
        {
            get
            {
                return txtSubject.Text;
            }
            set
            {
                txtSubject.Text = value;
            }
        }

        /// <summary>
        /// Get the message content
        /// </summary>
        String IWriteMessageView.Content
        {
            get
            {
                return editorContent.Content;
            }
            set
            {
                editorContent.Content = value; editorContent.Focus();
            }
        }

        /// <summary>
        /// Get email cc list
        /// </summary>
        String IWriteMessageView.CCList
        {
            get
            {
                return Convert.ToString(acbCcList.Entries);
            }
            set
            {
                //if (_isTemplateSelected)
                //{
                //    string[] cclist = value.Split(';');
                //    for (int i = 0; i < cclist.Count(); i++)
                //    {
                //        if (!string.IsNullOrEmpty(cclist[i].Trim()))
                //            acbCcList.Entries.Add(new AutoCompleteBoxEntry { Text = cclist[i] });
                //    } 
                //}
            }
        }

        /// <summary>
        /// Get the email tolist
        /// </summary>
        String IWriteMessageView.ToList
        {
            get
            {
                return Convert.ToString(acbToList.Entries);
            }
            set
            {
                //if (_isTemplateSelected)
                //{
                //    String[] toList = value.Split(';');
                //    for (int i = 0; i < toList.Count(); i++)
                //    {
                //        if (!String.IsNullOrEmpty(toList[i].Trim()))
                //            acbToList.Entries.Add(new AutoCompleteBoxEntry { Text = toList[i] });
                //    } 
                //}
            }
        }

        public Guid MessageId
        {
            get
            {

                return (Guid)ViewState["MessageId"];
            }
            set
            {
                ViewState["MessageId"] = value;
            }
        }

        /// <summary>
        /// View Contract
        /// </summary>
        MessagingContract IWriteMessageView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new MessagingContract();
                }
                return _viewContract;
            }
        }
        /// <summary>
        /// Get current userId
        /// </summary>
        Dictionary<Int32, String> IWriteMessageView.TenantTypes
        {
            get;
            set;
            /* // INITIAL PHASE get
              {
                  Dictionary<Int32, String> messageType = new Dictionary<Int32, String>();
                  if (!cmbTenantType.SelectedItem.Text.Equals("--SELECT--", StringComparison.OrdinalIgnoreCase))
                  {
                      messageType.Add(Convert.ToInt32(cmbTenantType.SelectedValue), cmbTenantType.SelectedItem.Text);
                  }
                  return messageType;
              }
              set
              {
                  cmbTenantType.DataSource = value;
                  cmbTenantType.DataBind();
              }*/
        }

        /// <summary>
        /// Get and set MessageType
        /// </summary>
        Int32 IWriteMessageView.MessageType
        {
            get;
            set;
        }

        public String CommunicationType
        {
            get
            {
                return Convert.ToString(Request[AppConsts.COMMUNICATION_TYPE_QUERY_STRING]);
            }
        }

        /// <summary>
        /// Get and set DocumentID
        /// </summary>
        String IWriteMessageView.DocumentID
        {
            get;
            set;
        }

        Dictionary<String, String> IWriteMessageView.AttachedFiles
        {
            get
            {
                Dictionary<String, String> attachedFiles = null;
                if (acbAttachedFiles.Entries != null && acbAttachedFiles.Entries.Count > 0)
                {
                    attachedFiles = new Dictionary<string, string>();
                    foreach (AutoCompleteBoxEntry entry in acbAttachedFiles.Entries)
                        attachedFiles.Add(entry.Value, entry.Text);
                }
                return attachedFiles;
            }
            set
            {
                if (value != null && value.Count > 0)
                {
                    foreach (var attachedFile in value)
                    {
                        acbAttachedFiles.Entries.Add(new AutoCompleteBoxEntry()
                        {
                            Text = attachedFile.Value,
                            Value = attachedFile.Key
                        });
                    }
                }
                else
                    acbAttachedFiles.Entries.Clear();
            }
        }

        public byte[] DocumentFile
        {
            get;
            set;
        }

        public string OriginalDocumentName
        {
            get;
            set;
        }

        /// <summary>
        /// Get queuetype
        /// </summary>
        Int32 IWriteMessageView.QueueType
        {
            get
            {
                return Convert.ToInt32(Request["queueType"]);
            }
        }

        IQueryable<ADBMessage> IWriteMessageView.CompanyTemplates
        {
            set
            {
                var mes = value.FirstOrDefault(x => x.ADBMessageID == new Guid("2A0DC07F-928C-471F-B8B3-AE5E797D1F43"));
                cmbTemplates.DataSource = value;
                cmbTemplates.DataBind();
                cmbTemplates.Items.Insert(0, new RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT));
            }
        }
        /// <summary>
        /// Get current userId
        /// </summary>
        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IQueryable<lkpMessageType> BindMessageType
        {
            set
            {
                /* // INITIAL PHASE   cmbMessageType.DataTextField = "MessageTypeCode";
                    cmbMessageType.DataValueField = "MessageTypeID";
                    cmbMessageType.DataSource = value;
                    cmbMessageType.ClearSelection();
                    cmbMessageType.DataBind();
                    cmbMessageType.Items.Insert(0, new RadComboBoxItem("--SELECT--", "0"));*/
            }
        }
        /// <summary>
        /// Receiver type
        /// </summary>
        String IWriteMessageView.ReceiverType
        {
            get;
            set;
        }

        public MessagingAction Action
        {
            get
            {
                if (ViewState["Action"] != null)
                    return (MessagingAction)ViewState["Action"];
                return MessagingAction.NewMail;
            }
            set
            {
                ViewState["Action"] = value;
            }
        }


        public Boolean IsDraftMessage
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDraftMessage"]);
            }
            set
            {
                ViewState["IsDraftMessage"] = value;
            }
        }

        /// <summary>
        /// Get and set MessageType
        /// </summary>
        Int32 IWriteMessageView.MessageTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Receiver type id
        /// </summary>
        Int32 IWriteMessageView.ReceiverTypeId
        {
            get;
            set;
        }

        ///// <summary>
        ///// Get current userId
        ///// </summary>
        //Int32 IWriteMessageView.UserGroupId
        //{
        //    get
        //    {
        //        return Convert.ToInt32(Request["userGroupId"].ToString());
        //    }
        //}

        String IWriteMessageView.DocumentName
        {
            get { return Convert.ToString(ViewState["DocumentName"]); }
            set { ViewState["DocumentName"] = value; }
        }

        String IWriteMessageView.EVaultDocumentID
        {
            get;
            set;
        }
        ///// <summary>
        ///// Get the email tolist
        ///// </summary>
        //String IWriteMessageView.IsUserGroup
        //{
        //    get
        //    {
        //        return hdnIsUserGroupforCompany.Value;
        //    }
        //    set
        //    {
        //        hdnIsUserGroupforCompany.Value = value;
        //    }
        //}

        ///// <summary>
        ///// Get the email tolist
        ///// </summary>
        //String IWriteMessageView.CcIsUserGroup
        //{
        //    get
        //    {
        //        return hdnCcIsUserGroupforCompany.Value;
        //    }
        //    set
        //    {
        //        hdnCcIsUserGroupforCompany.Value = value;
        //    }
        //}

        /// <summary>
        /// Get the email tolist
        /// </summary>
        String IWriteMessageView.ToListIds
        {
            get
            {
                //if (!IsApplicant)
                //{

                StringBuilder toIds = new StringBuilder();
                for (int i = 0; i < acbToList.Entries.Count; i++)
                {
                    toIds.Append(Convert.ToString(acbToList.Entries[i].Value) + ";");
                }
                return Convert.ToString(toIds);
                //}
                //return string.Empty;
            }
            set
            {
                //if (!IsApplicant)
                //{
                //acbToList.Entries.Clear(); UAT-439
                String[] toList = value.Split(';');
                for (int i = 0; i < toList.Count(); i++)
                {
                    if (!String.IsNullOrEmpty(toList[i].Trim()))
                        acbToList.Entries.Add(new AutoCompleteBoxEntry { Text = toList[i].Split(':')[0], Value = toList[i].Split(':')[1] });
                }
                //}
            }
        }


        /// <summary>
        /// Get the email Cclist
        /// </summary>
        String IWriteMessageView.CcListIds
        {
            get
            {
                //if (!IsApplicant)
                //{
                StringBuilder ccIds = new StringBuilder();
                for (int i = 0; i < acbCcList.Entries.Count; i++)
                {
                    ccIds.Append(Convert.ToString(acbCcList.Entries[i].Value) + ";");
                }
                return Convert.ToString(ccIds);
                //}
                //return string.Empty;
            }
            set
            {
                //if (!IsApplicant)
                //{
                //acbCcList.Entries.Clear(); UAT-439
                String[] ccList = value.Split(';');
                for (int i = 0; i < ccList.Count(); i++)
                {
                    if (!String.IsNullOrEmpty(ccList[i].Trim()))
                        acbCcList.Entries.Add(new AutoCompleteBoxEntry { Text = ccList[i].Split(':')[0], Value = ccList[i].Split(':')[1] });
                }
                //}
            }
        }

        /// <summary>
        /// Get the email BCclist
        /// </summary>
        String IWriteMessageView.BccListIds
        {
            get
            {
                StringBuilder bccIds = new StringBuilder();
                for (int i = 0; i < acbBccList.Entries.Count; i++)
                {
                    bccIds.Append(Convert.ToString(acbBccList.Entries[i].Value) + ";");
                }
                return Convert.ToString(bccIds);
            }
            set
            {
                // acbBccList.Entries.Clear(); //UAT-439
                String[] bccList = value.Split(';');
                for (int i = 0; i < bccList.Count(); i++)
                {
                    if (!String.IsNullOrEmpty(bccList[i].Trim()))
                        acbBccList.Entries.Add(new AutoCompleteBoxEntry { Text = bccList[i].Split(':')[0], Value = bccList[i].Split(':')[1] });
                }
            }
        }

        String IWriteMessageView.FileSize
        {
            set { hdnAllowedFileSize.Value = value; }

        }

        /// <summary>
        /// Get the email tolist
        /// </summary>
        String IWriteMessageView.ToListGroupIds
        {
            get
            {
                if (IsApplicant && CurrentViewContext.Action != MessagingAction.Reply && CurrentViewContext.Action != MessagingAction.ReplyAll)
                {
                    StringBuilder toIds = new StringBuilder();
                    for (int i = 0; i < acbToList.Entries.Count; i++)
                    {
                        if (acbToList.Entries[i].Value.Split(':')[1].Equals("Group"))
                            toIds.Append(Convert.ToString(acbToList.Entries[i].Value.Split(':')[0]) + ";");
                    }
                    return Convert.ToString(toIds);
                }
                return string.Empty;
            }
            set
            {
                if (IsApplicant && CurrentViewContext.Action != MessagingAction.Reply && CurrentViewContext.Action != MessagingAction.ReplyAll)
                {
                    acbToList.Entries.Clear();
                    String[] toList = value.Split(';');
                    for (int i = 0; i < toList.Count(); i++)
                    {
                        if (!String.IsNullOrEmpty(toList[i].Trim()))
                            acbToList.Entries.Add(new AutoCompleteBoxEntry { Text = toList[i].Split(':')[0], Value = toList[i].Split(':')[1] });
                    }
                }
            }
        }

        /// <summary>
        /// Get the email tolist
        /// </summary>
        String IWriteMessageView.ToListUsersForApplicant
        {
            get
            {
                if (IsApplicant && CurrentViewContext.Action != MessagingAction.Reply && CurrentViewContext.Action != MessagingAction.ReplyAll)
                {
                    StringBuilder toIds = new StringBuilder();
                    for (int i = 0; i < acbToList.Entries.Count; i++)
                    {
                        if (acbToList.Entries[i].Value.Split(':')[1].Equals("admin"))
                            toIds.Append(Convert.ToString(acbToList.Entries[i].Value.Split(':')[0]) + ";");
                    }
                    return Convert.ToString(toIds);
                }
                return string.Empty;
            }
            set
            {
                if (IsApplicant && CurrentViewContext.Action != MessagingAction.Reply && CurrentViewContext.Action != MessagingAction.ReplyAll)
                {
                    acbToList.Entries.Clear();
                    String[] toList = value.Split(';');
                    for (int i = 0; i < toList.Count(); i++)
                    {
                        if (!String.IsNullOrEmpty(toList[i].Trim()))
                            acbToList.Entries.Add(new AutoCompleteBoxEntry { Text = toList[i].Split(':')[0], Value = toList[i].Split(':')[1] });
                    }
                }
            }
        }


        /// <summary>
        /// Get the email Cclist
        /// </summary>
        String IWriteMessageView.CcListGroupIds
        {
            get
            {
                if (IsApplicant)
                {
                    StringBuilder ccIds = new StringBuilder();
                    for (int i = 0; i < acbCcList.Entries.Count; i++)
                    {
                        if (acbCcList.Entries[i].Value.Split(':').Count() > 1)
                        {
                            if (acbCcList.Entries[i].Value.Split(':')[1].Equals("Group"))
                                ccIds.Append(Convert.ToString(acbCcList.Entries[i].Value.Split(':')[0]) + ";");
                        }
                    }
                    return Convert.ToString(ccIds);
                }
                return string.Empty;
            }
            set
            {
                if (IsApplicant)
                {
                    acbCcList.Entries.Clear();
                    String[] ccList = value.Split(';');
                    for (int i = 0; i < ccList.Count(); i++)
                    {
                        if (!String.IsNullOrEmpty(ccList[i].Trim()))
                            acbCcList.Entries.Add(new AutoCompleteBoxEntry { Text = ccList[i].Split(':')[0], Value = ccList[i].Split(':')[1] });
                    }
                }
            }
        }


        /// <summary>
        /// Get the email Cclist
        /// </summary>
        String IWriteMessageView.CcListOfUserForApplicant
        {
            get
            {
                if (IsApplicant)
                {
                    StringBuilder ccIds = new StringBuilder();
                    for (int i = 0; i < acbCcList.Entries.Count; i++)
                    {
                        if (acbCcList.Entries[i].Value.Split(':').Count() > 1)
                        {
                            if (acbCcList.Entries[i].Value.Split(':')[1].Equals("admin"))
                                ccIds.Append(Convert.ToString(acbCcList.Entries[i].Value.Split(':')[0]) + ";");
                        }
                    }
                    return Convert.ToString(ccIds);
                }
                return string.Empty;
            }
            set
            {
                if (IsApplicant)
                {
                    acbCcList.Entries.Clear();
                    String[] ccList = value.Split(';');
                    for (int i = 0; i < ccList.Count(); i++)
                    {
                        if (!String.IsNullOrEmpty(ccList[i].Trim()))
                            acbCcList.Entries.Add(new AutoCompleteBoxEntry { Text = ccList[i].Split(':')[0], Value = ccList[i].Split(':')[1] });
                    }
                }
            }
        }

        /// <summary>
        /// Gets/Sets the high important
        /// </summary>
        public bool IsHighImportant
        {
            get
            {
                return Boolean.Parse(hdnMessageImportance.Value);
            }
            set
            {
                RadToolBarButton radToolBarButton = (RadToolBarButton)WclToolBar1.FindButtonByCommandName("HighImportance");
                if (radToolBarButton != null)
                    radToolBarButton.Checked = value;

                hdnMessageImportance.Value = value.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsApplicant
        {
            get
            {
                return SecurityManager.GetOrganizationUser(CurrentUserId).IsApplicant.GetValueOrDefault(false);
            }
        }

        #region UAT-3098
        public bool IsShared
        {
            get
            {
                return SecurityManager.GetOrganizationUser(CurrentUserId).IsSharedUser.GetValueOrDefault(false);
            }
        }
        #endregion

        #region UAT-4179
        Boolean IWriteMessageView.IsCopyOfMailToSender
        {
            get
            {
                return chkIsCopyOfMailToSender.Checked;
            }
        }

        public bool IsNededToShowCopyMeInMailCheckBox
        {
            get
            {
                if (!ViewState["IsNededToShowCopyMeInMailCheckBox"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsNededToShowCopyMeInMailCheckBox"]);
                return false;
            }
            set
            {
                ViewState["IsNededToShowCopyMeInMailCheckBox"] = value;
            }
        }

        #endregion

        public Boolean IsSendMessageSuccess { get; set; }
        protected override void OnInit(EventArgs e)
        {
            UpdatePanel updPanel = this.Master.FindControl("updpPopupContent") as UpdatePanel;
            if (!updPanel.IsNullOrEmpty())
            {
                updPanel.Triggers.Add(new AsyncPostBackTrigger()
                {
                    ControlID = WclToolBar1.UniqueID
                    //EventName = "Click", // this may be optional
                });
            }
            base.OnInit(e);          
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    //UAT-233 START
                    if (!Request[AppConsts.SCREEN_NAME_QUERY_STRING].IsNullOrEmpty()
                            && (Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.PORTFOLIO_SEARCH.ToLower() ||
                            Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.COMPLIANCE_SEARCH.ToLower() ||
                            Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.BACKGROUND_ORDER_SEARCH.ToLower() ||
                            Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.ADMIN_DATA_ITEM_SEARCH.ToLower() ||
                            Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.STUDENT_BUCKET_ASSIGNEMNT.ToLower() ||
                            Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.ROTATION_DETAIL_FORM.ToLower() ||
                            Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.ROTATION_STUDENT_DETAIL.ToLower() //UAT-3098
                            || Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.SCHOOL_REPRESENTATIVE_DETAILS.ToLower()//UAT-3319
                            || Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.REQUIREMENT_NONCOMPLIANT_SEARCH.ToLower() //UAT 4006
                            || Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.ROTATION_STUDENT_SEARCH.ToLower() //UAT-4013
                            )
                        )
                    {

                        //When User click's on To, CC, BCC buttons, only selected applicants will visible on grid
                        _isNeedToEnableToCcButtonsAndEmailFunctionality = true;

                        RadToolBarButton RadToolBarButton = (RadToolBarButton)WclToolBar1.FindButtonByCommandName("SaveMessage");
                        if (RadToolBarButton != null)
                        {
                            if ((!Request[AppConsts.SCREEN_NAME_QUERY_STRING].IsNullOrEmpty() && (Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.ROTATION_STUDENT_DETAIL.ToLower()
                                || Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.SCHOOL_REPRESENTATIVE_DETAILS.ToLower())) && (IsShared != null && IsShared)) //UAT-3098
                            {
                                RadToolBarButton.Visible = false;
                            }
                            else
                            {
                                RadToolBarButton.Enabled = false;
                            }
                        }

                        //UAT-4179
                        if (Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.COMPLIANCE_SEARCH.ToLower())
                        {
                            hdnIsNeedToShowUsersInBCCInsteadOfTo.Value = "true";

                            CurrentViewContext.IsNededToShowCopyMeInMailCheckBox = true;
                            SetBCCListToSendMessage();
                        }
                        else
                        {
                            if (Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.ROTATION_DETAIL_FORM.ToLower())
                            { 
                                hdnIsNeedToShowUsersInBCCInsteadOfTo.Value = "true"; //UAT-4222 
                                CurrentViewContext.IsNededToShowCopyMeInMailCheckBox = true;
                                SetBCCListToSendMessage();//UAT-4222 
                            }
                            else //UAT-4222 
                                SetToListToSendMessage();
                        }
                        hdnIsReplyMode.Value = "false";
                    }
                    //UAT-4179
                    else if (!Request[AppConsts.SCREEN_NAME_QUERY_STRING].IsNullOrEmpty()
                            && (Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.COMMUNICATION_CENTER.ToLower() && !IsApplicant))
                    {
                        CurrentViewContext.IsNededToShowCopyMeInMailCheckBox = true;
                    }
                    else
                    {
                        if (!Session["OrgUsersToList"].IsNullOrEmpty())
                            Session.Remove("OrgUsersToList");
                    }

                    //UAT-233 END
                    CurrentViewContext.Action = String.IsNullOrEmpty(Request[AppConsts.ACTION_TYPE_QUERY_STRING]) ? (MessagingAction)AppConsts.ONE : (MessagingAction)Convert.ToInt32(Request[AppConsts.ACTION_TYPE_QUERY_STRING]);

                    if (Request[AppConsts.MESSAGE_ID_QUERY_STRING].IsNullOrEmpty() || Convert.ToString(Request[AppConsts.MESSAGE_ID_QUERY_STRING]) == AppConsts.ZERO)
                        CurrentViewContext.MessageId = Guid.NewGuid();
                    else
                    {
                        CurrentViewContext.MessageId = new Guid(Request[AppConsts.MESSAGE_ID_QUERY_STRING]);
                        if (CurrentViewContext.Action.Equals(MessagingAction.Draft))
                            hdnMessageId.Value = Request[AppConsts.MESSAGE_ID_QUERY_STRING];
                    }

                    Presenter.GetTenantTypes();
                    Presenter.GetTemplates();
                    GetMessageDetails();

                    #region UAT-3463
                    if (!Session["HideCommunicationModeForIP"].IsNullOrEmpty())
                    {
                        _isNeedToEnableToCcButtonsAndEmailFunctionality = false;
                        Session.Remove("HideCommunicationModeForIP");
                        rdlCommunicationMode.Items[0].Selected = false;
                        rdlCommunicationMode.Items[1].Selected = true;
                        trAttachemnets.Visible = false;
                        trAttachFiles.Visible = false;
                        WclPane2.Height = 160;
                    }
                    #endregion

                    if (_isNeedToEnableToCcButtonsAndEmailFunctionality)
                    {
                        //WclPane2.MinHeight = 248;
                        //WclPane2.Height = 248;
                        trCommunicationSelection.Visible = true;
                    }
                    else
                    {
                        if (CurrentViewContext.Action == MessagingAction.Reply || CurrentViewContext.Action == MessagingAction.ReplyAll)
                        {
                            //WclPane2.MinHeight = 199;
                            //WclPane2.Height = 199;
                        }
                        else
                        {
                            //WclPane2.MinHeight = 225;
                            //WclPane2.Height = 225;
                        }
                        trCommunicationSelection.Visible = false;
                    }
                }
                Presenter.OnViewLoaded();
                if (!IsPostBack)
                {
                    if ((CurrentViewContext.Action.Equals(MessagingAction.Reply) || CurrentViewContext.Action.Equals(MessagingAction.ReplyAll)))
                    {
                        btnTo.Enabled = false;
                        btnCc.Enabled = false;
                        hdnIsReplyMode.Value = "true";
                        dvBccusers.Visible = false;
                    }
                    else
                    {
                        //Open Service Descriptor page in pop up
                        String openUserPopupScriptTo = string.Format("openWin2('{0}', '{1}'); return false;", acbToList.ClientID, CommunicationType);
                        String openUserPopupScriptCc = string.Format("openWin2('{0}', '{1}'); return false;", acbCcList.ClientID, CommunicationType);
                        //hyperServiceTypeDescription.Attributes.Add("onClick", openUserPopupScriptTo);
                        //hyperServiceTypeDescriptionCC.Attributes.Add("onClick", openUserPopupScriptCc); 
                        SetToListForMessage();
                        hdnIsReplyMode.Value = "false";
                    }

                    if ((!Request[AppConsts.SCREEN_NAME_QUERY_STRING].IsNullOrEmpty() && (Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.ROTATION_STUDENT_DETAIL.ToLower()
                        || Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.SCHOOL_REPRESENTATIVE_DETAILS.ToLower() || Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.REQUIREMENT_NONCOMPLIANT_SEARCH.ToLower())) && (IsShared)) //UAT-3098
                    {
                        rdlCommunicationMode.Items[0].Selected = false;
                        rdlCommunicationMode.Items[1].Selected = true;
                    }
                    #region UAT-3215
                    if (Session["rotationDetail"] != null)
                    {
                        var rotationDetails = (INTSOF.ServiceDataContracts.Modules.ClinicalRotation.ClinicalRotationDetailContract)Session["rotationDetail"];
                        editorContent.Content = Presenter.GetRotationDetails(rotationDetails);
                    }
                    #endregion
                }
                // acbToList.DataSource = Presenter.RetrieveUsers(CurrentUserId, lkpCommunicationTypeContext.MESSAGE);                
                WclToolBar1.ButtonClick += new RadToolBarEventHandler(WclToolBar1_ButtonClick);
                //comandSaveAtachments.SubmitClick += new EventHandler(comandSaveAtachments_SubmitClick);
                //ScriptManager.GetCurrent(this as Page).RegisterPostBackControl(WclToolBar1);
                //ScriptManager.GetCurrent(this as Page).RegisterPostBackControl(comandSaveAtachments);
                if (!hdnMessageId.Value.IsNullOrEmpty())
                {
                    CurrentViewContext.MessageId = new Guid(hdnMessageId.Value);
                    CurrentViewContext.Action = MessagingAction.Draft;
                }
                if (CurrentViewContext.IsApplicant)
                {
                    //lblTemplate.Visible = false;
                    cmbTemplates.Visible = false;
                    dvBccusers.Visible = false;
                    Boolean IsApplicantAllowToSendMessages = Presenter.CheckApplicantClientSettings() ? true : false;
                    if (!IsApplicantAllowToSendMessages)
                    {
                        RadToolBarButton RadToolBarButton = (RadToolBarButton)WclToolBar1.FindButtonByCommandName("Send");
                        if (RadToolBarButton != null)
                        {
                            RadToolBarButton.Enabled = false;
                        }
                    }
                }
                #region UAT-3098
                if ((!Request[AppConsts.SCREEN_NAME_QUERY_STRING].IsNullOrEmpty() && Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.ROTATION_STUDENT_DETAIL.ToLower()
                    || Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.SCHOOL_REPRESENTATIVE_DETAILS.ToLower() || Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.REQUIREMENT_NONCOMPLIANT_SEARCH.ToLower()
                    || Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.ROTATION_STUDENT_SEARCH.ToLower() //UAT-4013
                    ) && (IsShared != null && IsShared))
                {
                    //UAT-4013
                    if (Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.ROTATION_STUDENT_SEARCH.ToLower())
                    {
                        rdlCommunicationMode.SelectedValue = "2";
                    }

                    trCommunicationSelection.Visible = false;
                    trTemplates.Visible = false;
                    hdnIsSharedUser.Value = "Shareduser";
                    trAttachFiles.Visible = false;
                    trAttachemnets.Visible = false;
                    WclPane2.Height = 146;
                    RadToolBarButton radToolBarButton = (RadToolBarButton)WclToolBar1.FindButtonByCommandName("HighImportance");
                    if (radToolBarButton != null)
                        radToolBarButton.Visible = false;
                }
                #endregion

                //UAT-3319 
                if ((!Request[AppConsts.SCREEN_NAME_QUERY_STRING].IsNullOrEmpty()
                    && (Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.SCHOOL_REPRESENTATIVE_DETAILS.ToLower()
                    || Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.ROTATION_STUDENT_DETAIL.ToLower())) //UAT-3421
                    && (IsShared != null && IsShared))
                {
                    btnTo.Enabled = false;
                    btnCc.Enabled = false;
                    btnBcc.Enabled = false;
                    acbCcList.Enabled = false;
                    acbBccList.Enabled = false;
                    //UAT: 4002
                    EnabledOrDisabledCCBasedOnCondition();
                }
                //UAT-4179
                if (CurrentViewContext.IsNededToShowCopyMeInMailCheckBox)
                    dvIsCopyOfMailToSender.Visible = true;
                else
                    dvIsCopyOfMailToSender.Visible = false;
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
            cmbTemplates.Focus();
        }

        /// <summary>
        /// Bind message Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbTenantType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                /*// INITIAL PHASE cmbMessageType.ClearSelection();
                 cmbMessageType.Items.Clear();*/
                CurrentViewContext.ReceiverType = e.Text.Trim();
                Presenter.BindMessageType();
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        /// <summary>
        ///  Upload Control Event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void uploadControl_AttachmentAdded(object sender, AttachmentItemEventArgs args)
        {
            try
            {
                // UploadButtonVisiblity(true);
            }
            catch (SysXException ex)
            {
                //base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                //base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Upload Control Event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uploadControl_AttachmentRemoved(object sender, AttachmentItemEventArgs e)
        {
            //if (uploadControl.Items.Count.Equals(AppConsts.ONE))
            //{
            //    //  UploadButtonVisiblity(false);
            //}
        }

        void WclToolBar1_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            try
            {
                String commandName = (e.Item as RadToolBarButton).CommandName;

                if (commandName.ToLower() == MessageToolBarAction.SEND.GetStringValue().ToLower()
                    || commandName.ToLower() == MessageToolBarAction.SAVE.GetStringValue().ToLower())
                {
                    if (string.Compare(rdlCommunicationMode.SelectedValue, "1") == 0)
                    {
                        String documentId = String.Empty;
                        String eVaultDocumentID = String.Empty;
                        String documentName = String.Empty;

                        if (fupDocument.UploadedFiles.Count > 0)
                        {
                            UploadedDocument(fupDocument);
                        }
                        else
                        {
                            if (acbAttachedFiles.Entries != null && acbAttachedFiles.Entries.Count > 0)
                            {
                                foreach (AutoCompleteBoxEntry entry in acbAttachedFiles.Entries)
                                    CurrentViewContext.DocumentName += entry.Value + ";";
                            }
                        }

                        if (commandName.ToLower() == MessageToolBarAction.SEND.GetStringValue().ToLower())
                            Presenter.SendMessage(MessageMode.SENDMESSAGE.GetStringValue(), IsHighImportant);

                        else if (commandName.ToLower() == MessageToolBarAction.SAVE.GetStringValue().ToLower())
                            Presenter.SendMessage(MessageMode.DRAFTMESSAGE.GetStringValue(), IsHighImportant);                                                                     
                        if (CurrentViewContext.IsSendMessageSuccess)
                        {
                             ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "closeWindow();", true);                            
                        }
                        //Response.End();
                      //  System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else if (string.Compare(rdlCommunicationMode.SelectedValue, "2") == 0)
                    {
                        List<SystemCommunicationAttachment> lstSystemCommunicationAttachment = new List<SystemCommunicationAttachment>();
                        if (fupDocument.UploadedFiles.Count > 0)
                        {
                            lstSystemCommunicationAttachment = GetAttachedDocuments(fupDocument);
                        }

                        Presenter.SendEmail(lstSystemCommunicationAttachment);
                    }
                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }



        /// <summary>
        ///  button DeselectAll Click Event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeselectAll_Click(object sender, EventArgs e)
        {
            try
            {
                //uploadControl.DeleteAllAttachments();
                //UploadButtonVisiblity(false);
            }
            catch (SysXException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void comandSaveAtachments_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                if (fupDocument.UploadedFiles.Count > 0)
                {

                    UploadedDocument(fupDocument);
                    //Dictionary<string, string> attachedFiles = new Dictionary<string, string>();
                    ////CurrentViewContext.DocumentName.Contains(@"\");
                    //attachedFiles.Add(CurrentViewContext.DocumentName.Replace(@"\", @"\\"), CurrentViewContext.OriginalDocumentName);
                    //CurrentViewContext.AttachedFiles = attachedFiles;
                }
                if (!hdnMessageId.Value.IsNullOrEmpty())
                {
                    CurrentViewContext.Action = MessagingAction.Draft;
                }
                Presenter.SendMessage("D", false);
                hdnMessageId.Value = CurrentViewContext.MessageId.ToString();
                hdnIsSavedInDraft.Value = CurrentViewContext.MessageId.ToString();

            }
            catch (SysXException ex)
            {
                //LogError(ex);
                //ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                //base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// If admin tries to send message to selected user then the 
        /// ToList fo the message will automatically be populated.
        /// </summary>
        private void SetToListForMessage()
        {
            String applicantName = Request[AppConsts.APPLICANT_DESCRIPTON_TYPE_QUERY_STRING].IsNotNull() ? Request[AppConsts.APPLICANT_DESCRIPTON_TYPE_QUERY_STRING].ToString() : String.Empty;
            String applicantId = Request[AppConsts.APPLICANT_TYPE_QUERY_STRING].IsNotNull() ? Request[AppConsts.APPLICANT_TYPE_QUERY_STRING].ToString() : String.Empty;
            if (!(applicantName.Equals(String.Empty)) && !(applicantId.Equals(String.Empty)))
            {
                acbToList.Entries.Add(new AutoCompleteBoxEntry { Text = applicantName, Value = applicantId });
            }
        }

        /// <summary>
        /// If admin tries to send message to selected user then the 
        /// ToList fo the message will automatically be populated.
        /// </summary>
        /// 
        private void EnabledOrDisabledCCBasedOnCondition()
        {
            if (!Session["OrgUsersToList"].IsNullOrEmpty())
            {
                var orgUsersToList = Session["OrgUsersToList"] as List<Dictionary<Int32, String>>;
                if (orgUsersToList != null && orgUsersToList.Count > 2)
                {
                    acbCcList.Enabled = true;
                }
            }
        }
        private void SetToListToSendMessage()
        {
            //Get OrgUsersToList from sessions
            if (!Session["OrgUsersToList"].IsNullOrEmpty())
            {
                var orgUsersToList = Session["OrgUsersToList"] as List<Dictionary<Int32, String>>;
                if (!_isNeedToEnableToCcButtonsAndEmailFunctionality)
                {
                    Session.Remove("OrgUsersToList");
                }
                if (orgUsersToList != null && orgUsersToList.Count > 1)
                {
                    //Remove OrgUsersToList session after getting

                    BindEmailsBothApplicantandRep(orgUsersToList);
                }
                else
                {
                    var orgUsersToDictionary = Session["OrgUsersToList"] as Dictionary<Int32, String>;
                    orgUsersToDictionary.ForEach(x =>
                       acbToList.Entries.Add(new AutoCompleteBoxEntry { Value = Convert.ToString(x.Key), Text = x.Value }));

                }
            }
        }

        private void SetBCCListToSendMessage()
        {
            //Get OrgUsersToList from sessions
            if (!Session["OrgUsersToList"].IsNullOrEmpty())
            {
                var orgUsersToDictionary = Session["OrgUsersToList"] as Dictionary<Int32, String>;
                orgUsersToDictionary.ForEach(x =>
                                          acbBccList.Entries.Add(new AutoCompleteBoxEntry { Value = Convert.ToString(x.Key), Text = x.Value }));

            }
        }

        private void BindEmailsBothApplicantandRep(List<Dictionary<int, string>> orgUsersToList)
        {
            if (orgUsersToList.IsNotNull() && orgUsersToList.Count > 0)
            {
                string TypeOfEventTrigger = string.Empty;
                bool IsToListFill = false;
                foreach (var item in orgUsersToList)
                {
                    if (TypeOfEventTrigger == string.Empty && item.Any(x => x.Key == 0))
                    {
                        TypeOfEventTrigger = item.Select(x => x.Value).First();
                    }
                    else
                        if (IsToListFill == false)
                    {
                        item.ForEach(x =>
                        acbToList.Entries.Add(new AutoCompleteBoxEntry { Value = Convert.ToString(x.Key), Text = x.Value }));
                        IsToListFill = true;
                    }
                    else
                            if (TypeOfEventTrigger == "RepresentativeAndApplicant")
                    {
                        item.ForEach(x =>
                       acbCcList.Entries.Add(new AutoCompleteBoxEntry { Value = Convert.ToString(x.Key), Text = x.Value }));

                    }
                }
            }
        }

        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="ex"></param>
        private void LogError(String errorMessage, System.Exception ex)
        {
            _exceptionService.HandleError(errorMessage, ex);
        }

        /// <summary>
        /// Function to get message details.
        /// </summary>
        private void GetMessageDetails()
        {

            switch (CurrentViewContext.Action)
            {
                case MessagingAction.NewMail:
                    Title = "New Message";
                    break;
                case MessagingAction.Reply:
                    Presenter.GetMessage();
                    Title = "Reply";
                    break;
                case MessagingAction.ReplyAll:
                    Presenter.GetMessage();
                    Title = "Reply All";
                    break;
                case MessagingAction.Forward:
                    Presenter.GetForwardMessage();
                    Title = "Forward Message";
                    break;
                case MessagingAction.Draft:
                    Presenter.GetTemplateMessage(MessageId.ToString());
                    Title = "Drafted Message";
                    break;
            }


            /*  INITIAL PHASE if (!cmbTenantType.IsNull())
              {
                  //CurrentViewContext.ViewContract.TenantTypes
                  cmbTenantType.SelectedValue = Convert.ToString(CurrentViewContext.ReceiverTypeId);

                  //Filled Message Type on the base of selected receiver
                  if (!cmbTenantType.SelectedItem.IsNull())
                  {
                      CurrentViewContext.ReceiverType = cmbTenantType.SelectedItem.Text;
                      Presenter.BindMessageType();
                      cmbMessageType.SelectedValue = Convert.ToString(CurrentViewContext.MessageTypeId);
                  }
              }*/
        }

        public void cmbTemplates_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Presenter.GetTemplateMessage(cmbTemplates.SelectedValue);
                rdlCommunicationMode.Focus();
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        private void UploadedDocument(WclAsyncUpload fupDocument)
        {

            try
            {
                Boolean aWSUseS3 = false;
                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                Dictionary<String, String> attachedFiles = new Dictionary<String, String>();
                String filePath = WebConfigurationManager.AppSettings[MessagingFolder.MESSAGE_FILE_LOCATION];
                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

                if (tempFilePath.IsNullOrEmpty())
                {
                    LogError("Please provide path for the TemporaryFileLocation in web config.", null);
                    return;
                }
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                tempFilePath += DateTime.Now.ToString("yyyyMM") + @"\";

                if (!Directory.Exists(tempFilePath))
                {
                    Directory.CreateDirectory(tempFilePath);
                }
                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    if (filePath.IsNullOrEmpty())
                    {
                        LogError("Please provide path for the " + MessagingFolder.MESSAGE_FILE_LOCATION + " in web config", null);
                        return;
                    }
                    if (!filePath.EndsWith(@"\"))
                    {
                        filePath += @"\";
                    }
                    filePath = filePath + DateTime.Now.ToString("yyyyMM") + @"\";

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                }
                try
                {
                    List<ADBMessageDocument> messageDocument = new List<ADBMessageDocument>();
                    foreach (UploadedFile document in fupDocument.UploadedFiles)
                    {
                        ADBMessageDocument documentData = new ADBMessageDocument();
                        fileName = "MD_" + Guid.NewGuid().ToString() + Path.GetExtension(document.FileName);
                        //Save file
                        String newTempFilePath = Path.Combine(tempFilePath, fileName);
                        document.SaveAs(newTempFilePath);

                        //Check whether use AWS S3, true if need to use
                        if (aWSUseS3 == false)
                        {
                            //Move file to other location
                            String destFilePath = Path.Combine(filePath, fileName);
                            File.Copy(newTempFilePath, destFilePath);
                            documentData.DocumentName = destFilePath;
                        }
                        else
                        {
                            if (filePath.IsNullOrEmpty())
                            {
                                LogError("Please provide path for the " + MessagingFolder.MESSAGE_FILE_LOCATION + " in web config", null);
                                return;
                            }
                            if (!filePath.EndsWith(@"/"))
                            {
                                filePath += @"/";
                            }

                            //AWS code to save document to S3 location
                            AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                            String destFolder = filePath + DateTime.Now.ToString("yyyyMM") + @"/";
                            String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                            documentData.DocumentName = returnFilePath; //Path.Combine(destFolder, fileName);
                        }
                        try
                        {
                            if (!String.IsNullOrEmpty(newTempFilePath))
                                File.Delete(newTempFilePath);
                        }
                        catch (Exception) { }
                        documentData.OriginalDocumentName = document.FileName;
                        documentData.DocumentSize = document.ContentLength;
                        messageDocument.Add(documentData);
                    }
                    attachedFiles = Presenter.SaveDocumentAndGetDocumentId(messageDocument);
                    if (!attachedFiles.IsNullOrEmpty())
                    {
                        attachedFiles.ForEach(a => CurrentViewContext.DocumentName += a.Key.ToString() + ";");
                    }
                    CurrentViewContext.AttachedFiles = attachedFiles;
                }
                catch (Exception ex)
                {

                }
                //return isInvalidFile;
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }

        }

        private List<SystemCommunicationAttachment> GetAttachedDocuments(WclAsyncUpload fupDocument)
        {
            List<SystemCommunicationAttachment> lstSystemCommunicationAttachment = new List<SystemCommunicationAttachment>();

            try
            {
                Int16 docAttachmentTypeId = 0;
                List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();

                docAttachmentTypeId = !docAttachmentType.IsNullOrEmpty()
                        ? Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == DocumentAttachmentType.APPLICANT_DOCUMENT.GetStringValue()).DAT_ID)
                        : Convert.ToInt16(AppConsts.NONE);

                Boolean aWSUseS3 = false;
                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                Dictionary<String, String> attachedFiles = new Dictionary<String, String>();
                String filePath = WebConfigurationManager.AppSettings[MessagingFolder.MESSAGE_FILE_LOCATION];
                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

                if (tempFilePath.IsNullOrEmpty())
                {
                    LogError("Please provide path for the TemporaryFileLocation in web config.", null);
                    return lstSystemCommunicationAttachment;
                }
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                tempFilePath += DateTime.Now.ToString("yyyyMM") + @"\";

                if (!Directory.Exists(tempFilePath))
                {
                    Directory.CreateDirectory(tempFilePath);
                }

                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    if (filePath.IsNullOrEmpty())
                    {
                        LogError("Please provide path for the " + MessagingFolder.MESSAGE_FILE_LOCATION + " in web config", null);
                        return lstSystemCommunicationAttachment;
                    }
                    if (!filePath.EndsWith(@"\"))
                    {
                        filePath += @"\";
                    }
                    filePath = filePath + DateTime.Now.ToString("yyyyMM") + @"\";

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                }
                try
                {

                    foreach (UploadedFile document in fupDocument.UploadedFiles)
                    {
                        SystemCommunicationAttachment documentData = new SystemCommunicationAttachment();
                        documentData.SCA_OriginalDocumentID = -1;

                        fileName = "MD_" + Guid.NewGuid().ToString() + Path.GetExtension(document.FileName);
                        //Save file
                        String newTempFilePath = Path.Combine(tempFilePath, fileName);
                        document.SaveAs(newTempFilePath);

                        //Check whether use AWS S3, true if need to use
                        if (aWSUseS3 == false)
                        {
                            //Move file to other location
                            String destFilePath = Path.Combine(filePath, fileName);
                            File.Copy(newTempFilePath, destFilePath);
                            documentData.SCA_DocumentPath = destFilePath;
                        }
                        else
                        {
                            if (filePath.IsNullOrEmpty())
                            {
                                LogError("Please provide path for the " + MessagingFolder.MESSAGE_FILE_LOCATION + " in web config", null);
                                return lstSystemCommunicationAttachment;
                            }
                            if (!filePath.EndsWith(@"/"))
                            {
                                filePath += @"/";
                            }

                            //AWS code to save document to S3 location
                            AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                            String destFolder = filePath + DateTime.Now.ToString("yyyyMM") + @"/";
                            String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                            documentData.SCA_DocumentPath = returnFilePath; //Path.Combine(destFolder, fileName);
                        }
                        try
                        {
                            if (!String.IsNullOrEmpty(newTempFilePath))
                                File.Delete(newTempFilePath);
                        }
                        catch (Exception) { }
                        documentData.SCA_OriginalDocumentName = document.FileName;
                        documentData.SCA_DocumentSize = document.ContentLength;
                        documentData.SCA_DocAttachmentTypeID = docAttachmentTypeId;
                        documentData.SCA_TenantID = SecurityManager.DefaultTenantID;
                        documentData.SCA_IsDeleted = false;
                        documentData.SCA_CreatedBy = CurrentUserId;
                        documentData.SCA_CreatedOn = DateTime.Now;
                        lstSystemCommunicationAttachment.Add(documentData);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
            return lstSystemCommunicationAttachment;
        }

        [WebMethod]
        public static String AutoSaveData(String messageIdUser, String toList, String ccList, String bccList, String toListName, String ccListName, String bccListName, String subject, String content, String documentName, String originalDocumentName, String communicationType, String messageType1, String mainText, String isReplyMode)
        {
            Int32 messageType = Convert.ToInt32(messageType1);
            Guid messageId;
            MessagingAction action;
            if (messageIdUser.IsNullOrEmpty())
            {
                action = MessagingAction.NewMail;
                messageId = Guid.Empty;
            }
            else
            {
                action = MessagingAction.Draft;
                messageId = new Guid(messageIdUser);
            }
            Int32 currentUserId = SysXWebSiteUtils.SessionService.OrganizationUserId;
            Boolean isApplicant = SecurityManager.GetOrganizationUser(currentUserId).IsApplicant.GetValueOrDefault(false);
            MessagingContract viewContract = new MessagingContract();
            UserGroup userGroup = null;
            List<String> toUserEmailIds;
            List<int> toIds;
            if (isApplicant && !Convert.ToBoolean(isReplyMode))
            {
                //"9:admin;1:Group;"
                String[] list = toList.Split(';');
                String toListGroupIds = String.Empty;
                String toListUserForApplicant = String.Empty;
                foreach (String item in list)
                {
                    if (!item.Equals(String.Empty) && item.Split(':')[1].Equals("admin"))
                    {
                        toListUserForApplicant += item.Split(':')[0] + ";";
                    }
                    else if (!item.Equals(String.Empty))
                    {
                        toListGroupIds += item.Split(':')[0] + ";";
                    }
                }


                toIds = toListGroupIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                toUserEmailIds = MessageManager.GetEmailIdsByGroupIds(toIds).ToList();
                viewContract.ToUserGroupIds = toListGroupIds;

                toIds.Clear();

                toIds = toListUserForApplicant.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                toUserEmailIds = MessageManager.GetEmailId(toIds);
                viewContract.ToUserIds = toListUserForApplicant;
            }
            else
            {
                String toListIds = toList;
                toIds = toListIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                toUserEmailIds = MessageManager.GetEmailId(toIds);
                viewContract.ToUserIds = toListIds;
            }
            viewContract.toUserList = String.Join(";", toUserEmailIds.ToArray());


            List<int> ccIds;
            List<String> ccUserEmailIds;

            if (isApplicant && !Convert.ToBoolean(isReplyMode))
            {
                String[] list = toList.Split(';');
                String ccListGroupIds = String.Empty;
                String ccListUserForApplicant = String.Empty;
                foreach (String item in list)
                {
                    if (!item.Equals(String.Empty) && item.Split(':')[1].Equals("admin"))
                    {
                        ccListUserForApplicant += item.Split(':')[0] + ";";
                    }
                    else if (!item.Equals(String.Empty))
                    {
                        ccListGroupIds += item.Split(':')[0] + ";";
                    }
                }

                ccIds = ccListGroupIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                ccUserEmailIds = MessageManager.GetEmailId(ccIds).ToList();
                viewContract.CCUserGroupIds = ccListGroupIds;

                ccIds.Clear();


                ccIds = ccListUserForApplicant.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                ccUserEmailIds = MessageManager.GetEmailIdsByGroupIds(ccIds).ToList();
                viewContract.CcUserIds = ccListUserForApplicant;

            }
            else
            {
                String ccListIds = ccList;
                ccIds = ccListIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                ccUserEmailIds = MessageManager.GetEmailId(ccIds);
                viewContract.CcUserIds = ccListIds;
            }
            viewContract.CcUserList = String.Join(";", ccUserEmailIds.ToArray());

            List<int> bccIds;
            List<String> bccUserEmailIds;

            if (!isApplicant)
            {
                String bccListIds = bccList;
                bccIds = bccListIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                ////UAT-4179
                //if (Convert.ToBoolean(IsSenderNeededCopyOfMailInBCC))
                //{

                //    bccIds.Add(currentUserId);
                //}

                bccUserEmailIds = MessageManager.GetEmailId(bccIds);
                viewContract.BccUserIds = bccListIds;
            }
            viewContract.BccUserList = String.Join(";", ccUserEmailIds.ToArray());
            viewContract.CommunicationType = communicationType;

            viewContract.ToList = toListName;
            {
                userGroup = MessageManager.GetUserGroupByCurrentUserIdAndMessageType(currentUserId, messageType);
                if (!userGroup.IsNull())
                {
                    viewContract.CurrentUserId = userGroup.UserGroupID;
                    viewContract.From = userGroup.UserGroupName;
                }
                else
                {
                    viewContract.CurrentUserId = currentUserId;
                    viewContract.From = MessageManager.GetEmailId(currentUserId);
                }
            }

            //check if we can pull the Id from lkpMessageFolder entity by passing "(Int32)lkpMessageFolderContext.DRAFTS : (Int32)lkpMessageFolderContext.INBOX;"

            viewContract.CcList = ccListName;

            viewContract.MessageType = messageType;

            // Draft based folder code will be set here. Else it will be set in the database SP, based on the type of rule applicable on it.
            viewContract.FolderId = MessageManager.GetFolderIdByCode(lkpMessageFolderContext.DRAFTS.GetStringValue());

            viewContract.MessageMode = "D";
            //viewContract.IsHighImportance = isHighImportance;

            viewContract.Action = action;
            //viewContract.QueueType = Convert.ToInt32(Request["queueType"]);
            viewContract.MessageId = messageId;
            viewContract.Subject = subject.Trim();
            //viewContract.TenantTypes = View.TenantTypes;
            viewContract.Content = content;
            //viewContract.DocumentID = View.DocumentID;
            //viewContract.EVaultDocumentID = View.EVaultDocumentID;
            viewContract.DocumentName = documentName;
            viewContract.OriginalDocumentName = originalDocumentName;

            //Database Name
            viewContract.ApplicationDatabaseName = MessageManager.GetDatabaseName(ConfigurationManager.ConnectionStrings[AppConsts.APPLICATION_CONNECTION_STRING].ConnectionString);
            if (!messageIdUser.IsNullOrEmpty() || !toList.IsNullOrEmpty() || !ccList.IsNullOrEmpty()
                || !subject.IsNullOrEmpty() || !mainText.IsNullOrEmpty() || !documentName.IsNullOrEmpty() || !bccList.IsNullOrEmpty())
            {
                MessageManager.SendMessage(viewContract);
                //Todoset the hidden field value
                //ViewState["MessageId"] = viewContract.MessageId.ToString();
                return viewContract.MessageId.ToString();
            }
            return String.Empty;

        }

        protected void rdlCommunicationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadToolBarButton radToolBarButton = (RadToolBarButton)WclToolBar1.FindButtonByCommandName("HighImportance");

            if (string.Compare(rdlCommunicationMode.SelectedValue, "1") == 0)
            {
                trAttachFiles.Visible = true;
                trAttachemnets.Visible = true;
                //WclPane2.MinHeight = WclPane2.MinHeight + 53;
                //WclPane2.Height = WclPane2.MinHeight;
                //WclPane3.Height = WclPane2.MinHeight + 20;

                if (radToolBarButton != null)
                    radToolBarButton.Visible = true;

            }
            else if (string.Compare(rdlCommunicationMode.SelectedValue, "2") == 0)
            {
                trAttachFiles.Visible = false;
                trAttachemnets.Visible = false;
                //WclPane2.MinHeight = WclPane2.MinHeight - 53;
                //WclPane2.Height = WclPane2.MinHeight;
                //WclPane3.Height = WclPane2.MinHeight + 125;

                if (radToolBarButton != null)
                    radToolBarButton.Visible = false;
            }
            btnTo.Focus();
        }
    }
}

