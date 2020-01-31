using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface ITrackingNumberViewer
    {
        ITrackingNumberViewer CurrentViewContext { get; }
        Int32 CurrentUserId { get; }
        Int32 TenantId { get; set; }
        Int32 DetailExtId { get; set; }
        Int32 TrackingNumber { get; set; }

    }
}
