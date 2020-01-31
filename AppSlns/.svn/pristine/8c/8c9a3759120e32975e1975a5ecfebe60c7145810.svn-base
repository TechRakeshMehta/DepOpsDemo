using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity.ClientEntity;
using System.Globalization;

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderSummary : BaseUserControl, IBkgOrderSummaryView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private BkgOrderSummaryPresenter _presenter = new BkgOrderSummaryPresenter();
        private String _parentScreenName = "";

        #endregion

        #endregion


        #region Properties

        #region Private Properties


        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        private IBkgOrderSummaryView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        private BkgOrderSummaryPresenter Presenter
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

        Int32 IBkgOrderSummaryView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
            }
        }

        Int32 IBkgOrderSummaryView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        Int32 IBkgOrderSummaryView.OrderID
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
            }
        }

        #region UAT-844
        Int32 IBkgOrderSummaryView.ServiceGroupID
        {
            get
            {
                return Convert.ToInt32(ViewState["ServiceGroupID"]);
            }
            set
            {
                ViewState["ServiceGroupID"] = value;
            }
        }

        String IBkgOrderSummaryView.ServiceGroupName
        {
            set
            {
                lblSvcGroupName.Text = value;
            }
        }

        BkgOrderPackageSvcGroup IBkgOrderSummaryView.bkgOrderPackageSvcGroup
        {
            get
            {
                return (BkgOrderPackageSvcGroup)ViewState["bkgOrderPackageSvcGroup"];
            }
            set
            {
                ViewState["bkgOrderPackageSvcGroup"] = value;
            }

        }

        List<lkpBkgSvcGrpReviewStatusType> IBkgOrderSummaryView.lstServiceGroupReviewStatus
        {
            get
            {
                return (List<lkpBkgSvcGrpReviewStatusType>)ViewState["lstServiceGroupReviewStatus"];
            }
            set
            {
                ViewState["lstServiceGroupReviewStatus"] = value;
            }
        }

        Int32 IBkgOrderSummaryView.orderPkgSvcGroupID
        {
            get
            {
                return Convert.ToInt32(ViewState["orderPkgSvcGroupID"]);
            }
            set
            {
                ViewState["orderPkgSvcGroupID"] = value;
            }
        }



        public String ParentScreenName
        {
            get;
            set;
        }

        public Int32 OrderPackageSvcGrpID
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region UAT-806 Creation of granular permissions for Client Admin users

        public String SSNPermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["SSNPermissionCode"]);
            }
            set
            {
                ViewState["SSNPermissionCode"] = value;
            }
        }
        public Boolean IsDOBDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDOBDisabled"] ?? false);
            }
            set
            {
                ViewState["IsDOBDisabled"] = value;
            }
        }
        #endregion

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (ParentScreenName == AppConsts.BKG_ORDER_REVIEW_QUEUE)
                    {
                        _parentScreenName = AppConsts.BKG_ORDER_REVIEW_QUEUE;
                        CurrentViewContext.orderPkgSvcGroupID = OrderPackageSvcGrpID;
                        Presenter.GetOrderPackageServiceGroupData();
                    }
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
                    ApplyActionLevelPermission(ActionCollection, "Order Detail Page");
                }
                BindDetails();
                HideShowControlsForGranularPermission();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Methods

        protected void BindDetails()
        {
            try
            {
                OrderDetailClientAdmin orderDetail = Presenter.GetOrderDetailsInfo();
                OrderDetailInfoClientAdmin orderDetailInfo = orderDetail.OrderDetailInfoClientAdmin;
                txtAddress1.Text = orderDetailInfo.Address1;
                txtAddress2.Text = orderDetailInfo.Address2;
                txtApplicantName.Text = orderDetailInfo.ApplicantName;
                txtCategory.Text = orderDetailInfo.Category;
                txtCity.Text = orderDetailInfo.City;
                //txtDateCompleted.Text = orderDetailInfo.DateCompleted;
                if (orderDetailInfo.DateCompleted.HasValue)
                {
                    txtDateCompleted.Text = orderDetailInfo.DateCompleted.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
                }
                else
                {
                    txtDateCompleted.Text = String.Empty;
                }
                //txtDateCreated.Text = orderDetailInfo.DateCreated;
                if (orderDetailInfo.DateCreated.HasValue)
                {
                    txtDateCreated.Text = orderDetailInfo.DateCreated.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
                }
                else
                {
                    txtDateCreated.Text = String.Empty;
                }
                //txtDOB.Text = orderDetailInfo.DOB;
                if (!orderDetailInfo.DOB.IsNullOrEmpty())
                {
                    txtDOB.Text = (DateTime.ParseExact(orderDetailInfo.DOB, "dd-MM-yyyy", null)).ToString("MM/dd/yyyy");
                }
                else
                {
                    txtDOB.Text = String.Empty;
                }
                txtEmail.Text = orderDetailInfo.Email;
                txtGender.Text = orderDetailInfo.Gender;
                txtHierarchy.Text = orderDetailInfo.InstitutionHierarchy;
                txtOrderId.Text = Convert.ToString(orderDetailInfo.OrderID);
                txtOrderStatus.Text = orderDetailInfo.OrderStatus;
                //txtpaidDate.Text = orderDetailInfo.DatePaid;
                if (orderDetailInfo.DatePaid.HasValue)
                {
                    txtpaidDate.Text = orderDetailInfo.DatePaid.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
                }
                else
                {
                    txtpaidDate.Text = String.Empty;
                }
                //txtPaymentMethod.Text = orderDetailInfo.PaymentType;
                cmbPaymentType.DataSource = orderDetail.PaymentTypesAndStatus.OrderByDescending(cond => cond.OrderPaymentDetailID);
                cmbPaymentType.DataBind();
                txtPhone.Text = orderDetailInfo.PhoneNumber;
                //UAT-806 Creation of granular permissions for Client Admin users
                txtSSNMAsked.Text = Presenter.GetMaskedSSN(orderDetailInfo.SSN);
                txtSSN.Text = Presenter.GetFormatttedSSN(orderDetailInfo.SSN);
                txtState.Text = orderDetailInfo.State;
                txtZipCode.Text = orderDetailInfo.Zip;
                //Show Hide Electronic drug screening documnet view link.
                if (Presenter.IsEdsServiceExitForOrder())
                {
                    divEDSDocumentLink.Visible = true;
                    hdfOrderID.Value = Convert.ToString(orderDetailInfo.OrderID);
                    hdfDocumentType.Value = "EDS_AuthorizationForm";
                    hdfTenantId.Value = Convert.ToString(CurrentViewContext.SelectedTenantId);
                }
                else
                {
                    divEDSDocumentLink.Visible = false;
                }

                #region UAT-844 - Showing Service Group Section.
                if (_parentScreenName == AppConsts.BKG_ORDER_REVIEW_QUEUE)
                {
                    dvServiceGroup.Visible = true;
                    CurrentViewContext.ServiceGroupName = CurrentViewContext.bkgOrderPackageSvcGroup.BkgSvcGroup.BSG_Name;
                    BindServiceGroupReviewStatus();
                }
                #endregion
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Method to Bind Service Group Review Status combobox
        /// </summary>
        private void BindServiceGroupReviewStatus()
        {
            Presenter.GetServiceGroupReviewStatusList();
            cmbSvcGroupReviewStatus.DataSource = CurrentViewContext.lstServiceGroupReviewStatus;
            cmbSvcGroupReviewStatus.DataBind();
            cmbSvcGroupReviewStatus.SelectedValue = CurrentViewContext.bkgOrderPackageSvcGroup.lkpBkgSvcGrpReviewStatusType.BSGRS_ReviewCode;
            cmbSvcGroupReviewStatus.Enabled = false;
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.IsDOBDisable)
            {
                divDOB.Visible = false;
            }
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                divSSNMasked.Visible = false;
            }
            else if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                divSSNMasked.Visible = true;
            }
        }
        #endregion
        #endregion
    }
}