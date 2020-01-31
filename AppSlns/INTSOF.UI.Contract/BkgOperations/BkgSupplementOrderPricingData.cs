using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    /// <summary>
    /// Class to represent the Supplement Background order package data, extracted from the stored procedure
    /// </summary>
    public class SupplementOrderServices
    {
        // Removed now
        // public Int32 ServiceId { get; set; }

        /// <summary>
        /// Primarky key of the Order table i.e. OrderID
        /// </summary>
        public Int32 OrderId { get; set; }

        /// <summary>
        /// Line Items for the Service, from the Supplement order Pricing stored procedure
        /// </summary>
        public List<SupplementOrderOrderLineItem_PricingData> lstOrderLineItems { get; set; }
    }

    /// <summary>
    /// Class to represent the OrderLineItem level data from the Supplement order Pricing stored procedure
    /// </summary>
    public class SupplementOrderOrderLineItem_PricingData
    {
        /// <summary>
        /// Description Tag for the Price Item
        /// </summary>
        public String LineItemDescription { get; set; }
        public Int32 PackageSvcGrpID { get; set; }
        public Int32 PackageServiceItemId { get; set; }
        public Int32 PackageOrderItemPriceId { get; set; }

        public Decimal TotalPrice { get; set; }
        public Decimal Price { get; set; }
        public String PriceDescription { get; set; }

        public List<SupplementOrderFee_PricingData> lstFees { get; set; }
        public List<SupplementOrderBkgSvcAttributeDataGroup_PricingData> lstBkgSvcAttributeDataGroup { get; set; }
    }

    /// <summary>
    /// Class to represent the Fee related data from the Supplement order Pricing stored procedure
    /// </summary>
    public class SupplementOrderFee_PricingData
    {
        public Int32 PackageOrderItemFeeId { get; set; }
        public Decimal Amount { get; set; }
        public String Description { get; set; }
    }

    /// <summary>
    /// Class to represent the data from the Supplement order Pricing stored procedure
    /// </summary>
    public class SupplementOrderBkgSvcAttributeDataGroup_PricingData
    {
        /// <summary>
        /// AttributeGroupID tag in BkgSvcAttributeDataGroup pricing output XML
        /// </summary>
        public Int32 AttributeGroupId { get; set; }

        /// <summary>
        /// InstanceID tag in BkgSvcAttributeDataGroup pricing output XML
        /// </summary>
        public String InstanceId { get; set; }

        /// <summary>
        /// List of AttributeID's and their Data, from the pricing SP output XML.
        /// </summary>
        public List<SupplementOrderAttributeData_PricingData> lstAttributeData { get; set; }
    }

    /// <summary>
    /// Represents the AttributeID's and their values, from the Supplement Order pricing SP output XML.
    /// </summary>
    public class SupplementOrderAttributeData_PricingData
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
