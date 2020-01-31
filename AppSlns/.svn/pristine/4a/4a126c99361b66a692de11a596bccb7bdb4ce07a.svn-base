#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;


#endregion

#region UserDefined

using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;
using System.Text;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceOperations.Views;
using System.Web;
using Business.RepoManagers;
using System.Data;
using INTSOF.Contracts;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ItemPayment : BaseUserControl, IItemPaymentView
    {
        //[UAT-3077: (1 of ?) Initial Analysis and begin Dev: Pay per submission item type (CC only) for Tracking and Rotation]

        #region Variables

        #region Private Variables

        private Int32 _tenantId;
        private ItemPaymentPresenter _presenter = new ItemPaymentPresenter();
        //private ApplicantOrderCart _applicantOrderCart;
        private String _viewType;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region  Public Properties

        public ItemPaymentPresenter Presenter
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

        List<PkgPaymentOptions> IItemPaymentView.lstPaymentOptions
        {
            get;
            set;
        }
        String IItemPaymentView.PaymentModeCode
        {
            get;
            set;
        }
        /// <summary>
        /// Id for the Credit Card Payment Mode
        /// </summary>
        Int32 IItemPaymentView.PaymentModeId
        {
            get
            {
                return Convert.ToInt32(ViewState["CCId"]);
            }
            set
            {
                ViewState["CCId"] = value;
            }
        }


        Int32 IItemPaymentView.TenantId
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
            set
            {
                _tenantId = value;
            }
        }
        Int32 IItemPaymentView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        Int32 IItemPaymentView.OrganizationUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        IItemPaymentView CurrentViewContext
        {
            get { return this; }
        }

        /// <summary>
        /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
        /// </summary>

        ItemPaymentContract IItemPaymentView.itemPaymentContract
        {
            get
            {
                return ViewState["ItemPaymentContract"].IsNull() ? GetSesionValues() : (ItemPaymentContract)(ViewState["ItemPaymentContract"]);
            }
            set
            {
                ViewState["ItemPaymentContract"] = value;
            }
        }
        Boolean IItemPaymentView.IsInstructorPreceptorPackage
        {
            get
            {
                return ViewState["IsInstructorPreceptorPackage"].IsNull() ? CheckInstructorPackageExist() : (Boolean)(ViewState["IsInstructorPreceptorPackage"]);
            }
            set
            {
                ViewState["IsInstructorPreceptorPackage"] = value;
            }
        }

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// OnInit event to set page titles
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Order";
                //CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault basePage = base.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault;
                //basePage.SetModuleTitle("Create Order");
                //basePage.Title = "Create Order";
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
            try
            {
                //CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault basePage = base.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault;
                //basePage.SetModuleTitle("Create Order");

                if (!this.IsPostBack)
                {

                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);

                        if (args.ContainsKey("IsOrderCreated"))
                        {
                            if (Convert.ToBoolean(args["IsOrderCreated"]))
                            {
                                hdnIsOrderCreated.Value = AppConsts.TRUE;
                            }
                        }
                       
                        if (args.ContainsKey("SelectedTenantID"))
                        {
                            _tenantId = Convert.ToInt32(args["SelectedTenantID"]);
                        }
                    }
                }
                Presenter.OnViewInitialized();
                BindPaymentOptions(); // To do check
                BindPaymentInstructions();

                if (_tenantId == AppConsts.NONE)
                    Presenter.GetTenantId();
                else
                    CurrentViewContext.TenantId = _tenantId;

                if (CurrentViewContext.IsInstructorPreceptorPackage)
                {
                    GetInstructorPreceptorPackagTenantID();
                }
                BindPaymentDetails();
                Presenter.OnViewLoaded();
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

        #region Button Events

        protected void btnProceedPayment_Click(object sender, EventArgs e)
        {
            try
            {
                //lblValidationMsg
                if (chkAccept.Checked)
                {
                    Int32 OrderID = AppConsts.NONE;
                    String InvoiceNumber = String.Empty;
                    ItemPaymentContract res = new ItemPaymentContract();
                    res = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as ItemPaymentContract;
                    if (CurrentViewContext.IsInstructorPreceptorPackage)
                        res = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;
                    if (res != null && res.orderID > AppConsts.NONE)
                    {
                        OrderID = res.orderID;
                        InvoiceNumber = res.invoiceNumber;

                        if (CurrentViewContext.IsInstructorPreceptorPackage)
                        {
                            Session.Remove(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART);
                            res.OrganizationUserID = CurrentViewContext.OrganizationUserID;
                            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART, res);
                        }
                        else
                        {
                            Session.Remove(ResourceConst.APPLICANT_PARKING_CART);
                            res.OrganizationUserID = CurrentViewContext.OrganizationUserID;
                            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_PARKING_CART, res);
                        }
                    }
                    else
                    {
                        //Create Order
                        #region[Create Order]
                        if (Session["ClientMachineIP"].IsNullOrEmpty())
                        {
                            Session["ClientMachineIP"] = Request.UserHostAddress;
                        }
                        CurrentViewContext.itemPaymentContract.MachineIP = Convert.ToString(Session["ClientMachineIP"]);
                        Int32 catID = CurrentViewContext.itemPaymentContract.CategoryID;
                        res = Presenter.GenerateInvoiceNumber();
                        #endregion
                        OrderID = res.orderID;
                        InvoiceNumber = res.invoiceNumber;
                        if (CurrentViewContext.IsInstructorPreceptorPackage)
                        {
                            Session.Remove(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART);
                            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART, res);
                        }
                        else
                        {
                            Session.Remove(ResourceConst.APPLICANT_PARKING_CART);
                            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_PARKING_CART, res);
                        }
                    }

                    if (OrderID > AppConsts.NONE && !String.IsNullOrEmpty(InvoiceNumber) && !res.IsNullOrEmpty())
                    {
                        if (res.IsRequirementPackage)
                        {
                            RequirementVerificationManager.SyncRequirementVerificationToFlatData(Convert.ToString(CurrentViewContext.itemPaymentContract.PkgSubscriptionId), CurrentViewContext.TenantId, CurrentViewContext.CurrentLoggedInUserId);
                            var rotMovementserviceResponse = ClinicalRotationManager.PerformRotationLiveDataMovement(CurrentViewContext.TenantId, CurrentViewContext.itemPaymentContract.PkgSubscriptionId, CurrentViewContext.itemPaymentContract.CategoryID, CurrentViewContext.CurrentLoggedInUserId); //UAT 3164

                            //UAT-3805
                            IEnumerable<DataRow> rows = rotMovementserviceResponse.AsEnumerable();
                            List<INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation.ApplicantRequirementParameterContract> rotData = new List<INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation.ApplicantRequirementParameterContract>();

                            rotData.AddRange(rows.Select(col => new INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation.ApplicantRequirementParameterContract
                            {
                                RequirementPkgSubscriptionId = col["RPSID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RPSID"]),
                                RequirementCategoryId = col["ReqCategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqCategoryID"]),
                                prevCategoryStatusCode = col["OldCategoryStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["OldCategoryStatus"]),
                                NewCategoryStatusCode = col["NewCategoryStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["NewCategoryStatus"]),
                                AppRequirementItemDataID = col["ApplicantRequirementItemDataID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ApplicantRequirementItemDataID"]),
                                RequirementItemId = col["ReqItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqItemID"]),
                            }).ToList());

                            rotData.DistinctBy(cond => cond.RequirementPkgSubscriptionId).ForEach(row =>
                            {
                                List<Int32> categoryIds = rotData.Where(cond => cond.RequirementPkgSubscriptionId == row.RequirementPkgSubscriptionId
                                                                                && cond.prevCategoryStatusCode != RequirementCategoryStatus.APPROVED.GetStringValue())
                                                                 .Select(s => s.RequirementCategoryId).Distinct().ToList();

                                if (!categoryIds.IsNullOrEmpty())
                                {
                                    INTSOF.UI.Contract.ProfileSharing.ItemDocNotificationRequestDataContract itemDocRequestData = new INTSOF.UI.Contract.ProfileSharing.ItemDocNotificationRequestDataContract();
                                    List<INTSOF.UI.Contract.ProfileSharing.ItemDocNotificationRequestDataContract> itemDocNotificationRequestData = new List<INTSOF.UI.Contract.ProfileSharing.ItemDocNotificationRequestDataContract>();

                                    String categoryIDs = String.Join(",", categoryIds);

                                    itemDocRequestData.TenantID = CurrentViewContext.TenantId;
                                    itemDocRequestData.CategoryIds = categoryIDs;
                                    // itemDocRequestData.ApplicantOrgUserID = View.ApplicantId;
                                    itemDocRequestData.ApprovedCategoryIds = String.Empty;
                                    itemDocRequestData.RequestTypeCode = lkpUseTypeEnum.ROTATION.GetStringValue();
                                    itemDocRequestData.PackageSubscriptionID = null;
                                    itemDocRequestData.RPS_ID = row.RequirementPkgSubscriptionId;
                                    itemDocRequestData.CurrentLoggedInUserID = CurrentViewContext.CurrentLoggedInUserId;
                                    itemDocNotificationRequestData.Add(itemDocRequestData);

                                    //UAT-3805
                                    var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                                    var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                                    Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                                    dicParam.Add("CategoryData", itemDocNotificationRequestData);
                                    ProfileSharingManager.RunParallelTaskItemDocNotificationOnCatApproval(dicParam, LoggerService, ExceptiomService);
                                }
                            });
                        }
                        String redirectUrl = String.Empty;
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                    {
                                                        { "invnum" , InvoiceNumber.ToString()},
                                                        {"OrderId" , OrderID.ToString()},
                                                        {"IsItemPayment" , AppConsts.TRUE},
                                                        { "IsInstructorPreceptorPackage" ,CurrentViewContext.IsInstructorPreceptorPackage.ToString()},
                                                        { "SelectedTenantID" ,CurrentViewContext.TenantId.ToString()}
                                                    };
                        redirectUrl = "~/ComplianceOperations/Pages/CIMAccountSelection.aspx";
                        redirectUrl = String.Format(redirectUrl + "?args={0}", queryString.ToEncryptedQueryString());
                        Response.Redirect(redirectUrl);
                    }
                    {
                        lblValidationMsg.Text = "Unable to proceed for item payment. Please contact your admin department.";
                    }
                }
                else
                {
                    lblValidationMsg.Text = "Please read and accept the User Agreement";
                }
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

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            try
            {
                //Dictionary<String, String> queryString = new Dictionary<String, String>
                //                                                 { 
                //                                                    { "Child", AppConsts.APPLICANT_PARKING_PAYMENT_CONTROL} 
                //                                                 };
                //if (CurrentViewContext.itemPaymentContract.IsRequirementPackage)
                //{
                //    String menuId = "10";
                //    Response.Redirect(AppConsts.DASHBOARD_URL + "?MenuId=" + menuId + "&ReqPkgSubscriptionId=" + CurrentViewContext.itemPaymentContract.PkgSubscriptionId + "&ClinicalRotationId=" + CurrentViewContext.itemPaymentContract.ClinicalRotationID, true);
                //}
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ClosePopup();", true);
                // Response.Redirect(AppConsts.DASHBOARD_URL + "?ItemPaymentPkgSubscriptionId=" + CurrentViewContext.itemPaymentContract.PkgSubscriptionId, true);
                //String url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", AppConsts.APPLICANT_PARKING_PAYMENT_CONTROL, queryString.ToEncryptedQueryString());
                //Response.Redirect(url, true);
                //  SetSesionValues();
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

        #region Combobox Events
        protected void cmbPaymentModes_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {

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

        #region Method

        #region Public
        #endregion

        #region Private

        private void BindPaymentDetails()
        {
            if (!CurrentViewContext.itemPaymentContract.IsNullOrEmpty())
            {
                cmbPaymentModes.SelectedValue = Convert.ToString(CurrentViewContext.PaymentModeId);
                txtTotalPrice.Text = Convert.ToString(CurrentViewContext.itemPaymentContract.TotalPrice);
                lblItemName.Text = CurrentViewContext.itemPaymentContract.ItemName.HtmlEncode();
                hdfPkgId.Value = Convert.ToString(CurrentViewContext.itemPaymentContract.PkgId);
                hdfPkgSubscriptionId.Value = Convert.ToString(CurrentViewContext.itemPaymentContract.PkgSubscriptionId);
            }
        }

        private ItemPaymentContract GetSesionValues()
        {
            ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as ItemPaymentContract;
            if (CurrentViewContext.IsInstructorPreceptorPackage)
                itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;
            return itemPaymentContract;
        }

        private void SetSesionValues()
        { SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_PARKING_CART, CurrentViewContext.itemPaymentContract); }

        private void RedirectToPackageDataEntryScreen()
        {

            //Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
            //Dictionary<String, String> queryString = new Dictionary<String, String>
            //                                                     { 
            //                                                        { AppConsts.CHILD,  ChildControls.ApplicantPendingOrder}
            //                                                     };
            //Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }

        /// <summary>
        /// Bind the Package level payment Options
        /// </summary>
        private void BindPaymentOptions()
        {
            Presenter.GetPkgPaymentOptions();
            cmbPaymentModes.DataSource = CurrentViewContext.lstPaymentOptions;
            cmbPaymentModes.DataValueField = "PaymentOptionId";
            cmbPaymentModes.DataTextField = "PaymentOptionName";
            cmbPaymentModes.DataBind();

        }

        /// <summary>
        /// Bind the Instructions for the Payment Mode selected
        /// </summary>
        /// <param name="cmbPaymentModes"></param>
        private void BindPaymentInstructions()
        {
            //List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns = new List<Entity.ClientEntity.lkpPaymentOption>();
            //List<String> lstPaymentOptnCode = new List<String>();


            //UAT-1480: WB: Updates to the Credit Card Agreement Statement on our websites (AMS and Complio)
            if (!CurrentViewContext.lstPaymentOptions.IsNullOrEmpty() && CurrentViewContext.lstPaymentOptions.Where(d => d.PaymentOptionCode.Contains(PaymentOptions.Credit_Card.GetStringValue())).Any())
            {
                dvUserAgreement.Visible = true;
                litText.Text = Presenter.GetCreditCardAgreement();
            }
            else
            {
                dvUserAgreement.Visible = false;
            }
            //End
            List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns = new List<Entity.ClientEntity.lkpPaymentOption>();
            List<Entity.lkpPaymentOption> _lstMasterPaymentOptns = Presenter.GetMasterPaymentSettings(out lstClientPaymentOptns);
            // var _masterPaymentOption = _lstMasterPaymentOptns.Where(mpo => mpo.Code == CurrentViewContext.PaymentModeCode).First();
            var _clientPaymentOptn = CurrentViewContext.lstPaymentOptions.Where(po => po.PaymentOptionId == CurrentViewContext.PaymentModeId).FirstOrDefault();
            var _controlId = "pi_" + _clientPaymentOptn.PaymentOptionCode;
            var _isControlAdded = (pnlInstructions.FindControl(_controlId) as System.Web.UI.Control).IsNullOrEmpty() ? false : true;

            if (_clientPaymentOptn.IsNotNull() && !_isControlAdded)
            {
                var _masterPaymentOption = _lstMasterPaymentOptns.Where(mpo => mpo.Code == _clientPaymentOptn.PaymentOptionCode).First();

                if (!_masterPaymentOption.InstructionText.IsNullOrEmpty())
                {
                    System.Web.UI.Control _piInstructions = Page.LoadControl("~/ComplianceOperations/UserControl/PaymentInstructions.ascx");
                    (_piInstructions as PaymentInstructions).ID = _controlId;
                    (_piInstructions as PaymentInstructions).InstructionsText = _masterPaymentOption.InstructionText;
                    (_piInstructions as PaymentInstructions).PaymentModeText = _clientPaymentOptn.PaymentOptionName;
                    pnlInstructions.Controls.Add(_piInstructions);
                }
            }

        }

        //private itemPaymentContract GetItemPaymentDetails()
        //{
        //    Dictionary<String, String> args = new Dictionary<String, String>();
        //    itemPaymentContract itemPaymentContract = new itemPaymentContract();
        //    if (!Request.QueryString["args"].IsNull())
        //    {
        //        args.ToDecryptedQueryString(Request.QueryString["args"]);

        //        if (args.ContainsKey("IsRequirementPackage"))
        //        {
        //            itemPaymentContract.IsRequirementPackage = Convert.ToBoolean(args["IsRequirementPackage"]);
        //        }
        //        if (args.ContainsKey("ParkingPrice"))
        //        {
        //            itemPaymentContract.TotalPrice = Convert.ToDecimal(args["ParkingPrice"]);
        //        }
        //        if (args.ContainsKey("PkgName"))
        //        {
        //            itemPaymentContract.PkgName = Convert.ToString(args["PkgName"]);
        //        }
        //        if (args.ContainsKey("CategoryName"))
        //        {
        //            itemPaymentContract.CategoryName = Convert.ToString(args["CategoryName"]);
        //        }
        //        if (args.ContainsKey("ItemName"))
        //        {
        //            itemPaymentContract.ItemName = Convert.ToString(args["ItemName"]);
        //        }
        //        if (args.ContainsKey("PkgSubscriptionId"))
        //        {
        //            itemPaymentContract.PkgSubscriptionId = Convert.ToInt32(args["PkgSubscriptionId"]);
        //        }
        //        if (args.ContainsKey("PkgId"))
        //        {
        //            itemPaymentContract.PkgId = Convert.ToInt32(args["PkgId"]);
        //        }
        //    }
        //    itemPaymentContract.MachineIP = Convert.ToString(HttpContext.Current.Session["ClientMachineIP"]);
        //    return itemPaymentContract;
        //}
        private Boolean CheckInstructorPackageExist()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey("IsInstructorPreceptorPackage"))
                {
                    CurrentViewContext.IsInstructorPreceptorPackage = Convert.ToBoolean(args["IsInstructorPreceptorPackage"]);
                    return true;
                }
            }
            return false;
        }

        private void GetInstructorPreceptorPackagTenantID()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey("SelectedTenantID"))
                {
                    CurrentViewContext.TenantId = Convert.ToInt32(args["SelectedTenantID"]);                    
                }
            }     
        }

        #endregion

        #endregion

    }
}