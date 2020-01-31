using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IDocumentViewerView
    {
        Int32 TenantId { get; set; }
        Int32 ApplicantDocumentId { get; set; }
        Int32 ClientSysDocId { get; set; }
        Boolean IsApplicantDocument { get; set; }
        Int32 loggedinUserID { get; set; }
        String DocumentType { get; set; }

        Int32 WebsiteId { get; set; }
        String LoginImageUrl { get; set; }
        String RightLogoImageUrl { get; set; }
        Int32 SystemDocumentID
        {
            get;
            set;
        }
        Int32 OrderID
        {
            get;
            set;
        }
        Int32 ServiceGroupID
        {
            get;
            set;
        }
        String ReportType
        {
            get;
            set;
        }
        String ReportName
        {
            get;
            set;
        }

        Int32 ApplicantID
        {
            get;
            set;
        }



        String FromDate
        {
            get;
            set;
        }

        String ToDate
        {
            get;
            set;
        }

        Boolean IsFromReport
        {
            get;
            set;
        }

        String UserIdForReport
        {
            get;
            set;
        }
        String Institute
        {
            get;
            set;
        }
        String Hierarchy
        {
            get;
            set;
        }

        Int32 HierarchyNodeID
        {
            get;
            set;
        }
        Boolean IsReportSentToStudent
        {
            get;
            set;
        }
        Int32 OrganizationUserID
        {
            get;
            set;
        }

        #region Profile Sharing Attestation Report
        Int32 InvitationId
        {
            get;
            set;
        }
        #endregion

        Int32 UserAttestationDocumentID
        {
            get;
            set;
        }

        String DocumentPath
        {
            get;
            set;
        }

        String UAFDocumentStage
        {
            get;
            set;
        }

        Int32 InvitationDocumentID
        {
            get;
            set;
        }

        Int32 ProfileSharingInvitationID
        {
            get;
            set;
        }

        String AttestationTypeCode { get; set; }

        #region UAT-2774
        Int32 ProfileSharingInvitationGroupId { get; set; }
        #endregion

        #region UAT-4592
        Int32 DisclaimerDocumentSystemDocumentID
        {
            get;
            set;
        }
        #endregion
    }
}




