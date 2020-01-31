using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ApplicantOperations;
using INTSOF.Utils;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public class RequirementItemFormPresenter : Presenter<IRequirementItemFormView>
    {

        private ApplicantClinicalRotationProxy _applicantClinicalRotationProxy
        {
            get
            {
                return new ApplicantClinicalRotationProxy();
            }
        }

        public void GetRequirementItemForControls()
        {
            if (View.CurrentViewContext.ItemId > 0 && View.TenantId > 0)
            {
                ServiceRequest<Int32,Int32> serviceRequest = new ServiceRequest<Int32,Int32>();
                serviceRequest.Parameter1 = View.CurrentViewContext.ItemId;
                serviceRequest.SelectedTenantId = View.TenantId;
                serviceRequest.Parameter2 = View.CurrentViewContext.RequirementCategoryId;
                var _serviceResponse = _applicantClinicalRotationProxy.GetDataEntryRequirementItem(serviceRequest);
                View.RequirementItem = _serviceResponse.Result;
            }
            else
            {
                View.RequirementItem = new RequirementItemContract();
            }
        }

        public void GetApplicantRequirementItemData()
        {
            ApplicantRequirementParameterContract appParameterContract = new ApplicantRequirementParameterContract();
            appParameterContract.RequirementPkgSubscriptionId = View.CurrentViewContext.RequirementPkgSubscriptionId;
            appParameterContract.RequirementItemId = View.ItemId;
            appParameterContract.RequirementCategoryId = View.CurrentViewContext.RequirementCategoryId;
            appParameterContract.TenantId = View.TenantId;

            ServiceRequest<ApplicantRequirementParameterContract, Int32> serviceRequest = new ServiceRequest<ApplicantRequirementParameterContract, Int32>();
            serviceRequest.Parameter1 = appParameterContract;
            serviceRequest.Parameter2 = View.CurrentViewContext.CurrentLoggedInUserId;

            if (View.CurrentViewContext.RequirementPkgSubscriptionId > AppConsts.NONE && View.ItemId > AppConsts.NONE
                && View.CurrentViewContext.RequirementCategoryId > AppConsts.NONE && View.TenantId > AppConsts.NONE)
            {
                var _serviceResponse = _applicantClinicalRotationProxy.GetApplicantRequirementItemData(serviceRequest);
                View.CurrentViewContext.RequirementItemData = _serviceResponse.Result;
            }
            else
            {
                View.CurrentViewContext.RequirementItemData = new ApplicantRequirementItemDataContract();
            }

        }

        public void GetAttributeObjectTreeProperties()
        {
            if (View.RequirementPackageId > AppConsts.NONE && View.ItemId > AppConsts.NONE && View.CurrentViewContext.RequirementCategoryId > AppConsts.NONE)
            {
                ApplicantRequirementParameterContract appParameterContract = new ApplicantRequirementParameterContract();
                appParameterContract.RequirementPackageId = View.RequirementPackageId;
                appParameterContract.RequirementItemId = View.ItemId;
                appParameterContract.RequirementCategoryId = View.CurrentViewContext.RequirementCategoryId;
                appParameterContract.TenantId = View.TenantId;

                ServiceRequest<ApplicantRequirementParameterContract, Int32> serviceRequest = new ServiceRequest<ApplicantRequirementParameterContract, Int32>();
                serviceRequest.Parameter1 = appParameterContract;
                serviceRequest.Parameter2 = View.CurrentViewContext.CurrentLoggedInUserId;
                var _serviceResponse = _applicantClinicalRotationProxy.GetAttributeObjectTreeProperties(serviceRequest);
                View.LstRequirementObjTreeProperty = _serviceResponse.Result;
            }
            else
            {
                View.LstRequirementObjTreeProperty = null;
            }
        }
        //UAT-3083
        public Tuple<Boolean, Int32> CheckItemPayment()
        {
            return Business.RepoManagers.ComplianceDataManager.CheckItemPayment(View.TenantId, View.CurrentViewContext.RequirementItemData.IsNullOrEmpty() ? AppConsts.NONE : View.CurrentViewContext.RequirementItemData.RequirementItemDataID, View.ItemId,true);
        }
    }
}
