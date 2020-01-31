using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;

namespace Business.RepoManagers
{
    public class AgencyReviewManager
    {

        /// <summary>
        /// Returns the list of the 'lkpAgencySearchStatus'
        /// </summary>
        /// <returns></returns>
        public static List<AgencySearchStatusContract> GetAgencySearchStatus()
        {
            try
            {
                var _lstTemp = LookupManager.GetSharedDBLookUpData<lkpAgencySearchStatu>();
                var _lstSearchStatus = new List<AgencySearchStatusContract>();

                foreach (var srchSts in _lstTemp)
                {
                    _lstSearchStatus.Add(new AgencySearchStatusContract
                    {
                        SearchStatusCode = srchSts.SS_Code,
                        SearchStatusName = srchSts.SS_Name
                    });
                }

                return _lstSearchStatus;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get the Agency Review Queue related data
        /// </summary>
        /// <param name="selectedStatusCodes"></param>
        /// <param name="selectedTenantIds"></param>
        /// <param name="sortingFilteringXML"></param>
        /// <returns></returns>
        public static List<AgencyReviewQueueContract> GetAgencyQueueData(String selectedStatusCodes, String selectedTenantIds, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetAgencyReviewRepoInstance().GetAgencyQueueData(selectedStatusCodes, selectedTenantIds, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SetAgencySearchStatus(List<Int32> lstSelectedAgencyIds, String statusCode, Int32 currentUserId)
        {
            try
            {
                var statusId = LookupManager.GetSharedLookUpIDbyCode<lkpAgencySearchStatu>(ss => ss.SS_Code == statusCode);
                BALUtils.GetAgencyReviewRepoInstance().SetAgencySearchStatus(lstSelectedAgencyIds, statusId, currentUserId);
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
    }
}
