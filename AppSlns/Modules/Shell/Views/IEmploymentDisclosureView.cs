using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Shell.Views
{
    public interface IEmploymentDisclosureView
    {
        Int32 TenantID { get; set; }
        Int32 OrganizationUserID { get; set; }
        String DocumentTypeCode { get; set; }
        List<Int32> lstAnnouncementID { get; set; }
    }
}
