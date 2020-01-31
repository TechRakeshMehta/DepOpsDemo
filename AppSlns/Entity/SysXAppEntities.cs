#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXAppDBEntities.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.Data.Entity.Core.Objects;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Data.Entity.Core.Objects.DataClasses;



#endregion

#region Application Specific

using INTSOF.Utils;
using System.Collections.Generic;


#endregion

#endregion

using INTSOF.ServiceUtil;
using System.Data.Entity;
using System.ServiceModel;

namespace Entity
{
    /// <summary>
    /// This class handles the operations of SysXAppDBEntities.
    /// </summary>
    public partial class SysXAppDBEntities
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private Guid _auditGroup;

        private Int32 _currentVersion;

        private List<EntityObject> _parentList;

        private Boolean _isTrackingNumberFound;

        static String key = "SysXAppDBEntities";

        static SysXAppDBEntities _appContext;

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

        #region Public Methods

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static SysXAppDBEntities GetContext()
        {
            if (HttpContext.Current.IsNull())
            {
                SysXAppDBEntities objContext = new SysXAppDBEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                return objContext;
            }

            else
            {
                if (!HttpContext.Current.Items.Contains(key))
                {
                    SysXAppDBEntities objContext = new SysXAppDBEntities();
                    objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                    HttpContext.Current.Items.Add(key, objContext);
                }

                return HttpContext.Current.Items[key] as SysXAppDBEntities;
            }
        }

        //public static SysXAppDBEntities GetOperationContext()
        //{
        //    if (OperationContext.Current.IsNotNull() && OperationContext.Current.IncomingMessageHeaders.IsNotNull())
        //    {
        //        if (!OperationContext.Current.IncomingMessageProperties.ContainsKey(key))
        //        {
        //            SysXAppDBEntities objContext = new SysXAppDBEntities();
        //            objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
        //            OperationContext.Current.IncomingMessageProperties.Add(key, objContext);
        //        }

        //        return OperationContext.Current.IncomingMessageProperties[key] as SysXAppDBEntities;
        //    }
        //    else
        //    {
        //        SysXAppDBEntities objContext = new SysXAppDBEntities();
        //        objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
        //        return objContext;
        //    }
        //}

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static SysXAppDBEntities GetContext(String connectionString)
        {
            if (HttpContext.Current.IsNull())
            {
                SysXAppDBEntities objContext = new SysXAppDBEntities(connectionString);
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                return objContext;
            }

            else
            {
                if (!HttpContext.Current.Items.Contains(key))
                {
                    SysXAppDBEntities objContext = new SysXAppDBEntities(connectionString);
                    objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                    HttpContext.Current.Items.Add(key, objContext);
                }

                return HttpContext.Current.Items[key] as SysXAppDBEntities;
            }
        }
        /// <summary>
        /// Clears the context.
        /// </summary>
        /// <remarks></remarks>
        public static void ClearContext()
        {
            if (!HttpContext.Current.IsNull())
            {
                if (HttpContext.Current.Items.Contains(key))
                {
                    HttpContext.Current.Items.Remove(key);
                }
            }
        }

        /// <summary>
        ///  Dispose Db context.
        /// </summary>
        /// <remarks></remarks>
        public static void DisposeDbContext()
        {
            if (!HttpContext.Current.IsNull())
            {
                if (HttpContext.Current.Items.Contains(key))
                {
                    var context = (ObjectContext)HttpContext.Current.Items[key];
                    context.Dispose();
                    context = null;
                    HttpContext.Current.Items.Remove(key);
                }
            }
        }
        /// <summary>
        /// Gets the app context.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static SysXAppDBEntities GetAppContext()
        {
            //if (_appContext.IsNull())
            //{
            _appContext = new SysXAppDBEntities();
            _appContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
            return _appContext;
            //}

            //return _appContext;
        }

        #region Audit Code


        /// <summary>
        /// Override base class method to log any entity change
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public override int SaveChanges(System.Data.Entity.Core.Objects.SaveOptions options)
        {
            //AuditLog();
            return base.SaveChanges(options);
        }

        /// <summary>
        /// Return true if entity has any changed value
        /// </summary>
        /// <returns></returns>
        private Boolean IsDirtySave()
        {
            if (this.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted).Count() > 0)
            {
                return false;
            }

            IEnumerable<ObjectStateEntry> changes = this.ObjectStateManager.GetObjectStateEntries(EntityState.Modified);

            foreach (ObjectStateEntry stateEntryEntity in changes)
            {
                if (IsEntityModified(stateEntryEntity))
                    return false;
            }
            return true;
        }


        private Boolean IsEntityModified(ObjectStateEntry stateEntryEntity)
        {
            foreach (String propName in stateEntryEntity.GetModifiedProperties())
            {
                if (!(propName.ToUpper().Equals("MODIFIEDON")))
                {
                    if (!stateEntryEntity.OriginalValues[propName].Equals(stateEntryEntity.CurrentValues[propName]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Create a clone of object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Returns clone of object</returns>
        public EntityObject CloneEntity(EntityObject entityObject)
        {
            DataContractSerializer dcSerializer = new DataContractSerializer(entityObject.GetType());

            EntityObject newObject;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                dcSerializer.WriteObject(memoryStream, entityObject);
                memoryStream.Position = AppConsts.NONE;
                newObject = (EntityObject)dcSerializer.ReadObject(memoryStream);
                memoryStream.Flush();
                memoryStream.Close();
            }

            return newObject;
        }

        #endregion

        #endregion

        #region Private Methods

        #region Audit Code



        /// <summary>
        /// Add parent entity in parentList collection if any parent have trackingNumber property. 
        /// </summary>
        /// <param name="parent"></param>
        void GetParentList(EntityObject parent)
        {
            if (_isTrackingNumberFound)
            {
                return;
            }

            EntityObject newparent = null;
            foreach (IRelatedEnd relationship in this.ObjectStateManager.GetObjectStateEntry(parent).RelationshipManager.GetAllRelatedEnds())
            {
                if (_isTrackingNumberFound)
                {
                    return;
                }

                if (!parent.GetType().GetProperty(relationship.TargetRoleName).IsNull())
                {
                    if (parent.GetType().GetProperty(relationship.TargetRoleName).GetValue(parent, null) is EntityObject)
                    {
                        newparent = (EntityObject)parent.GetType().GetProperty(relationship.TargetRoleName).GetValue(parent, null);
                        if (!newparent.IsNull())
                        {
                            if (!_parentList.Contains(newparent))
                            {
                                if (!newparent.GetType().GetProperty("TrackingNumber").IsNull())
                                {
                                    _parentList.Add(newparent);
                                    _isTrackingNumberFound = true;
                                    break;
                                }

                                GetParentList(newparent);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method checks TrackingNumber and if found any entity that have TrackingNumber it sets AuditNumber and CurrentVersion
        /// </summary>
        /// <param name="entityObject"></param>
        /// <returns>Retunrs True if TrackingNumber found in entity</returns>
        bool IsTrackingNumberExists(EntityObject entityObject)
        {
            PropertyInfo trackingNumber = entityObject.GetType().GetProperty("TrackingNumber");
            if (!trackingNumber.IsNull())
            {
                if (entityObject.EntityState == EntityState.Added)
                {
                    //_auditGroup = Guid.NewGuid();
                    //_currentVersion = 1;
                    SetAuditGroupAndCurrentVersion(null, null);

                    trackingNumber.SetValue(entityObject, _auditGroup, null);

                    PropertyInfo currentVersion = entityObject.GetType().GetProperty("CurrentVersion");
                    currentVersion.SetValue(entityObject, _currentVersion, null);
                }
                else
                {
                    SetAuditGroupAndCurrentVersion(trackingNumber, entityObject);

                    trackingNumber.SetValue(entityObject, _auditGroup, null);

                    PropertyInfo currentVersion = entityObject.GetType().GetProperty("CurrentVersion");
                    if (entityObject.EntityState != EntityState.Deleted)
                    {
                        currentVersion.SetValue(entityObject, _currentVersion, null);
                    }
                }
                return true;
            }
            return false;
        }

        void SetAuditGroupAndCurrentVersion(PropertyInfo trackingNumber, EntityObject entityObject)
        {
            if (!_auditGroup.Equals(new Guid()))
                return;

            if (trackingNumber.IsNull())
            {
                _auditGroup = Guid.NewGuid();
                _currentVersion = 1;
            }
            else
            {
                Guid? audt = (Guid?)trackingNumber.GetValue(entityObject, null);
                if (audt.HasValue)
                {
                    _auditGroup = (Guid)audt;

                    PropertyInfo currentVersion = entityObject.GetType().GetProperty("CurrentVersion");
                    _currentVersion = Convert.ToInt32(currentVersion.GetValue(entityObject, null));
                    _currentVersion++;
                }
                else
                {
                    _auditGroup = Guid.NewGuid();
                    _currentVersion = 1;
                }
            }

        }

        /// <summary>
        /// Method will update TrackNumber and CurrentVersion of parent entity 
        /// </summary>
        /// <param name="firstEntityObject"></param>
        /// <returns> Return False if there will be no entity that have TrackingNumber </returns>
        bool UpdateAuditGroup(EntityObject firstEntityObject)
        {
            GetParentList(firstEntityObject);
            _parentList.Insert(0, firstEntityObject);

            foreach (EntityObject entityObject in _parentList)
            {
                if (IsTrackingNumberExists(entityObject))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns modified column list 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private string GetModifiedColumnList(ObjectStateEntry entry)
        {
            StringBuilder strColumnList = new StringBuilder();
            foreach (String modifiedPropert in entry.GetModifiedProperties())
            {
                if (!entry.CurrentValues[modifiedPropert].Equals(entry.OriginalValues[modifiedPropert]))
                {
                    strColumnList.Append(strColumnList.Length == 0 ? modifiedPropert : ", " + modifiedPropert);
                }
            }
            return strColumnList.ToString();
        }



        private String GetEntryValueInString(ObjectStateEntry entry, bool isOrginal)
        {

            if (entry.Entity is EntityObject)
            {

                object target = CloneEntity((EntityObject)entry.Entity);


                foreach (string propName in entry.GetModifiedProperties())
                {
                    object setterValue = null;
                    if (isOrginal)
                    {
                        //Get original value
                        setterValue = entry.OriginalValues[propName];
                    }
                    else
                    {
                        setterValue = entry.CurrentValues[propName];
                    }
                    //Find property to update
                    PropertyInfo propInfo = target.GetType().GetProperty(propName);

                    //update property with original value
                    if (setterValue == DBNull.Value)
                    {//
                        setterValue = null;
                    }
                    propInfo.SetValue(target, setterValue, null);

                }//end foreach

                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity.GetType().GetProperties().Count(p => p.Name.Equals("CreatedOn")) > 0)
                    {
                        PropertyInfo createdDate = entry.Entity.GetType().GetProperty("CreatedOn");
                        createdDate.SetValue(entry.Entity, DateTime.Now, null);
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity.GetType().GetProperties().Count(p => p.Name.Equals("Deleted")) > 0)
                    {
                        PropertyInfo deleted = entry.Entity.GetType().GetProperty("Deleted");
                        Boolean value = Convert.ToBoolean(deleted.GetValue(target, null));
                        if (value == true)
                        {
                            if (entry.Entity.GetType().GetProperties().Count(p => p.Name.Equals("ExpireDate")) > 0)
                            {
                                PropertyInfo expireDate = entry.Entity.GetType().GetProperty("ExpireDate");
                                expireDate.SetValue(entry.Entity, DateTime.Now, null);
                            }
                        }
                    }
                    if (entry.Entity.GetType().GetProperties().Count(p => p.Name.Equals("ModifiedOn")) > 0)
                    {
                        PropertyInfo modifiedDate = entry.Entity.GetType().GetProperty("ModifiedOn");
                        modifiedDate.SetValue(entry.Entity, DateTime.Now, null);
                    }
                }

                XmlSerializer formatter = new XmlSerializer(target.GetType());
                XDocument document = new XDocument();
                String xmlContent = String.Empty;

                using (XmlWriter xmlWriter = document.CreateWriter())
                {
                    formatter.Serialize(xmlWriter, target);
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }

                return document.Root.ToString();
            }
            return null;
        }


        #endregion

        #endregion

        /// <summary>
        /// Gets the ObjectContext.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static SysXAppDBEntities GetServiceContext()
        {
            Object value;
            if (!ServiceContext.Current.DBContexts.TryGetValue(key, out value))
            {
                SysXAppDBEntities objContext = new SysXAppDBEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ServiceContext.Current.DBContexts.Add(key, objContext);
            }

            return ServiceContext.Current.DBContexts[key] as SysXAppDBEntities;
        }
        /// <summary>
        /// Gets the ObjectContext.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static SysXAppDBEntities GetParallelTaskContext()
        {
            Object value;
            if (!ParallelTaskContext.Current.DBContexts.TryGetValue(key, out value))
            {
                SysXAppDBEntities objContext = new SysXAppDBEntities();
                objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                ParallelTaskContext.Current.DBContexts.Add(key, objContext);
            }

            return ParallelTaskContext.Current.DBContexts[key] as SysXAppDBEntities;
        }
        #endregion
    }

}

