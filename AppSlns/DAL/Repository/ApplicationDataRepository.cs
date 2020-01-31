#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ApplicationDataRepository.cs
// Purpose:
//

#endregion

#region Namespaces

#region SystemDefined

using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

#endregion

#region UserDefined

using Entity;
using DAL.Interfaces;
using INTSOF.Utils;
using System.Collections.Generic;
using INTSOF.UI.Contract.SysXSecurityModel;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System.Xml.Serialization;
using System.Text;

#endregion

#endregion

namespace DAL.Repository
{
    public class ApplicationDataRepository : BaseRepository, IApplicationDataRepository
    {
        #region Variables

        #region Public Variables



        #endregion

        #region Private Variables

        private SysXAppDBEntities _dbNavigation;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public ApplicationDataRepository()
        {
            _dbNavigation = base.Context;
        }

        #endregion

        #region Properties

        #region Public Properties



        #endregion

        #region Private Properties



        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// get object from database with the provided key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Object IApplicationDataRepository.GetObjectDataByKey(String key)
        {
            if (key == "ApplicantInstData")// temporary changes to accommodate multiple instances of ApplicantInstData
            {
                Dictionary<String, ApplicantInsituteDataContract> dictionaryData = new Dictionary<String, ApplicantInsituteDataContract>(); ;

                EntityConnection connection = this.Context.Connection as EntityConnection;
                List<WebApplicationData> WebAppdata = new List<WebApplicationData>();
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                    {
                    new SqlParameter("@key", key),
                    };

                    base.OpenSQLDataReaderConnection(con);

                    using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "Usp_GetWebAppData", sqlParameterCollection))
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                WebApplicationData webAppDataInst = new WebApplicationData();
                                webAppDataInst.WAD_AppKey = Convert.ToString(dr["WAD_AppKey"]);
                                webAppDataInst.WAD_CreatedOn = Convert.ToDateTime(dr["WAD_CreatedOn"]);
                                webAppDataInst.WAD_IsDeleted = Convert.ToBoolean(dr["WAD_IsDeleted"]);
                                webAppDataInst.WAD_Object_Data = (byte[])(dr["WAD_Object_Data"]);
                                webAppDataInst.WAD_Timespan = Convert.ToInt32(dr["WAD_Timespan"]);
                                WebAppdata.Add(webAppDataInst);
                            }
                        }
                    }

                    /* WebAppdata = _dbNavigation.WebApplicationDatas
                    .Where(x => x.WAD_AppKey.StartsWith(key) && x.WAD_IsDeleted != true 
                    && x.WAD_CreatedOn + new TimeSpan(0, 0, x.WAD_Timespan) >= DateTime.Now
                    ).ToList();*/
                    if (WebAppdata.IsNullOrEmpty()) return null;
                    foreach (var item in WebAppdata)
                    {
                        var obj = ByteArrayToObject(item.WAD_Object_Data);
                        Dictionary<String, String> serializedValueDictionaryData = (Dictionary<String, String>)obj;

                        foreach (var data in serializedValueDictionaryData)
                        {
                            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ApplicantInsituteDataContract));
                            TextReader reader = new StringReader(Convert.ToString(data.Value));
                            var tempObject = (ApplicantInsituteDataContract)serializer.Deserialize(reader);

                            //we want to keep the latest token in case same token is present in multiple instances of webapplicationdata
                            if (!(dictionaryData.Keys.Contains(data.Key) && dictionaryData[data.Key].TokenCreatedTime > tempObject.TokenCreatedTime))
                            {
                                dictionaryData[data.Key] = tempObject;
                            }
                        }
                    }

                    return SerializeForConsistency(dictionaryData);

                }
            }
            var webApplicationData = _dbNavigation.WebApplicationDatas.FirstOrDefault(x => x.WAD_AppKey == key && x.WAD_IsDeleted != true);

            if (webApplicationData.IsNotNull())
            {
                return ByteArrayToObject(webApplicationData.WAD_Object_Data);
            }
            return null;
        }

        private static object SerializeForConsistency(Dictionary<string, ApplicantInsituteDataContract> dictionaryData)
        {
            Dictionary<String, String> serializedValueDictionaryData = new Dictionary<String, String>();

            foreach (var data in dictionaryData)
            {
                var serializer = new XmlSerializer(typeof(ApplicantInsituteDataContract));
                var sb = new StringBuilder();

                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, data.Value);
                }
                serializedValueDictionaryData.Add(data.Key, Convert.ToString(sb));
            }
            return serializedValueDictionaryData;
        }

        /// <summary>
        /// Add object as binary array in Database
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="validtimespan"></param>
        void IApplicationDataRepository.AddWebApplicationData(String key, Object data, Int32 validtimespan)
        {
            if (_dbNavigation.WebApplicationDatas.Any(x => x.WAD_AppKey == key))
            {
                ((IApplicationDataRepository)this).RemoveWebApplicationData(key);
            }

            WebApplicationData wad = new WebApplicationData();
            wad.WAD_AppKey = key;
            wad.WAD_Object_Data = this.ObjectToByteArray(data);
            wad.WAD_Timespan = validtimespan;
            wad.WAD_CreatedOn = DateTime.Now;
            wad.WAD_IsDeleted = false;

            _dbNavigation.WebApplicationDatas.AddObject(wad);
            _dbNavigation.SaveChanges();
        }

        /// <summary>
        /// remove key from table
        /// </summary>
        /// <param name="key"></param>
        void IApplicationDataRepository.RemoveWebApplicationData(String key)
        {
            var webApplicationData = _dbNavigation.WebApplicationDatas.FirstOrDefault(x => x.WAD_AppKey == key && x.WAD_IsDeleted != true);

            if (webApplicationData.IsNotNull())
            {
                _dbNavigation.WebApplicationDatas.DeleteObject(webApplicationData);
                _dbNavigation.SaveChanges();
            }
        }

        void IApplicationDataRepository.UpdateWebApplicationData(String key, Object data)
        {
            if (key == "ApplicantInstData")// temporary changes to allow multiple instances of ApplicantInstData
            {
                
                   key += DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                (this as IApplicationDataRepository).AddWebApplicationData(key, data, 300);
            }
            else
            {
                var webApplicationData = _dbNavigation.WebApplicationDatas.FirstOrDefault(x => x.WAD_AppKey == key && x.WAD_IsDeleted != true);

                if (webApplicationData.IsNotNull())
                {
                    webApplicationData.WAD_Object_Data = this.ObjectToByteArray(data);
                    webApplicationData.WAD_CreatedOn = DateTime.Now;
                    webApplicationData.WAD_IsDeleted = false;
                }
                _dbNavigation.SaveChanges();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Function to get object from byte array
        /// </summary>
        /// <param name="_ByteArray">byte array to get object</param>
        /// <returns>object</returns>
        private Object ByteArrayToObject(byte[] byteArray)
        {
            // convert byte array to memory stream
            using (MemoryStream memStream = new MemoryStream(byteArray))
            {
                // create new BinaryFormatter
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                // set memory stream position to starting point
                memStream.Position = 0;

                // Deserializes a stream into an object graph and return as a object.
                return binaryFormatter.Deserialize(memStream);
            }
        }

        private byte[] ObjectToByteArray(Object obj)
        {
            // convert byte array to memory stream
            using (MemoryStream memStream = new MemoryStream())
            {
                // set memory stream position to starting point
                memStream.Position = 0;

                // create new BinaryFormatter
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                // Serializes an object graph into binary stream and copy to memory stream.
                binaryFormatter.Serialize(memStream, obj);

                return memStream.GetBuffer();
            }
        }

        #endregion

        #endregion

        #region Compiled Queries



        #endregion
    }
}