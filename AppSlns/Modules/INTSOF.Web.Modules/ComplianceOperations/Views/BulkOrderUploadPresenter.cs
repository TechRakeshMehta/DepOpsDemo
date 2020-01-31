using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public class BulkOrderUploadPresenter : Presenter<IBulkOrderUploadView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantID;
            }
        }

        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.LstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        /// <summary>
        /// Upload and get bulk orders data
        /// </summary>
        /// <param name="filePath"></param>
        public void UploadBulkOrdersData(String filePath)
        {
            List<BulkOrderUploadContract> lstBulkOrderUploadContract = new List<BulkOrderUploadContract>();
            if (View.SelectedTenantId > AppConsts.NONE && View.ApplicantXmlData.IsNotNull())
            {
                String orderDataSourceCode = Orderdatasource.Manual.GetStringValue();
                lstBulkOrderUploadContract = ComplianceDataManager.UploadBulkOrdersData(View.SelectedTenantId, View.ApplicantXmlData, filePath, View.CurrentLoggedInUserId, orderDataSourceCode);
            }
            View.ApplicantDataList = lstBulkOrderUploadContract;

            if (!lstBulkOrderUploadContract.IsNullOrEmpty())
            {
                //todo: Send Mails to accepted records
                String orderAcceptedCode = BulkOrderStatus.OrderAccepted.GetStringValue();
                lstBulkOrderUploadContract = lstBulkOrderUploadContract.Where(x => x.BulkOrderStatusCode == orderAcceptedCode).ToList();
                var applicantRegisterAccountList = lstBulkOrderUploadContract.Where(x => x.OrganizationUserId.IsNullOrEmpty()).ToList();
                var applicantCompleteOrderList = lstBulkOrderUploadContract.Where(x => !x.OrganizationUserId.IsNullOrEmpty()).ToList();
                String tenantName = SecurityManager.GetTenant(View.SelectedTenantId).TenantName;
                String applicationUrl = WebSiteManager.GetInstitutionUrl(View.SelectedTenantId);

                foreach (var applicantRegisterAccount in applicantRegisterAccountList)
                {
                    //Create Dictionary
                    Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                    dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, applicantRegisterAccount.ApplicantFirstName + " " + applicantRegisterAccount.ApplicantLastName);
                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                    dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, applicationUrl);

                    //Create MockUp Data
                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.UserName = applicantRegisterAccount.ApplicantFirstName + " " + applicantRegisterAccount.ApplicantLastName;
                    mockData.EmailID = applicantRegisterAccount.EmailAddress;
                    mockData.ReceiverOrganizationUserID = AppConsts.SYSTEM_COMMUNICATION_USER_VALUE;

                    //Send mail
                    CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_REGISTER_ACCOUNT_COMPLETE_ORDER, dictMailData, mockData, View.SelectedTenantId, applicantRegisterAccount.OrderNodeID.Value, null, null, true);

                }
                foreach (var applicantCompleteOrder in applicantCompleteOrderList)
                {
                    //Create Dictionary
                    Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                    dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, applicantCompleteOrder.ApplicantFirstName + " " + applicantCompleteOrder.ApplicantLastName);
                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                    dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, applicationUrl);

                    //Create MockUp Data
                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.UserName = applicantCompleteOrder.ApplicantFirstName + " " + applicantCompleteOrder.ApplicantLastName;
                    mockData.EmailID = applicantCompleteOrder.EmailAddress;
                    mockData.ReceiverOrganizationUserID = applicantCompleteOrder.OrganizationUserId.Value;

                    //Send mail
                    CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_APPLICANT_COMPLETE_ORDER, dictMailData, mockData, View.SelectedTenantId, applicantCompleteOrder.OrderNodeID.Value, null, null, true);

                }
            }
        }

        /// <summary>
        /// UAT-2697
        /// </summary>
        /// <param name="filePath"></param>
        public void UploadBulkRepeatedOrdersData(String filePath) 
        {
            List<BulkOrderUploadContract> lstBulkOrderUploadContract = new List<BulkOrderUploadContract>();
            if (View.SelectedTenantId > AppConsts.NONE && View.ApplicantXmlData.IsNotNull())
            {
                String orderDataSourceCode = Orderdatasource.Previous.GetStringValue();
                lstBulkOrderUploadContract = ComplianceDataManager.UploadBulkRepeatedOrdersData(View.SelectedTenantId, View.ApplicantXmlData, filePath, View.CurrentLoggedInUserId, orderDataSourceCode);
            }
            View.ApplicantDataList = lstBulkOrderUploadContract;
        }
    }
}