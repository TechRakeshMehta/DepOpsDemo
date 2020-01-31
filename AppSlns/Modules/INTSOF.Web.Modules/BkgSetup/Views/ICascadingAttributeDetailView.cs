using INTSOF.UI.Contract.BkgSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface ICascadingAttributeDetailView
    {
        int SelectedTenantId { get; set; }
        int SelectedAttributeId { get; set; }
        List<CascadingAttributeOptionsContract> AttributeOptions { get; set; }
        CascadingAttributeOptionsContract CurrentOption { get; set; }
        Int32 CurrentLoggedInUserId { get; }
    }
}
