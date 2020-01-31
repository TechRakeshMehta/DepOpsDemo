using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils;
using CoreWeb.Shell;


namespace CoreWeb.Shell.Views
{
    public partial class ClientDropDown : BaseUserControl, IClientDropDownView
    {
        private ClientDropDownPresenter _presenter=new ClientDropDownPresenter();

        Int32 tenantId = 0;

        String _dataTextField = "TenantName";
        String _dataValueField = "TenantID";

        #region Methods

        #region Public Methods

        public void Refresh()
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "Refresh();", true);
        }

        #endregion


        #region Private Methods

        private void BindControls()
        {
            ddlTenantName.DataTextField = DataTextField;
            ddlTenantName.DataValueField = DataValueField;
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();

            if (Presenter.IsDefaultTenant)
            {
                //divTenant.Visible = true;
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantId = 0;
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
                ddlTenantName.SelectedValue = Convert.ToString(CurrentViewContext.TenantId);
                Refresh();

            }

            //BindAdminProgramStudy();
        }

        #endregion

        #endregion

        #region Properties

        #region Public Poperties

        
        public ClientDropDownPresenter Presenter
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

        public IClientDropDownView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Entity.ClientEntity.Tenant> lstTenant
        {
            get;
            set;
        }

        public int CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantIdSearch"]);
            }
            set
            {
                ViewState["TenantIdSearch"] = value.ToString();
            }
        }

        public Boolean IsDefaultTenant
        {
            get { return Convert.ToBoolean(Presenter.IsDefaultTenant); }
        }

        public Int32 TenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;
            }
            set { tenantId = value; }
        }



        #region DropDown Properties

        public  WclComboBox SubmitButton { get { return ddlTenantName; } }

        public event RadComboBoxSelectedIndexChangedEventHandler SelectedIndexChange;

        public String DataTextField
        {
            get { return _dataTextField; }
            set { _dataTextField = value; }
        }

        public String DataValueField
        {
            get { return _dataValueField; }
            set { _dataValueField = value; }
        }

        public String SelectedValue
        {
            get { return ddlTenantName.SelectedValue; }
            set { ddlTenantName.SelectedValue = value; }
        }

        public Int32 SelectedIndex
        {
            get { return ddlTenantName.SelectedIndex; }
            set { ddlTenantName.SelectedIndex = value; }
        }

        #endregion

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                Presenter.GetTenants();
                BindControls();
            }
            ddlTenantName.SelectedIndexChanged +=new  RadComboBoxSelectedIndexChangedEventHandler(ddlTenantName_SelectedIndexChanged);
            Presenter.OnViewLoaded();
        }

        #endregion

        #region Drop Down Events


        void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (SelectedIndexChange != null) { SelectedIndexChange(sender, e); }
        }

        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        #endregion  
        #endregion 
    }
}

