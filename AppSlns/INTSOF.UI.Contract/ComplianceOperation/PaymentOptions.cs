using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    /// <summary>
    /// Contract to Store the Pacakge level and Node level Payment Options
    /// </summary>
    [Serializable]
    [DataContract]
    public class PkgList
    {
        [DataMember]
        public Int32 PkgId { get; set; }
        [DataMember]
        public String PkgName { get; set; }

        /// <summary>
        /// Will be DPPId for Compliance Package and BPHMID for BkgPackage
        /// </summary>
        [DataMember]
        public Int32 PkgNodeMappingId { get; set; }
        [DataMember]
        public List<PkgPaymentOptions> lstPaymentOptions { get; set; }
        [DataMember]
        public Boolean IsBkgPkg { get; set; }
        [DataMember]
        public Boolean IsPkgLevel { get; set; }
        
        //UAT-3268
        [DataMember]
        public Decimal? BasePrice { get; set; }
        [DataMember]
        public Decimal? AdditionalPrice { get; set; }
        [DataMember]
        public String AdditionalPaymentOption { get; set; }
        [DataMember]
        public Int32? AdditionalPaymentOptionID { get; set; }
        [DataMember]
        public Boolean IsApprovalRequired { get; set; }
    }

    [Serializable]
    [DataContract]
    public class PkgPaymentOptions
    {
        /// <summary>
        /// Lookup-ID of Payment Option
        /// </summary>
        [DataMember]
        public Int32 PaymentOptionId { get; set; }

        /// <summary>
        /// Lookup Name of Payment Option
        /// </summary>
        [DataMember]
        public String PaymentOptionName { get; set; }

        /// <summary>
        /// Look-up Code of Payment Option
        /// </summary>
        [DataMember]
        public String PaymentOptionCode { get; set; }

        /// <summary>
        /// UAT-3958
        /// Bit for payment option, if approval is required.
        /// </summary>
        [DataMember]
        public Boolean IsApprovalRequired { get; set; }
    }

    //UAT-3268
    public class PkgAdditionalPaymentInfo
    {
        public String PackageName { get; set; }
        public String PackageLable { get; set; }
        public Int32 PackageID { get; set; }
        public Int32 BPHM_ID { get; set; }
        public Decimal? BasePrice { get; set; }
        public Decimal? AdditionalPrice { get; set; }
        public String AdditionalPaymentOption { get; set; }
        public Int32? AdditionalPaymentOptionID { get; set; }
    }
}
