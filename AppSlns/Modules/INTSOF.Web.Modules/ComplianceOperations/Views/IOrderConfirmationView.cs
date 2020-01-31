#region Namespaces

#region SystemDefined

using System;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Collections.Generic;
using INTSOF.UI.Contract.BkgOperations;
using System.Data;
using INTSOF.UI.Contract.FingerPrintSetup;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IOrderConfirmationView
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
        Int32 ZipCodeId { get; set; }
        IOrderConfirmationView CurrentViewContext
        {
            get;
        }
        List<Int32> DPPSIds
        {
            get;
            set;
        }
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        Boolean IsLocationServiceTenant { get; set; }
        String Gender { get; set; }
        Int32 GenderId { get; set; }
        //DeptProgramPackageSubscription SelectedPackageDetails { get; set; }
        Entity.ZipCode ApplicantZipCodeDetails { get; set; }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }

        String InstitutionHierarchy
        {
            get;
            set;
        }

        List<OrderCartCompliancePackage> CompliancePackages { get; set;}

        /// <summary>
        /// List of Instructions to bind 
        /// </summary>
        List<Tuple<String, String>> lstClientPaymentOptns
        {
            get;
            set;
        }

        /// <summary>
        /// Store the OPD-ID, in case single package is being used
        /// </summary>
        Int32 OrderPaymentDetaildId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        List<OrderPaymentDetail> lstOPDs { get; set; }

        DataTable lstExternalPackages
        {
            get;
            set;
        }

        List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }

        /// <summary>
        /// Payment Type Code for the Payment OptionID used for ApplicantBalancePayment scenario
        /// </summary>
        String AppChangeSubPaymentTypeCode
        {
            get;
            set;
        }

        #region E DRUG SCREENING PROPERTIES
        Int32 EDrugScreenCustomFormId
        {
            get;
            set;
        }
        Int32 EDrugScreenAttributeGroupId
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
        /// Data of the Order being shown
        /// </summary>
        List<Order> OrderData
        {
            get;
            set;
        }

        #endregion
        #endregion

        #endregion

        #region Methods

        #region Public Methods


        #endregion

        #region Private Methods


        #endregion

        #endregion

        //UAT-781
        String DecryptedSSN { get; set; }

        //UAT 1438
        List<UserGroup> selectedUserGrpList { get; set; }

        Int32 OrgUsrID { get; }

        #region UAT-1648
        /// <summary>
        /// Property identify the OPDs those are recently added in Applicant Completing Order Process for "SentForOnlinePayment".
        /// </summary>
        List<Int32> RecentAddedOPDs { get; set; }

        String OrderRequestType { get; set; }
        #endregion

        List<AttributeFieldsOfSelectedPackages> LstInternationCriminalSrchAttributes
        {
            get;
            set;
        }
        //UAT-3268

        List<OrderPaymentDetail> lstAdditionalPaymentModes { get; set; }


        //UAT-3541
        AppointmentOrderScheduleContract AppointmentDetailContract { get; set; }
        List<Entity.lkpSuffix> lstSuffixes { get; set; }
        String LanguageCode { get; }
    }
}




