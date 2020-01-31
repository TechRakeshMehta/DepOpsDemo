using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class ClinicalRotationDetailContract
    {
        //SharedUserInvitationReviewID,SharedUserInvitationReviewStatusName
        #region UAT-2316
        //Add two new contracts acc. to UAT-2316. Previously student status is showing according to rotation status nu now indivual status need to be displayed
        //UAT-2316: Un- Merge Cells in Rotation Student
        [DataMember]
        public Int32 SharedUserInvitationReviewID { get; set; }//RotationReviewID
        [DataMember]
        public String SharedUserInvitationReviewStatusName { get; set; }//RotationReviewStatusName
        #endregion

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 RotationID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 AgencyID { get; set; }

        [DataMember]
        public String AgencyIDs { get; set; }

        /// <summary>
        /// This property will be NULL for every user except client admins
        /// </summary>
        [DataMember]
        public Int32? CurrentLoggedInClientUserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String AgencyName { get; set; }


        [DataMember]
        public String AgencyNameSpltdWithBreak { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Department { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Program { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Course { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String UnitFloorLoc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String RotationName { get; set; }

        /// <summary>
        /// UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
        /// </summary>
        [DataMember]
        public float? Students { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public float? RecommendedHours { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 Days { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String DaysIdList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String DaysName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public TimeSpan? EndTime { get; set; }

        [DataMember]
        public String RotationStartTime { get; set; }

        [DataMember]
        public String RotationEndTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Shift { get; set; }
        [DataMember]
        public DateTime? DroppedDate { get; set; } //UAT-4460

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ContactNames { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ContactIdList { get; set; }

        /// <summary>
        /// Tenant Id List
        /// </summary>
        [DataMember]
        public String TenantIdList { get; set; }

        /// <summary>
        /// TenantID
        /// </summary>
        [DataMember]
        public Int32 TenantID { get; set; }

        /// <summary>
        /// TenantName
        /// </summary>
        [DataMember]
        public String TenantName { get; set; }

        /// <summary>
        /// Tenant Detail List
        /// </summary>
        [DataMember]
        public List<TenantDetailContract> TenantDetailList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ComplioID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<ClientContactContract> ContactsToSendEmail { get; set; }

        [DataMember]
        public String SyllabusFileName { get; set; }

        [DataMember]
        public String SyllabusFilePath { get; set; }

        [DataMember]
        public Int32? SyllabusFileSize { get; set; }

        [DataMember]
        public Boolean IfSyllabusFileRemoved { get; set; }
        [DataMember]
        public Int32 TotalRecordCount { get; set; }

        #region 4062
        [DataMember]
        public string ClinicalRotationDocumentUpdatedIds { get; set; }

        [DataMember]
        public List<MultipleAdditionalDocumentsContract> listOfMultipleDocument { get; set; }


        #endregion

        /// <summary>
        /// Used to Perform Search in the Applicant Rotation Listing
        /// </summary>
        [DataMember]
        public String StatusTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32? PkgSubscriptionId { get; set; }

        [DataMember]
        public Int32 ProfileSharingInvGroupID { get; set; }

        [DataMember]
        public String Term { get; set; }

        /// <summary>
        /// Client Contact ID
        /// </summary>
        [DataMember]
        public Int32 ClientContactID { get; set; }

        /// <summary>
        /// This property should be true if instructor/Preceptor packages are included in Clinical rotation.
        /// </summary>
        [DataMember]
        public Boolean IsInstructorPreceptorPkgAvailable { get; set; }

        [DataMember]
        public Boolean IsProfileShared { get; set; }

        [DataMember]
        public Boolean IsSearchClicked { get; set; }

        [DataMember]
        public String ReviewStatusIDs { get; set; }

        [DataMember]
        public Int32 RotationReviewID { get; set; }

        [DataMember]
        public String RotationReviewStatusName { get; set; }

        //UAT-3165
        [DataMember]
        public Boolean IsCustomFilterApplied { get; set; }

        //UAT-3977
        [DataMember]
        public Boolean IsInstructorShare { get; set; }

        // UAT 1414 notification to go out prior to student's start date for clinical rotation
        [DataMember]
        public Int32? DaysBefore { get; set; }

        [DataMember]
        public String Frequency { get; set; }

        [DataMember]
        public String TypeSpecialty { get; set; }


        [DataMember]
        public string ApplicantName { get; set; }

        [DataMember]
        public Boolean IsInstructor { get; set; }

        [DataMember]
        public String RequirementPackageStatusDesc { get; set; }

        [DataMember]
        public String RequirementPackageStatusCode { get; set; }

        [DataMember]
        public String HierarchyNodes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String AgencyIdList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String HierarchyNodeIDList { get; set; }
        //UAT-1778
        [DataMember]
        public String CustomAttributes { get; set; }

        //UAT-3977
        [DataMember]
        public String RequirementPackageTypeCode { get; set; }

        [DataMember]
        public String ReviewStatus { get; set; }

        #region UAT 1701: New Agency User Search
        [DataMember]
        public String FirstName { get; set; }

        [DataMember]
        public String LastName { get; set; }

        //Property used for handling Coloring of rows in Grid.
        [DataMember]
        public String unit { get; set; }

        [DataMember]
        public String EnterData { get; set; }

        [DataMember]
        public String ViewAttestation { get; set; }

        [DataMember]
        public String ViewDetail { get; set; }

        #endregion
        public String XML
        {
            get
            {
                return CreateXml();
            }
        }

        private String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(ClinicalRotationDetailContract));
            var sb = new StringBuilder();
            ClinicalRotationDetailContract xmlData = this;

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }
            return sb.ToString();
        }

        [DataMember]
        public DateTime InvitationDate { get; set; }

        [DataMember]
        public String IpReqPkgStatus { get; set; }

        [DataMember]
        public string RequirementPackageName { get; set; }
        [DataMember]
        public string RequirementPackageStatus { get; set; }
        [DataMember]
        public int RequirementPackageID { get; set; }
        [DataMember]
        public int ReqPkgSubsID { get; set; }

        [DataMember]
        public String ApplicantRequirementPkgStatus { get; set; }

        //UAT-2289
        [DataMember]
        public DateTime? DeadlineDate { get; set; }

        //UAT-2289
        [DataMember]
        public String IsComplianceRequiredforRotation { get; set; }

        //UAT-2424 for cloning purpose 
        [DataMember]
        public Int32 InstructorPreceptorPkgID { get; set; }

        //UAT-2424 To check if it is a Cloning rotation in repo
        [DataMember]
        public Boolean IsCloningRotation { get; set; }

        //UAT-2424 To check if we need to associate the packages or not 
        [DataMember]
        public Boolean IsAgencyUpdated { get; set; }

        [DataMember]
        public String ArchieveStatusId { get; set; }

        [DataMember]
        public String AgencyHierarchyID { get; set; }

        [DataMember]
        public Int32 RootNodeID { get; set; }

        [DataMember]
        public string RotationReviewStatusIdList
        { get; set; }

        [DataMember]
        public String AgencyHierarchyIDs { get; set; }
        //UAT-2905
        [DataMember]
        public Boolean IsAllowNotification { get; set; }
        [DataMember]
        public Int32 CreatedByID { get; set; }
        [DataMember]
        public Boolean IsPDF { get; set; }

        //UAT-2979
        [DataMember]
        public String DPMIds { get; set; }
        public String InstituteHierarchySelectedNode { get; set; }

        //UAT 3041
        [DataMember]
        public Boolean IsEditableByClientAdmin { get; set; }
        [DataMember]
        public Boolean IsEditableByAgencyUser { get; set; }

        //UAT-3211 Tab updates(Advanced Search)
        [DataMember]
        public Boolean IsAdvanceSearchPanelDisplay { get; set; }


        /// <summary>
        /// UAT-3490
        /// </summary>
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public String SelectedRotationInvitationArchiveStateCode { get; set; } //UAT-3470
        //UAT-3977
        [DataMember]
        public String IsComplianceRequiredforInstructorPreceptorRotationPkgs { get; set; }
        [DataMember]
        public List<String> InviteeTypeCode { get; set; }

        [DataMember]
        public Int32? AgnecyHierarchyRootNodeSettingMappingID { get; set; }
        [DataMember]
        public String AgnecyHierarchyRootNodeSettingMappingValue { get; set; }
        [DataMember]
        public String SchoolRepresentativeName { get; set; }


        [DataMember]
        public String InstructorName { get; set; }

        //UAT-4150
        [DataMember]
        public Boolean IsSchoolSendingInstructor { get; set; }

        //UAT-4323
        [DataMember]
        public Int32 ApplicantCount { get; set; }

        //UAT-4499
        [DataMember]
        public String CreatedBy { get; set; }
        //UAT-4395
        [DataMember]
        public Boolean IsCloneRotationStudentCheck { get; set; }

        //UAT-4395
        [DataMember]
        public Int32 CloneRotationId { get; set; }


    }
}
