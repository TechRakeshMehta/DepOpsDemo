using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class OrderSubscriptionHistory : BaseUserControl, IOrderSubscriptionHistoryView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variable

        private OrderSubscriptionHistoryPresenter _presenter = new OrderSubscriptionHistoryPresenter();
        private Int32 _tenantid;

        #endregion

        #endregion

        #region Properties

        public OrderSubscriptionHistoryPresenter Presenter
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

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }

        public String ControlUseType
        {
            get
            {
                if (ViewState["ControlUseType"] != null)
                    return (Convert.ToString(ViewState["ControlUseType"]));
                return String.Empty;
            }
            set
            {
                ViewState["ControlUseType"] = value;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
        }

        //to check whether applicant has complio buisness channel or not.
        public Boolean IsComplioBuisnessChannelTypeAvlbl
        {
            get;
            set;
        }

        public Boolean IsLocationServiceTenant
        {
            get
            {
                if (!ViewState["IsLocationServiceTenant"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsLocationServiceTenant"]);
                return false;
            }
            set
            {
                ViewState["IsLocationServiceTenant"] = value;
            }
        }

        public Boolean HavingPackageOtherthanScreening
        {
            get
            {
                if (!ViewState["HavingPackageOtherthanScreening"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["HavingPackageOtherthanScreening"]);
                return false;
            }
            set
            {
                ViewState["HavingPackageOtherthanScreening"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ucOrderHistory.Visiblity = Visiblity;
                ucOrderHistory.ControlUseType = ControlUseType;
                ucOrderHistory.HavingPackageOtherthanScreening = HavingPackageOtherthanScreening;
                ucSubscriptionhistory.Visiblity = Visiblity;
                ucSubscriptionhistory.ControlUseType = ControlUseType;
                if (!Visiblity) return;// below code is required only if this control is visible.

                Presenter.GetBuisnessChannelTypeByTenantId(TenantId);
                if (!IsComplioBuisnessChannelTypeAvlbl)// hide subscriptions grid for non compliance institution applicants
                {
                    ucSubscriptionhistory.Visible = false;
                    ucSubscriptionhistory.Visiblity = false;
                }

                if (!(this.hdnPostbacksource.Text == "OSH") && this.ControlUseType == AppConsts.DASHBOARD)
                {
                    ucOrderHistory.IsFalsePostBack = true;
                    ucSubscriptionhistory.IsFalsePostBack = true;
                }
                //CBI||CABS
                Presenter.IsLocationServiceTenant(TenantId);
                if (IsLocationServiceTenant)
                {
                    ucSubscriptionhistory.Visible = false;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
    }
}