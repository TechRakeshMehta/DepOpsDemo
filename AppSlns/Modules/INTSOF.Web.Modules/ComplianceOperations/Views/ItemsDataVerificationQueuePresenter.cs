using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Linq;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using INTSOF.ServiceUtil;
using System.Web;
using INTSOF.Contracts;
using INTSOF.UI.Contract.ComplianceOperation;
using WebSiteUtils.SharedObjects;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ItemsDataVerificationQueuePresenter : Presenter<IItemsDataVerificationQueueView>
    {
        /// <summary>
        /// Executes the code at the time of page load.
        /// </summary>
        public override void OnViewLoaded()
        {
            GetVerificationQueueData();
        }

        /// <summary>
        /// Executes the code when page is initialised.
        /// </summary>
        public override void OnViewInitialized()
        {
            String tenantType = GetTenantType();
            if (SecurityManager.DefaultTenantID == View.TenantId || tenantType.Equals(TenantType.Institution.GetStringValue()))
            {
                if (SecurityManager.DefaultTenantID == View.TenantId)
                    View.ShowClientDropDown = true;
                View.lstTenant = ComplianceDataManager.getClientTenant();
            }
            else if (tenantType == TenantType.Compliance_Reviewer.GetStringValue())
            {
                View.ShowClientDropDown = true;
                View.lstTenant = ComplianceDataManager.getParentTenant(View.TenantId);
            }
            else
            {
                View.lstCompliancePackage = ComplianceSetupManager.GetPermittedPackagesByUserID(View.TenantId, View.CurrentLoggedInUserId);
            }

        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        public Boolean IsThirdPartyTenant
        {
            get
            {
                return SecurityManager.IsTenantThirdPartyType(View.TenantId, TenantType.Compliance_Reviewer.GetStringValue());
            }
        }

        /// <summary>
        /// Gets the tenant Type for the looged in user.
        /// </summary>
        /// <returns></returns>
        public String GetTenantType()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.Tenant.lkpTenantType.TenantTypeCode;
        }

        /// <summary>
        /// Gets the data from table ApplicantComplianceDataItems.
        /// </summary>
        public void GetVerificationQueueData()
        {
            int clientId = 0;

            ItemVerificationQueueData verificationQueueData = GetItemVerificationQueueContract(ref clientId);

            View.VerificationGridCustomPaging.DefaultSortExpression = "ApplicantComplianceItemId";
            View.VerificationGridCustomPaging.SecondarySortExpression = QueueConstants.DEFAULT_SORTING_FIELDS_ASSIGNMENT;

            if (!((IsDefaultTenant || IsThirdPartyTenant) && View.SelectedTenantId == 0))
            {
                try
                {
                    View.lstVerificationQueue = ComplianceDataManager.GetApplicantComplianceItemData(clientId, verificationQueueData, View.VerificationGridCustomPaging, View.CustomDataXML, View.DPMIds);
                    View.VirtualPageCount = View.VerificationGridCustomPaging.VirtualPageCount;
                    View.CurrentPageIndex = View.VerificationGridCustomPaging.CurrentPageIndex;
                }
                catch (Exception e)
                {
                    View.lstVerificationQueue = new List<ApplicantComplianceItemDataContract>();
                    throw e;
                }
            }
        }

        public Boolean AssignItemsToUser(Int32 selectedUserId)
        {
            QueueRecords queueRecords = new QueueRecords();
            List<Int32> complianceItemIds = new List<Int32>();
            Int32 clientId = View.TenantId;
            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if ((IsDefaultTenant || IsThirdPartyTenant) && View.SelectedTenantId != 0)
            {
                clientId = View.SelectedTenantId;
            }

            if (View.SelectedVerificationItemsNew.Count > 0)
            {
                List<Int32> lstSelectedApplicantComplianceItemID = View.SelectedVerificationItemsNew.Select(sel => sel.ApplicantComplienceItemId).ToList(); //UAT 2809

                String mainDataXML = "<Queues>";
                View.SelectedVerificationItemsNew.ForEach(x =>
                {
                    if (x.IsChecked)
                        mainDataXML += QueueManagementManager.GetQueueFieldXMLString(x.dicQueueFields, x.queueId, x.ApplicantComplienceItemId);
                });

                mainDataXML += "</Queues>";

                if (!(IsDefaultTenant || IsThirdPartyTenant))
                {
                    List<Int32> hierarchyNodeIds = new List<Int32>();
                    //List<Int32> userNodePermissions = ComplianceDataManager.GetUserNodePermissions(clientId, selectedUserId, clientId)
                    //    .Select(x => x.DPM_ID).ToList();
                    List<Int32> userNodePermissions = ComplianceDataManager.GetUserNodePermissionBasedOnHierarchyPermissionType(clientId, selectedUserId
                        , clientId, HierarchyPermissionTypes.COMPLIANCE.GetStringValue()).Select(x => x.DPM_ID).ToList();
                    foreach (var node in View.SelectedVerificationNodes)
                    {
                        bool isInList = hierarchyNodeIds.IndexOf(node.Key) != -1;
                        if (!isInList)
                        {
                            hierarchyNodeIds.Add(node.Key);
                        }
                    }
                    if (hierarchyNodeIds.Where(cond => !userNodePermissions.Contains(cond)).ToList().Count > 0)
                    {
                        View.ErrorMessage = "User {0} does not have permission on one or more selected item(s).";
                        return false;
                    }
                }
                Tuple<List<Int32>, String> idsUnAssigned = ComplianceDataManager.AssignItemsToUserNew(clientId, mainDataXML, View.CurrentLoggedInUserId, selectedUserId, View.IsMutipleTimesAssignmentAllowed);

                if (!idsUnAssigned.Item2.IsNullOrEmpty())  //UAT 2809
                {
                    View.ErrorMessage = idsUnAssigned.Item2.ToString();
                    View.IsUserAlreadyAssigned = true;
                    return false;
                }

                if (idsUnAssigned.Item1.Count > 0)
                {
                    String item = String.Empty;

                    List<String> lstOfItms = View.SelectedVerificationItemsNew.Where(x => idsUnAssigned.Item1.Contains(x.ApplicantComplienceItemId))
                          .Select(x => x.ItemName).ToList();
                    lstOfItms.ForEach(x =>
                        item = item.IsNullOrEmpty() ? x : item + " , " + x
                        );
                    View.ErrorMessage = item + " item(s) cannot be assigned to the selected user.";
                    return false;
                }
                SetQueueImaging();
                return true;
            }
            View.ErrorMessage = String.Empty;
            return false;
        }

        public void GetUserListForSelectedTenant()
        {
            Int32 clientId = View.TenantId;
            if ((IsDefaultTenant || IsThirdPartyTenant) && View.SelectedTenantId != 0)
            {
                View.lstOrganizationUser = SecurityManager.GetOganisationUsersByTanentId(clientId, IsDefaultTenant,true,false,false,false).Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName + " " + x.LastName,
                    OrganizationUserID = x.OrganizationUserID
                }).ToList();
            }
            else
            {
                //List<AssignmentUsers> users = ComplianceDataManager.GetUsersForAssignment(clientId, View.CurrentLoggedInUserId, clientId);
                View.lstOrganizationUser = ComplianceDataManager.GetUsersForAssignment(clientId, View.CurrentLoggedInUserId, clientId).Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName,
                    OrganizationUserID = x.OrganizationUserID
                }).ToList();
            }

            //if (!((IsDefaultTenant || IsThirdPartyTenant) && View.SelectedTenantId == 0))
            //{
            //    View.lstOrganizationUser = SecurityManager.GetOganisationUsersByTanentId(clientId).Select(x => new Entity.OrganizationUser
            //        {
            //            FirstName = x.FirstName + " " + x.LastName,
            //            OrganizationUserID = x.OrganizationUserID
            //        }).ToList();
            //}
        }

        public void GetComplianceCategory()
        {
            Int32 clientId = View.TenantId;
            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if ((IsDefaultTenant || IsThirdPartyTenant) && View.SelectedTenantId != 0)
            {
                clientId = View.SelectedTenantId;
            }
            try
            {
                List<ComplianceCategory> _lstCategories = ComplianceSetupManager.GetcomplianceCategoriesByPackage(View.SelectedPackageId, clientId, false).Select(x => x.ComplianceCategory).ToList();
                _lstCategories.ForEach(cat => cat.CategoryName = String.IsNullOrEmpty(cat.CategoryLabel)
                    ? cat.CategoryName : cat.CategoryLabel);

                View.lstComplianceCategory = _lstCategories;
            }
            catch (Exception e)
            {
                View.lstComplianceCategory = new List<ComplianceCategory>();
                //throw e;
            }
        }

        public void GetCompliancePackage()
        {
            Int32 clientId = View.TenantId;
            Int32? orgUserId = null;
            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if ((IsDefaultTenant || IsThirdPartyTenant) && View.SelectedTenantId != 0)
            {
                clientId = View.SelectedTenantId;
            }
            if (!IsDefaultTenant)
            {
                orgUserId = View.CurrentLoggedInUserId;
            }
            try
            {
                View.lstCompliancePackage = ComplianceSetupManager.GetPermittedPackagesByUserID(clientId, orgUserId);
            }
            catch (Exception e)
            {
                View.lstCompliancePackage = new List<ComplaincePackageDetails>();
                //throw e;
            }
        }

        public void GetAllUserGroups()
        {
            Int32 clientId = View.TenantId;
            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if (IsDefaultTenant || IsThirdPartyTenant)
            {
                clientId = View.SelectedTenantId;
            }
            if (clientId == 0)
                View.lstUserGroup = new List<UserGroup>();
            else
            {
                //UAT-2284: User Group permisson/access and availability by node
                Int32? currentUserId = IsDefaultTenant ? (Int32?)null : View.CurrentLoggedInUserId;
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(clientId, currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
        }

        //UAT-718
        private void SetQueueImaging()
        {

            Dictionary<String, Object> dataDictionary = new Dictionary<String, Object>();
            dataDictionary.Add("TenantID", View.SelectedTenantId);
            QueueImagingManager.AsignQueueImagingRepoInstance(dataDictionary);
        }

        private ItemVerificationQueueData GetItemVerificationQueueContract(ref int clientId)
        {
            ItemVerificationQueueData verificationQueueData = new ItemVerificationQueueData();
            List<String> lstStatusCode = new List<String>();
            String reviewerTypeCode = String.Empty;
            clientId = View.TenantId;
            Int32 reviewerId = 0;
            if (clientId == SecurityManager.DefaultTenantID)
            {
                lstStatusCode.Add(ApplicantItemComplianceStatus.Pending_Review.GetStringValue());
                //Code commented for UAT - 833 : Assignment queue for ADB admins should only show pending review for admin items
                //lstStatusCode.Add(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
                reviewerTypeCode = LkpReviewerType.Admin;
            }
            else
            {
                reviewerTypeCode = LkpReviewerType.ClientAdmin;
                if (IsThirdPartyTenant)
                {
                    lstStatusCode.Add(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue());
                    reviewerId = View.TenantId;
                }
                else
                {
                    lstStatusCode.Add(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
                }
            }

            if (lstStatusCode.Count > 0)
            {
                verificationQueueData.lstStatusCode = lstStatusCode.Select(x => new StatusCode { statusCode = x }).ToList();
            }
            verificationQueueData.ReviewerType = reviewerTypeCode;
            verificationQueueData.ReviewerId = reviewerId;
            verificationQueueData.SelectedUserGroupId = View.SelectedUserGroupId;
            verificationQueueData.IsDefaultOrThrdPrty = false;
            verificationQueueData.ShowIncompleteItems = View.ShowIcompleteItems;
            verificationQueueData.AssignedToUserId = AppConsts.MINUS_ONE;
            verificationQueueData.selectedPackageId = View.SelectedPackageId;
            verificationQueueData.CategoryId = View.SelectedCategoryId;
            verificationQueueData.CurrentLoggedInUser = View.CurrentLoggedInUserId;
            verificationQueueData.BussinessProcessId = 1;


            verificationQueueData.ShowOnlyRushOrder = View.ShowOnlyRushOrders;
            View.IsVerificationGrid = true;
            //Checks if the logged in user is admin and some client is selected from the dropdown.

            if ((IsDefaultTenant || IsThirdPartyTenant))
            {
                if (View.SelectedTenantId != 0)
                {
                    clientId = View.SelectedTenantId;
                    verificationQueueData.QueueId = LookupManager.GetLookUpData<QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals("AAAA")).FirstOrDefault().QMD_QueueID.ToString();
                    if (IsThirdPartyTenant)
                        verificationQueueData.QueueId = LookupManager.GetLookUpData<QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals("AAAC")).FirstOrDefault().QMD_QueueID.ToString();
                    //verificationQueueData.QueueId = "3";
                }
                verificationQueueData.IsDefaultOrThrdPrty = true;

            }
            else //client admin
            {
                verificationQueueData.QueueId = LookupManager.GetLookUpData<QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals("AAAB")).FirstOrDefault().QMD_QueueID.ToString();
                //verificationQueueData.QueueId = "2";
            }
            if (!View.QueueCode.IsNullOrEmpty() && (clientId != SecurityManager.DefaultTenantID))
            {
                View.IsEscalationRecords = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == View.QueueCode).FirstOrDefault().QMD_IsEscalationQueue;
                verificationQueueData.IsEscalationRecords = View.IsEscalationRecords;
                verificationQueueData.QueueId = Convert.ToString(LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == View.QueueCode).FirstOrDefault().QMD_QueueID);
            }

            return verificationQueueData;
        }

        public void GetAllVerificationItems()
        {
            int clientId = 0;

            ItemVerificationQueueData verificationQueueData = GetItemVerificationQueueContract(ref clientId);

            View.VerificationGridCustomPaging.DefaultSortExpression = "ApplicantComplianceItemId";
            View.VerificationGridCustomPaging.SecondarySortExpression = QueueConstants.DEFAULT_SORTING_FIELDS_ASSIGNMENT;
            View.VerificationGridCustomPaging.PageSize = View.VirtualPageCount;

            View.SelectedVerificationItemsNew = new List<WebSiteUtils.SharedObjects.AssignQueueRecords>();

            if (!((IsDefaultTenant || IsThirdPartyTenant) && View.SelectedTenantId == 0))
            {
                List<ApplicantComplianceItemDataContract> lstApplicantComplianceItemDataContract = ComplianceDataManager.GetApplicantComplianceItemData(clientId, verificationQueueData, View.VerificationGridCustomPaging, View.CustomDataXML, View.DPMIds);

                if (!lstApplicantComplianceItemDataContract.IsNullOrEmpty())
                {
                    View.SelectedVerificationItemsNew = lstApplicantComplianceItemDataContract
                            .Select(item => new WebSiteUtils.SharedObjects.AssignQueueRecords
                            {
                                ApplicantComplienceItemId = item.ApplicantComplianceItemId,
                                ComplianceItemId = item.ComplianceItemId,
                                CategoryId = item.CategoryId,
                                verificationStatusCode = item.VerificationStatusCode,
                                ItemName = item.ItemName,
                                IsChecked = true,
                                tenantID = (!(IsDefaultTenant || IsThirdPartyTenant)) ? View.TenantId : View.SelectedTenantId,
                                IsDefaultThirdPartyTenant = (!(IsDefaultTenant || IsThirdPartyTenant)) ? false : true,
                                IsEsclationRecord = View.IsEscalationRecords,
                                QueueCode = View.QueueCode
                            }).ToList();
                }
            }
        }

        #region UAT-2310
        public void UpdateAppConfiguration(String AutomaticItemsAssignToAdminInProgress, String UpdatedValue)
        {
            SecurityManager.UpdateAppConfiguration(AutomaticItemsAssignToAdminInProgress, UpdatedValue);
        }
        public Tuple<Boolean, String> GetAppConfiguration(String AutomaticItemsAssignToAdminInProgress)
        {
            String ResponseMessage = String.Empty;
            Boolean ResponseResult = false;
            String AppConfigurationValue = SecurityManager.GetAppConfiguration(AutomaticItemsAssignToAdminInProgress).AC_Value;
            var result = AppConfigurationValue.Split('|');
            if (result[0] == AppConsts.ZERO)
            {
                ResponseResult = true;
            }
            else
            {
                DateTime OldDateTimeStamp = Convert.ToDateTime(result[1]);
                DateTime CurrentDateTimeStamp = DateTime.Now;
                Int32 ProcessIntervalMinutes = CurrentDateTimeStamp.Subtract(OldDateTimeStamp).Minutes;
                if (ProcessIntervalMinutes > AppConsts.FIFTY_NINE)
                {
                    ResponseMessage = AppConsts.Automatic_Items_Assign_To_Admin_Error_Message;
                }
            }
            return new Tuple<Boolean, String>(ResponseResult, ResponseMessage);
        }

        public void ActivateAutomaticAssignItemToUserProcessForAllTenants()
        {
            try
            {
                AutoAssignItemsToUserListContract objAutoAssignItemsToUserListContract = new AutoAssignItemsToUserListContract();
                List<AutoAssignQueueRecords> autoAssignQueueRecordsList = new List<AutoAssignQueueRecords>();

                #region Bind Tenant List
                List<Int32> tenantIds = new List<Int32>();
                tenantIds.Add(View.SelectedTenantId);
                objAutoAssignItemsToUserListContract.TenantIds = tenantIds;
                #endregion
                String inputXml = null;
                if (View.SelectedTenantId.IsNotNull())
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    sb.Append("<TenantId>" + View.SelectedTenantId.ToString() + "</TenantId>");

                    inputXml = "<TenantIdList>" + sb + "</TenantIdList>";
                }
                objAutoAssignItemsToUserListContract.MultiTenantInputXml = inputXml;
                #region Records XML
                int clientId = 0;

                ItemVerificationQueueData verificationQueueData = GetItemVerificationQueueContract(ref clientId);

                View.VerificationGridCustomPaging.DefaultSortExpression = "ApplicantComplianceItemId";
                View.VerificationGridCustomPaging.SecondarySortExpression = QueueConstants.DEFAULT_SORTING_FIELDS_ASSIGNMENT;
                View.VerificationGridCustomPaging.PageSize = Convert.ToInt32(AppConsts.MINUS_ONE);

                objAutoAssignItemsToUserListContract.clientId = clientId;
                objAutoAssignItemsToUserListContract.CustomDataXML = View.CustomDataXML;
                objAutoAssignItemsToUserListContract.VerificationGridCustomPaging = View.VerificationGridCustomPaging;
                objAutoAssignItemsToUserListContract.DPMIds = View.DPMIds;
                objAutoAssignItemsToUserListContract.verficationQueueDataXml = verificationQueueData.XML;
                //  var recordList = ComplianceDataManager.GetApplicantComplianceItemData(clientId, verificationQueueData, View.VerificationGridCustomPaging, View.CustomDataXML, View.DPMIds);
                //  if (recordList.Count > 0)
                //  {
                // List<AssignQueueRecords> items = new List<AssignQueueRecords>();
                #region Bind Record List
                //recordList.ForEach(assignQueueRecords => items.Add(new AssignQueueRecords()
                //{
                //    ApplicantComplienceItemId = assignQueueRecords.ApplicantComplianceItemId
                //    ,
                //    ComplianceItemId = assignQueueRecords.ComplianceItemId
                //    ,
                //    CategoryId = assignQueueRecords.CategoryId
                //    ,
                //    verificationStatusCode = assignQueueRecords.VerificationStatusCode
                //    ,
                //    ItemName = assignQueueRecords.ItemName
                //    ,
                //    tenantID = View.SelectedTenantId
                //    ,
                //    IsDefaultThirdPartyTenant = true
                //    ,
                //    IsEsclationRecord = View.IsEscalationRecords
                //    ,
                //    QueueCode = View.QueueCode
                //}));


                //items.ForEach(x => autoAssignQueueRecordsList.Add(new AutoAssignQueueRecords
                //{
                //    ApplicantComplienceItemId = x.ApplicantComplienceItemId,
                //    dicQueueFields = x.dicQueueFields,
                //    queueId = x.queueId,
                //    isProcessed = false
                //}));
                #endregion

                objAutoAssignItemsToUserListContract.autoAssignQueueRecordsList = autoAssignQueueRecordsList;

                SecurityManager.UpdateAppConfiguration(AppConsts.Automatic_Items_Assign_To_Admin_In_Progress, String.Format("{0}|{1}", Convert.ToString(AppConsts.ONE), Convert.ToString(DateTime.Now)));
                Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
                dataDict.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                dataDict.Add("AutoAssignContract", objAutoAssignItemsToUserListContract);
                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                ParallelTaskContext.PerformParallelTask(AutomaticAssignItemsToUser, dataDict, LoggerService, ExceptiomService);
                // AutomaticAssignItemsToUser(dataDict);
                //  }
                #endregion

            }
            catch (Exception ex)
            {
                //after sucessfully the value of key is again set to zero in AppConfiguration table in security db.
                SecurityManager.UpdateAppConfiguration(AppConsts.Automatic_Items_Assign_To_Admin_In_Progress, String.Format("{0}|{1}", AppConsts.ZERO, Convert.ToString(DateTime.Now)));
            }
        }

        private void AutomaticAssignItemsToUser(Dictionary<String, Object> data)
        {
            try
            {
                Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("CurrentLoggedInUserId"));
                AutoAssignItemsToUserListContract AutoAssignContract = (AutoAssignItemsToUserListContract)(data.GetValue("AutoAssignContract"));
                ComplianceDataManager.AutomaticAssigningItemsToUsers(currentLoggedInUserId, AutoAssignContract);
                SecurityManager.UpdateAppConfiguration(AppConsts.Automatic_Items_Assign_To_Admin_In_Progress, String.Format("{0}|{1}", AppConsts.ZERO, Convert.ToString(DateTime.Now)));
                UpdateQueueImaging();
            }
            catch (Exception)
            {
                SecurityManager.UpdateAppConfiguration(AppConsts.Automatic_Items_Assign_To_Admin_In_Progress, String.Format("{0}|{1}", AppConsts.ZERO, Convert.ToString(DateTime.Now)));
            }

        }
        public void UpdateQueueImaging()
        {
            ComplianceDataManager.UpdateQueueImaging(View.SelectedTenantId);
        }
        #endregion

        #region UAT-4067
        public void GetSelectedNodeIDBySubscriptionID(Int32 selectedtenantID, Int32 packageSubscriptionID)
        {
            var lstSelectedNodeIDForOrders = ComplianceDataManager.GetSelectedNodeIDBySubscriptionID(selectedtenantID, packageSubscriptionID);
            View.selectedNodeIDs = lstSelectedNodeIDForOrders.Where(x => !x.IsDeleted).DistinctBy(x => x.Order.SelectedNodeID).Select(x => x.Order.SelectedNodeID ?? 0).ToList();
        }
        public void GetAllowedFileExtensions()
        {
            String selectedNodeIDs = String.Join(",", View.selectedNodeIDs);
            var lstAllowedFileExtensions = ComplianceDataManager.GetAllowedFileExtensionsByNodeIDs(View.SelectedTenantId, selectedNodeIDs);
            View.allowedFileExtensions = lstAllowedFileExtensions.Select(x => x.Name).ToList();
        }
        #endregion

    }
}

