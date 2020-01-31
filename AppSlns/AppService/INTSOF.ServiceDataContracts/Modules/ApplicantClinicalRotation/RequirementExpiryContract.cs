using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation
{
    public class RequirementExpiryContract
    {
        public Int32 RequirementSubId { get; set; }
        public Int32 RequirementPkgId { get; set; }
        public Int32 RequirementCatId { get; set; }
        public Int32 RequirementItemId { get; set; }
        public Int32 RequirementItemDataId { get; set; }
        public String RequirementPkgSubStatusCode { get; set; }
        public Boolean IsNewPackage { get; set; }
    }
}
