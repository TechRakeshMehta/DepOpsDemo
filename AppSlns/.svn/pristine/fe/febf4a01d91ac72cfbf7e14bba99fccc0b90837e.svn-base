using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CoreWeb.Search.Views;
using Microsoft.Practices.ObjectBuilder;
using System.Web.Services;
using INTSOF.Utils;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using Business.RepoManagers;
using Entity;

namespace CoreWeb.Search.Views
{
    public partial class SearchDefault : BasePage, IDefaultView
    {
        private DefaultViewPresenter _presenter = new DefaultViewPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            base.SetModuleTitle("Manage Search");
        }


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
        protected override void OnInitComplete(EventArgs e)
        {
            base.dynamicPlaceHolder = this.plcDynamic;
            base.OnInitComplete(e);
        }

        #region Methods

        #region Public Methods

        /// <summary>
        /// Updates the status of whether the client admin can receive Internal message from Applicants or Not
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static Boolean UpdateInternalMsgNotificationSettings(Int32 id, Int32 cid)
        {
            if (Business.RepoManagers.SecurityManager.UpdateInternalMsgNotificationSettings(id, cid))
                return true;
            return false;
        }

        [WebMethod]
        public static List<Tuple<Int32, String, String>> GetCategoryUpdatedUrl(String PackageSubscription, String Tenant)
        {
            Int32 PackageSubscriptionId = Convert.ToInt32(PackageSubscription);
            Int32 TenantId = Convert.ToInt32(Tenant);
            //UAT 3106
            //String OptionalCategoryClientSetting = AppConsts.STR_ONE;
            //Entity.ClientEntity.ClientSetting OptionalCategorySetting = ComplianceDataManager.GetClientSetting(TenantId, Setting.EXECUTE_COMPLIANCE_RULE_WHEN_OPTIONAL_CATEGORY_COMPLIANCE_RULE_MET.GetStringValue());
            Boolean OptionalCategoryClientSetting = ComplianceDataManager.GetOptionalCategorySettingForNode(TenantId, AppConsts.NONE, PackageSubscriptionId, SubscriptionTypeCategorySetting.COMPLIANCE_PACKAGE.GetStringValue());
            //if (!OptionalCategorySetting.IsNullOrEmpty())
            //{
            //    OptionalCategoryClientSetting = OptionalCategorySetting.CS_SettingValue;
            //}

            if (PackageSubscriptionId <= AppConsts.NONE || TenantId <= AppConsts.NONE)
            {
                return new List<Tuple<int, string, string>>();
            }

            List<INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass> lstCategoryDetails = Business.RepoManagers.ComplianceDataManager.GetApplicantComplianceCategoryData(PackageSubscriptionId, TenantId);

            List<Tuple<Int32, String, String>> result = new List<Tuple<Int32, String, String>>();

            foreach (var categoryDetails in lstCategoryDetails)
            {
                if (!categoryDetails.CategoryStatusName.IsNullOrEmpty() && !categoryDetails.CategoryStatusCode.IsNullOrEmpty())
                {
                    var catComplianceStatusName = categoryDetails.CategoryStatusName;
                    var catComplianceStatus = categoryDetails.CategoryStatusCode;
                    var CategoryExceptionStatusCode = categoryDetails.CategoryExceptionStatusCode == null ? String.Empty : categoryDetails.CategoryExceptionStatusCode;

                    if (catComplianceStatus.IsNullOrEmpty() || (catComplianceStatus == ApplicantCategoryComplianceStatus.Incomplete.GetStringValue()
                            && !String.IsNullOrEmpty(CategoryExceptionStatusCode)
                            && CategoryExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPLIED.GetStringValue()))
                    {
                        catComplianceStatusName = "Pending Review";
                        catComplianceStatus = ApplicantCategoryComplianceStatus.Pending_Review.GetStringValue();
                    }

                    string url = "";

                    if (catComplianceStatus.IsNullOrEmpty() || catComplianceStatus == ApplicantCategoryComplianceStatus.Incomplete.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/no16.png");
                    }
                    else if (!(String.IsNullOrEmpty(CategoryExceptionStatusCode)) && CategoryExceptionStatusCode == "AAAD" && catComplianceStatus == ApplicantCategoryComplianceStatus.Approved.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/yesx16.png");
                    }
                    else if (catComplianceStatus == ApplicantCategoryComplianceStatus.Approved.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/yes16.png");
                    }
                    else if (catComplianceStatus == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/yesx16.png");
                    }
                    else if (catComplianceStatus == ApplicantCategoryComplianceStatus.Pending_Review.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/attn16.png");
                    }

                    //UAT 3106 
                    if (!categoryDetails.IsComplianceRequired)
                    {
                        if (!OptionalCategoryClientSetting)
                        {
                            url = String.Format("../Resources/Mod/Compliance/icons/optional.png");
                        }
                        else if (categoryDetails.RulesStatusID.Trim() != "1")
                        {
                            url = String.Format("../Resources/Mod/Compliance/icons/optional.png");
                        }
                        else if (categoryDetails.RulesStatusID.Trim() == "1")
                        {
                            url = String.Format("../Resources/Mod/Compliance/icons/yes16.png");
                        }
                    }

                    String tooltipComplianceStatus = String.Empty;
                    if (catComplianceStatusName == "Approved" && CategoryExceptionStatusCode == "AAAD")
                    {
                        tooltipComplianceStatus = "Approved Override";
                    }
                    else
                    {
                        tooltipComplianceStatus = catComplianceStatusName;
                    }

                    result.Add(new Tuple<Int32, String, String>(categoryDetails.CategoryId, url, tooltipComplianceStatus));
                }
            }

            return result;
        }

        [WebMethod]
        public static String GetSingleDocumentForPDFViewer(String documentId, String tenantId)
        {
            if (!documentId.IsNullOrEmpty() && !tenantId.IsNullOrEmpty())
            {
                Dictionary<String, String> requestSingleDocViewerArgs = new Dictionary<String, String>();
                requestSingleDocViewerArgs = new Dictionary<String, String>
                                                                 {
                                                                    {"OrganizationUserId", AppConsts.ZERO },
                                                                    {"SelectedTenantId", tenantId},
                                                                    {"SelectedCatUnifiedStartPageID",AppConsts.ZERO},
                                                                    {"DocumentId",documentId},
                                                                    {"IsRequestAuth",Convert.ToString(AppConsts.TRUE)},
                                                                    {"DocumentViewType",UtilityFeatures.Single_Document.GetStringValue()}
                                                                 };
                //hdnSelectedCatUnifiedStartPageID.Value = Convert.ToString(selectedCatUnifiedStartPageID);
                String _redirectUrl = String.Format(@"/ComplianceOperations/UnifiedPdfDocViewer.aspx?args={0}", requestSingleDocViewerArgs.ToEncryptedQueryString());
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                return javaScriptSerializer.Serialize(new { redirectUrl = _redirectUrl });
            }
            return null;
        }

        #region UAT-2276:Regression testing and performance optimization

        [WebMethod]
        public static ApplicantSMSNotificationDataContract GetAndUpdateSMSNotificationData(String organizationUserId, String currentLoggedInUserId)
        {
            Int32 organizationUserID = Convert.ToInt32(organizationUserId);
            Int32 currentLoggedInUserID = Convert.ToInt32(currentLoggedInUserId);
            //SMSNotificationManager.UpdateSubscriptionStatusFromAmazon(organizationUserID, currentLoggedInUserID);
            Entity.OrganisationUserTextMessageSetting ApplicantSMSSubscriptionData = SMSNotificationManager.GetSMSDataByApplicantId(organizationUserID);

            ApplicantSMSNotificationDataContract appSMSDataContract = new ApplicantSMSNotificationDataContract();

            if (!ApplicantSMSSubscriptionData.IsNullOrEmpty()
                    && ApplicantSMSSubscriptionData.OUTMS_ID > AppConsts.NONE)
            {
                appSMSDataContract.IsReceiveTextNotification = ApplicantSMSSubscriptionData.OUTMS_ReceiveTextNotification;
                appSMSDataContract.PhoneNumber = ApplicantSMSSubscriptionData.OUTMS_MobileNumber;
            }
            else
            {
                appSMSDataContract.IsReceiveTextNotification = false;
                appSMSDataContract.IsComfirmMessageVisible = false;
            }
            return appSMSDataContract;
        }
        #endregion


        [WebMethod]
        public static String ArchiveOrUnArchive(Int32 tenantID, Int32? bkgOrderID, Int32 currentUserID, String buttonid, Int32? packageSubscriptionID, String packageType, String archiveStatus, Int32 orderId)
        {
            String retrunResult = String.Empty;
            String msg = String.Empty;
            String msgType = String.Empty;
            if (bkgOrderID.HasValue)
            {
                List<Int32> lstSelectedOrderIds = new List<Int32>();
                if (archiveStatus == ArchiveState.Active.GetStringValue())
                {
                    lstSelectedOrderIds.Add(orderId);
                    retrunResult = Business.RepoManagers.StoredProcedureManagers.ArchieveBkgOrderIds(lstSelectedOrderIds, tenantID, currentUserID);
                    msg = retrunResult == "true" ? " Background Order(s) archived successfully." : (retrunResult == "false" ? " Unable to archive background Order(s)." : retrunResult);
                    msgType = retrunResult == "true" ? "sucs" : "info";
                }
                else
                {
                    List<Int32> bkgOrderArchiveHistoryIds = ComplianceDataManager.GetBkgOrderArchiveHistoryIds(bkgOrderID.Value, tenantID);
                    Boolean finalResult = ComplianceDataManager.ApproveUnArchivalRequests(tenantID, bkgOrderArchiveHistoryIds, currentUserID, SubscriptionType.ARCHIVED_SUBSCRIPTIONS.GetStringValue(), ArchivePackageType.Screening.GetStringValue());
                    retrunResult = finalResult == true ? "true" : "false";
                    msg = retrunResult == "true" ? "  Background Order(s) un-Archived successfully." : (retrunResult == "false" ? " Unable to un-archive background Order(s)." : retrunResult);
                    msgType = retrunResult == "true" ? "sucs" : "info";
                }

            }
            else if (packageSubscriptionID.HasValue)
            {

                List<Int32> lstPackageSubscriptionID = new List<Int32>();
                if (archiveStatus == ArchiveState.Active.GetStringValue())
                {
                    lstPackageSubscriptionID.Add(packageSubscriptionID.Value);
                    retrunResult = ComplianceDataManager.ArchieveSubscriptionsManually(lstPackageSubscriptionID, tenantID, currentUserID);
                    msg = retrunResult == "true" ? "Subscription(s) archived successfully." : (retrunResult == "false" ? " Unable to archive subscription(s)." : retrunResult);
                    msgType = retrunResult == "true" ? "sucs" : "info";
                }
                else
                {
                    List<Int32> pkgSubArchiveHistoryIds = ComplianceDataManager.GetPkgSubArchiveHistoryIds(packageSubscriptionID.Value, tenantID);
                    Boolean finalResult = ComplianceDataManager.ApproveUnArchivalRequests(tenantID, pkgSubArchiveHistoryIds, currentUserID, SubscriptionType.ARCHIVED_SUBSCRIPTIONS.GetStringValue(), ArchivePackageType.Tracking.GetStringValue());
                    retrunResult = finalResult == true ? "true" : "false";
                    msg = retrunResult == "true" ? "Subscription(s) un-Archived successfully" : (retrunResult == "false" ? " Unable to un-archive subscription(s)." : retrunResult);
                    msgType = retrunResult == "true" ? "sucs" : "info";
                }
            }

            String result = "{\"archiveOrUnArchiveStatus\":\"" + Convert.ToString(retrunResult) + "\",\"buttonid\": \"" + buttonid + "\",\"msg\": \"" + msg + "\",\"msgType\": \"" + msgType + "\" }";
            return result;
        }

        #endregion

        #endregion

        #region UAT-2930
        /// <summary>
        /// Updates the Two factor authentication for user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static Boolean UpdateUserTwoFactorAuthenticationSettings(String Userid, Int32 currentUserid, Int32 tenantId)
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(currentUserid);
            organizationUser.PrimaryEmailAddress = organizationUser.aspnet_Users.aspnet_Membership.Email.IsNullOrEmpty() ? String.Empty : organizationUser.aspnet_Users.aspnet_Membership.Email;
            if (Business.RepoManagers.SecurityManager.DeleteTwofactorAuthenticationForUserID(Userid, currentUserid))
            {
                CommunicationManager.SendMailOnTwoFactorAuthentication(organizationUser, tenantId);
                return true;
            }
            return false;
        }
        #endregion
    }
}
