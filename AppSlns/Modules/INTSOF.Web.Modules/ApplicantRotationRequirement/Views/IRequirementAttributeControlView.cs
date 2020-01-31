using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public interface IRequirementAttributeControlView
    {
        Int32 ItemId { get; set; }
        Int32 TenantId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        IRequirementAttributeControlView CurrentViewContext { get; }
        RequirementFieldContract RequirementFieldContract { get; set; }
        ApplicantRequirementFieldDataContract ApplicantFieldData { get; set; }
        List<ApplicantDocumentContract> ApplicantDocuments { get; set; }
        List<RequirementObjectTreeContract> LstRequirementObjTreeProperty { get; set; }
    }
}
