using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using Entity.ClientEntity;
using System.Web.UI;
using INTSOF.Utils;
using System.Linq;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class CustomAttributeRowControl : BaseUserControl, ICustomAttributeRowControlView
    {
        #region   Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private CustomAttributeRowControlPresenter _presenter = new CustomAttributeRowControlPresenter();

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            LoadAttributeControl();
        }

        #endregion

        #region  Methods

        #region Public Methods

        #endregion

        #region Private Methods

        private void LoadAttributeControl()
        {
            for (int i = 0; i < CurrentViewContext.lstTypeCustomAttributes.Count; i++)
            {
                Control customAttribute = Page.LoadControl("~\\ComplianceAdministration\\UserControl\\CustomAttributeControl.ascx");
                (customAttribute as CustomAttributeControl).TypeCustomtAttribute = CurrentViewContext.lstTypeCustomAttributes[i];
                (customAttribute as CustomAttributeControl).SelectedRecordId = CurrentViewContext.SelectedRecordId;

                //UAT-4997 //UAT 4829
                if (this.NeedTocheckCustomAttributeEditableSetting && !this.IsReadOnly)
                {
                    //[ToDo:Added check for IsEditableBYApplicant]
                    (customAttribute as CustomAttributeControl).IsReadOnly = CurrentViewContext.lstTypeCustomAttributes[i].IsEditableByApplicant.HasValue? 
                                                                             !CurrentViewContext.lstTypeCustomAttributes[i].IsEditableByApplicant.Value:false;
                }
                else
                {
                    (customAttribute as CustomAttributeControl).IsReadOnly = this.IsReadOnly;
                }
                (customAttribute as CustomAttributeControl).ControlDisplayMode = this.ControlDisplayMode;

                #region UAT 1438: Enhancement to allow students to select a User Group.

                (customAttribute as CustomAttributeControl).TenantID = this.TenantID;
                (customAttribute as CustomAttributeControl).OrganizationUserId = this.OrganizationUserId;
                (customAttribute as CustomAttributeControl).ShowReadOnlyUserGroupCustomAttribute = this.ShowReadOnlyUserGroupCustomAttribute;
                (customAttribute as CustomAttributeControl).lstUserGroups = this.lstUserGroups;
                (customAttribute as CustomAttributeControl).lstUserGroupsForUser = this.lstUserGroupsForUser;
                (customAttribute as CustomAttributeControl).lstPreviousSelectedUserGroupIds = this.lstPreviousSelectedUserGroupIds; //UAT-3455
                (customAttribute as CustomAttributeControl).IsUserGroupSlctdValuesdisabled = this.IsUserGroupSlctdValuesdisabled;
                (customAttribute as CustomAttributeControl).lstUsrGrpSavedValues = this.lstUsrGrpSavedValues; //UAT-3455
                (customAttribute as CustomAttributeControl).IsApplicantProfileScreen = this.IsApplicantProfileScreen; //UAT-3455
                (customAttribute as CustomAttributeControl).IsIntegrationClientOrganisationUser = this.IsIntegrationClientOrganisationUser; //UAT-3133
                (customAttribute as CustomAttributeControl).PeopleSoftId = this.PeopleSoftId;//UAT-3133
                (customAttribute as CustomAttributeControl).NeedTocheckCustomAttributeEditableSetting = this.NeedTocheckCustomAttributeEditableSetting; //UAT 4829
                #endregion

                if (this.AttributeValues.IsNotNull() && AttributeValues.Count() > 0)
                    (customAttribute as CustomAttributeControl).AttributeValues = this.AttributeValues;


                if (!String.IsNullOrEmpty(this.ValidationGroup))
                    (customAttribute as CustomAttributeControl).ValidationGroup = this.ValidationGroup;

                pnlRow.Controls.Add(customAttribute);
            }

        }

        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public CustomAttributeRowControlPresenter Presenter
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
        /// Property to enable disable the controls
        /// </summary>
        public Boolean IsReadOnly { get; set; }

        /// <summary>
        /// Property to display the labels or actual controls
        /// </summary>
        public DisplayMode ControlDisplayMode { get; set; }

        public List<TypeCustomAttributes> lstTypeCustomAttributes
        {
            get;
            set;
        }

        public ICustomAttributeRowControlView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 SelectedRecordId
        {
            get;
            set;
        }

        /// <summary>
        /// Receive the attribute values and set to the controls, in Edit profile, in order flow
        /// </summary>
        public List<TypeCustomAttributes> AttributeValues
        {
            get;
            set;
        }

        public String ValidationGroup
        {
            get;
            set;
        }

        #region  UAT 1438: Enhancement to allow students to select a User Group.
        public Int32 TenantID
        {
            get;
            set;
        }

        public Int32 OrganizationUserId
        {
            get;
            set;
        }

        public Boolean ShowReadOnlyUserGroupCustomAttribute
        {
            get;
            set;
        }

        public IQueryable<UserGroup> lstUserGroups
        {
            get;
            set;
        }

        public IList<UserGroup> lstUserGroupsForUser
        {
            get;
            set;
        }

        public List<Int32> lstPreviousSelectedUserGroupIds //UAT-3455
        {
            get
            {
                if (!ViewState["lstPreviousSelectedUserGroupIds"].IsNullOrEmpty())
                    return (List<Int32>)ViewState["lstPreviousSelectedUserGroupIds"];
                return null;
            }
            set
            {
                ViewState["lstPreviousSelectedUserGroupIds"] = value;
            }
        }

        public Boolean IsUserGroupSlctdValuesdisabled //UAT-3455
        {
            get;
            set;
        }

        public Boolean IsApplicantProfileScreen //UAT-3455
        {
            get;
            set;
        }

        //UAT-3455
        public IList<UserGroup> lstUsrGrpSavedValues
        {
            get;
            set;
        }

        #region UAT-3133
        public Boolean IsIntegrationClientOrganisationUser
        {
            get;
            set;
        }
        public Int32 PeopleSoftId
        {
            get;
            set;
        }
        #endregion

        //UAT-4997 //UAT 4829
        public Boolean NeedTocheckCustomAttributeEditableSetting
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Private Properties

        #endregion

        #endregion

    }
}

