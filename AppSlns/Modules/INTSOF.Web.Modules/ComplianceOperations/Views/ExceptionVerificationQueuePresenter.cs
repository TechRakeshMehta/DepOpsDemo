using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Linq;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using WebSiteUtils.SharedObjects;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ExceptionVerificationQueuePresenter : Presenter<IExceptionVerificationQueueView>
    {

        /// <summary>
        /// Executes the code at the time of page load.
        /// </summary>
        public override void OnViewLoaded()
        {
            GetExceptionQueueData();
        }

        /// <summary>
        /// Executes the code when page is initialised.
        /// </summary>
        public override void OnViewInitialized()
        {
            String tenantType = GetTenantType();
            if (SecurityManager.DefaultTenantID == View.TenantId || tenantType == TenantType.Institution.GetStringValue())
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
        public void GetExceptionQueueData()
        {
            if (SecurityManager.IsTenantThirdPartyType(View.TenantId, TenantType.Compliance_Reviewer.GetStringValue()))
            {
                View.lstExceptionQueue = null;
                return;
            }
            Int32 clientId = View.TenantId;
            ItemVerificationQueueData verificationQueueData = GetVerificationQueueData(ref clientId);

            View.ExceptionGridCustomPaging.DefaultSortExpression = "ApplicantComplianceItemId";
            View.ExceptionGridCustomPaging.SecondarySortExpression = QueueConstants.DEFAULT_SORTING_FIELDS_ASSIGNMENT;

            if (!((IsDefaultTenant || IsThirdPartyTenant) && View.SelectedTenantId == 0))
            {
                try
                {
                    //Code that gets the data
                    View.lstExceptionQueue = ComplianceDataManager.GetApplicantComplianceItemData(clientId, verificationQueueData, View.ExceptionGridCustomPaging, View.CustomDataXML, View.DPMIds);
                    View.VirtualPageCount = View.ExceptionGridCustomPaging.VirtualPageCount;
                    View.CurrentPageIndex = View.ExceptionGridCustomPaging.CurrentPageIndex;
                }
                catch (Exception e)
                {
                    View.lstExceptionQueue = new List<ApplicantComplianceItemDataContract>();
                    throw e;
                }
            }
        }

        private ItemVerificationQueueData GetVerificationQueueData(ref int clientId)
        {
            ItemVerificationQueueData verificationQueueData = new ItemVerificationQueueData();

            List<String> lstStatusCode = new List<String>();
            String reviewerTypeCode = String.Empty;
            Int32 reviewerId = 0;

            lstStatusCode.Add(ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue());
            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if (lstStatusCode.Count > 0)
            {
                verificationQueueData.lstStatusCode = lstStatusCode.Select(x => new StatusCode { statusCode = x }).ToList();
            }
            verificationQueueData.ReviewerType = reviewerTypeCode;
            verificationQueueData.ReviewerId = reviewerId;
            verificationQueueData.SelectedUserGroupId = View.SelectedUserGroupId;
            if (View.SelectedWorkQueueType == WorkQueueType.ExceptionAssignmentWorkQueue)
            {
                verificationQueueData.AssignedToUserId = AppConsts.MINUS_ONE;
            }
            else if (View.SelectedWorkQueueType == WorkQueueType.ExceptionUserWorkQueue)
            {
                verificationQueueData.AssignedToUserId = View.CurrentLoggedInUserId;
            }
            verificationQueueData.IsDefaultOrThrdPrty = false;
            verificationQueueData.ShowIncompleteItems = false;
            verificationQueueData.BussinessProcessId = 1;
            verificationQueueData.selectedPackageId = View.SelectedPackageId;
            verificationQueueData.CategoryId = View.SelectedCategoryId;
            verificationQueueData.CurrentLoggedInUser = View.CurrentLoggedInUserId;
            verificationQueueData.ShowOnlyRushOrder = View.ShowOnlyRushOrders;
            if ((IsDefaultTenant || IsThirdPartyTenant) && View.SelectedWorkQueueType == WorkQueueType.ExceptionAssignmentWorkQueue)
            {
                if (View.SelectedTenantId != 0)
                {
                    clientId = View.SelectedTenantId;
                    verificationQueueData.QueueId = LookupManager.GetLookUpData<QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals("AAAD")).FirstOrDefault().QMD_QueueID.ToString();
                }
                verificationQueueData.IsDefaultOrThrdPrty = true;
            }
            else if (IsDefaultTenant && View.SelectedWorkQueueType == WorkQueueType.ExceptionUserWorkQueue)
            {
                if (View.SelectedTenantId != 0)
                {
                    clientId = View.SelectedTenantId;
                    verificationQueueData.QueueId = LookupManager.GetLookUpData<QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals("AAAD")).FirstOrDefault().QMD_QueueID.ToString();
                }
                verificationQueueData.IsDefaultOrThrdPrty = true;
            }
            else
            {
                verificationQueueData.QueueId = LookupManager.GetLookUpData<QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals("AAAE")).FirstOrDefault().QMD_QueueID.ToString();
            }
            if (!View.QueueCode.IsNullOrEmpty() && (clientId != SecurityManager.DefaultTenantID))
            {
                View.IsEscalationRecords = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == View.QueueCode).FirstOrDefault().QMD_IsEscalationQueue;
                verificationQueueData.IsEscalationRecords = View.IsEscalationRecords;
                verificationQueueData.QueueId = Convert.ToString(LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == View.QueueCode).FirstOrDefault().QMD_QueueID);

            }

            return verificationQueueData;
        }

        public Boolean AssignItemsToUser(Dictionary<int, bool> selectedItems, List<KeyValuePair<Int32, Int32>> selectedNodes, Int32 selectedUserId)
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
                    List<Int32> userNodePermissions = ComplianceDataManager.GetUserNodePermissionBasedOnHierarchyPermissionType(clientId, selectedUserId,
                            clientId, HierarchyPermissionTypes.COMPLIANCE.GetStringValue())
                             .Select(x => x.DPM_ID).ToList();
                    foreach (var node in selectedNodes)
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
                Tuple<List<Int32>, String> idsUnAssigned = ComplianceDataManager.AssignItemsToUserNew(clientId, mainDataXML, View.CurrentLoggedInUserId, selectedUserId, true);

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
                List<Entity.OrganizationUser> lstAllOrganizationUser = new List<Entity.OrganizationUser>();
                lstAllOrganizationUser = SecurityManager.GetOganisationUsersByTanentId(clientId, IsDefaultTenant, true, false, false, false).Select(x => new Entity.OrganizationUser
                  {
                      FirstName = x.FirstName + " " + x.LastName + " (ADB Admin)",
                      OrganizationUserID = x.OrganizationUserID
                  }).ToList();


                //UAT-3650
                List<Entity.OrganizationUser> lstClientAdminOrganizationUser = new List<Entity.OrganizationUser>();
                lstClientAdminOrganizationUser = ComplianceDataManager.GetUsersForAssignment(View.SelectedTenantId, null, View.SelectedTenantId).Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName + " (Client Admin)",
                    OrganizationUserID = x.OrganizationUserID
                }).ToList();

                lstAllOrganizationUser.AddRange(lstClientAdminOrganizationUser);
                View.lstOrganizationUser = lstAllOrganizationUser;
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

        public void GetAllVerificationItems()
        {
            Int32 clientId = View.TenantId;
            ItemVerificationQueueData verificationQueueData = GetVerificationQueueData(ref clientId);

            View.ExceptionGridCustomPaging.DefaultSortExpression = "ApplicantComplianceItemId";
            View.ExceptionGridCustomPaging.SecondarySortExpression = QueueConstants.DEFAULT_SORTING_FIELDS_ASSIGNMENT;
            View.ExceptionGridCustomPaging.PageSize = View.VirtualPageCount;

            if (!((IsDefaultTenant || IsThirdPartyTenant) && View.SelectedTenantId == 0))
            {
                //Code that gets the data
                List<ApplicantComplianceItemDataContract> lstApplicantComplianceItemDataContract = ComplianceDataManager.GetApplicantComplianceItemData(clientId, verificationQueueData, View.ExceptionGridCustomPaging, View.CustomDataXML, View.DPMIds);

                View.SelectedVerificationItemsNew = new List<AssignQueueRecords>();

                if (!lstApplicantComplianceItemDataContract.IsNullOrEmpty())
                {
                    View.SelectedVerificationItemsNew = lstApplicantComplianceItemDataContract
                        .Select(item => new AssignQueueRecords
                        {
                            ApplicantComplienceItemId = item.ApplicantComplianceItemId,
                            ComplianceItemId = item.ComplianceItemId,
                            CategoryId = item.CategoryId,
                            verificationStatusCode = item.VerificationStatusCode,
                            ItemName = item.ItemName,
                            IsChecked = true,
                            workQueueType = View.SelectedWorkQueueType.ToString(),
                            IsEsclationRecord = View.IsEscalationRecords,
                            QueueCode = View.QueueCode,
                            IsDefaultThirdPartyTenant = (!(IsDefaultTenant || IsThirdPartyTenant)) ? false : true,
                            tenantID = (!(IsDefaultTenant || IsThirdPartyTenant)) ? View.TenantId : View.SelectedTenantId,
                        }).ToList();
                }
            }
        }

        public void GetSelectedNodeIDBySubscriptionID(String selectedtenantID, String packageSubscriptionID)
        {
            Int32 tenantID = Convert.ToInt32(selectedtenantID);
            Int32 pkgSubscriptionID = Convert.ToInt32(packageSubscriptionID);
            var lstSelectedNodeIDForOrders = ComplianceDataManager.GetSelectedNodeIDBySubscriptionID(tenantID, pkgSubscriptionID);
            View.selectedNodeIDs = lstSelectedNodeIDForOrders.Where(x => !x.IsDeleted).DistinctBy(x => x.Order.SelectedNodeID).Select(x => x.Order.SelectedNodeID ?? 0).ToList();
        }
        public void GetAllowedFileExtensions(String tenantID)
        {
            Int32 tenantId = Convert.ToInt32(tenantID);
            String selectedNodeIDs = String.Join(",", View.selectedNodeIDs);
            var lstAllowedFileExtensions = ComplianceDataManager.GetAllowedFileExtensionsByNodeIDs(tenantId, selectedNodeIDs);
            View.allowedFileExtensions = lstAllowedFileExtensions.Select(x => x.Name).ToList();
        }

    }
}




