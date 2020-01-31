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
    public class ConfigureCustomFormPresenter : Presenter<IConfigureCustomFormView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetAllAttrGrpForCustomForm()
        {
            View.CustomFormAttributeGroups = BackgroundSetupManager.GetCustomFormAttrGrpsByCustomFormId(View.ViewContract.CustomFormConfigSelectedCustomFormID);
        }

        public void SetCustomFormInfo()
        {
            CustomForm customForm = BackgroundSetupManager.GetCurrentCustomFormInfo(View.ViewContract.CustomFormConfigSelectedCustomFormID);
            View.CustomFormName = customForm.CF_Name;
            View.CustomFormTitle = customForm.CF_Title;
        }

        public string GetAttrGrpNameById(int attrGrpId)
        {
            BkgSvcAttributeGroup attributeGroup = BackgroundSetupManager.GetAllBkgSvcAttributeGroup().Where(obj => obj.BSAD_ID == attrGrpId).FirstOrDefault();
            return (attributeGroup != null) ? attributeGroup.BSAD_Name : "";
        }

        public void GetAllBkgSvcAttributeGroupNotMapped(int? currentAttributeGrp, String currentAttributeGrpName = null)
        {
            // Fetch already mapped AttributeGroupIds
            List<Int32?> lstMappedAttrGrpId = BackgroundSetupManager.GetCustomFormAttrGrpsByCustomFormId(View.ViewContract.CustomFormConfigSelectedCustomFormID).Select(obj => obj.CFAG_BkgSvcAttributeGroupId).ToList();

            // As Per Task : User should not be able to add following attribute groups to custom forms: Personal Information, MVR Attribute Group.	Sumit	06-06-2014	06-06-2014	
            //VM : Education History,Personal Alias-13-06-2014

            //Guid currentGuid = new Guid(); // Fetch Guid of Attr Grp of currentAttributeGrp ID
            Guid currentGuid = BackgroundSetupManager.GetCodeForCurrentAttributeGroup(currentAttributeGrp);
            List<Guid> lstRestrictedAttrGrpstemp = new List<Guid>();
            lstRestrictedAttrGrpstemp.Add(new Guid("CC184FC4-5401-445D-90AA-E77167227904"));
            lstRestrictedAttrGrpstemp.Add(new Guid("CF76960D-2120-46FE-9E03-01C218F8A336"));


            //List<String> lstRestrictedAttrGrps = new List<String> { "Personal Information","MVR Attribute Group" }; 

            //Allow Current Attribute in list
            if (currentAttributeGrp.HasValue)
                lstMappedAttrGrpId = lstMappedAttrGrpId.Where(obj => obj.Value != currentAttributeGrp.Value).ToList();
            if (!currentGuid.IsNullOrEmpty())
                lstRestrictedAttrGrpstemp = lstRestrictedAttrGrpstemp.Where(obj => obj != currentGuid).ToList();
            //if (!currentAttributeGrpName.IsNullOrEmpty())
            //    lstRestrictedAttrGrps = lstRestrictedAttrGrps.Where(obj => obj != currentAttributeGrpName).ToList();

            // Remove already mapped AttributeGroupIds  & Restricted Attribute Groups         
            List<BkgSvcAttributeGroup> lstServiceAttributeGrp = BackgroundSetupManager.GetAllBkgSvcAttributeGroup().Where(obj => !lstMappedAttrGrpId.Contains(obj.BSAD_ID)).ToList();
            if (lstRestrictedAttrGrpstemp.IsNotNull())
                lstServiceAttributeGrp = lstServiceAttributeGrp.Where(x => !lstRestrictedAttrGrpstemp.Contains(x.BSAD_Code)).OrderBy(con => con.BSAD_Name).ToList();

            lstServiceAttributeGrp.Insert(0, new BkgSvcAttributeGroup { BSAD_Name = "--Select--", BSAD_ID = 0 });
            View.lstAttributeGroup = lstServiceAttributeGrp;

        }

        /// <summary>
        /// Saves the Custom Form Attribute Group in the MasterDB.
        /// </summary>
        public void SaveCustomFormAttributeGroup()
        {
            if (BackgroundSetupManager.CheckIfCustomFormAttrGrpMappingAlreadyExist(View.ViewContract.CustomFormConfigSelectedCustomFormID, View.ViewContract.CustomFormConfigSelectedAttrGroup))
            {
                View.ErrorMessage = "Custom Form Mapping with Attribute Group can not be duplicate.";
            }
            else
            {
                CustomFormAttributeGroup newCustomFormAttrGrp = new CustomFormAttributeGroup
                {
                    CFAG_SectionTitle = View.ViewContract.CustomFormConfigSectionTitle,
                    CFAG_CustomHTML = View.ViewContract.CustomFormConfigCustomHTML,
                    CFAG_DisplayColumn = (int)View.ViewContract.CustomFormConfigDisplayColumn,
                    CFAG_Occurrence = View.ViewContract.CustomFormConfigOccurrence,
                    CFAG_BkgSvcAttributeGroupId = View.ViewContract.CustomFormConfigSelectedAttrGroup,
                    CFAG_CustomFormId = View.ViewContract.CustomFormConfigSelectedCustomFormID
                };
                BackgroundSetupManager.SaveCustomFormAttributeGroupDetail(newCustomFormAttrGrp, View.CurrentLoggedInUserId);
            }
        }

        /// <summary>
        /// Updates the Custom Form Attribute Group Mapping.
        /// </summary>
        public void UpdateCustomFormAttributeGroup()
        {
            if (BackgroundSetupManager.GetCurrentCustomFormAttributeGroup(View.ViewContract.CustomFormConfigID) == null)
            {
                View.ErrorMessage = "Custom Form Attribute Mapping does not exist.";
            }
            else
            {
                CustomFormAttributeGroup newCustomFormAttrGrp = new CustomFormAttributeGroup
                {
                    CFAG_SectionTitle = View.ViewContract.CustomFormConfigSectionTitle,
                    CFAG_CustomHTML = View.ViewContract.CustomFormConfigCustomHTML,
                    CFAG_DisplayColumn = (int)View.ViewContract.CustomFormConfigDisplayColumn,
                    CFAG_Occurrence = View.ViewContract.CustomFormConfigOccurrence,
                    CFAG_BkgSvcAttributeGroupId = View.ViewContract.CustomFormConfigSelectedAttrGroup
                };
                BackgroundSetupManager.UpdateCustomFormAttributeGroupDetail(newCustomFormAttrGrp, View.ViewContract.CustomFormConfigID, View.CurrentLoggedInUserId);
            }
        }

        /// <summary>
        /// Deletes the Custom Form Attribute Group.
        /// </summary>
        public Boolean DeleteCustomFormAttributeGroup()
        {

            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.CheckIfBkgSvcCustomFormMappingExist(View.ViewContract.CustomFormConfigSelectedCustomFormID);
            if (response.CheckStatus == CheckStatus.True)
            {
                CustomForm customForm = BackgroundSetupManager.GetCurrentCustomFormInfo(View.ViewContract.CustomFormConfigSelectedCustomFormID);
                View.ErrorMessage = String.Format(response.UIMessage, customForm.CF_Name);
                return false;
            }
            else
            {
                Boolean result = BackgroundSetupManager.DeleteCustomFormAttributeGroup(View.ViewContract.CustomFormConfigID, View.CurrentLoggedInUserId); ;
                if (!result)
                    View.ErrorMessage = "Custom Form Attribute Group can not be removed. Try again!";
                return result;
            }
        }

        /// <summary>
        /// Gets specific CustomFormAttributeGroup 
        /// </summary>
        /// <param name="customFormAttributeGroupID">customFormAttributeGroupID</param>
        public CustomFormAttributeGroup getCurrentCustomFormAttributeGroup(Int32 customFormAttributeGroupID)
        {
            return BackgroundSetupManager.GetCurrentCustomFormAttributeGroup(customFormAttributeGroupID);
        }

        /// <summary>
        ///Updates Custom Form Attribute Group Mapping Sequence.
        /// </summary>
        /// <param name="customFormsToMove">IList of CustomFormAttributeGroup Entity</param>
        /// <param name="destinationIndex">Index</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public Boolean UpdateCustomFormAttributeGroupSequence(IList<CustomFormAttributeGroup> customFormAttributeGroupsToMove, Int32 destinationIndex)
        {
            return BackgroundSetupManager.UpdateCustomFormAttributeGroupSequence(customFormAttributeGroupsToMove, destinationIndex, View.CurrentLoggedInUserId);

        }
    }
}
