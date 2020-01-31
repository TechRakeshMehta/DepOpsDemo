using DAL.Interfaces;
using Entity.ClientEntity;
using System;

namespace DAL.Repository
{
    public class BackgroundCaptureOrderRepository : ClientBaseRepository, IBackgroundCaptureOrderRepository
    {
       private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;

        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
       public BackgroundCaptureOrderRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;
        }
    }
}
