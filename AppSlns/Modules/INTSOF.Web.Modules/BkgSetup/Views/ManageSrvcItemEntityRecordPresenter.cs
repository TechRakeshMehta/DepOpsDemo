using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public class ManageSrvcItemEntityRecordPresenter : Presenter<IManageSrvcItemEntityRecordView>
    {
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {

        }
        public void GetServiceItemEntityList()
        {
            View.ServiceItemEntityList = BackgroundSetupManager.getServiceItemEntityList(View.PackageServiceItemId, View.SelectedTenantId);
        }

        public void GetAttributeList()
        {
            View.AttributeList = BackgroundSetupManager.getAttribteListForServiceItemEntity(View.PackageServiceItemId, View.SelectedTenantId);
        }

        public void GetStateList()
        {
            View.StateList = SecurityManager.GetStates().ToList();
        }

        public void GetCountyList()
        {
            View.CountyList = SecurityManager.GetCounties().Where(x => x.StateID == View.SelectedStateId).ToList();
        }

        public Boolean SavePackageServiceItemEntity()
        {
            List<PackageServiceItemEntity> newServiceItemEntityList = new List<PackageServiceItemEntity>();
            String attributeValue = String.Empty;
            Int32 attributeId = View.CurrentViewContext.SelectedAttributeId;
            attributeValue = View.AttributeValue;
            //If All occurences is checked.
            if (View.CurrentViewContext.ifAllOccurenceChecked)
            {
                attributeValue = "$$$AllOccurences$$$";
            }
            if (View.CurrentViewContext.AttributeType == SvcAttributeDataType.STATE.GetStringValue())
            {
                if (!View.CurrentViewContext.ifAllOccurenceChecked)
                    attributeValue = View.CurrentViewContext.SelectedStateValue;

            }
            if (View.CurrentViewContext.AttributeType == SvcAttributeDataType.COUNTY.GetStringValue())
            {
                if (!View.CurrentViewContext.ifAllOccurenceChecked)
                    attributeValue = View.CurrentViewContext.SelectedStateValue;

                var atttributeIdForState = View.AttributeList.FirstOrDefault(x => x.AttribteType == SvcAttributeDataType.STATE.GetStringValue());
                if (atttributeIdForState != null)
                {
                    attributeId = atttributeIdForState.AttributeGroupMappingId;
                }
                else
                {
                    View.ErrorMessage = "This type of attribute combination is not valid for this Service Item";
                    return false;
                }
            }
           

            PackageServiceItemEntity newServiceItemEntity = new PackageServiceItemEntity
            {
                PSIE_PackageServiceItemID = View.CurrentViewContext.PackageServiceItemId,
                PSIE_AttributeID = attributeId,
                PSIE_Value = attributeValue,
                PSIE_IsDeleted = false
            };
            newServiceItemEntityList.Add(newServiceItemEntity);

            //if county is selected add a record for county and state both.

            if (View.CurrentViewContext.AttributeType == SvcAttributeDataType.COUNTY.GetStringValue())
            {
                string countyAttributeValue = String.Empty;
                if (View.CurrentViewContext.ifAllOccurenceChecked)
                {
                    countyAttributeValue = "$$$AllOccurences$$$";
                }
                else
                {
                    countyAttributeValue = View.CurrentViewContext.SelectedCountyValue;
                }
                PackageServiceItemEntity newServiceItemEntityForCounty = new PackageServiceItemEntity
                {
                    PSIE_PackageServiceItemID = View.CurrentViewContext.PackageServiceItemId,
                    PSIE_AttributeID = View.CurrentViewContext.SelectedAttributeId,
                    PSIE_Value =countyAttributeValue,
                    PSIE_IsDeleted = false
                };
                newServiceItemEntityList.Add(newServiceItemEntityForCounty);
            }
            return BackgroundSetupManager.SavePackageServiceItemEntity(newServiceItemEntityList, View.SelectedTenantId, View.CurrentLoggedInUserId);
        }

        public Boolean DeletePackageServiceItemEntity()
        {
            return BackgroundSetupManager.DeletePackageServiceItemEntityRecord(View.CurrentViewContext.ServiceItemEntityId, View.SelectedTenantId, View.CurrentLoggedInUserId);
        }
    }
}
