using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public interface ICustomFormLoadForServiceItemView
    {
        Int32 MasterOrderID { get; }
        Int32 SelectedTenantID { get; }
        List<SupplementServiceItemCustomForm> lstSupplementServiceCustomFormList { get; set; }
        List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        //List<Int32> SelectedServiceItem { get; }
        #region UAT-586 WB: AMS: When adding supplemental services to an order, need to be able to see what the applicant has already entered (location, alias, etc.)
        List<AttributesForCustomFormContract> lstPreExitingSupplementAttributes { get; set; }
        #endregion

        List<Int32> LstDistinctCustomFormId { get; set; }

        //UAT-2116: Move "Select Services" to the next page and remove its current screen
        //List<Int32> SelectedSupplementServices { get; }
        List<Int32> SelectedSupplementServices { get; set; }
        //List<Int32> SetSupplementServicesToSession { set; }
        List<SupplementServicesInformation> lstSupplementServiceList { get; set; }
        //UAT-2249
        Int32 OrderPackageSvcGroupID
        {
            get;
            set;
        }
        Boolean IsOtherServiceGroupsAreCompleted { get; set; }
        Boolean IsSuccessIndicatorApplicable { get; set; }
        Boolean IsAllExistingSearchesAreClear { get; set; }
    }
}
