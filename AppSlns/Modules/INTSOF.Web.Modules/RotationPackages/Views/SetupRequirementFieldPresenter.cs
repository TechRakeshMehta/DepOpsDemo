#region NameSpace
#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion
#region Project Specific
using INTSOF.SharedObjects;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;
using Entity.SharedDataEntity;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
#endregion
#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class SetupRequirementFieldPresenter : Presenter<ISetupRequirementFieldView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            GetUniversalAttributes();
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        public void GetRequirementFieldDetail()
        {
            if (View.RequirementFieldID > AppConsts.NONE)
            {
                ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
                serviceRequest.Parameter1 = View.RequirementFieldID;
                serviceRequest.Parameter2 = View.RequirementCategoryId;
                ServiceResponse<RequirementFieldContract> serviceResponse = _requirementPackageProxy.GetRequirementFieldDataByID(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.FieldData = serviceResponse.Result;
                    View.IsCustomSettings = serviceResponse.Result.IsCustomSetting;
                    if (!serviceResponse.Result.SelectedEditableBy.IsNullOrEmpty())
                        View.lstEditableBy = serviceResponse.Result.SelectedEditableBy;
                }
            }
        }

        public RequirementItemContract GetRequirementItemDetail()
        {
            if (View.RequirementItemID > AppConsts.NONE)
            {
                ServiceRequest<Int32, Int32,Int32?> serviceRequest = new ServiceRequest<Int32, Int32,Int32?>();
                serviceRequest.Parameter1 = View.RequirementFieldID;
                serviceRequest.Parameter2 = View.RequirementItemID;
                serviceRequest.Parameter3 = View.RequirementCategoryId;
                ServiceResponse<RequirementItemContract> serviceResponse = _requirementPackageProxy.GetRequirementItemDetail(serviceRequest);
                return serviceResponse.Result;
            }
            return new RequirementItemContract();
        }

        public void SaveUpdateRequirementFieldData(RequirementFieldContract reqFieldContarct)
        {
            if ((View.UniversalAttributeData.IsNullOrEmpty() || View.UniFieldID > AppConsts.NONE)
            || (!View.UniversalAttributeData.IsNullOrEmpty() && View.UniversalAttributeData.UniversalFieldMappingID != View.UniFieldMappingID))
            {
                reqFieldContarct.UniversalAttributeData = new UniversalAttributeContract();
                reqFieldContarct.UniversalAttributeData.UniversalFieldMappingID = View.UniFieldMappingID == AppConsts.NONE ? View.UniversalAttributeData.UniversalFieldMappingID : View.UniFieldMappingID;
                reqFieldContarct.UniversalAttributeData.UniversalFieldID = View.UniFieldID;

                if (reqFieldContarct.IsNewPackage)
                {
                    reqFieldContarct.UniversalAttributeData.IsNewPackage = true;
                }

                reqFieldContarct.UniversalAttributeData.lstAttributeInputData = View.UniversalAttributeData.lstAttributeInputData;
                reqFieldContarct.UniversalAttributeData.lstOptionMapping = View.UniversalAttributeData.lstOptionMapping;
            }
            reqFieldContarct.SelectedEditableBy = View.lstEditableBy;
            reqFieldContarct.IsCustomSetting = View.IsCustomSettings;
            ServiceRequest<RequirementFieldContract> serviceRequest = new ServiceRequest<RequirementFieldContract>();
            serviceRequest.Parameter = reqFieldContarct;
            ServiceResponse<Int32> serviceResponse = _requirementPackageProxy.SaveUpdateRequirementFieldData(serviceRequest);
            if (serviceResponse.Result < AppConsts.NONE)
            {
                View.ErrorMessage = "Some error has occurred.Please try again.";
            }
            else
            {
                //SaveUpdateAttributeInputPriority();
                View.RequirementFieldID = serviceResponse.Result;
                if (View.IsEditMode)
                {
                    GetUniversalAttributeDetails();
                }
            }
        }

        public Boolean DeleteReqItemFieldMapping(Int32 reqItemFieldID)
        {
            ServiceRequest<Int32, String, Boolean, Int32> serviceRequest = new ServiceRequest<Int32, String, Boolean, Int32>();
            serviceRequest.Parameter1 = reqItemFieldID;
            serviceRequest.Parameter2 = View.ItemHId;
            serviceRequest.Parameter3 = View.IsNewPackage;
            serviceRequest.Parameter4 = View.RequirementCategoryId;
            ServiceResponse<String> serviceResponse = _requirementPackageProxy.DeleteReqItemFieldMapping(serviceRequest);
            View.ErrorMessage = serviceResponse.Result;
            return View.ErrorMessage.IsNullOrEmpty() ? true : false;
        }

        public void GetRequirementItemFields()
        {
            View.lstItemFields = new List<RequirementFieldContract>();
            if (View.RequirementItemID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.RequirementItemID;
                ServiceResponse<List<RequirementFieldContract>> serviceResponse = _requirementPackageProxy.GetRequirementFieldsByItemID(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstItemFields = serviceResponse.Result.OrderBy(x => x.RequirementFieldDisplayOrder).ToList(); //UAT-3078
                }
            }
        }

        public void GetRotationFieldDataTypes()
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = AppConsts.NONE;
            serviceRequest.Parameter2 = true;
            ServiceResponse<List<RotationFieldDataTypeContract>> _serviceResponse = _requirementPackageProxy.GetRotationFieldDataTypes(serviceRequest);
            View.LstRotationFieldDataType = _serviceResponse.Result;
        }

        #region UAT-2305

        public void GetUniversalAttributes()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.RequirementItemID;
            serviceRequest.Parameter2 = View.RequirementCategoryId;
            ServiceResponse<List<UniversalAttributeContract>> _serviceResponse = _requirementPackageProxy.GetUniversalAttributes(serviceRequest);
            if (!_serviceResponse.Result.IsNullOrEmpty())
            {
                View.lstUniversalAttributes = _serviceResponse.Result;
            }
        }
        public void GetUniversalAttributeDetails()
        {
            ServiceRequest<Int32, Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32, Int32>();
            serviceRequest.Parameter1 = View.RequirementItemID;
            serviceRequest.Parameter2 = View.RequirementFieldID;
            serviceRequest.Parameter3 = View.RequirementCategoryId;
            ServiceResponse<UniversalAttributeContract> _serviceResponse = _requirementPackageProxy.GetUniversalFieldAttributeDetails(serviceRequest);
            if (!_serviceResponse.Result.IsNullOrEmpty())
            {
                View.UniversalAttributeData = _serviceResponse.Result;
                if (!View.UniversalAttributeData.IsNullOrEmpty() && View.UniversalAttributeData.UniversalFieldMappingID > AppConsts.NONE)
                {
                    View.UniFieldMappingID = View.UniversalAttributeData.UniversalFieldMappingID;
                    View.UniFieldID = View.UniversalAttributeData.UniversalFieldID;
                    GetAtrInputPriority(View.UniversalAttributeData.UniversalFieldMappingID);
                }
            }
        }
        public void GetAtrInputPriority(Int32 UniFieldMappingID)
        {
            if (UniFieldMappingID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = UniFieldMappingID;
                ServiceResponse<List<InputTypeComplianceAttributeServiceContract>> _serviceResponse = _requirementPackageProxy.GetUniversalFieldAtrInputPriorityByID(serviceRequest);
                if (!_serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstSelectedInputAttribute = _serviceResponse.Result;
                }
                else
                {
                    View.lstSelectedInputAttribute = new List<InputTypeComplianceAttributeServiceContract>();
                }
            }
        }
        public void SaveUpdateAttributeInputPriority()
        {
            if (!View.UniversalAttributeData.IsNullOrEmpty() && View.UniFieldID > AppConsts.NONE)
            {
                if (View.UniversalAttributeData.UniItmAttrMappingID == AppConsts.NONE)
                    View.UniversalAttributeData.UniItmAttrMappingID = View.UniFieldID;
                ServiceRequest<UniversalAttributeContract> serviceRequest = new ServiceRequest<UniversalAttributeContract>();
                serviceRequest.Parameter = View.UniversalAttributeData;
                ServiceResponse<Boolean> _serviceResponse = _requirementPackageProxy.SaveUpdateAttributeInputPriority(serviceRequest);
            }
        }

        public Dictionary<Int32, String> GetUniversalAtrOptionData(Int32 UniFieldID)
        {
            //if (View.UniItmAttrMappingID > AppConsts.NONE)
            if (UniFieldID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = UniFieldID;
                ServiceResponse<Dictionary<Int32, String>> _serviceResponse = _requirementPackageProxy.GetUniversalFieldAtrOptionData(serviceRequest);
                if (!_serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstAttributeOptionValue = _serviceResponse.Result;
                    return _serviceResponse.Result;
                }
            }
            return new Dictionary<Int32, String> { };
        }

        public List<String> GetUniversalAtrOptionSelected(Int32 uniFieldOptionID, Int32 uniFieldMappingID)
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = uniFieldOptionID;
            serviceRequest.Parameter2 = uniFieldMappingID;
            ServiceResponse<List<Int32>> _serviceResponse = _requirementPackageProxy.GetUniversalFieldAtrOptionSelected(serviceRequest);
            if (!_serviceResponse.Result.IsNullOrEmpty())
            {
                List<String> result = new List<String>();
                foreach (var item in _serviceResponse.Result)
                {
                    result.Add(item.ToString());
                }
                return result;
            }
            return new List<String> { AppConsts.ZERO };
        }
        #endregion

        #region Rules

        public void GetRequirementRuleDetail()
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            if (View.RequirementFieldID > AppConsts.NONE)
                serviceRequest.Parameter = String.Concat("2-", View.RequirementCategoryId, "|3-", View.RequirementItemID, "|4-", View.RequirementFieldID);
            else
                serviceRequest.Parameter = String.Concat("2-", View.RequirementCategoryId, "|3-", View.RequirementItemID);
            ServiceResponse<Int32> serviceResponse1 = _requirementPackageProxy.GetRequirementObjectTreeIDByprntID(serviceRequest);
            if (!serviceResponse1.Result.IsNullOrEmpty())
            {
                View.RequirementObjectTreeID = serviceResponse1.Result;
            }

            ServiceRequest<Int32> serviceRequest2 = new ServiceRequest<Int32>();
            serviceRequest2.Parameter = View.RequirementObjectTreeID;
            ServiceResponse<List<RequirementRuleContract>> serviceResponse = _requirementPackageProxy.GetReqFixedRuleDetailByObjectTreeID(serviceRequest2);
            if (!serviceResponse.Result.IsNullOrEmpty())
            {
                View.lstItemRules = serviceResponse.Result;
            }
        }
        public void SaveUpdateRequiremntRule()
        {
            if (View.RequirementFieldID > AppConsts.NONE && !View.IsEditMode)
            {
                ServiceRequest<Int32> serviceRequest2 = new ServiceRequest<Int32>();
                serviceRequest2.Parameter = View.RequirementFieldID;
                ServiceResponse<List<Int32>> serviceResponse2 = _requirementPackageProxy.GetRequirementObjectTreeIDByReqFieldID(serviceRequest2);

                if (!serviceResponse2.Result.IsNullOrEmpty())
                {
                    foreach (Int32 ROTID in serviceResponse2.Result.ToList())
                    {
                        ServiceRequest<List<RequirementRuleContract>, Int32, Int32> serviceRequest = new ServiceRequest<List<RequirementRuleContract>, Int32, Int32>();
                        serviceRequest.Parameter1 = View.lstItemRules;
                        serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
                        serviceRequest.Parameter3 = ROTID;
                        ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.SaveUpdateNewRequirementFieldRule(serviceRequest);
                        if (!serviceResponse.Result.IsNullOrEmpty())
                        {
                            //View.lstItemRules = serviceResponse.Result;
                        }
                    }
                }
            }
            else
            {
                ServiceRequest<List<RequirementRuleContract>, Int32, Int32> serviceRequest = new ServiceRequest<List<RequirementRuleContract>, Int32, Int32>();
                serviceRequest.Parameter1 = View.lstItemRules;
                serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
                serviceRequest.Parameter3 = View.RequirementObjectTreeID;
                ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.SaveUpdateNewRequirementFieldRule(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    //View.lstItemRules = serviceResponse.Result;
                }
            }
        }
        #endregion

        #region UAT-2788
        public void GetComplianceAttributeType()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = AppConsts.NONE;
            ServiceResponse<List<RequirementFieldType>> _serviceResponse = _requirementPackageProxy.GetAttributeType(serviceRequest);
            View.lstFieldType = _serviceResponse.Result;
        }
        #endregion

        #region UAT-3176
        public void GetRequirementAttributeGroups()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = AppConsts.NONE;
            ServiceResponse<List<RequirementAttributeGroups>> _serviceResponse = _requirementPackageProxy.GetRequirementAttributeGroups(serviceRequest);
            View.lstAttributeGroups = _serviceResponse.Result;
        }
        #endregion
        #region UAT-4001
        public List<RequirementDocumentAcroFieldType> GetDocumentAcroFieldType()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = AppConsts.NONE;
            ServiceResponse<List<RequirementDocumentAcroFieldType>> serviceResponse = _requirementPackageProxy.GetDocumentAcroFieldTypeData(serviceRequest);
            if (!serviceResponse.Result.IsNullOrEmpty())
            {
                return serviceResponse.Result;
            }
            return new List<RequirementDocumentAcroFieldType>();
        }
        #endregion
    }
}
