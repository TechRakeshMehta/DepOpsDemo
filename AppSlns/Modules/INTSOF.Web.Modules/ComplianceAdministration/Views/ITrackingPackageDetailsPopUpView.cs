using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using System;
using System.Collections.Generic;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ITrackingPackageDetailsPopUpView
    {
        Int32 TenantId
        {
            set;
            get;
        }

        Int32 trackingPackageRequiredDOCURLId
        {
            get;
            set;
        }

        List<CompliancePackage> lstPackagesNames
        {
            get;
            set;
        }

        List<TrackingPackageRequiredContract> lstPackagesIDs
        {
            get;
            set;
        }

        ITrackingPackageDetailsPopUpView CurrentViewContext { get; }

    }
}
