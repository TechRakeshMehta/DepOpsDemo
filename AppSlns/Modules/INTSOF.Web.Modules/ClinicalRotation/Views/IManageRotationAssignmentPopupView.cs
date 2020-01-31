using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IManageRotationAssignmentPopupView
    {
        Int32 CurrentLoggedInUserID { get; }
        Int32 TenantId { get; set; }
        Int32 AgencyID { get; set; }
        String RotationIDs { get; set; }
        String RotationAssignmentTypeCode { get; set; }
        List<InstructorAvailabilityContract> lstInstructorAvailabilityContracts { get; set; }
        List<ClientContactContract> ClientContactList
        {
            set;
        }

        List<RequirementPackageContract> lstTenantRequirementPackage
        {
            get;
            set;
        }

        List<RequirementPackageContract> lstSharedRequirementPackage
        {
            get;
            set;
        }

        List<RequirementPackageContract> lstCombinedRequirementPackage
        {
            set;
        }

        List<RequirementPackageContract> lstSharedInstructorRequirementPackages
        {
            get;
            set;
        }


        List<RequirementPackageContract> lstCombinedInstructorRequirementPackages
        {
            set;
        }
        List<RequirementPackageContract> lstInstructorRequirementPackage
        {
            get;
            set;
        }

        Int32 SelectedInstructionPackageID
        {
            get;
        }
        Int32 SelectedRequirementPackageID { get; }

        //// <summary>
        ///// Gets the view contract.
        ///// </summary>
        ///// <remarks></remarks>
        ClinicalRotationDetailContract RotationDataContarct
        {
            get;
            set;
        }

        #region Check Rot. Eff. Start Date & EndDate
        List<Int32> UnMappedRotationIDList { get; set; }
        List<String> UnMappedRotationNameList { get; set; }
        #endregion

        String RotationAgencyIDs { get; set; }

        String ComplioIDs { get; set; }
      
    }
}
