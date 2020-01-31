using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Business.RepoManagers;


namespace CoreWeb.ComplianceOperations.Views
{
    public class VerificationUserQueuePresenter : Presenter<IVerificationUserQueueView>
    {
        /// <summary>
        /// Check if logged in user is default tenant/ADB admin
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return View.TenantId == SecurityManager.DefaultTenantID;
            }
        }

        /// <summary>
        /// To get Verification Assignment Queue ID
        /// </summary>
        /// <returns></returns>
        public Int32 GetQueueID()
        {
            String queueCode = GetQueueCode();
            var queueMetaData = QueueManagementManager.GetQueueMetaDataByCode(View.TenantId, queueCode);
            if (queueMetaData.IsNotNull())
            {
                return queueMetaData.QMD_QueueID;
            }
            return 0;
        }

        /// <summary>
        /// To get Queue code
        /// </summary>
        /// <returns></returns>
        public String GetQueueCode()
        {
            String code = String.Empty;

            //If ADB Admin
            if (IsDefaultTenant)
            {
                code = QueueMetaDataType.Escalated_Verification_Queue_For_Admin.GetStringValue();
            }
            else if (!IsDefaultTenant && View.TenantTypeCode == TenantType.Institution.GetStringValue()) //else if Client Admin
            {
                code = QueueMetaDataType.Escalated_Verification_Queue_For_ClientAdmin.GetStringValue();
            }
            else if (!IsDefaultTenant && View.TenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()) //else if TenantType is Compliance_Reviewer i.e. Third Party
            {
                code = QueueMetaDataType.Escalated_Verification_Queue_For_Third_Party.GetStringValue();
            }
            return code;
        }
    }
}
