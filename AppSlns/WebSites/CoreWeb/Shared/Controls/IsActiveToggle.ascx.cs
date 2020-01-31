using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils;
using CoreWeb.Shell;
using System.Configuration;

namespace CoreWeb.Shell.Views
{
    public partial class IsActiveToggle : BaseUserControl, IIsActiveToggle
    {
        #region Private Variables
        IsActiveTogglePresenter _presenter = new IsActiveTogglePresenter();

        #endregion

        #region Public Variables
        #endregion

        #region Public Properties
        public IsActiveTogglePresenter Presenter
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

        public IIsActiveToggle CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Boolean Checked
        {
            get
            {
                return IsActiveToggleYes.Checked;
            }
            set
            {
                if (value.IsNotNull())
                {
                    IsActiveToggleYes.Checked = value;
                    IsActiveToggleNo.Checked = !value;
                }
            }
        }

        public String IsActiveToggleYesLabel
        {
            set
            {
                if (!value.IsNullOrEmpty())
                    IsActiveToggleYes.Text = value;
            }
        }

        public String IsActiveToggleNoLabel
        {
            set
            {
                if (!value.IsNullOrEmpty())
                    IsActiveToggleNo.Text = value;
            }
        }

        public Boolean IsActiveEnable
        {
            get
            {
                return IsActiveToggleYes.Enabled;
            }
            set
            {
                IsActiveToggleYes.Enabled = value;
                IsActiveToggleNo.Enabled = value;
            }
        }

        public Boolean IsAutoPostBack
        {
            get
            {
                return IsActiveToggleYes.AutoPostBack;
            }
            set
            {
                IsActiveToggleYes.AutoPostBack = value;
                IsActiveToggleNo.AutoPostBack = value;
            }
        }

        public String IsActiveYesClientID
        {
            get
            {
                return IsActiveToggleYes.ClientID;
            }
        }

        public String OnClientCheckedChanged
        {
            get;
            set;
        }

        #endregion

        #region Public Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }

            ////UAT 234 WB: Change "Is Active" from 1 checkbox to 2 radio buttons -- Code to revert the UAT234
            //IsActiveToggleYes.ToggleType = ButtonToggleType.CheckBox;
            //IsActiveToggleNo.Visible = false;

            //RadButtonToggleState isActiveYes = new RadButtonToggleState();
            //isActiveYes.Text = "Yes";
            //RadButtonToggleState isActiveNo = new RadButtonToggleState();
            //isActiveNo.Text = "No";

            //IsActiveToggleYes.ToggleStates.Add(isActiveYes);
            //IsActiveToggleYes.ToggleStates.Add(isActiveNo);

            //UAT-906: If assignment properties are active at category and item level then no asterisk sign appears for mandatory fields. Also, admin is able to save the assignment properties with Null values. 
            IsActiveToggleYes.CheckedChanged += IsActiveToggleYes_CheckedChanged;
            if (!OnClientCheckedChanged.IsNullOrEmpty())
            {
                IsActiveToggleYes.OnClientCheckedChanged = OnClientCheckedChanged;
                IsActiveToggleNo.OnClientCheckedChanged = OnClientCheckedChanged;
            }

            Presenter.OnViewLoaded();
        }


        #endregion

        void IsActiveToggleYes_CheckedChanged(object sender, EventArgs e)
        {
            if (onCheckChanged != null) { onCheckChanged(sender, e); }
        }

        #region RadioButton Properties
        public event EventHandler onCheckChanged;
        #endregion

    }
}