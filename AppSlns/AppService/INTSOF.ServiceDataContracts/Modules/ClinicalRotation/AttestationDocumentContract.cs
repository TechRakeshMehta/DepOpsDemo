using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    public class AttestationDocumentContract
    {
        public Int32 ProfileSharingInvitationID { get; set; }

        public Int32 ProfileSharingInvitationGroupID { get; set; }

        public Int32 InvitationDocumentID { get; set; }

        public Int32 InvitationDocumentMappingID { get; set; }

        public String DocumentFilePath { get; set; }

        public Boolean IsVerticalAttestation { get; set; }

        public String SharedSystemDocumentTypecode { get; set; }

        public String SharedInfoTypeCode { get; set; }  //UAT-2443

        public String MasterInfoTypeCode { get; set; } //UAT-2443

    }
}

