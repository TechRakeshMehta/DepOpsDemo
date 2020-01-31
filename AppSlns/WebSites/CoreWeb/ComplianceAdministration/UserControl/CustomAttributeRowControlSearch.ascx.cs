using System;
using Microsoft.Practices.ObjectBuilder;
using System.Web.UI;
using System.Collections.Generic;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
	public partial class CustomAttributeRowControlSearch : BaseUserControl, ICustomAttributeRowControlSearchView
	{

        #region   Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private CustomAttributeRowControlSearchPresenter _presenter=new CustomAttributeRowControlSearchPresenter();


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
                Control customAttribute = Page.LoadControl("~\\ComplianceAdministration\\UserControl\\CustomAttributeControlSearch.ascx");
                (customAttribute as CustomAttributeControlSearch).TypeCustomtAttribute = CurrentViewContext.lstTypeCustomAttributes[i];
                (customAttribute as CustomAttributeControlSearch).previousValues = CurrentViewContext.previousValues;
                pnlRow.Controls.Add(customAttribute);
            }
        }

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        
        public CustomAttributeRowControlSearchPresenter Presenter
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




        public List<TypeCustomAttributesSearch> lstTypeCustomAttributes
        {
            get;
            set;
        }

        public ICustomAttributeRowControlSearchView CurrentViewContext
        {
            get { return this; }
        }

        public String previousValues
        {
            get;
            set;
        }

        /// <summary>
        /// Receive the attribute values and set to the controls, in Edit profile, in order flow
        /// </summary>
        public List<TypeCustomAttributesSearch> AttributeValues
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

    }
}

