#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXPersistViewStateProvider.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    public class SysXPersistViewStateProvider : IPersistViewState
    {
        #region Variables

        #region Public Variables
        #endregion

        #region Private Variables
        private String _connectionString = ConfigurationManager.ConnectionStrings[SysXCachingConst.CONNECTION_STRING_VIEWSTATESTORE].ConnectionString;
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region public Methods
        /// <summary>
        /// Loads the View state for a page.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public String Load(String sessionId, String pageUrl)
        {
            try
            {
                string viewData = string.Empty;

                if (!(sessionId.IsNullOrEmpty() || pageUrl.IsNullOrEmpty()))
                {
                    String key = sessionId + SysXCachingConst.DELIMETER_SESSIONPAGEURL + pageUrl;
                    using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(SysXCachingConst.SP_GET_VIEWSTATE, sqlConnection))
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.Add(SysXCachingConst.SP_PARAMETER_ID, SqlDbType.VarChar).Value = key;
                            sqlConnection.Open();
                            viewData = sqlCommand.ExecuteScalar() as String;
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException(SysXCachingConst.NULLID_NOTIFICATION);
                }
                return viewData;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the view sate for a page.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="content">The content.</param>
        /// <remarks></remarks>
        public void Save(String sessionId, String pageUrl, String content, Boolean isOverWritable)
        {
            try
            {
                if (!(sessionId.IsNullOrEmpty() || pageUrl.IsNullOrEmpty() || content.IsNullOrEmpty()))
                {
                    String key = sessionId + SysXCachingConst.DELIMETER_SESSIONPAGEURL + pageUrl;
                    if (!isOverWritable)
                        sessionId = string.Empty;

                    using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(SysXCachingConst.SP_STORE_VIEWSTATE, sqlConnection))
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.Add(SysXCachingConst.SP_PARAMETER_ID, SqlDbType.VarChar).Value = key;
                            sqlCommand.Parameters.Add(SysXCachingConst.SP_PARAMETER_VIEWSTATEDATA, SqlDbType.Text).Value = content;
                            sqlCommand.Parameters.Add(SysXCachingConst.SP_PARAMETER_OVERWRITEID, SqlDbType.VarChar).Value = sessionId;
                            sqlCommand.Parameters.Add(SysXCachingConst.SP_PARAMETER_ISOVERWRITABLE, SqlDbType.Bit).Value = isOverWritable;
                            sqlConnection.Open();
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException(SysXCachingConst.NULLID_NOTIFICATION);
                }
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the view sate for a page.
        /// </summary>
        /// <param name="sessionId">The session id.</param>        
        /// <remarks></remarks>
        public void Delete(String sessionId)
        {
            try
            {
                if (!sessionId.IsNullOrEmpty())
                {

                    using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(SysXCachingConst.SP_DELETE_VIEWSTATE, sqlConnection))
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.Add(SysXCachingConst.SP_PARAMETER_ID, SqlDbType.VarChar).Value = sessionId;
                            sqlConnection.Open();
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException(SysXCachingConst.NULLID_NOTIFICATION);
                }
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #endregion
    }
}