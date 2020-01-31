using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System.Xml.Linq;

namespace CoreWeb.RotationPackages.Views
{
    public class BulkRotationPackageCopyPresenter : Presenter<IBulkRotationPackageCopyView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgencies()
        {
            //UAT-1881
            if (IsAdminLoggedIn())
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.TenantID;
                ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAgencies(serviceRequest);
                //  UAT-1448 "Agency" field should display checkboxes in alphabetical order on the manage rotation package screen.
                View.lstAgency = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            }
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.SelectedTenantId = View.TenantID;
                serviceRequest.Parameter = View.CurrentLoggedInUserId;
                var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                View.lstAgency = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsAdminLoggedIn()
        {
            return (SecurityManager.DefaultTenantID == View.TenantID);
        }

        public void GetPackageDetailsWithAgencyIDs()
        {
            List<RequirementPackageContract> lstRequirementPackageContract = new List<RequirementPackageContract>();
            if (!View.ReqRotPackageIDs.IsNullOrEmpty())
            {
                ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
                serviceRequest.Parameter = View.ReqRotPackageIDs;
                lstRequirementPackageContract = _requirementPackageProxy.GetRequirementPackageDetail(serviceRequest).Result;
            }

            if (!lstRequirementPackageContract.IsNullOrEmpty())
            {
                View.lstRequirementPackageContract = lstRequirementPackageContract;
            }
        }


        private String CreateXmlForBulkCopy(List<BulkPackageCopyContract> lstBulkPackageCopyContract)
        {
            StringBuilder sb = new StringBuilder();
            foreach (BulkPackageCopyContract opbjBulkPackageCopyContract in lstBulkPackageCopyContract)
            {
                XElement root = new XElement("BulkPackageCopy");
                XElement row = null;
                row = new XElement("SourcePackageID");
                row.Value = opbjBulkPackageCopyContract.SourcePackageID.ToString();
                root.Add(row);
                row = new XElement("TargetPackageName");
                row.Value = opbjBulkPackageCopyContract.TargetPackageName.ToString();
                root.Add(row);
                row = new XElement("TargetPackageLabel");
                row.Value = opbjBulkPackageCopyContract.TargetPackageLabel.ToString();
                root.Add(row);
                row = new XElement("TargetPackageEffectiveStartDate");
                row.Value = opbjBulkPackageCopyContract.TargetPackageEffectiveStartDate.ToString();
                root.Add(row);
                //UAT-2651
                //row = new XElement("TargetPackageAgencyIds");
                //row.Value = opbjBulkPackageCopyContract.TargetPackageAgencyIds.ToString();
                //root.Add(row);
                row = new XElement("TargetAgencyHierarchyIds");
                row.Value = opbjBulkPackageCopyContract.TargetAgencyHierarchyIds.ToString();
                root.Add(row);
                sb.Append(root.ToString());
            }
            return sb.ToString();
        }

        public Boolean BulkPackageCopy(List<BulkPackageCopyContract> lstBulkPackageCopyContract)
        {
            String XML_DATA = CreateXmlForBulkCopy(lstBulkPackageCopyContract);

            if (!XML_DATA.IsNullOrEmpty())
            {
                ServiceRequest<String, Int32> serviceRequest = new ServiceRequest<String, Int32>();
                serviceRequest.Parameter1 = XML_DATA;
                serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
                ServiceResponse<Boolean> serviceResponse = new ServiceResponse<Boolean>();
                serviceResponse = _requirementPackageProxy.BulkPackageCopy(serviceRequest);
                if (serviceResponse.Result)
                {
                    AgencyHierarchyManager.CallDigestionProcess(String.Join(",", lstBulkPackageCopyContract.Select(sel => sel.TargetAgencyHierarchyIds).ToList()), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);
                }
                return serviceResponse.Result;
            }
            return false;
        }
    }
}
