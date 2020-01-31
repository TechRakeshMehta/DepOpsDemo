using Business.RepoManagers;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebSiteUtils.SharedObjects;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageMultiTenantAssignementDataPresenter : Presenter<IManageMultiTenantAssignementDataView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads

        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
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
        ///Get Tenanat List 
        /// </summary>
        public void GetTenantList()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode); ;
        }

        /// <summary>
        /// Get Verification detail data from multiple institutions 
        /// </summary>
        public void GetMultiInstitutionAssignmentData()
        {
            String inputXml = null;

            if (View.SelectedTenantIds.IsNotNull() && View.SelectedTenantIds.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Int32 item in View.SelectedTenantIds)
                {
                    sb.Append("<TenantId>" + item.ToString() + "</TenantId>");
                }

                inputXml = "<TenantIdList>" + sb + "</TenantIdList>";
            }

            View.lstMultiInstitutionAssignmentData = SecurityManager.GetMultiInstitutionAssignmentData(inputXml, View.GridCustomPaging);


            if (View.lstMultiInstitutionAssignmentData != null && View.lstMultiInstitutionAssignmentData.Count > 0)
            {
                View.VirtualPageCount = View.GridCustomPaging.VirtualPageCount;
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualPageCount = 0;
                View.CurrentPageIndex = 1;
            }
        }

        /// <summary>
        /// Get the ADB user list 
        /// </summary>
        public void GetUserListForSelectedTenant()
        {
            Int32 clientId = View.TenantId;
            if ((IsDefaultTenant || IsThirdPartyTenant))
            {
                View.lstOrganizationUser = SecurityManager.GetOganisationUsersByTanentId(clientId, IsDefaultTenant, true, false, false, false).Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName + " " + x.LastName,
                    OrganizationUserID = x.OrganizationUserID
                }).ToList();
            }
            else
            {
                View.lstOrganizationUser = ComplianceDataManager.GetUsersForAssignment(clientId, View.CurrentLoggedInUserId, clientId).Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName,
                    OrganizationUserID = x.OrganizationUserID
                }).ToList();
            }

        }

        /// <summary>
        /// Methord Used to assignment of verification record to specific user
        /// </summary>
        /// <param name="selectedUserId"></param>
        /// <returns></returns>
        public Boolean AssignItemsToUser(Int32 selectedUserId)
        {
            List<Int32> lstFvdIds = View.FVdIdList;
            AssignQueueRecordsForMultipleTenant assignQueueRecordsForMultipleTenant = null;
            Dictionary<String, Object> dicMultipleTenantData = new Dictionary<String, Object>();

            if (View.SelectedVerificationItemsNew.Count > 0)
            {

                List<Int32> lstTenants = View.SelectedVerificationItemsNew.DistinctBy(x => x.tenantID).Select(x => x.tenantID).ToList();

                foreach (Int32 tenantId in lstTenants)
                {
                    String mainDataXML = "<Queues>";
                    View.SelectedVerificationItemsNew.ForEach(x =>
                    {
                        if (x.IsChecked && x.tenantID == tenantId)
                        {
                            //UAT 3212
                            StringBuilder queueFieldString = new StringBuilder();
                            queueFieldString.Append("<QueueDetail>");
                            queueFieldString.Append("<QueueID>" + x.queueId + "</QueueID>");
                            queueFieldString.Append("<RecordID>" + x.ApplicantComplienceItemId + "</RecordID>");
                            queueFieldString.Append("</QueueDetail>");
                            mainDataXML += queueFieldString.ToString();
                            // QueueManagementManager.GetQueueFieldXMLString(x.dicQueueFields, x.queueId, x.ApplicantComplienceItemId);
                        }
                    });

                    mainDataXML += "</Queues>";

                    assignQueueRecordsForMultipleTenant = new AssignQueueRecordsForMultipleTenant();
                    assignQueueRecordsForMultipleTenant.TenantID = tenantId;
                    assignQueueRecordsForMultipleTenant.QueueXML = mainDataXML;
                    assignQueueRecordsForMultipleTenant.AssignToUserID = selectedUserId;
                    assignQueueRecordsForMultipleTenant.CurrentLoggedInUserID = View.CurrentLoggedInUserId;

                    dicMultipleTenantData.Add(tenantId.ToString(), assignQueueRecordsForMultipleTenant);
                }

                //UAT 2809
                if (!View.IsMutipleTimesAssignmentAllowed)
                {
                    StringBuilder TenantSpecificDataXML = new StringBuilder();
                    TenantSpecificDataXML.Append("<Mappings>");
                    View.SelectedVerificationItemsNew.ForEach(x =>
                    {
                        TenantSpecificDataXML.Append("<Mapping>");
                        TenantSpecificDataXML.Append("<TenantID>" + x.tenantID + "</TenantID>");
                        TenantSpecificDataXML.Append("<AppComplianceItemID>" + x.ApplicantComplienceItemId + "</AppComplianceItemID>");
                        TenantSpecificDataXML.Append("</Mapping>");
                    });
                    TenantSpecificDataXML.Append("</Mappings>");

                    String ErrorMessage = String.Empty;
                    ErrorMessage = ComplianceDataManager.CheckIfUserAlreadyAssigned(TenantSpecificDataXML.ToString(), selectedUserId);

                    if (!ErrorMessage.IsNullOrEmpty())
                    {
                        View.ErrorMessage = ErrorMessage;
                        View.IsUserAlreadyAssigned = true;
                        return false;
                    }
                }

                return ComplianceDataManager.AsignItemToUserForMultipleTenant(dicMultipleTenantData, View.FVdIdList, View.CurrentLoggedInUserId, selectedUserId);
            }
            return false;
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
                objAutoAssignItemsToUserListContract.TenantIds = View.SelectedTenantIds;
                #endregion



                #region Records XML
                String inputXml = null;

                if (View.SelectedTenantIds.IsNotNull() && View.SelectedTenantIds.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Int32 item in View.SelectedTenantIds)
                    {
                        sb.Append("<TenantId>" + item.ToString() + "</TenantId>");
                    }

                    inputXml = "<TenantIdList>" + sb + "</TenantIdList>";
                }
                View.GridCustomPaging.PageSize = AppConsts.MINUS_ONE;

                //  var recordList = SecurityManager.GetMultiInstitutionAssignmentData(inputXml, View.GridCustomPaging);

                //if (recordList.Count > 0)
                //{

                #region Bind Record List
                //List<AssignQueueRecords> items = new List<AssignQueueRecords>();
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
                //    //,
                //    //tenantID = View.SelectedTenantId
                //    ,
                //    IsDefaultThirdPartyTenant = true
                //    //,
                //    //IsEsclationRecord = View.IsEscalationRecords
                //    //,
                //    //QueueCode = View.QueueCode
                //}));


                //items.ForEach(x => autoAssignQueueRecordsList.Add(new AutoAssignQueueRecords
                //{
                //    ApplicantComplienceItemId = x.ApplicantComplienceItemId,
                //    dicQueueFields = x.dicQueueFields,
                //    queueId = x.queueId,
                //    isProcessed = false
                //}));

                //objAutoAssignItemsToUserListContract.autoAssignQueueRecordsList = autoAssignQueueRecordsList;
                #endregion

                //MultiTenantInputXml
                objAutoAssignItemsToUserListContract.IsMultiTenant = true;
                objAutoAssignItemsToUserListContract.MultiTenantInputXml = inputXml;
                objAutoAssignItemsToUserListContract.MultiTenantGridCustomPaging = View.GridCustomPaging;
                SecurityManager.UpdateAppConfiguration(AppConsts.Automatic_Items_Assign_To_Admin_In_Progress, String.Format("{0}|{1}", Convert.ToString(AppConsts.ONE), Convert.ToString(DateTime.Now)));
                Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
                dataDict.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                dataDict.Add("AutoAssignContract", objAutoAssignItemsToUserListContract);
                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                ParallelTaskContext.PerformParallelTask(AutomaticAssignItemsToUser, dataDict, LoggerService, ExceptiomService);
                //}
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
            }
            catch (Exception)
            {
                SecurityManager.UpdateAppConfiguration(AppConsts.Automatic_Items_Assign_To_Admin_In_Progress, String.Format("{0}|{1}", AppConsts.ZERO, Convert.ToString(DateTime.Now)));
            }

        }

        #endregion
    }
}
