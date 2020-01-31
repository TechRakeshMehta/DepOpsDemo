using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgOrderResidentialHistoriesView
    {
        List<PreviousAddressContract> lstPreviousAddress { get; set; }

        Int32 MasterOrderID { get; }

        Int32 SelectedTenantID { get; }

        List<Int32> PackageIDs
        {
            get;
            set;
        }

        Boolean ShowCriminalAttribute_MotherName
        {
            get;
            set;
        }

        Boolean ShowCriminalAttribute_License
        {
            get;
            set;
        }

        Boolean ShowCriminalAttribute_Identification
        {
            get;
            set;
        }
    }
}
