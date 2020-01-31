using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Linq;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public class UserItemsDataQueuePresenter : Presenter<IUserItemsDataQueueView>
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
        /// Gets the tenant Type for the looged in user.
        /// </summary>
        /// <returns></returns>
        public String GetTenantType()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.Tenant.lkpTenantType.TenantTypeCode;
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
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Gets the data from table ApplicantComplianceDataItems.
        /// </summary>
        public void GetVerificationQueueData()
        {
            ItemVerificationQueueData verificationQueueData = new ItemVerificationQueueData();
            List<String> lstStatusCode = new List<String>();
            Int32 clientId = View.TenantId;
            if (clientId == SecurityManager.DefaultTenantID)
            {
                lstStatusCode.Add(ApplicantItemComplianceStatus.Pending_Review.GetStringValue());
                lstStatusCode.Add(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
            }
            else
            {
                if (SecurityManager.IsTenantThirdPartyType(clientId, TenantType.Compliance_Reviewer.GetStringValue()))
                {
                    lstStatusCode.Add(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue());
                }
                else
                {
                    lstStatusCode.Add(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
                    verificationQueueData.QueueId = LookupManager.GetLookUpData<QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals("AAAB")).FirstOrDefault().QMD_QueueID.ToString();
                }
            }
            if (lstStatusCode.Count > 0)
            {
                verificationQueueData.lstStatusCode = lstStatusCode.Select(x => new StatusCode { statusCode = x }).ToList();
            }

            verificationQueueData.SelectedUserGroupId = View.SelectedUserGroupId;

            if (IsDefaultTenant || IsThirdPartyTenant)
            {
                if (View.SelectedTenantId != 0)
                {
                    clientId = View.SelectedTenantId;
                    String queue1 = LookupManager.GetLookUpData<QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals("AAAA")).FirstOrDefault().QMD_QueueID.ToString();
                    String queue2 = LookupManager.GetLookUpData<QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals("AAAB")).FirstOrDefault().QMD_QueueID.ToString();
                    verificationQueueData.QueueId = queue1 + "," + queue2;
                    if (IsThirdPartyTenant)
                        verificationQueueData.QueueId = LookupManager.GetLookUpData<QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals("AAAC")).FirstOrDefault().QMD_QueueID.ToString();
                }
                verificationQueueData.IsDefaultOrThrdPrty = true;

            }
            else// Client Admin
            {
                verificationQueueData.IsDefaultOrThrdPrty = false;
                verificationQueueData.QueueId = LookupManager.GetLookUpData<QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals("AAAB")).FirstOrDefault().QMD_QueueID.ToString();
            }
            if (!View.QueueCode.IsNullOrEmpty() && (clientId != SecurityManager.DefaultTenantID))
            {
                View.IsEscalationRecords = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == View.QueueCode).FirstOrDefault().QMD_IsEscalationQueue;
                verificationQueueData.IsEscalationRecords = View.IsEscalationRecords;
                verificationQueueData.QueueId = Convert.ToString(LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(clientId).Where(x => x.QMD_IsDeleted == false && x.QMD_Code == View.QueueCode).FirstOrDefault().QMD_QueueID);
            }
            verificationQueueData.ShowIncompleteItems = false;
            verificationQueueData.AssignedToUserId = View.CurrentLoggedInUserId;
            verificationQueueData.selectedPackageId = View.SelectedPackageId;
            verificationQueueData.CategoryId = View.SelectedCategoryId;
            verificationQueueData.CurrentLoggedInUser = View.CurrentLoggedInUserId;
            verificationQueueData.BussinessProcessId = 1;
            verificationQueueData.ShowOnlyRushOrder = View.ShowOnlyRushOrders;
            View.IsVerificationGrid = true;
            //Checks if the logged in user is admin and some client is selected from the dropdown.

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
            catch
            {
                View.lstComplianceCategory = new List<ComplianceCategory>();
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
            catch
            {
                View.lstCompliancePackage = new List<ComplaincePackageDetails>();
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
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(clientId,currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
        }

        /// <summary>
        /// TO get the code for logged user for Verification Code
        /// </summary>
        /// <returns></returns>
        public String GetQueueCode() 
        {
            String code=String.Empty;
            //ADB Admin
            if (IsDefaultTenant) 
            {
                code = QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();                  
            }   
            //For Client Admin
            else if(GetTenantType()== TenantType.Institution.GetStringValue())
            {
                code = QueueMetaDataType.Verification_Queue_For_ClientAdmin.GetStringValue();
            }
            //For Third Party
            else if(IsThirdPartyTenant)
            {
                code = QueueMetaDataType.Verification_Queue_For_Third_Party.GetStringValue();
            }
            return code;
        }
    }
}




