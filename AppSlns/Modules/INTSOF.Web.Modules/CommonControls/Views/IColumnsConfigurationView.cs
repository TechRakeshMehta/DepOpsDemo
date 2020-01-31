using INTSOF.UI.Contract.CommonControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.CommonControls.Views
{
    public interface IColumnsConfigurationView
    {
        List<ColumnsConfigurationContract> ColumnsConfigurationData { get; set; }
        Int32 CurrentLoggedInUserID { get; set; }
        List<String> lstGridCode { get; set; }
        //   List<String> lstPredefinedHiddenColumn { get; set; }
        List<PreHiddenColumnsContract> lstPredefinedHIddenColumns { get; set; }
        Int32 OrganisationUserID { get; set; }
    }
}
