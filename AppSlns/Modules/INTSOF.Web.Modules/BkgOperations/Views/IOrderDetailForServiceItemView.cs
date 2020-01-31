using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public interface IOrderDetailForServiceItemView
    {
        List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        Int32 SelectedTenantID { get; }

        #region UAT-2117:"Continue" button behavior
        Boolean IsSuccessIndicatorApplicable{ get; set; }
        Boolean IsAllExistingSearchesAreClear { get; set; }
        #endregion

        #region UAT-2200:Should send back to queue if everything in the service group is complete and clear and other service group(s) are in progress
        Int32 OrderPackageSvcGroupID
        {
            get;
            set;
        }

        Boolean IsOtherServiceGroupsAreCompleted { get; set; }
        #endregion
    }
}
