
using INTSOF.UI.Contract.DataFeed_Framework;
using INTSOF.Utils;
using System;
using System.Collections.Generic;

namespace Business.DataFeed_Framework.DataFeed_Formatter
{
    public interface IDataFeedFormatter
    {
        Dictionary<String, Dictionary<String, String>> FormatDataFeed(FileFormat Type, String Data, Int32 TenantID, Int32 FormatID, DataFeedSettingContract dataFeedSettingContract);
        //String ConvertDictionarytoCSV(Dictionary<String, Dictionary<String, String>> filteredData, Int32 tenantID, Int32 formatID);
        String ConvertDictionarytoCSV(Dictionary<String, Dictionary<String, String>> filteredData, DataFeedSettingContract dataFeedSettingContract);
        Boolean GetIncludeOnlynew(Int32 tenanatId, Int32 SettingID);
    }
}
