using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Collections.Generic;
using System.Linq;
using INTSOF.UI.Contract.BkgOperations;
using INTERSOFT.WEB.UI.WebControls;
using Business.RepoManagers;
using System.Globalization;
using INTSOF.UI.Contract.Globalization;
using System.Xml;
using Telerik.Web.UI;



namespace CoreWeb.ComplianceOperations.Views
{
    public partial class OrderLineItems : BaseUserControl, IOrderLineItemsView
    {
        private OrderLineItemsPresenter _presenter = new OrderLineItemsPresenter();

        private ApplicantOrderCart _applicantOrderCart;


        private ApplicantOrderCart GetApplicantOrderCart()
        {
            if (_applicantOrderCart.IsNull())
            {
                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            }
            return _applicantOrderCart;
        }

        public Dictionary<string, DeptProgramPackageSubscription> SelectedPackageDetails
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        public IOrderLineItemsView CurrentViewContext
        {
            get { return this; }
        }
        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }


        public List<OrderLineItem> lstOrderLineItems
        {
            get;
            set;
        }
        public Boolean IfInvoiceOnlyPymntOptn
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IfInvoiceOnlyPymnOptn"])))
                    return (Boolean)ViewState["IfInvoiceOnlyPymnOptn"];
                return false;
            }
            set
            {
                ViewState["IfInvoiceOnlyPymnOptn"] = value;
            }
        }

        public Boolean IsLineItemFromCart
        {
            get
            {
                if (this.ID == "OLI_OrderConfirmation" || this.ID == "OLI_PackageDetails")
                {
                    return true;
                }
                else
                { return false; }
            }
        }

        public Boolean IsLocationServiceTenant
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IsLocationServiceTenant"])))
                    return (Boolean)ViewState["IsLocationServiceTenant"];
                return false;
            }
            set
            {
                ViewState["IsLocationServiceTenant"] = value;
            }
        }

        #region Admin Entry Portal
        public Boolean IsAdminEntryTenant
        {
            get
            {
                if (!ViewState["IsAdminEntryTenant"].IsNullOrEmpty())
                    return (Boolean)ViewState["IsAdminEntryTenant"];
                return false;
            }
            set
            {
                ViewState["IsAdminEntryTenant"] = value;
            }
        }
        #endregion
        #region Language Translation
        public String LanguageCode
        {
            get
            {
                LanguageContract langContract = LanguageTranslateUtils.GetCurrentLanguageFromSession();
                if (!langContract.IsNullOrEmpty())
                {
                    return langContract.LanguageCode;
                }
                return Languages.ENGLISH.GetStringValue();
            }
        }

        public Int32 OrderId
        {
            get
            {
                if (!ViewState["OrderId"].IsNull())
                {
                    return (Int32)ViewState["OrderId"];
                }
                return 0;
            }
            set
            {
                ViewState["OrderId"] = value;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
         {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            if (!IsPostBack)
            {

                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    if (IsLineItemFromCart)
                    {
                        _applicantOrderCart = GetApplicantOrderCart();
                        divBkgSvcBreakdwnFees.Visible = true;
                        List<BackgroundPackagesContract> _lstPackages = _applicantOrderCart.lstApplicantOrder[0].lstPackages;
                        if (_lstPackages.Count == 1)
                        {
                            foreach (BackgroundPackagesContract _item in _lstPackages)
                            {
                                if (_item.ServiceCode == BkgServiceType.SIMPLE.GetStringValue())
                                {
                                    divBkgSvcBreakdwnFees.Visible = false;
                                }
                            }
                        }
                        if (!_applicantOrderCart.lstOrderLineItems.IsNullOrEmpty() && _applicantOrderCart.lstOrderLineItems.Count > 0)
                        {
                            CurrentViewContext.lstOrderLineItems = _applicantOrderCart.lstOrderLineItems;
                        }
                        else
                        {
                            //To fetch service code.
                            //XmlDocument xmlDoc = new XmlDocument();

                            //string BkgOrderServiceDetailsxml = Presenter.GetBkgOrderServiceDetails(_applicantOrderCart.OrderId);                           
                            //if (!BkgOrderServiceDetailsxml.IsNullOrEmpty())
                            //{
                            //    xmlDoc.LoadXml(BkgOrderServiceDetailsxml);
                            //    XmlNodeList elemlist = xmlDoc.GetElementsByTagName("ServiceType");                                
                               
                            //        for(int i=0;i< elemlist.Count;i++ )
                            //        {
                            //            _lstPackages[i].ServiceCode = elemlist[i].InnerText;
                            //        }                            
                            //}
                            Presenter.GetOrderLineItems(_applicantOrderCart);
                        }
                        if (divBkgSvcBreakdwnFees.Visible)
                        {
                            Repeater rptBackgroundPackages = this.Parent.FindControl("rptBackgroundPackages") as Repeater;
                            rptBackgroundPackages.Visible = false;
                            //Presenter.GetOrderLineItems(_applicantOrderCart);
                            BindPackageDetails();
                        }
                        if (this.ID == "OLI_OrderConfirmation")
                        {
                            divOLTHeader.Visible = false;
                        }
                    }
                    else
                    {
                        divBkgSvcBreakdwnFees.Visible = true;
                        Presenter.GetSavedOrderLineItems(OrderId);
                        BindPackageDetails();
                    }
                }
            }
        }

        private void BindPackageDetails()
        {
            //if (this.ID == "OLI_OrderConfirmation" || this.ID== "OLI_PackageDetails")
            //{
            //    foreach (var item in CurrentViewContext.lstOrderLineItems)
            //    {
            //        if (item.ServiceCode == null)
            //            item.Price = null;
            //    }

            //}

            grdOrderServiceLinePriceInfo.DataSource = CurrentViewContext.lstOrderLineItems;
            grdOrderServiceLinePriceInfo.DataBind();
        }


        protected void grdOrderServiceLinePriceInfo_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                GridDataItem dataItem = (GridDataItem)e.Item;               
                if ((Convert.ToString((dataItem)["ServiceCode"].Text) == null  || Convert.ToString((dataItem)["ServiceCode"].Text)=="&nbsp;") && (Convert.ToString((dataItem)["Price"].Text) != string.Empty) && (Convert.ToString((dataItem)["Quantity"].Text)) == "&nbsp;")
                    (dataItem)["Price"].Text = String.Empty;            
            }
        }



        private void GetPricingData()
        {
            if (_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
                return;

        }




        public OrderLineItemsPresenter Presenter
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



    }
}

