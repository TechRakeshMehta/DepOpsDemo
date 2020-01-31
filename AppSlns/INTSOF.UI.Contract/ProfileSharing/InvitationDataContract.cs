using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ProfileSharing
{
    /// <summary>
    /// Contract used to display the data in the Invitation grids for the Applicant and Shared user.
    /// </summary>
    [Serializable]
    public class InvitationDataContract
    {
        public Int32 ID { get; set; }

        /// <summary>
        /// This is the Applicant name when this contract is used to display the shared user invitations (IsApplicantType will be False)
        /// and this is name of the Shared user, when used to display the applicant invitations (IsApplicantType will be True)
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// This is the Applicant Email address when this contract is used to display the shared user invitations (IsApplicantType will be False)
        /// and this is Email address of the Shared user, when used to display the applicant invitations (IsApplicantType will be True)
        /// </summary>
        public String EmailAddress { get; set; }
        public String Phone { get; set; }
        public Int32 TenantID { get; set; }
        public String TenantName { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? InvitationDate { get; set; }
        public DateTime? LastViewedDate { get; set; }
        public Int32? ViewsRemaining { get; set; }
        public Int32? ViewsTotal { get; set; }
        public Int32 InviteTypeID { get; set; }
        public String InviteTypeCode { get; set; }
        public String InviteTypeName { get; set; }
        public String Notes { get; set; }
        public Int32 ApplicantOrgUserID { get; set; }
        public Int32 TotalCount { get; set; }
        public Boolean IsInvitationVisible { get; set; }

        /// <summary>
        /// This will differentiate whether the contract is used to display the invitations in Applicant screen 
        /// or the Shared user screen.
        /// </summary>
        public Boolean IsApplicantType { get; set; }


        /// <summary>
        /// Represnts the Agency of the Shared User
        /// </summary>
        public String Agency { get; set; }

        /// <summary>
        /// Represents the lkpInvitationExpirationType Code 
        /// </summary>
        public String ExpirationTypeCode { get; set; }

        /// <summary>
        /// Decides whether to display the Expiration Date column
        /// </summary>
        public Boolean IsExpirationDateVisible { get; set; }

        /// <summary>
        /// Decides whether to display the Expiration Count column
        /// </summary>
        public Boolean IsExpirationCountVisible { get; set; }


        public String SharedUserInvitationReviewStatusName { get; set; }

        public String SharedUserInvitationReviewStatusCode { get; set; }

        /// <summary>
        /// Represnts the Agency name
        /// </summary>
        public String AgencyName { get; set; }

        //UAT 1882
        public Boolean IsIndividualShare { get; set; }
    }
}
