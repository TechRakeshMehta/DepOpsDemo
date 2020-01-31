using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

using WidgetDataWebService.Model;
using WidgetDataWebService.Utils;

namespace WidgetDataWebService.Controller
{
    public class WebServiceController
    {
        public static string GetData(string tenantId, string widgetName, string parameters, Boolean useDefaultTenantId)
        {
            JavaScriptSerializer s = new JavaScriptSerializer();
            List<CustDictionary> sr = s.Deserialize<List<CustDictionary>>(parameters);

            Dictionary<string, string> ps = new Dictionary<string, string>();
            if (sr != null)
                foreach (CustDictionary param in sr)
                    ps.Add(param.Key, param.Value);
            else
                ps = null;

            Dictionary<string, List<Dictionary<string, string>>> rs = BLL.WidgetData.Get(Convert.ToInt32(tenantId), widgetName, ps,useDefaultTenantId);

            List<CustRecordset> recordSets = new List<CustRecordset>();
            foreach (string key in rs.Keys)
            {
                CustRecordset recordSet = new CustRecordset();
                recordSet.Name = key;
                recordSet.Rows = new List<List<CustDictionary>>();
                foreach (Dictionary<string, string> row in rs[key])
                {
                    List<CustDictionary> record = new List<CustDictionary>();
                    foreach (string colKey in row.Keys)
                    {
                        record.Add(new CustDictionary(colKey, row[colKey]));
                        String path = GetPath(tenantId, widgetName, colKey, row);

                        if (path != null)
                        {
                            record.Add(new CustDictionary("RecordDetailPath", path));
                        }
                    }
                    recordSet.Rows.Add(record);
                }
                recordSets.Add(recordSet);
            }

            return s.Serialize(recordSets);
        }

        public static String GetPath(String tenantId, String widgetName, String colKey, Dictionary<String, String> row)
        {
            if (widgetName == "OrderHistory" && colKey == "OrderNumber")
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", tenantId },
                                                                    { "Child", WidgetDataConsts.ORDER_PAYMENT_DETAILS},
                                                                    { "OrderId", row[colKey]},
                                                                    {"ShowApproveRejectButtons",false.ToString()},
                                                                    {"Parent", WidgetDataConsts.DASHBOARD}
                                                                 };
                return String.Format("../../ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
            }
            else if (widgetName == "Subscriptions" && colKey == "CompliancePackageID")
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", tenantId },
                                                                    { "PackageId", row[colKey]},
                                                                    { "Child", WidgetDataConsts.SUBSCRIPTION_DETAIL}
                                                                 };
                return String.Format("../../ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
            }
            else if (widgetName == "RecentMessages" && colKey == "ADBMessageID")
            {
                return String.Format("../../Messaging/Pages/MessageViewer.aspx?messageID={0}&cType={1}&isImportant={2}&From={3}&isDashboardMessage={4}&Date={5}",
                    row[colKey], row["CommunicationTypeCode"], row["IsHighImportant"], row["FromID"], true, row["ReceiveDateFormatted"]);
            }
            else if (widgetName == "MyTasks" && colKey == "PackageID")
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", tenantId },
                                                                    { "ApplicantComplianceItemId", row["ApplicantComplianceItemId"]},
                                                                    { "PackageId", row[colKey]},
                                                                    { "Child", WidgetDataConsts.SUBSCRIPTION_DETAIL}
                                                                 };
                return String.Format("../../ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
            }
            else if (widgetName == "TaskDescription" && colKey == "CountOfRecords")
            {
                if (Convert.ToInt32(row[colKey]) == 0)
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String> { 
                                                                    { "Child", WidgetDataConsts.ApplicantPendingOrder },
                                                                    { "TenantId", tenantId }
                                                                  };
                    return String.Format("../../ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                }
                else
                {
                    if (!row[colKey].Equals("-1"))
                    {
                        if (row["ColumnName"].Equals("Enter Data"))
                        {
                            Dictionary<String, String> queryString = new Dictionary<String, String>();
                            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", tenantId },
                                                                    { "PackageId", row[colKey]},
                                                                    { "Child", WidgetDataConsts.SUBSCRIPTION_DETAIL}
                                                                 };
                            return String.Format("../../ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                        }
                        else if (row["ColumnName"].Equals("Renew Subscription"))
                        {
                            Dictionary<String, String> queryString = new Dictionary<String, String>();
                            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "OrderId", row[colKey] },
                                                                    { "TenantId", tenantId },
                                                                    {"IsDashbordNavigation","true"},
                                                                    { "Child",WidgetDataConsts.RenewalOrder}
                                                                 };
                            return String.Format("../../ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                        }
                    }
                    else
                    {
                        Dictionary<String, String> queryString = new Dictionary<String, String>();
                        queryString = new Dictionary<String, String> { 
                                                                    { "Child", WidgetDataConsts.PackageSubscription },
                                                                    { "TenantId", tenantId }
                                                                  };
                        return String.Format("../../ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                    }
                }
            }

            else if (widgetName == "VideoTutorial" && colKey == "APV_ID")
            {
                return String.Format("../../AdbVideos.aspx?WidgetVideo=" + true + "&APV_ID=" + row[colKey]);
            }
            return null;
        }
    }
}