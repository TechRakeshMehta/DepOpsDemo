using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;

namespace CoreWeb.CommonControls.Views
{
    public interface IAgencySearch
    {
        List<usp_SearchAgency_Result> LstAgencies { get; set; }
    }
}
