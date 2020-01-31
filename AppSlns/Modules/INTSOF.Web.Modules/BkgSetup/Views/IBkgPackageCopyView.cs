using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IBkgPackageCopyView
    {
        Int32 CurrentLoggedInUserId { get; }
        IBkgPackageCopyView CurrentViewContext { get; }
        Int32 TenantId { get; set; }
        Int32 SourceHierarchyNodeId { get; set; }
        Int32 TargetHierarchyNodeId { get; set; }
        String BackGroundPackageName { get; set; }
        Int32 BPHM_ID { get;set; }
        String ErrorMessage { get; set; }
    }
}
