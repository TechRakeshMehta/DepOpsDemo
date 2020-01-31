using INTSOF.UI.Contract.DataFeed_Framework;
using INTSOF.Utils;
using System;
using System.Collections.Generic;

namespace Business.RepoManagers
{
    public class DataFeedManager
    {
        #region DATA FEED FRAMEWORK

       
        /// <summary>
        /// Method to check the Setting and access key authentication.
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <param name="settingID">settingID</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns>Boolean</returns>
        public static Boolean IsDataFeedAccessSettingAuthenticated(Int32 tenantID, Int32 settingID, Guid accessKey)
        {
            try
            {
               return BALUtils.GetDataFeedRepoInstance(tenantID).IsDataFeedAccessSettingAuthenticated(settingID, accessKey);
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
        /// Method to get include only new bit 
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="SettingID">SettingID</param>
        /// <returns></returns>
        public static Boolean GetIncludeOnlyNew(Int32 tenantId, Int32 SettingID)
        {
            try
            {
                return BALUtils.GetDataFeedRepoInstance(tenantId).GetIncludeOnlyNew(SettingID);
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
        /// Method to get the Data feed xml on the basis of settingID
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <param name="settingID">settingID</param>
        /// <returns>XMLDatafeed</returns>
        public static Dictionary<String,String> GetDataFeedXml(Int32 tenantID, Int32 settingID, DateTime recordOriginStartDate, DateTime recordOriginEndDate)
        {
            try
            {
                String EntityCode= BALUtils.GetDataFeedRepoInstance(tenantID).GetEntityCode(settingID);
                if (EntityCode == "AAAA")// Background Orders
                {
                    return BALUtils.GetDataFeedRepoInstance(tenantID).GetDataFeedXml(settingID, recordOriginStartDate, recordOriginEndDate);
                }
                else
                {
                    return new Dictionary<String,String>();
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

        /// <summary>
        /// Get format of CSV based on query 
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="OutputID">FormatID</param>
        /// <param name="FormatID"></param>
        /// <returns></returns>
        public static List<OutputColumn> GetFormat(Int32 tenantId, Int32 FormatID)
        {
            try
            {
                return BALUtils.GetDataFeedRepoInstance(tenantId).GetFormat(FormatID);
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
