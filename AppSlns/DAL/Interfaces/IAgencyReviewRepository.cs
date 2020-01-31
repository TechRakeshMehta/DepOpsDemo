using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;

namespace DAL.Interfaces
{
    public interface IAgencyReviewRepository
    {
        /// <summary>
        /// Get the Agency Review Queue related data
        /// </summary>
        /// <param name="selectedStatusCodes"></param>
        /// <param name="selectedTenantIds"></param>
        /// <param name="sortingFilteringXML"></param>
        /// <returns></returns>
        List<AgencyReviewQueueContract> GetAgencyQueueData(String selectedStatusCodes, String selectedTenantIds, CustomPagingArgsContract customPagingArgsContract);


        /// <summary>
        /// Set Agency status to reviwed or available, based on the StatusID
        /// </summary>
        /// <param name="lstSelectedAgencyIds"></param>
        /// <param name="statusId"></param>
        void SetAgencySearchStatus(List<Int32> lstSelectedAgencyIds, Int32 statusId, Int32 currentUserId);
    }
}
