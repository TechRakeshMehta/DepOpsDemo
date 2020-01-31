using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using System.Collections.Generic;
using System.Linq;
using Entity;
using Telerik.Web.UI;
using System.Text;
using CoreWeb.Shell;
using System.Web.Services;
using Business.RepoManagers;

namespace CoreWeb.Messaging.Views
{
    public partial class ManageTemplate : System.Web.UI.Page, IManageTemplateView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private ManageTemplatePresenter _presenter=new ManageTemplatePresenter();
        #endregion

        #endregion

        #region Properties

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>

        
        public ManageTemplatePresenter Presenter
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
        #endregion


        #region Private Properties

        /// <summary>
        /// Get new messageId
        /// </summary>
        Guid IManageTemplateView.MessageId
        {
            get
            {
                return new Guid(Request["messageID"].ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string TemplateName
        {
            get
            {
                return txtTemplateName.Text;
            }
            set
            {
                txtTemplateName.Text = value;
            }
        }

        /// <summary>
        /// Get the email subject.
        /// </summary>
        String IManageTemplateView.Subject
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
        String IManageTemplateView.Content
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
        /// Get queuetype
        /// </summary>
        Int32 IManageTemplateView.QueueType
        {
            get
            {
                return Convert.ToInt32(Request["queueType"]);
            }
        }
        /// <summary>
        /// Get current userId
        /// </summary>
        public Int32 CurrentOrganizationUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Guid CurrentUserId
        {
            get
            {
                return new Guid(SysXWebSiteUtils.SessionService.UserId);
            }
        }



        /// <summary>
        /// Get current context
        /// </summary>
        private IManageTemplateView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IManageTemplateView.ToList
        {
            get
            {
                return Convert.ToString(acbToList.Entries);
            }
            set
            { }
        }

        String IManageTemplateView.CCList
        {
            get
            {
                return Convert.ToString(acbCcList.Entries);
            }
            set
            { }
        }

        String IManageTemplateView.BCCList
        {
            get
            {
                return Convert.ToString(acbBccList.Entries);
            }
            set
            { }
        }

        /// <summary>
        /// Get current userId
        /// </summary>
        Dictionary<Int32, String> IManageTemplateView.TenantTypes
        {
            get;
            set;
        }

        /// <summary>
        /// Get and set MessageType
        /// </summary>
        public Int32 CommunicationTypeId
        {
            get
            {
                return Convert.ToInt32(ViewState["CommunicationId"]);
            }
            set
            {
                ViewState["CommunicationId"] = value.ToString();
            }
        }

        public String CommunicationTypeCode
        {
            get
            {
                return "CT01";
            }
        }

        /// <summary>
        /// Get the email tolist
        /// </summary>
        String IManageTemplateView.IsUserGroup
        {
            get
            {
                return hdnIsUserGroupforCompany.Value;
            }
            set
            {
                hdnIsUserGroupforCompany.Value = value;
            }
        }

        /// <summary>
        /// Get the email tolist
        /// </summary>
        String IManageTemplateView.CcIsUserGroup
        {
            get
            {
                return hdnCcIsUserGroupforCompany.Value;
            }
            set
            {
                hdnCcIsUserGroupforCompany.Value = value;
            }
        }

        /// <summary>
        /// Get the email tolist
        /// </summary>
        String IManageTemplateView.ToListIds
        {
            get
            {
                if (!IsApplicant)
                {

                    StringBuilder toIds = new StringBuilder();
                    for (int i = 0; i < acbToList.Entries.Count; i++)
                    {
                        toIds.Append(Convert.ToString(acbToList.Entries[i].Value) + ";");
                    }
                    return Convert.ToString(toIds);
                }
                return string.Empty;
            }
            set
            {
                if (!IsApplicant)
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
        String IManageTemplateView.CcListIds
        {
            get
            {
                if (!IsApplicant)
                {
                    StringBuilder ccIds = new StringBuilder();
                    for (int i = 0; i < acbCcList.Entries.Count; i++)
                    {
                        ccIds.Append(Convert.ToString(acbCcList.Entries[i].Value) + ";");
                    }
                    return Convert.ToString(ccIds);
                }
                return string.Empty;
            }
            set
            {
                if (!IsApplicant)
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
        /// Get the email tolist
        /// </summary>
        String IManageTemplateView.ToListGroupIds
        {
            get
            {
                if (IsApplicant)
                {
                    StringBuilder toIds = new StringBuilder();
                    for (int i = 0; i < acbToList.Entries.Count; i++)
                    {
                        toIds.Append(Convert.ToString(acbToList.Entries[i].Value) + ";");
                    }
                    return Convert.ToString(toIds);
                }
                return string.Empty;
            }
            set
            {
                if (IsApplicant)
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
        String IManageTemplateView.CcListGroupIds
        {
            get
            {
                if (IsApplicant)
                {
                    StringBuilder ccIds = new StringBuilder();
                    for (int i = 0; i < acbCcList.Entries.Count; i++)
                    {
                        ccIds.Append(Convert.ToString(acbCcList.Entries[i].Value) + ";");
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

        public bool IsApplicant
        {
            get
            {
                return SecurityManager.GetOrganizationUser(CurrentOrganizationUserId).IsApplicant.GetValueOrDefault(false);
            }
        }



        /// <summary>
        /// Get or set the list of communication type
        /// </summary>
        public List<lkpCommunicationType> CommunicationTypeList
        {
            get
            {
                return new List<lkpCommunicationType>();
            }
            set
            {
                CommunicationTypeList = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>        
        public IQueryable<ADBMessage> CompanyTemplates
        {
            set
            {
                cmbTemplates.DataSource = value;
                cmbTemplates.DataBind();
                cmbTemplates.Items.Insert(0, new RadComboBoxItem("New.."));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ADBMessageId
        {
            get
            {
                return cmbTemplates.SelectedValue;
            }
        }

        /// <summary>
        /// Get the email Cclist
        /// </summary>
        String IManageTemplateView.BccListIds
        {
            get
            {
                if (!IsApplicant)
                {
                    StringBuilder bccIds = new StringBuilder();
                    for (int i = 0; i < acbBccList.Entries.Count; i++)
                    {
                        bccIds.Append(Convert.ToString(acbBccList.Entries[i].Value) + ";");
                    }
                    return Convert.ToString(bccIds);
                }
                return string.Empty;
            }
            set
            {
                if (!IsApplicant)
                {
                    acbBccList.Entries.Clear();
                    String[] bccList = value.Split(';');
                    for (int i = 0; i < bccList.Count(); i++)
                    {
                        if (!String.IsNullOrEmpty(bccList[i].Trim()))
                            acbBccList.Entries.Add(new AutoCompleteBoxEntry { Text = bccList[i].Split(':')[0], Value = bccList[i].Split(':')[1] });
                    }
                }
            }
        }

        #endregion
        #endregion

        #region Events
        #region Page Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Presenter.OnViewInitialized();
                var communicationTypeId = Presenter.GetCommuncationTypeByCode(CurrentViewContext.CommunicationTypeCode);
                CurrentViewContext.CommunicationTypeId = communicationTypeId.IsNullOrEmpty() ? 0 : communicationTypeId.CommunicationTypeID;
            }
            Presenter.OnViewLoaded();
            if (cmbTemplates.SelectedValue == "")
            {
                btnDelete.Enabled = false;
            }
            else
            {
                btnDelete.Enabled = true;
            }
        }
        #endregion

        #region Control Related Events

        protected void cmbTemplates_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Presenter.GetTemplateMessage(cmbTemplates.SelectedValue);
        }

        //protected void cmbCommunicationTypes_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    acbToList.Entries.Clear();
        //    acbCcList.Entries.Clear();
        //}

        #endregion

        #region Grid Related Events
        #endregion

        #endregion

        #region Methods
        #region Public Methods
        public void Delete()
        {
            Presenter.Delete();
        }

        public void BindTemplates()
        {
            btnDelete.Enabled = false;
            Presenter.BindTemplates();
        }

        [WebMethod]
        public static string GetUniqueTemplateName(string templateName, string adbMessageId)
        {
            Guid messageId = string.IsNullOrEmpty(adbMessageId) ? new Guid() : new Guid(adbMessageId);
            return MessageManager.GetUniqueTemplateName(messageId, SysXWebSiteUtils.SessionService.OrganizationUserId, templateName);
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Invoked when message is drafted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveMessage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbTemplates.SelectedValue))
            {
                if (Presenter.SendMessage("T"))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CloseManageTemplatePopup(true,true);", true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CloseManageTemplatePopup(true,false);", true);
                }
            }
            else
            {
                if (Presenter.UpdateMessage("T"))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CloseManageTemplatePopup(false,true);", true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CloseManageTemplatePopup(false,false);", true);
                }
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbTemplates.SelectedIndex > AppConsts.NONE)
            {
                Delete();
                BindTemplates();
            }
        }

    }
        #endregion
        #endregion

}


