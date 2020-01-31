using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManagePackageSubscriptionView
    {
        List<CompliancePackage> ClientCompliancePackages { get; set; }
        Int32 loggedInUserId { get;  }
        Int32 tenantId { get; set; }
        Int32 packageId { get; set; }
    }
}




