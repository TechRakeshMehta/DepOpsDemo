using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using System.Runtime.Serialization.Json;

namespace CoreWeb.Shell.Views
{
    public class DummyComplioAPIPresenter : Presenter<IDummyComplioAPIView>
    {
        public override void OnViewLoaded()
        {
            View.ListTenants = SecurityManager.GetTenantList();

            View.ListEntityType = SecurityManager.GetAPIMetaDataList();
        }
        public override void OnViewInitialized()
        {
        }
        public void ChangeDataFormat()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("tenantid", typeof(Int32));
            dt.Columns.Add("EntityTypeCode", typeof(string));
            dt.Columns.Add("SPInputData", typeof(string));
            dt.Rows.Add(View.SelectedTenantID,View.SelectedEntityTypeCode,View.InputData);

            String JsonData = DataTableToJSONWithStringBuilder(dt);
                                   
            //String Json = JSONConvert.SerializeObject(dt); 
        }

        private string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }

        public String GetInstitutionUrlByExternalUserTenantID()
        {
            return WebSiteManager.GetInstitutionUrl(View.SelectedTenantID);
        }
    }
}
