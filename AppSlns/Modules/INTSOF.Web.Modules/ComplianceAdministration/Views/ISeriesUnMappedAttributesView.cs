using INTSOF.UI.Contract.ComplianceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ISeriesUnMappedAttributesView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; }
        Int32 ItemSeriesId { get; set; }
        List<SeriesAttributeContract> UnMappedAttributesList { get; set; }
        String ControlIdSuffix { get; set; }

    }
}
