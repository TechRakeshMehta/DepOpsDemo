using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageServiceItemDetailView
    {
        IManageServiceItemDetailView CurrentViewContext { get; }
        Int32 PackageServiceItemId { get; set; }
        Int32 SelectedTenantId { get; set; }
    }
}
