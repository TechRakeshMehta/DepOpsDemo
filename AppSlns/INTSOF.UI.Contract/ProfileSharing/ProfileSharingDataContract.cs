using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class ProfileSharingDataContract
    {
        public Int32 InvitationId
        {
            get;
            set;
        }

        public Int32 ApplicantUserID
        {
            get;
            set;
        }

        public String ApplicantName
        {
            get;
            set;
        }

        public String InviteeName
        {
            get;
            set;
        }

        public DateTime? InvitationDate
        {
            get;
            set;
        }

        public Boolean ViewedStatus
        {
            get;
            set;
        }

        public List<SharedPackages> lstSharedPackages
        {
            get;
            set;
        }

        public Boolean InvitationSentStatus
        {
            get;
            set;
        }
        public String ScheduledInvitationStatus
        {
            get;
            set;
        }
        public DateTime? EffectiveDate
        {
            get;
            set;
        }
        public String InviteeUserType { get; set; }
        public Int32? ViewsRemaining { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public String ExpirationType { get; set; }

        public Boolean IsDocumentUploaded { get; set; }

    }

    [Serializable]
    public class SharedPackages
    {
        public Guid PackageIdentifier
        {
            get;
            set;
        }

        public Int32? PackageId
        {
            get;
            set;
        }

        public String PackageName
        {
            get;
            set;
        }

        //public Boolean IsCompliancePackage
        //{
        //    get;
        //    set;
        //}
        public String PackageTypeCode
        {
            get;
            set;
        }
        public List<SharedEntity> lstSharedEntity { get; set; }

        public Int32? OrderID
        {
            get;
            set;
        }

        public String OrderNumber
        {
            get;
            set;
        }
    }

    [Serializable]
    public class SharedEntity
    {
        public String SharedEntityName
        {
            get;
            set;
        }

        public Int32? IsDocumentShared
        {
            get;
            set;
        }
    }
}
