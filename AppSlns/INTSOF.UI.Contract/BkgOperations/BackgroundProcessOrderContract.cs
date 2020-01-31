using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.BkgOperations
{

    public class BackroundOrderServiceLinePrice
    {
        public Int32 BackgroundServiceID { get; set; }
        public String BackgroundServiceName { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal? AdjAmount { get; set; }
        public Decimal NetAmount { get; set; }
        public String Description { get; set; }
    }
    public class ExternalVendorServiceContract
    {
        public Int32 BOR_ID { get; set; }
        public Int32 PSLI_ID { get; set; }
        public String BSE_Name { get; set; }
        public String VendorStatus { get; set; }
        public Boolean Flagged { get; set; }
        public String FlaggedText { get; set; }
        public String EVOD_VendorProfileID { get; set; }
        public String VendorCode { get; set; }
        public String VendorOrderID { get; set; }
    }

    public class OrderLineDetailsContract
    {
        public BkgOrderLineItemResultCopy BkgOrderLineItemResultCopy { get; set; }
        public ExtVendorBkgOrderLineItemDetail ExtVendorBkgOrderLineItemDetails { get; set; }
    }


    public class BkgOrderPackageInfo
    {
        public String BkgPackageName { get; set; }
        public Decimal? BkgPackagePrice { get; set; }
        //UAT-3481
        public String BkgPackageLabel { get; set; }
    }


    public class OrderServiceLineItemPriceInfo
    {
        public List<BkgOrderPackageInfo> BkgOrderPkg { get; set; }
        public List<BackroundOrderServiceLinePrice> BOSLPrice { get; set; }
        public String CompliancePackageName { get; set; }
        public Decimal? CompliancePkgAmount { get; set; }
        public Decimal? CompliancePkgTotalAmount { get; set; }



    }
}
