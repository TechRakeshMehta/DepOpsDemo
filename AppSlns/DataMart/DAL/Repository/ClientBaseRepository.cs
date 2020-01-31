using Entity.ClientEntity;
using System;
using System.Data.Entity.Core.EntityClient;
using Entity.SharedDataEntity;
using DataMart.DAL.Interfaces;

namespace DataMart.DAL.Repository
{
    public class ClientBaseRepository : BaseRepository
    {

        private Entity.SysXAppDBEntities _securityContext;
        private Int32 TenantID;
        public Entity.SysXAppDBEntities SecurityContext
        {
            get
            {
                return _securityContext;
            }
        }
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;
        public ADB_LibertyUniversity_ReviewEntities ClientDBContext
        {
            get
            {
                return _ClientDBContext;
            }
        }

        private Entity.ADBMessageDB_DevEntities _messagingContext;
        public Entity.ADBMessageDB_DevEntities MessagingContext
        {
            get { return _messagingContext; }
        }

        private ADB_SharedDataEntities _sharedDataDBContext;
        public ADB_SharedDataEntities SharedDataDBContext
        {
            get
            {
                return _sharedDataDBContext;
            }
        }

        public ClientBaseRepository(Int32 TenantId, String connectinString = "")
        {
            this.TenantID = TenantId;
            _securityContext = base.Context;
            _messagingContext = base.MessagingContext;
            //Entity.Tenant rootTenant = GetRootTenant(TenantId);
            _ClientDBContext = ADB_LibertyUniversity_ReviewEntities.GetContext(GetConnectionString(TenantId, connectinString));
            _sharedDataDBContext = base.SharedDataDBContext;
        }
        public virtual void ResetClientContext()
        {
            ADB_LibertyUniversity_ReviewEntities.ClearContext();
            _ClientDBContext = ADB_LibertyUniversity_ReviewEntities.GetContext(GetConnectionString(this.TenantID));
        }

        private string GetConnectionString(int TenantId, String connectinString = "")
        {
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
            ISecurityRepository objSecurity = new SecurityRepository();
            String tenantConnectionString = connectinString;
            if (String.IsNullOrEmpty(tenantConnectionString))
            {
                tenantConnectionString = objSecurity.GetClientConnectionString(TenantId);
            }
            entityBuilder.ProviderConnectionString = tenantConnectionString;
            tenantConnectionString += ";Max Pool Size=1000;Pooling=true";
            entityBuilder.Provider = "System.Data.SqlClient";
            entityBuilder.Metadata = @"res://*/ClientEntity.ADBClientEntity.csdl|res://*/ClientEntity.ADBClientEntity.ssdl|res://*/ClientEntity.ADBClientEntity.msl";
            return entityBuilder.ToString();
        }
    }
}
