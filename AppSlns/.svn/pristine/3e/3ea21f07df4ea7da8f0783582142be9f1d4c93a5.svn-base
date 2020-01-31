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
    public partial class CustomAttributeDisplayRowControl : BaseUserControl, ICustomAttributeDispalyRowControlView
    {
        #region   Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private CustomAttributeDisplayRowControlPresenter _presenter=new CustomAttributeDisplayRowControlPresenter();

        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public CustomAttributeDisplayRowControlPresenter Presenter
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

        public List<CustomAttribteContract> lstTypeCustomAttributes
        {
            get;
            set;
        }

        public ICustomAttributeDispalyRowControlView CurrentViewContext
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

        #endregion

        #region Private Methods

        private void LoadAttributeControl()
        {
            for (int i = 0; i < CurrentViewContext.lstTypeCustomAttributes.Count; i++)
            {
                Control customAttribute = Page.LoadControl("~\\ClinicalRotation\\UserControl\\CustomAttributeDisplayControl.ascx");
                (customAttribute as CustomAttributeDisplayControl).TypeCustomtAttribute = CurrentViewContext.lstTypeCustomAttributes[i];
                (customAttribute as CustomAttributeDisplayControl).SelectedRecordId = CurrentViewContext.SelectedRecordId;
                (customAttribute as CustomAttributeDisplayControl).IsReadOnly = this.IsReadOnly;
                (customAttribute as CustomAttributeDisplayControl).ControlDisplayMode = this.ControlDisplayMode;

                if (this.AttributeValues.IsNotNull() && AttributeValues.Count() > 0)
                    (customAttribute as CustomAttributeDisplayControl).AttributeValues = this.AttributeValues;


                if (!String.IsNullOrEmpty(this.ValidationGroup))
                    (customAttribute as CustomAttributeDisplayControl).ValidationGroup = this.ValidationGroup;

                pnlRow.Controls.Add(customAttribute);
            }
        }

        #endregion

        #endregion

        

    }
}

