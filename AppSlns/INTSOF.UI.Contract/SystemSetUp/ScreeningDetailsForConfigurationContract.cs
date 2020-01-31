using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SystemSetUp
{
    [Serializable]
    public class ScreeningDetailsForConfigurationContract
    {
        public List<BackgroundPackageDetailsForConfigurationContract> BackgroundPackageDetailsList { get; set; }
        public List<ServiceFormDetailsForConfigurationContract> ServiceFormDetailsList { get; set; }
        public List<ServiceItemFeeDetailsForConfigurationContract> ServiceItemFeeDetailsList { get; set; }
    }

    [Serializable]
    public class BackgroundPackageDetailsForConfigurationContract
    {
        public Int32 HierarchyNodeID { get; set; }
        public String HierarchyLabel { get; set; }
        public String PackageName { get; set; }
        public Int32? PackageSvcGrpID { get; set; }
        public Int32? ServiceGroupID { get; set; }
        public Int32? PackageServiceID { get; set; }
        public Int32? ServiceID { get; set; }
        public String ServiceName { get; set; }
        public String ServiceGroupName { get; set; }
        public Int32? QuantityIncluded { get; set; }
        public Int32? PackageServiceItemID { get; set; }
        public String PackageServiceItemName { get; set; }
        public String AdditionalOccurencePriceType { get; set; }
        public Decimal? AdditionalOccurencePriceAmount { get; set; }
        public String QuantityGroup { get; set; }
        public String GlobalFeeTypeCode { get; set; }
        public Int32? PackageServiceItemFeeID { get; set; }        
        public String GlobalFeeName { get; set; }
    }

    [Serializable]
    public class ServiceFormDetailsForConfigurationContract
    {
        public Int32 ServiceID { get; set; }
        public Int32 ServiceAtachedFormID { get; set; }
        public String ServiceAtachedFormName { get; set; }
        public Int32? SystemDocumentID { get; set; }
        public String DocumentName { get; set; }
        public Boolean SendAutomatically { get; set; }
        public String DocumentPath { get; set; }
    }

    [Serializable]
    public class ServiceItemFeeDetailsForConfigurationContract
    {
        public Int32 PackageServiceID { get; set; }
        public Int32 PackageServiceItemID { get; set; }
        public Int32? PackageServiceItemFeeID { get; set; }    
        public String FeeItemType { get; set; }
        public String LocalFeeTypeCode { get; set; }
    }

}

