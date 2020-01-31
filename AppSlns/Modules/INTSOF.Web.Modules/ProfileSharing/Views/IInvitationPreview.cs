using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ProfileSharing;
using CoreWeb.IntsofSecurityModel;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IInvitationPreview
    {
        IInvitationPreview CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// Data contract to manage the CRUD operations
        /// </summary>
        InvitationDetailsContract InvitationData
        {
            get;
            set;
        }

        /// <summary>
        /// InvitationId of the invitation being previewed
        /// </summary>
        Int32 InvitationId
        {
            get;
            set;
        }

        /// <summary>
        /// TenantId
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Identify whether a new invitation was sent or existing is updated
        /// </summary>
        Boolean IsNewInvitation
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the Current Logged-in Applicant
        /// </summary>
        String ApplicantName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        String ProfileSharingUrl
        {
            get;
        }


        /// <summary>
        /// 
        /// </summary>
        String CentralLoginUrl
        {
            get;
        }

        /// <summary>
        /// Subject of the Email
        /// </summary>
        String EmailSubject
        {
            get;
            set;
        }

        /// <summary>
        /// Actual Email content
        /// </summary>
        String EmailContent
        {
            get;
            set;
        }

        /// <summary>
        /// HTML String of the Packages selected.
        /// </summary>
        String Packages
        {
            get;
            set;
        }

        /// <summary>
        /// HTML String of the Information shared
        /// </summary>
        String SharedInformation
        {
            get;
            set;
        }

        /// <summary>
        /// Data of the Logged in user, to be used in Template
        /// </summary>
        Entity.OrganizationUser UserData
        {
            get;
            set;
        }

        Boolean IsAgencySelected { get; set; }

        SysXMembershipUser CurrentUser { get; }

        List<RotationAndTrackingPkgStatusContract> LstErrorMessages { get; set; }
    }
}
