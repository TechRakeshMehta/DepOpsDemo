#region Namespaces

#region SystemDefined

using INTSOF.SharedObjects;


#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using System;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;
using System.Collections.Generic;
using INTSOF.Utils;
using System.Linq;

#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class EditRequirementFieldPresenter : Presenter<IEditRequirementFieldView>
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

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        public Int32 SaveRequirementPackageData()
        {
            ServiceRequest<RequirementPackageContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Int32>();
            serviceRequest.Parameter1 = View.RequirementPackageContractSessionData;
            serviceRequest.Parameter2 = View.SelectedTenantId;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserId;
            ServiceResponse<Int32> addedPackageID = _requirementPackageProxy.SaveRequirementPackage(serviceRequest);
            return addedPackageID.Result;
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
    }
}



