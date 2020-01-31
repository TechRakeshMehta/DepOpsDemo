using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils;


namespace CoreWeb.BkgSetup.Views
{
    public class ManageServiceAttributeGroupPresenter : Presenter<IManageServiceAttributeGroupView>
    {
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetAttributeGroup()
        {
            View.SvcAttributeGrpLst = BackgroundSetupManager.GetAttributeGrps(View.ServiceId, View.DefaultTenantId);
        }
        public void GetAllAttributeGroups(Boolean isupdate)
        {
            View.ListAttributeGrps = BackgroundSetupManager.GetAllAttributeGroups(View.ServiceId, isupdate, View.DefaultTenantId);
        }

        public void GetAllAttributes(Int32 attributegrpID)
        {
            View.ListAttributes = BackgroundSetupManager.GetAllAttributes(attributegrpID, View.DefaultTenantId);
        }

        public void SaveAttributeGrpMappings(List<Int32> ChkAttributeIdsLst)
        {
            if (ChkAttributeIdsLst.IsNullOrEmpty() && ChkAttributeIdsLst.Count() < 0)
            {
                View.ErrorMessage = "Please Select atleast one Attribute";
                return;
            }
            else
            {
                //get all AttributegrpmappingIds to map with service
                List<Int32> _attributesGroupMappingIds = BackgroundSetupManager.GetAllAttributesMappingIDs(ChkAttributeIdsLst, View.SelectedAttributeGrp, View.DefaultTenantId);
                if (_attributesGroupMappingIds.Count > 0)
                {
                    foreach (Int32 svcAttributegrpMappingId in _attributesGroupMappingIds)
                    {
                        Entity.BkgSvcAttributeGroupMapping newSvcAttributeGrpMapping = new Entity.BkgSvcAttributeGroupMapping();
                        newSvcAttributeGrpMapping.BSAGM_ServiceId = View.ServiceId;
                        newSvcAttributeGrpMapping.BSAGM_AttributeGroupMappingID = svcAttributegrpMappingId;
                        newSvcAttributeGrpMapping.BSAGM_IsEditable = true;
                        newSvcAttributeGrpMapping.BSAGM_IsSystemPreConfigured = false;
                        newSvcAttributeGrpMapping.BSAGM_CreatedBy = View.CurrentLoggedInUserId;
                        newSvcAttributeGrpMapping.BSAGM_CreatedOn = DateTime.Now;
                        newSvcAttributeGrpMapping.BSAGM_Code = Guid.NewGuid();
                        BackgroundSetupManager.SaveAttributeGrpMappings(newSvcAttributeGrpMapping, View.DefaultTenantId);
                    }
                }
                else
                {
                    View.ErrorMessage = "There is no Mapping Exist between Attribute Group and Attributes.";
                }
            }
        }


        public List<Int32> GetAllAttributeIDsRelatedToAttributeGrpID(Int32 attributegrpID)
        {
            return BackgroundSetupManager.GetAllAttributeIDsRelatedToAttributeGrpID(attributegrpID, View.ServiceId, View.DefaultTenantId);
        }

        public void UpdateAtttributeMappingLst(Int32 attributegrpID, List<Int32> updatedattributeIdLst)
        {
            if (updatedattributeIdLst.Count > 0)
            {
                BackgroundSetupManager.UpdateAtttributeMappingLst(attributegrpID, View.ServiceId, View.CurrentLoggedInUserId, updatedattributeIdLst, View.DefaultTenantId);
            }
            else
            {
                View.ErrorMessage = "Please Select atleast one Attribute to map the group";
                return;
            }
        }

        public void DeleteAttributeServiceMappingByAttributeId(Int32 attributegrpID, Int32 attributeId) 
        {
            BackgroundSetupManager.DeleteAttributeServiceMappingByAttributeId(attributegrpID, attributeId, View.ServiceId, View.CurrentLoggedInUserId);
        }


        public void DeleteAttributMappingwithServicebyAttributeGroupid(Int32 attributegrpID) 
        {
            BackgroundSetupManager.DeleteAttributMappingwithServicebyAttributeGroupid(attributegrpID, View.ServiceId, View.CurrentLoggedInUserId);
        }
        // void DeleteAttributMappingwithServicebyAttributeGroupid(Int32 attributegrpID, Int32 serviceId, Int32 currentLoggedInUserId)
    }
}
