using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using System.Web.UI.HtmlControls;

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderResidentialHistories : BaseUserControl, IBkgOrderResidentialHistoriesView
    {
        #region Variables

        #region Private Variables

        private BkgOrderResidentialHistoriesPresenter _presenter = new BkgOrderResidentialHistoriesPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        List<PreviousAddressContract> IBkgOrderResidentialHistoriesView.lstPreviousAddress { get; set; }

        IBkgOrderResidentialHistoriesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IBkgOrderResidentialHistoriesView.MasterOrderID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID);
                return 0;
            }
        }

        Int32 IBkgOrderResidentialHistoriesView.SelectedTenantID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID);
                return 0;
            }
        }

        public Int32 HeaderTextId { get; set; }

        public Int32 CurrentAddressCount { get; set; }

        #endregion

        #region Public Properties

        public BkgOrderResidentialHistoriesPresenter Presenter
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

        public Boolean ShowCriminalAttribute_MotherName
        {
            get;
            set;
        }

        public Boolean ShowCriminalAttribute_License
        {
            get;
            set;
        }

        public Boolean ShowCriminalAttribute_Identification
        {
            get;
            set;
        }

        List<Int32> IBkgOrderResidentialHistoriesView.PackageIDs
        {
            get
            {
                if (ViewState["PackageIDs"].IsNullOrEmpty())
                {
                    return new List<Int32>();
                }
                return ViewState["PackageIDs"] as List<Int32>;
            }
            set
            {
                ViewState["PackageIDs"] = value;
            }
        }

        #region Anirudh Order Review Page

        public Boolean OrderReview { get; set; }
        public Boolean OrderConfirmation { get; set; }
        public List<PreviousAddressContract> lstPrevAddresses { get; set; }

        #endregion

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (OrderReview)
            {
                HeaderTextId = 1;
                rptrResidentialHistory.DataSource = lstPrevAddresses.Where(x => !x.isCurrent && !x.isDeleted).OrderBy(x => x.ResHistorySeqOrdID).ToList();
                rptrResidentialHistory.DataBind();
            }
            else if (OrderConfirmation)
            {
                HeaderTextId = 1;
                rptrOrderConfirmation.DataSource = lstPrevAddresses.Where(x => !x.isCurrent && !x.isDeleted).OrderBy(x => x.ResHistorySeqOrdID).ToList();
                rptrOrderConfirmation.DataBind();
            }
            else
            {
                HeaderTextId = 0;
                Presenter.GetBackGroundPackagesForOrderId();
                Presenter.GetResidentialHistoryByOrderId();
                if (!Presenter.IsResidentialHistoryRequired() && !CurrentViewContext.lstPreviousAddress.IsNullOrEmpty())
                {
                    CurrentViewContext.lstPreviousAddress = CurrentViewContext.lstPreviousAddress.Where(x => x.isCurrent).Select(x => x).ToList();
                }
                Presenter.CheckInternationCriminalSearchAttributes();

                rptrResidentialHistory.DataSource = CurrentViewContext.lstPreviousAddress.OrderByDescending(cond => cond.isCurrent).ThenByDescending(resDate => resDate.ResidenceStartDate);
                rptrResidentialHistory.DataBind();
            }
        }

        protected void rptrResidentialHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblResHistoryIndex = e.Item.FindControl("lblResHistoryIndex") as Label;
                System.Web.UI.HtmlControls.HtmlControl dvMoveinDate = e.Item.FindControl("dvMoveinDate") as System.Web.UI.HtmlControls.HtmlControl;

                switch (HeaderTextId)
                {
                    case 0:
                        lblResHistoryIndex.Text = "Current Address";
                        HeaderTextId = 1;
                        if (dvMoveinDate.IsNotNull())
                            dvMoveinDate.Visible = false;
                        break;
                    case 1:
                        lblResHistoryIndex.Text = "Residence Previous";
                        HeaderTextId = 2;
                        if (dvMoveinDate.IsNotNull())
                            dvMoveinDate.Visible = true;
                        break;

                    default:
                        lblResHistoryIndex.Text = "Residence - " + (HeaderTextId).ToString() + " before current";
                        HeaderTextId++;
                        if (dvMoveinDate.IsNotNull())
                            dvMoveinDate.Visible = true;
                        break;
                }

                var dataItem = ((PreviousAddressContract)((e.Item).DataItem));
                if (dataItem.IsNotNull())
                {
                    Label lblResidentFrom = e.Item.FindControl("lblResidentFrom") as Label;
                    HtmlGenericControl divInternationalCriminalSearchAttributes = e.Item.FindControl("divInternationalCriminalSearchAttributes") as HtmlGenericControl;
                    if (!dataItem.ResidenceStartDate.IsNullOrEmpty())
                    {
                        lblResidentFrom.Text = (Convert.ToDateTime(dataItem.ResidenceStartDate)).ToShortDateString();
                    }
                    Label lblResidentTill = e.Item.FindControl("lblResidentTill") as Label;
                    if (!dataItem.ResidenceEndDate.IsNullOrEmpty())
                    {
                        lblResidentTill.Text = (Convert.ToDateTime(dataItem.ResidenceEndDate)).ToShortDateString();
                    }
                    if (dataItem.ZipCodeID == 0)
                    {
                        Label stateText = e.Item.FindControl("lblStateText") as Label;
                        stateText.Text = "State/Province";
                        Label zipText = e.Item.FindControl("lblZipText") as Label;
                        zipText.Text = "Postal Code";
                        if (dataItem.CountyName.IsNullOrEmpty())
                        {
                            Control divsbutton = e.Item.FindControl("dvCounty") as Control;
                            divsbutton.Visible = false;
                        }
                    }
                    if (dataItem.CountryId != AppConsts.COUNTRY_USA_ID && dataItem.CountryId != 0)
                    {
                        if (ShowCriminalAttribute_Identification)
                        {
                            divInternationalCriminalSearchAttributes.Visible = true;
                            (e.Item.FindControl("divIdentificationNumber") as HtmlGenericControl).Visible = true;
                            (e.Item.FindControl("lblIdentificationNumber") as Label).Text = dataItem.IdentificationNumber;
                        }
                        if (ShowCriminalAttribute_License)
                        {
                            divInternationalCriminalSearchAttributes.Visible = true;
                            (e.Item.FindControl("divCriminalLicenseNumber") as HtmlGenericControl).Visible = true;
                            (e.Item.FindControl("lblCriminalLicenseNumber") as Label).Text = dataItem.LicenseNumber;
                        }
                        if (ShowCriminalAttribute_MotherName)
                        {
                            divInternationalCriminalSearchAttributes.Visible = true;
                            (e.Item.FindControl("divMothersName") as HtmlGenericControl).Visible = true;
                            (e.Item.FindControl("lblMotherName") as Label).Text = dataItem.MotherName;
                        }
                    }
                    else
                    {
                        divInternationalCriminalSearchAttributes.Visible = false;
                    }
                    if (!dataItem.isCurrent)
                    {
                        Control dvAddressLine = e.Item.FindControl("dvAddressLine") as Control;
                        if (dvAddressLine.IsNotNull())
                            dvAddressLine.Visible = false;
                    }
                }
            }
        }

        #endregion
    }
}