#region Namespaces

#region SystemDefined

using INTSOF.SharedObjects;


#endregion

#region UserDefined

using Business.RepoManagers;
using System;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;


#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class CategorySummaryPresenter : Presenter<ICategorySummaryView>
    {
        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public Int32 SaveRequirementPackageData()
        {
            //UAT-1621 WB: As a rotation package creator, I should be able to create a package so that it applies to both applicants and instructor/preceptors 
            ServiceResponse<Int32> addedPackageID = null;
            Int32 addedPackageIDForRotScreen = AppConsts.NONE;

            if (View.RequirementPackageContractSessionData.RequirementPkgTypes != null && View.RequirementPackageContractSessionData.RequirementPkgTypes.Count > 0)
            {
                //UAT-1621
                //If from Rotation Screen then reqPkgTypeCode will be set
                var reqPkgTypeCode = View.RequirementPackageContractSessionData.RequirementPkgTypeCode;

                foreach (var RequirementTypes in View.RequirementPackageContractSessionData.RequirementPkgTypes)
                {
                    View.RequirementPackageContractSessionData.RequirementPkgTypeID = RequirementTypes.Key;
                    View.RequirementPackageContractSessionData.RequirementPkgTypeCode = RequirementTypes.Value;
                    View.RequirementPackageContractSessionData.RequirementPackageCode = Guid.NewGuid();
                    ServiceRequest<RequirementPackageContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Int32>();
                    serviceRequest.Parameter1 = View.RequirementPackageContractSessionData;
                    serviceRequest.Parameter2 = View.SelectedTenantId;
                    serviceRequest.Parameter3 = View.CurrentLoggedInUserId;
                    addedPackageID = _requirementPackageProxy.SaveRequirementPackage(serviceRequest);
                    //UAT-1621
                    if (!reqPkgTypeCode.IsNullOrEmpty() && reqPkgTypeCode == RequirementTypes.Value)
                        addedPackageIDForRotScreen = addedPackageID.Result;
                }
            }
            else
            {
                ServiceRequest<RequirementPackageContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Int32>();
                serviceRequest.Parameter1 = View.RequirementPackageContractSessionData;
                serviceRequest.Parameter2 = View.SelectedTenantId;
                serviceRequest.Parameter3 = View.CurrentLoggedInUserId;
                addedPackageID = _requirementPackageProxy.SaveRequirementPackage(serviceRequest);
            }

            //UAT-1621
            if (View.RequirementPackageContractSessionData.IsSharedUserLoggedIn)
            {
                AgencyHierarchyManager.CallDigestionProcess(String.Join(",", View.RequirementPackageContractSessionData.LstAgencyHierarchyIDs), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);
            }

            if (addedPackageIDForRotScreen > AppConsts.NONE)
            {
                return addedPackageIDForRotScreen;
            }
            else
            {
                return addedPackageID.Result;
            }

        }
    }
}



