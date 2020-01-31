using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class OrderPackageSelection
    {
        public String PackageName { get; set; }

        /// <summary>
        /// Represents the DPPId
        /// </summary>
        public Int32 PackageMappingId { get; set; }

        public Int32 PackageId { get; set; }

        public Int32 BundleId { get; set; }

        /// <summary>
        /// Either Compliance or Background
        /// </summary>
        public String MasterPackageTypeCode { get; set; }

        /// <summary>
        /// Either Immunization or Administrative
        /// </summary>
        public String CompliancePackageTypeCode { get; set; }

        /// <summary>
        /// Details of the Package
        /// </summary>
        public String PackageDetail { get; set; }

        public Boolean IsViewDetailsVisible { get; set; }

        #region Properties Only for Compliance Packages

        /// <summary>
        /// ID of the Node to which the CompliancePackages belong to. 
        /// Used to Check IfInvoice Payment Option type only.
        /// </summary>
        public Int32 DPMId { get; set; }

        #endregion

        #region Properties Only for Background Packages

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsInvoiceOnlyAtPackageLevel { get; set; }

        /// <summary>
        /// Only for Screening packages
        /// </summary>
        public String CustomPriceText { get; set; }

        /// <summary>
        /// Only for Screening packages
        /// </summary>
        public Boolean IsExclusiveBkgPackage { get; set; }

        public Decimal BkgPackageBasePrice { get; set; }

        /// <summary>
        /// Applicable only for Screening packages
        /// </summary>
        public Int32 MaxNumberOfYearforResidence { get; set; }

        /// <summary>
        /// Applicable only for Screening packages
        /// </summary>
        public Int32 BPHMId { get; set; }

        #endregion

        public Boolean DisplayNotesAbove { get; set; }

        public Boolean DisplayNotesBelow { get; set; }

        public Boolean IsReqToQualifyInRotation { get; set; }
        public Decimal? AdditionalPrice { get; set; }

    }

    /// <summary>
    /// Represents the Bundle Level Data
    /// </summary>
    public class BundleData
    {
        public Int32 BundleId { get; set; }

        public String BundleName { get; set; }
        public Boolean IsExclusive { get; set; }
        public List<OrderPackageSelection> lstCompliancePkgs { get; set; }

        public List<OrderPackageSelection> lstBkgPkgs { get; set; }

        public String ExplanatoryNotes { get; set; }
    }
}
