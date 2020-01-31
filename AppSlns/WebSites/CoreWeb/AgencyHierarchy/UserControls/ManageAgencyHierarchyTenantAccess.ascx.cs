using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class ManageAgencyHierarchyTenantAccess : BaseUserControl, IManageAgencyHierarchyTenantAccessView
    {
        #region Handlers
        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;

        public delegate void ShowCtrHandler();
        public event ShowCtrHandler eventShowCtr;

        #endregion

        #region Variables
        ManageAgencyHierarchyTenantAccessPresenter _presenter = new ManageAgencyHierarchyTenantAccessPresenter();
        Int32 _tenantId = AppConsts.NONE;
        #endregion

        #region Properties

        #region Public Proerties

        public ManageAgencyHierarchyTenantAccessPresenter Presenter
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

        public IManageAgencyHierarchyTenantAccessView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region Private Properties

        Int32 IManageAgencyHierarchyTenantAccessView.TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        Boolean IManageAgencyHierarchyTenantAccessView.IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 IManageAgencyHierarchyTenantAccessView.CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        List<TenantDetailContract> IManageAgencyHierarchyTenantAccessView.lstTenant
        {
            set 
            {
                cmbTenant.DataSource = value;
                cmbTenant.DataBind();
            }
        }

        public Int32 NodeId
        {
            get
            {
                if (ViewState["NodeId"].IsNotNull())
                    return Convert.ToInt32(ViewState["NodeId"]);
                return 0;
            }
            set
            {
                ViewState["NodeId"] = Convert.ToString(value);
            }
        }
        List<Int32> IManageAgencyHierarchyTenantAccessView.lstSelectedTenantIds
        {
            get
            {
                return cmbTenant.CheckedItems.Select(sel => Convert.ToInt32(sel.Value)).ToList();
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    cmbTenant.Items.Where(cond=>value.Contains(Convert.ToInt32(cond.Value))).ForEach(x =>
                    {
                        x.Checked = true;
                    });
                }
            }
        }

        List<Int32> IManageAgencyHierarchyTenantAccessView.lstSelectedTenantPrevsIds
        {
            get
            {
                if (ViewState["lstSelectedTenantPrevsIds"].IsNotNull())
                    return ViewState["lstSelectedTenantPrevsIds"] as List<Int32>;
                return new List<Int32>();
            }
            set
            {
                ViewState["lstSelectedTenantPrevsIds"] = value;
            }
        }

        #endregion

        #endregion

        #region Event

        #region Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Presenter.GetAllTenant();
                    Presenter.GetAgencyHierarchyTenantAccessDetails();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }
        #endregion

        #region Button Event

        protected void fsucCmdBar_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                if (IsChangeInTenantList(CurrentViewContext.lstSelectedTenantPrevsIds, CurrentViewContext.lstSelectedTenantIds))
                {
                    if (Presenter.SaveUpdateAgencyHierarchyTenantAccessMapping())
                    {
                        eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Institution mapped successfully.");
                    }
                    else
                    {
                        eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "some error has occur, please try after some time.");
                    }
                    Presenter.GetAgencyHierarchyTenantAccessDetails();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void fsucCmdBar_CancelClick(object sender, EventArgs e)
        {
            try
            {
                ResetControls();
                Presenter.GetAgencyHierarchyTenantAccessDetails();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        #endregion

        #endregion        

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        private Boolean IsChangeInTenantList(List<Int32> oldData, List<Int32> newData)
        {
            var extraIds = newData.Except(oldData).Union(oldData.Except(newData)).ToList();

            if (extraIds.IsNullOrEmpty())
                return false;
            return true;
        }

        private void ResetControls()
        {
            cmbTenant.ClearCheckedItems();
        }

        #endregion

        #endregion
    }
}