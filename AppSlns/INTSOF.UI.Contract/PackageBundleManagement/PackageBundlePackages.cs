using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.PackageBundleManagement
{
    [Serializable]
    public class PackageBundlePackages
    {
        /// <summary>
        /// 
        /// </summary>
        public Int32 PackageID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String PackageName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32 PackageNodeMappingID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String PackageTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCompliancePackage { get; set; }
    }
}
