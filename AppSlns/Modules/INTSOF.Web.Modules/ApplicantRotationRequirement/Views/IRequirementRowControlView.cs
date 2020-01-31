using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public interface IRequirementRowControlView
    {
        /// <summary>
        /// Id of the Master compliance Item (Client database), present in the ApplicantComplianceItem Entity. 
        /// </summary>
        Int32 ItemId { get; set; }

        Int32 TenantId { get; set; }
        Int32 NoOfFieldsPerRow { get; set; }
        IRequirementRowControlView CurrentViewContext { get; }
        Int32 ApplicantRequirementItemId { get; set; }
        List<ApplicantRequirementFieldDataContract> ApplicantRequirementFieldData { get; set; }
        List<RequirementFieldContract> RequirementItemFields { get; set; }
        //Implemented code for UAT-708
        Int32 CategoryId
        {
            get;
            set;
        }

        List<RequirementObjectTreeContract> LstRequirementObjTreeProperty { get; set; }
    }
}
