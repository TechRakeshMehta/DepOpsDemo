using Business.RepoManagers;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public partial class ClinicalRotationDefault : BasePage, IDefaultView
    {
        #region VARIABLES

        #region Private Variables

        private DefaultViewPresenter _presenter = new DefaultViewPresenter();

        #endregion

        #endregion

        #region PROPERTIES

        public DefaultViewPresenter Presenter
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


        #endregion

        #region EVENTS

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }

        /// <summary>
        /// Page OnInitComplete event.
        /// </summary>
        /// <param name="e">Event</param>
        protected override void OnInitComplete(EventArgs e)
        {

            try
            {
                base.dynamicPlaceHolder = phDynamic;
                base.OnInitComplete(e);
                SetModuleTitle("Clinical Rotation");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region PUBLIC METHODS
        [WebMethod]
        public static string GetSelectedItemPaymentDetail()
        {
            ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;
            if (itemPaymentContract.IsNullOrEmpty())
                itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;

            if (!itemPaymentContract.IsNullOrEmpty())
            {
                if (itemPaymentContract.orderID.IsNotNull() && itemPaymentContract.orderID > AppConsts.NONE)
                {
                    String IsRequirementPackage = itemPaymentContract.IsRequirementPackage ? AppConsts.TRUE : AppConsts.FALSE;
                    //Get Order Number
                    itemPaymentContract.RefreshItemSectionHtml = true;
                    itemPaymentContract.OrderNumber = ComplianceDataManager.GetOrderNumberByOrderID(itemPaymentContract.orderID, itemPaymentContract.TenantID);

                    if (!itemPaymentContract.PkgSubscriptionId.IsNullOrEmpty() && !itemPaymentContract.ItemID.IsNullOrEmpty())
                    {
                        ItemPaymentContract itempayment = ComplianceDataManager.GetItemPaymentDetail(itemPaymentContract.PkgSubscriptionId, itemPaymentContract.TenantID, itemPaymentContract.IsRequirementPackage).Where(cond => cond.ItemID == itemPaymentContract.ItemID).FirstOrDefault();
                        if (itempayment.IsNotNull())
                        {
                            itemPaymentContract.invoiceNumber = itempayment.invoiceNumber;
                            itemPaymentContract.OrganizationUserProfileID = itempayment.OrganizationUserProfileID;
                            itemPaymentContract.IsPaid = itempayment.OrderStatusCode == ApplicantOrderStatus.Paid.GetStringValue() ? true : false;
                            itemPaymentContract.OrderStatusCode = itempayment.OrderStatusCode;
                        }

                        if (!itemPaymentContract.IsPaid && itempayment.OrderStatusCode != ApplicantOrderStatus.Cancelled.GetStringValue())
                            itemPaymentContract.CompleteItemPaymentClickHtml = String.Format("<a href='#' class='completePaymentLink' style='color:#0000ff;' onclick=\"ItemPaymentClick('" + itemPaymentContract.PkgName + "','" + itemPaymentContract.CategoryName + "','" + itemPaymentContract.ItemID + "','" + itemPaymentContract.CategoryID + "','" + itemPaymentContract.ItemName + "','" + itemPaymentContract.PkgId + "','" + itemPaymentContract.PkgSubscriptionId + "','" + itemPaymentContract.orderID + "','" + itemPaymentContract.OrderNumber + "','" + itemPaymentContract.TotalPrice + "','" + itemPaymentContract.invoiceNumber + "','" + IsRequirementPackage + "','" + itemPaymentContract.OrganizationUserProfileID + "','" + AppConsts.ONE.ToString() + "','" + AppConsts.ONE.ToString() + "','" + itemPaymentContract.ClinicalRotationID + "','" + itemPaymentContract.TenantID + "','" + itemPaymentContract.CreatedByID + "')\" >{0}</a></br>", " (Complete your payment)");
                        else
                            itemPaymentContract.CompleteItemPaymentClickHtml = String.Empty;
                    }
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(itemPaymentContract);
            }
            else
                return null;
            //Int32 _organizationUserID = SysXWebSiteUtils.SessionService.OrganizationUserId;
            //SecurityManager.SaveNonPreferredBrowserLog(_organizationUserID, UtilityFeatures.NonPrefferedBrowser.GetStringValue());
            //return "Success";
        }


        protected void btnItemPaymentHidden_Click(object sender, EventArgs e)
        {
            #region Set Custom Data
            Session.Remove(ResourceConst.APPLICANT_PARKING_CART);
            INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract itemPaymentContract = new INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract();
            itemPaymentContract.PkgName = hdnItemPaymentPackageName.Value;
            itemPaymentContract.TenantID = Convert.ToInt32(hdnTenantID.Value);
            itemPaymentContract.CategoryName = hdnItemPaymentCategoryName.Value;
            itemPaymentContract.ItemID = !String.IsNullOrEmpty(hdnItemPaymentComplianceItemId.Value) ? Convert.ToInt32(hdnItemPaymentComplianceItemId.Value) : AppConsts.NONE;
            itemPaymentContract.CategoryID = !String.IsNullOrEmpty(hdnItemPaymentComplianceCategoryId.Value) ? Convert.ToInt32(hdnItemPaymentComplianceCategoryId.Value) : AppConsts.NONE;
            itemPaymentContract.ItemName = !String.IsNullOrEmpty(hdnItemPaymentItemName.Value) ? hdnItemPaymentItemName.Value : String.Empty;
            itemPaymentContract.PkgId = !String.IsNullOrEmpty(hdnItemPaymentPackageId.Value) ? Convert.ToInt32(hdnItemPaymentPackageId.Value) : AppConsts.NONE;
            itemPaymentContract.PkgSubscriptionId = Convert.ToInt32(hdnItemPaymentPackageSubscriptionID.Value);
            itemPaymentContract.orderID = !String.IsNullOrEmpty(hdnItemPaymentOrderID.Value) ? Convert.ToInt32(hdnItemPaymentOrderID.Value) : AppConsts.NONE;
            itemPaymentContract.OrderNumber = !String.IsNullOrEmpty(hdnItemPaymentOrderNumber.Value) ? hdnItemPaymentOrderNumber.Value : String.Empty;
            itemPaymentContract.invoiceNumber = !String.IsNullOrEmpty(hdnInvoiceNumber.Value) ? hdnInvoiceNumber.Value : String.Empty;
            itemPaymentContract.TotalPrice = !String.IsNullOrEmpty(hdnItemPaymentAmount.Value) ? Convert.ToDecimal(hdnItemPaymentAmount.Value) : AppConsts.NONE;
            itemPaymentContract.ClinicalRotationID = !String.IsNullOrEmpty(hdnItemPaymentClinicalRotationID.Value) ? Convert.ToInt32(hdnItemPaymentClinicalRotationID.Value) : AppConsts.NONE;
            itemPaymentContract.OrganizationUserProfileID = !String.IsNullOrEmpty(hdnItemPaymentOrganizationUserProfileID.Value) ? Convert.ToInt32(hdnItemPaymentOrganizationUserProfileID.Value) : AppConsts.NONE;
            itemPaymentContract.CreatedByID = Convert.ToInt32( hdnCreatedByID.Value); //CurrentViewContext.CurrentLoggedInUserID; //UAT-3083(Bug ID: 18515)
            if (itemPaymentContract.TotalPrice > AppConsts.NONE && itemPaymentContract.ItemID > AppConsts.NONE && itemPaymentContract.PkgSubscriptionId > AppConsts.NONE && itemPaymentContract.orderID > AppConsts.NONE && !String.IsNullOrEmpty(itemPaymentContract.invoiceNumber) && itemPaymentContract.OrganizationUserProfileID > AppConsts.NONE)
            {
                itemPaymentContract.IsRequirementPackage = Convert.ToBoolean(hdnItemPaymentIsRequirement.Value);

                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART, itemPaymentContract);
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "IsOrderCreated", AppConsts.TRUE },
                                                                    { "IsInstructorPreceptorPackage" , AppConsts.TRUE},
                                                                    { "SelectedTenantID", itemPaymentContract.TenantID.ToString() }

                                                                 };
                String url = String.Format("~/ComplianceOperations/Pages/ItemPaymentPopup.aspx?args={0}", queryString.ToEncryptedQueryString());
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenItemPaymentForm('" + url + "');", true);
            }


            #endregion
        }


        protected void btnRefeshPage_Click(object sender, EventArgs e)
        {
            ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;
            Session.Remove(ResourceConst.APPLICANT_PARKING_CART);
            if (itemPaymentContract.IsNullOrEmpty())
            {
                itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;
                Session.Remove(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART);
            }
            if (!itemPaymentContract.IsNullOrEmpty())
            {
                if (itemPaymentContract.IsRequirementPackage)
                {
                    //Dictionary<String, String> queryString = new Dictionary<String, String>
                    //                                             { 
                    //                                                { "Child", AppConsts.INSTRUCTOR_PRESEPTOR_DASHBOARD },
                    //                                                {"MenuId", "4" },
                    //                                                {"ReqPkgSubscriptionId",itemPaymentContract.PkgSubscriptionId.ToString() },
                    //                                                {"ClinicalRotationId",itemPaymentContract.ClinicalRotationID.ToString() }
                                                                    
                    //                                             };
                    //Response.Redirect(String.Format("~/ApplicantRotationRequirement/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                }
                if (itemPaymentContract.PkgSubscriptionId > AppConsts.NONE)
                {
                    //Dictionary<String, String> queryString = new Dictionary<String, String>
                    //                                             { 
                    //                                                { "Child", AppConsts.INSTRUCTOR_PRESEPTOR_DASHBOARD },
                    //                                                {"ItemPaymentPkgSubscriptionId", itemPaymentContract.PkgSubscriptionId.ToString() }
                                                                    
                    //                                             };
                    //Response.Redirect(String.Format("~/ApplicantRotationRequirement/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                }
            }
            Dictionary<String, String> queryStringdata = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child","~/ProfileSharing/UserControl/SharedUserRotationRequirementDataEntry.ascx" }
                                                                    
                                                                 };
            Response.Redirect(String.Format("~/ApplicantRotationRequirement/Default.aspx?args={0}", queryStringdata.ToEncryptedQueryString()));
        }

#endregion
    }
}
