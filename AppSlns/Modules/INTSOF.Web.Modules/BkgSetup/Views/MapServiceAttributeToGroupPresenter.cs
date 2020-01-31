using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;


namespace CoreWeb.BkgSetup.Views
{
    public class MapServiceAttributeToGroupPresenter : Presenter<IMapServiceAttributeToGroupView>
    {
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {

        }

        public void GetMappedAttributes()
        {
            View.MappedSvcAttributeList = BackgroundSetupManager.GetMappedAttributes(View.ServiceGroupId);
        }

        public void GetUnmappedAttributes()
        {
            View.UnmappedSvcAttributeList = BackgroundSetupManager.GetUnmappedAttributes(View.ServiceGroupId);
        }

        public void GetSourceAttributes()
        {
            View.SourceSvcAttributeList = BackgroundSetupManager.GetSourceAttributes(View.SelectedAttributeId,View.SelectedAttributeGrp);
        }

        public void SaveAttributeGrpMappings(List<Int32> lstAttributeIds)
        {
            List<BkgAttributeGroupMapping> lstBkgSvcAttributeGroupMapping = new List<BkgAttributeGroupMapping>();
            for (int i = 0; i < lstAttributeIds.Count(); i++)
            {
                BkgAttributeGroupMapping newAttributeGroupMapping = new BkgAttributeGroupMapping()
                {
                    BAGM_BkgSvcAtributeID = lstAttributeIds[i],
                    BAGM_BkgSvcAttributeGroupId = View.ServiceGroupId,
                    BAGM_Code = Guid.NewGuid(),
                    BAGM_IsDeleted = false,
                    BAGM_IsEditable = true,
                    BAGM_IsSystemPreConfigured = false,
                    BAGM_CreatedBy = View.CurrentLoggedInUserId,
                    BAGM_IsRequired = View.ViewContract.IsRequired,
                    BAGM_IsDisplay = View.ViewContract.IsDisplay,
                    BAGM_CreatedOn = DateTime.Now,
                    BAGM_IsHiddenFromUI=View.ViewContract.IsHiddenFromUI
                };
                lstBkgSvcAttributeGroupMapping.Add(newAttributeGroupMapping);
            }

            BackgroundSetupManager.SaveAttributeGroupMapping(lstBkgSvcAttributeGroupMapping, View.ServiceGroupId);
        }

        public Boolean DeleteAttributeGroupMapping()
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfAttributeGroupMappingCanBeDeleted(View.ViewContract.AttributeGroupMappingID);
            if (response.CheckStatus == CheckStatus.True)
            {
                BkgSvcAttribute svcAttribute = BackgroundSetupManager.GetMasterServiceAttributeBasedOnAttributeID(View.ViewContract.AttributeID);
                View.ErrorMessage = String.Format(response.UIMessage, svcAttribute.BSA_Name);
                return false;
            }
            else
            {
                return BackgroundSetupManager.DeleteAttributeGroupMapping(View.ViewContract.AttributeGroupMappingID, View.CurrentLoggedInUserId);
            }
        }

        public Boolean UpdateAttributeSequence(IList<MapServiceAttributeToGroupContract> attributesToMove, Int32? destinationIndex)
        {
            return BackgroundSetupManager.UpdateAttributeSequence(attributesToMove, destinationIndex, View.CurrentLoggedInUserId);

        }

        public Boolean UpdateAttributeGrpMappings()
        {
            return BackgroundSetupManager.UpdateAttributeGroupMapping(View.ViewContract.AttributeGroupMappingID, View.CurrentLoggedInUserId, Convert.ToBoolean(View.ViewContract.IsRequired), Convert.ToBoolean(View.ViewContract.IsDisplay), View.ViewContract.SourceAttributeID,Convert.ToBoolean(View.ViewContract.IsHiddenFromUI));
        }
    }
}
