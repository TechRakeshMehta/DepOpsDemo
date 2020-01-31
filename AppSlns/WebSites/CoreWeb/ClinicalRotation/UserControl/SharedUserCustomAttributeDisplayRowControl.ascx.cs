using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Web.UI;
using INTSOF.Utils;
using System.Linq;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;


namespace CoreWeb.ClinicalRotation.Views
{
    public partial class SharedUserCustomAttributeDisplayRowControl : BaseUserControl, ISharedUserCustomAttributeDispalyRowControlView
    {
        #region   Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SharedUserCustomAttributeDisplayRowControlPresenter _presenter = new SharedUserCustomAttributeDisplayRowControlPresenter();

        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public SharedUserCustomAttributeDisplayRowControlPresenter Presenter
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

        public Boolean DoNotShowDefaultValues { get; set; }

        /// <summary>
        /// Property to display the labels or actual controls
        /// </summary>
        public DisplayMode ControlDisplayMode { get; set; }

        public List<CustomAttribteContract> lstTypeCustomAttributes
        {
            get;
            set;
        }

        public ISharedUserCustomAttributeDispalyRowControlView CurrentViewContext
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
        public List<CustomAttribteContract> AttributeValues
        {
            get;
            set;
        }

        public String ValidationGroup
        {
            get;
            set;
        }
        //UAT-1778
        public String previousValues
        {
            get;
            set;
        }

        public Boolean IsSearchTypeControl
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

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

        /// <summary>
        /// UAT-1778: To Reset Custom Attributes Hidden fields
        /// </summary>
        public void ResetCustomAttributes()
        {
            foreach (var control in pnlRow.Controls)
            {
                if (control.GetType().FullName.ToLower().Contains("sharedusercustomattributedisplaycontrol"))
                {
                    ((SharedUserCustomAttributeDisplayControl)control).ResetCustomAttribute();
                }
            }
        }

        #endregion

        #region Private Methods

        private void LoadAttributeControl()
        {
            for (int i = 0; i < CurrentViewContext.lstTypeCustomAttributes.Count; i++)
            {
                Control customAttribute = Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeDisplayControl.ascx");
                (customAttribute as SharedUserCustomAttributeDisplayControl).TypeCustomtAttribute = CurrentViewContext.lstTypeCustomAttributes[i];
                (customAttribute as SharedUserCustomAttributeDisplayControl).SelectedRecordId = CurrentViewContext.SelectedRecordId;
                (customAttribute as SharedUserCustomAttributeDisplayControl).IsReadOnly = this.IsReadOnly;
                (customAttribute as SharedUserCustomAttributeDisplayControl).DoNotShowDefaultValues = this.DoNotShowDefaultValues;
                (customAttribute as SharedUserCustomAttributeDisplayControl).ControlDisplayMode = this.ControlDisplayMode;

                if (this.AttributeValues.IsNotNull() && AttributeValues.Count() > 0)
                    (customAttribute as SharedUserCustomAttributeDisplayControl).AttributeValues = this.AttributeValues;


                if (!String.IsNullOrEmpty(this.ValidationGroup))
                    (customAttribute as SharedUserCustomAttributeDisplayControl).ValidationGroup = this.ValidationGroup;

                //
                (customAttribute as SharedUserCustomAttributeDisplayControl).previousValues = this.previousValues;
                (customAttribute as SharedUserCustomAttributeDisplayControl).IsSearchTypeControl = this.IsSearchTypeControl;
                pnlRow.Controls.Add(customAttribute);
            }
        }

        #endregion

        #endregion



    }
}

