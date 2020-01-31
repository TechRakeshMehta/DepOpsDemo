using INTSOF.Utils;
using System;
using System.Data.Entity.Core.EntityClient;

namespace EmailDispatcherService
{
    public class CheckDB
    {
        static public bool TestConnString(string connectionString)
        {
            bool returnVal = true;
            
            try
            {
                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                entityBuilder.ProviderConnectionString = connectionString;
                entityBuilder.Provider = AppConsts.SQL_DATA_PROVIDER;
                entityBuilder.Metadata = AppConsts.TENANT_ENTITY_METADATA;
                //entityBuilder.Provider = "System.Data.SqlClient";
                //entityBuilder.Metadata = @"res://*/ClientEntity.ADBClientEntity.csdl|res://*/ClientEntity.ADBClientEntity.ssdl|res://*/ClientEntity.ADBClientEntity.msl";
                using (var db = new Entity.ClientEntity.ADB_LibertyUniversity_ReviewEntities(entityBuilder.ToString()))
                {
                    bool DbExists = db.DatabaseExists();
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
            }
            catch (Exception)
            {
                // no working config
                returnVal = false;
            }

            return returnVal;
        }
    }
}
