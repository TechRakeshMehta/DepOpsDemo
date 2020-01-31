using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class SetUpAttributeForAttributeGroupPresenter : Presenter<ISetUpAttributeForAttributeGroupView>
    {
        #region Public Methods

        /// <summary>
        /// Method to get all maaped attributes with attribute group.
        /// </summary>
        public void GetMappedAttributeWithGroup()
        {
            View.MappedAttributeList = BackgroundSetupManager.GetMappedAttributeList(View.TenantId, View.BackgroundServiceId, View.BackgroundServiceGroupId, View.AttributeGroupId, View.BackgroundPackageId);
            if (View.MappedAttributeList.Count > 0)
                View.MappedAttributeIds = View.MappedAttributeList.Select(x => x.AttributeID).ToList();
            else
                View.MappedAttributeIds = new List<Int32>();
        }

        /// <summary>
        /// Method to delete mapping of attribute with service and attribute group.
        /// </summary>
        /// <param name="bkgPackageSvcAttributeMappingId"></param>
        /// <returns></returns>
        public Boolean DeletedBkgSvcAttributeMapping(Int32 bkgPackageSvcAttributeMappingId)
        {
            IntegrityCheckResponse responseOrderPlaced = BackgroundServiceIntegrityManager.IfAttributeMappingCanBeDeleted(View.BackgroundPackageId, View.TenantId);
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfAttributeAssociatedWithExtSVC(View.TenantId, bkgPackageSvcAttributeMappingId, View.BackgroundServiceId);
            if (response.CheckStatus == CheckStatus.True || responseOrderPlaced.CheckStatus == CheckStatus.True)
            {
                if (responseOrderPlaced.CheckStatus == CheckStatus.True)
                View.ErrorMessage = responseOrderPlaced.UIMessage;
                else if (response.CheckStatus == CheckStatus.True && responseOrderPlaced.CheckStatus == CheckStatus.True)
                    View.ErrorMessage = responseOrderPlaced.UIMessage;
                else if (response.CheckStatus == CheckStatus.True)
                    View.ErrorMessage = response.UIMessage;
                return false;
            }
            else
            {
                return BackgroundSetupManager.DeletedBkgSvcAttributeMapping(View.TenantId, bkgPackageSvcAttributeMappingId, View.CurrentLoggedInUserId);
            }
        }

        /// <summary>
        /// Method to get All attribute 
        /// </summary>
        public void GetAllAttributeList()
        {
            List<AttributeDataSecurityClient> tempAttributeList = new List<AttributeDataSecurityClient>();
            if (View.TenantId > 0)
                tempAttributeList = BackgroundSetupManager.GetAllAttribute(View.TenantId, View.MappedAttributeIds, View.AttributeGroupId);
            tempAttributeList.Insert(0, new AttributeDataSecurityClient { AttributeId = 0, AttributeName = "Create New" });
            View.AttributeList = tempAttributeList;
        }

        public String IsmappingOfThisTypeAllowed(String attributeType)
        {
            return BackgroundSetupManager.IsmappingOfThisTypeAllowed(View.TenantId, attributeType, View.AttributeGroupId);
        }

        /// <summary>
        /// Save new attribute and its mapping .
        /// </summary>
        /// <param name="bkgSvcAttributeData">bkgSvcAttributeData</param>
        /// <returns></returns>
        public Boolean SaveAttributeAndMapping(BkgSvcAttribute bkgSvcAttributeData)
        {
            return BackgroundSetupManager.SaveAttributeAndMapping(bkgSvcAttributeData, View.TenantId, View.AttributeGroupId, View.BkgPackageSvcId, View.CurrentLoggedInUserId, View.IsRequired, View.IsDisplay,View.IsHiddenFromUI);
        }

        /// <summary>
        /// Save existing attribute mapping.
        /// </summary>
        /// <returns></returns>
        public Boolean SaveExistingAttributeMapping()
        {
            return BackgroundSetupManager.SaveExistingAttributeMapping(View.SelectedAttributeId, View.TenantId, View.AttributeGroupId, View.BkgPackageSvcId, View.CurrentLoggedInUserId, View.IsRequired, View.IsDisplay);
        }

        /// <summary>
        /// Get BkgPackageSvcId on the basis of serviceId, serviceGroupId and packageId.
        /// </summary>
        public void GetBkgPackageSvcId()
        {
            View.BkgPackageSvcId = BackgroundSetupManager.GetBkgPackageSvcId(View.TenantId, View.BackgroundServiceId, View.BackgroundServiceGroupId, View.BackgroundPackageId);
        }

        /// <summary>
        ///Get Attribyte Data Type List. 
        /// </summary>
        public void GetAttributeDataType()
        {
            List<lkpSvcAttributeDataType> tempServiceAttributeDtatType = new List<lkpSvcAttributeDataType>();
            if (View.TenantId > 0)
            {
                tempServiceAttributeDtatType = BackgroundSetupManager.GetAttributeDataType(View.TenantId);
            }
            tempServiceAttributeDtatType.Insert(0, new lkpSvcAttributeDataType { SADT_ID = 0, SADT_Name = "--SELECT--" });

            View.listAttributeDataType = tempServiceAttributeDtatType;
        }

        #endregion

        public Boolean UpdateDisplaySequence(IList<AttributeSetupContract> statusToMove, Int32? destinationIndex)
        {
            return BackgroundSetupManager.UpdateAttributeDisplaySequence(View.TenantId, statusToMove, destinationIndex, View.CurrentLoggedInUserId);
        }

        public void GetAttributesWithGroup()
        {
            View.MappedAttributeList = BackgroundSetupManager.GetBkgAttributesBasedOnGroup(View.TenantId, View.AttributeGroupId);
        }
    }
}
