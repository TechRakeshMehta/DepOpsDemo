using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IAttestationReportsWithoutSignature
    {
        List<AttestationDocumentContract> LstInvitationDocumentContract { get; set; }

        List<ClinicalRotationDetailContract> LstAttestationDocumentsContract { get; set; }

        Int32 CurrentLoggedInUserID { get;}

        String ErrorMessage { get; set; }

        String SuccessMessage { get; set; }

        String StatusMessage { get; set; }
    }
}
