using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

#region UserDefined

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.ServiceUtil;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.SharedObjects;
using INTSOF.Contracts;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion


namespace CoreWeb.Mobility.Views
{
    public class ProgramChangeApproveModifyPresenter : Presenter<IProgramChangeApproveModifyView>
    {
        #region Public Method

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Gets the subscription details
        /// </summary>
        /// 
        public void GetDeptProgramPackageSubscription()
        {
            if (View.DeptProgramPackage.IsNotNull())
            {
                var deptProgramPackageSubscriptionList = ComplianceDataManager.GetDeptProgramPackageSubscription(View.DeptProgramPackage.DPP_ID, View.TenantId);
                if (deptProgramPackageSubscriptionList.IsNotNull())
                {
                    View.lstDeptProgramPackageSubscription = deptProgramPackageSubscriptionList;
                }
            }
        }

        public Int32 GetTenant()
        {
            Entity.Organization _org = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization;
            //  View.InstitutionName = _org.OrganizationName;
            return _org.TenantID.Value;
        }

        public void GetSourceNodeDeatil(Int32 sourceNodeId, Int32 sourcePageId)
        {
            var sourceNodeDetails = MobilityManager.GetSourceNodeDeatils(View.TenantId, sourceNodeId, sourcePageId).FirstOrDefault();
            if (sourceNodeDetails.IsNotNull())
            {
                View.PreviousHierarchyNode = sourceNodeDetails.HierarchyLabel;
                View.PreviousPackage = sourceNodeDetails.PackageName;
            }
        }

        public String GetTargetNodeHierarchyLabel(Int32 departmentProgramMappingId)
        {
            String TargetHierarchylabel = String.Empty;
            TargetHierarchylabel = MobilityManager.GetTargetNodeHierarchyLabel(View.TenantId, departmentProgramMappingId);
            if (!TargetHierarchylabel.IsNullOrEmpty())
            {
                return TargetHierarchylabel;
            }
            else
            {
                return TargetHierarchylabel;
            }
        }

        public void GetSourceSubscription(List<Int32> sourceSubscriptionList)
        {
            var packageSourceSubscriptionList = MobilityManager.GetSourceSubscriptionDetails(View.TenantId, sourceSubscriptionList);
            if (packageSourceSubscriptionList.IsNotNull())
            {
                View.SourceSubscriptionList = packageSourceSubscriptionList;
            }
        }

        //public Boolean CreateNewSubscriptionForMobilityNode(String xml, Int32 packageMappingMasterId, Int32 mappingInstanceId, String targetHierarchyLabel)
        //{
        //    try
        //    {
        //        List<usp_SubscriptionChange_Result> MobilitySubscriptionChangedData = MobilityManager.CreateNewSubscriptionForMobilityNode(View.TenantId, xml);
        //        if (MobilitySubscriptionChangedData != null)
        //        {
        //            MobilityManager.UpdateMappingInstanceforPackageSubscription(packageMappingMasterId, mappingInstanceId, View.CurrentLoggedInUserId, View.TenantId);
        //            Entity.Tenant tenant = SecurityManager.GetTenant(View.TenantId);
        //            String tenantName = tenant.TenantName;
        //            String applicationUrl = WebSiteManager.GetInstitutionUrl(View.TenantId);
        //            String changedHierarchyNode = targetHierarchyLabel;

        //            Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
        //            dataDict.Add("tenantName", tenantName);
        //            dataDict.Add("applicationUrl", applicationUrl);
        //            dataDict.Add("changedHierarchyNode", changedHierarchyNode);
        //            dataDict.Add("MobilitySubscriptionChangedData", MobilitySubscriptionChangedData);

        //            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
        //            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
        //            ParallelTaskContext.PerformParallelTask(SendMailForChangeSubscription, dataDict, LoggerService, ExceptiomService);

        //        }
        //        return true;
        //    }
        //    catch (SysXException ex)
        //    {
        //        throw ex;
        //    }

        //}

        private void SendMailForChangeSubscription(Dictionary<String, Object> data)
        {
            List<usp_SubscriptionChange_Result> MobilitySubscriptionChangedData = data.GetValue("MobilitySubscriptionChangedData") as List<usp_SubscriptionChange_Result>;
            String tenantName = data.GetValue("tenantName") as String;
            String applicationUrl = data.GetValue("applicationUrl") as String;
            String changedHierarchyNode = data.GetValue("changedHierarchyNode") as String;
            foreach (usp_SubscriptionChange_Result mobSubscrptnData in MobilitySubscriptionChangedData)
            {
                //Send Mail
                MobilityManager.sendMailForChangeSubscription(View.TenantId, mobSubscrptnData, applicationUrl, tenantName, changedHierarchyNode, mobSubscrptnData.SelectedNodeId);//Change HierarchyId with selectedNodeId UAT-1067
            }
        }

        public void GetDeptProgramPackage()
        {
            //Boolean NoUserMode = true;
            String packageType = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
            //List<DeptProgramPackage> _deptPrgrmPackage = ComplianceDataManager.GetDeptProgramPackage(View.UserId, View.TenantId, View.SelectedHierarchyNodeIds, NoUserMode);
            List<AvailablePackageContarct> _deptPrgrmPackage = StoredProcedureManagers.GetAvailableCompAndBkgPackages(View.TenantId, View.SelectedHierarchyNodeIds[0], packageType).Where(cond => cond.IsCompliancePackage).ToList();
            View.DeptProgramPackages = _deptPrgrmPackage;
            if (_deptPrgrmPackage.IsNotNull() && _deptPrgrmPackage.Count > 0)
            {
                /*Commented For UAT-1067
                  View.DeptProgramPackage = _deptPrgrmPackage[0];
                 * //View.NodeId = _deptPrgrmPackage[0].DeptProgramMapping.DPM_InstitutionNodeID;
                 */
                // View.ProgramDuration = View.DeptProgramPackage.IsNull() ? null : View.DeptProgramPackage.DeptProgramMapping.InstitutionNode.IN_Duration;
                View.TargetNodeID = _deptPrgrmPackage[0].DPM_ID;
                View.IsPackageSubscribe = false;
            }
            else if (_deptPrgrmPackage.IsNotNull() && _deptPrgrmPackage.Count == 0)
            {
                View.IsPackageSubscribe = true;
            }
            else
            {
                View.DeptProgramPackage = null;
                View.IsPackageSubscribe = false;
            }
        }

        public Int32 GetMappingInstanceId(Int32 packageMappingMasterId)
        {
            return MobilityManager.GetMappingInstanceId(packageMappingMasterId, View.CurrentLoggedInUserId);
        }
        public Int32 GetMappingMasterId(Int32 sourcePackageId, Int32 targetPackageId, Int32 sourceTenantId, Int32 targetTenantId, Int32 sourceNodeId, Int32? targetNodeId)
        {
            return MobilityManager.GetMappingData(sourcePackageId, targetPackageId, sourceTenantId, targetTenantId, sourceNodeId, targetNodeId);
        }

        #region Order Package Type
        public Int32 GetOrderPackageTypeId()
        {
            return MobilityManager.GetOrderPackageTypes(View.TenantId, OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue());
        }
        #endregion

        public void GetDeptProgramPackageById(Int32 DPP_ID)
        {
            if (DPP_ID > AppConsts.NONE && View.TenantId > AppConsts.NONE)
                View.DeptProgramPackage = ComplianceDataManager.GetDeptProgramPackageById(DPP_ID, View.TenantId);
        }
        #endregion

        #region UAT-2387
        public Boolean ChangePackageAndSubscription(String xml, Int32 packageMappingMasterId, Int32 mappingInstanceId, String targetHierarchyLabel, Boolean isOnlyPackageChange)
        {
            try
            {
                List<usp_SubscriptionChange_Result> MobilitySubscriptionChangedData = MobilityManager.ChangePackageAndSubscription(View.TenantId, xml, isOnlyPackageChange);
                if (MobilitySubscriptionChangedData != null)
                {
                    MobilityManager.UpdateMappingInstanceforPackageSubscription(packageMappingMasterId, mappingInstanceId, View.CurrentLoggedInUserId, View.TenantId);
                    Entity.Tenant tenant = SecurityManager.GetTenant(View.TenantId);
                    String tenantName = tenant.TenantName;
                    String applicationUrl = WebSiteManager.GetInstitutionUrl(View.TenantId);
                    String changedHierarchyNode = targetHierarchyLabel;

                    Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
                    dataDict.Add("tenantName", tenantName);
                    dataDict.Add("applicationUrl", applicationUrl);
                    dataDict.Add("changedHierarchyNode", changedHierarchyNode);
                    dataDict.Add("MobilitySubscriptionChangedData", MobilitySubscriptionChangedData);

                    var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                    ParallelTaskContext.PerformParallelTask(SendMailForChangeSubscription, dataDict, LoggerService, ExceptiomService);

                }
                return true;
            }
            catch (SysXException ex)
            {
                throw ex;
            }

        }
        #endregion
    }
}




