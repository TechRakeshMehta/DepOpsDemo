using System;

namespace DataMart.UI.Contracts
{
    public class ItemDataCountReportContract
    {
        public String PackageType { get; set; }
        public String TenantName { get; set; }
        public String ItemName { get; set; }

        public Int32 ItemCount { get; set; }
    }
}
