#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.Mobility.Views
{
    public interface IApplicantBalancePaymentView
    {
        #region Variables

        #region Private Variables



        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties
        //UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
        Boolean IsDisplayNonPreferredOption
        {
            // get;
            set;
        }
        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserID
        {
            get;
        }

        Int32 OrderID
        {
            get;
            set;
        }

        String OrderNumber
        {
            get;
            set;
        }

        Int32? PreviousOrderID
        {
            get;
            set;
        }

        Int32 DPPSId
        {
            get;
            set;
        }

        /// <summary>
        /// PK of the DPP table
        /// </summary>
        Int32 DPPId
        {
            get;
            set;
        }

        List<lkpPaymentOption> LstPaymentOptions 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Payment Options based on Node or Pkg level
        /// </summary>
        List<PkgList> lstPaymentOptions
        {
            get;
            set;
        }

        Decimal? AmountDue
        {
            get;
            set;
        }

        OrganizationUserProfile OrganizationUserProfile
        {
            get;
            set;
        }

        String PackagePrice
        {
            get;
            set;
        }
        
        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }

        Int32 PaymentTypeID
        {
            get;
        }

        String InstitutionHierarchy
        {
            get;
            set;
        }

        String SourceInstitutionHierarchy
        {
            get;
            set;
        }

        String Package
        {
            get;
            set;
        }

        String SourcePackage
        {
            get;
            set;
        }

        String SubscriptionPeriodMonths
        {
            get;
            set;
        }

        Boolean IsOnlineOrderFailed
        {
            get;
            set;
        }
        String InfoMessage { get; set; }

        Int32? TargetHierarchyNodeID { get; set; }


        Int32 PaymentMode_InvoiceWithoutApprovalId { get; set; }

        Int32 PaymentMode_InvoicetoInstitutionId { get; set; }

        /// <summary>
        /// SelectedNodeId column of Order table. Will be used as DPMId to fetch the 
        /// PaymentOptions, if the PAckage level payment options are not defined.
        /// </summary>
        Int32 SelectedNodeId
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion

        #endregion
    }
}


