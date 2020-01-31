using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class RotationDetailFieldChanges
    {

        /// <summary>
        /// TenantID
        /// </summary>
        [DataMember]
        public Int32 TenantID { get; set; }

        /// <summary>
        /// ClinicalRotationId 
        /// </summary>
        [DataMember]
        public Int32 RotationID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ComplioID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String RotationFieldChanges { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 CurrentLoggedInUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean NeedToSendEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ModifiedByName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String TenantName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean IsClinicalRotationUpdatedSuccessfully { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String HierarchyNodeIDs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String PrimaryEmailAddress { get; set; }

        //UAT-4561
        [DataMember]
        public Boolean IsNeedToSendEndDateMail { get; set; }

        //UAT-4428
        [DataMember]
        public Boolean IsStartDateUpdated { get; set; }
    }


    public class RotationStudentDropped
    {
        /// <summary>
        /// TenantID
        /// </summary>
        [DataMember]
        public Int32 TenantID { get; set; }
        [DataMember]
        public Int32 InviteeOrgId { get; set; }

        /// <summary>
        /// ClinicalRotationId 
        /// </summary>
        [DataMember]
        public Int32 RotationID { get; set; }
        [DataMember]
        public Int32? AgencyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ComplioID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String TenantName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String HierarchyNodeIDs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String RemovedApplicantNames { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean IsRemovedApplicantsFromRotation { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 CurrentLoggedInUserId { get; set; }

    }
}
