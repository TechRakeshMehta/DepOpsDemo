using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;

using System.Configuration;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using Entity.ClientEntity;
using System.Linq;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManagePackageSubscription : BaseUserControl, IManagePackageSubscriptionView
    {
        #region variables
        private ManagePackageSubscriptionPresenter _presenter=new ManagePackageSubscriptionPresenter();
        private String _viewType;
        #endregion

        #region Properties

        
        public ManagePackageSubscriptionPresenter Presenter
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


        public List<CompliancePackage> ClientCompliancePackages
        {
            get;
            set;

        }

        public int loggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }

        }

        public int tenantId
        {
            get;
            set;

        }

        public int packageId
        {
            get;
            set;

        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManagePackageSubscriptionView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #endregion

        #region PageEvents
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled
        /// to fire the DI engine.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                //base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_TENANTS);
                //lblManageTenant.Text = base.Title;
                base.Title = "Subscriptions";
                base.SetPageTitle("Package Subscriptions");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }

        #endregion
        protected void grdSubscription_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetClientCompliancePackages();
            if (CurrentViewContext.ClientCompliancePackages.Any(x => x.CompliancePackageID != 0))
            {
                grdSubscription.DataSource = CurrentViewContext.ClientCompliancePackages;
            }
        }

        protected void grdSubscription_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();

                    if (e.Item.DataItem is CompliancePackage)
                    {
                        CompliancePackage package = (CompliancePackage)e.Item.DataItem;
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString( package.TenantID) },
                                                                    { "PackageId", Convert.ToString( package.CompliancePackageID) },
                                                                    { "Child", ChildControls.SubscriptionDetail}
                                                                    
                                                                 };

                        HtmlAnchor ancServiceFootprint = (HtmlAnchor)e.Item.FindControl("ancManagePackageData");

                        ancServiceFootprint.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                        queryString.Clear();
                    }
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

