using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IApplicantDataSummaryPopupView
    {
        Int32 CurrentLoggedInUserID
        {
            get;
        }

        DateTime? UserLastLoginTime
        {
            get;
            set;
        }

        List<ApplicantDataSummaryContract> lstApplicantSummary { get; set; }
        Int32 ApplicantTenantId { get; set; }

        List<ApplicantBackgroundSummaryContract> lstApplicantBackgroundSummary { get; set; }

        List<RequirementSharesDataContract> lstApprovedRotations { get; set; }
        List<UpcomingCategoryExpirationContract> lstUpcomingCategoryExpiration { get; set; }

        //UAT-2003: Add ability to extend/renew when clicking "place order"
        List<Entity.ClientEntity.vwSubscription> ListSubscription { get; set; }

        //Int32 ClientSettingBeforeExpiry
        //{
        //    get;
        //    set;
        //}
        List<SubscriptionFrequency> lstSubscriptionFrequencies { get; set; }
    }
}
