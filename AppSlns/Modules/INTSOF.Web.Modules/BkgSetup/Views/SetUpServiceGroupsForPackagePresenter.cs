using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class SetUpServiceGroupsForPackagePresenter : Presenter<ISetUpServiceGroupsForPackageView>
    {
        public void SaveServiceGroupDetail(BkgPackageSvcGroup bkgPackageSvcGroup)
        {
            View.ErrorMessage = BackgroundSetupManager.SaveEditServiceGroupDetail(View.tenantId, bkgPackageSvcGroup);
        }

        public void GetServiceGroupGridData()
        {
            View.lstSvcGroup = BackgroundSetupManager.GetServiceGroupGridData(View.tenantId, View.packageId);
        }

        public List<BkgSvcGroup> GetServiceGroupForDropDown()
        {
            List<Int32> lstTobeRemoved = View.lstSvcGroup.Select(x => x.BSG_ID).ToList();
            List<BkgSvcGroup> tempBkgSvcGroupList = BackgroundSetupManager.GetServiceGroupForDropDown(View.tenantId, lstTobeRemoved).OrderBy(col => col.BSG_Name).ToList();
            tempBkgSvcGroupList.Insert(0, new BkgSvcGroup { BSG_ID = 0, BSG_Name = "Create New" });
            return tempBkgSvcGroupList;
        }

        public List<LocalAttributeGroupMappedToBkgPackage> GetAttributeGroupForDropDown()
        {
            //List<Int32> lstTobeRemoved = new List<Int32>() { 1, 10, 13 }; // 09/07/2014 - To Remove Personal Information,Personal Alias, MVR from Attribute Group
            return BackgroundSetupManager.GetAttributeGroupMappedToBkgPackage(View.tenantId, View.packageId).OrderBy(col => col.AttributeGroupName).ToList();
            //.Where(cond=> !lstTobeRemoved.Contains(cond.AttributeGroupId)).ToList();

        }

        public void GetBkgPkgAttributeGroupInstructionText()
        {
            BkgPkgAttributeGroupInstruction bkgPkgAttrGrpInstruction = BackgroundSetupManager.GetBkgPkgAttributeGroupInstructionText(View.tenantId, View.packageId, View.SelectedAttrGrp);
            if (bkgPkgAttrGrpInstruction.IsNotNull())
                View.InstructionText = bkgPkgAttrGrpInstruction.BPAGI_InstructionText;
        }

        public Boolean SavePkgAttrGroupInstruction(Int32 currentLoggedInUserId)
        {
            BkgPkgAttributeGroupInstruction bkgPkgAttrGrpInstruction = BackgroundSetupManager.GetBkgPkgAttributeGroupInstructionText(View.tenantId, View.packageId, View.SelectedAttrGrp);
            if (bkgPkgAttrGrpInstruction.IsNotNull())
            {
                bkgPkgAttrGrpInstruction.BPAGI_InstructionText = View.InstructionText;
                bkgPkgAttrGrpInstruction.BPAGI_ModifiedByID = currentLoggedInUserId;
                bkgPkgAttrGrpInstruction.BPAGI_ModifiedOn = DateTime.UtcNow;
                if (BackgroundSetupManager.UpdateTenantChanges(View.tenantId))
                    return true;
            }
            else
            {
                BkgPkgAttributeGroupInstruction bkgPkgAttrGrpInstructionNew = new BkgPkgAttributeGroupInstruction
                {
                    BPAGI_BackgroundPackageID = View.packageId,
                    BPAGI_BkgSvcAttributeGroupID = View.SelectedAttrGrp,
                    BPAGI_InstructionText = View.InstructionText,
                    BPAGI_CreatedByID = currentLoggedInUserId,
                    BPAGI_CreatedOn = DateTime.UtcNow,
                    BPAGI_IsDeleted = false

                };
                return BackgroundSetupManager.SaveBkgPkgAttributeGroupInstruction(View.tenantId, bkgPkgAttrGrpInstructionNew);
            }
            return false;
        }

        public void UpdatePackage(BackgroundPackage backgroundPackage, Int32 currentLoggedInUserId, List<Int32> targetPackageIds, Int32 months,Boolean isActive)
        {
            View.ErrorMessage = BackgroundSetupManager.SaveEditPackagedetail(View.tenantId, backgroundPackage, currentLoggedInUserId, targetPackageIds, months,isActive, View.packageId, true);
        }

        public BackgroundPackage GetPackageDetail()
        {
            return BackgroundSetupManager.GetPackageDetail(View.tenantId, View.packageId);
        }

        public Boolean DeleteServiceGroupMapping(Int32 serviceGroupId)
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfServiceGroupMappingCanBeDeleted(serviceGroupId, View.packageId, View.tenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                View.ErrorMessage = response.UIMessage;
                return false;
            }
            else
            {
                return BackgroundSetupManager.DeleteServiceGroupMapping(View.tenantId, serviceGroupId, View.packageId);
            }

        }

        public void SaveNewServiceGroupDetail(BkgSvcGroup bkgSvcGroup, Int32 currentLoggedInUserId)
        {
            Int32 bkgSvcGroupId = BackgroundSetupManager.SaveServiceGroup(bkgSvcGroup, currentLoggedInUserId, View.tenantId);
            if (bkgSvcGroupId > 0)
                View.ErrorMessage = String.Empty;
            else
                View.ErrorMessage = "Some error has occured.Please try again.";
        }

        #region UAT-803
        public void GetStateList()
        {
            List<Entity.State> objStateList = SecurityManager.GetStates().Where(x => x.Country.CountryID == AppConsts.COUNTRY_USA_ID && !x.StateName.Equals(AppConsts.COMBOBOX_ITEM_SELECT)).ToList();

            if (objStateList.IsNotNull() && objStateList.Count > 0)
            {
                View.lstState = objStateList;
            }
        }


        public void SaveBkgPkgStateSearchCriteria()
        {
            if (BackgroundSetupManager.SaveBkgPkgStateSearchCriteria(View.tenantId, View.lstStateSearchContract, View.CurrentLoggedInUserId))
            {
                View.ErrorMessage = String.Empty;
            }
            else
            {
                View.ErrorMessage = "Some error has ocured while saving the state search criteria. Please try again.";
            }
        }

        public void GetBkgPkgStateSearchCriteria()
        {
            View.lstBkgPkgStateSearch = BackgroundSetupManager.GetBkgPkgStateSearchCriteria(View.packageId, View.tenantId);
        }
        #endregion


        public void UpdateStateSearchSettingsFromMaster()
        {
            View.lstBkgPkgStateSearch = BackgroundSetupManager.UpdateStateSearchSettingsFromMaster(View.tenantId, View.CurrentLoggedInUserId, View.packageId);
            if (View.lstBkgPkgStateSearch.IsNotNull() && View.lstBkgPkgStateSearch.Count > 0)
            {
                View.ErrorMessage = String.Empty;
            }
            else
            {
                View.ErrorMessage = "State Search criteria can not be updated from Master";
            }
        }

        public void GetPackageNotesPosition(String selectedPositionCode)
        {
            View.NotesPositionId = BackgroundSetupManager.GetPackageNotesPosition(View.tenantId, selectedPositionCode).PNP_ID;
        }

        #region UAT:2388
        public void GetBkgPackages()
        {
            List<BackgroundPackage> tempBkgPackages = new List<BackgroundPackage>();
            if (View.tenantId > AppConsts.NONE && View.packageId > AppConsts.NONE)
            {
                tempBkgPackages = BackgroundSetupManager.GetAutomaticInvitationBackgroundPackages(View.tenantId, View.packageId);
            }
            if (!tempBkgPackages.IsNullOrEmpty())
            {
                tempBkgPackages = tempBkgPackages.OrderBy(col => col.BPA_Name).ToList();
            }
            View.lstBackgroundPackage = tempBkgPackages;
        }
        public Boolean GetAutomaticPackageInvitationSetting()
        {
            Boolean result =  BackgroundSetupManager.GetAutomaticPackageInvitationSetting(View.tenantId, View.packageId);
            View.isAutomaticPackageInvitationActive = result;
            return result;
        }
        #endregion

        #region UAT-3268:- Flow for if applicant is allowed in a rotation.

        public void GetRotationQualifyingSetting()
        {
            View.IsReqToQualifyInRotation = BackgroundSetupManager.GetRotationQualifyingSetting(View.tenantId, View.packageId);
        }
        #endregion

        #region UAT-3525
        public List<BkgPackageType> GetBkgPackageType()
        {
            return BackgroundSetupManager.GetAllBkgPackageTypes(View.tenantId).ToList();
        }
        #endregion
    }
}
