using System;
using System.Runtime.Serialization;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.Utils;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class ClinicalRotationSearchContract : SearchContract
    {
        [DataMember]
        public Int32? OrganizationUserId { get; set; }

        [DataMember]
        public Int32? FilterUserGroupID { get; set; }

        [DataMember]
        public Int32? MatchUserGroupID { get; set; }

        [DataMember]
        public Int32? LoggedInUserId { get; set; }

        [DataMember]
        public Int32? LoggedInUserTenantId { get; set; }

        [DataMember]
        public Int32? ClinicalRotationID { get; set; }

        [DataMember]
        public Int32? AgencyID { get; set; }

        [DataMember]
        public Boolean IsEditMode { get; set; }

        [DataMember]
        public CustomPagingArgsContract GridCustomPagingArguments { get; set; }

        [DataMember]
        public String SelectedDPMIds { get; set; }

        [DataMember]
        public String SelectedNodeLabels { get; set; }

        [DataMember]
        public Boolean IsRotationStart { get; set; }
        //UAT 3041
        [DataMember]
        public Boolean IsEditableByClientAdmin { get; set; }
        [DataMember]
        public Boolean IsEditableByAgencyUser { get; set; }
    }
}
