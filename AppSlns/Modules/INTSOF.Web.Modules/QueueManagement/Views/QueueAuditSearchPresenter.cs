using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.QueueManagement;
using INTSOF.Utils;

namespace CoreWeb.QueueManagement.Views
{
    public class QueueAuditSearchPresenter : Presenter<IQueueAuditSearchView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            //GetTenants();
        }

        public void GetTenants()
        {
            List<Tenant> lstTemp = ComplianceDataManager.getClientTenant();
            lstTemp.Insert(0, new Tenant { TenantID = 0, TenantName = "--Select--" });
            View.lstTenant = lstTemp;
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        public void GetMetaDataQueueList()
        {
            List<QueueMetaData> lstQueueMetaDataTemp = new List<QueueMetaData>();
            List<QueueMetaData> tmplstQueueMetaData = new List<QueueMetaData>();
            if (ClientId == AppConsts.NONE)
                lstQueueMetaDataTemp = new List<QueueMetaData>();
            else
                lstQueueMetaDataTemp = QueueManagementManager.GetQueueDataList(ClientId);

            if (View.SelectedBusinessProcessId > 0)
                lstQueueMetaDataTemp = lstQueueMetaDataTemp.Where(x => x.QMD_BusinessProcessID == View.SelectedBusinessProcessId).ToList();


            tmplstQueueMetaData = lstQueueMetaDataTemp.OrderBy(x => x.QMD_QueueName).ToList();
            tmplstQueueMetaData.Insert(0, new QueueMetaData { QMD_QueueID = 0, QMD_QueueName = "--Select--" }); //UAT- sort dropdowns by Name
            View.lstQueueData = tmplstQueueMetaData;

        }

        public void GetUserList()
        {
            List<OrganizationUserContract> lstUserTemp = new List<OrganizationUserContract>();
             List<OrganizationUserContract> tmpUsers = new List<OrganizationUserContract>();
            if (ClientId == AppConsts.NONE && View.SelectedQueueId == AppConsts.NONE)
                lstUserTemp = new List<OrganizationUserContract>();
            else
                lstUserTemp = QueueManagementManager.GetOrganizationUserListByQueueID(ClientId, View.SelectedQueueId);
            tmpUsers = lstUserTemp.OrderBy(x => x.FullName).ToList();
            tmpUsers.Insert(0, new OrganizationUserContract { OrganizationUserId = 0, FullName = "--Select--" }); //UAT- sort dropdowns by Name
            View.lstOrganizationUser = tmpUsers;

        }

        public void GetBusinessProcessList()
        {
            List<lkpQueueBusinessProcess> lstBusinessProcess = new List<lkpQueueBusinessProcess>();
            if (ClientId == AppConsts.NONE)
                lstBusinessProcess = new List<lkpQueueBusinessProcess>();
            else
                lstBusinessProcess = QueueManagementManager.GetBusinessProcessList(ClientId);
            lstBusinessProcess.Insert(0, new lkpQueueBusinessProcess { QBP_ID = 0, QBP_Name = "--Select--" });
            View.lstBusinessProcess = lstBusinessProcess;

        }

        /// <summary>
        /// Get the Applicant Data audit History
        /// </summary>
        public void GetQueueAuditRecord()
        {
            try
            {
                if (ClientId != 0 && ClientId.IsNotNull())
                {
                    QueueFrameworkSearchDataContract searchDataContract = new QueueFrameworkSearchDataContract();
                    if (!IsDefaultTenant && View.CurrentLoggedInUserId != AppConsts.NONE)
                    {
                        searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
                    }
                    searchDataContract.LoggedInUserTenantId = View.TenantId;
                    if (View.SelectedUserId != AppConsts.NONE && View.SelectedUserId.IsNotNull())
                    {
                        searchDataContract.SelectedUserId = View.SelectedUserId;
                    }
                    if (View.SelectedQueueId != AppConsts.NONE && View.SelectedQueueId.IsNotNull())
                    {
                        searchDataContract.SelectedQueueId = View.SelectedQueueId;
                    }
                    if (!View.TimeStampFromDate.IsNullOrEmpty() && View.TimeStampFromDate != DateTime.MinValue)
                    {
                        searchDataContract.FromDate = View.TimeStampFromDate;
                    }
                    else
                    {
                        searchDataContract.FromDate = null;
                    }
                    if (!View.TimeStampToDate.IsNullOrEmpty() && View.TimeStampToDate != DateTime.MinValue)
                    {
                        searchDataContract.ToDate = View.TimeStampToDate;
                    }
                    else
                    {
                        searchDataContract.ToDate = null;
                    }
                    if (View.SelectedBusinessProcessId != AppConsts.NONE)
                    {
                        searchDataContract.SelectedBusinessProcessId = View.SelectedBusinessProcessId;
                    }
                    if (View.RecordId != AppConsts.NONE)
                    {
                        searchDataContract.RecordId = View.RecordId;
                    }
                    View.QueueAuditRecordList = QueueManagementManager.GetQueueRecordAuditData(ClientId, searchDataContract, View.GridCustomPaging);
                    if (View.QueueAuditRecordList.IsNotNull() && View.QueueAuditRecordList.Count > 0)
                    {
                        if (View.QueueAuditRecordList[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.QueueAuditRecordList[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                }
                else
                {
                    View.QueueAuditRecordList = new List<QueueAuditRecordContract>();
                }
            }
            catch (Exception e)
            {
                View.QueueAuditRecordList = new List<QueueAuditRecordContract>();
                throw e;
            }
        }

        #region Properties
        /// <summary>
        /// To get Client Id
        /// </summary>
        private Int32 ClientId
        {
            get
            {
                if (IsDefaultTenant)
                    return View.SelectedTenantId;
                return View.TenantId;
            }
        }
        #endregion
    }
}
