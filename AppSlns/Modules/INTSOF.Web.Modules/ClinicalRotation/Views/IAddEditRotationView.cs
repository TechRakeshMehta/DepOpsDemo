using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using System;
using System.Collections.Generic;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IAddEditRotationView
    {
        List<MultipleAdditionalDocumentsContract> MultipleAdditionalDocumentsContract { get; set; }

        AgencyHierarchyRotationFieldOptionContract agencyHierarchyRotationFieldOptionContract { get; set; }

        string hierarchyID
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }

        List<AgencyDetailContract> lstAgency
        {
            get;
            set;
        }

        Int32 SelectedAgencyID
        {
            get;
            set;
        }

        List<CustomAttribteContract> GetCustomAttributeList
        {
            get;
            set;
        }

        List<ClientContactContract> ClientContactList
        {
            get;
            set;
        }

        List<WeekDayContract> WeekDayList
        {
            get;
            set;
        }

        Int32 SelectedTenantID
        {
            set;
            get;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            set;
            get;
        }

        Dictionary<String, String> dicGranularPermissions
        {
            get;
            set;
        }

        List<TenantDetailContract> lstTenant
        {
            get;
            set;
        }

        String HierarchyNode
        {
            get;
            set;
        }

        ClinicalRotationDetailContract ViewContract
        {
            get;
        }

        List<Int32> SelectedClientContacts
        {
            get;
            set;
        }

        List<CustomAttribteContract> SaveCustomAttributeList
        {
            get;
            set;
        }

        #region UAT-2424
        List<ClinicalRotationDetailContract> lstClinicalRotation
        {
            get;
            set;
        }
        Int32 SelectedRotationIDForCloning
        {
            get;
            set;
        }

        ClinicalRotationDetailContract CloneContract
        {
            get;
            set;
        }

        #endregion

        Int32 SelectedRotationID
        {
            set;
            get;
        }

        ClinicalRotationDetailContract clinicalRotationDetailContract
        {
            set;
            get;
        }

        List<RotationFieldUpdatedByAgencyContract> lstRotationFieldUpdaeByAgency
        {
            set;
            get;
        }
       String CommandName
        {
            set;
            get;
        }

        #region UAT-3121
    
        Boolean IsApplicantPkgNotAssignedThroughCloning { get; set; }
   
        Boolean IsInstructorPkgNotAssignedThroughCloning { get; set; }
        #endregion

        #region UAT-3961
        List<AgencyHierarchyRootNodeSettingContract> TypeSpecialtyList
        {
            get;
            set;
        }
        #endregion
        
        //UAT-4150
        Boolean IsInstAvailabilityDefined { get; set; }


        #region UAT: 4395
         bool IsCloneRotationCheckbox { get; set; }
         Int32 CloneRotationId { get; set; }
        #endregion
        #region UAT-4323
        List<RotationMemberDetailContract> RotationMemberDetailList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the value for Rotation Start Date.
        /// </summary>
        DateTime? RotationStartDate
        {
            get;

            set;

        }

        /// <summary>
        /// Gets or Sets the value for Is Rotation Start.
        /// </summary>
        Boolean IsRotationStart
        {
            get;
            set;
        }
        #endregion
    }
}
