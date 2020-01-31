using System;
using Microsoft.Practices.ObjectBuilder;
using Telerik.Web.UI;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using System.Collections.Generic;
using Entity;
using System.Linq;
using INTSOF.UI.Contract.Messaging;
using System.Web.UI;

namespace CoreWeb.Messaging.Views
{
    public partial class TransferRulesMaintenanceForm : BaseUserControl, ITransferRulesMaintenanceFormView
    {
        private object _dataItem = null;
        MessagingRulesContract _viewContract;
        private TransferRulesMaintenanceFormPresenter _presenter=new TransferRulesMaintenanceFormPresenter();
        private List<lkpMessageFolder> _folderList;
        private List<Tenant> _institutions;
        //Commented by Sachin Singh for flexible hierarchy.
        //private List<AdminProgramStudy> _institutionPrograms;
        private List<OrganizationLocation> _institutionLocations;
        private Dictionary<Int32, String> _messageFromUsers;


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

        public Int32 TenantID
        {
            get
            {
                if (!ViewState["TenantId"].IsNullOrEmpty())
                {
                    return (Int32)ViewState["TenantId"];
                }
                else
                    return AppConsts.ONE;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }
        /// <summary>
        /// DataItemViewState to get DataItem
        /// </summary>
        private Object DataItemViewState
        {
            set
            {
                if (!value.IsNull() && (!(DataItem is GridInsertionObject)))
                {
                    ViewState["DataItemViewState"] = value;
                }
            }
        }

        /// <summary>
        /// CurrentViewContext for Accesing Properties Value
        /// </summary>
        ITransferRulesMaintenanceFormView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Tenant> Institutions
        {
            set
            {
                _institutions = value;
                _institutions.Insert(AppConsts.NONE, new Tenant { TenantID = AppConsts.NONE, TenantName = AppConsts.COMBOBOX_ITEM_ANY });
            }
            get
            {
                return _institutions;
            }
        }

        public List<OrganizationLocation> InstitutionLocations
        {
            set
            {
                _institutionLocations = value;
                _institutionLocations.Insert(AppConsts.NONE, new OrganizationLocation { OrganizationLocationID = AppConsts.NONE, LocationName = AppConsts.COMBOBOX_ITEM_ANY });
            }
            get
            {
                return _institutionLocations;
            }
        }

        //Commented by Sachin Singh for flexible hierarchy.
        //public List<AdminProgramStudy> InstitutionPrograms
        //{
        //    set
        //    {
        //        _institutionPrograms = value;
        //        _institutionPrograms.Insert(AppConsts.NONE, new AdminProgramStudy { AdminProgramStudyID = AppConsts.NONE, ProgramStudy = AppConsts.COMBOBOX_ITEM_ANY });
        //    }
        //    get
        //    {
        //        return _institutionPrograms;
        //    }
        //}

        public Int32? InstitutionId
        {
            get
            {
                return Convert.ToInt32(cmbInstitutions.SelectedValue) == AppConsts.NONE ? (Int32?)null : Convert.ToInt32(cmbInstitutions.SelectedValue);
            }
            set
            {
                cmbInstitutions.SelectedValue = Convert.ToString(value);
            }
        }

        public Int32? LocationId
        {
            get
            {
                return Convert.ToInt32(cmbLocations.SelectedValue) == AppConsts.NONE ? (Int32?)null : Convert.ToInt32(cmbLocations.SelectedValue);
            }
            set
            {
                cmbLocations.SelectedValue = Convert.ToString(value);
            }
        }

        public Int32? ProgramId
        {
            get
            {
                //Commented by Sachin Singh for flexible hierarchy.
                //return Convert.ToInt32(cmbPrograms.SelectedValue) == AppConsts.NONE ? (Int32?)null : Convert.ToInt32(cmbPrograms.SelectedValue);
                return null;
            }
            set
            {
                //Commented by Sachin Singh for flexible hierarchy.
                //cmbPrograms.SelectedValue = Convert.ToString(value);
            }
        }
        public List<lkpMessageFolder> FolderList
        {
            get
            {
                return _folderList;
            }
            set
            {
                _folderList = value;
            }
            //set
            //{
            //    cmbFolders.DataSource = value;
            //    cmbFolders.DataBind();
            //}
        }
        public int CurrentUserID
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public int UserGroupID
        {
            get
            {
                return AppConsts.NONE;
            }
        }

        public MessagingRulesContract ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new MessagingRulesContract();
                }

                return _viewContract;
            }
            set
            {
                _viewContract = value;
            }
        }

        /// <summary>
        /// Rule ID
        /// </summary>
        private Int32 RuleID
        {
            get
            {
                return ViewState[AppConsts.RULE_ID__VIEW_STATE].IsNull() ? -1 : (Int32)ViewState[AppConsts.RULE_ID__VIEW_STATE];
            }
            set
            {
                ViewState[AppConsts.RULE_ID__VIEW_STATE] = CurrentViewContext.ViewContract.RuleId = value;
            }
        }


        public Dictionary<Int32, String> MessageFromUsers
        {
            get
            {
                _messageFromUsers = new Dictionary<Int32, String>();
                for (int i = 0; i < autxSelectusers.Entries.Count; i++)
                {
                    _messageFromUsers.Add(Convert.ToInt32(autxSelectusers.Entries[i].Value), "");
                }
                return _messageFromUsers;
            }
            set
            {
                _messageFromUsers = new Dictionary<Int32, String>();
                _messageFromUsers = value;
                foreach (var user in _messageFromUsers)
                {
                    autxSelectusers.Entries.Add(new AutoCompleteBoxEntry { Text = user.Value, Value = Convert.ToString(user.Key) });
                }
            }
        }


        /// <summary>
        /// On init method for setting page title.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                InitializeComponent();

                base.OnInit(e);

                if (!this.IsPostBack)
                {
                    this._presenter.OnViewInitialized();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            DataItemViewState = DataItem;

            if (!DataItem.IsNull() && (!(DataItem is GridInsertionObject)))
            {
                RuleID = (Int32)DataBinder.Eval(DataItem, AppConsts.RULE_ID__VIEW_STATE);
            }

            Presenter.OnViewLoaded();

            //string openUserPopupScript = string.Format("openUsersPopUpWindow('{0}'); return false;", autxSelectusers.ClientID);
            //btnSelectUsers.Attributes.Add("onClick", openUserPopupScript);

        }

        private void InitializeComponent()
        {
            this.DataBinding -= new EventHandler(TransferRulesMaintenanceForm_DataBinding);
            this.DataBinding += new EventHandler(TransferRulesMaintenanceForm_DataBinding);
        }
        private void BindComboBox()
        {
            FolderList.Insert(AppConsts.NONE, new lkpMessageFolder { Code = AppConsts.ZERO, Name = AppConsts.COMBOBOX_ITEM_SELECT });
            cmbFolders.DataSource = FolderList.Select(con => new
                {
                    //MessageFolderCode = String.Format("{0}#{1}", con.MessageFolderID, con.MessageFolderCode),
                    MessageFolderCode = con.Code,
                    MessageFolderName = con.Name,
                    MessageFolderID = con.MessageFolderID,
                }
                );


            //cmbFolders.Items.Insert(AppConsts.NONE, new RadComboBoxItem { Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.ZERO });
            cmbFolders.DataTextField = "MessageFolderName";
            cmbFolders.DataValueField = "MessageFolderID";
            cmbFolders.DataBind();
        }
        void TransferRulesMaintenanceForm_DataBinding(object sender, EventArgs e)
        {
            Presenter.GetFolders();
            BindComboBox();
            Presenter.BindInstitutions();
            //Presenter.BindInstitutionLocations();
            // Presenter.BindInstitutionPrograms();

            BindCombo(cmbInstitutions, _institutions);

            cmbInstitutions_SelectedIndexChanged(cmbInstitutions, new RadComboBoxSelectedIndexChangedEventArgs(String.Empty, String.Empty, cmbInstitutions.SelectedValue, String.Empty));


            if (CurrentViewContext.ViewContract.TransactionalObject.IsNull() && RuleID > -1)
            {
                BindEditFormData();
            }

        }

        protected void cmbInstitutions_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //Presenter.BindInstitutions();
            Presenter.BindInstitutionLocations();
            //Guid? addressHandleId = CurrentViewContext.Institutions.WhereSelect(x => x.TenantID == Convert.ToInt32(cmbInstitutions.SelectedValue)).FirstOrNew().AddressHandleID;


            //if (addressHandleId.HasValue)
            BindCombo(cmbLocations, CurrentViewContext.InstitutionLocations);
            //else
            //    BindCombo(cmbLocations, CurrentViewContext.InstitutionLocations);

            cmbLocation_SelectedIndexChanged(cmbLocations, new RadComboBoxSelectedIndexChangedEventArgs(String.Empty, String.Empty, cmbLocations.SelectedValue, String.Empty));
        }
        protected void cmbLocation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Presenter.BindInstitutionPrograms();
            //if (Convert.ToInt32(cmbInstitutions.SelectedValue) > AppConsts.NONE && Convert.ToInt32(cmbLocations.SelectedValue) > AppConsts.NONE)
            //Commented by Sachin Singh for flexible hierarchy.
            //BindCombo(cmbPrograms, CurrentViewContext.InstitutionPrograms);
            //else if (Convert.ToInt32(cmbInstitutions.SelectedValue) > AppConsts.NONE)
            //    BindCombo(cmbPrograms, CurrentViewContext.InstitutionPrograms.WhereSelect(x => (x.InstitutionId == Convert.ToInt32(cmbInstitutions.SelectedValue)) || (x.ProgramId == AppConsts.NONE)));
            //else if (Convert.ToInt32(cmbLocations.SelectedValue) > AppConsts.NONE)
            //    BindCombo(cmbPrograms, CurrentViewContext.InstitutionPrograms.WhereSelect(x => (x.LocationId == Convert.ToInt32(cmbLocations.SelectedValue)) || (x.ProgramId == AppConsts.NONE)));
            //else
            //    BindCombo(cmbPrograms, CurrentViewContext.InstitutionPrograms);
        }

        protected void btnSelectUsers_Click(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "openUsersPopUpWindow('" + autxSelectusers.ClientID + "');", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.UserID = base.CurrentUserId;
                CurrentViewContext.ViewContract.FolderId = Convert.ToInt32(cmbFolders.SelectedValue);
                CurrentViewContext.ViewContract.RuleDescription = "Move Messages from Institution : " + (cmbInstitutions.SelectedValue == AppConsts.ZERO ? AppConsts.ANY : cmbInstitutions.SelectedItem.Text) +
                    ", Location : " + (cmbLocations.SelectedValue == AppConsts.ZERO ? AppConsts.ANY : cmbLocations.SelectedItem.Text) +
                    //Commented by Sachin Singh for flexible hierarchy.
                     //", Program : " + (cmbPrograms.SelectedValue == AppConsts.ZERO ? AppConsts.ANY : cmbPrograms.SelectedItem.Text) +
                     ", To Folder : " + (cmbFolders.SelectedItem.Text)+
                     " and From User : " + autxSelectusers.Entries;
                //CurrentViewContext.ViewContract.InstitutionID = Convert.ToInt32(cmbInstitutions.SelectedValue);
                //CurrentViewContext.ViewContract.LocationID = Convert.ToInt32(cmbLocations.SelectedValue);
                //CurrentViewContext.ViewContract.ProgramID = Convert.ToInt32(cmbPrograms.SelectedValue);

                CurrentViewContext.ViewContract.InstitutionID = CurrentViewContext.InstitutionId == (Int32?)AppConsts.NONE ? (Int32?)null : CurrentViewContext.InstitutionId;
                CurrentViewContext.ViewContract.LocationID = CurrentViewContext.LocationId == (Int32?)AppConsts.NONE ? (Int32?)null : CurrentViewContext.LocationId;
                CurrentViewContext.ViewContract.ProgramID = CurrentViewContext.ProgramId == (Int32?)AppConsts.NONE ? (Int32?)null : CurrentViewContext.ProgramId;
                CurrentViewContext.ViewContract.MessageFromUsers = CurrentViewContext.MessageFromUsers;

                Presenter.SaveMessageRules();
                ViewState[AppConsts.RULE_ID__VIEW_STATE] = null;
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

        
        public TransferRulesMaintenanceFormPresenter Presenter
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

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:


        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
        }

        private void BindEditFormData()
        {
            MessageRuleLocation ucData = (DataItem as MessageRule).MessageRuleLocations.WhereSelect(x => x.MessageRuleID == CurrentViewContext.ViewContract.RuleId).FirstOrNew();
            List<MessageRuleUserLocation> lstMessageRuleUserLocation = ucData.MessageRuleUserLocations.ToList();
            Dictionary<Int32, String> dicUsers = new Dictionary<Int32, String>();

            foreach (var user in lstMessageRuleUserLocation)
            {
                dicUsers.Add(user.UserID, user.OrganizationUser.FirstName);
            }

            CurrentViewContext.MessageFromUsers = dicUsers;

            CurrentViewContext.InstitutionId = Convert.ToInt32(ucData.InstitutionID);
            cmbInstitutions_SelectedIndexChanged(cmbInstitutions, new RadComboBoxSelectedIndexChangedEventArgs(String.Empty, String.Empty, cmbInstitutions.SelectedValue, String.Empty));
            //BindCombo(cmbLocations,CurrentViewContext.ViewContract

            //cmbLocations.SelectedValue = Convert.ToString(ucData.LocationID);
            CurrentViewContext.LocationId = Convert.ToInt32(ucData.LocationID);
            cmbLocation_SelectedIndexChanged(cmbLocations, new RadComboBoxSelectedIndexChangedEventArgs(String.Empty, String.Empty, cmbLocations.SelectedValue, String.Empty));

            //cmbPrograms.SelectedValue = Convert.ToString(ucData.ProgramID);
            CurrentViewContext.ProgramId = Convert.ToInt32(ucData.ProgramID);
            cmbFolders.SelectedValue = Convert.ToString(ucData.MessageRule.MessageFolderID);
        }
        protected void cmbFolders_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            String msgCode = (e.Item.DataItem as dynamic).MessageFolderCode;

            if (msgCode.Equals(lkpMessageFolderContext.INBOX.GetStringValue()) || msgCode.Equals(lkpMessageFolderContext.SENTITEMS.GetStringValue()) || msgCode.Equals(lkpMessageFolderContext.DELETEDITEMS.GetStringValue()) || msgCode.Equals(lkpMessageFolderContext.DRAFTS.GetStringValue()) || msgCode.Equals(lkpMessageFolderContext.FOLLOWUP.GetStringValue()) || msgCode.Equals(lkpMessageFolderContext.PERSONALFOLDERS.GetStringValue()))
            {
                e.Item.Remove();
            }
        }


       
    }
}

