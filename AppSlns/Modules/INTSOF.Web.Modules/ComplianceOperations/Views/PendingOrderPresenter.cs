#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#region UserDefined

using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System.Text;
using INTSOF.UI.Contract.BkgOperations;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class PendingOrderPresenter : Presenter<IPendingOrderView>
    {
        #region Variables

        #region Private Variables



        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties



        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
            GetHierarchyNodes(true);
            ShowRushOrderSetting();
            GetTrackingPackageLabels();
            GetScreeningPackageLabels();
            //UAT-1214:Changes to "Required" and "Optional" labels in order flow
            // UAT 1545 - WB: Change to order package selection screen
            /* GetBackgroundOrderPackageLabels();*/
        }

        public void GetChangeSubscriptionSourceNodeId()
        {
            List<CurrentNodePredecessors> lstCurrentNodeHierarchy;
            DeptProgramPackage _deptProgramPackage = ComplianceDataManager.GetChangeSubscriptionSourceNode(View.PreviousOrderId, View.TenantId);
            View.ChangeSubscriptionSourceNodeId = _deptProgramPackage.DPP_DeptProgramMappingID;
            View.ChangeSubscriptionSourceNodeDPPId = _deptProgramPackage.DPP_ID;
            View.ChangeSubscriptionCompliancePackageTypeId = _deptProgramPackage.CompliancePackage.CompliancePackageTypeID;
        }

        public void GetHierarchyNodes(Boolean isParent)
        {
            Int32 _nodeId = 0;
            if (isParent)
                _nodeId = View.TenantId;
            else
                _nodeId = View.SelectedNodeId;

            Int32 _possibleNodeId = 0;
            View.lstHierarchy = ComplianceDataManager.GetHierarchyNode(_nodeId, View.TenantId, isParent, out _possibleNodeId, View.ChangeSubscriptionSourceNodeId, View.ChangeSubscriptionSourceNodeDPPId, View.LanguageCode);
            View.ChangeSubscriptionTargetNodeId = Convert.ToString(_possibleNodeId);
        }


        public void GetDeptProgramPackage()
        {
            List<MobilityNodePackages> _lstMobilityNodePackages = new List<MobilityNodePackages>();
            //List<DeptProgramPackage> _deptPrgrmPackage = ComplianceDataManager.GetDeptProgramPackage(View.CurrentLoggedInUserId, View.TenantId, View.SelectedHierarchyNodeIds, out _lstMobilityNodePackages);
            Dictionary<String, List<DeptProgramPackage>> dicDepPrgrmPackage = new Dictionary<String, List<DeptProgramPackage>>();

            Int32? previousPkgId = null;
            Int32? previousNodeId = null;
            if (Convert.ToString(View.OrderType) == OrderRequestType.ChangeSubscription.GetStringValue())
            {
                var packageSubs = ComplianceDataManager.GetPackageSubscriptionDetailByOrderId(View.TenantId, View.PreviousOrderId);
                previousPkgId = packageSubs.CompliancePackageID;
                previousNodeId = packageSubs.Order.SelectedNodeID;
            }
            dicDepPrgrmPackage = ComplianceDataManager.GetCompliancePackages(View.CurrentLoggedInUserId, View.TenantId, View.SelectedHierarchyNodeIds, previousPkgId, previousNodeId);
            List<DeptProgramPackage> _purchasedDepPrgrmPkg = new List<DeptProgramPackage>();
            List<DeptProgramPackage> _deptPrgrmPackage = dicDepPrgrmPackage.ContainsKey("AvailablePurchasedPkg") == true ? dicDepPrgrmPackage["AvailablePurchasedPkg"] : null;

            //UAT-729 As an applicant, if I have an active Compliance package, that package should not appear as an option in the order process.
            _purchasedDepPrgrmPkg = dicDepPrgrmPackage.ContainsKey("AlreadyPurchasedPkg") == true ? dicDepPrgrmPackage["AlreadyPurchasedPkg"] : new List<DeptProgramPackage>();
            if (_purchasedDepPrgrmPkg.IsNotNull() && _purchasedDepPrgrmPkg.Count > 0)
            {
                StringBuilder strBuilder = new StringBuilder();
                _purchasedDepPrgrmPkg.ForEach(cond =>
                {
                    strBuilder.Append((String.IsNullOrEmpty(cond.CompliancePackage.PackageLabel) ? cond.CompliancePackage.PackageName : cond.CompliancePackage.PackageLabel) + ", ");
                });

                View.AlreadyPurchasedPackages = strBuilder.ToString().Remove(strBuilder.Length - 2).TrimEnd();
            }
            else
                View.AlreadyPurchasedPackages = String.Empty;

            View.DeptProgramPackages = _deptPrgrmPackage;
            if (_deptPrgrmPackage.IsNotNull() && _deptPrgrmPackage.Count > 0)
            {
                if (Convert.ToString(View.OrderType) == OrderRequestType.ChangeSubscription.GetStringValue() && View.ChangeSubscriptionCompliancePackageTypeId != AppConsts.NONE)
                {
                    _deptPrgrmPackage.RemoveAll(dpp => dpp.CompliancePackage.CompliancePackageTypeID != View.ChangeSubscriptionCompliancePackageTypeId);

                    //UAT-3259 
                    List<Int32> lstAlreadyExpiredComplPackages = ComplianceDataManager.GetAlreadyExpiredComplPackageIds(View.CurrentLoggedInUserId, View.TenantId);
                    if (!lstAlreadyExpiredComplPackages.IsNullOrEmpty() && lstAlreadyExpiredComplPackages.Count > AppConsts.NONE)
                        _deptPrgrmPackage.RemoveAll(dpp => lstAlreadyExpiredComplPackages.Contains(dpp.CompliancePackage.CompliancePackageID));
                }
                //START UAT 1185 Rajeev Jha -commenting out below 2 lines
                //View.DeptProgramPackage = _deptPrgrmPackage[0];
                //View.ProgramDuration = View.DeptProgramPackage.IsNull() ? null : View.DeptProgramPackage.DeptProgramMapping.InstitutionNode.IN_Duration;
                View.AvailableComplaincePackageTypes = (from d in _deptPrgrmPackage select d.CompliancePackage.lkpCompliancePackageType.CPT_Code).Distinct().ToList<string>();
                //END UAT-1185

                if (_deptPrgrmPackage.Count() > 0 && _deptPrgrmPackage.IsNotNull())
                {
                    View.NodeId = _deptPrgrmPackage[0].DeptProgramMapping.DPM_InstitutionNodeID;
                }

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
            // return _lstMobilityNodePackages;
        }

        public void GetDeptProgramPackageSubscription()
        {
            View.lstDeptProgramPackageSubscription = ComplianceDataManager.GetDeptProgramPackageSubscription(View.DeptProgramPackage.DPP_ID, View.TenantId);
        }

        public Int32 GetTenant()
        {
            Entity.Organization _org = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization;
            View.InstitutionName = _org.OrganizationName;
            return _org.TenantID.Value;
        }

        public void GetDepartments()
        {
            Entity.Organization organization = SecurityManager.GetOrganizationForTenantID(View.TenantId);
            View.Departments = SecurityManager.GetDepartments(organization.OrganizationID).ToList();
        }

        /// <summary>
        /// Get the Background Packages for the Selected Node
        /// </summary>
        /// <param name="DpmId"></param>
        /// <returns></returns>
        public List<BackgroundPackagesContract> GetBackgroundPackages(Dictionary<Int32, Int32> dicDPMIds, Int32 organizationUserId, Int32 tenantId, Boolean IsLocationServiceTenant)
        {
            dicDPMIds = dicDPMIds.OrderByDescending(dpmId => dpmId.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            StringBuilder _sbDMPIds = new StringBuilder();
            _sbDMPIds.Append("<DPMIds>");
            foreach (var _dpmId in dicDPMIds)
            {
                _sbDMPIds.Append("<DPMId nodeLevel='" + _dpmId.Key + "'>" + _dpmId.Value + "</DPMId>");
            }
            _sbDMPIds.Append("</DPMIds>");

            List<BackgroundPackagesContract> _tempList = ComplianceDataManager.GetBackgroundPackages(Convert.ToString(_sbDMPIds), organizationUserId, tenantId, IsLocationServiceTenant);
            List<BackgroundPackagesContract> _finalList = new List<BackgroundPackagesContract>();

            foreach (var _dpmId in dicDPMIds)
            {
                if (_tempList.Any(bp => bp.NodeLevel == _dpmId.Key))
                {
                    _finalList.AddRange(_tempList.Where(bp => bp.NodeLevel == _dpmId.Key).ToList());
                    break;
                }
            }
            if (_finalList.IsNotNull() && _finalList.Count > AppConsts.NONE)
                _finalList = IncludeParentNodeBackgroundPackages(_tempList, dicDPMIds, _finalList);

            return _finalList.OrderBy(x => x.DisplayOrder).ToList();
        }

        private List<BackgroundPackagesContract> IncludeParentNodeBackgroundPackages(List<BackgroundPackagesContract> _tempList, Dictionary<Int32, Int32> dicDPMIds, List<BackgroundPackagesContract> _finalList)
        {
            Dictionary<Int32, Int32> dicDPMId = dicDPMIds.OrderByDescending(dpmId => dpmId.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            dicDPMId.Remove(dicDPMIds.Where(x => x.Key == _finalList.FirstOrDefault().NodeLevel).FirstOrDefault().Key);

            foreach (var _dpmId in dicDPMId)
            {
                if (_tempList.Any(bp => bp.NodeLevel == _dpmId.Key))
                {
                    _finalList.AddRange(_tempList.Where(bp => bp.NodeLevel == _dpmId.Key && _finalList.All(x => x.PackageTypeCode != bp.PackageTypeCode)).ToList());
                }
            }

            return _finalList;
        }
        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.PendingOrder);
        }


        public DeptProgramPackage GetDeptProgramPackageById(Int32 departmentProgramPackageId, Int32 tenantId)
        {
            //return ComplianceDataManager.GetDeptProgramPackageById(departmentProgramPackageId, tenantId, out _lstMobilityNodePackages);
            return ComplianceDataManager.GetDeptProgramPackageById(departmentProgramPackageId, tenantId);
        }

        public Int32 GetDefaultNodeId(Int32 tenantId)
        {
            return ComplianceDataManager.GetDefaultNodeId(tenantId);
        }

        public String GetInstitutionDPMID()
        {
            return ComplianceDataManager.GetInstitutionDPMID(View.TenantId).ToString();
        }

        public Int32 GetLastNodeInstitutionId(Int32 selectedDPMId)
        {
            return ComplianceDataManager.GetLastNodeInstitutionId(selectedDPMId, View.TenantId);
        }


        /// <summary>
        /// To show Rush Order depending on client settings
        /// </summary>
        public void ShowRushOrderSetting()
        {
            List<lkpSetting> lkpSettingList = ComplianceDataManager.GetSettings(View.TenantId);
            List<ClientSetting> clientSettingList = ComplianceDataManager.GetClientSetting(View.TenantId);
            Int32 rushOrderID = lkpSettingList.WhereSelect(cond => cond.Code == Setting.Enable_Rush_Order.GetStringValue(), col => col.SettingID).FirstOrDefault();
            String enableRushOrderValue = clientSettingList.WhereSelect(t => t.CS_SettingID == rushOrderID, col => col.CS_SettingValue).FirstOrDefault();
            View.ShowRushOrder = String.IsNullOrEmpty(enableRushOrderValue) ? false : ((enableRushOrderValue == "0") ? false : true);
        }

        /// <summary>
        /// Gets the total number of Custom forms available for the selected background packages
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public Int32 GetCustomFormsCount(String packageId)
        {
            List<CustomFormDataContract> lstCustomForm = BackgroundProcessOrderManager.GetCustomFormsForThePackage(View.TenantId, packageId);
            if (!lstCustomForm.IsNullOrEmpty())
                return lstCustomForm.DistinctBy(cstFrm => cstFrm.customFormId).Count();

            return AppConsts.NONE;
        }

        /// <summary>
        /// Get the payment options
        /// </summary>
        /// <param name="dpmId">Will be used, in case, when NO Compliance package was selected for the purchase.</param>
        public Boolean IfInvoiceIsOnlyPaymentOptions(Int32 dpmId)
        {
            List<lkpPaymentOption> paymentOptions = ComplianceDataManager.GetPaymentOptionsByDPMId(View.TenantId, dpmId);
            if (paymentOptions.Count == 1)
            {
                return paymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
            }
            else if (paymentOptions.Count == 2)
            {
                return (paymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()) && paymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()));
            }
            return false;
        }

        public BackroundOrderContract GetPreviousOrderHistory(Int32 organizationUserId, Int32 tenantId)
        {
            BackroundOrderContract _tempList = ComplianceDataManager.GetPreviousOrderHistory(organizationUserId, tenantId);
            return _tempList;
        }

        #endregion

        #region Private Methods



        #endregion

        #endregion

        #region UAT-1214
        private void GetBackgroundOrderPackageLabels()
        {
            /*List<ClientSetting> lstClientSetting = ComplianceDataManager.GetBkgOrdFlowLabelSetting(View.TenantId, Setting.BACKGROUND_ORDER_FLOW_REQUIRED_LABEL.GetStringValue(),
                                                                                                  Setting.BACKGROUND_ORDER_FLOW_OPTIONAL_LABEL.GetStringValue());
            //Required Package Label
            if (lstClientSetting.FirstOrDefault(x => x.lkpSetting.Code == Setting.BACKGROUND_ORDER_FLOW_REQUIRED_LABEL.GetStringValue()).IsNullOrEmpty())
            {
                View.RequiredPackageLabel = AppConsts.BACKGROUND_ORDER_FLOW_REQUIRED_DEFAULT_LABEL;
            }
            else
            {
                View.RequiredPackageLabel = lstClientSetting.FirstOrDefault(x => x.lkpSetting.Code == Setting.BACKGROUND_ORDER_FLOW_REQUIRED_LABEL.GetStringValue()).CS_SettingValue;
            }

            //Optional Package Label 
            // UAT 1545: WB: Change to order package selection screen
            if (lstClientSetting.FirstOrDefault(x => x.lkpSetting.Code == Setting.BACKGROUND_ORDER_FLOW_OPTIONAL_LABEL.GetStringValue()).IsNullOrEmpty())
            {
                View.OptionalPackageLabel = AppConsts.BACKGROUND_ORDER_FLOW_OPTIONAL_DEFAULT_LABEL;
            }
            else
            {
                View.OptionalPackageLabel = lstClientSetting.FirstOrDefault(x => x.lkpSetting.Code == Setting.BACKGROUND_ORDER_FLOW_OPTIONAL_LABEL.GetStringValue()).CS_SettingValue;
            }*/
        }
        #endregion

        /// <summary>
        /// Get the labels to be displayed for Tracking Package sections.
        /// </summary>
        private void GetTrackingPackageLabels()
        {
            List<String> lstCodes = new List<String>();
            lstCodes.Add(Setting.ORDER_FLOW_ADMINISTRATIVE_PACKAGE_SECTION_LABEL.GetStringValue());
            lstCodes.Add(Setting.ORDERFLOW_IMMINIZATION_PACKAGE_SECTION_LABEL.GetStringValue());

            List<ClientSetting> lstClientSetting = ComplianceDataManager.GetClientSettingsByCodes(View.TenantId, lstCodes);
            var _setting = lstClientSetting.FirstOrDefault(cs => cs.lkpSetting.Code == Setting.ORDERFLOW_IMMINIZATION_PACKAGE_SECTION_LABEL.GetStringValue());
            if (_setting.IsNullOrEmpty())
            {
                View.ImmnuizationPackageLabel = AppConsts.ORDER_FLOW_IMMUNIZATIONPKG_DEFAULT_LABEL;
            }
            else
            {
                View.ImmnuizationPackageLabel = _setting.CS_SettingValue;
            }

            _setting = lstClientSetting.FirstOrDefault(cs => cs.lkpSetting.Code == Setting.ORDER_FLOW_ADMINISTRATIVE_PACKAGE_SECTION_LABEL.GetStringValue());
            if (_setting.IsNullOrEmpty())
            {
                View.AdministrativePackageLabel = AppConsts.ORDER_FLOW_ADMINISTRATIVEPKG_DEFAULT_LABEL;
            }
            else
            {
                View.AdministrativePackageLabel = _setting.CS_SettingValue;
            }
        }
        ///<summary>
        ///Get the labels to be displayed for Screening package
        ///</summary>
        private void GetScreeningPackageLabels()
        {
            List<String> lstCodes = new List<String>();
            lstCodes.Add(Setting.ORDERFLOW_SCREENING_PACKAGE_SECTION_LABEL.GetStringValue());
            List<ClientSetting> lstClientSetting = ComplianceDataManager.GetClientSettingsByCodes(View.TenantId, lstCodes, View.LanguageCode);
            var _setting = lstClientSetting.FirstOrDefault(x => x.lkpSetting.Code == Setting.ORDERFLOW_SCREENING_PACKAGE_SECTION_LABEL.GetStringValue());
            //var islocationServicetenant = SecurityManager.IsLocationServiceTenant(View.TenantId);
            if (_setting.IsNullOrEmpty())
            {
                View.ScreeningHeaderLabel = AppConsts.SCREENING_PACKAGE_HEADER_DEFAULT_LABEL;
            }
            else
            {
                View.ScreeningHeaderLabel = _setting.CS_SettingValueLangugaeSpecific;
            }
        }

        /// <summary>
        /// Get the list of Bundles as per selected criteria
        /// </summary>
        public void GetPackageBundlesAvailableForOrder()
        {
            View.lstPackageBundle = PackageBundleManager.GetPackageBundlesAvailableForOrder(View.CurrentLoggedInUserId, View.SelectedHierarchyNodeIds, View.TenantId);
        }

        /// <summary>
        /// Get the detailed packages of the Bundles available.
        /// </summary>
        /// <returns></returns>
        public List<PackageBundleNodePackage> GetPackagelistAvailableUnderBundle()
        {
            var _lstBundleIds = View.lstPackageBundle.Select(pbu => pbu.PBU_ID).ToList();

            List<PackageBundleNodePackage> lstPackages = PackageBundleManager.GetBundlePackages(_lstBundleIds, View.TenantId);

            View.lstBundleDeptProgramPackages = lstPackages.Where(sel => sel.PBNP_BkgPackageHierarchyMappingID == null).ToList();
            View.lstBundleBkgPackages = lstPackages.Where(sel => sel.BkgPackageHierarchyMapping != null && sel.BkgPackageHierarchyMapping.BackgroundPackage.BPA_IsAvailableForApplicantOrder && sel.PBNP_DeptProgramPackageID == null).ToList();
            //View.lstBundleBkgPackages = lstPackages.Where(sel => sel.BkgPackageHierarchyMapping != null && sel.BkgPackageHierarchyMapping.BackgroundPackage.BPA_IsAvailableForApplicantOrder && sel.PBNP_DeptProgramPackageID == null && sel.BkgPackageHierarchyMapping.lkpPackageAvailability.PA_Code == (PackageAvailability.AVAILABLE_FOR_ORDER.GetStringValue())).ToList();

            if (!View.lstBundleDeptProgramPackages.IsNullOrEmpty())
            {
                if (View.DeptProgramPackages.IsNull())
                {
                    View.DeptProgramPackages = new List<DeptProgramPackage>();
                }

                View.DeptProgramPackages.AddRange(View.lstBundleDeptProgramPackages.Select(sel => sel.DeptProgramPackage).Where(cond => !cond.DPP_IsDeleted).ToList());
                //View.AvailableComplaincePackageTypes = (from d in View.DeptProgramPackages select d.CompliancePackage.lkpCompliancePackageType.CPT_Code).Distinct().ToList<string>();
            }

            return lstPackages;

            //View.lstBundleBkgPackages = new List<BackgroundPackagesContract>();
            //if (lstCompliancePackages.IsNotNull() && lstCompliancePackages.Count > AppConsts.NONE)
            //{
            //    // View.DeptProgramPackages = lstCompliancePackages.Select(sel => sel.DeptProgramPackage).Where(cond => !cond.DPP_IsDeleted).ToList();
            //    //View.lstBundleDeptProgramPackages = lstCompliancePackages.Select(sel => sel.DeptProgramPackage).Where(cond => !cond.DPP_IsDeleted).ToList();
            //    View.AvailableComplaincePackageTypes = (from d in View.DeptProgramPackages select d.CompliancePackage.lkpCompliancePackageType.CPT_Code).Distinct().ToList<string>();
            //}
            //else
            //{
            //    //View.DeptProgramPackages = new List<DeptProgramPackage>();
            //    View.lstBundleDeptProgramPackages = new List<PackageBundleNodePackage>();
            //}
            //if (lstBkgPackages.IsNotNull() && lstBkgPackages.Count > AppConsts.NONE)
            //{
            //    foreach (var bkgPackage in lstBkgPackages)
            //    {
            //        BkgPackageHierarchyMapping bkgPackageHierarchyMapping = bkgPackage.BkgPackageHierarchyMapping;
            //        View.lstBundleBkgPackages.Add(new BackgroundPackagesContract
            //      {
            //          BPAId = bkgPackageHierarchyMapping.BackgroundPackage.BPA_ID,
            //          BPAName = bkgPackageHierarchyMapping.BackgroundPackage.BPA_Label.IsNullOrEmpty() ? bkgPackageHierarchyMapping.BackgroundPackage.BPA_Name
            //                                                                                  : bkgPackageHierarchyMapping.BackgroundPackage.BPA_Label,
            //          BPAViewDetails = bkgPackageHierarchyMapping.BackgroundPackage.BPA_IsViewDetailsInOrderEnabled,
            //          BPHMId = bkgPackageHierarchyMapping.BPHM_ID,
            //          IsExclusive = bkgPackageHierarchyMapping.BPHM_IsExclusive,
            //          BasePrice = bkgPackageHierarchyMapping.BPHM_PackageBasePrice.Value,
            //          CustomPriceText = bkgPackageHierarchyMapping.BPHM_CustomPriceText,
            //          //NodeLevel = bkgPackageHierarchyMapping.,
            //          MaxNumberOfYearforResidence = bkgPackageHierarchyMapping.BPHM_MaxNumberOfYearforResidence.HasValue ?
            //                                                     bkgPackageHierarchyMapping.BPHM_MaxNumberOfYearforResidence.Value : -1,
            //          DisplayOrder = bkgPackageHierarchyMapping.BPHM_Sequence.Value,
            //          PackageDetail = bkgPackageHierarchyMapping.BackgroundPackage.BPA_PackageDetail.IsNullOrEmpty() ? String.Empty : bkgPackageHierarchyMapping.BackgroundPackage.BPA_PackageDetail,
            //          IsInvoiceOnlyAtPackageLevel = bkgPackageHierarchyMapping.BkgPackagePaymentOptions.Any()
            //      });
            //    }
            //}
        }

        #region UAT-1560: WB: We should be able to add documents that need to be signed to the order process

        public void GetAdditionalDocuments(List<BackgroundPackagesContract> lstBkgPackages, Int32 selectedHierarchyId, Dictionary<string,
                                           OrderCartCompliancePackage> CompliancePackages, Boolean isCompliancePackageSelected)
        {
            var packagesList = lstBkgPackages;
            List<Int32> BkgPackages = new List<Int32>();
            List<Int32> compPackageIdList = new List<Int32>();
            Boolean isAdditionalDocumentExist = false;
            if (!packagesList.IsNullOrEmpty())
            {
                foreach (var item in packagesList)
                {
                    BkgPackages.Add(item.BPAId);
                }
            }

            //Get Compliance package Ids.

            if (isCompliancePackageSelected)
            {
                CompliancePackages.ForEach(cnd =>
                {
                    if (!cnd.Value.IsNullOrEmpty())
                    {
                        compPackageIdList.Add(cnd.Value.CompliancePackageID);
                    }
                });
            }
            List<Entity.SystemDocument> additionalDocument = BackgroundSetupManager.GetAdditionalDocuments(BkgPackages, compPackageIdList, selectedHierarchyId, View.TenantId);
            if (!additionalDocument.IsNullOrEmpty())
            {
                isAdditionalDocumentExist = true;
            }
            View.IsAdditionalDocumentExist = isAdditionalDocumentExist;
        }
        #endregion

        //UAT-2754
        public Boolean IsNeedToShowBundlePackageNotes()
        {
            List<String> lstCodes = new List<String>();
            lstCodes.Add(Setting.DISPLAY_BUNDLE_PACKAGE_NOTES_SETTINGS.GetStringValue());
            List<ClientSetting> lstClientSetting = ComplianceDataManager.GetClientSettingsByCodes(View.TenantId, lstCodes);
            var _setting = lstClientSetting.FirstOrDefault(cs => cs.lkpSetting.Code == Setting.DISPLAY_BUNDLE_PACKAGE_NOTES_SETTINGS.GetStringValue());
            if (!_setting.IsNullOrEmpty())
            {
                return Convert.ToBoolean(Convert.ToInt32(_setting.CS_SettingValue));
            }
            else
            {
                return true;
            }
        }
        #region "UAT - 2802"
        public Boolean IsExistingNodeSelected(Int32 currentSelectedNodeId)
        {
            Boolean IsExistingNode = false;
            IsExistingNode = ComplianceDataManager.IsExistingNodeSelected(View.TenantId, View.CurrentLoggedInUserId, currentSelectedNodeId);
            return IsExistingNode;
        }

        public Boolean IsOrderFlowSettingsEnable()
        {
            Boolean IsClientSttings = ComplianceDataManager.IsClientOrderFlowMessageSetting(View.TenantId, Setting.ORDER_FLOW_MESSAGE_SETTING.GetStringValue());
            return IsClientSttings;
        }
        #endregion

        #region UAT:-2587:- Pop up for backgroung orders that will have multiple emails sent.

        public List<BackroundServicesContract> AcknowledgeMessagePopUpContent(String bkgPackageIds, Int32 selectedNodeId)
        {
            //Return the services name with service form and mails will be send.
            List<BackroundServicesContract> lstBkgServices = BackgroundProcessOrderManager.AcknowledgeMessagePopUpContent(View.TenantId, bkgPackageIds, selectedNodeId);
            return lstBkgServices;
        }
        #endregion



        public void GetLocationHierarchy()
        {
            View.lstLocationHierarchy = FingerPrintDataManager.GetLocationHierarchy(View.TenantId, View.FingerPrintData.LocationId);
        }
        public void GetServiceDescription()
        {
            View.ServiceDescription = FingerPrintDataManager.GetServiceDescription(View.TenantId);
        }
        //#region Cbi CABS
        //public void IsLocationServiceTenant()
        //{
        //    View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.TenantId);
        //}
        //#endregion

        public Boolean ValidateCBIUniqueID()
        {
            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic = FingerPrintDataManager.ValidateCBIUniqueID(View.TenantId, View.FingerPrintData.CBIUniqueID);
            Boolean IsValidID = Convert.ToBoolean(dic.GetValue("IsValidID"));
            View.CBIBillingCode = new KeyValuePair<string, string>("", "");
            if (IsValidID)
            {
                View.CBIBillingCode = new KeyValuePair<string, string>(View.FingerPrintData.CBIUniqueID, dic[AppConsts.CBIBillingCode]);
                dic.Remove(AppConsts.CBIBillingCode);
                View.FingerPrintData.lstAutoFilledAttributes = new Dictionary<Int32, String>();
                View.FingerPrintData.IsSSNRequired = dic.GetValue("IsSSNRequired") == "Y" ? true : false;
                View.FingerPrintData.IsLegalNameChange = dic.GetValue("IsLegalNameChange") == "true" ? true : false;
                dic.Remove("IsSSNRequired");
                dic.Remove("IsValidID");
                dic.Remove("IsLegalNameChange");
                foreach (var item in dic)
                {
                    View.FingerPrintData.lstAutoFilledAttributes.Add(Convert.ToInt32(item.Key), item.Value);
                }

                return true;
            }
            return false;
        }

        //UAT-3850

        public CBIBillingStatu GetCBIBillingStatusData()
        {
            CBIBillingStatu cbiBillingStatusData = new CBIBillingStatu();
            cbiBillingStatusData = FingerPrintDataManager.GetCBIBillingStatusData(View.TenantId, View.FingerPrintData.CBIUniqueID, View.BillingCode);
            return cbiBillingStatusData;
        }

        public void GetAdditionalServiceFeeOption()
        {            
            var FeeItemId = SecurityManager.GetAdditionalServiceFeeItemID();
            View.lstAdditionalServiceFeeOption = BackgroundPricingManager.GetAditionalServiceItemFeeRecordContract(SecurityManager.DefaultTenantID, FeeItemId);
        }


        #region UAT-3706
        public Boolean IsNoServicesExistInPackage(Int32 bkgPackageId, Int32 tenantId)
        {
            List<Entity.ClientEntity.GetBkgPackageDetailTree> lstBkgPackageDetails = ComplianceSetupManager.GetBkgPackageDetailTree(tenantId, bkgPackageId).ToList();
            if (!lstBkgPackageDetails.IsNullOrEmpty() && lstBkgPackageDetails.Count > AppConsts.ONE)
                return false;
            return true;

        }

        #region UAT-4271
        public List<LookupContract> GetCBIUniqueIdByAcctNameOrNumber(String acctNameOrNumber)
        {
            List<LookupContract> lstCBIUniqueIds = new List<LookupContract>();
            lstCBIUniqueIds = FingerPrintDataManager.GetCBIUniqueIdByAcctNameOrNumber(View.TenantId,acctNameOrNumber);
            return lstCBIUniqueIds;
        }


        #endregion

        #endregion
    }
}




