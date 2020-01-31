#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  LookupRepository.cs
// Purpose:   Return lookup data
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#region Application Defined

using Entity;
using DAL.Interfaces;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;
using System.Data.Common;

#endregion

#endregion

namespace DAL.Repository
{
    /// <summary>
    /// LookupRepository : To get lookup data 
    /// </summary>
    public class LookupRepository : BaseRepository, ILookupRepository
    {

        #region Private Variables

        private SysXAppDBEntities _dbNavigation;

        #endregion

        
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public LookupRepository()
        {
            _dbNavigation = base.Context;
        }

        #endregion


        #region Methods

        #region Public Methods

        #region GetLookup Data 

        /// <summary>
        /// GetLookupData
        /// </summary>
        /// <param name="lookup"></param>
        /// <returns></returns>
        public List<TEntity> GetLookupData<TEntity>()
        {            

            List<TEntity> lstLookUpData = new List<TEntity>();
            
            String entitySetName =
                        _dbNavigation.MetadataWorkspace.GetEntityContainer(_dbNavigation.DefaultContainerName, DataSpace.CSpace).BaseEntitySets
                                                                  .Where(bes => bes.ElementType.Name == typeof(TEntity).Name).FirstOrDefault().Name;

            ObjectQuery<DbDataRecord> query = GetEnitityData(entitySetName);

            foreach (var rec in query)
            {
                lstLookUpData.Add((TEntity)rec.GetValue(0));
            }

            return lstLookUpData;  
        }


        #endregion 

        #endregion
        
        #region Private Methods


        /// <summary>
        /// GetEnitityData
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entitySetName"></param>
        /// <returns></returns>
        private ObjectQuery<DbDataRecord> GetEnitityData(string entitySetName)
        {

            EntityContainer container = _dbNavigation.MetadataWorkspace.GetItems<EntityContainer>
                                                                (DataSpace.CSpace).First();

            EntitySetBase entitySetBase = container.BaseEntitySets
                .FirstOrDefault(set => set.Name == entitySetName);

            StringBuilder stringBuilder = new StringBuilder().Append("SELECT entity ");
            stringBuilder.Append(" FROM " + container.Name.Trim() + "." + entitySetBase.Name + " AS entity");
            ObjectQuery<DbDataRecord> query = new ObjectQuery<DbDataRecord>(stringBuilder.ToString(), _dbNavigation);

            return query;
        }

        
        

        #endregion


        #endregion 



    }
}
