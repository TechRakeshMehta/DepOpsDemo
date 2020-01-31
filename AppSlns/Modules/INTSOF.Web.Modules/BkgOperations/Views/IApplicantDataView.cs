using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IApplicantDataView
    {
        /// <summary>
        /// PK of the Order table i.e. OrderId
        /// </summary>
        Int32 MasterOrderId { get; set; }

        /// <summary>
        /// Id of the selected Tenant
        /// </summary>
        Int32 TenantId { get; set; }

        #region UAT-2062:
        List<SupplementAdditionalSearchContract> lstSSNResultForAdditionalSearch
        {
            get;
            set;
        }

        List<SupplementAdditionalSearchContract> lstMatchedNameForAdditionalSearch
        {
            get;
            set;
        }

        List<SupplementAdditionalSearchContract> lstMatchedLocationForAdditionalSearch
        {
            get;
            set;
        }
        #endregion

        String ParentScreenName { get; set; }
    }
}
