using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderServiceLinePriceInfo : BaseUserControl, IBkgOrderServiceLinePriceView
    {
        #region Variables

        private BkgOrderServiceLinePricePresenter _presenter = new BkgOrderServiceLinePricePresenter();

        #endregion

        #region Properties

        public BkgOrderServiceLinePricePresenter Presenter
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

        public IBkgOrderServiceLinePriceView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IBkgOrderServiceLinePriceView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
            }
        }

        Int32 IBkgOrderServiceLinePriceView.OrderID
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
            }
        }

        //UAT-3481
        public Boolean IsRedirectedFromOrderQueueDetails
        {
            get
            {
                if (ViewState["IsRedirectedFromOrderQueueDetails"].IsNotNull())
                {
                    return (Boolean)(ViewState["IsRedirectedFromOrderQueueDetails"]);
                }
                return false;
            }
            set
            {
                ViewState["IsRedirectedFromOrderQueueDetails"] = value;
            }
        }



        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Price Info";
                base.OnInit(e);
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

        #endregion

        #region Grid Events

        protected void grdOrderServiceLinePriceInfo_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            OrderServiceLineItemPriceInfo orderServiceLineItemPriceInfo = Presenter.GetBackroundOrderServiceLinePriceByOrderID();
            if (orderServiceLineItemPriceInfo.BOSLPrice.IsNotNull())
            {
                grdOrderServiceLinePriceInfo.DataSource = orderServiceLineItemPriceInfo.BOSLPrice;
            }
            else
            {
                grdOrderServiceLinePriceInfo.DataSource = String.Empty;
            }
            //UAT-3481
            if (CurrentViewContext.IsRedirectedFromOrderQueueDetails && !orderServiceLineItemPriceInfo.IsNullOrEmpty() && !orderServiceLineItemPriceInfo.BkgOrderPkg.IsNullOrEmpty())
            {
                orderServiceLineItemPriceInfo.BkgOrderPkg.ForEach(s => s.BkgPackageName = s.BkgPackageLabel.IsNullOrEmpty() ? s.BkgPackageName : s.BkgPackageLabel);
            }
            rptrBackgroundPackage.DataSource = orderServiceLineItemPriceInfo.BkgOrderPkg;
            rptrBackgroundPackage.DataBind();

            if (!String.IsNullOrEmpty(orderServiceLineItemPriceInfo.CompliancePackageName))
            {
                divOtherCharges.Visible = true;
                lblCompliancePackageName.Text = orderServiceLineItemPriceInfo.CompliancePackageName.HtmlEncode();
                if (orderServiceLineItemPriceInfo.CompliancePkgTotalAmount.IsNotNull() && orderServiceLineItemPriceInfo.CompliancePkgTotalAmount != 0)
                {
                    dvRushOrder.Visible = true;
                    lblRushOrderPrice.Text = String.Format("{0:c}", orderServiceLineItemPriceInfo.CompliancePkgTotalAmount);
                }
                lblCompliancePkgAmount.Text = String.Format("{0:c}", orderServiceLineItemPriceInfo.CompliancePkgAmount);
            }
        }

        #endregion

        #endregion

    }
}