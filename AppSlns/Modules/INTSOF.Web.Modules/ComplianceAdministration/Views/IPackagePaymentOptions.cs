using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IPackagePaymentOptions
    {
        /// <summary>
        /// List of Payment Options, along with flag whether they are selected or not
        /// </summary>
        List<Tuple<Int32, String, Boolean>> PaymentOptions
        {
            set;
        }

        /// <summary>
        /// Gets the Id's of the Payment Options selected for Save/Update, by admin
        /// </summary>
        List<Int32> SelectedPaymentOptions
        {
            get;
        }

        /// <summary>
        /// Id of the Selected Tenant
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Code to identify whether we are using the control for Compliance Package or Background Package
        /// </summary>
        String PackageTypeCode
        {
            get;
            set;
        }

        /// <summary>
        /// Will be DPP_ID for Compliance Package 
        /// and BPHM_ID for Background Packae
        /// </summary>
        Int32 PkgNodeMappingId
        {
            get;
            set;
        }
        //UAT-2073
        Int32 PaymentApprovalID
        {
            get;
            set;
        }
        Int32 NotSpecifiedPaymentApprovalID
        {
            get;
            set;
        }
        List<lkpPaymentApproval> PaymentApprovalList
        {
            set;
        }

        //UAT-3268

        //Boolean IsReqToQualifyInRotation
        //{ get; set; }
        //Int32 AdditionalPriceSelectedPaymentOptionID { get; }
        //List<lkpPaymentOption> AdditionalPaymentOptions { get; set; }
        //Int32 SelectedAdditionalPaymentTypeID { set; }
    }
}
