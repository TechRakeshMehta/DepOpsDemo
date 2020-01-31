#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  InstituteHierarchyNodePackagePresenter.cs
// Purpose:   
//

#endregion

#region Namespace

#region System Defined

using System;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Business.RepoManagers;
using System.Collections.Generic;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.Mobility;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public class InstituteHierarchyNodePackagePresenter : Presenter<IInstituteHierarchyNodePackageView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        /// <summary>
        /// To get Institution Node Types
        /// </summary>
        public void GetInstitutionNodeTypes()
        {
            var institutionNodeTypes = ComplianceSetupManager.GetInstitutionNodeTypes(View.TenantId);
            if (institutionNodeTypes.IsNotNull())
            {
                String institutionCode = NodeType.Institution.GetStringValue();
                View.ListInstitutionNodeType = institutionNodeTypes.Where(x => x.INT_Code != institutionCode).ToList();
            }
        }

        /// <summary>
        /// To get Institution Nodes
        /// </summary>
        public void GetInstitutionNodes()
        {
            View.ListInstitutionNode = ComplianceSetupManager.GetInstitutionNodes(View.TenantId, View.SelectedNodeTypeId);
        }

        /// <summary>
        /// To get node label/name
        /// </summary>
        public void GetNodeLabel()
        {
            var deptProgramMapping = MobilityManager.GetDeptProgramMappingById(View.DeptProgramMappingID, View.TenantId);
            if (deptProgramMapping.IsNotNull() && deptProgramMapping.InstitutionNode.IsNotNull())
            {
                View.NodeLabel = "Node: " + deptProgramMapping.InstitutionNode.IN_Name;
                View.IsRootNode = deptProgramMapping.DPM_ParentNodeID.IsNull() ? true : false;

                View.IsAvailableForOrderEditMode = deptProgramMapping.DPM_IsAvailableForOrder.IsNull() ||
                                           Convert.ToBoolean(deptProgramMapping.DPM_IsAvailableForOrder)
                                           ? true
                                           : false;

                //UAT-1794 : Ability to restrict admin data entry by node.
                View.IsAdminDataEntryAllow = deptProgramMapping.DPM_IsAdminDataEntryAllowed.IsNotNull() ? deptProgramMapping.DPM_IsAdminDataEntryAllowed : null;

                //UAT-3683 : Move Optional Category Setting From Client Settings to institution hierarchy with look up
                View.OptionalCategorySetting = deptProgramMapping.DPM_OptionalCategorySetting.IsNotNull() ? (deptProgramMapping.DPM_OptionalCategorySetting == AppConsts.ONE ? "Y" : "N" ) : null;             
                //UAT-1176 - To Get Current Settings of Employment Type of selected Node.
                View.IsEmploymentTypeEditMode = (deptProgramMapping.DPM_IsEmployment ?? false)
                                                ? true
                                                : false;
                //To get splash url
                View.SplashPageUrlEditMode = deptProgramMapping.DPM_SplashPageUrl.IsNull() ? String.Empty : deptProgramMapping.DPM_SplashPageUrl;
                //To get Expiration Frequency
                View.BeforeExpirationFrequencyEditMode = View.ParentID==0 && deptProgramMapping.DPM_ExpirationFrequency.IsNull() ? "60,30,15,7,0" : deptProgramMapping.DPM_ExpirationFrequency;
                //UAT-4060
                View.AfterExpirationFrequencyEditMode = View.ParentID==0 && deptProgramMapping.DPM_AfterExpirationFrequency.IsNullOrEmpty()?15: deptProgramMapping.DPM_AfterExpirationFrequency;
                View.SubscriptionBeforeExpiryEditMode = View.ParentID==0 && deptProgramMapping.DPM_SubscriptionBeforeExpFrequency.IsNullOrEmpty()?30: deptProgramMapping.DPM_SubscriptionBeforeExpFrequency;
                View.SubscriptionAfterExpiryEditMode = View.ParentID==0 && deptProgramMapping.DPM_SubscriptionAfterExpFrequency.IsNullOrEmpty()?30: deptProgramMapping.DPM_SubscriptionAfterExpFrequency;
                View.SubscriptionExpiryFrequencyEditMode = View.ParentID==0 && deptProgramMapping.DPM_SubscriptionEmailFrequency.IsNullOrEmpty()?15: deptProgramMapping.DPM_SubscriptionEmailFrequency;
                
                //UAT-2073
                View.PaymentApprovalID = Convert.ToInt32(deptProgramMapping.DPM_PaymentApprovalID);
            }
        }

        /// <summary>
        /// To get selected node label/name
        /// </summary>
        public void GetSelectedNodeLabel(string selectedNodeID)
        {
            List<InstitutionNode> lstInstitutionNodes = ComplianceSetupManager.GetInstitutionNodes(View.TenantId, View.SelectedNodeTypeId);
            View.SelectedNodeLabel = lstInstitutionNodes.Where(x => x.IN_ID == Convert.ToInt32(selectedNodeID)).Select(cond => cond.IN_Label).FirstOrDefault();
        }


        /// <summary>
        /// method to get payment options
        /// </summary>
        public void GetPaymentOptions()
        {
            View.ListPaymentOption = ClientSecurityManager.GetAllPaymentOption(View.TenantId).ToList();
        }

        /// <summary>
        /// To get selected Payment Options
        /// </summary>
        public void GetSelectedPaymentOptions()
        {
            View.SelectedMappedPaymentOptions = ClientSecurityManager.GetMappedDepProgramPaymentOption(View.TenantId, View.DeptProgramMappingID)
                                                                     .Select(x => x.DPPO_PaymentOptionID).ToList();

            /*List<Int32> selectedPaymentOptionIds = new List<Int32>();
            var deptProgramPaymentOption = ClientSecurityManager.GetMappedDepProgramPaymentOption(View.TenantId, View.DepProgramMappingID).ToList();

            if (deptProgramPaymentOption.IsNotNull())
            {
                deptProgramPaymentOption.ForEach(x => selectedPaymentOptionIds.Add(x.DPPO_PaymentOptionID));
                View.SelectedPaymentOptions = selectedPaymentOptionIds;
            } */
        }

        /// <summary>
        /// To get InstHierarchyMobility data
        /// </summary>
        public void GetInstHierarchyMobility()
        {
            var instHierarchyMobility = MobilityManager.GetInstHierarchyMobility(View.DeptProgramMappingID, View.TenantId);
            if (instHierarchyMobility.IsNotNull())
            {
                View.SelectedFirstStartDate = instHierarchyMobility.IHM_FirstStartDate;
                View.SelectedDurationTypeID = instHierarchyMobility.IHM_DurationTypeID;
                View.SelectedDuration = instHierarchyMobility.IHM_Duration;
                View.SelectedInstanceInterval = instHierarchyMobility.IHM_InstanceInterval.HasValue ? instHierarchyMobility.IHM_InstanceInterval.Value : 0;
                View.SelectedSuccessorNodeID = instHierarchyMobility.IHM_SuccessorID;
                View.SelectedEnableMobility = true;
            }
            else
            {
                View.SelectedEnableMobility = false;
            }
        }

        /// <summary>
        /// To get duration types
        /// </summary>
        public void GetDurationType()
        {
            View.ListDurationType = ClientSecurityManager.GetDurationTypes(View.TenantId).ToList();
        }

        /// <summary>
        /// To get successor nodes
        /// </summary>
        public void GetSuccessorNodes()
        {
            View.ListSuccessorNodes = ComplianceSetupManager.GetInstitutionNodesByProgramMapId(View.ParentID, View.TenantId)
                                                            .Where(x => x.DPM_ID != View.DeptProgramMappingID)
                                                            .Select(x => new LookupContract { ID = x.DPM_ID, Name = x.InstitutionNode.IN_Name })
                                                            .OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// To save Program Package Mapping Node
        /// </summary>
        public void SaveProgramPackageMappingNode()
        {
            ComplianceSetupManager.SaveProgramPackageMappingNode(View.DeptProgramMappingID, View.SelectedNodeId, View.SelectedNodeName, View.SelectedPaymentOptions, View.SelectedFileExtensions, View.CurrentLoggedInUserId,
                View.TenantId, View.SelectedNodeLabel, View.IsAvailableForOrderAddMode, View.IsEmploymentTypeAddMode, View.SplashPageUrlAddMode, View.BeforeExpirationFrequencyAddMode,
                View.AfterExpirationFrequencyAddMode,View.SubscriptionBeforeExpiryAddMode, View.SubscriptionAfterExpiryAddMode, View.SubscriptionExpiryFrequencyAddMode,View.PaymentApprovalIDAddMode, false,View.ArchivalGracePeriod);
        }

        /// <summary>
        /// To get Program Packages
        /// </summary>
        public void GetProgramPackages()
        {
            View.ProgramPackages = ComplianceSetupManager.GetProgramPackagesByProgramMapId(View.DeptProgramMappingID, View.TenantId);
        }

        /// <summary>
        /// To get Successor Packages
        /// </summary>
        public void GetSuccessorPackages()
        {
            //View.ProgramPackages = ComplianceSetupManager.GetProgramPackagesByProgramMapId(View.SelectedSuccessorNodeID.HasValue ? View.SelectedSuccessorNodeID.Value : 0, View.TenantId);
            View.ListSuccessorPackages = ComplianceSetupManager.GetProgramPackagesByProgramMapId(View.SelectedSuccessorNodeID.HasValue ?
                                         View.SelectedSuccessorNodeID.Value : 0, View.TenantId)
                                         .Select(x => new LookupContract { ID = x.DPP_ID, Name = x.CompliancePackage.PackageName }).ToList();
        }

        /// <summary>
        /// Get the successor package dropdownlist selectedvalue
        /// </summary>
        public void GetSuccessorPackageIds()
        {
            View.listMobilityPackageRelation = ComplianceSetupManager.GetSuccessorPackageIds(View.DeptProgramMappingID, Convert.ToInt32(View.SelectedSuccessorNodeID), View.TenantId);
        }

        /// <summary>
        /// To get not mapped Compliance Packages
        /// </summary>
        /// <returns></returns>
        public List<Entity.ClientEntity.CompliancePackage> GetNotMappedCompliancePackages()
        {
            return ComplianceSetupManager.GetNotMappedCompliancePackagesByMapId(View.DeptProgramMappingID, View.TenantId);
        }

        /// <summary>
        /// To 
        /// Program Package Mapping and return the ID generated
        /// </summary>
        /// <param name="lstSelectedPaymentOptions">These are the Package Level Payment Options</param>
        /// <returns></returns>
        public void SaveProgramPackageMapping()
        {
            ComplianceSetupManager.SaveProgramPackageMapping(View.DeptProgramMappingID, View.SelectedPackageId, View.CurrentLoggedInUserId, View.lstSelectedOptions, View.PaymentApprovalIDForPackage, View.TenantId);
        }

        /// <summary>
        /// To delete Program Package Mapping
        /// </summary>
        public void DeleteProgramPackageMapping()
        {
            IntegrityCheckResponse response = IntegrityManager.IsPackageHasOrder(View.DeptProgramPackageID, View.TenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                String deptProMappingName = ComplianceSetupManager.GetCompliancePackafeOfNode(View.DeptProgramPackageID, View.TenantId); ;
                //String GetCompliancePackafeOfNode(Int32 DeptProgramPackageId);
                View.InfoMessage = String.Format(response.UIMessage, deptProMappingName);

            }
            else
            {
                ComplianceSetupManager.DeleteProgramPackageByID(View.DeptProgramPackageID, View.CurrentLoggedInUserId, View.TenantId);
            }
        }

        public void GetNodeList()
        {
            var deptProgramMapping = ComplianceSetupManager.GetInstitutionNodesByProgramMapId(View.DeptProgramMappingID, View.TenantId);
            if (deptProgramMapping.IsNotNull())
            {
                var nodeList = deptProgramMapping.OrderBy(x => x.DPM_DisplayOrder).ToList();
                List<Int32> deptProgramMappingIDs = nodeList.Select(x => x.DPM_ID).ToList();
                // View.ChildNodeList = ComplianceSetupManager.GetInstitutionChildNodesByProgramMapId(deptProgramMappingIDs, View.TenantId);
            }
            ObjectResult<GetChildNodesWithPermission> objChildNodesWithPermission = ComplianceSetupManager.GetChildNodesWithPermission(View.DeptProgramMappingID, IsAdminLoggedIn() ? (Int32?)null : View.CurrentLoggedInUserId, View.TenantId);
            View.NodeList = objChildNodesWithPermission.OrderBy(x => x.DPM_DisplayOrder).ToList();
        }
        /// <summary>
        /// To delete node
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteNode()
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.CheckIfBkgNodeAssociated(View.NodeId, View.TenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                String deptProMappingName = ComplianceSetupManager.GetDeptProgMappingLabel(View.NodeId, View.TenantId);
                View.InfoMessage = String.Format(response.UIMessage, deptProMappingName);
                return false;
            }
            else
            {
                if (ComplianceSetupManager.DeleteProgramPackageMappingByID(View.NodeId, View.CurrentLoggedInUserId, View.TenantId))
                {
                    View.SuccessMessage = "Node deleted successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred. Please try again.";
                    return false;
                }
            }
        }

        /// <summary>
        /// To save mapped Payment Options and Update the availability of the node, for the Order process
        /// </summary>
        public void SaveNodeSettings()
        {
            ComplianceSetupManager.SaveMappedPaymentOptionsNodeAvailability(View.DeptProgramMappingID, View.SelectedMappedPaymentOptions, View.SelectedMappedFileExtensions, View.CurrentLoggedInUserId, View.IsAvailableForOrderEditMode,
                                                                            View.IsEmploymentTypeEditMode, View.TenantId, View.SplashPageUrlEditMode,
                                                                            View.BeforeExpirationFrequencyEditMode,View.AfterExpirationFrequencyEditMode,
                                                                            View.SubscriptionBeforeExpiryEditMode,View.SubscriptionAfterExpiryEditMode,View.SubscriptionExpiryFrequencyEditMode,
                                                                            View.IsAdminDataEntryAllow, View.PaymentApprovalID, View.OptionalCategorySetting);
        }

        /// <summary>
        /// UAT 3683 Move Optional Category Setting From Client Settings to institution hierarchy with look up
        /// </summary>

        public void ExecuteOptionalCategoryRule()
        {
            ComplianceDataManager.ExecuteOptionalCategoryRule(View.TenantId, View.CurrentLoggedInUserId, View.DeptProgramMappingID);
        }

        /// <summary>
        /// UAT 3683 Move Optional Category Setting From Client Settings to institution hierarchy with look up
        /// </summary>

        public String GetOldOptionalCategorySetting()
        {
            var deptProgramMapping = MobilityManager.GetDeptProgramMappingById(View.DeptProgramMappingID, View.TenantId);
            if (deptProgramMapping.IsNotNull())
            {
                return deptProgramMapping.DPM_OptionalCategorySetting.IsNotNull() ? (deptProgramMapping.DPM_OptionalCategorySetting == AppConsts.ONE ? "Y" : "N") : null;
            }

            else
                return null;
        }
            

        /// <summary>
        /// To save mobility data
        /// </summary>
        public Boolean SaveMobilityData()
        {
            return MobilityManager.SaveMobilityData(View.DeptProgramMappingID, View.SelectedFirstStartDate, View.SelectedDurationTypeID, View.SelectedDuration,
                                                    View.SelectedInstanceInterval, View.SelectedSuccessorNodeID, View.listMobilityPackageRelation, View.CurrentLoggedInUserId, View.TenantId);
        }

        /// <summary>
        /// To delete mobility data
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteMobilityData()
        {
            return MobilityManager.DeleteMobilityData(View.DeptProgramMappingID, View.CurrentLoggedInUserId, View.TenantId);
        }

        public void GetHierarchyPermission()
        {
            String complianceHierarchyPermissionType = HierarchyPermissionTypes.COMPLIANCE.GetStringValue();
            View.ComplianceHierarchyPermissionList = new List<vwHierarchyPermission>();
            var hierarchyPermissionList = ComplianceSetupManager.GetHierarchyPermissionList(View.TenantId, View.DeptProgramMappingID);
            if (!hierarchyPermissionList.IsNullOrEmpty())
            {
                View.HierarchyPermissionList = hierarchyPermissionList.ToList();
                View.ComplianceHierarchyPermissionList = View.HierarchyPermissionList.Where(cond => cond.HierarchyPermissionTypeCode != null && cond.HierarchyPermissionTypeCode.Equals(complianceHierarchyPermissionType)).ToList();
            }
        }

        public void GetOrganizationUserList()
        {
            //var hierarchyPermissionList = View.HierarchyPermissionList;
            var OrganizationUserList = SecurityManager.GetOganisationUsersByTanentId(View.TenantId);

            //if (hierarchyPermissionList.IsNotNull())
            if (!View.ComplianceHierarchyPermissionList.IsNullOrEmpty())
            {
                var uniqueOrganizationUserList = OrganizationUserList.Where(p => !View.ComplianceHierarchyPermissionList.Any(p2 => p2.OrganizationUserID == p.OrganizationUserID));
                View.OrganizationUserList = uniqueOrganizationUserList.Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName + " " + x.LastName,
                    OrganizationUserID = x.OrganizationUserID
                }).ToList();
            }
            else
            {
                View.OrganizationUserList = OrganizationUserList.Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName + " " + x.LastName,
                    OrganizationUserID = x.OrganizationUserID
                }).ToList();
            }
        }

        public void GetPermissionList(Boolean OnlyPackagePermissions)
        {
            if (OnlyPackagePermissions)
            {
                View.UserPacakgePermissionList = ComplianceSetupManager.GetPermissionList(View.TenantId,OnlyPackagePermissions).ToList();
            }
            else
            {
                View.UserPermissionList = ComplianceSetupManager.GetPermissionList(View.TenantId, OnlyPackagePermissions).ToList();
            }
        }

        public void SaveHierarchyPermission()
        {
            List<String> lstHierarchyPermissionTypeCode = new List<String>() { HierarchyPermissionTypes.COMPLIANCE.GetStringValue() };
            if (View.IsIncludeAnotherHierarchyPermissionType)
            {
                lstHierarchyPermissionTypeCode.Add(HierarchyPermissionTypes.BACKGROUND.GetStringValue());
            }
            HierarchyPermission hierarchyPermission = new HierarchyPermission();
            hierarchyPermission.HP_OrganizationUserID = View.OrganizationUserID;
            hierarchyPermission.HP_PermissionID = View.PermissionId;
            hierarchyPermission.HP_HierarchyID = View.DeptProgramMappingID;
            hierarchyPermission.HP_IsDeleted = false;
            hierarchyPermission.HP_CreatedBy = View.CurrentLoggedInUserId;
            hierarchyPermission.HP_CreatedOn = DateTime.Now;
            hierarchyPermission.HP_VerificationPermissionID = View.VerificationPermissionId;
            hierarchyPermission.HP_ProfilePermissionID = View.ProfilePermissionId;
            //UAT-1181: Ability to restrict additional nodes to the order queue
            hierarchyPermission.HP_OrderQueuePermissionID = View.OrderQueuePermissionId;
            hierarchyPermission.HP_PackagePermissionID = View.PackagePermissionID;  // UAT 2834
           
            if (ComplianceSetupManager.SaveHierarchyPermission(View.TenantId, hierarchyPermission, lstHierarchyPermissionTypeCode))
            {
                View.SuccessMessage = "User Hierarchy Permission mapping saved successfully.";
            }
            else
            {
                View.ErrorMessage = "An error occured while mapping User Hierarchy Permission. Please try again.";
            }

        }

        public void UpdateHierarchyPermission()
        {
            HierarchyPermission hierarchyPermission = ComplianceSetupManager.GetHierarchyPermissionByID(View.TenantId, View.HierarchyPermissionID);
            if (hierarchyPermission.IsNotNull())
            {
                hierarchyPermission.HP_ID = View.HierarchyPermissionID;
                hierarchyPermission.HP_PermissionID = View.PermissionId;
                hierarchyPermission.HP_ModifiedBy = View.CurrentLoggedInUserId;
                hierarchyPermission.HP_ModifiedOn = DateTime.Now;
                hierarchyPermission.HP_ProfilePermissionID = View.ProfilePermissionId;
                hierarchyPermission.HP_VerificationPermissionID = View.VerificationPermissionId;
                //UAT-1181: Ability to restrict additional nodes to the order queue
                hierarchyPermission.HP_OrderQueuePermissionID = View.OrderQueuePermissionId;
                hierarchyPermission.HP_PackagePermissionID = View.PackagePermissionID;  // UAT 2834
                if (ComplianceSetupManager.UpdateHierarchyPermission(View.TenantId))
                {
                    View.SuccessMessage = "User Hierarchy Permission mapping updated successfully.";
                }
                else
                {
                    View.ErrorMessage = "An error occured while updating User Hierarchy Permission. Please try again.";
                }
            }
            else
            {
                View.ErrorMessage = "An error occured while updating User Hierarchy Permission. Please try again.";
            }


        }

        public void DeleteHierarchyPermission()
        {
            HierarchyPermission hierarchyPermission = ComplianceSetupManager.GetHierarchyPermissionByID(View.TenantId, View.HierarchyPermissionID);
            if (hierarchyPermission.IsNotNull())
            {
                hierarchyPermission.HP_IsDeleted = true;
                hierarchyPermission.HP_ModifiedBy = View.CurrentLoggedInUserId;
                hierarchyPermission.HP_ModifiedOn = DateTime.Now;

                if (ComplianceSetupManager.DeleteHierarchyPermission(View.TenantId))
                {
                    View.SuccessMessage = "User Hierarchy Permission mapping deleted successfully.";
                }
                else
                {
                    View.ErrorMessage = "An error occured while deleting User Hierarchy Permission. Please try again.";
                }
            }
            else
            {
                View.ErrorMessage = "An error occured while deleting User Hierarchy Permission. Please try again.";
            }
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (GetTenantId() == SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }


        #region Private Methods


        #endregion

        public void SaveArchivalGracePeriod(Int32? archivalGracePeriodValue)
        {
            if (!ComplianceSetupManager.SaveArchivalGracePeriod(View.DeptProgramMappingID, archivalGracePeriodValue, View.CurrentLoggedInUserId, View.TenantId))
            {
                View.ErrorMessage = "An error occured while updating Archival Grace Period. Please try again.";
            }
        }

        public void GetEffectiveArchivalGracePeriod()
        {
            Dictionary<String, Int32> dicObj = new Dictionary<String, Int32>();
            dicObj = ComplianceSetupManager.GetEffectiveArchivalGracePeriod(View.DeptProgramMappingID, View.CurrentLoggedInUserId, View.TenantId);
            View.EffectiveArchivalGracePeriod = dicObj.GetValue("EffectiveArchivalGracePeriod");
            View.NeedEffectiveArchival = dicObj.GetValue("NeedEffectiveArchival");
        }

        #region UAT-1011 Drag/Drop Nodes
        public Boolean UpdateNodeDisplayOrder(List<GetChildNodesWithPermission> lstDPMIds, Int32? destinationIndex)
        {
            return StoredProcedureManagers.UpdateNodeDisplayOrder(lstDPMIds, destinationIndex, View.CurrentLoggedInUserId, View.TenantId);
        }
        #endregion

        #region UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.

        /// <summary>
        /// Get Payment Approval List
        /// </summary>
        public void GetPaymentApprovals()
        {
            String notSpecifiedApprovalCode = PaymentApproval.NOT_SPECIFIED.GetStringValue();
            var paymentApprovalList = LookupManager.GetLookUpData<lkpPaymentApproval>(View.TenantId).Where(con => con.PA_IsDeleted == false).ToList();
            if (paymentApprovalList.IsNotNull())
            {
                foreach (var paymentApproval in paymentApprovalList)
                {
                    if (paymentApproval.PA_Code == PaymentApproval.APPROVAL_REQUIRED_BEFORE_PAYMENT.GetStringValue())
                        paymentApproval.PA_Name = AppConsts.YES;
                    else if (paymentApproval.PA_Code == PaymentApproval.APPROVAL_NOT_REQUIRED_BEFORE_PAYMENT.GetStringValue())
                        paymentApproval.PA_Name = AppConsts.NO;
                }

                View.PaymentApprovalList = paymentApprovalList;
                View.PaymentApprovalIDAddMode = paymentApprovalList.FirstOrDefault(con => con.PA_Code == notSpecifiedApprovalCode).PA_ID;
            }
        }

        #endregion

        #region UAT-3873

        /// <summary>
        /// To get Program Background Packages
        /// </summary>
        public void GetProgramAvailablePackages()
        {
            View.lstNodePackagesDetails = ComplianceSetupManager.GetProgramAvailablePackagesByProgramMapId(View.DeptProgramMappingID, View.TenantId);
        }
      

        #endregion

        /// <summary>
        /// Method to get file extensions
        /// </summary>
        public void BindFileExtensions()
        {
            View.ListFileExtension = ComplianceSetupManager.BindFileExtensions(View.TenantId).ToList();
        }

        /// <summary>
        /// To get selected Payment Options
        /// </summary>
        public void GetSelectedFileExtensions()
        {
            View.SelectedMappedFileExtensions = ComplianceSetupManager.GetSelectedFileExtensions(View.TenantId, View.DeptProgramMappingID)
                                                                     .Select(x => x.DPRFE_FileExtensionID).ToList();
        }
    }
}




