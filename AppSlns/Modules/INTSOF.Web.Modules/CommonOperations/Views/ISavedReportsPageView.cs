
using Entity;
using System;
using System.Collections.Generic;

namespace CoreWeb.CommonOperations.Views
{
    public interface ISavedReportsPageView
    {
        ISavedReportsPageView CurrentViewContext { get; }
        List<ReportFavouriteParameter> LstReportFavouriteParameter { set; get; }
        Int32 CurrentUserId { get; }
        Int32 TenantId { get; set; }
        Int32 NodeId { get; set; }
    }
}

