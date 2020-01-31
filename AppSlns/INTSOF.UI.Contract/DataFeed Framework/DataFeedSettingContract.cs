using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.DataFeed_Framework
{
    [Serializable]
    public class DataFeedSettingContract
    {
        public Int32 DataFeedSettingId { get; set; }
        public Guid AccessKey { get; set; }
        public Int32 TenantID { get; set; }
        public String TenantDBName { get; set; }
        public String TenantName { get; set; }
        public DateTime? RecordOriginStartDate { get; set; }
        public DateTime? RecordOriginEnddate { get; set; }
        public Int32 DataFeedInvokeHistoryId { get; set; }
        public DateTime? DataFeedInvokeDateTime { get; set; }
        public List<DataFeedInvokeHistoryDetail> DataFeedInvokeDetailList { get; set; }
        public Int32 OutputID { get; set; }
        public String OutputCode { get; set; }
        public Int32 DeliveryTypeId { get; set; }
        public String DeliveryTypeCode { get; set; }
        public Int32 FormatID { get; set; }
        public Int32 ServiceIntervalMonth { get; set; }
        public Int32 ServiceIntervalDays { get; set; }
        public Int32 NoOfDays { get; set; }
        public Int32 NoOfMonths { get; set; }
        public Int32? DataFeedIntervalModeID { get; set; }
        public String DataFeedIntervalModeCode { get; set; }
        public Int32 DataFeedFTPDetailID { get; set; }
        public Boolean IncludeOnlyNew { get; set; }
        public String FieldSeparator { get; set; }
        public String RowSeparator { get; set; }
        //UAT-1166 Changes
        public String ReportingEmails { get; set; }
        public String FileNameSuffix { get; set; }
        public String ReportingEmailSubject { get; set; }
        public Boolean IncludeServiceGroup { get; set; }
        public Boolean IncludeCustomFields { get; set; }
    }
}
