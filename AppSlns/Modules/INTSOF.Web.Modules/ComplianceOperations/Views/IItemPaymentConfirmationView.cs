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

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IItemPaymentConfirmationView
    {
        #region Properties

        #region Private Properties

        Boolean IsSSNDisabled { get; set; }

        #endregion

        #region Public Properties

        Int32 TenantId { get; set; }
        Int32 ZipCodeId { get; set; }
        IItemPaymentConfirmationView CurrentViewContext
        {
            get;
        }
        
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        String Gender { get; set; }
        Int32 GenderId { get; set; }

        Entity.ZipCode ApplicantZipCodeDetails { get; set; }
         
        /// <summary>
        /// List of Instructions to bind 
        /// </summary>
        List<Tuple<String, String>> lstClientPaymentOptns
        {
            get;
            set;
        }

        #endregion

        #endregion

        //UAT-781
        String DecryptedSSN { get; set; }

        Int32 OrgUsrID { get; }

        List<PkgPaymentOptions> lstPaymentOptions { get; set; }
        String PaymentModeCode { get; set; }
        String PaymentModeDisplayName { get; set; }
        String InstructionText { get; set; }
        Int32 PaymentModeId { get; set; }
        Boolean IsInstructorPreceptorPackage { get; set; }
    }
}




