using DAL.Interfaces;
//using Entity;
using Entity.ClientEntity;
using INTSOF.UI.Contract.DataFeed_Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Repository
{
    public class DataFeedRepository : ClientBaseRepository, IDataFeedRepository
    {        
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;

        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
        public DataFeedRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;
        }

        #region DATA FEED FRAMEWORK
               
        /// <summary>
        /// Method to authenticate Data Feed Settings
        /// </summary>
        /// <param name="settingID">settingID</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        Boolean IDataFeedRepository.IsDataFeedAccessSettingAuthenticated(Int32 settingID, Guid accessKey)
        {
            return base.SecurityContext.DataFeedSettings.Any(cond => cond.DFS_SettingID == settingID && cond.DFS_IsDeleted == false && cond.WCFAccess.WCFA_AccessKey == accessKey);
        }

        /// <summary>
        /// Method to get include only new bit
        /// </summary>
        /// <param name="settingId">settingId</param>
        /// <returns></returns>
        Boolean IDataFeedRepository.GetIncludeOnlyNew(Int32 settingId)
        {
            return base.SecurityContext.DataFeedSettings.Where(x => x.DFS_SettingID == settingId).Select(x => x.DFS_IncludeOnlyNew).FirstOrDefault();
        }

        /// <summary>
        /// Method to get the Data feed xml on the basis of settingID
        /// </summary>
        /// <param name="settingID">settingID</param>
        /// <returns>XMLDatafeed</returns>
        Dictionary<String,String> IDataFeedRepository.GetDataFeedXml(Int32 settingID, DateTime recordOriginStartDate, DateTime recordOriginEndDate)
        {
            int dataFeedLength =Convert.ToInt32(ConfigurationManager.AppSettings["DataFeedLength"]);
            String EntityFilterxml = GetEntityFilters(settingID);
            var res = new Dictionary<String, String>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("dff.usp_GetDataFeedXML", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RecordOriginStartDate", recordOriginStartDate);
                command.Parameters.AddWithValue("@RecordOriginEndDate", recordOriginEndDate);
                command.Parameters.AddWithValue("@SettingID", settingID);
                command.Parameters.AddWithValue("@EntityFilterSettingxml", EntityFilterxml);
                command.CommandTimeout = 120;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["XMLDataFeed"].ToString().Length < dataFeedLength)
                    {
                        res.Add("Result", "SUCCCESS");
                        res.Add("Text", ds.Tables[0].Rows[0]["XMLDataFeed"].ToString());
                        return res;
                    }
                    else
                    {
                        res.Add("Result", "TOOLARGE");
                        res.Add("Text", "Data volume seems to be too large. Please consider a shorter date range.");
                        return res;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// returns entity type code based on settings id
        /// </summary>
        /// <param name="SettingID">SettingID</param>
        /// <returns></returns>
        String IDataFeedRepository.GetEntityCode(Int32 SettingID)
        {
            return base.SecurityContext.DataFeedSettings.Where(x => x.DFS_SettingID == SettingID).Select(x => x.lkpEntityType.Code).FirstOrDefault();
        }

        /// <summary>
        /// Get entity filters based on settting ID
        /// </summary>
        /// <param name="SettingID"></param>
        /// <returns></returns>
        public String GetEntityFilters(Int32 SettingID)
        {
            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("dff.usp_GetEntityFilters", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SettingID", SettingID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["FilterSettings"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Method to get format of output based on formatid
        /// </summary>
        /// <param name="FormatID"></param>
        /// <returns></returns>
        List<OutputColumn> IDataFeedRepository.GetFormat(Int32 FormatID)
        {
            List<OutputColumn> lst = new List<OutputColumn>();
            OutputColumn obj = new OutputColumn();
            String query = base.SecurityContext.lkpFormats.Where(x => x.FormatID == FormatID).Select(x => x.Query).FirstOrDefault();
            if (!String.IsNullOrEmpty(query))
            {
                EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@formatID", FormatID);
                    if (con.State == ConnectionState.Closed)
                    { con.Open(); }
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            obj = new OutputColumn();
                            obj.DisplayOrder = Convert.ToInt32(sdr[0].ToString());
                            obj.DataXPath = sdr[1].ToString();
                            obj.DisplayHeader = sdr[2].ToString();
                            obj.HeaderTextName = Convert.ToString(sdr[3]);
                            obj.IsServiceGroup = Convert.ToBoolean(sdr[4]);
                            obj.IsCustomAttribute = Convert.ToBoolean(sdr[5]);                            
                            lst.Add(obj);
                        }
                    }
                }
            }

            return lst;
        }

     
        #endregion
    }
}
