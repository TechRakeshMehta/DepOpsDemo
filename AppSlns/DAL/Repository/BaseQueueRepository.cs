#region Header Comment Block
// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  BaseQueueRepository.cs
// Purpose:   Base class for all Queues.
//
// Revisions:
// Comment
// -------------------------------------------------
// Initial Release
// 
#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.Metadata.Edm;


#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using INTSOF.ServiceUtil;

#endregion

#endregion

namespace DAL.Repository
{
    public class BaseQueueRepository : BaseRepository
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private String _classModule;
        /*private SupplierQueue _suppQueue;
       */
        private SysXAppDBEntities _appContext;
        private ADBMessageDB_DevEntities _adbMessageQueueContext;

        private ADBApplicantMessageDB_DevEntities _applicantQueue;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for setting context.
        /// </summary>
        public BaseQueueRepository()
        {
            //_appContext = HttpContext.Current.IsNull() ? SysXAppDBEntities.GetAppContext() : SysXAppDBEntities.GetContext();
          //  _appContext = HttpContext.Current.IsNull() ? (ServiceContext.Current == null ? SysXAppDBEntities.GetAppContext() : SysXAppDBEntities.GetServiceContext()) : SysXAppDBEntities.GetContext();
            _appContext = HttpContext.Current.IsNull() ?
                                     (ServiceContext.Current == null ?
                                            (ParallelTaskContext.Current.IsNull() ? SysXAppDBEntities.GetAppContext()
                                              : SysXAppDBEntities.GetParallelTaskContext())
                                      : SysXAppDBEntities.GetServiceContext())
                                : SysXAppDBEntities.GetContext();
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


        #endregion

        #region Protected Properties

        /*protected SupplierQueue SupplierQueueContext
        {
            get
            {
                if (_suppQueue.IsNull())
                {
                    _suppQueue = HttpContext.Current.IsNull() ? SysXQueue.GetAppSupplierQueueContext() : SysXQueue.GetSupplierQueueContext();
                }
                return _suppQueue;
            }
        }
        */
        protected SysXAppDBEntities AppDBContext
        {
            get
            {
                if (_appContext.IsNull())
                {
                    //_appContext = HttpContext.Current.IsNull() ? SysXQueue.GetAppContext() : SysXQueue.GetContext();
                   // _appContext = HttpContext.Current.IsNull() ? (ServiceContext.Current == null ? SysXQueue.GetAppContext() : SysXQueue.GetServiceContext()) : SysXQueue.GetContext();
                    _appContext = HttpContext.Current.IsNull() ?
                                    (ServiceContext.Current == null ?
                                           (ParallelTaskContext.Current.IsNull() ? SysXQueue.GetAppContext()
                                             : SysXQueue.GetParallelTaskContext())
                                     : SysXQueue.GetServiceContext())
                               : SysXQueue.GetContext();
                }
                return _appContext;
            }
        }

        protected ADBMessageDB_DevEntities ADB_MessageQueueContext
        {
            get
            {
                if (_adbMessageQueueContext.IsNull())
                {
                    //_adbMessageQueueContext = HttpContext.Current.IsNull() ? SysXQueue.GetAppADB_MessageQueueContext() : SysXQueue.GetADB_MessageQueueContext();
                  //  _adbMessageQueueContext = HttpContext.Current.IsNull() ? (ServiceContext.Current == null ? SysXQueue.GetAppADB_MessageQueueContext() : SysXQueue.GetMessageServiceContext()) : SysXQueue.GetADB_MessageQueueContext();
                    _adbMessageQueueContext = HttpContext.Current.IsNull() ?
                                      (ServiceContext.Current == null ?
                                             (ParallelTaskContext.Current.IsNull() ? SysXQueue.GetAppADB_MessageQueueContext()
                                               : SysXQueue.GetMessageParallelTaskContext())
                                       : SysXQueue.GetMessageServiceContext())
                                 : SysXQueue.GetADB_MessageQueueContext();
                }
                return _adbMessageQueueContext;
            }
        }



        protected ADBApplicantMessageDB_DevEntities ApplicantQueueContext
          {
              get
              {
                  if (_applicantQueue.IsNull())
                  {
                      _applicantQueue = HttpContext.Current.IsNull() ? SysXQueue.GetAppApplicantQueueContext() : SysXQueue.GetApplicantQueueContext();
                  }
                  return _applicantQueue;
              }
          }
          

        #endregion

        #endregion

        #region Methods

        #region Public Methods


        #endregion

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
                    _appContext.MetadataWorkspace.GetEntityContainer(_appContext.DefaultContainerName, DataSpace.CSpace).BaseEntitySets
                                                              .Where(bes => bes.ElementType.Name == typeof(T).Name).FirstOrDefault().Name;

                _appContext.AddObject(entitySetName, entity);

                _appContext.SaveChanges();
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
                    _appContext.SaveChanges();
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
        /*
                /// <summary>
                /// Generic method to update an entity in DB
                /// </summary>
                /// <param name="updatedEntityObject"></param>
                /// <returns></returns>
                public virtual Boolean UpdateObjectEntity(EntityObject updatedEntityObject, WFConfigDBEntities _wfConfigContext)
                {
                    try
                    {
                        if (updatedEntityObject != null)
                        {
                            _wfConfigContext.SaveChanges();
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
                                _appContext.DeleteObject(entityObject);
                                _appContext.SaveChanges();
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
        */
        #endregion



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
                    _appContext.MetadataWorkspace.GetEntityContainer(_appContext.DefaultContainerName, DataSpace.CSpace).BaseEntitySets
                                                              .Where(bes => bes.ElementType.Name == typeof(T).Name).FirstOrDefault().Name;

                _appContext.AddObject(entitySetName, entity);

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
        //        _appContext.SaveChanges();
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
                        _appContext.DeleteObject(entityObject);
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
            catch (Exception e)
            {
                //logger.LogError(this.ClassModule + SysXException.ShowTrace(), Environment.NewLine + e.Message);
                throw (new SysXException(this.ClassModule + SysXException.ShowTrace() + Environment.NewLine + "[" + e.Message + "]"));
            }
        }

        #endregion

        #endregion

        #region Compiled Queries



        #endregion
    }
}
