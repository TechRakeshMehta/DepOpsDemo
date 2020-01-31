using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IReportEmploymentDisclosureView
    {
        Int32 TenantID { get; set; }
        Int32 OrganizationUserID { get; set; }
        String DocumentTypeCode { get; set; }
        List<Int32> lstAnnouncementID { get; set; }
    }
}
