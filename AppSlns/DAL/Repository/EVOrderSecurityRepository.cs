#region Header Comment Block

// 
// Copyright 2014 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  EVOrderSecurityRepository.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Collections.Generic;

#endregion

#region Application Specific

using DAL.Interfaces;
using Entity;
using INTSOF.Utils;

#endregion

#endregion

namespace DAL.Repository
{
    public class EVOrderSecurityRepository : BaseRepository, IEVOrderSecurityRepository
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SysXAppDBEntities _dbNavigation;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public EVOrderSecurityRepository()
        {
            _dbNavigation = base.Context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get External Service Code for BkgService
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        Dictionary<Int32, String> IEVOrderSecurityRepository.GetExternalServiceCodeForBkgService(List<Int32> backgroundServiceIDs)
        {
            IEnumerable<KeyValuePair<Int32, String>> externalBkgServiceResult = _dbNavigation.BkgSvcExtSvcMappings.Include("ExternalBkgSvc").
                                                                                Where(cond => backgroundServiceIDs.Contains(cond.BSESM_BkgSvcId)).ToList()
                                                                                .Select(col => new KeyValuePair<Int32, String>
                                                                                 (
                                                                                 col.BSESM_BkgSvcId,
                                                                                 col.ExternalBkgSvc.EBS_ExternalCode
                                                                                 ));
            Dictionary<Int32, String> externalBkgServiceCodes = new Dictionary<Int32, String>();
            externalBkgServiceCodes.AddRange(externalBkgServiceResult);
            return externalBkgServiceCodes;
        }

        Boolean IEVOrderSecurityRepository.SaveExtSvcIntegrationRecord(List<ExtSvcIntegrationRecord> ExtSvcIntegrationRecordToSave)
        {
            foreach (ExtSvcIntegrationRecord extSvcIntegrationRecordItem in ExtSvcIntegrationRecordToSave)
            {
                _dbNavigation.ExtSvcIntegrationRecords.AddObject(extSvcIntegrationRecordItem);
            }
            _dbNavigation.SaveChanges();
            return true;
        }

        IQueryable<vw_BkgExtSvcAttributeMapping> IEVOrderSecurityRepository.GetBkgExtSvcAttributeMappings()
        {
            return _dbNavigation.vw_BkgExtSvcAttributeMapping;
        }

        IQueryable<ExternalBkgSvcAttribute> IEVOrderSecurityRepository.GetExtSvcAttributes()
        {
            return _dbNavigation.ExternalBkgSvcAttributes;
        }

        #endregion
    }
}
