using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    /// <summary>
    /// Class to represent the Package level data from the Pricing stored procedure
    /// </summary>
    public class Package_PricingData
    {
        public Int32 PackageId { get; set; }

        /// <summary>
        /// TotalPrice Tag value in output XML from Pricing XML
        /// </summary>
        public Decimal TotalBkgPackagePrice { get; set; }
        public List<OrderLineItem_PricingData> lstOrderLineItems { get; set; }
    }

    /// <summary>
    /// Class to represent the OrderLineItem level data from the Pricing stored procedure
    /// </summary>
    public class OrderLineItem_PricingData
    {
        public Int32 PackageSvcGrpID { get;set;}
        public Int32 PackageServiceId { get; set; }
        public Int32 PackageServiceItemId { get; set; }
        public String Description { get; set; }
        public Int32 PackageOrderItemPriceId { get; set; }
        public Decimal Price { get; set; }
        public String PriceDescription { get; set; }

        public List<Fee_PricingData> lstFees { get; set; }
        public List<BkgSvcAttributeDataGroup_PricingData> lstBkgSvcAttributeDataGroup { get; set; }

        public Boolean IsDummyLineItem { get; set; } //UAT-4498
    }

    /// <summary>
    /// Class to represent the Fee related data from the Pricing stored procedure
    /// </summary>
    public class Fee_PricingData
    {
        public Int32? PackageOrderItemFeeId { get; set; }
        public Decimal Amount { get; set; }
        public String Description { get; set; }
    }

    public class BkgSvcAttributeDataGroup_PricingData
    {
        /// <summary>
        /// AttributeGroupID tag in BkgSvcAttributeDataGroup pricing output XML
        /// </summary>
        public Int32 AttributeGroupId { get; set; }

        /// <summary>
        /// InstanceID tag in BkgSvcAttributeDataGroup pricing output XML
        /// </summary>
        public Int32 InstanceId { get; set; }

        /// <summary>
        /// List of AttributeID's and their Data, from the pricing SP output XML.
        /// </summary>
        public List<AttributeData_PricingData> lstAttributeData { get; set; }
    }

    /// <summary>
    /// Represents the AttributeID's and their values, from the pricing SP output XML.
    /// </summary>
    public class AttributeData_PricingData
    {
        /// <summary>
        /// PK of BkgAttributeGroupMapping table - AttributeGroupMappingID tag value in BkgSvcAttributeData tag, in pricing output XML
        /// </summary>
        public Int32 AttributeGroupMappingID { get; set; }

        /// <summary>
        /// Value tag value in BkgSvcAttributeData tag, in pricing output XML
        /// </summary>
        public String AttributeValue { get; set; }
    }
}
