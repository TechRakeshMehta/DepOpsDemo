using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementVerificationReadOnlyItemControlView
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
        IRequirementVerificationReadOnlyItemControlView CurrentViewContext
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

        #region UAT-1470 :As a student, there should be a way to close out of the video once you open it.
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
    }
}
