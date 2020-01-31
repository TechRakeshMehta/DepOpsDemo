using INTSOF.Utils;
using Business.RepoManagers;
using System.Web;
using INTSOF.ServiceUtil;
using INTSOF.Contracts;
using Entity.ClientEntity;
using System.Web.Configuration;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.SharedObjects;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public class SchoolRepresentativeDetailsPresenter : Presenter<ISchoolRepresentativeDetailsView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
            View.lstAgency = ProfileSharingManager.GetAgencyUserMappedAgencies(View.CurrentUserId);
            //GetTenants();
            List<TenantDetailContract> tenants = GetTenants();
            List<Int32> sharedUserTenantIDs = new List<Int32>();
            sharedUserTenantIDs = GetSharedUserTenantIds();
            var tenantList = tenants.Where(x => sharedUserTenantIDs.Contains(x.TenantID)).ToList();
            View.lstTenants = tenantList;
        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public List<TenantDetailContract> GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// Method to get the shared user tenants.
        /// </summary>
        /// <returns></returns>
        public List<Int32> GetSharedUserTenantIds()
        {
            ServiceRequest<List<String>> serviceRequest = new ServiceRequest<List<String>>();
            serviceRequest.Parameter = View.SharedUserTypeCodes;
            var _serviceResponse = _clinicalRotationProxy.GetSharedUserTenantIDs(serviceRequest);
            return _serviceResponse.Result;
        }
        /// <summary>
        /// Method t UAT-3319
        /// </summary>
        /// <returns></returns>
        public void GetClientDataForAgencyAndAgencyHierarchy(Int32 loggedINUserID, String agency, String tenantid)
        {
            View.lstClientData = SecurityManager.GetClientDataForAgencyAndAgencyHierarchy(loggedINUserID, agency, tenantid, View.customPagingArgsContract);
            if (View.lstClientData.Count > 0)
            {
                View.customPagingArgsContract.VirtualPageCount = View.lstClientData.FirstOrDefault().TotalCount;
            }
            else
            {
                View.customPagingArgsContract.VirtualPageCount = 0;
            }
            View.VirtualPageCount = View.customPagingArgsContract.VirtualPageCount;
            View.CurrentPageIndex = View.customPagingArgsContract.CurrentPageIndex;
        }
    }
}
