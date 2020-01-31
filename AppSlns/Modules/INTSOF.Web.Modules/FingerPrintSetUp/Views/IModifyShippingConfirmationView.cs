using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IModifyShippingConfirmationView
    {
        #region Variables

        #region Private Variables



        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties

        Boolean IsSSNDisabled { get; set; }

        #endregion

        #region Public Properties

        Int32 TenantId { get; }
       
        IModifyShippingConfirmationView CurrentViewContext
        {
            get;
        }
        List<Int32> DPPSIds
        {
            get;
            set;
        }
        Int32 OrgUsrID { get; }
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        List<OrderPaymentDetail> lstAdditionalPaymentModes { get; set; }
        String Gender
        {
            get;
            set;
        }
        DataTable lstExternalPackages
        {
            get;
            set;
        }
        Int32 GenderId { get; set; }
        String DecryptedSSN { get; set; }
        List<Entity.lkpSuffix> lstSuffixes { get; set; }
        Boolean IsLocationServiceTenant { get; set; }
        String LanguageCode { get; }



        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }
        List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        List<AttributeFieldsOfSelectedPackages> LstInternationCriminalSrchAttributes
        {
            get;
            set;
        }

        String InstitutionHierarchy
        {
            get;
            set;
        }

        List<OrderCartCompliancePackage> CompliancePackages { get; set; }
        AppointmentOrderScheduleContract AppointmentDetailContract { get; set; }

        /// <summary>
        /// List of Instructions to bind 
        /// </summary>
        List<Tuple<String, String>> lstClientPaymentOptns
        {
            get;
            set;
        }

        Boolean IsOrderStatusPaid
        {
            get;
            set;
        }

        /// <summary>
        /// Store the OPD-ID, in case single package is being used
        /// </summary>
        Int32 OrderPaymentDetaildId { get; set; }

        List<Order> OrderData
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        List<OrderPaymentDetail> lstOPDs { get; set; }

        /// <summary>
        /// Payment Type Code for the Payment OptionID used for ApplicantBalancePayment scenario
        /// </summary>
        String AppChangeSubPaymentTypeCode
        {
            get;
            set;
        }

        List<Int32> RecentAddedOPDs { get; set; }

        String OrderRequestType { get; set; }

        #endregion

        #endregion
    }
}
