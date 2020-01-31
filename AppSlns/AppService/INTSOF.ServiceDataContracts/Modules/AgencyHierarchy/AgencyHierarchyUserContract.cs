using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.AgencyHierarchy
{
    [Serializable]
    [DataContract]
    public class AgencyHierarchyUserContract
    {
        [DataMember]
        public Int32 AgencyHierarchyUserID { get; set; } //Primary Key
        [DataMember]
        public Int32 AgencyHierarchyID { get; set; }
        [DataMember]
        public Int32 CurrentLoggedInUser { get; set; }
        [DataMember]
        public Int32 AGU_ID { get; set; }
        [DataMember]
        public String AGU_Name { get; set; }
        [DataMember]
        public Int32? AGU_ComplianceSharedInfoTypeID { get; set; }
        [DataMember]
        public Int32? AGU_BkgSharedInfoTypeID { get; set; }
        [DataMember]
        public String AGU_BkgSharedInfoTypeName { get; set; }
        [DataMember]
        public String SharedInfoTypeName { get; set; }
        [DataMember]
        public Int32 AGU_ReqRotationSharedInfoTypeID { get; set; }
        [DataMember]
        public String AGU_ReqRotationSharedInfoTypeName { get; set; }
        [DataMember]
        public Boolean AGU_RotationPackagePermission { get; set; }
        [DataMember]
        public Boolean AGU_AgencyUserPermission { get; set; }

        //Code commented for UAT-2803
        //[DataMember]
        //public Boolean IsEmailNeedToSend { get; set; }

        [DataMember]
        public String AGU_ComplianceSharedInfoTypeName { get; set; }
        [DataMember]
        public List<Int32> LstAGU_AgencyID { get; set; }
        [DataMember]
        public List<Int32> lstApplicationInvitationMetaDataID { get; set; }
        [DataMember]
        public List<Int32> lstInvitationSharedInfoTypeID { get; set; }
        [DataMember]
        public Boolean AttestationRptPermission { get; set; }
        [DataMember]
        public Boolean SSN_Permission { get; set; }
        [DataMember]
        public Boolean HideAgencyPortalDetailLink { get; set; } // UAT-3220
        [DataMember]
        public Boolean IsManageAttestationPermission { get; set; }
        [DataMember]
        public Boolean IsUpdateFlag { get; set; }
        //UAT-2706
        [DataMember]
        public Boolean AGU_RotationPackageViewPermission { get; set; }
        //UAT-2427
        [DataMember]
        public Boolean AGU_AllowJobPosting { get; set; }

        [DataMember]
        public Boolean AGU_DoNotShowNonAgencyShares { get; set; }

        #region UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
        [DataMember]
        public Boolean IsRequirementSharingNonRotationNotification { get; set; }

        [DataMember]
        public Boolean IsRequirementSharingRotationNotification { get; set; }

        [DataMember]
        public Boolean IsRotationInvitationApprovalRejectionNotification { get; set; }

        [DataMember]
        public Boolean IsIndividualProfileSharingWithEmailNotification { get; set; }

        #region UAT-2942
        [DataMember]
        public Boolean IsProfileSharingWithEmailNotification { get; set; }
        #endregion

        [DataMember]
        public Dictionary<Int32, Boolean> dicNotificationData { get; set; }

        #endregion

        [DataMember]
        public Boolean SendOutOfComplianceNotification { get; set; }

        [DataMember]
        public Boolean SendUpdatedApplicantRequirementNotification { get; set; }

        //UAT-3108
        [DataMember]
        public Boolean SendUpdatedRotationDetailsNotification { get; set; }

        //UAT-3222
        [DataMember]
        public Boolean SendStudentDroppedFromRotationNotification { get; set; }

        [DataMember] //uat-3316
        public Int32 AGU_TemplateId { get; set; }

        [DataMember]
        public List<Int32> lstCheckedReportsTypeID { get; set; }

        ////UAT-3998
        [DataMember]
        public Boolean SendItSystemAccessFormNotification { get; set; }

        #region UAT-4561
        [DataMember]
        public Boolean SendRotationEndDateChangeNotification { get; set; }
        #endregion
    }
}
