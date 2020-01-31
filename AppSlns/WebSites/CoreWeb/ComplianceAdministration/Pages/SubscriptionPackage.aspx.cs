#region Namespaces

#region System Defined Namespaces

using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Collections;

#endregion

#region User Defined Namespaces

using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;


#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class SubscriptionPackage : BaseWebPage, ISubscriptionPackageView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SubscriptionPackagePresenter _presenter = new SubscriptionPackagePresenter();
        private String _viewType;
        Int32 tenantId = 0;

        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public SubscriptionPackagePresenter Presenter
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

        public ISubscriptionPackageView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Entity.ClientEntity.SubscriptionOption> ListSubscriptionOption
        {
            get;
            set;
        }

        public List<Entity.ClientEntity.lkpPriceModel> ListPriceModel
        {
            get;
            set;
        }

        public Int32 Priority
        {
            get
            {
                if (String.IsNullOrEmpty(txtPriority.Text.Trim()))
                    return 0;
                return Convert.ToInt32(txtPriority.Text.Trim());
            }
            set
            {
                txtPriority.Text = value.ToString();
            }
        }

        public Int32 TenantId
        {
            get { return (Int32)(ViewState["TenantId"]); }
            set { ViewState["TenantId"] = value; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public List<Int32> SelectedSubscriptionOptions
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < chkSubscriptionOption.Items.Count; i++)
                {
                    if (chkSubscriptionOption.Items[i].Selected)
                    {
                        selectedIds.Add(Convert.ToInt32(chkSubscriptionOption.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < chkSubscriptionOption.Items.Count; i++)
                {
                    chkSubscriptionOption.Items[i].Selected = value.Contains(Convert.ToInt32(chkSubscriptionOption.Items[i].Value));
                }
            }
        }

        Int32 ISubscriptionPackageView.PackageId
        {
            get
            {
                return Convert.ToInt32(ViewState["PackageId"]);
            }
            set
            {
                ViewState["PackageId"] = value;
            }
        }

        Int32 ISubscriptionPackageView.DeptProgramPackageID
        {
            get
            {
                return Convert.ToInt32(ViewState["DeptProgramPackageID"]);
            }
            set
            {
                ViewState["DeptProgramPackageID"] = value;
            }
        }

        public Int32 SelectedPriceModel
        {
            get
            {
                if (String.IsNullOrEmpty(ddlPriceModel.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlPriceModel.SelectedValue);
            }
            set
            {
                ddlPriceModel.SelectedValue = value.ToString();
            }
        }

        public Int32 SavedPriceModelId
        {
            get
            {
                if (String.IsNullOrEmpty(hdnSavedPriceModelId.Value))
                    return 0;
                return Convert.ToInt32(hdnSavedPriceModelId.Value);
            }
            set
            {
                hdnSavedPriceModelId.Value = value.ToString();
            }
        }

        public Boolean IsAutoRenewInvoiceOrder
        {
            get
            {
                return chkAutoRenewInvoiceOrder.Checked;
            }
            set
            {
                chkAutoRenewInvoiceOrder.Checked = value;
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public String PermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["PermissionCode"]);
            }
            set
            {
                ViewState["PermissionCode"] = value;
            }
        }

        /// <summary>
        /// List of the selected PaymentOptions at the Package Level
        /// </summary>
        public List<Int32> lstSelectedOptions
        {
            get;
            set;
        }

        #region UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.

        Int32 ISubscriptionPackageView.PaymentApprovalID
        {
            get;
            set;
        }

        #endregion

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Page_Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    //CurrentViewContext.PackageId = Convert.ToInt32(Request.QueryString["Id"]);
                    //CurrentViewContext.DeptProgramPackageID = Convert.ToInt32(Request.QueryString["MappingID"]);
                    CurrentViewContext.DeptProgramPackageID = Convert.ToInt32(Request.QueryString["Id"]);
                    CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
                    CurrentViewContext.PermissionCode = Request.QueryString["PermissionCode"];
                    BindControls();

                    BindPkgPaymentOptions();
                }
                Presenter.OnViewLoaded();

                //To check if admin logged in or not
                if (!Presenter.IsAdminLoggedIn())
                {
                    DisableControls();
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

        /// <summary>
        /// ddlPriceModel_DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPriceModel_DataBound(object sender, EventArgs e)
        {
            //ddlPriceModel.Items.Insert(0, new DropDownListItem("--SELECT--", String.Empty));
            ddlPriceModel.Items.Insert(0, new RadComboBoxItem { Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.ZERO });
        }

        /// <summary>
        /// CmdBarSave_Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSave_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean availability = Convert.ToBoolean(chkAvailableForOrder.Checked);
                CurrentViewContext.lstSelectedOptions = ucPkgPaymentOptions.GetSelectedPaymentOptions();
                //UAT-2073
                CurrentViewContext.PaymentApprovalID = ucPkgPaymentOptions.GetApprovalRequiredForCreditCard();
                Presenter.SaveProgramPackageSubscriptionMapping(availability);
                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    //Presenter.SetCompliancePkgAvailabilityForOrder(availability);
                    base.ShowSuccessMessage("Subscription(s) and Package Payment Options saved successfully.");
                    BindControls();
                    ResetControls();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }
                else
                {
                    base.ShowInfoMessage(ErrorMessage);
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

        //protected void CmdBarCancel_Click(object sender, EventArgs e)
        //{
        //    ResetControls();
        //}

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// To bind all controls
        /// </summary>
        private void BindControls()
        {
            BindSubscriptionOption();
            BindPriceModel();
            BindAvailabilityForOrder();
            Presenter.BindAutoRenewInvoiceOrderForPackage();
        }

        /// <summary>
        /// To bind Subscription Options checkbox
        /// </summary>
        private void BindSubscriptionOption()
        {
            Presenter.GetSubscriptionOptions();
            chkSubscriptionOption.DataSource = CurrentViewContext.ListSubscriptionOption;
            chkSubscriptionOption.DataBind();

            Presenter.GetSelectedSubscriptionOptions();
            if (chkSubscriptionOption.Items.Count > AppConsts.NONE)
            {
                chkSubscriptionOption.Items[0].Enabled = false;
            }
        }

        /// <summary>
        /// To bind Price Model
        /// </summary>
        private void BindPriceModel()
        {
            Presenter.GetPriceModel();
            ddlPriceModel.DataSource = CurrentViewContext.ListPriceModel;
            ddlPriceModel.DataBind();

            Presenter.GetSelectedPriceModelPriority();
        }

        private void BindAvailabilityForOrder()
        {
            chkAvailableForOrder.Checked = Convert.ToBoolean(Presenter.BindAvailabilityForOrder());
        }

        /// <summary>
        /// To reset Controls
        /// </summary>
        private void ResetControls()
        {
            //ddlPriceModel.SelectedIndex = AppConsts.NONE;
            //txtPriority.Text = String.Empty;
        }

        /// <summary>
        /// To disable controls as per permissions
        /// </summary>
        private void DisableControls()
        {
            if (CurrentViewContext.PermissionCode == LkpPermission.ReadOnly.GetStringValue()
                || CurrentViewContext.PermissionCode == LkpPermission.NoAccess.GetStringValue())
            {
                fsucCmdBarSubscription.SubmitButton.Enabled = false;
                chkSubscriptionOption.Enabled = false;
                ddlPriceModel.Enabled = false;
                txtPriority.Enabled = false;
                chkAvailableForOrder.Enabled = false;
            }
        }

        /// <summary>
        /// Binds the Package level Payment options for the Compliance Package
        /// </summary>
        private void BindPkgPaymentOptions()
        {
            ucPkgPaymentOptions.TenantId = CurrentViewContext.TenantId;
            ucPkgPaymentOptions.PackageTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
            ucPkgPaymentOptions.PkgNodeMappingId = CurrentViewContext.DeptProgramPackageID;
            ucPkgPaymentOptions.BindPaymentOptions();
        }

        #endregion

        #endregion
    }
}

