using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    /// <summary>
    /// Contract used to display the data in the Invitation grids for the Applicant and Shared user.
    /// </summary>
    [Serializable]
    [DataContract]
    public class RotationAndTrackingPkgStatusContract
    {
        [DataMember]
        public String ErrorMessage { get; set; }
        [DataMember]
        public String PackageType { get; set; }
    }
}
