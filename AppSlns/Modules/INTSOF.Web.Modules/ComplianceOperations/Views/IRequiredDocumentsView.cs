using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IRequiredDocumentsView
    {
        List<ApplicantRequiredDocumentsContract> lstApplicantRequiredDocumentsContract { get; set; }

        Int32 TenantId { get; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 OrgUsrID { get; }

        #region UAT-2306
        //Applicantid Get and Set when screen opens from Admin(Applicant View from Compliance Search Screen)
        Int32 FromAdminApplicantID
        { get; set; }

        //ApplicantTenantid Get and Set when screen opens from Admin(Applicant View from Compliance Search Screen)
        Int32 FromAdminTenantID
        { get; set; }

        //Get and Set IsAdminView from Compliance Search Screen
        Boolean IsAdminView { get; set; }
        #endregion

        #region UAT-3161

        List<ApplicantRequiredDocumentsContract> lstApplicantRotReqdDocumentsContract { get; set; }

        #endregion
    }
}




