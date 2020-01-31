using System;
using System.Collections.Generic;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WidgetDataWebService.DAL
{
    public class WidgetData
    {
        public static DataSet Get(int tenantId, string name, Dictionary<string, string> parameters)
        {
            String dBConnectionString = ConfigurationManager.ConnectionStrings["WidgetMainDBConnection"].ConnectionString;

            if (name.ToLower() == "recentmessages")
            {
                dBConnectionString = ConfigurationManager.ConnectionStrings["WidgetMessagingDBConnection"].ConnectionString;
            }

            using (SqlConnection con = new SqlConnection(dBConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_GetWidgetData";

                    DataTable dt;
                    // create data table to insert items
                    dt = new DataTable("Parameters");
                    dt.Columns.Add("ParamName", typeof(string));
                    dt.Columns.Add("ParamValue", typeof(string));
                    if (parameters != null)
                        foreach (string key in parameters.Keys)
                        {
                            dt.Rows.Add(key, parameters[key]);
                        }

                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                    cmd.Parameters.AddWithValue("@WidgetName", name);
                    SqlParameter tvpParam = cmd.Parameters.AddWithValue("@Params", dt); //Needed TVP
                    tvpParam.SqlDbType = SqlDbType.Structured; //tells ADO.NET we are passing TVP

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        DataSet dataset = new DataSet();
                        da.Fill(dataset);

                        return dataset;
                    }
                }
            }
        }
    }
}