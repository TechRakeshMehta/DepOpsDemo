using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public interface IRequirementRotation
    {
        /// <summary>
        ///  Tenant-Id of the logged-in Applicant
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// List of the Agencies that belong to the Current Applicants' Tenant
        /// </summary>
        List<AgencyDetailContract> lstAgency
        {
            get;
            set;
        }

        /// <summary>
        /// List of the ClientContacts that belong to the Current Applicants' Tenant
        /// </summary>
        List<ClientContactContract> lstClientContacts
        {
            get;
            set;
        }

        /// <summary>
        /// ID of the Agency selected for Search
        /// </summary>
        Int32 SelectedAgencyId
        {
            get;
            set;
        }

        /// <summary>
        /// ID of the ClientContact, selected for Search
        /// </summary>
        String ClientContactId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the Current View Context
        /// </summary>
        IRequirementRotation CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        String StatusTypeCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ClinicalRotationDetailContract SearchContract
        {
            get;
        }

        /// <summary>
        /// Represents the list of Rotations oto which the applicant belongs to.
        /// </summary>
        List<ClinicalRotationDetailContract> lstApplicantRotations
        {
            get;
            set;
        }
    }
}
