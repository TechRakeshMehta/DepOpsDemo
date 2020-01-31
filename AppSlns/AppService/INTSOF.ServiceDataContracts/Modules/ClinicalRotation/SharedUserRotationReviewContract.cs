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
    public class SharedUserRotationReviewContract
    {
        [DataMember]
        public Int32 SharedUserRotationReviewID { get; set; }
        [DataMember]
        public Int32 ClinicalRotaionID { get; set; }
        [DataMember]
        public Int32 RotationReviewStatusID { get; set; }
        [DataMember]
        public Int32 OrganizationUserID { get; set; }
        [DataMember]
        public Int32 IsDeleted { get; set; }
        [DataMember]
        public Int32 CreatedOn { get; set; }
        [DataMember]
        public Int32 CreatedByID { get; set; }
        [DataMember]
        public Int32 ModifiedOn { get; set; }
        [DataMember]
        public Int32 ModifiedByID { get; set; }
        [DataMember]
        public Int32 TenantID { get; set; }
        [DataMember]
        public Boolean Checked { get; set; }

    }
}
