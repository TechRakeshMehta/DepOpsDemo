using INTSOF.Contracts;
using INTSOF.ServiceUtil;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Web;

namespace Business.RepoManagers
{
    public class QueueImagingManager
    {
        #region Verification Data sync

        public static void SyncVerificationDataForTenant(Int32 tenantID)
        {
            try
            {
                BALUtils.GetQueueImagingRepoInstance().SyncVerificationDataForTenant(tenantID);
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

        public static List<Int32> GetTenantListDueForImaging()
        {
            try
            {
                return BALUtils.GetQueueImagingRepoInstance().GetTenantListDueForImaging();
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

        #endregion

        #region Update Due Imaging
        /// <summary>
        /// Method that update and insert in QueueImaging
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <returns></returns>
        public static Boolean UpdateInsertQueueImagingDue(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetQueueImagingRepoInstance().UpdateInsertQueueImagingDue(tenantId);
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

        public static void AsignQueueImagingRepoInstance(Dictionary<String, Object> dicMultipleTenantData)
        {

            try
            {
                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                
                    ParallelTaskContext.PerformParallelTask(ParalleAssignQueueImagingRepoInstance, dicMultipleTenantData, LoggerService, ExceptiomService);
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

        public static void ParalleAssignQueueImagingRepoInstance(Dictionary<String, Object> dicMultipleTenantData)
        {
            try
            {
                if (dicMultipleTenantData.IsNotNull() && dicMultipleTenantData.Count > 0)
                {

                    if (dicMultipleTenantData.ContainsKey("TenantID"))
                    {
                        BALUtils.GetQueueImagingRepoInstance().UpdateInsertQueueImagingDue(Convert.ToInt32(dicMultipleTenantData["TenantID"]));
                    }

                }
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

        #endregion
    }
}
