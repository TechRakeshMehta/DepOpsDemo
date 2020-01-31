#region Namespaces

#region System Defined

using System;
using System.Text;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using Entity.ClientEntity;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceRuleEngine;
using System.Collections;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Web;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class VerificationApplicantPanelPresenter : Presenter<IVerificationApplicantPanelView>
    {
        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public VerificationApplicantPanelPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {

        }

        /// <summary>
        /// call when View Initialized
        /// </summary>
        public override void OnViewInitialized()
        {
            GetApplicantComplianceData();
            GetGranularPermissions();
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unMaskedSSN);
        }

        public String GetFormattedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }

        public void GetApplicantComplianceData()
        {
            vwApplicantComplianceItemData ApplicantComplianceItemDataList;
            String status = "";
            Entity.CustomPagingArgs customPagingArgs = new Entity.CustomPagingArgs();
            String itemComplianceStatusCode = String.Empty;
            String reviewerTypeCode = String.Empty;
            Int32 clientId = View.OrganizationUserData.Organization.Tenant.TenantID;
            Int32 reviewerId = 0;
            if (clientId == SecurityManager.DefaultTenantID)
            {
                status += "'" + ApplicantItemComplianceStatus.Pending_Review.GetStringValue() + "'";
                //Code commented for UAT - 833 : Assignment queue for ADB admins should only show pending review for admin items
                //status += ",'" + ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue() + "'";
                reviewerTypeCode = LkpReviewerType.Admin;
            }
            else
            {
                reviewerTypeCode = LkpReviewerType.ClientAdmin;
                if (IsThirdPartyTenant)
                {
                    status += "'" + ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue() + "'";
                    reviewerId = View.LoggedInUser.TenantID;
                }
                else
                {
                    status += "'" + ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue() + "'";
                }
            }
            //if (View.WorkQueue == WorkQueueType.)
            //{
            //}
            if (View.IsException)
            {
                status = "'" + ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue() + "'";
            }
            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if ((IsDefaultTenant || IsThirdPartyTenant) && View.TenantId_Global != 0)
            {
                clientId = View.TenantId_Global;
            }

            if (!((IsDefaultTenant || IsThirdPartyTenant) && View.TenantId_Global == 0))
            {

                if (View.WorkQueue != WorkQueueType.AssignmentWorkQueue && View.WorkQueue != WorkQueueType.UserWorkQueue)
                {
                    View.PackageSubscriptionIdList = new List<PkgSubscriptionIDList>();

                    //UAT-2460
                    List<Int32> PackageSubscriptionIds = new List<Int32>();
                    if (View.WorkQueue == WorkQueueType.ComplianceSearch)
                    {
                        if (View.TenantId != SecurityManager.DefaultTenantID)
                        {
                            PackageSubscriptionIds = ComplianceDataManager.GetNextAndPrevPackageSubscriptionIds(View.TenantId_Global, View.SelectedPackageSubscriptionID_Global, GetXMLString(GetArchiveStateId()), View.CurrentLoggedInUserId);
                        }
                        else
                        {
                            PackageSubscriptionIds = ComplianceDataManager.GetNextAndPrevPackageSubscriptionIds(View.TenantId_Global, View.SelectedPackageSubscriptionID_Global, GetXMLString(GetArchiveStateId()));
                        }
                    }

                    if (!PackageSubscriptionIds.IsNullOrEmpty())
                    {
                        foreach (var id in PackageSubscriptionIds)
                        {
                            View.PackageSubscriptionIdList.Add(new PkgSubscriptionIDList
                            {
                                PackageSubscriptionID = id,
                                applicantcomplianceitemid = View.ItemDataId_Global,
                                applicantcompliancecategoryid = 0,
                                complianceitemid = 0
                            });
                        }
                    }
                    else
                    {
                        View.PackageSubscriptionIdList.Add(new PkgSubscriptionIDList
                        {
                            PackageSubscriptionID = View.SelectedPackageSubscriptionID_Global,
                            applicantcomplianceitemid = View.ItemDataId_Global,
                            applicantcompliancecategoryid = 0,
                            complianceitemid = 0
                        });
                    }
                }
                //else if ((View.SelectedPackageSubscriptionID_Global <= AppConsts.MINUS_ONE || View.PackageSubscriptionIdList == null || View.PackageSubscriptionIdList.IndexOf(View.SelectedPackageSubscriptionID_Global) < AppConsts.NONE) && View.WorkQueue != WorkQueueType.DataItemSearch)
                //UAT-755: Clicking previous or next button on left panel of verification details should become gray if admin has navigated away from the first/last student (and has completed all assigned verifications) in the queue 
                else if (View.WorkQueue != WorkQueueType.DataItemSearch)
                {
                    Boolean isDefaultThirdParty = false;
                    if ((IsDefaultTenant || IsThirdPartyTenant) && View.WorkQueue == WorkQueueType.ExceptionAssignmentWorkQueue)
                    {
                        isDefaultThirdParty = true;
                    }
                    else if (IsDefaultTenant && View.WorkQueue == WorkQueueType.ExceptionUserWorkQueue)
                    {
                        isDefaultThirdParty = true;
                    }
                    else if ((IsDefaultTenant || IsThirdPartyTenant))
                    {
                        isDefaultThirdParty = true;
                    }

                    int AssignedToUserID = AppConsts.MINUS_ONE;
                    if (View.WorkQueue == WorkQueueType.UserWorkQueue)
                        AssignedToUserID = View.CurrentLoggedInUserId;
                    //if (View.VerificationGridCustomPaging.IsNotNull())
                    //{
                    //    View.VerificationGridCustomPaging.PageSize = AppConsts.NONE;
                    //    View.VerificationGridCustomPaging.DefaultSortExpression = "ApplicantComplianceItemId";
                    //    View.VerificationGridCustomPaging.SecondarySortExpression = QueueConstants.DEFAULT_SORTING_FIELDS;
                    //    resultQuery = customPagingArgs.ApplyFilterOrSort(resultQuery, View.VerificationGridCustomPaging);
                    //}  
                    Int32 pageIndex = 0; // View.SubPageIndex;
                    Int32 totalPages = 0; // View.SubTotalPages;
                    Int32? orgUserId;
                    //Added this logic for permission check if adb user loggedin then pass orguserid=null to get all records
                    // otherwise get only those records on which nodes user has permisson
                    if (IsDefaultTenant)
                        orgUserId = null;
                    else
                        orgUserId = View.CurrentLoggedInUserId;
                    //View.PackageSubscriptionIdList = ComplianceDataManager.GetSubscriptionIdList(View.TenantId_Global, (View.PackageId == 0) ? (Int32?)null : View.PackageId, (View.CategoryId == 0) ? (Int32?)null : View.CategoryId, (View.UserGroupId == 0) ? (Int32?)null : View.UserGroupId, View.IncludeIncompleteItems, View.ShowOnlyRushOrders, status, reviewerId, reviewerTypeCode, AssignedToUserID, (View.SelectedPackageSubscriptionID_Global > 0) ? View.SelectedPackageSubscriptionID_Global : (Int32?)null, orgUserId, out pageIndex, out totalPages, isDefaultThirdParty, View.IsEscalationRecords);
                    //UAT-839: Previous/Next student (Purple) buttons in Verification Details screen turning gray when items remain in queue.
                    if (HttpContext.Current.Session["CurentPackageSubscriptionID"] == null
                        || Convert.ToInt32(HttpContext.Current.Session["CurentPackageSubscriptionID"]) != View.SelectedPackageSubscriptionID_Global)
                    {
                        View.PackageSubscriptionIdList = ComplianceDataManager.GetSubscriptionIdList(View.TenantId_Global, (View.PackageId == 0) ? (Int32?)null : View.PackageId, (View.CategoryId == 0) ? (Int32?)null : View.CategoryId, (View.UserGroupId == 0) ? (Int32?)null : View.UserGroupId, View.IncludeIncompleteItems, View.ShowOnlyRushOrders, status, reviewerId, reviewerTypeCode, AssignedToUserID, (View.SelectedPackageSubscriptionID_Global > 0) ? View.SelectedPackageSubscriptionID_Global : (Int32?)null, View.ItemDataId_Global, orgUserId, isDefaultThirdParty, View.IsEscalationRecords, View.CurrentLoggedInUserId);
                        View.SubPageIndex = pageIndex;
                        View.SubTotalPages = totalPages;
                        if (View.SelectedPackageSubscriptionID_Global == AppConsts.MINUS_TWO)
                            View.SelectedPackageSubscriptionID_Global = View.PackageSubscriptionIdList.LastOrDefault().PackageSubscriptionID; // ?? 0;
                        else if (View.SelectedPackageSubscriptionID_Global <= AppConsts.NONE)
                            View.SelectedPackageSubscriptionID_Global = View.PackageSubscriptionIdList.FirstOrDefault().PackageSubscriptionID; // ?? 0;

                        HttpContext.Current.Session["CurentPackageSubscriptionID"] = View.SelectedPackageSubscriptionID_Global;
                        HttpContext.Current.Session["CurrentSubscriptionIDList"] = View.PackageSubscriptionIdList;
                    }
                    else
                    {
                        View.PackageSubscriptionIdList = (List<PkgSubscriptionIDList>)(HttpContext.Current.Session["CurrentSubscriptionIDList"]);
                    }
                }

                //ApplicantComplianceItemDataList = ComplianceDataManager.GetApplicantComplianceItemData(View.TenantId_Global, View.SelectedPackageSubscriptionID_Global, AppConsts.MINUS_ONE, null, true, reviewerId).FirstOrDefault();
                ApplicantComplianceItemDataList = ComplianceDataManager.GetApplicantVerificationDetails(View.SelectedPackageSubscriptionID_Global, View.TenantId_Global);
                View.CurrentCompliancePackageId_Global = ApplicantComplianceItemDataList.PackageID;
                View.CurrentApplicantId_Global = ApplicantComplianceItemDataList.ApplicantID;
                View.SubscriptionExpirationDate = ApplicantComplianceItemDataList.ExpirationDate;

                // if (View.SelectedComplianceCategoryId_Global <= AppConsts.NONE)
                /*                View.SelectedComplianceCategoryId_Global = ApplicantComplianceItemDataList.CategoryID;*/

                //PackageSubscription _packageSubscription = ComplianceDataManager.GetPackageSubscriptionByPackageID(View.CurrentCompliancePackageId_Global, View.CurrentApplicantId_Global, View.TenantId_Global);
                View.CurrentCompliancePackageStatus = ApplicantComplianceItemDataList.ComplianceStatusName;//_packageSubscription.lkpPackageComplianceStatu.Name;
                View.CurrentCompliancePackageStatusCode = ApplicantComplianceItemDataList.ComplianceStatusCode;//_packageSubscription.lkpPackageComplianceStatu.Code;

                View.packageName = ApplicantComplianceItemDataList.PackageName;
                View.CurrentPackageBredCrum = ApplicantComplianceItemDataList.InstitutionHierarchy;
                if (View.PackageSubscriptionIdList.Count > 1)
                {
                    for (int i = 0; i < View.PackageSubscriptionIdList.Count; i++)
                    {
                        if (View.PackageSubscriptionIdList[i].PackageSubscriptionID == View.SelectedPackageSubscriptionID_Global)
                        {
                            View.ItemDataId_Global = View.PackageSubscriptionIdList[i].applicantcomplianceitemid;
                            //UAT-755: Clicking previous or next button on left panel of verification details should become gray if admin has navigated away from the first/last student (and has completed all assigned verifications) in the queue 
                            if (i == AppConsts.NONE)
                            {
                                View.PrevPackageSubscriptionID = AppConsts.NONE;
                                View.PrevAppCmpItemID = AppConsts.NONE;
                            }
                            else
                            {
                                View.PrevPackageSubscriptionID = View.PackageSubscriptionIdList[i - 1].PackageSubscriptionID; //?? 0;
                                View.PrevAppCmpItemID = View.PackageSubscriptionIdList[i - 1].applicantcomplianceitemid;
                            }

                            if ((i + 1) < View.PackageSubscriptionIdList.Count)
                            {
                                View.NextPackageSubscriptionID = View.PackageSubscriptionIdList[i + 1].PackageSubscriptionID; // ?? 0;
                                View.NextAppCmpItemID = View.PackageSubscriptionIdList[i + 1].applicantcomplianceitemid;
                            }
                            else
                            {
                                View.NextPackageSubscriptionID = AppConsts.NONE;
                                View.NextAppCmpItemID = AppConsts.NONE;
                            }

                            //if (i > AppConsts.NONE)
                            //    View.PrevPackageSubscriptionID = View.PackageSubscriptionIdList[i - 1] ?? 0;
                            //else if (View.SubPageIndex == AppConsts.ONE)
                            //    View.PrevPackageSubscriptionID = AppConsts.NONE;
                            //else
                            //    View.PrevPackageSubscriptionID = AppConsts.MINUS_TWO;

                            //View.SelectedPackageSubscriptionID_Global = View.PackageSubscriptionIdList[i] ?? 0;

                            //if ((i + 1) < View.PackageSubscriptionIdList.Count)
                            //    View.NextPackageSubscriptionID = View.PackageSubscriptionIdList[i + 1] ?? 0;
                            ////else if ((View.SubPageIndex * 50) >= View.SubTotalPages)
                            ////Replaced the page Size with 3 instead of 50 UAT-755
                            //else if ((View.SubPageIndex * 3) >= View.SubTotalPages)
                            //    View.NextPackageSubscriptionID = AppConsts.NONE;
                            //else
                            //    View.NextPackageSubscriptionID = AppConsts.MINUS_ONE;
                            //break;
                        }
                    }
                }
                else
                {
                    View.NextPackageSubscriptionID = AppConsts.NONE;
                    View.PrevPackageSubscriptionID = AppConsts.NONE;
                    View.NextAppCmpItemID = AppConsts.NONE;
                    View.PrevAppCmpItemID = AppConsts.NONE;
                }
                View.ApplicantComplianceCategoryDataList = ComplianceDataManager.GetApplicantComplianceCategoryData(View.SelectedPackageSubscriptionID_Global, View.TenantId_Global);

                if (View.OrganizationUserData != null && View.OrganizationUserData.OrganizationUserID != View.CurrentApplicantId_Global)
                {
                    View.OrganizationUserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(View.CurrentApplicantId_Global);
                    //UAT-749:WB: Addition of "User Groups" to left panel of Verification Details screen
                    View.UserGroupDataList = ComplianceDataManager.GetUserGroupsForUser(View.TenantId_Global, View.CurrentApplicantId_Global);
                }
                View.IsRushOrder = (ApplicantComplianceItemDataList.RushOrderStatus == null) ? false : true;
                View.SelectedOrderId = ApplicantComplianceItemDataList.OrderID;
                View.SelectedOrderNumber = ApplicantComplianceItemDataList.OrderNumber;
                List<OrderPkgPaymentDetail> lstOrderPkgPaymentDetail = GetOrderPkgPaymentDetail();
                String compOrderPkgTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
                String compRushOrderPkgTypeCode = OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue();
                String orderPkgTypeToGet = View.IsRushOrder ? compRushOrderPkgTypeCode : compOrderPkgTypeCode;
                OrderPaymentDetail complianceOrderPaymentDetail = new OrderPaymentDetail();
                if (!lstOrderPkgPaymentDetail.IsNullOrEmpty())
                {
                    complianceOrderPaymentDetail = lstOrderPkgPaymentDetail.FirstOrDefault(x => x.lkpOrderPackageType.OPT_Code == orderPkgTypeToGet).OrderPaymentDetail;
                }

                if (!complianceOrderPaymentDetail.IsNullOrEmpty())
                {
                    View.OrderApprovalDate = complianceOrderPaymentDetail.OPD_ApprovalDate.HasValue ? complianceOrderPaymentDetail.OPD_ApprovalDate.Value : (DateTime?)null;
                }
            }
        }

        private List<OrderPkgPaymentDetail> GetOrderPkgPaymentDetail()
        {
            if (View.SelectedOrderId > AppConsts.NONE)
            {
                return ComplianceDataManager.GetOrderPkgPaymentDetailsByOrderID(View.TenantId_Global, View.SelectedOrderId);
            }
            return new List<OrderPkgPaymentDetail>();
        }



        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Entity.Tenant GetTenant(Int32 tenantID)
        {
            if (View.OrganizationUserData != null && View.OrganizationUserData.OrganizationUserID != tenantID)
                return SecurityManager.GetOrganizationUser(tenantID).Organization.Tenant;
            return View.OrganizationUserData.Organization.Tenant;
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.LoggedInUser.TenantID;
            }
        }

        public Boolean IsThirdPartyTenant
        {
            get
            {
                return SecurityManager.IsTenantThirdPartyType(View.LoggedInUser.TenantID, TenantType.Compliance_Reviewer.GetStringValue());
            }
        }

        //UAT-738
        #region GET ASSIGNED ITEM LIST BY CATEGORY
        public List<Int32> GetCategoryListOfAssignedItems()
        {
            //Commented out code after remove status checks from GetCategoryListForAssignedItem
            //Boolean? isExceptionUserWorkQueue = null;
            //if (View.IsException)
            //    isExceptionUserWorkQueue = true;

            if (View.WorkQueue == WorkQueueType.UserWorkQueue || View.WorkQueue == WorkQueueType.EsclationUserWorkQueue || (View.WorkQueue == WorkQueueType.UserWorkQueue && View.IsException))
            {
                return ComplianceDataManager.GetCategoryListForAssignedItem(View.TenantId_Global, View.SelectedPackageSubscriptionID_Global, View.CurrentLoggedInUserId);
            }
            return new List<Int32>();
        }

        #endregion

        public string GetFormattedPhoneNumber(string phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public void GetGranularPermissions()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (Business.RepoManagers.SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                //UAT-806
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = dicPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }
            }
        }

        #endregion

        /// <summary>
        /// Get the Current applicant data, including his address.
        /// </summary>
        public void GetApplicantData()
        {
            View.ApplicantData = ComplianceDataManager.GetUserData(View.CurrentApplicantId_Global, View.TenantId_Global);
        }


        public String GetApplicantSSN()
        {
            return SecurityManager.GetFormattedString(View.OrganizationUserData.OrganizationUserID, false);
        }
         
        #region UAT-2460
        private List<Int32> GetArchiveStateId()
        {
            if (!View.SelectedArchiveStateCode.IsNullOrEmpty())
            {
                return new List<Int32>{
                    ComplianceDataManager.GetArchiveStateList(View.TenantId_Global).FirstOrDefault(x => x.AS_Code.Equals(View.SelectedArchiveStateCode)).AS_ID};
            }
            return new List<Int32>();

        }
        private String GetXMLString(List<Int32> listOfIds)
        {
            if (listOfIds.IsNotNull() && listOfIds.Count > 0)
            {
                StringBuilder IdString = new StringBuilder();
                foreach (Int32 id in listOfIds)
                {
                    IdString.Append("<Root><Value>" + id.ToString() + "</Value></Root>");
                }

                return IdString.ToString();
            }
            return null;
        }
        #endregion

        public ClientSetting GetClientSettingByCode()
        {
            return ComplianceDataManager.GetClientSetting(View.TenantId_Global, Setting.EXECUTE_COMPLIANCE_RULE_WHEN_OPTIONAL_CATEGORY_COMPLIANCE_RULE_MET.GetStringValue());
        }

        public Boolean GetOptionalCategorySettingNode()
        {
            return ComplianceDataManager.GetOptionalCategorySettingForNode(View.TenantId_Global, AppConsts.NONE, View.SelectedPackageSubscriptionID_Global, SubscriptionTypeCategorySetting.COMPLIANCE_PACKAGE.GetStringValue());
        }
    }
}



