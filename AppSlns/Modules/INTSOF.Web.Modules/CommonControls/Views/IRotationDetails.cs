using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.CommonControls.Views
{
    public interface IRotationDetails
    {
        /// <summary>
        /// Represents the ID of the Rotation for which details are to be fetched.
        /// </summary>
        Int32 ClinicalRotationId { get; set; }

        /// <summary>
        /// Id of the Tenant to which the Rotation belongs to.
        /// </summary>
        Int32 TenantId { get; set; }
        List<CustomAttribteContract> CustomAttributeList { get; set; }
        List<MultipleAdditionalDocumentsContract> MultipleAdditionalDocumentsContract { get; set; }
        ClinicalRotationDetailContract ClinicalRotationDetails { get; set; }
        IRotationDetails CurrentViewContext { get; }
        Boolean IsRestrictToLoadFresshData { get; set; }

        Boolean ShowInstitutionName { get; set; }

        ClientContactSyllabusDocumentContract SyllabusDocumentContract { get; set; }

        Boolean IsRotationNameVisibleOnHeader { get; set; }

        //UAT-2666
        Boolean IsSharedUser { get; }
        RotationFieldUpdatedByAgencyContract RotationFieldUpdaeByAgency { get; set; }
        Boolean HighlightRotationFieldUpdated { get; set; }
        #region UAT-2688
        Int32 AgencyID { get; set; }
        Boolean IsAgencyUser { get; set; }

        Int32 OrganisationUserID { get; }

        Int32 CurrentLoggedInUserId { get; }
        #endregion

        Boolean IsEditableByAgencyUser { get; set;} //UAT 3041

        ClientContactSyllabusDocumentContract ClinicalRotationSyllabusDocumentContract { get; set; } //UAT-3197

        Boolean IsInvSharedByApplicantByAgencyDDl { get; set; } //UAT-3387

        Int32 ProfileShareInvGroupId { get; set; } //UAT-3387

        Boolean HideRotationEditBtn { get; set; } //UAT-4479
    }
}
