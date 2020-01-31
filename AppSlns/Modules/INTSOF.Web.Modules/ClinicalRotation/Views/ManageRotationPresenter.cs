using System;
using System.Collections.Generic;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Linq;
using System.Text;
using INTSOF.ServiceUtil;
using System.Web;
using INTSOF.Contracts;

namespace CoreWeb.ClinicalRotation.Views
{
    public class ManageRotationPresenter : Presenter<IManageRotationView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private ClientContactProxy _clientContactProxy
        {
            get
            {
                return new ClientContactProxy();
            }
        }


        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
            GetTenants();
        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }
        public void GetRotationDetail()
        {
            List<ClinicalRotationDetailContract> rotationData;
            if (View.SelectedTenantID == 0)
            {
                rotationData = new List<ClinicalRotationDetailContract>();
            }

            //else if (View.SelectedAgencyID == 0)
            //{
            //    rotationData = new List<ClinicalRotationDetailContract>();
            //    View.ErrorMessage = "Please select an agency.";
            //}
            else
            {
                View.SearchContract.TenantID = View.SelectedTenantID;
                if (SecurityManager.DefaultTenantID != View.TenantID)
                {
                    View.SearchContract.CurrentLoggedInClientUserID = View.CurrentLoggedInUserId;
                }
                else
                {
                    View.SearchContract.CurrentLoggedInClientUserID = null;
                }
                //UAT-2545
                if (View.SelectedArchiveStatusCode.IsNotNull())
                    View.SearchContract.ArchieveStatusId = GetArchiveStateId();
                else
                    View.SearchContract.ArchieveStatusId = null;

                /*UAT-2979: Add the Institution Hierarchy link as a way to filter out existing rotations on the Manage Rotation Search?*/

                if (!View.DeptProgramMappingID.IsNullOrEmpty())
                    View.SearchContract.DPMIds = View.DeptProgramMappingID;
                /*End UAT-2979*/
                ServiceRequest<ClinicalRotationDetailContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<ClinicalRotationDetailContract, CustomPagingArgsContract>();
                serviceRequest.Parameter1 = View.SearchContract;
                serviceRequest.Parameter2 = View.GridCustomPaging;
                var _serviceResponse = _clinicalRotationProxy.GetClinicalRotationQueueData(serviceRequest);
                rotationData = _serviceResponse.Result;
            }
            View.ClinicalRotationData = rotationData;

            #region UAT-2666
            List<Int32> lstClinicalRotationIds = View.ClinicalRotationData.Select(sel => sel.RotationID).ToList();
            GetRotationFieldUpdateByAgency(lstClinicalRotationIds);
            #endregion

            if (rotationData.IsNullOrEmpty())
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }
            else
            {
                View.VirtualRecordCount = rotationData[0].TotalRecordCount;
            }
        }

        public void GetRotationsMappedToAgencies(String rotationIDs)
        {
            RotationsMappedToAgenciesContract rotationsMappedToAgencies;
            if (View.SelectedTenantID == 0)
            {
                rotationsMappedToAgencies = new RotationsMappedToAgenciesContract();
            }
            else
            {
                ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
                serviceRequest.Parameter1 = View.SelectedTenantID;
                serviceRequest.Parameter2 = rotationIDs;
                var _serviceResponse = _clinicalRotationProxy.GetRotationsMappedToAgencies(serviceRequest);
                rotationsMappedToAgencies = _serviceResponse.Result;
            }
            View.RotationsMappedToAgenciesData = rotationsMappedToAgencies;

        }



        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            View.lstTenant = _serviceResponse.Result;
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantID);
        }


        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgency()
        {
            if (View.SelectedTenantID == 0)
                View.lstAgency = new List<AgencyDetailContract>();
            else
            {
                //UAT-1881
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                IsAdminLoggedIn();
                if (View.IsAdminLoggedIn)
                {
                    serviceRequest.Parameter = View.SelectedTenantID;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencies(serviceRequest);
                    View.lstAgency = _serviceResponse.Result;
                }
                else
                {
                    serviceRequest.SelectedTenantId = View.SelectedTenantID;
                    serviceRequest.Parameter = View.CurrentLoggedInUserId;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                    View.lstAgency = _serviceResponse.Result;
                }
            }
        }

        public void GetClientContacts()
        {
            if (View.SelectedTenantID == 0)
                View.ClientContactList = new List<ClientContactContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantID;
                var _serviceResponse = _clientContactProxy.GetClientContacts(serviceRequest);
                View.ClientContactList = _serviceResponse.Result;
            }
        }


        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetWeekDays()
        {

            if (View.SelectedTenantID == 0)
                View.WeekDayList = new List<WeekDayContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantID;
                var _serviceResponse = _clinicalRotationProxy.GetWeekDayList(serviceRequest);
                View.WeekDayList = _serviceResponse.Result;
            }
        }




        public Boolean SaveUpdateClinicalRotation()
        {

            ServiceRequest <ClinicalRotationDetailContract, List<CustomAttribteContract>, String > serviceRequest = new ServiceRequest<ClinicalRotationDetailContract, List<CustomAttribteContract>, String>();
            View.ViewContract.ContactsToSendEmail = View.ClientContactList.Where(x => View.SelectedClientContacts.Contains(x.ClientContactID)).ToList();
            View.ClientContactList.Where(x => View.SelectedClientContacts.Contains(x.ClientContactID)).ToList();
            serviceRequest.Parameter1 = View.ViewContract;
            serviceRequest.Parameter2 = View.SaveCustomAttributeList;
            serviceRequest.Parameter3 = System.Configuration.ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID];
          
            //var _serviceResponse = _clinicalRotationProxy.SaveUpdateClinicalRotation(serviceRequest);
            var _serviceResponse = _clinicalRotationProxy.SaveUpdateClinicalRotation(serviceRequest);

            //UAT-3053 : As an admin, after creating a rotation I should be directed to the rotation details screen of that rotation.
            View.ViewContract.RotationID = _serviceResponse.Result.Item1;
            View.IsApplicantPkgNotAssignedThroughCloning = _serviceResponse.Result.Item2;
            View.IsInstructorPkgNotAssignedThroughCloning = _serviceResponse.Result.Item3;

            if (!_serviceResponse.Result.IsNullOrEmpty() && _serviceResponse.Result.Item1 > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        public Boolean DeleteClinicalRotation()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.ViewContract.RotationID;
            //serviceRequest.Parameter2 = View.CurrentUserID;
            serviceRequest.Parameter2 = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.DeleteClinicalRotation(serviceRequest);
            return _serviceResponse.Result;
        }

        public void GetCustomAttributeList(Int32? rotationId)
        {
            ServiceRequest<Int32, String, Int32?> serviceRequest = new ServiceRequest<Int32, String, Int32?>();
            serviceRequest.Parameter1 = View.SelectedTenantIDForAddForm;
            serviceRequest.Parameter2 = CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue();
            serviceRequest.Parameter3 = rotationId;
            var _serviceResponse = _clinicalRotationProxy.GetCustomAttributeListMapping(serviceRequest);
            View.GetCustomAttributeList = _serviceResponse.Result;
        }


        /// <summary>
        /// Is Clinical Rotation Members exist for clinical rotation
        /// </summary>
        /// <returns></returns>
        public Boolean IsClinicalRotationMembersExistForRotation()
        {
            if (View.SelectedTenantID > AppConsts.NONE && View.ViewContract.RotationID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.SelectedTenantId = View.SelectedTenantID;
                serviceRequest.Parameter = View.ViewContract.RotationID;
                var _serviceResponse = _clinicalRotationProxy.IsClinicalRotationMembersExistForRotation(serviceRequest);

                return _serviceResponse.Result;
            }
            return false;
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgencyForAddForm()
        {
            if (View.SelectedTenantIDForAddForm == 0)
                View.lstAgencyForAddForm = new List<AgencyDetailContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                IsAdminLoggedIn();
                if (View.IsAdminLoggedIn)
                {
                    serviceRequest.Parameter = View.SelectedTenantIDForAddForm;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencies(serviceRequest);
                    View.lstAgencyForAddForm = _serviceResponse.Result;
                }
                else
                {
                    serviceRequest.SelectedTenantId = View.SelectedTenantIDForAddForm; ;
                    serviceRequest.Parameter = View.CurrentLoggedInUserId;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                    View.lstAgencyForAddForm = _serviceResponse.Result;
                }

            }
        }

        public void GetClientContactsForAddForm()
        {
            if (View.SelectedTenantIDForAddForm == 0)
                View.ClientContactListForAddForm = new List<ClientContactContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantIDForAddForm;
                var _serviceResponse = _clientContactProxy.GetClientContacts(serviceRequest);
                View.ClientContactListForAddForm = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetWeekDaysForAddForm()
        {

            if (View.SelectedTenantIDForAddForm == 0)
                View.WeekDayListForAddForm = new List<WeekDayContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantIDForAddForm;
                var _serviceResponse = _clinicalRotationProxy.GetWeekDayList(serviceRequest);
                View.WeekDayListForAddForm = _serviceResponse.Result;
            }
        }


        public Dictionary<Int32, String> GetDefaultPermissionForClientAdmin()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.SelectedTenantID;
            ServiceResponse<Dictionary<Int32, String>> _serviceResponse = _clinicalRotationProxy.GetDefaultPermissionForClientAdmin(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// UAT-1784: Get Granular Permissions for current user
        /// </summary>
        public void GetGranularPermissions()
        {
            var _serviceResponse = _clinicalRotationProxy.GetGranularPermissions();
            View.dicGranularPermissions = _serviceResponse.Result;
        }

        #region UAT-2034:
        public Boolean IsPreceptorAssignedForAllRotations(String rotationIDs)
        {
            ServiceRequest<String> serviceRequestToCheckDataEnteredForRot = new ServiceRequest<String>();
            serviceRequestToCheckDataEnteredForRot.SelectedTenantId = View.SelectedTenantID;
            serviceRequestToCheckDataEnteredForRot.Parameter = rotationIDs;

            ServiceResponse<Boolean> serviceResponse = _clinicalRotationProxy.IsPreceptorAssignedForAllRotations(serviceRequestToCheckDataEnteredForRot);
            if (!serviceResponse.IsNullOrEmpty())
            {
                return serviceResponse.Result;
            }
            return false;
        }
        #endregion

        #region UAT-2514

        public Dictionary<Boolean, DateTime> IsRotationEndDateRangeNeedToManage()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.ViewContract.RotationID;
            serviceRequest.Parameter2 = View.SelectedTenantID;
            ServiceResponse<Dictionary<Boolean, DateTime>> _serviceResponse = _clinicalRotationProxy.IsRotationEndDateRangeNeedToManage(serviceRequest);
            return _serviceResponse.Result;
        }

        #endregion

        #region UAT-2424
        public void GetClinicalRotationsForAddForm()
        {
            if (View.SelectedTenantIDForAddForm == 0)
                View.lstClinicalRotation = new List<ClinicalRotationDetailContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantIDForAddForm;
                var _serviceResponse = _clinicalRotationProxy.GetAllClincialRotations(serviceRequest);
                View.lstClinicalRotation = _serviceResponse.Result;
            }
        }

        public ClinicalRotationDetailContract GetClinicalRotationDetailsByID()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.SelectedTenantId = View.SelectedTenantIDForAddForm;
            serviceRequest.Parameter = View.SelectedRotationIDForCloning;
            // ServiceResponse<ClinicalRotationDetailContract> _serviceResponse = _clinicalRotationProxy.GetClinicalRotationDetailsById(serviceRequest);
            ServiceResponse<ClinicalRotationDetailContract> _serviceResponse = _clinicalRotationProxy.GetClinicalRotationDetailsById(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// Get the Applicant Package ID for selected rotation
        /// </summary>
        /// <returns></returns>
        public Int32 GetApplicantPackage()
        {
            //UAT-3121
            DateTime? RotationStartDate = View.ViewContract.StartDate;
            DateTime? RotationEndDate = View.ViewContract.EndDate;

            String reqPkgTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.SelectedTenantId = View.ViewContract.TenantID;
            serviceRequest.Parameter1 = View.SelectedRotationIDForCloning;
            serviceRequest.Parameter2 = reqPkgTypeCode;
            INTSOF.ServiceDataContracts.Modules.RequirementPackage.RequirementPackageContract requirementPackageContract = _clinicalRotationProxy.GetRotationPackageIDByRotationId(serviceRequest).Result;
            if (!requirementPackageContract.IsNullOrEmpty())
            {
                if ((requirementPackageContract.EffectiveEndDate.IsNull() || requirementPackageContract.EffectiveEndDate > RotationStartDate)
                    && (requirementPackageContract.EffectiveStartDate.IsNull() || requirementPackageContract.EffectiveStartDate < RotationEndDate)
                    && requirementPackageContract.IsActive && !requirementPackageContract.IsArchivedPackage)
                //&& (!requirementPackageContract.IsCopied || requirementPackageContract.IsNewPackage))
                {
                    return requirementPackageContract.RequirementPackageID;
                }
                else
                {
                    View.IsApplicantPkgNotAssignedThroughCloning = true;
                }
            }
            return AppConsts.NONE;
        }

        /// <summary>
        /// Get the instructor/Preceptor Package ID for Selected rotation
        /// </summary>
        /// <returns></returns>
        public Int32 GetInstructorPackage()
        {
            //UAT-3121
            DateTime? RotationStartDate = View.ViewContract.StartDate;
            DateTime? RotationEndDate = View.ViewContract.EndDate;

            String InstPkgTypeCode = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();
            ServiceRequest<Int32, String> serviceRequestForInstPkg = new ServiceRequest<Int32, String>();
            serviceRequestForInstPkg.SelectedTenantId = View.ViewContract.TenantID;
            serviceRequestForInstPkg.Parameter1 = View.SelectedRotationIDForCloning;
            serviceRequestForInstPkg.Parameter2 = InstPkgTypeCode;
            INTSOF.ServiceDataContracts.Modules.RequirementPackage.RequirementPackageContract requirementPackageContract = _clinicalRotationProxy.GetRotationPackageIDByRotationId(serviceRequestForInstPkg).Result;
            if (!requirementPackageContract.IsNullOrEmpty())
            {
                if ((requirementPackageContract.EffectiveEndDate.IsNull() || requirementPackageContract.EffectiveEndDate > RotationStartDate)
                    && (requirementPackageContract.EffectiveStartDate.IsNull() || requirementPackageContract.EffectiveStartDate < RotationEndDate)
                    && requirementPackageContract.IsActive && !requirementPackageContract.IsArchivedPackage)
                //&& (!requirementPackageContract.IsCopied || requirementPackageContract.IsNewPackage))
                {
                    return requirementPackageContract.RequirementPackageID;
                }
                else
                {
                    View.IsInstructorPkgNotAssignedThroughCloning = true;
                }
            }
            return AppConsts.NONE;
        }



        #region UAT-2554

        public Boolean IsPreceptorRequiredForAgency()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.SelectedTenantId = View.SelectedTenantIDForAddForm;
            serviceRequest.Parameter = View.SelectedAgencyIDForAddForm;
            ServiceResponse<Boolean> _serviceResponse = _clinicalRotationProxy.IsPreceptorRequiredForAgency(serviceRequest);
            return _serviceResponse.Result;
        }


        #endregion


        #region UAT-2545
        /// <summary>
        /// Gets the list of Archive State.
        /// </summary>
        public void GetArchiveStateList()
        {
            View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantID);
        }

        public Boolean ArchiveSelectedRotation()
        {
            ServiceRequest<List<Int32>, Int32> serviceRequest = new ServiceRequest<List<Int32>, Int32>();
            List<Int32> RotationIds = new List<Int32>();
            RotationIds.AddRange(View.DicOfSelectedRotation.Keys);
            serviceRequest.Parameter1 = RotationIds;
            serviceRequest.Parameter2 = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.ArchiveClinicalRotation(serviceRequest);
            return _serviceResponse.Result;
        }

        //UAT-3138
        public Boolean UnArchiveSelectedRotation()
        {
            ServiceRequest<List<Int32>, Int32> serviceRequest = new ServiceRequest<List<Int32>, Int32>();
            List<Int32> RotationIds = new List<Int32>();
            RotationIds.AddRange(View.DicOfSelectedRotation.Keys);
            serviceRequest.Parameter1 = RotationIds;
            serviceRequest.Parameter2 = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.UnArchiveClinicalRotation(serviceRequest);
            return _serviceResponse.Result;
        }

        private String GetArchiveStateId()
        {
            return ComplianceDataManager.GetArchiveStateList(View.SelectedTenantID).FirstOrDefault(x => x.AS_Code.Equals(View.SelectedArchiveStatusCode.FirstOrDefault())).AS_ID.ToString();

        }

        #endregion




        #endregion

        #region UAT-2666
        public void GetRotationFieldUpdateByAgency(List<Int32> lstClinicalRotationIds)
        {
            if (lstClinicalRotationIds.Count > AppConsts.NONE)
            {
                ServiceRequest<List<Int32>, Int32> serviceRequest = new ServiceRequest<List<Int32>, Int32>();
                serviceRequest.Parameter1 = lstClinicalRotationIds;
                serviceRequest.Parameter2 = View.SelectedTenantID;
                var _serviceResponse = _clinicalRotationProxy.GetRotationFieldUpdateByAgencyDetails(serviceRequest);
                View.lstRotationFieldUpdaeByAgency = _serviceResponse.Result;
            }
        }
        #endregion

        #region UAT-2617

        public void GetRotationReviewStatus()
        {
            var rotationReviewStatusList = LookupManager.GetLookUpData<Entity.ClientEntity.lkpSharedUserRotationReviewStatu>(View.SelectedTenantID).ToList();
            rotationReviewStatusList.Add(new Entity.ClientEntity.lkpSharedUserRotationReviewStatu { SURRS_ID = -2, SURRS_Name = "Pending Share" });
            rotationReviewStatusList.Add(new Entity.ClientEntity.lkpSharedUserRotationReviewStatu { SURRS_ID = -1, SURRS_Name = "Dropped" });
            View.lstRotationReviewStatus = rotationReviewStatusList;
        }
        #endregion

        #region 2696

        public Boolean IsCustomAttributeChecked(Int32 OrganizationUserId, String UniqueName, String GrdCode)
        {
            //ServiceRequest<Int32,Int32,String> serviceRequest = new ServiceRequest<Int32,Int32,String>();
            //serviceRequest.Parameter1 = TenantId;
            //serviceRequest.Parameter2 = OrganizationUserId;
            //serviceRequest.Parameter3 = UniqueName;
            //ServiceResponse<Dictionary<Int32, String>> _serviceResponse = _clinicalRotationProxy.IsCustomAttributeChecked(serviceRequest);
            //return _serviceResponse.Result;

            return SecurityManager.IsCustomAttributeChecked(OrganizationUserId, UniqueName, GrdCode);
        }

        #endregion

        #region UAT-3241

        public List<String> GetAgencyNamesByIds(List<Int32> lstAgencyIds)
        {
            ServiceRequest<Int32, List<Int32>> serviceRequest = new ServiceRequest<Int32, List<Int32>>();
            serviceRequest.Parameter1 = View.TenantID;
            serviceRequest.Parameter2 = lstAgencyIds;
            var _response = _clinicalRotationProxy.GetAgencyNamesByIds(serviceRequest);
            return _response.Result;
        }
        #endregion
    }
}
