using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.PlacementMatching;
using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.Utils;

namespace CoreWeb.PlacementMatching.Views
{
    public class SharedCustomAttributesPresenter : Presenter<ISharedCustomAttributesView>
    {
        public void GetSharedCustomAttributes()
        {
            View.lstSharedCustomAttributes = new List<SharedCustomAttributesContract>();
            View.lstSharedCustomAttributes = PlacementMatchingSetupManager.GetSharedCustomAttributes();
            if (!View.lstSharedCustomAttributes.IsNullOrEmpty() && !View.lstAgencyRootNodes.IsNullOrEmpty())
            {
                foreach (SharedCustomAttributesContract sharedCustomAttribute in View.lstSharedCustomAttributes)
                {
                    sharedCustomAttribute.AgencyName = View.lstAgencyRootNodes.Where(cond => cond.AgencyID == sharedCustomAttribute.AgencyHierarchyRootNodeID).FirstOrDefault().AgencyName;
                }
            }

            if (View.IsAgencyUserLoggedIn)
            {
                View.lstSharedCustomAttributes = View.lstSharedCustomAttributes.Where(cond => cond.AgencyHierarchyRootNodeID == View.SelectedAgencyRootNodeID).ToList();
            }
        }

        public Boolean SaveSharedCustomAttribute()
        {
            return PlacementMatchingSetupManager.SaveSharedCustomAttribute(View.CurrentLoggedInUserID, View.SharedCustomAttributes);
        }

        public Boolean DeleteSharedCustomAttribute()
        {
            return PlacementMatchingSetupManager.DeleteSharedCustomAttribute(View.CurrentLoggedInUserID, View.SelectSharedCustomAttributeID, View.SelectSharedCustomAttributeMappingID);
        }

        public void GetSharedAttributeDataTypes()
        {
            View.lstAttributeDataType = new List<lkpCustomAttributeDataType>();
            View.lstAttributeDataType = PlacementMatchingSetupManager.GetSharedAttributeDataTypes();
        }

        public void GetSharedAttributeUseTypes()
        {
            View.lstAttributeUseType = new List<lkpSharedCustomAttributeUseType>();
            View.lstAttributeUseType = PlacementMatchingSetupManager.GetSharedAttributeUseTypes();
        }

        public void GetAgencyHierarchyRootNodes()
        {
            View.lstAgencyRootNodes = new List<AgencyHierarchyContract>();
            View.lstAgencyRootNodes = PlacementMatchingSetupManager.GetAgencyHierarchyRootNodes(AppConsts.NONE);
        }

        public void GetAgencyRootNode()
        {
            Dictionary<Int32, String> dicAgencyRootNode = PlacementMatchingSetupManager.GetAgencyRootNode(View.UserId);
            if (!dicAgencyRootNode.IsNullOrEmpty())
            {
                View.SelectedAgencyRootNodeID = dicAgencyRootNode.Keys.FirstOrDefault();
            }
        }
    }
}

