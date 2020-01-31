using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SearchUI
{
    [Serializable]
    public class SharedUserSearchInvitationDetailsContract
    {
        public Int32? InvitationID
        {
            get;
            set;
        }

        public String TenantName
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

        public Int32? InvitationDocumentID
        {
            get;
            set;
        }

        public Int32? TenantID 
        { 
            get; 
            set; 
        }

        public String InvitationSourceCode
        {
            get;
            set;
        }
        /// <summary>
        /// UAT-1641 As an Agency User, I should be able to be linked to multiple agencies
        /// Agency name column in shared user search->Manage Search Invitation Details
        /// </summary>
        public String AgencyName
        {
            get;
            set;
        }
    }

    [Serializable]
    public class SharedPackages
    {
        public Guid PackageIdentifier
        {
            get;
            set;
        }

        public Int32? PackageID
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
        //UAT-1310
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

        public Int32? PackageSubscriptionID
        {
            get;
            set;
        }

        public Int32? SnapShotID { get; set; }

        public String FlagStatusImagePath { get; set; }

        public String ColorFlagPath { get; set; }

        public Boolean ShowFlagText { get; set; }
    }

    [Serializable]
    public class SharedEntity
    {
        public String SharedEntityName
        {
            get;
            set;
        }

        public Int32? SharedEntityID { get; set; }

        public Boolean IsResultReportVisible { get; set; }
    }
}
