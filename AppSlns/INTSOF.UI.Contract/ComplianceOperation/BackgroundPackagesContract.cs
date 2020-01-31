using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    /// <summary>
    /// Class to represent the Background Packages related data
    /// including the BackgroundPackage & BkgPackageHierarchyMapping table data
    /// </summary>
    [Serializable]
    [DataContract]
    public class BackgroundPackagesContract
    {
        /// <summary>
        /// BPHM_ID - Pk of BkgPackageHierarchyMapping table
        /// </summary>
        [DataMember]
        public Int32 BPHMId { get; set; }


        /// <summary>
        /// BPA_ID - Pk of the BackgroundPackage Table
        /// </summary>
        [DataMember]
        public Int32 BPAId { get; set; }

        /// <summary>
        /// BPA_Name - Background Package Name 
        /// </summary>
        [DataMember]
        public String BPAName { get; set; }

        /// <summary>
        /// BPAViewDetails - Is View Details for the Background Package Enabled
        /// </summary>
        [DataMember]
        public Boolean BPAViewDetails { get; set; }

        /// <summary>
        /// Identify whether the Package is exclusive or not
        /// </summary>
        [DataMember]
        public Boolean IsExclusive { get; set; }

        /// <summary>
        /// BasePrice - Price from the BkgPackageHierarchyMapping table
        /// </summary>
        [DataMember]
        public Decimal BasePrice { get; set; }

        /// <summary>
        /// CustomPriceText - CustomPriceText from the BkgPackageHierarchyMapping table
        /// </summary>
        [DataMember]
        public String CustomPriceText { get; set; }

        /// <summary>
        /// BasePrice+ LineItem based Price from the Pricing calculation SP 
        /// </summary>
        [DataMember]
        public Decimal TotalBkgPackagePrice { get; set; }

        /// <summary>
        /// LineItem based Price from the Pricing calculation SP 
        /// </summary>
        [DataMember]
        public Decimal TotalLineItemPrice { get; set; }

        /// <summary>
        /// Represents the NodeLevel(or Dropdown number) selected from the Pending order screen
        /// </summary>
        [DataMember]
        public Int32 NodeLevel { get; set; }

        [DataMember]
        public Int32 MaxNumberOfYearforResidence { get; set; }

        [DataMember]
        public Int32 DisplayOrder { get; set; }

        //UAT-947:WB: Add ability to show custom details below each package name on package selection screen
        [DataMember]
        public String PackageDetail { get; set; }

        //UAT - 916
        [DataMember]
        public Boolean? IsInvoiceOnlyAtPackageLevel { get; set; }

        /// <summary>
        /// Identify whether the Package notes dispaly above
        /// </summary>
        [DataMember]
        public Boolean DisplayNotesAbove { get; set; }

        /// <summary>
        /// Identify whether the Package notes dispaly below
        /// </summary>
        [DataMember]
        public Boolean DisplayNotesBelow { get; set; }
        /// <summary>
        /// Represents the NodeLevel(or Dropdown number) selected from the Pending order screen
        /// </summary>
        [DataMember]
        public Int32 InsitutionHierarchyNodeID { get; set; }

        //UAT-3264
        ///<summary>
        /// Represents whether the background package is to qualify in rotation.
        ///</summary>
        [DataMember]
        public Boolean IsReqToQualifyInRotation { get; set; }

        [DataMember]
        public Decimal? AdditionalPrice { get; set; }

        [DataMember]
        public String PackageTypeCode { get; set; }

        [DataMember]
        public String PackagePasscode { get; set; }

        [DataMember]
        public Boolean IsInvoiceToInstitutionType { get; set; }

        [DataMember]
        public PackageServiceItemContract ListPackagePaymentContract { get; set; }
        [DataMember]
        public String ServiceCode { get; set; }

        [DataMember]
        public Int32 CopiesCount { get; set; }
        [DataMember]
        public Int32 PPCopiesCount { get; set; }        
        [DataMember]
        public Decimal? FCAdditionalPrice { get; set; }
        [DataMember]
        public Decimal? PPAdditionalPrice { get; set; }
        [DataMember]
        public bool IsOrderHistory { get; set; }
        [DataMember]
        public String ServiceDescription { get; set; }
        [DataMember]
        public Int32? ReferencedOrderID { get; set; }

        #region Admin Entry
        [DataMember]
        public String HierarchyNodeName { get; set; }

        #endregion

    }   

    [Serializable]
    [DataContract]
    public class PackageServiceItemContract
    {
        [DataMember]
        public Int32 PackageID { get; set; }

        [DataMember]
        public List<Int32> BackgroundServiceID { get; set; }
    }
}

