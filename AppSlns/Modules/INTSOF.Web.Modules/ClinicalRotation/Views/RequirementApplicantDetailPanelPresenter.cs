using Business.RepoManagers;
using INTSOF.Contracts;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.ApplicantOperations;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RequirementApplicantDetailPanelPresenter : Presenter<IRequirementApplicantDetailPanel>
    {
        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private ApplicantClinicalRotationProxy _applicantClinicalRotationProxy
        {
            get
            {
                return new ApplicantClinicalRotationProxy();
            }
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.LoggedInUser.TenantID;
            }
        }

        public override void OnViewLoaded()
        {

        }

        /// <summary>
        /// call when View Initialized
        /// </summary>
        public override void OnViewInitialized()
        {
            GetApplicantRotationData();
            GetGranularPermissions();
        }

        /// <summary>
        /// Get Rotation deatils of applicant
        /// </summary>
        public void GetApplicantRotationData()
        {
            ServiceRequest<Int32, Int32?> serviceRequest = new ServiceRequest<Int32, Int32?>();
            serviceRequest.SelectedTenantId = View.SelectedTenantID;
            serviceRequest.Parameter1 = View.ClinicalRotationID;
            serviceRequest.Parameter2 = null;
            var _serviceResponse = _clientRotationProxy.GetClinicalRotationById(serviceRequest);
            View.RotationDeatils = _serviceResponse.Result;
            View.OrganizationUserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(View.CurrentApplicantID);
        }

        public Boolean IsAdminLoggedIn()
        {
            return (SecurityManager.DefaultTenantID == View.TenantID);
        }

        public ManageReqPkgSubscriptionContract GetReqPkgSubscriptionIdList(RequirementVerificationQueueContract searchDataContract, Int32 CurrentReqPkgSubscriptionID, Int32 ApplicantRequirementItemID, Boolean isRedirectedFromRequirementVerification)
        {
            //Note:-Submission Date in Rotation Verification Queue is wrong.So, We may have ticket in future to fix - submission date issue then save and next and save and previous navigation should be test and updated as well.
            ManageReqPkgSubscriptionContract objManageReqPkgSubscriptionContract = new ManageReqPkgSubscriptionContract();


            List<ReqPkgSubscriptionIDList> result = new List<ReqPkgSubscriptionIDList>();
            if (isRedirectedFromRequirementVerification && searchDataContract.TenantID > AppConsts.NONE)
            {
                searchDataContract.LoggedInUserId = IsAdminLoggedIn() ? AppConsts.NONE : searchDataContract.LoggedInUserId;
                //searchDataContract.IsCurrentRotation = true; //UAT-4465
                result = ComplianceDataManager.GetReqPkgSubscriptionIdList(searchDataContract, CurrentReqPkgSubscriptionID, ApplicantRequirementItemID, searchDataContract.TenantID);
            }
            else
            {
                //searchDataContract.IsCurrentRotation = true; //UAT-4465
                ServiceRequest<RequirementVerificationQueueContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementVerificationQueueContract, Int32, Int32>();
                serviceRequest.Parameter1 = searchDataContract;
                serviceRequest.Parameter2 = CurrentReqPkgSubscriptionID;
                serviceRequest.Parameter3 = ApplicantRequirementItemID;
                result = _clientRotationProxy.GetReqPkgSubscriptionIdList(serviceRequest).Result;
            }
            if (result.Count == AppConsts.THREE)
            {
                objManageReqPkgSubscriptionContract.PreviousSubscription = result[0];
                objManageReqPkgSubscriptionContract.CurrentSubscription = result[1];
                objManageReqPkgSubscriptionContract.NextSubscription = result[2];
            }
            else if (result.Count == AppConsts.TWO)
            {
                if (result[0].NextApplicantRequirementItemId > AppConsts.NONE)
                {
                    objManageReqPkgSubscriptionContract.PreviousSubscription = null;
                    objManageReqPkgSubscriptionContract.CurrentSubscription = result[0];
                    objManageReqPkgSubscriptionContract.NextSubscription = result[1];
                }
                else
                {
                    objManageReqPkgSubscriptionContract.PreviousSubscription = result[0];
                    objManageReqPkgSubscriptionContract.CurrentSubscription = result[1];
                    objManageReqPkgSubscriptionContract.NextSubscription = null;
                }
            }
            else if (result.Count == AppConsts.ONE)
            {
                if (CurrentReqPkgSubscriptionID == result[0].RequirementPackageSubscriptionID && result[0].ApplicantRequirementItemId == ApplicantRequirementItemID)
                {
                    objManageReqPkgSubscriptionContract.PreviousSubscription = null;
                    objManageReqPkgSubscriptionContract.CurrentSubscription = result[0];
                    objManageReqPkgSubscriptionContract.NextSubscription = null;
                }
            }
            else
            {
                objManageReqPkgSubscriptionContract.PreviousSubscription = null;
                objManageReqPkgSubscriptionContract.CurrentSubscription = null;
                objManageReqPkgSubscriptionContract.NextSubscription = null;
            }
            //searchDataContract.IsCurrentRotation = false; //UAT-4465
            return objManageReqPkgSubscriptionContract;
        }

        public void GetReqPkgSubscriptionIdListForRotationVerification(RequirementVerificationQueueContract searchDataContract, Int32 CurrentReqPkgSubscriptionID, Int32 ApplicantRequirementItemID, Int32 TenantId)
        {
            Int32 pageIndex = 0; // View.SubPageIndex;
            Int32 totalPages = 0; // View.SubTotalPages;
            ServiceRequest<RequirementVerificationQueueContract, Int32, Int32, Int32> serviceRequest =
                           new ServiceRequest<RequirementVerificationQueueContract, Int32, Int32, Int32>();
            serviceRequest.Parameter1 = searchDataContract;
            serviceRequest.Parameter2 = CurrentReqPkgSubscriptionID;
            serviceRequest.Parameter3 = ApplicantRequirementItemID;
            serviceRequest.Parameter4 = TenantId;
            View.lstReqPkgsubscriptionIdList = new List<ReqPkgSubscriptionIDList>();
            if (HttpContext.Current.Session["CurentPackageSubscriptionID"] == null
                        || Convert.ToInt32(HttpContext.Current.Session["CurentPackageSubscriptionID"]) != View.SelectedPackageSubscriptionID_Global)
            {
                View.lstReqPkgsubscriptionIdList = _clientRotationProxy.GetReqPkgSubscriptionIdListForRotationVerification(serviceRequest).Result;
                View.SubPageIndex = pageIndex;
                View.SubTotalPages = totalPages;
                if (View.SelectedPackageSubscriptionID_Global == AppConsts.MINUS_TWO)
                    View.SelectedPackageSubscriptionID_Global = View.lstReqPkgsubscriptionIdList.LastOrDefault().RequirementPackageSubscriptionID; // ?? 0;
                else if (View.SelectedPackageSubscriptionID_Global <= AppConsts.NONE)
                    View.SelectedPackageSubscriptionID_Global = View.lstReqPkgsubscriptionIdList.FirstOrDefault().RequirementPackageSubscriptionID; // ?? 0;

                HttpContext.Current.Session["CurentPackageSubscriptionID"] = View.SelectedPackageSubscriptionID_Global;
                HttpContext.Current.Session["CurrentSubscriptionIDList"] = View.lstReqPkgsubscriptionIdList;
            }

            if (HttpContext.Current.Session["CurentPackageSubscriptionID"] != null
                        && Convert.ToInt32(HttpContext.Current.Session["CurentPackageSubscriptionID"]) == View.SelectedPackageSubscriptionID_Global)
            {
                View.lstReqPkgsubscriptionIdList = (List<ReqPkgSubscriptionIDList>)(HttpContext.Current.Session["CurrentSubscriptionIDList"]);
            }
            //else
            //{

            //}
            if (View.lstReqPkgsubscriptionIdList.Count > 1)
            {
                for (int i = 0; i < View.lstReqPkgsubscriptionIdList.Count; i++)
                {
                    if (View.lstReqPkgsubscriptionIdList[i].RequirementPackageSubscriptionID == View.SelectedPackageSubscriptionID_Global)
                    {
                        View.ItemDataId_Global = View.lstReqPkgsubscriptionIdList[i].ApplicantRequirementItemId;
                        //UAT-755: Clicking previous or next button on left panel of verification details should become gray if admin has navigated away from the first/last student (and has completed all assigned verifications) in the queue 
                        if (i == AppConsts.NONE)
                        {
                            View.PrevReqPackageSubscriptionID = AppConsts.NONE;
                            View.PrevAppReqItemID = AppConsts.NONE;
                            View.PrevRotationID = AppConsts.NONE;
                        }
                        else
                        {
                            View.PrevReqPackageSubscriptionID = View.lstReqPkgsubscriptionIdList[i - 1].RequirementPackageSubscriptionID; //?? 0;
                            View.PrevAppReqItemID = View.lstReqPkgsubscriptionIdList[i - 1].ApplicantRequirementItemId;
                            View.PrevRotationID = View.lstReqPkgsubscriptionIdList[i - 1].RotationId;
                            View.PrevCategoryID = View.lstReqPkgsubscriptionIdList[i - 1].RequirementCategoryID;
                            View.PrevTenantID = View.SelectedTenantID;
                            View.PrevApplicantID = View.lstReqPkgsubscriptionIdList[i - 1].ApplicantId;
                        }

                        if ((i + 1) < View.lstReqPkgsubscriptionIdList.Count)
                        {
                            View.NextReqPackageSubscriptionID = View.lstReqPkgsubscriptionIdList[i + 1].RequirementPackageSubscriptionID; // ?? 0;
                            View.NextAppReqItemID = View.lstReqPkgsubscriptionIdList[i + 1].ApplicantRequirementItemId;
                            View.NextRotationID = View.lstReqPkgsubscriptionIdList[i + 1].RotationId;
                            View.NextCategoryID = View.lstReqPkgsubscriptionIdList[i + 1].RequirementCategoryID;
                            View.NextApplicantID = View.lstReqPkgsubscriptionIdList[i + 1].ApplicantId;
                            View.NextTenantID = View.SelectedTenantID;
                        }
                        else
                        {
                            View.NextReqPackageSubscriptionID = AppConsts.NONE;
                            View.NextAppReqItemID = AppConsts.NONE;
                            View.NextRotationID = AppConsts.NONE;
                        }

                    }
                }
            }
            else
            {
                View.NextReqPackageSubscriptionID = AppConsts.NONE;
                View.PrevReqPackageSubscriptionID = AppConsts.NONE;
                View.NextAppReqItemID = AppConsts.NONE;
                View.PrevAppReqItemID = AppConsts.NONE;
            }
        }
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

        /// <summary>
        /// Get the Current applicant data, including his address.
        /// </summary>
        public void GetApplicantData()
        {
            View.ApplicantData = ComplianceDataManager.GetUserData(View.CurrentApplicantID, View.SelectedTenantID);
        }

        public string GetFormattedPhoneNumber(string phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
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

        public String GetApplicantSSN()
        {
            return SecurityManager.GetFormattedString(View.OrganizationUserData.OrganizationUserID, false);
        }

        /// <summary>
        /// Returns Tenant Name.
        /// </summary>
        public string TenantName
        {
            get
            {
                Boolean SortByName = true;
                String clientCode = TenantType.Institution.GetStringValue();
                ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
                serviceRequest.Parameter1 = SortByName;
                serviceRequest.Parameter2 = clientCode;
                var _serviceResponse = _clientRotationProxy.GetTenants(serviceRequest);
                return _serviceResponse.Result.Where(cond => cond.TenantID == View.SelectedTenantID).Select(col => col.TenantName).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get the Requirement package category details.
        /// </summary>
        public void GetRequirementPackageCategoryData()
        {
            View.lstReqPkgSubData = RequirementVerificationManager.GetRequirementPackageCategoryData(View.ReqPkgSubsciptionID, View.SelectedTenantID, View.ClinicalRotationID);
            //UAT-2165: Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
            SetComplianceRequiredForCategory();
        }

        public Dictionary<Boolean, String> ApproveAllPendingItems()
        {
            #region  UAT-3273- Get status before rule execution
            var dataBeforeRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(View.SelectedTenantID, Convert.ToString(View.ReqPkgSubsciptionID));
            #endregion

            ServiceRequest<Int32, Int32,Boolean> serviceRequest = new ServiceRequest<Int32, Int32,Boolean>();
            serviceRequest.SelectedTenantId = View.SelectedTenantID;
            serviceRequest.Parameter1 = View.ReqPkgSubsciptionID;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            serviceRequest.Parameter3 = IsAdminLoggedIn();

            //UAT-3805
            List<Int32> approvedCategoryIDs = new List<Int32>();
            List<ItemDocNotificationRequestDataContract> itemDocNotificationRequestData = new List<ItemDocNotificationRequestDataContract>();

            approvedCategoryIDs = ProfileSharingManager.GetApprovedCategorIDs(View.SelectedTenantID, View.ReqPkgSubsciptionID
                                                                                  , new List<Int32>(), lkpUseTypeEnum.ROTATION.GetStringValue());

            Int32 affectedItemsCount = 0;
            var _serviceResponse = _clientRotationProxy.RotationSubscriptionApproveAllPendingItems(serviceRequest, ref affectedItemsCount);
            List<Int32> lstcategory = _serviceResponse.Result.Item1;
            //UAT-2366
            Boolean isSuccess = !_serviceResponse.Result.Item3.Keys.FirstOrDefault();
            if (!IsAdminLoggedIn())
            {
                View.PendingItemNames = _serviceResponse.Result.Item2;
            }
            if (isSuccess)
            {
                View.AffectedItemsCount = affectedItemsCount;
                SendNotificationsForItmAndPkgStatusChange(lstcategory);

                //UAT-2975
                SyncVerificationDataToFlat();

                #region  UAT-3273- Get status after rule execution
                //var dataAfterRuleExecution = _applicantClinicalRotationProxy.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(request);
                var dataAfterRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(View.SelectedTenantID, Convert.ToString(View.ReqPkgSubsciptionID));
                ProfileSharingManager.CheckAndSendNotificationForNewlyApprovedRotations(dataBeforeRuleExecution, dataAfterRuleExecution, View.SelectedTenantID);
                #endregion

                ItemDocNotificationRequestDataContract itemDocRequestData = new ItemDocNotificationRequestDataContract();

                if (!lstcategory.IsNullOrEmpty())
                {
                    String categoryIDs = String.Join(",", lstcategory);

                    String approvedCatIds = approvedCategoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", approvedCategoryIDs);

                    itemDocRequestData.TenantID = View.SelectedTenantID;
                    itemDocRequestData.CategoryIds = categoryIDs;
                    //itemDocRequestData.ApplicantOrgUserID = View.ApplicantId;
                    itemDocRequestData.ApprovedCategoryIds = approvedCatIds;
                    itemDocRequestData.RequestTypeCode = lkpUseTypeEnum.ROTATION.GetStringValue();
                    itemDocRequestData.PackageSubscriptionID = null;
                    itemDocRequestData.RPS_ID = View.ReqPkgSubsciptionID;
                    itemDocRequestData.CurrentLoggedInUserID = View.CurrentLoggedInUserId;
                    itemDocNotificationRequestData.Add(itemDocRequestData);

                    //UAT-3805
                    var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                    Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                    dicParam.Add("CategoryData", itemDocNotificationRequestData);
                    ProfileSharingManager.RunParallelTaskItemDocNotificationOnCatApproval(dicParam, LoggerService, ExceptiomService);
                }

                //UAT-4015
                ApplicantRequirementManager.SendMailToInstPrecpReqPKgCompliantStatus(View.SelectedTenantID, View.ReqPkgSubsciptionID,View.CurrentLoggedInUserId);
            }
            return _serviceResponse.Result.Item3;
        }


        private void SendNotificationsForItmAndPkgStatusChange(List<Int32> lstCategory)
        {
            List<RequirementVerificationDetailContract> lstCategoryData = new List<RequirementVerificationDetailContract>();

            ServiceRequest<Int32, List<Int32>, Int32, Int32> serviceRequest = new ServiceRequest<Int32, List<Int32>, Int32, Int32>();
            serviceRequest.Parameter1 = View.ReqPkgSubsciptionID;
            serviceRequest.Parameter2 = lstCategory;
            serviceRequest.Parameter3 = View.SelectedTenantID;
            serviceRequest.Parameter4 = View.ClinicalRotationID;
            var _serviceResponse = _clientRotationProxy.GetRequirementItemsByCategoryId(serviceRequest);
            lstCategoryData = _serviceResponse.Result;

            if (!lstCategoryData.IsNullOrEmpty())
            {
                var _lstData = lstCategoryData;

                var _distinctCatIds = _lstData.Select(vdd => vdd.CatId).Distinct().ToList();
                foreach (var catId in _distinctCatIds)
                {
                    var _distinctItems = _lstData.Where(vdd => vdd.CatId == catId).DistinctBy(cond => cond.ItemId).ToList();
                    foreach (var _item in _distinctItems)
                    {
                        ComplianceDataManager.SendNotificationOnItemStatusChangedToReviewStatus(true, View.SelectedTenantID, View.ReqPkgSubsciptionID, AppConsts.NONE, catId,
                                                                                                _item.ItemId, View.CurrentLoggedInUserId, View.CurrentApplicantID, _item.ItemStatusCode);
                    }
                }

                ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(View.SelectedTenantID, View.ReqPkgSubsciptionID, View.CurrentLoggedInUserId, lstCategoryData[0].PkgStatusCode);
            }
        }

        #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        private void SetComplianceRequiredForCategory()
        {
            if (!View.lstReqPkgSubData.IsNullOrEmpty())
            {
                Int32 reqPackageID = View.lstReqPkgSubData.FirstOrDefault().PkgId;

                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.SelectedTenantId = View.SelectedTenantID;
                serviceRequest.Parameter = reqPackageID;
                var _serviceResponse = _clientRotationProxy.GetComplianceRequiredRotCatForPackage(serviceRequest);
                Dictionary<Int32, Boolean> dicComplianceRequiredCategories = _serviceResponse.Result;

                View.lstReqPkgSubData = dicComplianceRequiredCategories.IsNullOrEmpty() ? View.lstReqPkgSubData :
                                                                                         View.lstReqPkgSubData.Select(slctCat =>
                                                                                           {
                                                                                               slctCat.IsComplianceRequired = dicComplianceRequiredCategories
                                                                                               .FirstOrDefault(crMatch => crMatch.Key == slctCat.CatId).Value; return slctCat;
                                                                                           }
                                                                                         ).ToList();
            }

        }
        #endregion

        #region UAT 2371
        public void GetSystemEntityUserPermission(Int32 clientOrganisationUserID, int tenantId)
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = clientOrganisationUserID;
            serviceRequest.Parameter2 = tenantId;
            var _serviceResponse = _clientRotationProxy.GetSystemEntityUserPermission(serviceRequest);
            View.EntityPermissionName = _serviceResponse.Result.SEP_PermissionName;
        }
        #endregion

        #region UAT-2975

        public void SyncVerificationDataToFlat()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.SelectedTenantId = View.SelectedTenantID;
            serviceRequest.Parameter = View.ReqPkgSubsciptionID;

            _applicantClinicalRotationProxy.SyncRequirementVerificationToFlatData(serviceRequest);
        }
        #endregion
    }
}
