using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.RotationPackages.Views
{
    public interface ISetupRequirementItemView
    {
        /// <summary>
        /// Requirement CategoryId
        /// </summary>
        Int32 RequirementCategoryID { get; set; }

        /// <summary>
        /// Requirement Item ID
        /// </summary>
        Int32 RequirementItemID { get; set; }

        /// <summary>
        /// Set or get Item Name.
        /// </summary>
        String ItemName { get; set; }

        String ItemLabel { get; set; }

        String ErrorMessage { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        List<RequirementItemURLContract> listRequirementItemURLContract { get; set; }
         List<DocumentUrlContract> DocumentUrlTempList { get; set; }
        

        Boolean IsEditMode { get; set; }
        List<RequirementItemContract> lstCategoryItems { get; set; }
        List<RequirementFieldContract> lstItemFields { get; set; }
        Boolean IsVersionRequired { get; set; }

        /// <summary>
        /// Selected Package ID
        /// </summary>
        Int32 SelectedPackageID { get; set; }

        String ItemHId { get; set; }

        #region UAT-2305

        //List<UniversalItemContract> lstUniversalItems { get; set; }

        //Int32 UniCatItmMappingID { get; set; }

        //UniversalItemContract UniversalItemData { get; set; }

        #endregion

        //UAT-2213:New Rotation Package Process: Master Setup
        Boolean IsNewPackage { get; set; }

        Boolean IsAllowDataMovement { get; set; }

        #region UAT-2676
        String ExplanatoryNotes
        {
            get;
            set;
        }
        #endregion

        #region UAT-3078
        Int32 RequirementItemDisplayOrder { get; set; }
        #endregion
        //UAT-3077
        /// <summary>
        /// Set or get Item Name.
        /// </summary>
        Boolean IsPaymentType
        {
            get;
            set;
        }

        /// <summary>
        /// Set or get Item Name.
        /// </summary>
        Decimal? Amount
        {
            get;
            set;
        }


        Int32 SelectedExistigItemID { get; set; }

        #region UAT-3309
        String ReqItemSampleDocumentFormURL { get; set; }
        #endregion
        Boolean IsAllowItemDataEntry { get; set; }
        #region UAt-4165
        Dictionary<String, Boolean> lstEditableBy
        { get; set; }
        Boolean IsCustomSettings { get; set; }
        #endregion

        Boolean IsDetailsEditable { get; set; }
    }
}
