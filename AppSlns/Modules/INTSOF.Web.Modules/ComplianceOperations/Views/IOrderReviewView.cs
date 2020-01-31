#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IOrderReviewView
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

        List<lkpPaymentOption> lstPaymentOptions { get; set; }
        Int32 TenantId { get; set; }
        Int32 ZipCodeId { get; set; }
        List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        IOrderReviewView CurrentViewContext
        {
            get;
        }
        Int32 DPPSId
        {
            get;
            set;
        }
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        Entity.ZipCode ApplicantZipCodeDetails { get; set; }
        String Gender { get; set; }
        Int32 GenderId { get; set; }
        Boolean UpdateOriginalData { get; set; }
        String PaymentModeCode { get; set; }
        Int32 GeneratedOrderId { get; set; }
        Boolean ShowRushOrderForInvioce { get; set; }

        Boolean ShowRushOrder{get;set;}
        Int32 PaymentMode_InvoiceId { get; set; }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }
        List<PersonAliasContract> PersonAliasList
        {
            get;
            set;
        } 

        /// <summary>
        /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
        /// </summary>
        String OrderType
        {
            get;
            set;
        }

        //UAT 1438
        List<UserGroup> selectedUserGrpList
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
        #endregion
        #endregion

        #endregion

        #region Methods

        #region Public Methods


        #endregion

        #region Private Methods


        #endregion

        #endregion

        List<AttributeFieldsOfSelectedPackages> LstInternationCriminalSrchAttributes
        {
            get;
            set;
        }

        //UAT-3541 CBI || CABS
        AppointmentSlotContract AppointmentSlotContract { get; set; }
        Boolean IsLocationServiceTenant { get; set; }
        List<Entity.lkpSuffix> lstSuffixes { get; set; }
        String LanguageCode { get; }
    }
}







