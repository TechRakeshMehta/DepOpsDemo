using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IInstructorPreceptorDashboardView
    {

        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; set; }
        IInstructorPreceptorDashboardView CurrentViewContext { get; }
        List<String> SharedUserTypeCodes { get; }
        Guid UserId { get; }
        SharedUserDashboardDetailsContract SharedUserDetails { get; set; }
    }
}
