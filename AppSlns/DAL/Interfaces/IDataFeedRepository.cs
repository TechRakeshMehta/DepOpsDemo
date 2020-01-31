using INTSOF.UI.Contract.DataFeed_Framework;
using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IDataFeedRepository
    {
        #region DATA FEED FRAMEWORK
        /// <summary>
        /// Method to check the Setting and access key authentication.
        /// </summary>
        /// <param name="settingID">settingID</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns>Boolean</returns>
        Boolean IsDataFeedAccessSettingAuthenticated(Int32 settingID, Guid accessKey);

        /// <summary>
        /// Method to get the Data feed xml on the basis of settingID
        /// </summary>
        /// <param name="settingID">settingID</param>
        /// <returns>XMLDatafeed</returns>
        Dictionary<String,String> GetDataFeedXml(Int32 settingID,DateTime recordOriginStartDate,DateTime recordOriginEndDate);

        /// <summary>
        /// Method to get the output format of the Data feed. 
        /// </summary>
        /// <param name="FormatID"></param>
        /// <returns></returns>
        List<OutputColumn> GetFormat(Int32 FormatID);

        /// <summary>
        /// Method to get the entity code based on the setting ID
        /// </summary>
        /// <param name="settingID"></param>
        /// <returns></returns>
        String GetEntityCode(Int32 settingID);

        /// <summary>
        /// Method to get Include only new field
        /// </summary>
        /// <param name="settingID"></param>
        /// <returns></returns>
        Boolean GetIncludeOnlyNew(Int32 settingID);
        #endregion

    }
}
