using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.ServiceProxy.Modules.ApplicantOperations;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RequirementVerificationDetailPresenter : Presenter<IRequirementVerificationDetail>
    {
        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private ApplicantClinicalRotationProxy _applicantClinicalRotationProxy
        {
            get
            {
                return new ApplicantClinicalRotationProxy();
            }
        }

        /// <summary>
        /// Get the UserData from Security database, by OrganizationUserID.
        /// </summary>
        public void GetApplicantData()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.SelectedApplicantId;
            serviceRequest.Parameter2 = View.SelectedTenantId;
            var _serviceResponse = _clientRotationProxy.GetUserData(serviceRequest);
            View.ApplicantData = _serviceResponse.Result;
        }

        /// <summary>
        /// Get the Requirement Verification Details Screen data, including the data entered by Applicant.
        /// </summary>
        public void GetVerificationDetailData()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.RPSId;
            serviceRequest.Parameter2 = View.SelectedTenantId;
            var _serviceResponse = _clientRotationProxy.GetVerificationDetailData(serviceRequest);
            View.lstVerificationDetailData = _serviceResponse.Result;
        }


        /// <summary>
        /// Get the Requirement Verification Details Screen data, including the data entered by Applicant.
        /// </summary>
        public void GetReqItemStatusTypes()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.SelectedTenantId;
            var _serviceResponse = _clientRotationProxy.GetRequirementItemStatusTypes(serviceRequest);
            View.lstReqItemStatusTypes = _serviceResponse.Result;
        }

        public Dictionary<Int32, String> SaveData()
        {
            ServiceRequest<RequirementVerificationData, Int32> serviceRequest = new ServiceRequest<RequirementVerificationData, Int32>();
            serviceRequest.Parameter1 = View.DataToSave;
            serviceRequest.Parameter2 = View.SelectedTenantId;
            Boolean isNewPackage = false;
            var _serviceResponse = _clientRotationProxy.SaveVerificationData(serviceRequest, ref isNewPackage);
            Boolean isSuccess = !_serviceResponse.Result.Any();
            if (isSuccess)
            {
                if (!isNewPackage)
                {
                    EvaluateRequirementBuisnessRules();
                }
                else
                {
                    EvaluateRequirementDynamicBuisnessRules();
                }
                SendNotificationsForItmAndPkgStatusChange();
            }
            return _serviceResponse.Result;
        }

        private void SendNotificationsForItmAndPkgStatusChange()
        {
            if (!View.lstVerificationDetailData.IsNullOrEmpty())
            {
                var _lstData = View.lstVerificationDetailData;

                var _distinctCatIds = _lstData.Select(vdd => vdd.CatId).Distinct().ToList();
                foreach (var catId in _distinctCatIds)
                {
                    var _distinctItems = _lstData.Where(vdd => vdd.CatId == catId).DistinctBy(cond => cond.ItemId).ToList();
                    foreach (var _item in _distinctItems)
                    {
                        ComplianceDataManager.SendNotificationOnItemStatusChangedToReviewStatus(true, View.SelectedTenantId, View.RPSId, AppConsts.NONE, catId, _item.ItemId, View.CurrentLoggedInUserId, View.SelectedApplicantId, _item.ItemStatusCode);
                    }
                }

                ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(View.SelectedTenantId, View.RPSId, View.CurrentLoggedInUserId, View.lstVerificationDetailData[0].PkgStatusCode);
            }
        }

        /// <summary>
        /// Execute the business rules after data is saved
        /// </summary>
        private void EvaluateRequirementBuisnessRules()
        {
            var _lstData = View.lstVerificationDetailData;
            var _currentPkgId = _lstData.First().PkgId;

            List<RequirementRuleObject> ruleObjectMappingList = new List<RequirementRuleObject>();

            var _distinctCatIds = _lstData.Select(vdd => vdd.CatId).Distinct().ToList();
            foreach (var catId in _distinctCatIds)
            {
                ruleObjectMappingList.Add(new RequirementRuleObject
                {
                    RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                    RuleObjectId = Convert.ToString(catId),
                    RuleObjectParentId = Convert.ToString(AppConsts.NONE)
                });

                var _distinctItemIds = _lstData.Where(vdd => vdd.CatId == catId).Select(vdd => vdd.ItemId).Distinct().ToList();
                foreach (var itemId in _distinctItemIds)
                {
                    ruleObjectMappingList.Add(new RequirementRuleObject
                    {
                        RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                        RuleObjectId = Convert.ToString(itemId),
                        RuleObjectParentId = Convert.ToString(catId)
                    });
                }
            }

            ServiceRequest<List<RequirementRuleObject>, Int32> serviceRequest = new ServiceRequest<List<RequirementRuleObject>, Int32>();
            serviceRequest.Parameter1 = ruleObjectMappingList;
            serviceRequest.Parameter2 = View.RPSId;
            serviceRequest.SelectedTenantId = View.SelectedTenantId;

            _clientRotationProxy.ExecuteRequirementObjectBuisnessRules(serviceRequest);
        }

        private void EvaluateRequirementDynamicBuisnessRules()
        {
            var _lstData = View.lstVerificationDetailData;
            var _currentPkgId = _lstData.First().PkgId;

            List<RequirementRuleObject> ruleObjectMappingList = new List<RequirementRuleObject>();

            var _distinctCatIds = _lstData.Select(vdd => vdd.CatId).Distinct().ToList();
            foreach (var catId in _distinctCatIds)
            {
                ruleObjectMappingList.Add(new RequirementRuleObject
                {
                    RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                    RuleObjectId = Convert.ToString(catId),
                    RuleObjectParentId = Convert.ToString(AppConsts.NONE)
                });

                var _distinctItemIds = _lstData.Where(vdd => vdd.CatId == catId).Select(vdd => vdd.ItemId).Distinct().ToList();
                foreach (var itemId in _distinctItemIds)
                {
                    ruleObjectMappingList.Add(new RequirementRuleObject
                    {
                        RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                        RuleObjectId = Convert.ToString(itemId),
                        RuleObjectParentId = Convert.ToString(catId)
                    });
                    var _distinctFieldIds = _lstData.Where(vdd => vdd.CatId == catId && vdd.ItemId == itemId).Select(vdd => vdd.FieldId).Distinct().ToList();
                    foreach (var fieldId in _distinctFieldIds)
                    {
                        ruleObjectMappingList.Add(new RequirementRuleObject
                        {
                            RuleObjectTypeCode = ObjectType.Compliance_ATR.GetStringValue(),
                            RuleObjectId = Convert.ToString(fieldId),
                            RuleObjectParentId = Convert.ToString(itemId)
                        });
                    }
                }
            }

            ServiceRequest<List<RequirementRuleObject>, Int32> serviceRequest = new ServiceRequest<List<RequirementRuleObject>, Int32>();
            serviceRequest.Parameter1 = ruleObjectMappingList;
            serviceRequest.Parameter2 = View.RPSId;
            serviceRequest.SelectedTenantId = View.SelectedTenantId;

            _applicantClinicalRotationProxy.EvaluateRequirementPostSubmitRules(serviceRequest);
        }
    }
}
