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
    public class RequirementAttributeGroupContract
    {
        [DataMember]
        public Int32 RequirementAttributeGroupID { get; set; }

        [DataMember]
        public Guid Code { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String Label { get; set; }

        [DataMember]
        public Boolean IsCreatedByAdmin { get; set; }

        [DataMember]
        public Guid? CopiedFromCode { get; set; }

        [DataMember]
        public Boolean IsDeleted { get; set; }

        [DataMember]
        public Int32 CreatedByID { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }


        [DataMember]
        public Int32? ModifiedByID { get; set; }

        [DataMember]
        public DateTime? ModifiedOn { get; set; }

    }
}
