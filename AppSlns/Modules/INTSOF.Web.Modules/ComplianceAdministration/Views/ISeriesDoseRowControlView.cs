using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ISeriesDoseRowControlView
    {
        Int32? ItemSeriesItemId { get; set; }

        Int32 SeriesId
        {
            get;
            set;
        }

        Int32 CurrentUserId
        {
            get;
        }


        Int32 TenantId
        {
            get;
            set;
        }

        Int32? CIId
        {
            get;
            set;
        }

    }
}
