using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class PackageDocumentDataPointContract
    {
        public Int32 BkgDataPointTypeID { get; set; }
        public String BkgDataPointTypeCode { get; set; }
        public Int32? SGID { get; set; }
        public Int32 MasterOrderID { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public String ServiceGroupName { get; set; }
        public Int32 PackageSubscriptionID { get; set; }
        public Int32 ArchiveStateID { get; set; }

        public Boolean IsScreeningDocAttributeMapped { get; set; }

        #region UAT-1254
        public Boolean IsMappingExist { get; set; }
        public Boolean IsOrderCompleted { get; set; }
        public Boolean IsSvcGroupCompleted { get; set; }
        #endregion

        #region UAT-2319
        public Int32 BkgCompliancePackageMappingID { get; set; }
        public Int32 ItemID { get; set; }
        #endregion
    }
}
