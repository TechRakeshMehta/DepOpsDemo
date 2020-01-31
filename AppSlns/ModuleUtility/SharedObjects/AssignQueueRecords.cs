using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebSiteUtils.SharedObjects
{
    [Serializable]
    public class AssignQueueRecords
    {
        public Int32 ApplicantComplienceItemId { get; set; }
        public Int32 CategoryId { get; set; }
        public Int32 ComplianceItemId { get; set; }
        public String verificationStatusCode { get; set; }
        public Boolean IsChecked { get; set; }
        public String ItemName { get; set; }
        public Int32 tenantID { get; set; }
        public String workQueueType { get; set; }
        public Boolean IsDefaultThirdPartyTenant { get; set; }
        public Boolean IsEsclationRecord { get; set; }
        public String QueueCode { get; set; }
        public Dictionary<String, Object> dicQueueFields
        {
            get
            {
                Dictionary<String, Object> queueFields = new Dictionary<String, Object>();
                String categoryCode = String.Empty;
                String applicantComplianceItemIdCode = String.Empty;
                String complianceItemIdCode = String.Empty;
                switch (this.queueId)
                {
                    case 1:
                        categoryCode = QueuefieldsMetaDataType.CategoryId_VerificationQueueAdmin.GetStringValue();
                        applicantComplianceItemIdCode = QueuefieldsMetaDataType.ApplicantComplianceItemID_VerificationQueueAdmin.GetStringValue();
                        complianceItemIdCode = QueuefieldsMetaDataType.ComplianceItemId_VerificationQueueAdmin.GetStringValue();
                        break;
                    case 2:
                        categoryCode = QueuefieldsMetaDataType.CategoryId_VerificationQueueClientAdmin.GetStringValue();
                        applicantComplianceItemIdCode = QueuefieldsMetaDataType.ApplicantComplianceItemID_VerificationQueueClientAdmin.GetStringValue();
                        complianceItemIdCode = QueuefieldsMetaDataType.ComplianceItemId_VerificationQueueClientAdmin.GetStringValue();
                        break;
                    case 3:
                        categoryCode = QueuefieldsMetaDataType.CategoryId_VerificationQueueThirdParty.GetStringValue();
                        applicantComplianceItemIdCode = QueuefieldsMetaDataType.ApplicantComplianceItemID_VerificationQueueThirdParty.GetStringValue();
                        complianceItemIdCode = QueuefieldsMetaDataType.ComplianceItemId_VerificationQueueThirdParty.GetStringValue();
                        break;
                    case 4:
                        categoryCode = QueuefieldsMetaDataType.CategoryId_ExceptionQueueAdmin.GetStringValue();
                        applicantComplianceItemIdCode = QueuefieldsMetaDataType.ApplicantComplianceItemID_ExceptionQueueAdmin.GetStringValue();
                        complianceItemIdCode = QueuefieldsMetaDataType.ComplianceItemId_ExceptionQueueAdmin.GetStringValue();
                        break;
                    case 5:
                        categoryCode = QueuefieldsMetaDataType.CategoryId_ExceptionQueueClientAdmin.GetStringValue();
                        applicantComplianceItemIdCode = QueuefieldsMetaDataType.ApplicantComplianceItemID_ExceptionClientAdmin.GetStringValue();
                        complianceItemIdCode = QueuefieldsMetaDataType.ComplianceItemId_ExceptionQueueClientAdmin.GetStringValue();
                        break;
                    case 6:
                        categoryCode = QueuefieldsMetaDataType.CategoryId_EsclationQueueAdmin.GetStringValue();
                        applicantComplianceItemIdCode = QueuefieldsMetaDataType.ApplicantComplianceItemID_EsclationQueueAdmin.GetStringValue();
                        complianceItemIdCode = QueuefieldsMetaDataType.ComplianceItemId_EsclationQueueAdmin.GetStringValue();
                        break;
                    case 7:
                        categoryCode = QueuefieldsMetaDataType.CategoryId_EsclationQueueClientAdmin.GetStringValue();
                        applicantComplianceItemIdCode = QueuefieldsMetaDataType.ApplicantComplianceItemID_EsclationQueueClientAdmin.GetStringValue();
                        complianceItemIdCode = QueuefieldsMetaDataType.ComplianceItemId_EsclationQueueClientAdmin.GetStringValue();
                        break;
                    case 8:
                        categoryCode = QueuefieldsMetaDataType.CategoryId_EsclationQueueThirdParty.GetStringValue();
                        applicantComplianceItemIdCode = QueuefieldsMetaDataType.ApplicantComplianceItemID_EsclationQueueThirdParty.GetStringValue();
                        complianceItemIdCode = QueuefieldsMetaDataType.ComplianceItemId_EsclationQueueThirdParty.GetStringValue();
                        break;
                    case 9:
                        categoryCode = QueuefieldsMetaDataType.CategoryId_ExceptionEsclationQueueAdmin.GetStringValue();
                        applicantComplianceItemIdCode = QueuefieldsMetaDataType.ApplicantComplianceItemID_ExceptionEsclationQueueAdmin.GetStringValue();
                        complianceItemIdCode = QueuefieldsMetaDataType.ComplianceItemId_ExceptionEsclationQueueAdmin.GetStringValue();
                        break;
                    case 10:
                        categoryCode = QueuefieldsMetaDataType.CategoryId_ExceptionEsclationQueueClientAdmin.GetStringValue();
                        applicantComplianceItemIdCode = QueuefieldsMetaDataType.ApplicantComplianceItemID_ExceptionEsclationClientAdmin.GetStringValue();
                        complianceItemIdCode = QueuefieldsMetaDataType.ComplianceItemId_ExceptionEsclationQueueClientAdmin.GetStringValue();
                        break;

                    default:
                        break;
                }
                queueFields.Add(QueueManagementManager.GetQueueFieldsMetaDataByQueueIdAndCode(this.queueId, this.tenantID, categoryCode).QF_ValueFieldName, this.CategoryId);
                queueFields.Add(QueueManagementManager.GetQueueFieldsMetaDataByQueueIdAndCode(this.queueId, this.tenantID, applicantComplianceItemIdCode).QF_ValueFieldName, this.ApplicantComplienceItemId);
                queueFields.Add(QueueManagementManager.GetQueueFieldsMetaDataByQueueIdAndCode(this.queueId, this.tenantID, complianceItemIdCode).QF_ValueFieldName, this.ComplianceItemId);
                return queueFields;

            }

        }

        public Int32 queueId
        {
            get
            {
                if(this.IsEsclationRecord)
                    return LookupManager.GetLookUpData<QueueMetaData>(this.tenantID).Where(x => x.QMD_IsDeleted == false && x.QMD_Code.Equals(this.QueueCode)).FirstOrDefault().QMD_QueueID;
                else if (this.verificationStatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()) && !this.IsDefaultThirdPartyTenant)
                    return 2;
                else if (this.verificationStatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()) && this.IsDefaultThirdPartyTenant)
                    return 3;
                else if (!this.workQueueType.IsNullOrEmpty() && this.workQueueType.Equals(WorkQueueType.ExceptionAssignmentWorkQueue.ToString()) && this.IsDefaultThirdPartyTenant)
                    return 4;
                else if (!this.workQueueType.IsNullOrEmpty() && this.workQueueType.Equals(WorkQueueType.ExceptionAssignmentWorkQueue.ToString()) && !this.IsDefaultThirdPartyTenant)
                    return 5;
                return 1;
            }
        }
    }

    public class AutoAssignQueueRecordsList
    {
        public AssignQueueRecords assignQueueRecords { get; set; }
        public List<Int32> tenantIDs { get; set; }
    }
}
