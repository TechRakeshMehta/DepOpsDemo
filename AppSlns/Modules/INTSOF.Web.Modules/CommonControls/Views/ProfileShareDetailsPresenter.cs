using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Entity.ClientEntity;
using Business.RepoManagers;

namespace CoreWeb.CommonControls.Views
{
    public class ProfileShareDetailsPresenter : Presenter<IProfileShareDetails>
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
        /// Get Clinical Rotation data to bind Rotation Detail section
        /// </summary>
        public void GetProfileShareDetail()
        {
            if (View.InvitationID > 0) // && !View.IsRestrictToLoadFresshData
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.InvitationID;
                var _serviceResponse = _clinicalRotationProxy.GetProfileShareDetailsById(serviceRequest);
                View.ProfileSharingDetails = _serviceResponse.Result;
            }
            else
            {
                View.ProfileSharingDetails = new ProfileSharingInvitationDetailsContract();
            }
        }

        /// <summary>
        /// Get Start Date or end dates client setting
        /// </summary>
        public void GetRotationClientSetting()
        {
            List<String> lstCodes = new List<String>();
            lstCodes.Add(Setting.ROTATION_START_DATE_REQUIRED.GetStringValue());
            lstCodes.Add(Setting.ROTATION_END_DATE_REQUIRED.GetStringValue());
            List<ClientSetting> lstClientSetting = ComplianceDataManager.GetClientSettingsByCodes(View.TenantId, lstCodes);
            var _setting = lstClientSetting.FirstOrDefault(cs => cs.lkpSetting.Code == Setting.ROTATION_START_DATE_REQUIRED.GetStringValue());
            if (!_setting.IsNullOrEmpty())
            {
                View.IsStartDateRequired = _setting.CS_SettingValue.Equals("1") ? true : false;
            }
            _setting = lstClientSetting.FirstOrDefault(cs => cs.lkpSetting.Code == Setting.ROTATION_END_DATE_REQUIRED.GetStringValue());
            if (!_setting.IsNullOrEmpty())
            {
                View.IsEndDateRequired = _setting.CS_SettingValue.Equals("1") ? true : false;
            }
        }

        public Boolean SaveUpdateProfileShareInvDetails()
        {
            ServiceRequest<ProfileSharingInvitationDetailsContract, Int32> serviceRequest = new ServiceRequest<ProfileSharingInvitationDetailsContract, Int32>();
            serviceRequest.Parameter1 = View.ProfileSharingDetails;                   
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            var _serviceResponse = _clinicalRotationProxy.UpdateProfileShareInvDetails(serviceRequest);
            return _serviceResponse.Result;
        }


    }
}
