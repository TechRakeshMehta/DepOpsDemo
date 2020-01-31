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
    public class SetupServiceAttributeGroupPresenter : Presenter<ISetupServiceAttributeGroupView>
    {

        public void GetServiceAttributeGroups()
        {
            View.ServiceAttributeGroupList = BackgroundSetupManager.GetServiceAttributeGroups();
        }

        public Boolean AddServiceAttributeGroup()
        {
            Boolean isAdded = false;
            if (BackgroundSetupManager.CheckIfSvcAttrGrpNameAlreadyExist(View.ViewContract.ServiceAttributeGroupName, View.ViewContract.ServiceAttributeGroupID))
            {
                View.ErrorMessage = "Service Attribute Group Name can not be duplicate.";
            }

            else
            {
                BkgSvcAttributeGroup newSvcAttrGrp = new BkgSvcAttributeGroup
                {
                    BSAD_Name = View.ViewContract.ServiceAttributeGroupName,
                    BSAD_Description = View.ViewContract.ServiceAttibuteGroupDesc,
                    BSAD_IsEditable = View.ViewContract.IsEditable,
                    BSAD_IsSystemPreConfigured = View.ViewContract.IsSystemPreConfigured,
                    BSAD_IsDeleted = View.ViewContract.IsDeleted,
                    BSAD_CreatedById = View.ViewContract.CreatedByID,
                    BSAD_CreatedDate = View.ViewContract.CreatedOn,
                    BSAD_ModifiedBy = View.ViewContract.ModifiedByID,
                    BSAD_ModifiedDate = View.ViewContract.ModifiedOn,
                    //BSAD_IsDisplay = View.ViewContract.IsDisplay,
                    //BSAD_IsRequired = View.ViewContract.IsRequired,
                   // BSAD_DisplaySequence = View.ViewContract.DisplaySequence
                };
                isAdded = BackgroundSetupManager.SaveServiceAttributeGroup(newSvcAttrGrp);
            }
            return isAdded;
        }

        public Boolean UpdateServiceAttributeGroup()
        {
            Boolean isUpdated = false;
            if (BackgroundSetupManager.CheckIfSvcAttrGrpNameAlreadyExist(View.ViewContract.ServiceAttributeGroupName, View.ViewContract.ServiceAttributeGroupID))
            {
                View.ErrorMessage = "Service Attribute Group Name can not be duplicate.";
            }

            else
            {
                BkgSvcAttributeGroup newSvcAttrGrp = new BkgSvcAttributeGroup
                {
                    BSAD_Name = View.ViewContract.ServiceAttributeGroupName,
                    BSAD_Description = View.ViewContract.ServiceAttibuteGroupDesc,
                    BSAD_IsEditable = View.ViewContract.IsEditable,
                    BSAD_ModifiedBy = View.ViewContract.ModifiedByID,
                    BSAD_ModifiedDate = View.ViewContract.ModifiedOn,
                    //BSAD_IsDisplay = View.ViewContract.IsDisplay,
                    //BSAD_IsRequired = View.ViewContract.IsRequired
                };
                isUpdated = BackgroundSetupManager.UpdateServiceAttributeGroup(newSvcAttrGrp, View.ViewContract.ServiceAttributeGroupID);
            }
            return isUpdated;
        }

        public Boolean DeleteServiceAttributeGroup(Int32 currentUserId)
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfServiceAttributeGroupCanBeDeleted(View.ViewContract.ServiceAttributeGroupID);
            if (response.CheckStatus == CheckStatus.True)
            {
                BkgSvcAttributeGroup svcAttributeGrp = BackgroundSetupManager.GetServiceAttributeGroupBasedOnAttributeGrpID(View.ViewContract.ServiceAttributeGroupID);
                View.ErrorMessage = String.Format(response.UIMessage, svcAttributeGrp.BSAD_Name);
                return false;
            }
            else
            {
                return BackgroundSetupManager.DeleteServiceAttributeGroup(View.ViewContract.ServiceAttributeGroupID, currentUserId);
            }
        }
    }
}
