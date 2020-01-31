using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IInstructorPreceptorRotationPackageSearchView
    {
        List<ClinicalRotationDetailContract> ClinicalRotationData
        {
            get;
            set;
        }

        Int32 TenantID
        {
            get;
            set;
        }
        List<TenantDetailContract> lstTenants  //UAT-3596
        {
            get;
            set;
        }
        List<TenantDetailContract> lstSelectedTenants //UAT-3596
        {
            get;
        }
        List<Int32> lstSelectedTenantIDs //UAT-3596
        {
            get;
            set;
        }
        List<Int32> lstSelectedAgencyIDs 
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

        ClinicalRotationDetailContract SearchContract
        {
            get;
            set;
        }

        List<String> SharedUserTypeCodes { get; }

        String UserID { get; }

        List<AttestationDocumentContract> LstInvitationDocumentContract { get; set; }

        Boolean IsInstructor { get; }

       //List<AgencyDetailContract> lstAgency { set; }

        List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract> lstAgencyHierarchyRootNodes { set; }
        List<Int32> lstSelectedAgencyHierarchyIDs
        {
            get;
            set;
        }

    }
}
