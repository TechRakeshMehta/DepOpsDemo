using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;

namespace CoreWeb.ClinicalRotation.Views
{
    public class AddEditRotationPresenter : Presenter<IAddEditRotationView>
    {

        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {

        }

        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        public void GetCustomAttributeList(Int32? rotationId)
        {
            View.GetCustomAttributeList = ClinicalRotationManager.GetCustomAttributeMappingList(View.SelectedTenantID, CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue(), rotationId);
        }

        public void GetClientContacts()
        {
            if (View.SelectedTenantID == 0)
                View.ClientContactList = new List<ClientContactContract>();
            else
            {
                View.ClientContactList = ClientContactManager.GetClientContacts(View.SelectedTenantID);
            }
        }

        public void GetAllAgency()
        {
            if (View.SelectedTenantID == 0)
                View.lstAgency = new List<AgencyDetailContract>();
            else
            {
                View.lstAgency = ClinicalRotationManager.GetAllAgency(View.SelectedTenantID);
            }
        }

        public void GetWeekDays()
        {
            if (View.SelectedTenantID == 0)
                View.WeekDayList = new List<WeekDayContract>();
            else
            {
                View.WeekDayList = ClinicalRotationManager.GetWeekDayList(View.SelectedTenantID);
            }
        }

        public Boolean IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GetGranularPermissions()
        {
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions);
            View.dicGranularPermissions = dicPermissions;
        }

        public Dictionary<Int32, String> GetDefaultPermissionForClientAdmin()
        {
            return ClinicalRotationManager.GetDefaultPermissionForClientAdmin(View.SelectedTenantID, View.CurrentLoggedInUserId);
        }

        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = ClinicalRotationManager.GetTenants(SortByName, clientCode);
        }

        public Boolean SaveUpdateClinicalRotation()
        {
            if (View.SelectedRotationIDForCloning > 0)
                View.ViewContract.CloneRotationId = View.SelectedRotationIDForCloning;

            View.ViewContract.ContactsToSendEmail = View.ClientContactList.Where(x => View.SelectedClientContacts.Contains(x.ClientContactID)).ToList();
            int clinicalRotationId = ClinicalRotationManager.SaveUpdateClinicalRotation(View.ViewContract, View.SaveCustomAttributeList, View.CurrentLoggedInUserId, ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]);
            //ClinicalRotationManager.SaveUpdateClinicalRotation(View.ViewContract, View.SaveCustomAttributeList, View.CurrentLoggedInUserId, ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]);
            if (clinicalRotationId > AppConsts.NONE)
            {

                return true;
            }
            return false;
        }

        public Boolean SaveUpdateClinicalRotationData()
        {
            if (View.SelectedRotationIDForCloning > 0)
                View.ViewContract.CloneRotationId = View.SelectedRotationIDForCloning;

            View.ViewContract.ContactsToSendEmail = View.ClientContactList.Where(x => View.SelectedClientContacts.Contains(x.ClientContactID)).ToList();
            Tuple<Int32, Boolean, Boolean> _response = ClinicalRotationManager.SaveUpdateClinicalRotationData(View.ViewContract, View.SaveCustomAttributeList, View.CurrentLoggedInUserId, ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]);

            //UAT-3053 : As an admin, after creating a rotation I should be directed to the rotation details screen of that rotation.
            View.ViewContract.RotationID = _response.Item1;
            View.IsApplicantPkgNotAssignedThroughCloning = _response.Item2;
            View.IsInstructorPkgNotAssignedThroughCloning = _response.Item3;

            if (!_response.IsNullOrEmpty() && _response.Item1 > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }
        #region UAT-2514

        public Dictionary<Boolean, DateTime> IsRotationEndDateRangeNeedToManage()
        {
            return ClinicalRotationManager.IsRotationEndDateRangeNeedToManage(View.ViewContract.RotationID, View.SelectedTenantID);
        }

        #endregion
        #region UAT-2424
        public void GetClinicalRotationsForAddForm()
        {
            if (View.SelectedTenantID == 0)
                View.lstClinicalRotation = new List<ClinicalRotationDetailContract>();
            else
            {
                View.lstClinicalRotation = ClinicalRotationManager.GetAllClinicalRotationsForLoggedInUser(View.SelectedTenantID, View.CurrentLoggedInUserId, IsAdminLoggedIn());
            }
        }

        public ClinicalRotationDetailContract GetClinicalRotationDetailsByID()
        {
            return ClinicalRotationManager.GetClinicalRotationDetailsById(View.SelectedTenantID, View.SelectedRotationIDForCloning);

        }

        /// <summary>
        /// Get the Applicant Package ID for selected rotation
        /// </summary>
        /// <returns></returns>
        public Int32 GetApplicantPackage()
        {
            String reqPkgTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
            return ClinicalRotationManager.GetRotationPackageIDByRotationId(View.SelectedTenantID, View.SelectedRotationIDForCloning, reqPkgTypeCode, View.ViewContract);
        }

        /// <summary>
        /// Get the instructor/Preceptor Package ID for Selected rotation
        /// </summary>
        /// <returns></returns>
        public Int32 GetInstructorPackage()
        {
            String InstPkgTypeCode = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();
            return ClinicalRotationManager.GetRotationPackageIDByRotationId(View.SelectedTenantID, View.SelectedRotationIDForCloning, InstPkgTypeCode, View.ViewContract);
        }

        #endregion

        #region UAT-2554
        public Boolean IsPreceptorRequiredForAgency()
        {
            return ClinicalRotationManager.IsPreceptorRequiredForAgency(View.SelectedTenantID, View.SelectedAgencyID);
        }
        #endregion

        #region UAT-3241
        public List<String> GetAgencyNamesByIds(List<Int32> lstAgencyIds)
        {
            List<String> Result = ClinicalRotationManager.GetAgencyNamesByIds(View.SelectedTenantID, lstAgencyIds);
            return Result;
        }
        #endregion

        public void GetClinicalRotationById()
        {
            View.clinicalRotationDetailContract = ClinicalRotationManager.GetClinicalRotationDetailsById(View.SelectedTenantID, View.SelectedRotationID);
            GetRotationFieldUpdateByAgency();
        }

        public void GetAdditionalDocumnet()
        {
            View.ViewContract.listOfMultipleDocument = ClinicalRotationManager.GetAdditionalDocumnetDetails(View.SelectedTenantID, View.SelectedRotationID);

        }

        public void GetValidationRotation()
        {
            View.agencyHierarchyRotationFieldOptionContract = ClinicalRotationManager.GetAgencyHierarchyRotationFieldOptionSetting(View.SelectedTenantID, View.hierarchyID);

        }

        #region UAT-2666
        public void GetRotationFieldUpdateByAgency()
        {
            List<Int32> clinicalRotationIds = new List<Int32>();
            clinicalRotationIds.Add(View.SelectedRotationID);
            View.lstRotationFieldUpdaeByAgency = ClinicalRotationManager.GetRotationFieldUpdateByAgencyDetails(clinicalRotationIds, View.SelectedTenantID);
        }

        public void GetTypeSpecialtyOptions(Int32 SelectedRootNodeId)
        {
            if (SelectedRootNodeId > AppConsts.NONE)
            {
                String agencyHierarchySettingTypeCode = AgencyHierarchyRootNodeSettingType.OPTIONS_FOR_TYPE_SPECIALTY_ROTATION_FIELD.GetStringValue();
                View.TypeSpecialtyList = AgencyHierarchyManager.GetAgencyHierarchyRootNodeMapping(SelectedRootNodeId, agencyHierarchySettingTypeCode);
            }
        }


        public Boolean IsAgencyHierarchyRootNodeSettingExist(Int32 SelectedRootNodeId)
        {
            if (SelectedRootNodeId > AppConsts.NONE)
            {
                String agencyHierarchySettingTypeCode = AgencyHierarchyRootNodeSettingType.OPTIONS_FOR_TYPE_SPECIALTY_ROTATION_FIELD.GetStringValue();
                return AgencyHierarchyManager.IsAgencyHierarchyRootNodeSettingExist(SelectedRootNodeId, agencyHierarchySettingTypeCode);
            }
            return false;
        }

        #endregion

        #region UAT-4147

        /// <summary>
        /// To check is selected Instructor(s) already exists as Applicant(s) in Clinical Rotation
        /// </summary>
        public List<ClinicalRotationMembersContract> IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(string rotationIDs, int tenantID, string selectedOrgUserIDs, string selectedClientContactIDs)
        {
            return ClinicalRotationManager.IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(rotationIDs, tenantID, selectedOrgUserIDs, selectedClientContactIDs);
        }
        #endregion

        #region UAT-4150
        public void InstAvailabilityHierarchyRootNodeSetting(Int32 SelectedRootNodeId)
        {
            View.IsInstAvailabilityDefined = false;
            if (SelectedRootNodeId > AppConsts.NONE)
            {
                String agencyHierarchySettingTypeCode = AgencyHierarchyRootNodeSettingType.OPTIONS_TO_SPECIFY_INSTRUCTOR_AVAILABILITY.GetStringValue();
                View.IsInstAvailabilityDefined = AgencyHierarchyManager.IsAgencyHierarchyRootNodeSettingExist(SelectedRootNodeId, agencyHierarchySettingTypeCode);
            }
        }
        #endregion

        //UAT-4323
        /// <summary>
        /// Gets Clinical Rotation Members
        /// </summary>
        public void GetClinicalRotationMembers()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.SelectedTenantId = View.SelectedTenantID;
            serviceRequest.Parameter1 = View.SelectedRotationID;
            serviceRequest.Parameter2 = View.SelectedAgencyID;
            var _serviceResponse = _clientRotationProxy.GetClinicalRotationMembers(serviceRequest);
            View.RotationMemberDetailList = _serviceResponse.Result;
            //UAT-2544
            View.RotationMemberDetailList.ForEach(cnd =>
            {
                cnd.IsRotationStart = View.IsRotationStart;
            });

        }


        public Boolean IsSelectedAgenycHierarchyAvailable(String agencyHierarchyIDs)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.SelectedTenantId = View.SelectedTenantID;
            serviceRequest.Parameter = agencyHierarchyIDs;

            var _serviceResponse = _clientRotationProxy.IsAgenycHierarchyAvailable(serviceRequest);

            return _serviceResponse.Result;
        }
    }
}
