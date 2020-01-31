using System;
using System.Linq;
using System.Collections.Generic;
using CoreWeb.IntsofSecurityModel;

using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.Configuration;
using System.IO;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Web.UI;
using Entity.ClientEntity;
using CoreWeb.AgencyHierarchy.Views;
using INTSOF.UI.Contract.CommonControls;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;

namespace CoreWeb.ClinicalRotation.Views
{
    public class ManageRotationAttributeGroupPresenter : Presenter<IManageRotationAttributeGroupView>
    {

        public override void OnViewLoaded()
        {
            View.ListTenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
            //View.TenantId = GetTenantId();
        }

        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;

        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            //   Int32 currentUserID = GetTenantId();
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
        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }
        public void GetAllRotationAttributeGroup()
        {
            List<RequirementAttributeGroupContract> requirementAttributeData;
            //if (View.SelectedTenantID == 0)
            //{
            //    requirementAttributeData = new List<RequirementAttributeGroupContract>();
            //    View.ListRotationAttributeGroup = requirementAttributeData;
            //}
            //else
            //{
                ServiceRequest<Int32, String, String> serviceRequest = new ServiceRequest<Int32, String, String>();
                serviceRequest.Parameter1 = View.SelectedTenantID;
                serviceRequest.Parameter2 = View.Name;
                serviceRequest.Parameter3 = View.Label;
                var _serviceResponse = _clientRotationProxy.GetAllRotationAttributeGroup(serviceRequest);
                View.ListRotationAttributeGroup = _serviceResponse.Result;
            //}
        }
        /// <summary>
        /// Save the Attribute Group 
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean SaveAttributeGroup()
        {
            if (IsAttributeGroupExist(View.Name, null))
            {
                RequirementAttributeGroupContract attributeGroup = new RequirementAttributeGroupContract();
                attributeGroup.Name = View.Name;
                attributeGroup.Label = View.Label;
                attributeGroup.IsDeleted = false;
                attributeGroup.CreatedByID = View.CurrentUserId;
                attributeGroup.CreatedOn = DateTime.Now;
                attributeGroup.Code = Guid.NewGuid();
                attributeGroup.IsCreatedByAdmin = View.TenantId == SecurityManager.DefaultTenantID ? true : false;

                ServiceRequest<RequirementAttributeGroupContract, Int32, Boolean> serviceRequest = new ServiceRequest<RequirementAttributeGroupContract, Int32, Boolean>();
                serviceRequest.Parameter1 = attributeGroup;
                serviceRequest.Parameter2 = View.CurrentUserId;
                serviceRequest.Parameter3 = false;

                //var _serviceResponse = _clientRotationProxy.SaveRotationAttributeGroup(serviceRequest);
                if (_clientRotationProxy.SaveUpdateRotationAttributeGroup(serviceRequest).Result)
                {
                    View.SuccessMessage = "Attribute group saved successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occured.Please try again.";
                    return false;
                }
            }
            else
            {
                View.ErrorMessage = "Attribute group already exist.";
                return false;
            }
        }
        /// <summary>
        /// Check the user Attribute existance
        /// </summary>
        /// <param name="userGroupName"></param>
        /// <param name="userGroupId"></param>
        /// <returns>Boolean</returns>
        public Boolean IsAttributeGroupExist(String userGroupName, Int32? userGroupId = null)
        {
            ServiceRequest<Int32,String,String> serviceRequest = new ServiceRequest<Int32,String,String>();
            serviceRequest.Parameter1 = View.SelectedTenantID;
            serviceRequest.Parameter2 = "";
            serviceRequest.Parameter3 = "";
            var _serviceResponse = _clientRotationProxy.GetAllRotationAttributeGroup(serviceRequest);
            View.ListRotationAttributeGroup = _serviceResponse.Result;
            if (userGroupId != null)
            {
                if (View.ListRotationAttributeGroup.Any(x => x.Name.ToLower() == userGroupName.ToLower() && x.RequirementAttributeGroupID != userGroupId))
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (View.ListRotationAttributeGroup.Any(x => x.Name.ToLower() == userGroupName.ToLower() && !x.IsDeleted))
                {
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// Update the Attribute Group
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdateAttributeGroup()
        {
            //if (IsAttributeGroupExist(View.Name, View.RequirementAttributeGroupId))
           // {
                ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
                serviceRequest.Parameter1 = View.SelectedTenantID;
                serviceRequest.Parameter2 = View.RequirementAttributeGroupId;
                var _serviceResponse = _clientRotationProxy.GetAttributeGroupById(serviceRequest);
                
                RequirementAttributeGroupContract rotationattributeGroup = new RequirementAttributeGroupContract();
                rotationattributeGroup.Name = View.Name;
                rotationattributeGroup.Label = View.Label;
                rotationattributeGroup.IsDeleted = false;
                rotationattributeGroup.ModifiedByID = View.CurrentUserId;
                rotationattributeGroup.ModifiedOn = DateTime.Now;
                rotationattributeGroup.Code = _serviceResponse.Result.Code;
                rotationattributeGroup.CreatedByID = _serviceResponse.Result.CreatedByID;
                rotationattributeGroup.CreatedOn = _serviceResponse.Result.CreatedOn;
                rotationattributeGroup.RequirementAttributeGroupID = View.RequirementAttributeGroupId;

                ServiceRequest<RequirementAttributeGroupContract, Int32,Boolean> _serviceRequest = new ServiceRequest<RequirementAttributeGroupContract, Int32,Boolean>();
                _serviceRequest.Parameter1 = rotationattributeGroup;
                _serviceRequest.Parameter2 = View.CurrentUserId;
                _serviceRequest.Parameter3 = _serviceResponse.Result == null ? false : true ;

                if (_clientRotationProxy.SaveUpdateRotationAttributeGroup(_serviceRequest).Result)                    
                {
                    View.SuccessMessage = "Attribute group updated successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occured.Please try again.";
                    return false;
                }
            //}
            //else
            //{
            //    View.ErrorMessage = "Attribute group already exist.";
            //    return false;
            //}
        }
        /// <summary>
        /// Delete the Attribute Group
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean DeleteAttributeGroup()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.SelectedTenantID;
            serviceRequest.Parameter2 = View.RequirementAttributeGroupId;

            if (!_clientRotationProxy.IsAttributeGroupMapped(serviceRequest).Result)
            {
                ServiceRequest<Int32, Int32> _serviceRequest = new ServiceRequest<Int32, Int32>();
                _serviceRequest.Parameter1 = View.SelectedTenantID;
                _serviceRequest.Parameter2 = View.RequirementAttributeGroupId;
                var serviceResponse = _clientRotationProxy.GetAttributeGroupById(serviceRequest);
               
                RequirementAttributeGroupContract requirementAttributeGroup = serviceResponse.Result;
                requirementAttributeGroup.IsDeleted = true;
                requirementAttributeGroup.ModifiedByID = View.CurrentUserId;
                requirementAttributeGroup.ModifiedOn = DateTime.Now;

                ServiceRequest<RequirementAttributeGroupContract, Int32, Boolean> _servicRequest = new ServiceRequest<RequirementAttributeGroupContract, Int32, Boolean>();
                _servicRequest.Parameter1 = requirementAttributeGroup;
                _servicRequest.Parameter2 = View.SelectedTenantID;
                _servicRequest.Parameter3 = serviceResponse.Result == null ? false : true;

                if (_clientRotationProxy.SaveUpdateRotationAttributeGroup(_servicRequest).Result)  
                {
                    View.SuccessMessage = "Attribute group deleted successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occured.Please try again.";
                    return false;
                }
            }
            else
            {
                View.ErrorMessage = "This Attribute group is in use.";
                return false;
            }
            return true;
        }

    }
}
