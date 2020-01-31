
using System;
namespace Business.DataFeed_Framework.DataFeed_Formatter
{
    public class DataFeedFormatterFactory 
    {
        public IDataFeedFormatter GetDataFeedFormatter(Int32 SettingId)
        {
            return new DataFeedFormatterCSV();
        }
    }
}
