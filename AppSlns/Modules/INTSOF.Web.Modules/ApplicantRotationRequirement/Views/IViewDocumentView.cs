using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public interface IViewDocumentView
    {
        /// <summary>
        /// ClientSectionId to fetch document URL
        /// </summary>
        Int32 ClientSysDocId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Int32 ReqFieldId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Int32 TenantId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Int32 ReqObjectTreeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Int32 ApplicantDocId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IViewDocumentView CurrentViewContext { get; }
        /// <summary>
        /// 
        /// </summary>
        ApplicantDocumentContract ClientSystemDocContract { get; set; }
        /// <summary>
        /// 
        /// </summary>
        ObjectAttributeContract objectAttributeContract { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ViewDocumentContract ViewDocContract { get; set; }

        Int32 OrganizationUserID { get; set; }

        Entity.OrganizationUser OrganizationUserData { get; set; }

        AddressContract Addresses { get; set; }

    }
}
