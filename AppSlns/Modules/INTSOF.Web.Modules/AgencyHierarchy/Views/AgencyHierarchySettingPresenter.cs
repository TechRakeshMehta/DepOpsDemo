using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class AgencyHierarchySettingPresenter : Presenter<IAgencyHierarchySettingView>
    {
        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }


        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed the every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void IsAgencyHierachySettingExisted()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID;
            serviceRequest.Parameter2 = AgencyHierarchySettingType.EXPIRATION_CRITERIA.GetStringValue();
            AgencyHierarchySettingContract result = _agencyHierarchyProxy.GetAgencyHierarchySetting(serviceRequest).Result;

            if (result.AgencyHierarchyID > AppConsts.NONE)
            {
                View.IsAgencyHierachySettingExisted = result.IsRootNode;
                View.AgencyHierarchySettingContract = result;
                //View.IsAutoArchivedRotationSettingExisted = result.IsRotationArchivedAutomatically;

                View.IsAgencyHierachySettingExisted = true;

            }
            else
            {
                View.IsAgencyHierachySettingExisted = false;
            }
            View.IsRootNode = result.IsRootNode;
        }

        public void IsAgencyHierachyAutomaticArchiveSettingExist()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID;
            serviceRequest.Parameter2 = AgencyHierarchySettingType.AUTOMATICALLY_ARCHIVED_ROTATION.GetStringValue();
            AgencyHierarchySettingContract result = _agencyHierarchyProxy.GetAgencyHierarchySetting(serviceRequest).Result;

            if (result.AgencyHierarchyID > AppConsts.NONE)
            {
                ////View.IsAutoArchivedRotationSettingExisted = result.IsRootNode;
                View.AutoArchivedRotationSettingContract = result;
                View.IsAutoArchivedRotationSettingExisted = true;
            }
            else
            {
                View.IsAutoArchivedRotationSettingExisted = false;
            }
            View.IsRootNode = result.IsRootNode;
        }

        //Start UAT-4673
        public void DoesUpdateReviewStatusSettingExist()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID;
            serviceRequest.Parameter2 = AgencyHierarchySettingType.UPDATE_REVIEW_STATUS.GetStringValue();
            AgencyHierarchySettingContract result = _agencyHierarchyProxy.GetAgencyHierarchySetting(serviceRequest).Result;

            if (result.AgencyHierarchyID > AppConsts.NONE)
            {
                View.UpdateReviewStatusSettingContract = result;
                View.IsUpdateReviewStatusSettingExisted = true;
            }
            else
            {
                View.IsUpdateReviewStatusSettingExisted = false;
            }
            View.IsRootNode = result.IsRootNode;
        }
        //End UAT-4673

        public Boolean SaveUpdateAgencyHierarchySetting(AgencyHierarchySettingContract agencyHierarchySettingContract)
        {
            ServiceRequest<AgencyHierarchySettingContract> serviceRequest = new ServiceRequest<AgencyHierarchySettingContract>();
            serviceRequest.Parameter = agencyHierarchySettingContract;
            Boolean result = _agencyHierarchyProxy.SaveAgencyHierarchySetting(serviceRequest).Result;
            if (result)
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyID.ToString(), AppConsts.CHANGE_TYPE_AGENCY_HIERARCHY_SETTING, View.CurrentLoggedInUserId);
            }
            return result;
        }

        #region UAT-3662

        public void GetInstPrecMandatIndividlShareSetting()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID;
            serviceRequest.Parameter2 = AgencyHierarchySettingType.INSTPRECEPTOR_MANDATORY_FOR_INDIVIDUAL_SHARE.GetStringValue();
            AgencyHierarchySettingContract result = _agencyHierarchyProxy.GetAgencyHierarchySetting(serviceRequest).Result;

            if (!result.IsNullOrEmpty())
                View.InstPrecepMandateIndividualShareContract = result;
            else
                View.InstPrecepMandateIndividualShareContract = new AgencyHierarchySettingContract();
        }

        #endregion
        #region UAT-3961

        public Boolean SaveAgencyHierarchyRootNodeSetting(AgencyHierarchyRootNodeSettingContract agencyHierarchySettingContract)
        {
            ServiceRequest<AgencyHierarchyRootNodeSettingContract> serviceRequest = new ServiceRequest<AgencyHierarchyRootNodeSettingContract>();
            serviceRequest.Parameter = agencyHierarchySettingContract;
            Boolean result = _agencyHierarchyProxy.SaveAgencyHierarchyRootNodeSetting(serviceRequest).Result;

            return result;
        }

        public List<AgencyHierarchyRootNodeSettingContract> GetAgencyHierarchyRootNodeMapping()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.SelectedRootNodeID;
            serviceRequest.Parameter2 = AgencyHierarchyRootNodeSettingType.OPTIONS_FOR_TYPE_SPECIALTY_ROTATION_FIELD.GetStringValue();
            List<AgencyHierarchyRootNodeSettingContract> result = _agencyHierarchyProxy.GetAgencyHierarchyRootNodeMapping(serviceRequest).Result;
            return result;
        }

        public Boolean SaveUpdateAgencyHierarchyRootNodeMapping(AgencyHierarchyRootNodeSettingContract agencyHierarchySettingContract)
        {
            ServiceRequest<AgencyHierarchyRootNodeSettingContract> serviceRequest = new ServiceRequest<AgencyHierarchyRootNodeSettingContract>();
            serviceRequest.Parameter = agencyHierarchySettingContract;
            Boolean result = _agencyHierarchyProxy.SaveUpdateAgencyHierarchyRootNodeMapping(serviceRequest).Result;
            return result;
        }

        public Boolean IsAgencyHierachyRootNodeSettingExists()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.SelectedRootNodeID;
            serviceRequest.Parameter2 = AgencyHierarchyRootNodeSettingType.OPTIONS_FOR_TYPE_SPECIALTY_ROTATION_FIELD.GetStringValue();
            Boolean result = _agencyHierarchyProxy.IsAgencyHierarchyRootNodeSettingExist(serviceRequest).Result;
            return result;
        }
        #endregion

        #region UAT-4150

        //public void GetAgencyHierarchyRootNodeSettings()
        //{
        //    ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
        //    serviceRequest.Parameter1 = View.SelectedRootNodeID;
        //    List<AgencyHierarchyRootNodeSettingContract> result = _agencyHierarchyProxy.GetAgencyHierarchyRootNodeSettings(serviceRequest).Result;

        //    if (!result.IsNullOrEmpty())
        //        View.lstAgencyHierarchyRootNodeSettingContract = result;
        //    else
        //        View.lstAgencyHierarchyRootNodeSettingContract = new List<AgencyHierarchyRootNodeSettingContract>();
        //}

        public Boolean InstAvailabilityHierarchyRootNodeSetting()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.SelectedRootNodeID;
            serviceRequest.Parameter2 = AgencyHierarchyRootNodeSettingType.OPTIONS_TO_SPECIFY_INSTRUCTOR_AVAILABILITY.GetStringValue();
            Boolean result = _agencyHierarchyProxy.IsAgencyHierarchyRootNodeSettingExist(serviceRequest).Result;
            return result;
        }
        #endregion
    
    }
}
