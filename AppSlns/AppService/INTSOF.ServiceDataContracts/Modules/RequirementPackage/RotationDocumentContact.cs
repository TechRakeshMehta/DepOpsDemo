using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RotationDocumentContact
    {
        [DataMember]
        public Int32 OrganizationUserID { get; set; }

        [DataMember]
        public String FirstName { get; set; }

        [DataMember]
        public String LastName { get; set; }

        [DataMember]
        public String ApplicantFullName { get; set; }

        [DataMember]
        public String ReqCatName { get; set; }

        [DataMember]
        public Int32 ApplicantDocumentID { get; set; }

        [DataMember]
        public String DocumentName { get; set; }

        [DataMember]
        public String DocumentPath { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public int ReqCatID { get; set; }
    }
}
