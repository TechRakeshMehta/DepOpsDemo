using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.RotationPackages.Views
{
    public class SetUpRequirementRulePresenter : Presenter<ISetUpRequirementRuleView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            GetRequirementRuleDetail();
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        public void GetRulesConstantTypes()
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = 1;
            serviceRequest.Parameter2 = true;
            ServiceResponse<List<RulesConstantTypeContract>> _serviceResponse = _requirementPackageProxy.GetRulesConstantTypes(serviceRequest);
            View.LstRulesConstantTypeContract = _serviceResponse.Result;
        }

        public void GetRequirementRuleDetail()
        {
            if (View.RequirementObjectTreeID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.RequirementObjectTreeID;
                ServiceResponse<List<RequirementRuleContract>> serviceResponse = _requirementPackageProxy.GetRequirementRuleDetail(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstItemRules = serviceResponse.Result;
                }
            }
        }

        public void GetRequirementItemDateFields(Int32 requirementItemId)
        {
            View.lstItemFields = new List<RequirementFieldContract>();
            if (requirementItemId > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = requirementItemId;
                ServiceResponse<List<RequirementFieldContract>> serviceResponse = _requirementPackageProxy.GetRequirementFieldsByItemID(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    var lstFields = serviceResponse.Result;

                    String dateTypeFieldCode = RequirementFieldDataType.DATE.GetStringValue();
                    var lstDateFields = lstFields.Where(cond => cond.RequirementFieldData.RequirementFieldDataTypeCode == dateTypeFieldCode).ToList();
                    View.lstItemFields = lstDateFields.OrderBy(x => x.RequirementFieldName).ToList();
                }
            }
        }

        public Boolean SaveUpdateRequirementRule()
        {
            ServiceRequest<List<RequirementRuleContract>> serviceRequest = new ServiceRequest<List<RequirementRuleContract>>();
            serviceRequest.Parameter = View.lstItemRules;
            ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.SaveUpdateRequirementRule(serviceRequest);
            return serviceResponse.Result;
        }

        public void GetRequirementCategoryItems()
        {
            View.lstCategoryItems = new List<RequirementItemContract>();
            if (View.RequirementCategoryID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.RequirementCategoryID;
                ServiceResponse<List<RequirementItemContract>> serviceResponse = _requirementPackageProxy.GetRequirementItemsByCategoryID(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstCategoryItems = serviceResponse.Result.OrderBy(x => x.RequirementItemName).ToList();
                }
            }
        }

        //UAT-3342
        public Boolean IsCalculatedAttribute()
        {
            if (View.fieldID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.fieldID;
                ServiceResponse<List<RequirementFieldContract>> serviceResponse = _requirementPackageProxy.IsCalculatedAttribute(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    var lstFieldData = serviceResponse.Result;

                    String attributeTypeCode = ComplianceAttributeType.Calculated.GetStringValue();

                    if (lstFieldData.Where(cond => cond.AttributeTypeCode.Equals(attributeTypeCode)).Any())
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
