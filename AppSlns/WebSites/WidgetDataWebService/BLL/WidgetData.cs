using System;
using System.Collections.Generic;
using System.Data;
using Business.RepoManagers;

namespace WidgetDataWebService.BLL
{
    public class WidgetData
    {
        public static Dictionary<string, List<Dictionary<string, string>>> Get(int tenantId, string name, Dictionary<string, string> parameters, Boolean useDefaultTenantId)
        {
            if (useDefaultTenantId)
            {
                tenantId = SecurityManager.DefaultTenantID;
            }

            DataSet resultds = DAL.WidgetData.Get(tenantId, name, parameters);

            Dictionary<string, List<Dictionary<string, string>>> resultSets = new Dictionary<string,List<Dictionary<string,string>>>();
            foreach (DataTable dt in resultds.Tables)
            {
                List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                Dictionary<string, string> row = null;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, string>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.Trim(), dr[col].ToString());
                    }
                    rows.Add(row);
                }
                resultSets.Add(dt.TableName, rows);
            }

            return resultSets;
        }
    }
}