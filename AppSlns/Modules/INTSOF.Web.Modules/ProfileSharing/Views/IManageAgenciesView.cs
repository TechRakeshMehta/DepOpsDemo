#region Namespaces

#region SystemDefined
using INTSOF.Utils;
using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.UI.Contract.ProfileSharing;
using Entity.SharedDataEntity;

#endregion

#endregion

namespace CoreWeb.ProfileSharing.Views
{
    public interface IManageAgenciesView
    {
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

        /// <summary>
        /// Id of the Current Loggedin User
        /// </summary>
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            set;
            get;
        }

        /// <summary>
        /// Get list of agencies.
        /// </summary>
        List<AgencyContract> ListAgencies
        {
            get;
            set;
        }

        AgencyContract AgencyData { get; set; }

        List<Tenant> lstTenant { get; set; }

        //String Name { get; set; }

        //String Description { get; set; }

        //String Address { get; set; }

        List<Int32> lstTenantID { get; set; }

        //Int32 AG_ID { get; set; }

        List<AgencyInstitution> lstAgencyInstitutions { get; set; }

        List<Int32> PrevSelectedTenantIDs { get; set; }

        List<Int32> CurrentSelectedTenantIDs { get; set; }

        List<Int32> TenantIDsToDisable { get; set; }

        //String NpiNumber { get; set; }

        AgencyInstitution agencyInstitution { get; set; }

        Int32 InstitutionID { get; set; }

        Int32 AgencyID { get; set; }

        String OldNPINumber { get; set; }

        List<AgencyHierarchyContract> LstAgencyHierarchy { get; set; }

        bool IsAgencyAssociateWithInstitution { get; set; }

        Int32 AgencyPermissionAccessTypeId { get; set; }

        Int32 AgencyPermissionTypeId { get; set; }

        Dictionary<Int32, Int32> DicAgencyPermissions { get; set; }

        //UAT-2181:Enhance adding tenants to agencies with check boxes on the Manage Agencies screen to select which agencies you would like to add the selected tenant to
        List<Int32> lstSelectedAgencyIDs { get; set; }

        //UAT-2632:
        DeptProgramMapping ClientAdminRootNode { get; set; }
        AgencyHierarchyAgencyProfileSharePermission AgHierarchyProfilePermission { get; set; }

        //UAT-2821 : Agency formatted attestations (uploaded document rather than our attestation)
        AgencyAttestationDetailContract AttestationPermissionForAgency { get; set; }

        string FilterValue { get; set; }
        System.Web.UI.Pair FilterInformation { get; set; }

        String LstSelectedAgencyIDs { get; set; }

    }
}
