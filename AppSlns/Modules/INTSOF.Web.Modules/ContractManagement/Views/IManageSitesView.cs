using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ContractManagement;

namespace CoreWeb.ContractManagement.Views
{
    public interface IManageSitesView
    {
        /// <summary>
        /// Represents the contractid to which the current Sites belong to
        /// </summary>
        Int32 ContractId { get; set; }

        /// <summary>
        /// Represents the TenantId 
        /// </summary>
        Int32 TenantId { get; set; }

        /// <summary>
        /// Represents the List of Sites under the selected Contract
        /// </summary>
        List<SiteContract> lstSiteContracts
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManageSitesView CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// Gets the current Logged in userid.
        /// </summary> 
        Int32 CurrentUserId
        {
            get;
        }

        /// <summary>
        /// MappingId of the Contract and SiteContract i.e. CSCM_ID of 'ContractSitesContractMapping' table
        /// </summary> 
        Int32 CSCMId
        {
            get;
            set;
        }

        /// <summary>
        /// Site to be Added or Updated
        /// </summary>
        SiteContract SiteContract
        {
            get;
            set;
        }
    }
}
