using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientServiceLibrary.DAL;


namespace ClientServiceLibrary.BAL
{
    public class Manager
    {
        public static TokenValidateDataContract GetTokenValidateData(int tenantId, string entityTypeCode, string mappingCode)
        {
            return Repository.GetTokenValidateData(tenantId, entityTypeCode, mappingCode);
        }
    }
}
