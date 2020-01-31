using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    [Serializable]
    public class BkgCompliancePackageMappingSearchData
    {
        public Int32 BCPM_ID { get; set; }
        public String BPA_Name { get; set; }
        public String BSG_Name { get; set; }
        public String BSE_Name { get; set; }
        public String BDPT_Name { get; set; }
        public String BDPT_Code { get; set; }
        public String PackageName { get; set; }
        public String CategoryName { get; set; }
        public String ComplianceItemName { get; set; }
        public String ComplianceAttributeName { get; set; }
        public Int32 BCPM_BkgPackageID { get; set; }
        public Int32 BCPM_BkgDataPointTypeID { get; set; }
        public Int32 BCPM_BkgSvcGroupID { get; set; }
        public Int32 BCPM_BkgSvcID { get; set; }
        public Int32 BCPM_PackageSvcItemID { get; set; }
        public Int32 BCPM_CompliancePkgID { get; set; }
        public Int32 BCPM_ComplianceCategoryID { get; set; }
        public Int32 BCPM_ComplianceItemID { get; set; }
        public Int32 BCPM_ComplianceAttributeID { get; set; }
        public Int32 TotalCount { get; set; }
        public DateTime BCPM_CreatedOn { get; set; }
    }
}
