using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.EntityClient;
using INTSOF.Utils;
using NLog;
using INTSOF.ServiceUtil;
using ExternalVendors;

namespace VendorOrderProcessService
{
    public class CheckDBConnection
    {
        static public Boolean TestConnString(String connectionString, String sloggerInstance)
        {
            Boolean returnVal = true;

            try
            {
                ServiceLogger.Info("Testing connection for connection string" + connectionString + " on  : " + DateTime.Now.ToString(), sloggerInstance);

                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                entityBuilder.ProviderConnectionString = connectionString;
                entityBuilder.Provider = AppConsts.SQL_DATA_PROVIDER;
                entityBuilder.Metadata = AppConsts.TENANT_ENTITY_METADATA;

                ServiceLogger.Debug<String>("Connection string passed to entity connection: ", entityBuilder.ToString(), sloggerInstance);
                using (var db = new Entity.ClientEntity.ADB_LibertyUniversity_ReviewEntities(entityBuilder.ToString()))
                {
                    Boolean DbExists = db.DatabaseExists();
                    ServiceLogger.Debug<Boolean>("Whether connection exists for connection string" + connectionString + ": ", DbExists, sloggerInstance);
                    if (DbExists)
                    {
                        // database is existing
                        returnVal = true;
                    }
                    else
                    {
                        // config is working, but database does not exist
                        returnVal = false;
                    }
                }

                ServiceLogger.Info("Tested connection for connection string: " + connectionString + " on  : " + DateTime.Now.ToString(), sloggerInstance);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in TestConnString method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", 
                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                    sloggerInstance);
                // no working config
                returnVal = false;
            }

            return returnVal;
        }
    }
}
