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
    public class ViewDocumentContract
    {
        [DataMember]
        public string DocumentPath { get; set; }
        [DataMember]
        public string DocumentName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public bool IsSignatureRequired { get; set; }
        [DataMember]
        public bool IsRequiredToView { get; set; }
        [DataMember]
        public bool IsApplicantDoc { get; set; }
      
    }
}
