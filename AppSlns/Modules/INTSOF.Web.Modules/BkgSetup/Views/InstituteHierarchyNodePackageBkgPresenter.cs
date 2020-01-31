using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public class InstituteHierarchyNodePackageBkgPresenter : Presenter<IInstituteHierarchyNodePackageBkgView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            GetExemptedHierarchyNodeOptions();
        }

        /// <summary>
        /// To get Institution Node Types
        /// </summary>
        public void GetInstitutionNodeTypes()
        {
            var institutionNodeTypes = ComplianceSetupManager.GetInstitutionNodeTypes(View.SelectedTenantId);
            if (institutionNodeTypes.IsNotNull())
            {
                String institutionCode = NodeType.Institution.GetStringValue();
                View.ListInstitutionNodeType = institutionNodeTypes.Where(x => x.INT_Code != institutionCode).OrderBy(col => col.INT_Name).ToList();
            }
        }

        /// <summary>
        /// To get Institution Nodes
        /// </summary>
        public void GetInstitutionNodes()
        {
            View.ListInstitutionNode = ComplianceSetupManager.GetInstitutionNodes(View.SelectedTenantId, View.SelectedNodeTypeId).OrderBy(col => col.IN_Name).ToList();
        }

        /// <summary>
        /// To get node label/name
        /// </summary>
        public void GetNodeLabel()
        {
            var deptProgramMapping = MobilityManager.GetDeptProgramMappingById(View.DeptProgramMappingID, View.SelectedTenantId);
            if (deptProgramMapping.IsNotNull() && deptProgramMapping.InstitutionNode.IsNotNull())
            {
                View.NodeLabel = "Node: " + deptProgramMapping.InstitutionNode.IN_Name;
            }
        }

        /// <summary>
        /// To get node Availability setting.
        /// </summary>
        public void GetNodeAvailability()
        {
            var deptProgramMapping = MobilityManager.GetDeptProgramMappingById(View.DeptProgramMappingID, View.SelectedTenantId);
            if (deptProgramMapping.IsNotNull())
            {
                View.IsAvailableForOrderEditMode = deptProgramMapping.DPM_IsAvailableForOrder.IsNull() ||
                                                   Convert.ToBoolean(deptProgramMapping.DPM_IsAvailableForOrder)
                                                   ? true
                                                   : false;

                View.IsRootNode = deptProgramMapping.DPM_ParentNodeID.IsNull() ? true : false;

                //UAT-1176 - To Get Current Settings of Employment Type of selected Node.
                View.IsEmploymentTypeEditMode = (deptProgramMapping.DPM_IsEmployment ?? false)
                                                ? true
                                                : false;
                //To get splash url
                View.SplashPageUrlEditMode = deptProgramMapping.DPM_SplashPageUrl.IsNull() ? String.Empty : deptProgramMapping.DPM_SplashPageUrl;
                //UAT-2073
                View.PaymentApprovalID = Convert.ToInt32(deptProgramMapping.DPM_PaymentApprovalID);

                View.PDFInclusionID = deptProgramMapping.DPM_BGPDFInclusionID == null ? ClientSecurityManager.GetPDFInclusionOptions(View.SelectedTenantId).Where(x => x.Code.Equals(PDFInclusionOptions.Not_Specified.GetStringValue())).FirstOrDefault().PDFInclusionID : Convert.ToInt32(deptProgramMapping.DPM_BGPDFInclusionID);
                View.ResultSentToApplicantID = deptProgramMapping.DPM_ResultSentToApplicantID == null ? ClientSecurityManager.GetResultSentToApplicantOptions(View.SelectedTenantId).Where(x => x.RSTA_Code.Equals(ResultsSentToApplicantOptions.Default.GetStringValue())).FirstOrDefault().RSTA_ID : Convert.ToInt32(deptProgramMapping.DPM_ResultSentToApplicantID);

                if (deptProgramMapping.DPM_ParentNodeID == null)
                {
                    View.NodeExemptedInRotaionEditMode = deptProgramMapping.DPM_NodeExemptedInRotaionID == null ? ClientSecurityManager.GetBkgHierarchyNodeExemptedType(View.SelectedTenantId).Where(x => x.HNET_Code.Equals(ExemptedHierarchyNodeValueType.No.GetStringValue())).FirstOrDefault().HNET_ID : Convert.ToInt32(deptProgramMapping.DPM_NodeExemptedInRotaionID);
                }
                else
                {
                    View.NodeExemptedInRotaionEditMode = deptProgramMapping.DPM_NodeExemptedInRotaionID == null ? ClientSecurityManager.GetBkgHierarchyNodeExemptedType(View.SelectedTenantId).Where(x => x.HNET_Code.Equals(ExemptedHierarchyNodeValueType.Default.GetStringValue())).FirstOrDefault().HNET_ID : Convert.ToInt32(deptProgramMapping.DPM_NodeExemptedInRotaionID);
                }
            }
        }

        /// <summary>
        /// To get selected node label/name
        /// </summary>
        public void GetSelectedNodeLabel(string selectedNodeID)
        {
            List<InstitutionNode> lstInstitutionNodes = ComplianceSetupManager.GetInstitutionNodes(View.SelectedTenantId, View.SelectedNodeTypeId);
            View.SelectedNodeLabel = lstInstitutionNodes.Where(x => x.IN_ID == Convert.ToInt32(selectedNodeID)).Select(cond => cond.IN_Label).FirstOrDefault();
        }

        /// <summary>
        /// method to get payment options
        /// </summary>
        public void GetPaymentOptions()
        {
            View.ListPaymentOption = ClientSecurityManager.GetAllPaymentOption(View.SelectedTenantId).ToList();
        }

        /// <summary>
        /// To get selected Payment Options
        /// </summary>
        public void GetSelectedPaymentOptions()
        {
            View.SelectedMappedPaymentOptions = ClientSecurityManager.GetMappedDepProgramPaymentOption(View.SelectedTenantId, View.DeptProgramMappingID)
                                                                     .Select(x => x.DPPO_PaymentOptionID).ToList();
        }

        /// <summary>
        /// Get the list of External Vendor Account and Institution Hierarchy Vendor Account Mapping for a given node
        /// </summary>
        /// <returns></returns>
        public void GetInstHierarchyVendorAcctMappingDetails()
        {
            View.ListExtVendorAcctMappingDetails = BackgroundSetupManager.GetInstHierarchyVendorAcctMappingDetails(View.SelectedTenantId, View.DeptProgramMappingID);
        }

        /// <summary>
        /// Get the list of Filtered (yet not selected) External Vendor Account for a given node
        /// </summary>
        /// <returns></returns>
        public void GetExternalVendorAccountsNotMapped()
        {
            List<Entity.ExternalVendorAccount> lstExternalVendorAccount = BackgroundSetupManager.GetExternalVendorAccountsNotMapped(View.SelectedTenantId, View.DeptProgramMappingID)
                .Where(obj => obj.EVA_VendorID == View.SelectedExtVendorID).ToList();
            if (lstExternalVendorAccount.Count > 0)
                View.ListExtVendorAcct = lstExternalVendorAccount;
            else
                View.InfoMessage = "No more External Vendor Account of this vendor is available for mapping";
        }

        /// <summary>
        /// Get the list of Filtered (yet not selected) External Vendor Account for a given node
        /// </summary>
        /// <returns></returns>
        public void GetExternalVendor()
        {
            View.ListExtVendor = BackgroundSetupManager.GetVendors();
        }

        public Boolean CheckIfResidentialHistoryAttributeGroupsMappedWithPkg()
        {
            List<MappedResidentialHistoryAttributeGroupsWithPkg> listAttrGrp = BackgroundSetupManager.GetMappedResidentialHistoryAttributeGroupsWithPkg(View.SelectedTenantId, View.SelectedPackageId);
            if (listAttrGrp.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Get the list of Filtered (yet not selected) Regulatory Entity Type for a given node
        /// </summary>
        /// <returns></returns>
        public void GetRegulatoryEntityTypeNotMapped()
        {
            List<Entity.lkpRegulatoryEntityType> lstEntity = BackgroundSetupManager.FetchRegulatoryEntityTypeNotMapped(View.SelectedTenantId, View.DeptProgramMappingID);
            if (lstEntity.Count > 0)
                View.ListRegulatoryEntityType = lstEntity.OrderBy(col => col.RET_Name).ToList();
            else
                View.InfoMessage = "No more Regulatory Entity is available for mapping";
        }


        /// <summary>
        /// Get the list of Inst Hierarchy Regulatory Entity Type Mappings for a given node
        /// </summary>
        /// <returns></returns>
        public void GetInstHierarchyRegulatoryEntityMappingDetails()
        {
            View.ListRegulatoryEntityMappingDetails = BackgroundSetupManager.GetInstHierarchyRegulatoryEntityMappingDetails(View.SelectedTenantId, View.DeptProgramMappingID);
        }


        /// <summary>
        /// To save Program Package Mapping Node
        /// </summary>
        public void SaveProgramPackageMappingNode()
        {
            ComplianceSetupManager.SaveProgramPackageMappingNode(View.DeptProgramMappingID, View.SelectedNodeId, View.SelectedNodeName, View.SelectedPaymentOptions, new List<Int32>(),
                View.CurrentLoggedInUserId, View.SelectedTenantId, View.SelectedNodeLabel, View.IsAvailableForOrderAddMode, View.IsEmploymentTypeAddMode, View.SplashPageUrlAddMode, string.Empty,null,null,null,null ,View.PaymentApprovalIDAddMode, true, PDFInclusionID: View.PDFInclusionIDAddMode, resultsSentToApplicantID: View.ResultSentToApplicantIDAddMode, hierarchyNodeExemptedType: View.NodeExemptedInRotaionAddMode);
        }

        /// <summary>
        /// To get childnodes of selected node.
        /// </summary>
        public void GetNodeList()
        {
            //Organization User Id is passed as null because we do not want to include permissin related checks for background setup.
            ObjectResult<GetChildNodesWithPermission> objChildNodesWithPermission = ComplianceSetupManager.GetChildNodesWithPermission(View.DeptProgramMappingID, null, View.SelectedTenantId);
            if (objChildNodesWithPermission.IsNotNull())
            {
                var nodeList = objChildNodesWithPermission.OrderBy(x => x.DPM_DisplayOrder).ToList();
                View.NodeList = nodeList;
                List<Int32> deptProgramMappingIDs = nodeList.Select(x => x.DPM_ID).ToList();
                //View.ChildNodeList = ComplianceSetupManager.GetInstitutionChildNodesByProgramMapId(deptProgramMappingIDs, View.SelectedTenantId);
            }
        }

        /// <summary>
        /// To delete node
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteNode()
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.CheckIfBkgNodeAssociated(View.NodeId, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                String deptProMappingName = ComplianceSetupManager.GetDeptProgMappingLabel(View.NodeId, View.SelectedTenantId);
                View.InfoMessage = String.Format(response.UIMessage, deptProMappingName);
                return false;
            }
            else if( FingerPrintDataManager.IsLocationMapped(View.SelectedTenantId,View.NodeId))
            {
                View.InfoMessage = "You can't delete this node because appointment(s) has been scheduled from this node";
                return false;
            }
            else
            {
                if (  FingerPrintDataManager.DeleteTenantNodeLocationMapping(View.SelectedTenantId,View.NodeId,View.CurrentLoggedInUserId) && ComplianceSetupManager.DeleteProgramPackageMappingByID(View.NodeId, View.CurrentLoggedInUserId, View.SelectedTenantId))
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
        /// To get Program Packages
        /// </summary>
        public void GetProgramPackages()
        {
            View.ProgramPackages = BackgroundSetupManager.GetProgramPackagesByHierarchyMappingId(View.DeptProgramMappingID, View.SelectedTenantId);

        }


        /// <summary>
        /// To save mapped Payment Options and Update the availability of the node, for the Order process
        /// </summary>
        public void SaveNodeSettings()
        {
            ComplianceSetupManager.SaveMappedPaymentOptionsNodeAvailability(View.DeptProgramMappingID, View.SelectedMappedPaymentOptions, new List<Int32>(),View.CurrentLoggedInUserId, View.IsAvailableForOrderEditMode, View.IsEmploymentTypeEditMode, View.SelectedTenantId, View.SplashPageUrlEditMode, String.Empty,null, null,null,null,null, View.PaymentApprovalID, null, View.PDFInclusionID, View.ResultSentToApplicantID, View.NodeExemptedInRotaionEditMode, true);
        }

        /// <summary>
        /// To get not mapped Compliance Packages
        /// </summary>
        /// <returns></returns>
        public List<BackgroundPackage> GetNotMappedCompliancePackages()
        {
            return BackgroundSetupManager.GetNotMappedBackGroungPackagesByMappingId(View.DeptProgramMappingID, View.SelectedTenantId).OrderBy(col => col.BPA_Name).ToList();
        }

        /// <summary>
        /// To save Program Package Mapping
        /// </summary>
        public void SaveProgramPackageMapping()
        {
            List<lkpPackageAvailability> pkgAvailability = ComplianceSetupManager.GetPackageAvailablity(View.SelectedTenantId);
            Int32 paID = 0;
            String code = String.Empty;
            if (View.IsBkgPackageAvailableForOrder)
            {
                code = PackageAvailability.AVAILABLE_FOR_ORDER.GetStringValue();
                paID = pkgAvailability.FirstOrDefault(x => x.PA_Code == code).PA_ID;
            }
            else
            {
                code = PackageAvailability.NOT_AVAILABLE_FOR_ORDER.GetStringValue();
                paID = pkgAvailability.FirstOrDefault(x => x.PA_Code == code).PA_ID;
            }

            BkgPackageHierarchyMapping newMapping = new BkgPackageHierarchyMapping();

            newMapping.BPHM_InstitutionHierarchyNodeID = View.DeptProgramMappingID;
            newMapping.BPHM_BackgroundPackageID = View.SelectedPackageId;
            newMapping.BPHM_PackageBasePrice = View.BasePrice;
            newMapping.BPHM_IsExclusive = View.IsPackageExclusive;
            newMapping.BPHM_IsActive = true;
            newMapping.BPHM_TransmitToVendor = View.TransmitToVendor;
            newMapping.BPHM_NeedFirstReview = View.RequireFirstReview;
            newMapping.BPHM_PkgSupplementalTypeID = (View.SelectedSupplemantalTypeID != AppConsts.NONE) ? View.SelectedSupplemantalTypeID : (short?)null;
            newMapping.BPHM_Instructions = View.Instruction;
            newMapping.BPHM_CustomPriceText = View.PriceText;
            newMapping.BPHM_MaxNumberOfYearforResidence = View.MaxNumberOfYearforResidence;
            newMapping.BPHM_PackageAvailabilityID = paID;
            newMapping.BPHM_IsAvailableForAdminEntry = View.IsBkgPackageAvailableForHRPortal;
            //UAT-2073
            newMapping.BPHM_PaymentApprovalID = View.PaymentApprovalIDForPackage;
            //UAT-3268
            if (View.lstBkgPackages.Where(cond => cond.BPA_ID == View.SelectedPackageId).Select(sel => sel.BPA_IsReqToQualifyInRotation).FirstOrDefault())
            {
                if (!View.IsAdditionalPriceAvailable.IsNullOrEmpty() && View.IsAdditionalPriceAvailable)
                {
                    newMapping.BPHM_IsAdditionalPriceAvailable = View.IsAdditionalPriceAvailable;
                    newMapping.BPHM_AdditionalPricePaymentOptionID = View.SelectedAdditonalPaymentOptionID;
                    newMapping.BPHM_AdditionalPrice = View.AdditionalPrice;
                }
            }
            if (BackgroundSetupManager.SaveHierarchyNodePackageMapping(newMapping, View.CurrentLoggedInUserId, View.lstPaymentOptionIds, View.SelectedTenantId))
            {
                View.SuccessMessage = "Package saved successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error occurred. Please try again.";
            }
        }

        /// <summary>
        /// To delete Program Package Mapping
        /// </summary>
        public void DeleteProgramPackageMapping()
        {

            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.CheckIfBkgPackageHasAnyOrder(View.BkgPackageHierarchyMappingID, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                String bkgPackage = ComplianceSetupManager.GetBkgpackageOfNode(View.BkgPackageHierarchyMappingID, View.SelectedTenantId);
                // View.ErrorMessage = String.Format(response.UIMessage, bkgPackage);
                View.InfoMessage = String.Format(response.UIMessage, bkgPackage);
                //return false;
            }
            else
            {
                if (!BackgroundSetupManager.DeleteHirarchyPackageMappingByID(View.BkgPackageHierarchyMappingID, View.CurrentLoggedInUserId, View.SelectedTenantId))
                {
                    View.ErrorMessage = "Some error occurred. Please try again.";
                }
            }
        }

        /// <summary>
        /// To save External Vendor Account Mapping
        /// </summary>
        public void SaveExternalVendorAccountMapping()
        {
            if (View.SelectedExtVendorAcctID > -1)
            {
                InstHierarchyVendorAcctMapping newMapping = new InstHierarchyVendorAcctMapping();

                newMapping.DPMEVAM_DeptProgramMappingID = View.DeptProgramMappingID;
                newMapping.DPMEVAM_IsDeleted = false;
                newMapping.DPMEVAM_ExternalVendorAccountID = View.SelectedExtVendorAcctID;
                newMapping.DPMEVAM_CreatedById = View.CurrentLoggedInUserId;
                newMapping.DPMEVAM_CreatedOn = DateTime.Now;

                if (BackgroundSetupManager.SaveInstHierarchyVendorAcctMapping(View.SelectedTenantId, newMapping))
                {
                    View.SuccessMessage = "External Vendor Account mapping saved successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error occurred. Please try again.";
                }
            }
            else
            {
                View.ErrorMessage = "Some error occurred. Please try again.";
            }
        }

        /// <summary>
        /// To save External Vendor Account Mapping
        /// </summary>
        public void SaveInstHierarchyRegulatoryEntityMapping()
        {
            if (View.SelectedRegulatoryEntityTypeID > -1)
            {
                InstHierarchyRegulatoryEntityMapping newMapping = new InstHierarchyRegulatoryEntityMapping();

                newMapping.IHRE_RegulatoryEntityTypeID = View.SelectedRegulatoryEntityTypeID;
                newMapping.IHRE_IsDeleted = false;
                newMapping.IHRE_InstitutionHierarchyID = View.DeptProgramMappingID;
                newMapping.IHRE_CreatedById = View.CurrentLoggedInUserId;
                newMapping.IHRE_CreatedOn = DateTime.Now;

                if (BackgroundSetupManager.SaveInstHierarchyRegulatoryEntityMapping(View.SelectedTenantId, newMapping))
                {
                    View.SuccessMessage = "Regulatory Entity mapping saved successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error occurred. Please try again.";
                }
            }
            else
            {
                View.ErrorMessage = "Some error occurred. Please try again.";
            }
        }

        /// <summary>
        /// To delete External Vendor Account Mapping
        /// </summary>
        public void DeleteExternalVendorAccountMapping(Int32 mappingID, Int32 vendorID)
        {

            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfVendorAccountMappingIsAssociated(View.SelectedTenantId, View.DeptProgramMappingID, vendorID);
            if (response.CheckStatus == CheckStatus.True)
            {
                View.InfoMessage = String.Format(response.UIMessage);
            }
            else
            {
                InstHierarchyVendorAcctMapping instHierarchyVendorAcctMappingInDB = BackgroundSetupManager.GetInstHierarchyVendorAcctMappingByID(View.SelectedTenantId, mappingID);
                instHierarchyVendorAcctMappingInDB.DPMEVAM_IsDeleted = true;
                instHierarchyVendorAcctMappingInDB.DPMEVAM_ModifiedByID = View.CurrentLoggedInUserId;
                instHierarchyVendorAcctMappingInDB.DPMEVAM_ModifiedOn = DateTime.Now;
                if (BackgroundSetupManager.UpdateTenantChanges(View.SelectedTenantId))
                    View.SuccessMessage = "External Vendor Account unmapped successfully";
                else
                    View.ErrorMessage = "Some error occurred. Please try again.";
            }
        }

        /// <summary>
        /// To delete External Vendor Account Mapping
        /// </summary>
        public void DeleteInstHierarchyRegulatoryEntityMapping(Int32 mappingID)
        {
            InstHierarchyRegulatoryEntityMapping mapping = BackgroundSetupManager.GetInstHierarchyRegEntityMappingByID(View.SelectedTenantId, mappingID);
            mapping.IHRE_IsDeleted = true;
            mapping.IHRE_ModifiedByID = View.CurrentLoggedInUserId;
            mapping.IHRE_ModifiedOn = DateTime.Now;
            if (BackgroundSetupManager.UpdateTenantChanges(View.SelectedTenantId))
                View.SuccessMessage = "Regulatory Entity unmapped successfully";
            else
                View.ErrorMessage = "Some error occurred. Please try again.";

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

        /// <summary>
        ///Updates Background Sequence.
        /// </summary>
        public Boolean UpdateBackgroundPackageSequence(IList<BkgPackageHierarchyMapping> hierarchyPackagesToMove, Int32? destinationIndex)
        {

            return BackgroundSetupManager.UpdateBackgroundPackageSequence(hierarchyPackagesToMove, destinationIndex, View.CurrentLoggedInUserId, View.SelectedTenantId);
        }
        public void SupplementalTypeList()
        {
            View.LstSupplemantalType = BackgroundSetupManager.SupplementalTypeList(View.SelectedTenantId);
        }

        #region UAT-1011 Drag/Drop Nodes
        public Boolean UpdateNodeDisplayOrder(List<GetChildNodesWithPermission> lstDPMIds, Int32? destinationIndex)
        {
            return StoredProcedureManagers.UpdateNodeDisplayOrder(lstDPMIds, destinationIndex, View.CurrentLoggedInUserId, View.SelectedTenantId);
        }
        #endregion

        #region UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.

        /// <summary>
        /// Get Payment Approval List from lkpPaymentApproval table
        /// </summary>
        public void GetPaymentApprovals()
        {
            String notSpecifiedApprovalCode = PaymentApproval.NOT_SPECIFIED.GetStringValue();
            var paymentApprovalList = LookupManager.GetLookUpData<lkpPaymentApproval>(View.SelectedTenantId).Where(con => con.PA_IsDeleted == false).ToList();
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

        #region UAT-2438

        /// <summary>
        /// method to get PDF inclusion options
        /// </summary>
        public void GetPDFInclusionOptions()
        {
            var PDFInclusionOption = ClientSecurityManager.GetPDFInclusionOptions(View.SelectedTenantId).ToList();
            View.ListPDFInclusionOption = PDFInclusionOption;
            View.PDFInclusionIDAddMode = PDFInclusionOption.Where(x => x.Code.Equals(PDFInclusionOptions.Not_Specified.GetStringValue())).FirstOrDefault().PDFInclusionID;
        }
        #endregion

        #region UAT-2842
        public void GetResultSentToApplicantOptions()
        {
            var lstResultSentToApplicantOptions = ClientSecurityManager.GetResultSentToApplicantOptions(View.SelectedTenantId).ToList();
            View.ListResultSentToApplicantOptions = lstResultSentToApplicantOptions;
            View.ResultSentToApplicantIDAddMode = lstResultSentToApplicantOptions.Where(cond => cond.RSTA_Code.Equals(ResultsSentToApplicantOptions.Default.GetStringValue())).FirstOrDefault().RSTA_ID;
        }

        public void GetExemptedHierarchyNodeOptions()
        {
            var lstExemptedHierarchyNodeOptions = ClientSecurityManager.GetBkgHierarchyNodeExemptedType(View.SelectedTenantId).ToList();
            View.ListExemptedHierarchyNodeOptions = lstExemptedHierarchyNodeOptions;
            View.NodeExemptedInRotaionAddMode = lstExemptedHierarchyNodeOptions.Where(cond => cond.HNET_Code.Equals(ExemptedHierarchyNodeValueType.Default.GetStringValue())).FirstOrDefault().HNET_ID;
        }

        public void BindAdditionalPricePaymentOption()
        {
            var lstPaymentOptions = LookupManager.GetLookUpData<Entity.ClientEntity.lkpPaymentOption>(View.SelectedTenantId).Where(con => con.IsDeleted == false).ToList();
            if (!lstPaymentOptions.IsNullOrEmpty())
            {
                View.AdditionalPaymentOptions = lstPaymentOptions.Where(cond => cond.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()
                                                                             || cond.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()).ToList();
            }
        }
        #endregion

        #region
        public int GetDpmLocationCount()
        {
            return FingerPrintDataManager.GetDPMLocations(View.SelectedTenantId, View.DeptProgramMappingID).Count();
        }
        #endregion
    }
}
