#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  BaseRepository.cs
// Purpose:   Base class for all reposiroty.
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.Metadata.Edm;


#endregion

#region Application Specific

using DAL.Interfaces;
using Entity;
using INTSOF.Utils;
using INTSOF.Logger;
using INTSOF.ServiceUtil;
using Entity.SharedDataEntity;
using System.Data.SqlClient;
using System.Data;
using Entity.LocationEntity;


#endregion

#endregion

namespace DAL.Repository
{
    /// <summary>
    /// Base Class for all repository
    /// </summary>
    public class BaseRepository : IBaseRepository
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SysXAppDBEntities _context;
        private ADBMessageDB_DevEntities _messagingContext;
        private ADB_SharedDataEntities _sharedDataDBContext;
        private String _classModule;
        private ILogger _logger;
        private ADB_LocationDataEntities _locationDBContext;


        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for setting context.
        /// </summary>
        public BaseRepository()
        {
            // _context = HttpContext.Current.IsNull() ? SysXAppDBEntities.GetAppContext() : SysXAppDBEntities.GetContext();

            // ServiceContext.initIfNotInitiallized();
            // var test = OperationContext.Current;


            _context = HttpContext.Current.IsNull() ?
                             (ServiceContext.Current == null ?
                                    (ParallelTaskContext.Current.IsNull() ?
                                        SysXAppDBEntities.GetAppContext()
                                      : SysXAppDBEntities.GetParallelTaskContext())
                              : SysXAppDBEntities.GetServiceContext())
                        : SysXAppDBEntities.GetContext();

            _messagingContext = HttpContext.Current.IsNull() ?
                                 (ServiceContext.Current == null ?
                                        (ParallelTaskContext.Current.IsNull()  ? 
                                            ADBMessageDB_DevEntities.GetAppContext() :
                                            ADBMessageDB_DevEntities.GetParallelTaskContext())
                                  : ADBMessageDB_DevEntities.GetServiceContext())
                            : ADBMessageDB_DevEntities.GetContext();

            _sharedDataDBContext = HttpContext.Current.IsNull() ?
                                    (ServiceContext.Current == null ?
                                           (ParallelTaskContext.Current.IsNull() ?
                                           ADBSharedDataEntity.GetAppContext()
                                              : ADBSharedDataEntity.GetParallelTaskContext())
                                     : ADBSharedDataEntity.GetServiceContext())
                               : ADBSharedDataEntity.GetContext();

            _locationDBContext = HttpContext.Current.IsNull() ?
                                    (ServiceContext.Current == null ?
                                           (ParallelTaskContext.Current.IsNull()
                                              ? ADBLocationDataEntities.GetAppContext()
                                                : ADBLocationDataEntities.GetParallelTaskContext())
                                     : ADBLocationDataEntities.GetServiceContext())
                               : ADBLocationDataEntities.GetContext();

            Init();
        }

        #endregion

        #region Properties

        #region Public Properties

        public String ClassModule
        {
            get
            {
                return _classModule;
            }
            set
            {
                _classModule = value;
            }
        }

        public SysXAppDBEntities Context
        {
            get { return _context; }
        }

        public ADBMessageDB_DevEntities MessagingContext
        {
            get { return _messagingContext; }
        }

        public ADB_SharedDataEntities SharedDataDBContext
        {
            get { return _sharedDataDBContext; }
        }

        public ADB_LocationDataEntities LocationDBContext
        {
            get { return _locationDBContext; }
        }

        #endregion

        #region Protected Properties

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #region IBaseRepository Implementation

        /// <summary>
        /// Generic method to add an entity in DB.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T AddObjectEntity<T>(T entity) where T : class
        {
            try
            {
                var entityTypeAttr = (EdmEntityTypeAttribute)entity.GetType().GetCustomAttributes(typeof(EdmEntityTypeAttribute), false).First();
                String entitySetName =
                    _context.MetadataWorkspace.GetEntityContainer(_context.DefaultContainerName, DataSpace.CSpace).BaseEntitySets
                                                              .Where(bes => bes.ElementType.Name == typeof(T).Name).FirstOrDefault().Name;

                _context.AddObject(entitySetName, entity);

                _context.SaveChanges();
                return entity;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Generic method to update an entity in DB
        /// </summary>
        /// <param name="updatedEntityObject"></param>
        /// <returns></returns>
        public virtual Boolean UpdateObjectEntity(EntityObject updatedEntityObject)
        {
            try
            {
                if (updatedEntityObject != null)
                {
                    _context.SaveChanges();
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Generic method to delete an entity from DB
        /// </summary>
        /// <param name="entityObject"></param>
        /// <returns></returns>
        public virtual Boolean DeleteObjectEntity(EntityObject entityObject)
        {
            try
            {
                return this.DeleteObjectEntity(entityObject, false);
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Generic method to delete an entity from DB
        /// </summary>
        /// <param name="entityObject"></param>
        /// <param name="isHardDelete"></param>
        /// <returns></returns>
        public virtual Boolean DeleteObjectEntity(EntityObject entityObject, Boolean isHardDelete)
        {
            try
            {
                if (entityObject != null)
                {
                    if (isHardDelete)
                    {
                        _context.DeleteObject(entityObject);
                        _context.SaveChanges();
                    }
                    else
                    {
                        UpdateObjectEntity(entityObject);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //#region Notes

        ///// <summary>
        /////  Get hoa notes by asset event Id.
        ///// </summary>
        ///// <param name="assetEventId">Uniquely identify asset event</param>
        ///// <returns>Note1</returns>
        //public virtual IQueryable<Note> GetCurrentContextNotesByEntityId(Int32 entityId, Int32 contextId)
        //{
        //    return CompiledGetCurrentContextNotesByentityId.Invoke(Context, entityId, contextId);
        //}

        ///// <summary>
        ///// Get note on basis of unique identifier.
        ///// </summary>
        ///// <param name="uniqueId"></param>
        ///// <returns></returns>
        //public virtual IQueryable<Note> GetNotesByUID(Guid uniqueId)
        //{
        //    return Context.Notes.Where(condition => condition.UID.Value.Equals(uniqueId));
        //}

        ///// <summary>
        ///// Returns a list of note types.
        ///// </summary>
        ///// <returns></returns>
        //public virtual IQueryable<lkpContext> GetNoteContexts(Int32 contextId)
        //{
        //    return CompiledGetNoteContexts.Invoke(Context, contextId);
        //}

        //#endregion

        #endregion






        #region Protected Methods

        /// <summary>
        /// Generic method to add an entity in DB in a single transaction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected T AddObjectEntityInTransaction<T>(T entity) where T : class
        {
            try
            {
                var entityTypeAttr = (EdmEntityTypeAttribute)entity.GetType().GetCustomAttributes(typeof(EdmEntityTypeAttribute), false).First();
                String entitySetName =
                    _context.MetadataWorkspace.GetEntityContainer(_context.DefaultContainerName, DataSpace.CSpace).BaseEntitySets
                                                              .Where(bes => bes.ElementType.Name == typeof(T).Name).FirstOrDefault().Name;

                _context.AddObject(entitySetName, entity);

                return entity;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Generic method to update an entity in DB. This method is of no use as we do not have any generic logic here. 
        /// So please write transaction update code in DAL itself. - Tarun
        /// </summary>
        /// <param name="updatedEntityObject"></param>
        /// <returns></returns>
        //protected Boolean UpdateObjectEntityInTranscation(EntityObject updatedEntityObject)
        //{
        //    if (updatedEntityObject != null)
        //    {
        //        _context.SaveChanges();
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// Generic method to delete an entity from DB in single transcation.
        /// </summary>
        /// <param name="entityObject"></param>
        /// <returns></returns>
        protected Boolean DeleteObjectEntityInTransaction(EntityObject entityObject)
        {
            try
            {
                return this.DeleteObjectEntityInTransaction(entityObject, false);
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Generic method to delete an entity from DB in single transcation.
        /// </summary>
        /// <param name="entityObject"></param>
        /// <param name="isHardDelete"></param>
        /// <returns></returns>
        protected Boolean DeleteObjectEntityInTransaction(EntityObject entityObject, Boolean isHardDelete)
        {
            try
            {
                if (entityObject != null)
                {
                    if (isHardDelete)
                    {
                        _context.DeleteObject(entityObject);
                    }
                    else
                    {
                        UpdateObjectEntity(entityObject);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            try
            {
                _classModule = GetType().FullName + ".";
            }
            catch (SysXException e)
            {
                //logger.LogError(this.ClassModule + SysXException.ShowTrace(), Environment.NewLine + e.Message);
                throw (new SysXException(this.ClassModule + SysXException.ShowTrace() + Environment.NewLine + "[" + e.Message + "]"));
            }
        }

        #endregion

        #endregion

        #region Compiled Queries

        //#region Notes

        ///// <summary>
        ///// Compiled query to get hoa notes by asset event Id. 
        ///// </summary>
        //Func<SysXAppDBEntities, Int32, Int32, IQueryable<Note>> CompiledGetCurrentContextNotesByentityId = CompiledQuery.Compile<SysXAppDBEntities, Int32, Int32, IQueryable<Note>>
        //                                                                                    (
        //                                                                                    (dbNavigation, entityId, contextId)
        //                                                                                     => (from notes in
        //                                                                                             dbNavigation.Notes
        //                                                                                         .Include(SysXEntityConstants.TABLE_ORGANIZATION_USER)
        //                                                                                          .Include(SysXEntityConstants.TABLE_LKP_CONTEXTS)
        //                                                                                         .Where(note => (contextId == 0 && note.EntityID == entityId)
        //                                                                                             || (note.ContextID == contextId && note.EntityID == entityId))
        //                                                                                         select notes)
        //                                                                                    );

        //Func<SysXAppDBEntities, Int32, IQueryable<lkpContext>> CompiledGetNoteContexts = CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<lkpContext>>
        //                                                                                   (
        //                                                                                   (dbNavigation, contextId)
        //                                                                                    => (from context in
        //                                                                                            dbNavigation.lkpContexts
        //                                                                                            .Where(contxt => contxt.ContextID == contextId || contxt.ParentContextID == contextId)
        //                                                                                        select context)
        //                                                                                   );

        //#endregion

        #endregion

        #region MyQueue's Virtual Methods

        /// <summary>
        /// Method to get detailed view of approval queue
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="clientId"></param>
        /// <param name="queueId"></param>
        /// <param name="queueStatusCode"></param>
        /// <param name="isClosed"></param>
        /// <returns></returns>
        public virtual IEnumerable<ComplexObject> GetPendingForApproval(Int32 currentUserId, Int32 clientId, Int32 queueId, String queueStatusCode, Boolean isClosed)
        {
            throw new SysXException("Method not implemented");
        }

        /// <summary>
        /// Method to get Detailed view of user queue
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="clientId"></param>
        /// <param name="queueId"></param>
        /// <param name="queueStatusCode"></param>
        /// <param name="isClosed"></param>
        /// <returns></returns>
        public virtual IEnumerable<ComplexObject> GetTaskQueue(Int32 currentUserId, Int32 clientId, Int32 queueId, String queueStatusCode, Boolean isClosed)
        {
            throw new SysXException("Method not implemented");
        }

        public virtual Boolean ApproveTransaction(Int32 transactionId, Int32 currentUserId)
        {
            throw new SysXException("Method not implemented");
        }

        public virtual Boolean RejectTransaction(Int32 transactionId, Int32 currentUserId)
        {
            throw new SysXException("Method not implemented");
        }

        #endregion


        protected SqlDataReader ExecuteSQLDataReader(SqlConnection con, String storedProcedureName, SqlParameter[] sqlParameterCollection = null)
        {
            SqlCommand sqlCommand = new SqlCommand(storedProcedureName, con);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            if (sqlParameterCollection != null)
            {
                sqlCommand.Parameters.AddRange(sqlParameterCollection);
            }

            SqlDataReader dr = sqlCommand.ExecuteReader();
            return dr;
        }

        protected void OpenSQLDataReaderConnection(SqlConnection con)
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
        }

        protected void CloseSQLDataReaderConnection(SqlConnection con)
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        protected DataSet ExecuteSQLDataSet(SqlConnection con, String storedProcedureName, SqlParameter[] sqlParameterCollection = null)
        {
            SqlCommand sqlCommand = new SqlCommand(storedProcedureName, con);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            if (sqlParameterCollection != null)
            {
                sqlCommand.Parameters.AddRange(sqlParameterCollection);
            }

            DataSet ds = new DataSet();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
            dataAdapter.Fill(ds);

            return ds;
        }

        //T IBaseRepository.AddObjectEntity<T>(T entity)
        //{
        //    throw new NotImplementedException();
        //}

        //bool IBaseRepository.UpdateObjectEntity(EntityObject updatedEntityObject)
        //{
        //    throw new NotImplementedException();
        //}

        //bool IBaseRepository.DeleteObjectEntity(EntityObject entityObject)
        //{
        //    throw new NotImplementedException();
        //}

        //bool IBaseRepository.DeleteObjectEntity(EntityObject entityObject, bool isHardDelete)
        //{
        //    throw new NotImplementedException();
        //}


    }
}
