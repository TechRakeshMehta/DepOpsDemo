using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ApplicantOperations;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RequirementVerificationItemControlPresenter : Presenter<IRequirementVerificationItemControl>
    {
        #region UAT-1470 :As a student, there should be a way to close out of the video once you open it.

        private ApplicantClinicalRotationProxy _applicantClinicalRotationProxy
        {
            get
            {
                return new ApplicantClinicalRotationProxy();
            }
        }

        public void GetAttributeObjectTreeProperties()
        {
            if (View.PackageID > AppConsts.NONE && View.ItemId > AppConsts.NONE && View.CurrentViewContext.CategoryID > AppConsts.NONE)
            {
                ApplicantRequirementParameterContract appParameterContract = new ApplicantRequirementParameterContract();
                appParameterContract.RequirementPackageId = View.PackageID;
                appParameterContract.RequirementItemId = View.ItemId;
                appParameterContract.RequirementCategoryId = View.CurrentViewContext.CategoryID;
                appParameterContract.TenantId = View.SelectedTenantId;

                ServiceRequest<ApplicantRequirementParameterContract, Int32> serviceRequest = new ServiceRequest<ApplicantRequirementParameterContract, Int32>();
                serviceRequest.Parameter1 = appParameterContract;
                serviceRequest.Parameter2 = 1;//TODO
                var _serviceResponse = _applicantClinicalRotationProxy.GetAttributeObjectTreeProperties(serviceRequest);
                View.LstRequirementObjTreeProperty = _serviceResponse.Result;
            }
            else
            {
                View.LstRequirementObjTreeProperty = null;
            }
        }

        public String GetObjectAttrProperties(Int32 reqObjectTreeId)
        {
            String boxOpenTime = String.Empty;
            if (reqObjectTreeId > 0 && View.CurrentViewContext.SelectedTenantId > 0)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<int>();
                serviceRequest.Parameter = reqObjectTreeId;
                serviceRequest.SelectedTenantId = View.CurrentViewContext.SelectedTenantId;
                var _serviceResponse = _applicantClinicalRotationProxy.GetObjectTreeProperties(serviceRequest);
                ObjectAttributeContract ObjectAttrContract = _serviceResponse.Result;
                if (!ObjectAttrContract.IsNullOrEmpty())
                {
                   boxOpenTime = ObjectAttrContract.BoxOpenTime;
                }
            }
            return boxOpenTime;
        }
        #endregion
    }
}
