using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.QueueManagement;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoreWeb.QueueManagement.Views
{
    public class AssignmentConfigurationQueuePresenter : Presenter<IAssignmentConfigurationQueueView>
    {
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            GetTenants();
        }

        public void GetTenants()
        {
            //get list ofTenant
            View.lstTenant = ComplianceDataManager.getClientTenant();
        }
        public void GetMetaDataQueueList()
        {
            //to get list of Queues Name
            if (View.SelectedTenantId == 0)
                View.lstQueueData = new List<QueueMetaData>();
            else
                View.lstQueueData = QueueManagementManager.GetQueueDataList(View.SelectedTenantId).Where(x => !x.QMD_IsEscalationQueue).ToList();
        }
        public void getQueueMetaDataFieldsList()
        {
            //to get list of MetaDataFields
            if (View.SelectedQueueId == 0)
                View.lstQueueFieldsMetaData = new List<QueueFieldsMetaData>();
            else
                View.lstQueueFieldsMetaData = QueueManagementManager.GetQueueFieldsMetaDataByQueueId(View.SelectedQueueId, View.SelectedTenantId);
        }

        public void getQueueSpecializationCriterionFieldsList()
        {
            ////to get list of MetaDataValues
            if (View.SelectedQueueFieldID == AppConsts.NONE)
                View.LstQueueSpecilizationCriterion = new List<GetQueueSpecilizationCriterion>();
            else
            {
                QueueFieldsMetaData queueFieldsMetaData = QueueManagementManager.GetQueueFieldsMetaDataByQueueFieldId(View.SelectedQueueFieldID, View.SelectedTenantId);
                if (queueFieldsMetaData.QF_DisplayFieldQuery!=null)
                    View.LstQueueSpecilizationCriterion = QueueManagementManager.GetQueueSpecializedFieldsList(queueFieldsMetaData.QF_DisplayFieldQuery, View.SelectedTenantId);
            }
        }
        /// <summary>
        /// To check the defaultTenant/Admin
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;

            }
        }
        private Int32 ClientId
        {
            get
            {
                if (IsDefaultTenant)
                    return View.SelectedTenantId;
                return View.TenantId;
            }
        }
        /// <summary>
        /// To Perform Search
        /// </summary>
        public void PerformSearch()
        {
            try
            {
                if (ClientId != 0 && ClientId.IsNotNull())
                {
                   //set the values in the Contracr Class 
                    QueueFrameworkSearchDataContract queueFrameworkSearchDataContract = new INTSOF.UI.Contract.QueueManagement.QueueFrameworkSearchDataContract();
                    queueFrameworkSearchDataContract.Description = String.IsNullOrEmpty(View.Description) ? null : View.Description;
                    queueFrameworkSearchDataContract.SelectedQueueFieldValue = String.IsNullOrEmpty(View.SelectedQueueFieldValue) ? null : View.SelectedQueueFieldValue;
                    if (View.SelectedTenantId > SysXDBConsts.NONE)
                    {
                        queueFrameworkSearchDataContract.TenantID = View.SelectedTenantId;
                    }
                    if (View.SelectedQueueId > SysXDBConsts.NONE)
                    {
                        queueFrameworkSearchDataContract.SelectedQueueId = View.SelectedQueueId;
                    }
                   if (View.SelectedQueueFieldID > SysXDBConsts.NONE)
                   {
                       queueFrameworkSearchDataContract.SelectedQueueFieldID = View.SelectedQueueFieldID;
                   }
                    
                    String searchParameter = BuildSearchParameterXML(queueFrameworkSearchDataContract);
                    //Search
                    View.AssignmentConfigRecord = QueueManagementManager.GetQueueAssignmentConfRecord(ClientId, View.GridCustomPaging, searchParameter, View.QueueTypeCode);

                    if (View.AssignmentConfigRecord != null && View.AssignmentConfigRecord.Count > 0)
                    {
                        if (View.AssignmentConfigRecord[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.AssignmentConfigRecord[0].TotalCount;
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
                    View.AssignmentConfigRecord = new List<QueueAssignmentConfRecord>();
                }

            }
            catch (Exception e)
            {
                View.AssignmentConfigRecord = null;
                throw e;
            }


        }

        private String BuildSearchParameterXML(QueueFrameworkSearchDataContract queueFrameworkSearchDataContract)
        {
            XElement root = new XElement("root");
            XElement row = null;

            if (queueFrameworkSearchDataContract.SelectedQueueId >0)
            {
                row = new XElement("SelectedQueueId");
                row.Value = queueFrameworkSearchDataContract.SelectedQueueId.ToString();
                root.Add(row);
            }
            if (queueFrameworkSearchDataContract.Description != null)
            {
                row = new XElement("Description");
                row.Value = queueFrameworkSearchDataContract.Description;
                root.Add(row);
            }
            if (queueFrameworkSearchDataContract.TenantID>0)
            {
                row = new XElement("TenantID");
                row.Value = queueFrameworkSearchDataContract.TenantID.ToString();
                root.Add(row);

            }
            
            if (queueFrameworkSearchDataContract.SelectedQueueFieldID>0)
            {
                row = new XElement("SelectedQueueFieldID");
                row.Value = queueFrameworkSearchDataContract.SelectedQueueFieldID.ToString();
                root.Add(row);
            }
            if (queueFrameworkSearchDataContract.SelectedQueueFieldValue!=null)
            {
                row = new XElement("SelectedQueueFieldValue");
                row.Value = queueFrameworkSearchDataContract.SelectedQueueFieldValue.ToString();
                root.Add(row);
            
            }
            
            return root.ToString();
        }
        /// <summary>
        /// Deletion Operation
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="queueAssignmentConfID">queueAssignmentConfID</param>
        /// <returns></returns>
        public Boolean DeleteQueueAssignmentConfigurationRecord(Int32 tenantId, Int32 queueAssignmentConfID) 
        {
           return QueueManagementManager.DeleteQueueAssignmentConfigurationRecord(tenantId, queueAssignmentConfID, View.CurrentLoggedInUserId, View.QueueType);
}

    }

}
