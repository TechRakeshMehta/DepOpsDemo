using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IShotSeriesShuffleTestView
    {
        IShotSeriesShuffleTestView CurrentViewContext
        {
            get;
        }

        List<SeriesAttributeContract> SeriesData
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentSeriesID
        {
            get;
            set;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Int32 DefaultTenantId
        {
            get;
            set;
        }

        List<lkpItemComplianceStatu> LstItemComplianceStatus { get; set; }

        List<RuleDetailsForTestContract> LstSeriesRuleDetails { get; set; }


        List<SeriesAttributeContract> SeriesDataAfterShuffle { get; set; }

        ShotSeriesSaveResponse ShotSeriesResponse { get; set; }

        Int32 CategoryID { get; set; }

        List<CompliancePackage> LstPackages { get; set; }

        Int32 SelectedPackageID { get; }
    }
}




