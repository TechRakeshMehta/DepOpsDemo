using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementVerificationItemControl
    {
        /// <summary>
        /// RequirementItemID
        /// </summary>
        Int32 ItemId { get; set; }

        /// <summary>
        /// ApplicantRequirementItemDataID
        /// </summary>
        Int32 ApplReqItemDataId { get; set; }

        /// <summary>
        /// Represents the Item level data
        /// </summary>
        List<RequirementVerificationDetailContract> lstItemLevelData
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the current Context
        /// </summary>
        IRequirementVerificationItemControl CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// List for 'lkpRequirementItemStatus' entity
        /// </summary>
        List<RequirementItemStatusContract> lstReqItemStatusTypes
        {
            get;
            set;
        }

        /// <summary>
        /// ID Prefix for the Field Control
        /// </summary>
        String FieldControlIdPrefix
        {
            get;
        }

        /// <summary>
        /// SelectedTenantId
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        String FieldControlIdGenerator
        {
            get;
        }

        #region UAT-1470
        /// <summary>
        /// PackageID
        /// </summary>
        Int32 PackageID { get; set; }

        /// <summary>
        /// CategoryID
        /// </summary>
        Int32 CategoryID { get; set; }

        List<RequirementObjectTreeContract> LstRequirementObjTreeProperty { get; set; }
        #endregion 

        /// <summary>
        /// Represents the screen from which the screen was opened
        /// </summary>
        String ControlUseType
        {
            get;
            set;
        }

       
        //UAT-2224: Admin access to upload/associate documents on rotation package items.
        Int32 SelectedApplicantId_Global { get; set; }
        Int32 CurrentRequirementPackageSubscriptionID_Global { get; set; }
        List<ApplicantDocumentContract> lstApplicantDocument { get; set; }
        List<ApplicantFieldDocumentMappingContract> lstApplicantRequirementDocumentMaps { get; set; }

        //UAT 2371
        String EntityPermissionName
        {
            get;
            set;
        }
        //UAT-4543
        Boolean IsViewDocFieldViewed
        {
            get;
            set;
        }
        #region UAT-3309
        String RequirementItemSampleDocURL
        {
            get;
            set;
        }
        #endregion

        #region UAT-4165
        Boolean IsItemEditable
        {
            get;
            set;
        }
        Int32 CurrentTenantId_Global { get; set; }
        #endregion

        #region UAT-4260

        void SetValidationMessage(String validationMessage);
        #endregion
        #region UAT-4368
        Boolean IsClientAdminLoggedIn
        {
            get;
            set;
        }
        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        #endregion
    }
}
