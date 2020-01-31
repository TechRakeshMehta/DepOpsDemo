using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using Entity.ClientEntity;
using System.Web.UI;
using INTSOF.Utils;
using System.Linq;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class NewCustomAttributeRowControl : BaseUserControl, ICustomAttributeRowControlView
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
                Control customAttribute = Page.LoadControl("~\\ComplianceAdministration\\UserControl\\NewCustomAttributeControl.ascx");
                (customAttribute as NewCustomAttributeControl).TypeCustomtAttribute = CurrentViewContext.lstTypeCustomAttributes[i];
                (customAttribute as NewCustomAttributeControl).SelectedRecordId = CurrentViewContext.SelectedRecordId;
                (customAttribute as NewCustomAttributeControl).IsReadOnly = this.IsReadOnly;
                (customAttribute as NewCustomAttributeControl).ControlDisplayMode = this.ControlDisplayMode;
                
                #region UAT 1438: Enhancement to allow students to select a User Group.

                (customAttribute as NewCustomAttributeControl).TenantID = this.TenantID;
                (customAttribute as NewCustomAttributeControl).OrganizationUserId = this.OrganizationUserId;
                (customAttribute as NewCustomAttributeControl).ShowReadOnlyUserGroupCustomAttribute = this.ShowReadOnlyUserGroupCustomAttribute;
                (customAttribute as NewCustomAttributeControl).lstUserGroups = this.lstUserGroups;
                (customAttribute as NewCustomAttributeControl).lstUserGroupsForUser = this.lstUserGroupsForUser;
                #endregion

                if (this.AttributeValues.IsNotNull() && AttributeValues.Count() > 0)
                    (customAttribute as NewCustomAttributeControl).AttributeValues = this.AttributeValues;


                if (!String.IsNullOrEmpty(this.ValidationGroup))
                    (customAttribute as NewCustomAttributeControl).ValidationGroup = this.ValidationGroup;

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
        #endregion

        #endregion

        #region Private Properties

        #endregion

        #endregion

    }
}

