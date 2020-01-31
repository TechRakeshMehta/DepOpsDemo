using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules
{
    public enum ResponseStatus
    {
        Success = 1,
        Exception = 2,
        BusinessException = 3,
        SqlTimeoutException = 4
    }
}
